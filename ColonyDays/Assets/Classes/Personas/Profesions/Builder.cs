using System;
using UnityEngine;

public class Builder : Profession
{
    private bool _nothingToBuild;
    private int _nothingToBuildCounter;

    private bool _wheelBarrowNow;

    public Builder(Person person, PersonFile pF)
    {
        if (pF == null)
        {
            CreatingNew(person);
        }
        else LoadingFromFile(person, pF);
    }

    void CreatingNew(Person person)
    {
        //in case was a Wheelbarrow the prevProfession and when home route back gives problem 
        person.PrevOrder = null;

        B4Init(person);
    }

    void LoadingFromFile(Person person, PersonFile pF)
    {
        _person = person;
        LoadAttributes(pF.ProfessionProp);
        B4Init(person);
    }

    void B4Init(Person person)
    {

        
        MyAnimation = "isHammer";
        _person = person;
        HandleNewProfDescrpSavedAndPrevJob(Job.Builder);

        DefineConstructingRoutine();
    }

    /// <summary>
    /// Use to address if consturcitng is null or is fully built already 
    /// </summary>
    void DefineConstructingRoutine()
    {
        if (_constructing == null || ReturnCurrentStage() == 4)
        {
            _constructing = FindBestToBuild();

            if (_constructing != null)
            {
                LookAtWork = _constructing.transform.position;
                RemoveFromBuilderManagerIfBridge();
            }
            else
            {
                LookAtWork = new Vector3();
            }
        }

        if (_constructing == null)
        {
            _nothingToBuild = true;
            _nothingToBuildCounter++;
            CheckHowManyNothingToBuild();
        }
        else
        {
            _nothingToBuildCounter = 0;
            Init();
        }
    }

    /// <summary>
    /// Need to remove brdige so no one else keeps working on it. since gives bugg bz is async the call when 
    /// removing it from BuilderManager
    /// </summary>
    void RemoveFromBuilderManagerIfBridge()
    {
        if (_constructing.HType.ToString().Contains("Bridge"))
        {
            _person.Work.BuildersManager1.RemoveConstruction(_constructing.MyId);       
            PersonPot.Control.BuildersManager1.RemoveConstruction(_constructing.MyId);       
        }
    }

    /// <summary>
    /// Cretaed to address when he has over 40 times nothing to build then will become a 
    /// WheelBarrow worker , until is something to build again 
    /// </summary>
    void CheckHowManyNothingToBuild()
    {
        if (_person == null || _person.Brain == null)
        {
            return;
        }

        if (_nothingToBuildCounter > 40 && _person.Brain.IAmHomeNow())
        {
            _nothingToBuildCounter = 0;
            //Convert to wheel Barrow worker
            _wheelBarrowNow = true;
            
            //Debug.Log("Now wheelbarrow ");
            _person.CreateProfession(Job.WheelBarrow);
        }
    }

    private void Init()
    {
        //AddMeToWaitListOnSystem();
        //if (!PersonPot.Control.OnSystemNow(_person.MyId))
        //{
        //    _takeABreakNow = true;
        //    return;
        //}


        //was destroy 
        if (_constructing == null)
        {
            B4Init(_person);
            return;
        }

        if (_person.Work == null)
        {
            throw new Exception("Work cant be null");
            return;
        }

        FinRoutePoint = FindFinRoutePoint();

        //means the anchors are not ready yet in the Bridge case 
        if (FinRoutePoint == new Vector3())
        {
            _takeABreakNow = true;
            return;
        }

        //UVisHelp.CreateHelpers(FinRoutePoint, Root.yellowCube);

        //bz I want '_finRoutePoint' to be moved away from building and affects too that it will look towards the 
        //building when is performng the action of buildign 
        MoveTowOrigin = -0.01f;

        //moving the route point a bit away from the origin 
        FinRoutePoint = Vector3.MoveTowards(FinRoutePoint, _constructing.transform.position, MoveTowOrigin);

        MoveTowOrigin = -0.05f;//when looks at on Profession works properly

        InitRoute();
    }

    //so when using dummy on CryBrdigeRoute can find its spawner
    //private string dummySpawnerId;
    /// <summary>
    /// Will return the init point which is the place will go to build and execute the animation on a building 
    /// </summary>
    /// <returns></returns>
    Vector3 FindFinRoutePoint()
    {
        //bridge
        if (_constructing.HType.ToString().Contains(H.Bridge.ToString()))
        {
            if (_constructing.Anchors.Count > 0)
            {
            //    dummySpawnerId = _person.Work.MyId;
                return Brain.ReturnClosestVector3(_person.Work.transform.position, _constructing.Anchors);
            }
        }
        else if (IsAShoreBuilding(_constructing))
        {
            if (_constructing.Anchors.Count > 0)
            {
                //return the closest anchor to SpawnPoint 
                var sp = (StructureParent) _constructing;
               // dummySpawnerId = _constructing.MyId;

                return Brain.ReturnClosestVector3(sp.SpawnPoint.transform.position, _constructing.Anchors);
            }
        }

        //so will take a break
        if (_constructing.Anchors.Count == 0)
        { return new Vector3();}

        //for all other cases 
        //forcing geting the anchors was giving anchors really far appart sometimes 
        //dummySpawnerId = _constructing.MyId;
        return _constructing.GetAnchors(true)[UMath.GiveRandom(0, 4)];
    }

    public static bool IsAShoreBuilding(Building constP)
    {
        if ((constP.HType.ToString().Contains("Dock") || constP.HType.ToString().Contains("DryDock")
            || constP.HType.ToString().Contains("Fish") || constP.HType.ToString().Contains("SaltMine")
            || constP.HType.ToString().Contains("Supplier")))
        {
            return true;
        }
        return false;
    }

    void InitRoute()
    {
        //if (_person == null)
        //{
        //    Debug.Log("_person null");

        //    _takeABreakNow = true;
        //    return;
        //}
        //if (_person.MyDummyProf == null)
        //{
        //    Debug.Log("dummyProf null" + _person.MyId);

        //    _takeABreakNow = true;
        //    return;

        //}
        //if (_constructing == null)
        //{
        //    Debug.Log("_const was null" + _person.MyId);
        //    _takeABreakNow = true;
        //    return;

        //}



        Router1 = null;
        RouterBack = null;

        RouterActive = true;
        IsRouterBackUsed = true;
        routerBackWasInit = false;
        //Debug.Log("routerBackWasInit = false");



        //that ID will remove dummy so can be cache and will add the FinRoutePoint so if another builder
        //will go to that corner can use the cached one 
        _person.MyDummyProf.MyId = _constructing.MyId + "." + FinRoutePoint;
        _person.MyDummyProf.transform.position = FinRoutePoint;

        //UVisHelp.CreateHelpers(FinRoutePoint, Root.yellowCube);

        _person.MyDummyProf.transform.LookAt(_constructing.transform.position);
        _person.MyDummyProf.HandleLandZoning(_constructing, FinRoutePoint);

        _person.MyDummyProf.DummyIdSpawner = _constructing.MyId;
        Router1 = new CryRouteManager(_person.Work, _person.MyDummyProf, _person, finDoor: false);
    }



    Building FindBestToBuild()
    {
        //first time wheel barrow chekcs 
        if (_person == null || _person.Work == null || _person.Work.BuildersManager1==null)
        {
            return null;
        }

        //ConstructingKey = PersonPot.Control.BuildersManager1.GiveMeBestConstruction();
        ConstructingKey = _person.Work.BuildersManager1.GiveMeBestConstruction(_person);

        //todo should ask for 2nd better building 
        if (_person.Brain.BlackList.Contains(ConstructingKey))
        {
            ConstructingKey = "";
            return null;
        }

        return Brain.GetBuildingFromKey(ConstructingKey);
    }



    public override void Update()
    {
        CheckIfRoute1IsReady();
        CheckIfWhatConstructingBlackListed();

        if (_takeABreakNow)
        {
            TakeABreak();
            return;
        }

        base.Update();


        AnyChange();
        Execute();

        CheckIfNothingToDo();

        WheelBarrowCheck();

        CheckIfConstructingWasDestroy();
    }

    private void CheckIfWhatConstructingBlackListed()
    {
        if (_takeABreakNow || _person == null || _constructing == null)
        {
            return;
        }

        if (_person.Brain.BlackList.Contains(_constructing.MyId))
        {
            ConstructingKey = "";
            _constructing = null;
            _takeABreakNow = true;
        }
    }


    private bool routerBackWasInit;
    /// <summary>
    /// So it doesnt blackList nothing in the second Route if he is blackListug a tree in the Router1
    /// </summary>
    private void CheckIfRoute1IsReady()
    {
        if (RouterActive && Router1.IsRouteReady && !routerBackWasInit && _person.FoodSource != null)
        {
            routerBackWasInit = true;
            //If the FoodSrc is not null will be used as way back
            RouterBack = new CryRouteManager(_person.MyDummyProf, _person.FoodSource, _person, HPers.InWorkBack, false, true);
        }
    }


    private void CheckIfConstructingWasDestroy()
    {
        if (_person == null)
        {
             return;
        }

        //when is just on site to play animation of building 
        if (_person.Body.Location == HPers.InWork && _workerTask == HPers.WalkingToJobSite && !_person.Body.MovingNow
            && _constructing == null)
        {
            //so skip all that and goes back to office 
            _workerTask = HPers.WalkingBackToOffice;
        }
    }

    void Execute()
    {
        if (ExecuteNow)
        {
            ExecuteNow = false;

            //if was destroy
            if (_constructing == null)
            {
                return;
            }

            //do stuff
            _constructing.AddToConstruction(200f * ToolsFactor(), _person);
        }
    }

    public override void AnyChange()
    {
        if (_person == null || _person.Brain == null || !_person.Brain.IAmHomeNow() )
        {
            return;
        }

        base.AnyChange();
        //means we finisished 

        if ( _constructing == null || ReturnCurrentStage() == 4)
        {
            SearchANewWorkPlace();
        }
    }

    /// <summary>
    /// Will return the current stage of the building depending on wht type of building is
    /// </summary>
    /// <returns></returns>
    int ReturnCurrentStage()
    {
        int stage = 0;
        if (_constructing.HType.ToString().Contains(H.Bridge.ToString()))
        {
            Bridge br = (Bridge)_constructing;
            stage = br.Pieces[0].CurrentStage;
        }
        else
        {
            var st = (Structure)_constructing;
            stage = st.CurrentStage;
        }
        return stage;
    }

    void SearchANewWorkPlace()
    {
        _readyToWork = false;//here so Professional can change for things ard him 

        //call init so we move on and start workiing on something new 
        DefineConstructingRoutine();
    }

    void CheckIfNothingToDo()
    {
        if (_nothingToBuild && !_wheelBarrowNow)
        {
            _nothingToBuild = false;
            DefineConstructingRoutine();
        }
    }


    #region WheelBarrow Work
    /// <summary>
    /// Will check so often to see if new Constructing is needed 
    /// </summary>
    void WheelBarrowCheck()
    {
        if (!_wheelBarrowNow)
        {return;}

        var constTry  = FindBestToBuild();

        if (constTry != null)
        {
            _wheelBarrowNow = false;
        }
    }

    private bool _takeABreakNow;
    private float _breakDuration = 1f;
    private float startIdleTime;
    /// <summary>
    /// Used so a person is asking for bridges anchors takes a break and let brdige anchors complete then can 
    /// work on it
    /// </summary>
    void TakeABreak()
    {
        if (startIdleTime == 0)
        { startIdleTime = Time.time; }

        if (Time.time > startIdleTime + _breakDuration)
        {
            _takeABreakNow = false;
            startIdleTime = 0;
            
            //so it restarted 
            Init();
        }
    }
    #endregion
}
