using System;
using UnityEngine;

/*
 * Created with the Simple purpose of From current point go get food and go home, drop food
 * And thats it
 * 
 */
public class Homer : Profession
{
    private Structure BuildToGoBackTo;//the food src of the homer

    public Homer(Person person, PersonFile pF)
    {
        if (pF == null)
        {
            CreatingNew(person);
        }
        else LoadingFromFile(person, pF);
    }

    void CreatingNew(Person person)
    {
        _person = person;
        Init();
    }

    void LoadingFromFile(Person person, PersonFile pF)
    {
        _person = person;

        LoadAttributes(pF.ProfessionProp);
        
        //so drops stuff in there 
        BuildToGoBackTo = BuildToGoBackToDefine();

        if (_person.PrevJob == Job.Farmer)
        {
            FinRoutePoint = DefineFinRoute();
            InitRoute();        
        }
    }

    private void Init()
    {
        //Debug.Log(_person.MyId + " new Homer");
        HandleNewProfDescrpSavedAndPrevJob(Job.Homer);
        BuildToGoBackTo = BuildToGoBackToDefine();
        FinRoutePoint = DefineFinRoute();

        InitRoute();
    }

    Structure BuildToGoBackToDefine()
    {
        //if is a wheelbarrow working from its DestinyBuild will go back to Work. and from there to home.
        //this is to make it neat with the PersonalObject spawned
        if (_person.PrevJob == Job.WheelBarrow && _person.Work!=null && _person.Work.HType==H.Masonry)
        {
            return _person.Work;
        }

        return _person.FoodSource;
    }

    bool IsWorkNaval()
    {
        return _person.Work != null && _person.Work.IsNaval();
    }

    Vector3 DefineFinRoute()
    {
        if (IsWorkNaval())
        {
            return _person.Work.SpawnPoint.transform.position;
        }
        else
        {
            return BuildToGoBackTo.SpawnPoint.transform.position;
        }
    }

    void InitRoute()
    {
        RouterActive = true;

        if (BuildToGoBackTo != null)
        {
            InitRouteWithFoodSrc();
        }
        else
        {
            throw new Exception("MyFoodSrc cant be null");
            //InitRouteWithOutFoodSrc();
        }
    }

 
    /// <summary>
    /// Init a route that person will go to FoodSrc and then to Home 
    /// </summary>
    void InitRouteWithFoodSrc()
    {
//       //Debug.Log(_person.MyId+ ".Prev job:" + _person.PrevJob);
        IsRouterBackUsed = true;

        Structure building = null;
        if (_person.PrevOrder!=null)
        {
            building = Brain.GetStructureFromKey(_person.PrevOrder.DestinyBuild);
        }

        if (building == null)
        {
            //exporting will finish at destiny build is null(not sure)
            building = _person.Work;
        }

        Router1 = new CryRouteManager(building, BuildToGoBackTo, _person);
        //Router1 = new RouterManager(building, MyFoodSrc, _person, HPers.InWork);
        RouterBack = new CryRouteManager(BuildToGoBackTo, _person.Home, _person,  HPers.InWork);
    }

    public override void Update()
    {
        if (_takeABreakNow)
        {
            TakeABreak();
            return;
        }

        base.Update();

        //if (_person == null)
        //{
        //    return;
        //}

        WorkAction(HPers.None);
        Execute();
        CheckWhenDone();
    }

    void Execute()
    {
        if (ExecuteNow)
        {
            ExecuteNow = false;
            DropAllMyGoods(BuildToGoBackTo);


            if (ExDockerIsGettingFood())
            {
                _person.GetFood(BuildToGoBackTo);
            }
            else if (_person.PrevJob != Job.Docker)
            {
                _person.GetFood(BuildToGoBackTo);
            }

            _person.Body.ResetPersonalObject();
            ComingBackToOffice();
        }
    }

    /// <summary>
    /// As exDocker will have wheelbarrow willget food once every 4 times random
    /// </summary>
    /// <returns></returns>
    bool ExDockerIsGettingFood()
    {
        if (_person.PrevJob!= Job.Docker)
        {
            return false;
        }
        return UMath.GiveRandom(0, 4) == 0;
    }

    private void CheckWhenDone()
    {
        //person its at home
        if (_person.Body.Location == HPers.Home && _person.Body.GoingTo == HPers.Home)
        {
            _person.HomeActivities();
            //UVisHelp.CreateText(_person.transform.position, "Home Now");

//         //Debug.Log(_person.MyId + " not homer anymore now will be a: " + _person.PrevJob);

            if (_person.PrevJob == Job.WheelBarrow || _person.PrevJob == Job.Builder || _person.PrevJob == Job.None)
            {
                ConvertToWheelBarrOrBuilder();
            }
            else
            {
                _person.CreateProfession(_person.PrevJob);
            }
        }
    }

    void ConvertToWheelBarrOrBuilder()
    {
        
        if (CheckIfCanBeWheelBar(_person))
        {
            _person.CreateProfession(Job.WheelBarrow);
        }
        else
        {
            _person.CreateProfession(Job.Builder);
        }        
    }

    /// <summary>
    /// Checks need to do when done 
    /// </summary>
    public static bool CheckIfCanBeWheelBar(Person per)
    {
        if (per == null || per.Work==null)
        {
            return false;
        }
        //his work is not a BUliding office 
        if (per.Work.BuildersManager1==null)
        {
            return true;
        }

        //will check only when is done 
        //if one building is up. This person will convert into a Builder 
        if (per.Work.BuildersManager1.IsAtLeastOneBuildUp())
        {
            //
            return false;
        }
        return true;
    }


    private bool _takeABreakNow;
    private float _breakDuration = 1f;
    private float startIdleTime;
    private bool _reInit;
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
}
