using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

//Person Brain more complicated clas so far apr 14 2015
//bz the many states 
public class Brain
{
    private HPers _currentTask = HPers.None;
    private HPers _previousTask = HPers.None;

    ///All Routes start on home, if person donstn have home will build shack or bohio
    /// They are only pulbic to be saved by the XML serial
    public TheRoute _workRoute = new TheRoute(); //from home to work 
    public TheRoute _foodRoute = new TheRoute(); //from home to food 
    public TheRoute _idleRoute = new TheRoute(); //from home to idle point 
    public TheRoute _religionRoute = new TheRoute(); //from home to idle point 
    public TheRoute _chillRoute = new TheRoute(); //from home to idle point 

    private CryRouteManager _routerWork;
    private CryRouteManager _routerFood;
    private CryRouteManager _routerIdle;
    private CryRouteManager _routerReligion;
    private CryRouteManager _routerChill;
    Person _person;

    private bool workRouteStart = true;
    private bool foodRouteStart = true;
    private bool idleRouteStart = true;
    private bool religionRouteStart = true;
    private bool chillRouteStart = true;

    private bool _isAllSet;//is true when person has all buildings

    //breaing down 2700 lines of codes in subclasses
    private MoveToNewHome _moveToNewHome;
    private MajorityAgeReached _majorAge;
    
    public HPers CurrentTask
    {
        get { return _currentTask; }
        set
        {
            if (_currentTask == HPers.MovingToNewHome && value != HPers.Restarting)
            {
              ////Debug.Log("CurrTask called:" + _person.MyId + ".new:" + value);
                //return;
            }
            _currentTask = value;
        }
    }

    public HPers PreviousTask
    {
        get { return _previousTask; }
        set { _previousTask = value; }
    }

    public MoveToNewHome MoveToNewHome
    {
        get { return _moveToNewHome; }
        set { _moveToNewHome = value; }
    }

    public MajorityAgeReached MajorAge
    {
        get { return _majorAge; }
        set { _majorAge = value; }
    }

    public Brain()
    {
    }

    public Brain(Person person)
    {
        _moveToNewHome = new MoveToNewHome(this, person);
        _majorAge = new MajorityAgeReached(this, person, MoveToNewHome);

        Init(person);
    }

    /// <summary>
    /// Intended to be used with person loading from file
    /// </summary>
    /// <param name="person"></param>
    /// <param name="pF"></param>
    public Brain(Person person, PersonFile pF)
    {
        _moveToNewHome = new MoveToNewHome(this, person);
        _majorAge = new MajorityAgeReached(this, person, MoveToNewHome) ;

        Init(person);
        LoadFromFile(pF);
    }

    string[] myBuilds;
    void LoadFromFile(PersonFile pF)
    {
        _person.Home = GetStructureFromKey(pF._home);
        _person.Work = GetStructureFromKey(pF._work);
        
        oldJob = pF._work;
        if (string.IsNullOrEmpty(oldJob))
        {
            oldJob = "";//so it keeps the logic on  WorkPositionsUpdate()
        }


        _person.FoodSource = GetStructureFromKey(pF._foodSource);
        _person.Religion = GetStructureFromKey(pF._religion);
        _person.Chill = GetStructureFromKey(pF._chill);

        CurrentTask = pF._brain.CurrentTask;
        _previousTask = pF._brain.PreviousTask;

        _workRoute = pF._brain._workRoute;
        _foodRoute = pF._brain._foodRoute;
        _idleRoute = pF._brain._idleRoute;
        _religionRoute = pF._brain._religionRoute;
        _chillRoute = pF._brain._chillRoute;
        goMindState = true;

        //so routes dont get started 
        oldHome = pF._home;
        oldWork = pF._work;
        oldFoodSrc = pF._foodSource;
        oldReligion = pF._religion;
        oldChill = pF._chill;

        //if we have an old route will start that one 
        WillRedoOldLoadedRoutes();

        //so the MindState() works
        _routerFood.IsRouteReady = true;
        _routerWork.IsRouteReady = true;
        _routerIdle.IsRouteReady = true;
        _routerReligion.IsRouteReady = true;
        _routerChill.IsRouteReady = true;

        _generalOldKeysList = pF._brain.GeneralOldKeysList;

        MoveToNewHome.HomeOldKeysList = pF._brain.MoveToNewHome.HomeOldKeysList;
        MoveToNewHome.OldHomeKey = pF._brain.MoveToNewHome.OldHomeKey;
        MoveToNewHome.RouteToNewHome = pF._brain.MoveToNewHome.RouteToNewHome;

        //is a shackBuilder , or its house was destroyed and father or mom is buiding shack
        if (_person.Home == null)
        {
            return;
        }

    }

    /// <summary>
    /// Created to redo old routes quickly. Once the are make "" the method CheckIfABuildWasChange() will 
    /// address the rerest
    /// </summary>
    void WillRedoOldLoadedRoutes()
    {
        if (_workRoute != null && _person.Work != null && _workRoute.DestinyKey != _person.Work.MyId)
        {
            oldWork = "";
        }
        if (_foodRoute != null && _person.FoodSource != null && _foodRoute.DestinyKey != _person.FoodSource.MyId)
        {
            oldFoodSrc = "";
        }
        if (_religionRoute != null && _person.Religion != null && _religionRoute.DestinyKey != _person.Religion.MyId)
        {
            oldReligion = "";
        }
        if (_chillRoute != null && _person.Chill != null && _chillRoute.DestinyKey != _person.Chill.MyId)
        {
            oldChill = "";
        }
    }

    /// <summary>
    /// Will return a Structure given a key
    /// </summary>
    /// <param name="keyP">Key to find Structure on 'BuilderPot.Control.Registro.AllBuilding'</param>
    static public Structure GetStructureFromKey(string keyP)
    {
        if (string.IsNullOrEmpty(keyP) || !BuildingPot.Control.Registro.AllBuilding.ContainsKey(keyP))
        { return null; }

        return BuildingPot.Control.Registro.AllBuilding[keyP] as Structure;
    }

    /// <summary>
    /// Will return a Building given a key
    /// </summary>
    /// <param name="keyP">Key to find Structure on 'BuilderPot.Control.Registro.AllBuilding'</param>
    static public Building GetBuildingFromKey(string keyP)
    {
        if (string.IsNullOrEmpty(keyP) || !BuildingPot.Control.Registro.AllBuilding.ContainsKey(keyP))
        { return null; }

        return BuildingPot.Control.Registro.AllBuilding[keyP];
    }

    public void Init(Person person)
    {
        _person = person;
        _routerFood = new CryRouteManager();
        _routerIdle = new CryRouteManager();
        _routerWork = new CryRouteManager();
        _routerReligion = new CryRouteManager();
        _routerChill = new CryRouteManager();
    }

    private DateTime askWork = new DateTime();
    private DateTime askFood = new DateTime();
    private DateTime askIdle = new DateTime();
    private DateTime askReligion = new DateTime();
    private DateTime askChill = new DateTime();

    void DefineWorkRoute()
    {
        AddToGenOldKeyIfAOldRouteHasOneOldBridgeOnIt(_workRoute);

       // _routerWork = new CryRouteManager(_person.Home, _person.Work, _person, HPers.Work, askDateTime: askWork);
        _routerWork = new CryRouteManager(_person.Home, _person.Work, _person, askDateTime: askWork);
     
        workRouteStart = true;
    }

    void DefineFoodSourceRoute()
    {
        AddToGenOldKeyIfAOldRouteHasOneOldBridgeOnIt(_foodRoute);

       // _routerFood = new RouterManager(_person.Home, _person.FoodSource, _person, HPers.FoodSource, askDateTime: askFood);
        _routerFood = new CryRouteManager(_person.Home, _person.FoodSource, _person, askDateTime: askFood);

        foodRouteStart = true;
    }

    private Structure dummyIdle;
    private Vector3 _idlePoint;
    void DefineIdleRoute()
    {
        AddToGenOldKeyIfAOldRouteHasOneOldBridgeOnIt(_idleRoute);
        _idlePoint = Return1HouseCorner();

        if (dummyIdle == null || dummyIdle.transform.position == new Vector3())
        {
            dummyIdle = CreateDummyIdle();
        }

        dummyIdle.transform.position = _idlePoint;
        dummyIdle.HandleLandZoning();

        //_routerIdle = new CryRouteManager(_person.Home, GameScene.dummySpawnPoint, _person, finDoor:false);
        _routerIdle = new CryRouteManager(_person.Home, dummyIdle, _person, finDoor: false);

        idleRouteStart = true;
    }

    Vector3 Return1HouseCorner()
    {
        var chosen = _person.Home.Anchors[UMath.GiveRandom(0, 4)];
        return Vector3.MoveTowards(chosen, _person.Home.transform.position, -.2f);
    }

    Structure CreateDummyIdle()
    {
        //return Program.gameScene.GimeMeUnusedDummy(_person.MyId+".Idle.Dummy");

        dummyIdle = (Structure)Building.CreateBuild(Root.dummyBuildWithSpawnPoint, new Vector3(), H.Dummy,
            container: Program.ClassContainer.transform);

        return dummyIdle;
    }

    void ResetDummyIdle()
    {
        if (dummyIdle == null)
        {
            return;
        }
        //Program.gameScene.ReturnUsedDummy(dummyIdle);
        dummyIdle = null;
    }

    void DefineReligionRoute()
    {
        AddToGenOldKeyIfAOldRouteHasOneOldBridgeOnIt(_religionRoute);

        //_routerReligion = new RouterManager(_person.Home, _person.Religion, _person, HPers.Religion, askDateTime: askReligion);
        _routerReligion = new CryRouteManager(_person.Home, _person.Religion, _person, askDateTime: askReligion);
        religionRouteStart = true;
    }

    void DefineChillRoute()
    {
        AddToGenOldKeyIfAOldRouteHasOneOldBridgeOnIt(_chillRoute);

        //_routerChill = new RouterManager(_person.Home, _person.Chill, _person, HPers.Chill, askDateTime: askChill);
        _routerChill = new CryRouteManager(_person.Home, _person.Chill, _person, askDateTime: askChill);
        chillRouteStart = true;
    }

    ///// <summary>
    ///// Idle route is half way going to food source route 
    ///// </summary>
    //void DefineIdleRoute()
    //{
    //    CheckPoint[] array = _foodRoute.CheckPoints.ToArray();

    //    for (int i = 0; i < array.Length / 2; i++)
    //    {
    //        _idleRoute.CheckPoints.Add(array[i]);
    //    }
    //    //CheckPoint halfPoint = array[(array.Length / 2) - 1];
    //    //CheckPoint halfPointPls1 = _foodRoute.CheckPoints[(_foodRoute.CheckPoints.Count / 2)];
    //    //Vector3 mid = (halfPoint.Point + halfPointPls1.Point) /2 ;
    //    //_idleRoute.CheckPoints.Add(new CheckPoint(mid, halfPoint.QuaterniRotation));
    //    //_idleRoute.CheckPoints.Add(halfPoint);
    //}

    /// <summary>
    /// So it only kills the brdige after we create a new route 
    /// 
    /// To address that brdiges were killed way before new routes were created 
    /// 
    /// Will turn the goMindState to false so can do the route. Is here bz when was b4 will stop the 
    /// Brain from going  
    /// </summary>
    void AddToGenOldKeyIfAOldRouteHasOneOldBridgeOnIt(TheRoute theRoute)
    {
        GoMindState = false;
       ////Debug.Log("AddToGenOldKey goMindState");

        if (String.IsNullOrEmpty(theRoute.BridgeKey))
        {
            return;
        }

        if (!BuildingPot.Control.Registro.AllBuilding.ContainsKey(theRoute.BridgeKey))
        {
            return;
        }

        //what happen is that it keeps creatign new routes and adding the bridge as an old key
        //and removes people from the bridge People Dict
        var bridge = BuildingPot.Control.Registro.AllBuilding[theRoute.BridgeKey];
        if (bridge.Instruction != H.WillBeDestroy)
        { return; }

        AddToList(_generalOldKeysList, theRoute.BridgeKey);
    }

    #region MindStates
    /// <summary>
    /// Really important the mind states . 
    /// Depending where the person is and goping and doing will do neext state
    /// </summary>
    void MindState()
    {
        //RemoveFromSystemIfNeed();


        //SkipIdleInHome();
        GoIdleInHome();//to clear and check stuff in case is not doing it like when is only working 

        SkipWork();
        GoWork();

        SkipFood();
        GoGetFood();

        GoIdle();

        SkipReligion();
        GoToReligion();

        SkipChill();
        GoChill();

        GoToNewHome();
    }

    /*Promts are created with the purpose of allow to be able to move arond only with 2 builds
     * If the person is ready to move to a new state and that building we are going to is not null is all good...
     * But if is null the promt will make Brain vars equal so next condition can be executed . and so on 
     */
    //private void SkipIdleInHome()
    //{
    //    //doesnt need anything bz need to execute IdleInHome() Always 
    //}


    public void SkipWorkForced()
    {
        ReadyToGetFood(true);
    }

    void SkipWork()
    {

        if ((ReadyToWork() && _person.Work == null)// || 
           // (ReadyToWork() && (_person.ProfessionProp == null|| !_person.ProfessionProp.ReadyToWork))
            ) 
        {
            ReadyToGetFood(true);
        }
    }

    private void SkipFood()
    {
        if (ReadyToGetFood() && (_person.FoodSource == null || _person.Age < _jobManager.startSchool))
        {
            ReadyToGoToReligion(true);
        }
    }

    private void SkipReligion()
    {
        if (ReadyToGoToReligion() && _person.Religion == null)
        {
            ReadyToGoChill(true);
        }
    }

    /// <summary>
    /// If is a minor will prompted to work
    /// </summary>
    private void SkipChill()
    {
        if (ReadyToGoChill() && (_person.Chill == null || !UPerson.IsMajor(_person.Age)))
        {
            ReadyToIdleInHome(true);
        }
    }

    /* This bools below are created to modularize the bool when asking for a specific state if is ready for its execution
     * This is to start the proccess of a person being able to move around only with 2 structures .. so wont be needed all
     * 5 structueres to work 
     */

    /// <summary>
    /// Tell if person is ready to work. 
    /// </summary>
    /// <returns></returns>

    bool ReadyToIdleInHome(bool makeItTrue = false)
    {
        if (makeItTrue)
        {
            CurrentTask = HPers.None;
            _person.Body.Location = HPers.None;
        }

        return CurrentTask == HPers.None &&
               (_person.Body.Location == HPers.Home || _person.Body.Location == HPers.None);
    }


    bool ReadyToWork(bool makeItTrue = false)
    {
        //this is when the prev state is calling this so it the state can get prompted to this new state 
        if (makeItTrue)
        {
            CurrentTask = HPers.IdleInHome;
            _person.Body.Location = HPers.None;
        }

        return   CurrentTask == HPers.IdleInHome &&
               (_person.Body.Location == HPers.Home || _person.Body.Location == HPers.None);
    }

    bool ReadyToGetFood(bool makeItTrue = false)
    {
        if (makeItTrue)
        {
            CurrentTask = HPers.Walking;
            _person.Body.Location = HPers.Home;
            _person.Body.GoingTo = HPers.Home;
        }

        return CurrentTask == HPers.Walking && _person.Body.Location == HPers.Home && _person.Body.GoingTo == HPers.Home;
    }

    bool ReadyToGoIdle(bool makeItTrue = false)
    {
        if (makeItTrue)
        {
            CurrentTask = HPers.IdleInHome;
            _person.Body.Location = HPers.Home;
            _person.Body.GoingTo = HPers.Home;
            _previousTask = HPers.IdleSpot;
        }

        return CurrentTask == HPers.IdleInHome && _person.Body.Location == HPers.Home &&
               _person.Body.GoingTo == HPers.Home
                && _previousTask == HPers.IdleSpot;
    }

    bool ReadyToGoToReligion(bool makeItTrue = false)
    {
        if (makeItTrue)
        {
            _previousTask = HPers.Praying;
            CurrentTask = HPers.IdleInHome;
            _person.Body.Location = HPers.Home;
            _person.Body.GoingTo = HPers.Home;
        }

        return _previousTask == HPers.Praying && CurrentTask == HPers.IdleInHome &&
               _person.Body.Location == HPers.Home && _person.Body.GoingTo == HPers.Home;
    }

    bool ReadyToGoChill(bool makeItTrue = false)
    {
        if (makeItTrue)
        {
            _previousTask = HPers.Chilling;
            CurrentTask = HPers.IdleInHome;
            _person.Body.Location = HPers.Home;
            _person.Body.GoingTo = HPers.Home;
        }

        return _previousTask == HPers.Chilling && CurrentTask == HPers.IdleInHome
               && (_person.Body.Location == HPers.Home || _person.Body.Location == HPers.None)
               && _person.Body.GoingTo == HPers.Home
               ;
    }

    //////////////////////////////
    /// <summary>
    /// Below all the Mind States Posible, this is the Logic of the Brain 
    /// </summary>
    //////////////////////////////



    private void GoIdleInHome()
    {
        if (ReadyToIdleInHome())
        {
            Idle(HPers.IdleInHome);
        }
    }


    void GoWork()
    {
        if (ReadyToWork() && _routerWork.IsRouteReady && _workRoute.CheckPoints.Count > 0
            && _workRoute.DestinyKey == _person.Work.MyId
            )
        {
            _person.Body.WalkRoutine(_workRoute, HPers.Work);
            CurrentTask = HPers.Walking;
        }
        else if (CurrentTask == HPers.Walking && _person.Body.Location == HPers.Work && _person.Body.GoingTo == HPers.Work)
        {
            CurrentTask = HPers.Working;
        }
        //("work now")
        else if (CurrentTask == HPers.Working && _person.Body.Location == HPers.Work)
        {
            //this is to address when loading a person to a Job that is marked to be destroy 
            if (_person.ProfessionProp.ProfDescription != Job.None)
            {
                _person.ProfessionProp.WorkAction(HPers.None);
            }
            else Idle(HPers.None);
        }
        else if (CurrentTask == HPers.None && _person.Body.Location == HPers.Work)
        {
            //to avoid jump to dock and then back to current building 
            if (
                //_person.PrevJob == Job.Docker && 
                _person.ProfessionProp.ProfDescription == Job.Homer)
            {
                return;
            }

            //print("back home now");
            _person.Body.WalkRoutine(_workRoute, HPers.Home, true);
            CurrentTask = HPers.Walking;
        }
    }

    void GoGetFood()
    {   //get food
        if (ReadyToGetFood() && _foodRoute!=null && _foodRoute.DestinyKey == _person.FoodSource.MyId)
        {
            CurrentTask = HPers.GettingFood;
        }
        else if (CurrentTask == HPers.GettingFood && _person.Body.Location == HPers.Home && _routerFood.IsRouteReady
            && _foodRoute.CheckPoints.Count > 0)
        {
            //print("getting food now");
            _person.Body.WalkRoutine(_foodRoute, HPers.FoodSource);
            CurrentTask = HPers.Walking;
        }
        else if (CurrentTask == HPers.Walking && _person.Body.Location == HPers.FoodSource && _person.Body.GoingTo == HPers.FoodSource)
        {
            _person.ProfessionProp.DropGoods();
            _person.GetFood(_person.FoodSource);
            _person.Body.WalkRoutine(_foodRoute, HPers.Home, true);
            CurrentTask = HPers.IdleSpot;//to idle
        }
        else if (CurrentTask == HPers.IdleSpot && _person.Body.Location == HPers.Home && _person.Body.GoingTo == HPers.Home)
        {
            _previousTask = HPers.IdleSpot;
            Idle(HPers.IdleInHome);
        }
    }

    void GoIdle()
    {
        //go idle
        if (ReadyToGoIdle() && _idleRoute.CheckPoints.Count > 0)
        {
            _person.Body.WalkRoutine(_idleRoute, HPers.IdleSpot, false, HPers.IdleSpot);
            CurrentTask = HPers.IdleSpot;
        }
        else if (CurrentTask == HPers.IdleSpot && _person.Body.Location == HPers.IdleSpot)
        {
            //print("idling now");
            Idle(HPers.Homing);
        }
        else if (CurrentTask == HPers.Homing && _person.Body.Location == HPers.IdleSpot && _person.Body.GoingTo == HPers.IdleSpot)
        {
            //HomeActivities();
            _person.Body.WalkRoutine(_idleRoute, HPers.Home, true, HPers.IdleSpot);
            CurrentTask = HPers.Praying;
        }
        else if (CurrentTask == HPers.Praying && _person.Body.Location == HPers.Home && _person.Body.GoingTo == HPers.Home)
        {
            _previousTask = HPers.Praying;
            Idle(HPers.IdleInHome);
        }
    }

    private void GoToReligion()
    {
        if (ReadyToGoToReligion() && _religionRoute.CheckPoints.Count > 0)
        {
            CurrentTask = HPers.Praying;
            _person.Body.WalkRoutine(_religionRoute, HPers.Religion);
        }
        else if (CurrentTask == HPers.Praying && _person.Body.Location == HPers.Religion)
        {
            //print("at religion now");
            //Idle(HPers.Homing);
            CurrentTask = HPers.Homing;
        }
        else if (CurrentTask == HPers.Homing && _person.Body.Location == HPers.Religion && _person.Body.GoingTo == HPers.Religion)
        {
            _person.Body.WalkRoutine(_religionRoute, HPers.Home, true);
            CurrentTask = HPers.Chilling;
        }
        else if (CurrentTask == HPers.Chilling && _person.Body.Location == HPers.Home && _person.Body.GoingTo == HPers.Home)
        {
            _previousTask = HPers.Chilling;
            Idle(HPers.IdleInHome);
        }
    }

    private void GoChill()
    {
        if (ReadyToGoChill() && _chillRoute.CheckPoints.Count > 0)
        {
            CurrentTask = HPers.Chilling;
            _person.Body.WalkRoutine(_chillRoute, HPers.Chill);
        }
        else if (CurrentTask == HPers.Chilling && _person.Body.Location == HPers.Chill)
        {
            //print("at chill now");
            //Idle(HPers.Homing);
            CurrentTask = HPers.Homing;
        }
        else if (CurrentTask == HPers.Homing && _person.Body.Location == HPers.Chill && _person.Body.GoingTo == HPers.Chill)
        {
            _person.Body.WalkRoutine(_chillRoute, HPers.Home, true);
            CurrentTask = HPers.None;//so reset the cycle
        }
    }

    private void GoToNewHome()
    {
        if (CurrentTask == HPers.MovingToNewHome
            //none is if person is just spwaneed and foudn new home 
            && (_person.Body.Location == HPers.Home || _person.Body.Location == HPers.None) && 
            _person.Body.GoingTo == HPers.Home
            && MoveToNewHome.RouteToNewHome.CheckPoints.Count > 0)
        {
            _person.Body.WalkRoutine(MoveToNewHome.RouteToNewHome, HPers.MovingToNewHome);
        }
        else if (CurrentTask == HPers.MovingToNewHome && _person.Body.Location == HPers.MovingToNewHome)
        {
            CurrentTask = HPers.Restarting;//so reset the cycle;
        }
        else if (CurrentTask == HPers.Restarting && _person.Body.Location == HPers.MovingToNewHome && _person.Body.GoingTo == HPers.MovingToNewHome)
        {
            CurrentTask = HPers.None;//so reset the cycle
            _person.Body.Location = HPers.Home;
            _person.Body.GoingTo = HPers.Home;//Home

            GoToNewHomeTail();
        }
    }

    /// <summary>
    /// Created for modularity and be called from another method 
    /// </summary>
    void GoToNewHomeTail()
    {
       ////Debug.Log("1 got to new home:" + _person.MyId + ".famID" + _person.FamilyId);

        //unbook
        _person.IsBooked = "";
        if (_person.Home.BookedHome1 != null)
        {
            //this is important here. They will be removed from Booking only when phisically are in the new home.
            //in tht way if anything happens that change the .Home, they will always be able to get the booked building
            //again and agin until are phisically in it
            _person.Home.BookedHome1.RemovePersonFromBooking(_person);
        }
        _person.DropFoodAtHome();

        //will add Home to avail spot if can
        var oldHomeH = GetStructureFromKey(MoveToNewHome.OldHomeKey);
        if (oldHomeH !=  null)
        {
            AddOldHomeToAvailHomeIfHasSpace(oldHomeH);
            RemoveMeFromOldHomeFamily(oldHomeH);
        }

       ////Debug.Log("2 got to new home:" + _person.MyId+".famID"+_person.FamilyId);
        MoveToNewHome.GetMyNameOutOfOldHomePeopleList();
        MoveToNewHome.CleanUpRouteToNewHome();

        //will add to genOldKeys since he wont use that route ever again. 
        AddToList(_generalOldKeysList, MoveToNewHome.RouteToNewHome.BridgeKey);

        //CheckIfClearBlackList();
    }

    private void RemoveMeFromOldHomeFamily(Structure oldHomeH)
    {
        for (int i = 0; i < oldHomeH.Families.Length; i++)
        {
            oldHomeH.Families[i].RemovePersonFromFamily(_person);
        }
    }

    #endregion

    private const float IDLETIME = 1f;
    private float _idleTime = 1f;//.5  4.5

    private float startIdleTime;
    private bool _isIdleHomeNow;//will tell if person is at home idleing now 

    //says if we ask for new routes. Created to stop the goMindState until the new routes are finished
    //this bool will hold the idle until the goStateMind is true. Then will release the idle 
    private bool _routesWereStarted;

    public bool RoutesWereStarted
    {
        get { return _routesWereStarted; }
        set { _routesWereStarted = value; }
    }

    /// <summary>
    /// Idle Action 
    /// </summary>
    /// <param name="nextTask">The task will have after Idle is done</param>
    void Idle(HPers nextTask)
    {
        if (startIdleTime == 0)
        { startIdleTime = Time.time; }

        if (nextTask == HPers.IdleInHome)
        {
            _isIdleHomeNow = true;
            ListOfActionsInHome();
        }

        AddIdleTimeIfHasOldKeys();

        if (Time.time > startIdleTime + _idleTime && !_routesWereStarted)
        {
            RealeaseIdle(nextTask);
        }
    }

    public void RealeaseIdle(HPers nextTask)
    {
        CurrentTask = nextTask;
        startIdleTime = 0;
        _idleTime = IDLETIME;//.5   4.5
        GoMindState = true;
        _isIdleHomeNow = false;
    }

    /// <summary>
    /// Created to make sure when is in home all things he need get done will get done 
    /// </summary>
    void ListOfActionsInHome()
    {
        _routesWereStarted = StartRoutes();
        _person.ProfessionProp.Update();//bz need to do somesutff when on home 

        //in case we have something to do 
        CheckConditions();
        _person.HomeActivities();
    }

    void ReRoutes()
    {
        //this are the 2 method tht do ReRouting
        CheckOnTheQueues();
        CheckIfABuildWasChange();
    }

    /// <summary>
    /// Will extend idle in home until we resolve the old keys we have 
    /// </summary>
    /// <param name="nextTask"></param>
    void AddIdleTimeIfHasOldKeys()
    {
        if (IAmHomeNow() && _routesWereStarted)
        {
            GoMindState = false;
            _idleTime++;
        }

        if (_idleTime > 5f)
        {
            _idleTime = 5f;
        }
    }

    public void Update()
    {
        DefineIfIsAllSet();
        DefineIfWaiting();

        UpdateRouters();



        //used to be below mindState
        StartRoutes();
        SetFinalRoutes();
        MoveToNewHome.BuildRouteToNewHomeRoutine();
        SearchAgain();
        if (!PersonPot.Control.Locked && _person.Home != null)
        {
            ExecuteSlowCheckUp();
        }
        Die();




        //if wating for rerouting must wait at home
        if (_waiting)
        {
            ReRoutesDealer();
            return;
        }

        if (goMindState)
        { MindState(); }
    }




    #region Rerouting Pool



    private int _timesCall;

    //u are only waiting if u are checked in ready to reroute
    private bool _waiting;//waiting to reroute 

    public bool Waiting
    {
        get { return _waiting; }
        set { _waiting = value; }
    }

    //last time I got into Checkin for new routes OnSystem
    //here so people when is the one of the first are not getting in all the time 
    private float _lastTimeICheckedInOnSystem;
    private float _delayToGetIntoOnSystem = 5f;

    /// <summary>
    /// Created to the Update doesnt run and ruins the Raoutes Start Booleans
    /// </summary>
    private void DefineIfWaiting()
    {
        //if is in process of moving dont need to deal with this
        if (MoveToNewHome.RouteToNewHome.CheckPoints.Count > 0 || _waiting)
        {
            return;
        }

        if (IJustSpawn() || IAmHomeNow() || LocatedAtHomeNow())
        {
            if (PersonPot.Control.CanIReRouteNow(_person.MyId) && Time.time > _lastTimeICheckedInOnSystem + _delayToGetIntoOnSystem)
            {
                CheckMeOnSystemNow();
            }
        }
    }

    /// <summary>
    /// So when reroute to new home needs to reroute all other as well
    /// </summary>
    public void CheckMeOnSystemNow()
    {
        //Debug.Log("Check OnSysyemt NOw:"+_person.MyId);
        PersonPot.Control.CheckPeopleIn(_person.MyId);

        PersonPot.Control.CheckMeOnSystem(_person.MyId);
        _lastTimeICheckedInOnSystem = Time.time;
        _waiting = true;
    }

    void AddMeToSystemWaitingList()
    {

        //if it was added to waiting list 
        if (PersonPot.Control.AddMeToOnSystemWaitList(_person.MyId))
        {
            PersonPot.Control.CheckPeopleIn(_person.MyId);

            _waiting = true;
        }
    }

    private bool LocatedAtHomeNow()
    {
        if (_person.Body.Location == HPers.Home && _person.Body.GoingTo == HPers.Home)
        {
            return true;
        }
        return false;
    }

    void ReRoutesDealer()
    {
        //so things get started 
        if (IJustSpawn() || IAmHomeNow() || IsInLimbo() || LocatedAtHomeNow())
        {
            //there is room for me to check now on System
            if (PersonPot.Control.OnSystemNow(_person.MyId) && _waiting)
            {
                //NewBornStuff();

                //redo routes to see if some change 
                ReRoutes();
                ReRouteCallsCounter();
            }
        }
    }

    /// <summary>
    /// When at home but location says none and is going to home 
    /// 
    /// So blacklisting works 
    /// </summary>
    /// <returns></returns>
    private bool IsInLimbo()
    {
        if (_person.Body.Location == HPers.None && _person.Body.GoingTo == HPers.Home)
        {
            //print("Just Spawn:"+Person.MyId);generalOldKeysList
            return true;
        }
        return false;
    }
    
    public void YourTurnToReRoute()
    {
        Update();
    }

    /// <summary>
    /// So a person can ask twice for rerouting then thats it 
    /// </summary>
    void ReRouteCallsCounter()
    {
        _timesCall++;
        if (_waiting && _timesCall > 65)//20 60
        {
            PersonPot.Control.DoneReRoute(_person.MyId);//so another people can use the Spot 
            _timesCall = 0;
            _waiting = false;

            //is called here bz regardless he updated or not his routes he checked into the elements
            //bz he wont reroute again
            PersonPot.Control.Queues.CheckMeInToQueueElements(_person.MyId);
        }
    }

#endregion


    /// <summary>
    /// Define isAllSet bool
    /// </summary>
    private void DefineIfIsAllSet()
    {
        //if (!_isAllSet)
        //{
        if (_person.Home != null &&
            (_person.Work != null || _person.FoodSource != null || _person.Religion != null || _person.Chill != null))
        {
            _isAllSet = true;
        }
        else
        {
            _isAllSet = false;

          
        }
        //}
    }

    /// <summary>
    /// So only check if a new builiding was changed if guy is home
    /// </summary>
    public bool IAmHomeNow()
    {
        return _isIdleHomeNow;
    }

    /// <summary>
    /// Needed for when the guys is spawn. First event in CheckIfABuildWasChange() starts
    /// </summary>
    bool IJustSpawn()
    {
        if (_person.Body.Location == HPers.None && _person.Body.GoingTo == HPers.None)
        {
            //print("Just Spawn:"+Person.MyId);generalOldKeysList
            return true;
        }
        return false;
    }


    private string oldHome;
    private string oldWork;
    private string oldFoodSrc;
    private Vector3 oldIdle;
    private string oldReligion;
    private string oldChill;
    //I will keep all the other buildings beside homse old keys here so in case a 
    //old key is overwritten is kepts here and then I will clear this list
    private List<string> _generalOldKeysList = new List<string>();

    public List<string> GeneralOldKeysList
    {
        get { return _generalOldKeysList; }
        set { _generalOldKeysList = value; }
    }

    /// <summary>
    /// This method starts all the routes too since the var above have empty vvalues
    /// 
    /// If isAllSet will find if a new building was changed ,, 
    /// ex: used to work in Clay now is working on FishSite
    /// 
    /// Evaluates if a new value is beeing found. For ex. if  nnew job is being found is bz is 
    /// diff that the value stored in 'oldWork'. Then will act upon and will store new value found in
    /// 'oldWork'
    /// </summary>
    private void CheckIfABuildWasChange()
    {
       // if (_isAllSet && (IAmHomeNow() || IJustSpawn()))
        if (_isAllSet)
        {
            //if home was changed u need to star all routes 
            if (oldHome != _person.Home.MyId)
            {
               ////Debug.Log(_person.MyId + " redoing all routes");

                workRouteStart = false;
                foodRouteStart = false;
                idleRouteStart = false;
                religionRouteStart = false;
                chillRouteStart = false;

                if (!IJustSpawn())
                {
                    //false pass as param so it doesnt remove the people from Houses
                    RestartVarsAndAddToGenList();
                }
                oldHome = _person.Home.MyId;
            }

            //work related
            if (_person.Work != null && oldWork != _person.Work.MyId)
            {
                _person.CreateProfession();//if a new job was found need to create a profession 
                workRouteStart = false;

                RestartVarsAndAddToGenList();
                oldWork = _person.Work.MyId;
            }


            if (_person.FoodSource != null && oldFoodSrc != _person.FoodSource.MyId)
            {
                foodRouteStart = false;
                //Debug.Log("New FoodSrc:" + _person.FoodSource+" ."+_person.MyId);

                RestartVarsAndAddToGenList();
                oldFoodSrc = _person.FoodSource.MyId;

                RedoProfession();
            }
            if (_person.Religion != null && oldReligion != _person.Religion.MyId)
            {
                religionRouteStart = false;

                RestartVarsAndAddToGenList();
                oldReligion = _person.Religion.MyId;
            }
            if (_person.Chill != null && oldChill != _person.Chill.MyId)
            {
                chillRouteStart = false;

                RestartVarsAndAddToGenList();
                oldChill = _person.Chill.MyId;
            }
        }
    }

   

    /// <summary>
    /// To address when a person changes FoodSrc , must change The whole Routes in Profession 
    /// otherwise will keep routes to old FoodSrc 
    /// </summary>
    private void RedoProfession()
    {
        if (_person.Work != null)
        {
           ////Debug.Log("Redoing Profesional:"+_person.MyId+" . "+_person.Work.MyId);
            _person.CreateProfession();
        }
    }

    #region Job Manangment

    /// <summary>
    /// If old job is not null or "" will remove from that Job position and 
    /// to the new one will be added to it 
    /// </summary>
    void RemoveAndAddPositionsToJob()
    {
        //students to get added to PeopleDict
        //if (_person.IsStudent)
        //{
        //    return;
        //}

        if (!string.IsNullOrEmpty(oldWork))
        {
            var build = GetBuildingFromKey(oldWork);
            build.RemovePosition();
        }

        if (_person.Work != null)
        {
            _person.Work.FillPosition();
        }
    }

#endregion

    public void AddToNewBuildRemoveFromOld(Structure oldBuildKey, string newBuildKey)
    {
        if (oldBuildKey != null)
        {
            if (oldBuildKey.MyId == newBuildKey)//so it doest get all the buildings
            { return; }

            //if its home has to be added to anotehr list 
            if (oldBuildKey != _person.Home)
            {
                if (!_generalOldKeysList.Contains(oldBuildKey.MyId))
                {
                    //_generalOldKeysList.Add(oldBuildKey.MyId);
                    RemoveFromPeopleDict(oldBuildKey.MyId);
                }
            }
        }
        AddToPeopleDict(newBuildKey);
    }

    /// <summary>
    /// The person gets added to the building peoples Dict
    /// </summary>
    void AddToPeopleDict(string keyP)
    {
        if (keyP == "")
        { return; }

        Structure s = BuildingPot.Control.Registro.AllBuilding[keyP] as Structure;
        if (!s.PeopleDict.Contains(_person.MyId))
        {
            s.PeopleDict.Add(_person.MyId);
            BuildingPot.Control.Registro.ResaveOnRegistro(s.MyId);
        }
        //print("added:" + Person.MyId + ".bld:" + currStructure.MyId);
    }

    /// <summary>
    /// Will add the person that is 'personKey' to the People Dict on the Building that has 'buildKey'
    /// </summary>
    /// <param name="buildKey"></param>
    /// <param name="personKey"></param>
    public void AddToBridgePeopleList(string buildKey, string personKey)
    {
        Building s = BuildingPot.Control.Registro.AllBuilding[buildKey];
        if (!s.PeopleDict.Contains(personKey))
        {
            s.PeopleDict.Add(personKey);
            BuildingPot.Control.Registro.ResaveOnRegistro(s.MyId);

        }
    }

    /// <summary>
    /// Restart the 'goMindState' 
    void RestartVarsAndAddToGenList()
    {
        GoMindState = false;
       ////Debug.Log("RestartVarsAndAddToGenList goMindState");
    }

    /// <summary>
    /// Checks if build was emptied and if was is destroyed... for all builds ...
    /// 
    /// If build was emptied and was a shack will be destroyed  
    /// </summary>
    public void DestroyOldBuildIfEmptyOrShack(string oldBuild)
    {
        if (!BuildingPot.Control.Registro.AllBuilding.ContainsKey(oldBuild))
        {
            return;
        }

        Building s = BuildingPot.Control.Registro.AllBuilding[oldBuild];
        if (s.PeopleDict.Count == 0 && 
            (s.Instruction == H.WillBeDestroy || s.HType == H.Shack))
        {
            s.DestroydHiddenBuild();
        }

        //GameScene.print(_person.MyId + "."+oldBuild + ".Count:" + s.PeopleDict.Count);
    }



    public bool GoMindState
    {
        get { return goMindState; }
        set
        {
            if (goMindState && !value)
            {
                var t = 1;
              ////Debug.Log(_person.MyId+" goMind false");
            }
            
            goMindState = value;
        }
    }


    bool DefineIfRouterHasABlackListedBuild(CryRouteManager router)
    {
        if (router == null)
        {
            return false;
        }
        return BlackList.Contains(router.OriginKey) || BlackList.Contains(router.DestinyKey);
    }

    //if is true the Brain will executed the MindStates()
    private bool goMindState;
    /// <summary>
    /// Routes Have to be set after they were started and are ready. Bz they are progesively going thru all 
    /// called by update 
    /// </summary>
    void SetFinalRoutes()
    {
        //this is so Person dont stay stuff there bz has some bacl listed buildings
        //what happens is that in ChangesBuildings this is start but never ended bz
        //then Building becomes null 
        var religBlack = DefineIfRouterHasABlackListedBuild(_routerReligion);
        var workBlack = DefineIfRouterHasABlackListedBuild(_routerWork);
        var foodBlack = DefineIfRouterHasABlackListedBuild(_routerFood);
        var chillBlack = DefineIfRouterHasABlackListedBuild(_routerChill);


        if (workRouteStart && _routerWork.IsRouteReady && _workRoute.CheckPoints.Count == 0)
        {
            _workRoute = _routerWork.TheRoute;
            CheckIfGoMindReady();
        }
        if (workRouteStart && workBlack)
        {
            workRouteStart = false;
            CheckIfGoMindReady();
        }


        if (foodRouteStart && _routerFood.IsRouteReady && _foodRoute.CheckPoints.Count == 0)
        {
            _foodRoute = _routerFood.TheRoute;
            CheckIfGoMindReady();
        }
        if (foodRouteStart && foodBlack)
        {
            foodRouteStart = false;
            CheckIfGoMindReady();
        }


        if (religionRouteStart && _routerReligion.IsRouteReady && _religionRoute.CheckPoints.Count == 0)
        {
            _religionRoute = _routerReligion.TheRoute;
            CheckIfGoMindReady();
        }
        if (religionRouteStart && religBlack)
        {
            religionRouteStart = false;
            CheckIfGoMindReady();
        }


        if (chillRouteStart && _routerChill.IsRouteReady && _chillRoute.CheckPoints.Count == 0)
        {
            _chillRoute = _routerChill.TheRoute;
            CheckIfGoMindReady();
        }
        if (chillRouteStart && chillBlack)
        {
            chillRouteStart = false;
            CheckIfGoMindReady();
        }


        if (idleRouteStart && _routerIdle.IsRouteReady && _idleRoute.CheckPoints.Count == 0)
        {
            ResetDummyIdle();

            _idleRoute = _routerIdle.TheRoute;
            CheckIfGoMindReady();
        }
    }

    /// <summary>
    /// Created to see if _isAllSet if true. If is and this is called from the SetFinalRoutes() once a new route is being
    /// establish then we can try to give it a go to goMindState
    /// </summary>
    void CheckIfGoMindReady()
    {
        _routesWereStarted = false;
        DefineIfIsAllSet();
        if (_isAllSet)
        {
            GoMindState = true;
            UpdateBridgeKeyListMarked();
        }
    }

    /// <summary>
    /// Will start routes depending which one is marked as false
    /// 
    /// Then the rooutes will be get from SetFinalRoutes()
    /// 
    /// Here the route is just initiated. Is an Async method call
    /// 
    /// Will return true if at least one was started 
    /// </summary>
    bool StartRoutes()
    {
        bool res = false;

        if (_person.Home != null && _person.Work != null && !workRouteStart)
        {
            //miust be clear in case a new route must be created since in SetFinalRoute() need to be empty
            //to meet if()
            _workRoute.CheckPoints.Clear();
            DefineWorkRoute();
            res = true;
        }
        if (_person.Home != null && _person.FoodSource != null && !foodRouteStart)
        {
            _foodRoute.CheckPoints.Clear();
            DefineFoodSourceRoute();
            res = true;
        }

        if (_person.Home != null && !idleRouteStart)
        {
            _idleRoute.CheckPoints.Clear();
            DefineIdleRoute();
            res = true;
        }

        if (_person.Home != null && _person.Religion != null && !religionRouteStart)
        {
            _religionRoute.CheckPoints.Clear();
            DefineReligionRoute();
            res = true;
        }
        if (_person.Home != null && _person.Chill != null && !chillRouteStart)
        {
            _chillRoute.CheckPoints.Clear();
            DefineChillRoute();
            res = true;
        }

        return res;
    }

    void UpdateRouters()
    {
        _routerFood.Update();
        _routerIdle.Update();
        _routerWork.Update();
        _routerReligion.Update();
        _routerChill.Update();
    }

    #region Brain Checker - MAIN REGION OF THIS CLASS

    private List<string> orderedFoodSources = new List<string>();
    HPers[] allPlaces = new HPers[] { HPers.Home, HPers.Work, HPers.FoodSource, HPers.Religion, HPers.Chill };
    
    public HPers[] AllPlaces
    {
        get { return allPlaces; }
        set { allPlaces = value; }
    }

    /// <summary>
    /// MAIN METHOD ON THIS CLASS
    /// Is called so often from a courutine in the Person.cs
    /// </summary>
    public void CheckConditions()
    {
        MajorAge.CheckIfMajorityRecentlyReached();
        //CheckIfABridgeIUseIsMarked();
        //CheckOnGenOldKeys();//here just bz it kill the .exe with 250 people if i called in the update()
        MoveToNewHome.CheckOnOldKeysList();

        bool ifIsgettingOutOfBuild = CheckIfAnyOfMyBuildsWillBeDestroyAndGetOut();
        if (ifIsgettingOutOfBuild) { return; }


        //CheckOnTheQueues();


        if (PersonController.UnivCounter == -1) { return; }

        if (!PersonPot.Control.PeopleHasCheck(_person.MyId)
            && (IAmHomeNow() || IJustSpawn())
            )
        {
            //PersonPot.Control.CheckPeopleIn(_person.MyId);

            CheckAround(BuildingPot.Control.IsNewHouseSpace, BuildingPot.Control.AreNewWorkPos,
                BuildingPot.Control.IsfoodSourceChange, BuildingPot.Control.IsNewReligion,
                BuildingPot.Control.IsNewChill);
        }
    }


    /// <summary>
    /// Teel u if person has old key from buidings he used to be on 
    /// </summary>
    /// <returns></returns>
    bool IHaveOldKeys()
    {
        if (_generalOldKeysList.Count > 0 || MoveToNewHome.HomeOldKeysList.Count > 0)
        {
            return true;
        }
        return false;
    }

    private void CheckOnGenOldKeys()
    {
        if (IHaveOldKeys())
        {
            for (int i = 0; i < _generalOldKeysList.Count; i++)
            {
                RemoveFromPeopleDict(_generalOldKeysList[i]);
            }
        }
    }

    void RemoveFromPeopleDict(string key)
    {
        var build = GetBuildingFromKey(key);
        if (build!=null)
        {
            build.PeopleDict.Remove(_person.MyId);
            BuildingPot.Control.Registro.ResaveOnRegistro(build.MyId);

        }
    }

    /// <summary>
    /// If one of the buildings he is will be destroy then he need to get out of there 
    /// </summary>
    private bool CheckIfAnyOfMyBuildsWillBeDestroyAndGetOut()
    {
        bool res = false;
        for (int i = 0; i < allPlaces.Length; i++)
        {
            //SetCurrents(allPlaces[i]);
            if (ReturnCurrStructure(allPlaces[i]) == null) { return false; }

            if (ReturnCurrStructure(allPlaces[i]).Instruction == H.WillBeDestroy)
            {
                GetOutOfBuild(allPlaces[i]);
                res = true;
            }
            //UpdateCurrent(allPlaces[i]);
        }
        return res;
    }

    #region Checking on Queues
    /// <summary>
    /// Check to see what is on the queues
    /// </summary>
    private void CheckOnTheQueues()
    {
        if (!_isAllSet || PersonPot.Control.Queues.IsEmpty())
        {
            return;
        }

        //bz IAmHomeNow() sometimes is too short of a time
        //with 3x is fine. just added LocatedAtHomeNow() so extends the changces of getting call
        if (IAmHomeNow() || LocatedAtHomeNow())
        {
            CheckQueuesLoop();
        }
    }

    //Rect[] _debugRect = new Rect[5];
    //public Rect[] DebugRect
    //{
    //    get { return _debugRect; }
    //    set { _debugRect = value; }
    //}

    private DateTime askInWork = new DateTime();
    private DateTime askInWorkBack = new DateTime();
    /// <summary>
    /// Check thru the queues for each route AreaRecy to se if collide with any Anchors on the queues
    /// </summary>
    void CheckQueuesLoop()
    {
        List<HPers> collisionsOn = new List<HPers>();

        if (_person.ProfessionProp!=null)
        {
            if (_person.ProfessionProp.Router1 != null && _person.ProfessionProp.Router1.TheRoute != null
                && PersonPot.Control.Queues.ContainAnyBuild(_person.ProfessionProp.Router1.TheRoute, _person.MyId))
            {
                collisionsOn.Add(HPers.InWork);
                askInWork = PersonPot.Control.Queues.GetLastCollisionTime();
            }
            if (_person.ProfessionProp.RouterBack != null && _person.ProfessionProp.RouterBack.TheRoute != null
            && PersonPot.Control.Queues.ContainAnyBuild(_person.ProfessionProp.RouterBack.TheRoute, _person.MyId))
            {
                collisionsOn.Add(HPers.InWorkBack);
                askInWorkBack = PersonPot.Control.Queues.GetLastCollisionTime();
            }
        }



        //bugg was doing if, else if below... where should be if, if , if
        if (PersonPot.Control.Queues.ContainAnyBuild(_workRoute, _person.MyId))
        {
            collisionsOn.Add(HPers.Work);
            askWork = PersonPot.Control.Queues.GetLastCollisionTime();
        }
        if (PersonPot.Control.Queues.ContainAnyBuild(_foodRoute, _person.MyId))
        {
            collisionsOn.Add(HPers.FoodSource);
            askFood = PersonPot.Control.Queues.GetLastCollisionTime();
        }

        if (PersonPot.Control.Queues.ContainAnyBuild(_idleRoute, _person.MyId))
        {
            collisionsOn.Add(HPers.IdleSpot);
            askIdle = PersonPot.Control.Queues.GetLastCollisionTime();
        }




        if (PersonPot.Control.Queues.ContainAnyBuild(_religionRoute, _person.MyId))
        {
            collisionsOn.Add(HPers.Religion);
            askReligion = PersonPot.Control.Queues.GetLastCollisionTime();
        }
        if (PersonPot.Control.Queues.ContainAnyBuild(_chillRoute, _person.MyId, HPers.Chill))
        {
            collisionsOn.Add(HPers.Chill);
            askChill = PersonPot.Control.Queues.GetLastCollisionTime();
        }

        RedoRoutes(collisionsOn);
        collisionsOn.Clear();

        //_debugRect = new Rect[] { _workRoute.AreaRect, _foodRoute.AreaRect, _religionRoute.AreaRect, _chillRoute.AreaRect };
    }

    /// <summary>
    /// Is called for redo all the routes that were found collided on queues
    /// </summary>
    /// <param name="collisionsOn">All the routes were collided with on Queues</param>
    void RedoRoutes(List<HPers> collisionsOn)
    {
        for (int i = 0; i < collisionsOn.Count; i++)
        {
            var which = collisionsOn[i];

           ////Debug.Log("Collided a builing with route:" + which + " on person:" + _person.MyId);
            if (which == HPers.Work)
            {
                //GameScene.print("Redo Work");
                workRouteStart = false;
            }
            if (which == HPers.FoodSource)
            {
                //GameScene.print("Redo Food");
                foodRouteStart = false;
            }
            if (which == HPers.IdleSpot)
            {
                //GameScene.print("Redo Food");
                idleRouteStart = false;
            }
            if (which == HPers.Religion)
            {
                //GameScene.print("Redo Religion");
                religionRouteStart = false;
            }
            if (which == HPers.Chill)
            {
                //GameScene.print("Redo Chill");
                chillRouteStart = false;
            }
        }

        if (collisionsOn.Contains(HPers.InWork))
        {
            PersonPot.Control.RoutesCache1.RemoveRoute(_person.ProfessionProp.Router1.TheRoute, askInWork);
            //RedoProfession();
        } 
        if (collisionsOn.Contains(HPers.InWorkBack))
        {
            PersonPot.Control.RoutesCache1.RemoveRoute(_person.ProfessionProp.RouterBack.TheRoute, askInWorkBack);
            //RedoProfession();
        }
    }
    #endregion

    /// <summary>
    /// Used to get a NewBorn Child going 
    /// </summary>
    public void SetNewHouseFound()
    {
        //so can reroutre and stuff
        //CheckMeOnSystemNow();
        AddMeToSystemWaitingList();
        CheckAround(false, false, false, false, false, true);
    }

    //void NewBornStuff()
    //{
    //    //new borns wont have this set yet 
    //    if (_isAllSet || !string.IsNullOrEmpty(_person.IsBooked))
    //    {
    //        return;
    //    }

       
    //}

    /// <summary>
    /// This method is the one that will look for new buildings if the respective flag is true 
    /// </summary>
    void CheckAround(bool checkHouse, bool checkWork, bool checkFood, bool checkReligion, bool checkChill, bool newHouseFound = false)
    {
        var tempKey = "";
        //bool newHouseFound = false;//if new house is found will allow to check for everything around him
        if (checkHouse)
        {
            if (_person.Home != null)
            {
                tempKey = _person.Home.MyId;
            }
            CheckHome();
            if (_person.Home != null && tempKey != _person.Home.MyId)
            {
                newHouseFound = true;
            }
        }

        if (_person.Home != null)
        {
            if ((checkWork || newHouseFound))
            {
                CheckWork();
            }
            if ((checkFood || newHouseFound))
            {
                CheckFood();
            }
            if (checkReligion || newHouseFound)
            {
                CheckClosestBuildRoutine(HPers.Religion);
            }
            if (checkChill || newHouseFound)
            {
                CheckClosestBuildRoutine(HPers.Chill);
            }
        }
    }

    #region Check Closest Build
    //Structure currStructure;
    //private List<string> currListOfBuild;

    /// <summary>
    /// Checks, and defined the colsest building of a type
    /// </summary>
    private void CheckClosestBuildRoutine(HPers which)
    {
        //SetCurrents(which);

        //isallset is here so will check if closest building exist
        if (!ItHasOneAlready(which) || _isAllSet || ReturnCurrStructure(which).Instruction == H.WillBeDestroy)
        {
            DefineClosestBuild(which);
            //UpdateCurrent(which);
        }
        UnivCounter(which);
    }

    /// <summary>
    /// Will return true if Person already has a Building of type 'which' param
    /// </summary>
    private bool ItHasOneAlready(HPers which)
    {
        if (which == HPers.Work && _person.Work != null)
        {
            return true;
        }
        if (which == HPers.Religion && _person.Religion != null)
        {
            return true;
        }
        if (which == HPers.Chill && _person.Chill != null)
        {
            return true;
        }
        return false;
    }

    


    public Structure ReturnCurrStructure(HPers which)
    {
        if (which == HPers.Home)
        {
            return _person.Home;
        }
        else if (which == HPers.FoodSource)
        {
            return _person.FoodSource;
        }
        else if (which == HPers.Work)
        {
            return _person.Work;
        }
        else if (which == HPers.Religion)
        {
            return _person.Religion;
        }
        else if (which == HPers.Chill)
        {
            return _person.Chill;
        }
        return null;
    }

    private Structure AssignCurrStructure(HPers which, Structure st)
    {
        if (which == HPers.Home)
        {
            _person.Home = st;
            return _person.Home;
        }
        else if (which == HPers.FoodSource)
        {
            _person.FoodSource = st;

            return _person.FoodSource;
        }
        else if (which == HPers.Work)
        {
            _person.Work = st;

            return _person.Work;
        }
        else if (which == HPers.Religion)
        {
            _person.Religion = st;

            return _person.Religion;
        }
        else if (which == HPers.Chill)
        {
            _person.Chill = st;

            return _person.Chill;
        }
        return null;
    }


    List<string> ReturnCurrListOfBuilds(HPers which)
    {
        if (which == HPers.Work)
        {
            return BuildingPot.Control.WorkOpenPos;
        }
        else if (which == HPers.Religion)
        {
            return BuildingPot.Control.ReligiousBuilds;
        }
        else if (which == HPers.Chill)
        {
            return BuildingPot.Control.ChillBuilds;
        }
        return  new List<string>();
    }



    /// <summary>
    /// Define the closest build of a type. Will defined in the 'currStructure'
    /// </summary>
    private void DefineClosestBuild(HPers which)
    {
        int size = ReturnCurrListOfBuilds(which).Count;
        List<VectorM> loc = new List<VectorM>();

        for (int i = 0; i < size; i++)
        {
            //to address if building was deleted and not updated on the list 
            string key = ReturnCurrListOfBuilds(which)[i];
            Structure building = GetStructureFromKey(key);

            if (!_blackList.Contains(key))
            {
                Vector3 pos = BuildingPot.Control.Registro.AllBuilding[key].transform.position;
                loc.Add(new VectorM(pos, _person.Home.transform.position, key));
            }
        }
        loc = ReturnOrderedByDistance(_person.Home.transform.position, loc);
        int index = 0;

        if (loc.Count > 0)
        {
            while (BuildingPot.Control.Registro.AllBuilding[loc[index].LocMyId].Instruction == H.WillBeDestroy)
            {
                index++;
                //this is to avoid exception when many buildings are on the blacklist
                if (index > loc.Count - 1)
                {
                    index = -1;
                    break;
                }
            }
        }

        DefineClosestBuildTail(loc, index, which);
    }

    /// <summary>
    /// Created for modularity
    /// </summary>
    void DefineClosestBuildTail(List<VectorM> loc, int index, HPers which)
    {
        string closestKey = "";
        if (index != -1 && loc.Count > 0)
        {
            closestKey = loc[index].LocMyId;
        }

        //b4 is reassigned
        AddToNewBuildRemoveFromOld(ReturnCurrStructure(which), closestKey);

        if (closestKey != "")
        {
            AssignCurrStructure(which,(Structure)BuildingPot.Control.Registro.AllBuilding[closestKey] )  ;
        }
        else AssignCurrStructure(which,null);
    }
    #endregion

    /// <summary>
    /// Starting from the Home will find all the food sources ordered by distance to home
    /// Will keep only the first 5 closest to home 
    /// </summary>
    void CheckFood()
    {
        //is allset is here so will check if closest building exist
        CheckFoodAction();
        UnivCounter(HPers.FoodSource);
    }


    void CheckFoodAction()
    {
        if (_person.FoodSource == null || _person.FoodSource.Instruction == H.WillBeDestroy)
        {
            UpdateOrderedFoodSources();

            if (orderedFoodSources.Count > 0)
            {
                //b4 is reassigned
                AddToNewBuildRemoveFromOld(_person.FoodSource, orderedFoodSources[0]);
                _person.FoodSource = (Structure)BuildingPot.Control.Registro.AllBuilding[orderedFoodSources[0]];
            }
            else
            {
                AddToNewBuildRemoveFromOld(_person.FoodSource, "");
                _person.FoodSource = null;
            }
        }
    }

    //todo GC
    /// <summary>
    /// I made i diff method for modularity and for be called from Checking on food availabait
    /// in case is diff from wht the 'BuilderPot.Control.FoodSources' is then we need to update
    /// </summary>
    void UpdateOrderedFoodSources()
    {
        orderedFoodSources.Clear();
        int size = BuildingPot.Control.FoodSources.Count;
        List<VectorM> loc = new List<VectorM>();

        for (int i = 0; i < size; i++)
        {
            string key = BuildingPot.Control.FoodSources[i];

            if (BuildingPot.Control.Registro.AllBuilding[key].Instruction == H.None && !_blackList.Contains(key))
            {
                Vector3 pos = BuildingPot.Control.Registro.AllBuilding[key].transform.position;
                loc.Add(new VectorM(pos, _person.Home.transform.position, key));
            }
        }
        loc = ReturnOrderedByDistance(_person.Home.transform.position, loc);

        for (int i = 0; i < loc.Count; i++)
        {
            //so it doesnt add to foodSources Strucutres that will be taken down 
            if (GetStructureFromKey(loc[i].LocMyId).Instruction == H.None)
            {
                orderedFoodSources.Add(loc[i].LocMyId);
                //GameScene. print(foodSources[i] + ".foodsrc#:" + i + ".myId:" + _person.MyId);
            }
        }
    }

    void CheckWork()
    {
        //isallset is here so will check if closest building exist
        if (!ItHasOneAlready(HPers.Work) || _person.Work.Instruction == H.WillBeDestroy)
        {
            var newWork = _jobManager.GiveWork(_person);
            _person.Work = newWork;
            WorkPositionsUpdate();
        }
        UnivCounter(HPers.Work);
    }

    /// <summary>
    /// Will find the house for this person. Depending in the person Gender and Age will fit it in a house
    /// </summary>
    void CheckHome()
    {
        //to avoid the jump of house when doenst have got to the first one yet 
        if (_person.Body.MovingNow)
        {
            return;
        }

        //set to check for a new home 
        if (_person.Home != null && (_person.Home.Instruction == H.WillBeDestroy || !string.IsNullOrEmpty(_person.IsBooked)))
        {
            //does only need to check on AllBuilding List... simple on the case if is a new home it was removed
            //already from tht list 
            CheckHomeLoop();
            return;
        }

        //if is not null and is not shack then dont need to  call CheckHomeLoop()
        if (_person.Home != null)
        {
            bool thereIsABetterHome = _realtor.PublicIsABetterHome(_person);

            if (!thereIsABetterHome)
            {
                UnivCounter(HPers.Home);
                return;
            }
        }
        CheckHomeLoop();
    }

  
    Realtor _realtor = new Realtor();
    public Realtor Realtor1
    {
        get { return _realtor; }
        set { _realtor = value; }
    }


    /// <summary>
    /// Looks thru all the 'BuilderPot.Control.HousesWithSpace' items to see if this person can find
    /// a suitable Home
    /// </summary>
    void CheckHomeLoop()
    {
        bool thereIsABetterHome = _realtor.PublicIsABetterHome(_person);

        //shack builders can not look into this. Othr wise they will stay on Limbo once better home found 
        if (thereIsABetterHome || !string.IsNullOrEmpty(_person.IsBooked))
        {
            var oldHomeP = PullOldHome();
            var s = _realtor.GiveMeTheBetterHome(_person);

            if (s != null)
            {
                //bz is has more than zero created the route already. is just getting the home here 
                if (oldHomeP != null && MoveToNewHome.RouteToNewHome.CheckPoints.Count==0)
                {
                    MoveToNewHome.AddToHomeOldKeysList(oldHomeP.MyId);
                    MoveToNewHome.OldHomeKey = "";

                    if (MoveToNewHome.RouteToNewHome.CheckPoints.Count > 0)
                    {
                       ////Debug.Log(_person.MyId + " clear RouteToNewHome");
                    }
                    MoveToNewHome.RouteToNewHome.CheckPoints.Clear();
                }
                else
                {  
                   ////Debug.Log(_person.MyId + " didNot clear RouteToNewHome");
                }


                AddToPeopleDict(s.MyId);
                _person.Home = s;
                //needs to be call here in case this person is the one ocpied the slot 
                PersonPot.Control.CleanHomeLessSlot(_person.MyId);



                _isIdleHomeNow = true;
                MoveToNewHome.CheckOnOldKeysList();
            }
        }
        UnivCounter(HPers.Home);
    }

    public Structure PullOldHome()
    {
        return GetStructureFromKey(oldHome);
    }

    /// <summary>
    /// Will check if old home has at least avail for one family. If so will be added to the List in BuidingControlelr
    /// </summary>
    /// <param name="oldHomeP"></param>
    void AddOldHomeToAvailHomeIfHasSpace(Structure oldHomeP)
    {
        if (oldHomeP == null)
        {
            return;
        }

        var oldHomeIsEmptyOrNull = IsTheFamilyThatICameFromEmpty(oldHomeP);


        if (oldHomeP.IsALeastOneFamilyEmpty() && oldHomeIsEmptyOrNull)
        {
            BuildingPot.Control.AddToHousesWithSpace(oldHomeP.MyId);

            PersonPot.Control.RestartController();
          ////Debug.Log("Home added :" + oldHomeP.MyId);
        }
    }

    /// <summary>
    /// Will say if the family u had in the old home was emptied or not 
    /// </summary>
    /// <returns></returns>
    bool IsTheFamilyThatICameFromEmpty(Structure oldHomeP)
    {

        //the var family on old home needs to be make it virgin 
        var FamilyOnOldHome = oldHomeP.FindMyFamilyChecksFamID(_person);

        if (FamilyOnOldHome == null)
        {
            return true;
        }
        return FamilyOnOldHome.IsFamilyEmpty();
    }

    /// <summary>
    /// This methd is what stops the person checking again and again.. bz this eventally
    /// Will make PersonController.UnivCounter = -1
    /// </summary>
    /// <param name="from">From where was called</param>
    void UnivCounter(HPers from)
    {
        //this is only needed to be working if the PersonController.UnivCounter was externally changed to 0
        //this is to correct a bugg in where since many methods are callling this. Will make the value 0 unintentionally
        if (PersonController.UnivCounter == -1) { return; }

        if (PersonPot.Control.IsPeopleCheckFull())
        {
            if (from == HPers.FoodSource)
            {
                BuildingPot.Control.IsfoodSourceChange = false;
                //print("Not checking for new food src anymore");
            }
            else if (from == HPers.Work)
            {
                BuildingPot.Control.AreNewWorkPos = false;
                //print("Not checking for new work anymore");
            }
            else if (from == HPers.Home)
            {
                BuildingPot.Control.IsNewHouseSpace = false;
                //print("Not checking for new houses anymore");
            }
            else if (from == HPers.Religion)
            {
                BuildingPot.Control.IsNewReligion = false;
                //print("Not checking for new relgio anymore");
            }
            else if (from == HPers.Chill)
            {
                BuildingPot.Control.IsNewChill = false;
                //print("Not checking for new Chill anymore");
            }
            PersonPot.Control.ClearPeopleCheck();

            //called here bz here I know the Checked on PersonPot was full and clear 
            //CheckIfShacksAreNeed();

            if (!BuildingPot.Control.IsfoodSourceChange && !BuildingPot.Control.AreNewWorkPos &&
                !BuildingPot.Control.IsNewHouseSpace && !BuildingPot.Control.IsNewReligion &&
                !BuildingPot.Control.IsNewChill)
            {
                PersonController.UnivCounter = -1;
               //GameScene.print("Univ at -1 Done Checking!!!!");
            }
        }
    }

    private static float MAXDISTANCE = 5000f;//the max distance a person will go to find a building //50
    /// <summary>
    /// Return an ordered list of places ordered by distance by stone . If the place element is farther then 
    /// MAXDISTANCE wont be added to the final result 
    /// </summary>
    /// <param name="stone">The initial point from where needs to be ordered</param>
    /// <param name="places">Places to order</param>
    static public List<VectorM> ReturnOrderedByDistance(Vector3 stone, List<VectorM> places)
    {
        var anchorOrdered = new List<VectorM>();
        for (int i = 0; i < places.Count; i++)
        {
            if (places[i] != null)
            {
                float distance = Vector3.Distance(places[i].Point, stone);

                if (distance < MAXDISTANCE)
                {
                    anchorOrdered.Add(new VectorM(places[i].Point, stone, places[i].LocMyId));
                }
            }
        }
        return anchorOrdered.OrderBy(a => a.Distance).ToList();
    }

    /// <summary>
    /// Will return the closest point to 'Stone'
    /// </summary>
    /// <param name="stone"></param>
    /// <param name="places"></param>
    /// <returns></returns>
    static public Vector3 ReturnClosestVector3(Vector3 stone, List<Vector3> places)
    {
        List<VectorM> vectorMs = new List<VectorM>();
        for (int i = 0; i < places.Count; i++)
        {
            vectorMs.Add(new VectorM(places[i], stone, ""));
        }

        var orderd = ReturnOrderedByDistance(stone, vectorMs);
        return orderd[0].Point;
    }



    #endregion

    #region Get Out Of Build and Search Again

    /// <summary>
    /// Gets out of building
    /// If was a house is gonna be added to 'old keys homes' otherwise
    /// will be searchAgain and 'who' will be set to the param 'whichType'
    /// so in the searchAgain only that one will be searched for 
    /// </summary>
    /// <param name="whichType">The type of building</param>
    public void GetOutOfBuild(HPers whichType)
    {
        //SetCurrents(whichType);
        if (whichType == HPers.Home)
        {
            MoveToNewHome.AddToHomeOldKeysList();
        }
        else
        {
            searchAgain = true;
            who = whichType;
        }
        //UpdateCurrent(whichType);
    }


    private bool searchAgain;
    //who activiated the searchAgain . created to address bugg where finds everyone 
    //eveytime is called 
    private HPers who;
    public HPers Who
    {
        get { return who; }
        set { who = value; }
    }
    /// <summary>
    /// Will search for the specific type of building on the var 'who'.
    /// 
    /// Will search again if is at home and searchAgain true... or if 'now' is true
    /// </summary>
    public void SearchAgain(bool now = false)
    {
        if ((IAmHomeNow() && searchAgain) || now)
        {
            SearchAgainAction();
        }
    }

    /// <summary>
    /// The action of search again
    /// 
    /// This was create for modularity and can be called from another method too if is ready
    /// </summary>
    void SearchAgainAction()
    {
        //set the flag
        SetFlag(who, true);

        CheckAround(_isHouse, _isWork, _isfood, _isReligion, _isChill);
        searchAgain = false;

        //to clear the flag
        SetFlag(who, false);
        who = HPers.None;
    }

    private bool _isfood;
    private bool _isWork;
    private bool _isHouse;
    private bool _isReligion;
    private bool _isChill;
    /// <summary>
    /// Set the aboves vars to param 'val'
    /// </summary>
    public void SetFlag(HPers which, bool val)
    {
        if (which == HPers.FoodSource)
        {
            _isfood = val;
        }
        else if (which == HPers.Work)
        {
            _isWork = val;
        }
        else if (which == HPers.Home)
        {
            _isHouse = val;
        }
        else if (which == HPers.Religion)
        {
            _isReligion = val;
        }
        else if (which == HPers.Chill)
        {
            _isChill = val;
        }
    }

    #endregion

    #region Slow Update: Here will find checks that are checked at a slower pace: Food Source Availability Check

    private bool slowCheckUp;

    /// <summary>
    /// Flag bool so we can check if the FoodSource was depleted
    /// </summary>
    internal void SlowCheckUp() { slowCheckUp = true; }

    /// <summary>
    /// Checks if the FoodSource was depleted
    /// </summary>
    void ExecuteSlowCheckUp()
    {
        if (slowCheckUp && IAmHomeNow())
        {
            CheckOnFoodSourceAvail();
            slowCheckUp = false;

            HandleNewJobSearch();
            UpdateBlackListing();

            CheckIfABridgeIUseIsMarked();
        }
    }

    JobManager _jobManager = new JobManager();

    void HandleNewJobSearch()
    {
        var newWork = _jobManager.ThereIsABetterJob(_person);
        if (newWork == null)
        {
           ////Debug.Log("New work should not be null");
        }
        _person.Work = newWork;
        WorkPositionsUpdate();
    }

    private string oldJob="";
    private void WorkPositionsUpdate()
    {
        var currWork = oldJob;

        if (_person.Work != null)
        {
            currWork = _person.Work.MyId;
        }
        //in case got a null new work
        else currWork = "";

        if (currWork != oldJob)
        {
            RemoveFromPeopleDict(oldJob);

            AddToPeopleDict(currWork);
            RemoveAndAddPositionsToJob();

            //when is a game loaded and changed work need to mannually restart Controller to see it 
            //forcing it here 
            PersonPot.Control.RestartControllerForPerson(_person.MyId);
        }
        //in case we have a job
        oldJob = currWork;
    }


    private int emptyCount;
    /// <summary>
    /// Checks the availability of food on the closest 5 food sources.
    /// If the closest doesnt have will keep looping until finds the one that has
    /// food
    /// 
    /// Then will make that your main food source
    /// </summary>
    void CheckOnFoodSourceAvail()
    {
        if (orderedFoodSources.Count == 0 || !IAmHomeNow()) { return; }
        //means that a Food Src was destroyed. Then have to update 'orderedFoodSources 
        UpdateOrderedFoodSources();

        emptyCount = 0;
        for (int i = 0; i < orderedFoodSources.Count; i++)
        {
            Structure s = GetStructureFromKey(orderedFoodSources[i]);
            //will assign the first one is not empty... 
            //now if we assign a diff one from the current then the Brain will trace route to new FoodSrc
            if (s.Inventory.FoodCatItems.Count > 0)
            {
                //so the buildings PeopleDict gets updated
                AddToNewBuildRemoveFromOld(_person.FoodSource, s.MyId);
                _person.FoodSource = s;
                PersonPot.Control.RestartControllerForPerson(_person.MyId);
                return;
            }
            emptyCount++;
            AreTheyAllEmpty(emptyCount);
        }
    }

    /// <summary>
    /// If all food sources are empty the closest is the one assign to it 
    /// </summary>
    /// <param name="index"></param>
    void AreTheyAllEmpty(int index)
    {
        if (index == BuildingPot.Control.FoodSources.Count)
        {
            Structure s = GetStructureFromKey(orderedFoodSources[0]);
            _person.FoodSource = s;
        }
    }
    #endregion

    #region Bridges Piece of Mind

    Dictionary<string,string> brigdesKeyRoutes = new Dictionary<string, string>();//routes that have a brdige key on it  
    /// <summary>
    /// Will be checking so if one Route has a brdige and was marked will Try to black list that building on the route
    /// </summary>
    void CheckIfABridgeIUseIsMarked()
    {
        if (!IAmHomeNow() || !goMindState) { return; }

        for (int i = 0; i < brigdesKeyRoutes.Count; i++)
        {
            var bridgeKey = brigdesKeyRoutes.ElementAt(i).Value;

            if (String.IsNullOrEmpty(bridgeKey))
            { continue; }

            var bridge = BuildingPot.Control.Registro.AllBuilding[bridgeKey];

            if (bridge.Instruction == H.WillBeDestroy)
            {
                var destinyKey = brigdesKeyRoutes.ElementAt(i).Key;
                BridgeMarkedAction(destinyKey, bridgeKey);
            }
        }
    }

    /// <summary>
    /// Will create a list with all  the routes that have brdiges  if is any
    /// </summary>
    private void UpdateBridgeKeyListMarked()
    {
        if (!string.IsNullOrEmpty(_workRoute.BridgeKey) && !string.IsNullOrEmpty(_workRoute.DestinyKey ))
        {
            brigdesKeyRoutes.Add(_workRoute.DestinyKey, _workRoute.BridgeKey);
        }
        if (!string.IsNullOrEmpty(_foodRoute.BridgeKey) && !string.IsNullOrEmpty(_foodRoute.DestinyKey))
        {
            brigdesKeyRoutes.Add(_foodRoute.DestinyKey,_foodRoute.BridgeKey);
        }
        if (!string.IsNullOrEmpty(_idleRoute.BridgeKey ) && !string.IsNullOrEmpty(_idleRoute.DestinyKey ))
        {
            brigdesKeyRoutes.Add(_idleRoute.DestinyKey, _idleRoute.BridgeKey);
        }
        if (!string.IsNullOrEmpty(_religionRoute.BridgeKey)  && !string.IsNullOrEmpty(_religionRoute.DestinyKey))
        {
            brigdesKeyRoutes.Add(_religionRoute.DestinyKey, _religionRoute.BridgeKey);
        }
        if (!string.IsNullOrEmpty(_chillRoute.BridgeKey ) && !string.IsNullOrEmpty(_chillRoute.DestinyKey ))
        {
            brigdesKeyRoutes.Add(_chillRoute.DestinyKey, _chillRoute.BridgeKey);
        }
    }

    /// <summary>
    /// If the param 'newKeyP' is not contain in the list and is not empty will be added to list 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="newKeyP"></param>
    /// <returns></returns>
    List<string> AddToList(List<string> list, string newKeyP)
    {
        if (!list.Contains(newKeyP) && !String.IsNullOrEmpty(newKeyP))
        {
            list.Add(newKeyP);
        }
        return list;
    }

    /// <summary>
    /// Will make the strcuture null so we reRoute again there to see if we can go thru a diff
    /// brdige or may have to just blacklist that buildign 
    /// 
    /// Then will call ChecKAround depending on which one was pass as 'Key'
    /// 
    /// If u use the 'buildFunc' param then will make that Structure in the person Null and will search again 
    /// for that kind of Structure
    /// 
    /// Will make the old key = "" so it clicks on 'CheckIfABuildWasChange()' and reRoute this is to address for ex:
    /// the case in where 2 bridges exist and only 1 work place we kill one brdige and we found the same
    /// place again ... it will be the same key value so and wont start to create new routes
    /// that why here i mke old value = "" so it will start new route even if the same key
    /// </summary>
    void MakeStructureNull(string key)
    {
        var build = GetStructureFromKey(key);
        if (build == null)
        {
            return;
        }
        //the function this  building has 
        var buildFunc = BuildingController.ReturnBuildingFunction(build.HType);


      ////Debug.Log("MakeStructureNull");
        var checkHome = false;
        var checkWork = false;
        var checkFood = false;
        var checkReligion = false;
        var checkChill = false;

        //todo remove person from People Dict of the place will be made null
        if ((buildFunc == HPers.Home ))
        {
            if (_person.Home.BookedHome1 != null)
            {
                _person.Home.BookedHome1.ClearBooking();
            }

            _person.Home = null;
            checkHome = true;
            oldHome = "";
        }
        else if ((buildFunc == HPers.Work ))
        {
            RemoveAndAddPositionsToJob();

            _person.Work = null;
            _person.ProfessionProp = new Profession();//so it cleans profession

            checkWork = true;
            oldWork = "";

        }
        else if ((buildFunc == HPers.FoodSource ))
        {
            _person.FoodSource = null;
            checkFood = true;
            oldFoodSrc = "";
        }
        else if ((buildFunc == HPers.Religion ))
        {
            _person.Religion = null;
            checkReligion = true;
            oldReligion = "";
        }
        else if ((buildFunc == HPers.Chill ))
        {
            _person.Chill = null;
            checkChill = true;
            oldChill = "";
        }
        CheckAround(checkHome, checkWork, checkFood, checkReligion, checkChill);
    }
    #endregion

    #region BlackListing
    //contains all buildings I can currently reach. Since all builignds in a piece of land should be reacheable
    //this list will clear if one bridge is built or moved to a new hose
    //this is not saved or loaded
    List<string> _blackList = new List<string>();

    //the amt of bridges 
    //used to know if a new bridge was built.
    //if so blackList can be clear 
    private int bridgesCount;

    public List<string> BlackList
    {
        get{return _blackList;}
        set{_blackList = value;}
    }

    /// <summary>
    /// Will handle the changes of BlackListing
    /// if new bridge is built willclear blackList
    /// </summary>
    private void UpdateBlackListing()
    {
        var newBridges = HowManyBuiltBridges();
        if (bridgesCount != newBridges)
        {
            bridgesCount = newBridges;


            CheckIfClearBlackList();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p"></param>
    /// <param name="checkedOnBrdges">Can only be marked as false when is asking from CheckConditions bz
    /// I have it really check yet how many brdiges are 
    /// 
    /// If is callign from BridgesRouter then should look at the exception bz if i called from there
    /// i could not find any bridge</param>
    internal void BlackListBuild(string p, string routeKey)
    {
        if (_blackList.Contains(p))
        {
            return;
        }

        //if (WasABuildFromThisRouteBlacked(routeKey))
        //{
        //    return;
        //}

        MoveToNewHome.RemovePeopleDict(p);
        Debug.Log("Blaclisted:"+p +" ."+_person.MyId);

        //the route key is added so we dont blaclist 2 buildings of a same route
        //only the 1st one need to be blacklisted. this applys for BridgeRouting 
        _blackList = AddToList(_blackList, p);
        
        BridgeMarkedAction(p);
        PersonPot.Control.WorkersRoutingQueue.RemoveMeFromSystem(_person.MyId);
    }

    /// <summary>
    /// Will tell if any of the two keys passed are contained in BlackList
    /// </summary>
    /// <param name="destKey"></param>
    /// <param name="oriKey"></param>
    /// <returns></returns>
    public bool IsContainOnBlackList(string destKey, string oriKey)
    {
        if (BlackList.Contains(destKey) || BlackList.Contains(oriKey))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// If a building of this route was blacklisted we dont have dto blaclist the 
    /// the second one too.
    /// 
    /// this applys for BridgeRouting
    /// </summary>
    /// <returns></returns>
    bool WasABuildFromThisRouteBlacked(string routeKey)
    {
        for (int i = 0; i < _blackList.Count; i++)
        {
            var storedRouteKey = _blackList[i].Split('.')[1];

            if (storedRouteKey == routeKey)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Created to address when is called from brain doesnt need to cjeck if is another bridge or not
    /// 
    /// Just needs to black list everytihng bz means that a brdige is gonna be out
    /// 
    /// It doesnt add to backlisting bz is not needed. I only need to reroute  
    /// </summary>
    void BridgeMarkedAction(string build, string bridge = "")
    {
        //and will add to genOldKeys in case that was used by him b4. Otherwise will be dimiss the key

        AddToList(_generalOldKeysList, build);
        MakeStructureNull(build);
    }

    int HowManyBuiltBridges()
    {
        int res = 0;
        for (int i = 0; i < BuildingPot.Control.Registro.Ways.Count; i++)
        {
            var b = (Trail)BuildingPot.Control.Registro.Ways.ElementAt(i).Value;
            if (b.MyId.Contains(H.Bridge.ToString()) && b.Pieces[0].CurrentStage == 4)
            {
                res++;
            }
        }
        return res;
    }

    /// <summary>
    /// Will clear blacklist if at least one item on it 
    /// </summary>
    void CheckIfClearBlackList()
    {
        if (_blackList.Count > 0)
        {
            ClearEachBlackListedBuilding();
            brigdesKeyRoutes.Clear();
          ////Debug.Log(_person.MyId+" cleared blackList .famID"+_person.FamilyId);
        }
    }

    /// <summary>
    /// Will clear the black list and will make those buildings in this person null so he 
    /// can search again for those kind of places .. just in case the ones blacklisted now are
    /// closer for him 
    /// </summary>
    void ClearEachBlackListedBuilding()
    {
        string[] arr = _blackList.ToArray();
        _blackList.Clear();

        for (int i = 0; i < arr.Length; i++)
        {
            if (BuildingPot.Control.Registro.AllBuilding.ContainsKey(arr[i]))
            {
                MakeStructureNull(arr[i]);
            }
        }
    }
    #endregion

    #region Die Related

    private bool _partido;



    public bool Partido
    {
        get { return _partido; }
        set { _partido = value; }
    }

    public int TimesCall
    {
        get { return _timesCall; }
        set { _timesCall = value; }
    }


    /// <summary>
    /// A person die 
    /// </summary>
    public void Die()
    {
        if (Partido && string.IsNullOrEmpty(_person.IsBooked))
        {
            _person.Body.DestroyAllPersonalObj();
            PersonPot.Control.Queues.PersonDie();
            
            PersonPot.Control.RemoveMeFromSystem(_person.MyId);
            PersonPot.Control.WorkersRoutingQueue.RemoveMeFromSystem(_person.MyId);

            PersonPot.Control.RemovePersonFromPeopleChecked(_person.MyId);
            //people can die anywhere
            if (_person.Home != null)
            {
                RemoveFromOldFamily();
                BuildingPot.Control.AddToHousesWithSpace(_person.Home.MyId);
                PersonPot.Control.RestartController();
            }
            RemoveFromAllPeopleDict();
            Partido = false;
            //so person goes to heaven, and ray is sent from Sky to take him //or angels take him 
            _person.DestroyCool();
            PersonPot.Control.All.Remove(_person);
        }
    }

    void RemoveFromOldFamily()
    {
        var fam = _person.Home.FindFamilyById(_person.FamilyId);

        if (fam == null)
        {
            var i = this;
            throw new Exception("Die():"+_person.MyId+" sp:"+_person.Spouse+
                " bInfo:"+_person.DebugBornInfo+" homeID:"+_person.Home.MyId+" famID:"+_person.FamilyId);
        }

        fam.RemovePersonFromFamily(_person);
        //fam.HandleKids();
    }

    /// <summary>
    /// Will remove the person from all PeoplesDict he might be on . Will call destroy building if is marked or is a shack
    /// </summary>
    void RemoveFromAllPeopleDict()
    {
        List<Structure> all = new List<Structure>(){_person.Home, _person.Work, _person.FoodSource, _person.Religion, _person.Chill};

        for (int i = 0; i < all.Count; i++)
        {
            if (all[i] != null)
            {
                all[i].PeopleDict.Remove(_person.MyId);
                BuildingPot.Control.Registro.ResaveOnRegistro(all[i].MyId);

                DestroyOldBuildIfEmptyOrShack(all[i].MyId);
            }
        }

        if (_person.Work!=null)
        {
            _person.Work.RemovePosition();
        }
    }

#endregion

    internal bool JustSpawned()
    {
        if (_person.Body.Location==HPers.None && _person.Body.GoingTo==HPers.None &&
            _person.Work == null && _person.FoodSource == null && _person.Religion == null &&
            _person.Chill == null)
        {
            return true;
        }
        return false;
    }
}
