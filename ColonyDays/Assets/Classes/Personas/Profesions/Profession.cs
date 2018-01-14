using System;
using System.Collections.Generic;
using UnityEngine;
/*
 * 
 * on Brain.cs, void GoWork() there is interactin with Docker, WheelBarrow, Homer, Forester 
 * 
 * 
 */

public class Profession
{
    protected Person _person;
    //protected static float radius = 50f;//200 f, how far will go to cut a tree 

    protected bool _readyToWork;//says if workers is ready to work 
    protected bool _workingNow;

    protected CryRouteManager _router = new CryRouteManager();
    protected CryRouteManager _routerBack = new CryRouteManager();
    //says if _routerBack is used.RouterBack is a instance of router and bassically means that a different route is used to 
    //go back home. For ex forester use it . As when finish cutting tree will go directrly to FoodSrc and from there Home 
    protected bool _isRouterBackUsed;
    protected bool _routerActive;//it says if in this moment im usign the RouterManager instances functionaliitie 

    protected Vector3 _finRoutePoint;//the final point of the route 
    protected float _moveTowOrigin = 0.275f;

    protected HPers _workerTask = HPers.None;
    protected string _myAnimation;//for a foresrte will be chop wood animation 

    protected List<VectorM> _orderedSites = new List<VectorM>();

    //if is routing will not allow the save to happen. So its not need to same the dummy
    //since everytime a Profession is saved all its Routes are saved already
    //protected Structure dummy;

    //Will execute the action it came to do in code... 
    //for ex will load inventory with Lumber from cutted tree 
    protected bool _executeNow;

    //says the exact moment when the person finished the work in the site 
    protected bool _doneWorkNow;
    protected float _workTime = 4f;//1    //how long will execute the animation of work
    protected Job _profDescription = Job.None;

    //ShackBuilder and Builders
    protected Building _constructing;//need to implement  to be saved and loaded 
    public string ConstructingKey;//need to implement  to be saved and loaded 

    private float _prodXShift = 0;//100 //Wht a person will produce or carry in a shift 

    //The pos to look at while working if is = new Vector3 the pos of the Work will be used then 
    private Vector3 _lookAtWork;

    //WheelBarrowe
    protected Order _order;//the order of a wheelBarrower
    protected string _sourceBuildKey;//from where taking the load 
    protected string _destinyBuildKey;//where taking load 

    protected Structure _destinyBuild;
    protected Structure _sourceBuild;

    //used for forester
    private string _stillElementID;

    //indicates that this intances of Profession was loaded from file 
    //so never has to be Init() bz all values were loaded 
    protected bool _wasLoaded;

    public Job ProfDescription
    {
        get { return _profDescription; }
        set { _profDescription = value; }
    }

    public bool ReadyToWork
    {
        get { return _readyToWork; }
        set { _readyToWork = value; }
    }

    public bool WorkingNow
    {
        get { return _workingNow; }
        set { _workingNow = value; }
    }

    public bool IsRouterBackUsed
    {
        get { return _isRouterBackUsed; }
        set { _isRouterBackUsed = value; }
    }

    public Vector3 FinRoutePoint
    {
        get { return _finRoutePoint; }
        set { _finRoutePoint = value; }
    }

    public float MoveTowOrigin
    {
        get { return _moveTowOrigin; }
        set { _moveTowOrigin = value; }
    }

    public HPers WorkerTask
    {
        get { return _workerTask; }
        set { _workerTask = value; }
    }

    public string MyAnimation
    {
        get { return _myAnimation; }
        set { _myAnimation = value; }
    }

    public List<VectorM> OrderedSites
    {
        get { return _orderedSites; }
        set { _orderedSites = value; }
    }

    public bool ExecuteNow
    {
        get { return _executeNow; }
        set { _executeNow = value; }
    }

    public bool DoneWorkNow
    {
        get { return _doneWorkNow; }
        set { _doneWorkNow = value; }
    }

    public float WorkTime
    {
        get { return _workTime; }
        set { _workTime = value; }
    }

    public CryRouteManager Router1
    {
        get { return _router; }
        set { _router = value; }
    }

    public CryRouteManager RouterBack
    {
        get { return _routerBack; }
        set { _routerBack = value; }
    }

    public float ProdXShift
    {
        get { return _prodXShift; }
        set { _prodXShift = value; }
    }

    public Vector3 LookAtWork
    {
        get { return _lookAtWork; }
        set { _lookAtWork = value; }
    }

    public Order Order1
    {
        get { return _order; }
        set
        {
            if (_order != null && value == null)
            {
                //Debug.Log("Who reWrote Order1."+_person.MyId+".");
            }
            _order = value;
        }
    }

    public string SourceBuildKey
    {
        get { return _sourceBuildKey; }
        set { _sourceBuildKey = value; }
    }

    public string DestinyBuildKey
    {
        get { return _destinyBuildKey; }
        set { _destinyBuildKey = value; }
    }

    public string StillElementId
    {
        get { return _stillElementID; }
        set { _stillElementID = value; }
    }

    public bool RouterActive
    {
        get { return _routerActive; }
        set { _routerActive = value; }
    }

    public Profession()
    {

        CleanOldProf();
        CleanOldVars();
     
    }

    /// <summary>
    /// Will set the ProdXShift Needs to account on:
    /// Age, School Years, Product to produce now, Genre, Tool
    /// </summary>
    private void SetProdXShift()
    {
        if (_person == null)
        {
            return;
        }

        //was set already
        if (ProdXShift > 0)
        {
            return;
        }
        
        var yearSchool = _person.YearsOfSchool;
        var produceFac = GetProduceFactor();

        //Grown man will prod 4.5KG of wood with 10 year of school
        //              (10 + 10     + 30        + ) * 0.09         = 4.5KG of Wood per shift
        //              (10 + 10     + 30        + ) * 0.008         = 0.4KG of Weapons per shift
        ProdXShift = (_person.HowMuchICanCarry() + yearSchool) * ToolsFactor() * produceFac/10;//1000

        if (ProfDescription == Job.Forester)
        {
            ProdXShift *= 1.2f;
        }

        //if is zero then will do this//is zero becasue one factor was zero. most likely the produceFac
        //for builders there is not produceFac
        if (ProdXShift == 0)
        {
            ProdXShift = (_person.HowMuchICanCarry() + yearSchool) * ToolsFactor();
        }
    }

    protected float ToolsFactor()
    {
        var thereAreTools = GameController.ThereIsAtLeastOneOfThisOnStorage(P.Tool);

        if (thereAreTools)
        {
            return 1;
        }
        return .5f;
    }

    float GetProduceFactor()
    {
        if (_person == null || _person.Work == null)
        {
            return 0;
        }

        var prod = _person.Work.CurrentProd.Product;
        if (StillElementId != "")
        {
            prod = Forester.FindProdImMining(StillElementId, _person);
        }

        var res = Program.gameScene.ExportImport1.ReturnProduceFactor(prod);
        if (res == 0)
        {
            //for builders
            res = 100;
        }

        return res;
    }

    /// <summary>
    /// Will show profesion. IWll address if homer or insider
    /// </summary>
    /// <returns></returns>
    internal string ProfessionDescriptionToShow()
    {
        Job res = ProfDescription;
        if (ProfDescription == Job.Homer)
        {
            res = Person.ReturnJobType(_person.Work);
        }
        if (res == Job.Insider)
        {
            if (_person.Work!=null)
            {
                if (_person.Work.HType.ToString().Contains("School"))
                {
                    return "Teacher";
                }
                else if(_person.Work.HType == H.BlackSmith)
                {
                    return "BlackSmith";
                }
            }

            return _person.Work.HType + " worker";
        }
        return Naming.CaseItRight(res+"");
    }

    /// <summary>
    /// Used to create a Dummy profession instance to save all attrb to file 
    /// </summary>
    /// <param name="prof"></param>
    public Profession(Profession prof)
    {
        LoadAttributes(prof);
    }

    protected void LoadAttributes(Profession prof)
    {
        if (prof.RouterBack != null)
        {
            RouterBack = new CryRouteManager();
            RouterBack.TheRoute = prof.RouterBack.TheRoute;
            PersonPot.Control.RoutesCache1.AddReplaceRoute(RouterBack.TheRoute);
        }
        if (prof.Router1 != null)
        {
            Router1 = new CryRouteManager();
            Router1.TheRoute = prof.Router1.TheRoute;
            PersonPot.Control.RoutesCache1.AddReplaceRoute(Router1.TheRoute);
        }

        ProfDescription=prof.ProfDescription;
        //Radius=prof.Radius;

        ReadyToWork=prof.ReadyToWork;
        WorkingNow=prof.WorkingNow;

        IsRouterBackUsed=prof.IsRouterBackUsed;
        FinRoutePoint=prof.FinRoutePoint;

        MoveTowOrigin=prof.MoveTowOrigin;
        WorkerTask=prof.WorkerTask;

        MyAnimation=prof.MyAnimation;
        OrderedSites=prof.OrderedSites;

        ExecuteNow=prof.ExecuteNow;

        DoneWorkNow=prof.DoneWorkNow;
        WorkTime=prof.WorkTime;

        Router1=prof.Router1;
        RouterBack = prof.RouterBack;

        ConstructingKey = prof.ConstructingKey;
        _constructing = Brain.GetStructureFromKey(ConstructingKey);

        ProdXShift = prof.ProdXShift;
        LookAtWork = prof.LookAtWork;

        Order1 = prof.Order1;
        DestinyBuildKey = prof.DestinyBuildKey;
        SourceBuildKey = prof.SourceBuildKey;

        ReadyToWork = prof.ReadyToWork;

        StillElementId = prof.StillElementId;
        FigureProdCarryingAndAmt();
    }



    private void CleanOldVars()
    {
        _workerTask = HPers.None;

        //_profDescription = Job.None;
        _person = null;
        _workTime = 4f;
        _readyToWork = false;
        _workingNow = false;
        _isRouterBackUsed = false;
        _routerActive = false;
        _finRoutePoint=new Vector3();

        _router = null;
        _routerBack = null;
    }


    protected void HandleNewProfDescrpSavedAndPrevJob(Job newJo)
    {
        _person.PrevJob = _person.SavedJob;
        _person.SavedJob = newJo;
        ProfDescription = newJo;
    }

    /// <summary>
    /// Work Action is called from brain when person is actually in job site 
    /// </summary>
    public virtual void WorkAction(HPers p)
    {
        var others = ProfDescription != Job.Forester && _readyToWork;

        //if cant take anything out of work should it go
        var forester = ProfDescription == Job.Forester && _readyToWork && _person.Work.CanTakeItOut(_person);

        CheckIfHasWorkInputOrder();

        if (others || forester)
        {
            //Debug.Log("workingNow:" + _person.MyId);
            _workingNow = true;
        }
        else
        {
            _person.Brain.CurrentTask = p;
        }
    }


    private void CheckIfHasWorkInputOrder()
    {
        if (_person.Body.Location != HPers.Work)
        {
            return;
        }
        if (_person.IsCarryingWorkInputOrder())
        {
            var ord = _person.ReturnFirstOrder();

            _person.AddToOrdersCompleted(_person.Inventory.InventItems[0].Amount);
            _person.ExchangeInvetoryItem(_person, _person.Work, ord.Product, _person.Inventory.InventItems[0].Amount);
        }
    }

    /// <summary>
    /// Meant to be called when work is done 
    /// 
    /// Once is called will promt the brain to continue to next StateMind which is Idle
    public virtual void DoneWork()
    {
        _person.Brain.CurrentTask = HPers.None;

        _person.Work.SmokePlay(false);
        //GameScene.print("Done work:" + _person.MyId);

        ////foresters reset when done work
        //if (ProfDescription == Job.Forester)
        //{
        //    ResetDummy();
        //}
    }

    protected Structure CreateDummy()
    {
        //added the finROute to name bz it could be different in a same building 
        //return Program.gameScene.GimeMeUnusedDummy(_constructing.MyId+".Dummy."+FinRoutePoint);

        return (Structure)Building.CreateBuild(Root.dummyBuildWithSpawnPoint, new Vector3(), H.Dummy,
           container: Program.ClassContainer.transform);
    }

    /// <summary>
    /// This needs to be called from every child and to work must be called too from Person.Update()
    /// </summary>
    public virtual void Update()
    {
        if (_breakInitNow)
        {
            TakeInitBreak();
            return;
        }

        RemoveMeFromQueueIfImThereAndNotUsingIt();
        RouterDealear();

        if (_workingNow)
        {
            WorkNow();
        }
        //GameScene.print("Update on Profession");

        //SetProdXShift();
	}

    /// <summary>
    /// bz sometimes profession goes and create a different profession that at the moment
    /// is not Routing and the guy gets stuck on System and doesnt let anyone else
    /// ReRoute
    /// </summary>
    private void RemoveMeFromQueueIfImThereAndNotUsingIt()
    {
        //if is routing then let it here so routes 
        if (_routerActive || _person==null)
        {
            return;
        }

        //if (PersonPot.Control.WorkersRoutingQueue.OnSystemNow(_person.MyId))
        //{
        //    PersonPot.Control.WorkersRoutingQueue.RemoveMeFromSystem(_person.MyId);
        //    //Debug.Log("remove form system prof:" + _person.MyId);
        //}
    }

    void RouterDealear()
    {
        if (_routerActive)
        {
            AddMeToWaitListOnSystem();

            //if (PersonPot.Control.WorkersRoutingQueue.OnSystemNow(_person.MyId))
            //{
                if (_isRouterBackUsed)
                {
                    BackRouterUpdate();
                }
                else SingleRouterUpdate();
            //}
        }
    }

    void AddMeToWaitListOnSystem()
    {
        //bz we pulled already the routes
        if (WereTheTwoRoutesInCache())
        {
            _routerActive = false;
            return;
        }

        //needs to finish thet route first. then will create this one 
        if (_person.Brain._workRoute.CheckPoints.Count==0)
        {
            return;
        }

        PersonPot.Control.WorkersRoutingQueue.AddMeToOnSystemWaitList(_person.MyId);
    }

    /// <summary>
    /// Will pull the routes if are in cache . will return true if they were both addressed or if was only one needed and
    /// addressed
    /// 
    /// Here bz otherwise will put Professional on queue to become a Homer for example when is not need bz the routes
    /// exist. And actually the Homer could be forever waiting on a Farm for example to get the new routes 
    /// </summary>
    /// <returns></returns>
    bool WereTheTwoRoutesInCache()
    {
        if ( Router1 != null && !Router1.IsRouteReady)
        {
            if (PersonPot.Control.RoutesCache1.ContainANewerOrSameRoute(Router1.OriginKey, Router1.DestinyKey,
                new DateTime()))
            {
                AddressRouter(Router1);
            }
        }
        if (IsRouterBackUsed && RouterBack != null && !RouterBack.IsRouteReady)
        {
            if (PersonPot.Control.RoutesCache1.ContainANewerOrSameRoute(RouterBack.OriginKey, RouterBack.DestinyKey,
                new DateTime()))
            {
                AddressRouter(RouterBack);
            }
        }

        if (IsRouterBackUsed)
        {
            if (RouterBack == null)
            {
                return false;
            }
            return RouterBack.IsRouteReady && Router1.IsRouteReady;
        }
        return Router1.IsRouteReady;
    }

    /// <summary>
    /// Things that need to be done to the Router if a new Route was found on RoutesCache
    /// </summary>
    /// <param name="routerP"></param>
    void AddressRouter(CryRouteManager routerP)
    {
        var route = PersonPot.Control.RoutesCache1.GiveMeTheNewerRoute();

        if (route!=null)
        {
            routerP.TheRoute = route;
            routerP.IsRouteReady = true;

            if (IsRouterBackUsed)
            {
                BackRouterUpdate();
            }
            else SingleRouterUpdate();
        }
    }

    /// <summary>
    /// </summary>
    void ReRouteDone()
    {
        var timeOnSys = PersonPot.Control.WorkersRoutingQueue.DoneReRoute(_person.MyId);//so another people can use the Spot

//        Debug.Log("remove from cntrl prof:" + _person.MyId + " :" + ProfDescription + " on Sys:" + timeOnSys);

    }

    /// <summary>
    /// Decisions  on the update when the Back Routers is used
    /// </summary>
    void BackRouterUpdate()
    {
        if (!_router.IsRouteReady || (_routerBack!=null && !_routerBack.IsRouteReady))
            //if routerBack is null is bz routerBackWasInit was not set to false
        {
            _router.Update();

            //for foreseter that 1st does Router1 then RouterBack
            if (_routerBack!=null)
            {
                _routerBack.Update();
            }
        }                                 //for forrester
        else if (_router.IsRouteReady && _routerBack!=null && _routerBack.IsRouteReady)
        {
            _readyToWork = true;
            _routerActive = false;
            Unlock();
            ReRouteDone();

            //foresters reset when done work
            if (ProfDescription!=Job.Forester)
            {
                ResetDummy();
            }
        }
    }

    /// <summary>
    /// Decisions on the update when the back router is not used 
    /// </summary>
    void SingleRouterUpdate()
    {
        if (!_router.IsRouteReady )
        {
            _router.Update();
        }
        else if (_router.IsRouteReady )
        {
            _readyToWork = true;
            _routerActive = false;
            Unlock();
            ReRouteDone();

            //foresters reset when done work
            if (ProfDescription != Job.Forester)
            {
                ResetDummy();
            }
        }
    }

    /// <summary>
    /// Created to address that sometimes derived classs dont even spawn the dummy helper 
    /// </summary>
    protected void ResetDummy()
    {
        if (_person==null)
        {
            return;
        }
        _person.MyDummyProf.MyId = "DummyProfReset";

        _person.MyDummyProf.LandZone1.Clear();
        _person.MyDummyProf.DummyIdSpawner = "";

        //if (dummy == null)
        //{
        //    return;
        //}

        //if (ProfDescription==Job.Forester)
        //{
        //    //Debug.Log("Destroy dummy");
        //    dummy.Destroy();
        //    return;
        //}

        ////Debug.Log("Reset dummy:" + _person.MyId);
        //Program.gameScene.ReturnUsedDummy(dummy);
        //dummy = null;
    }


    bool IsAnExistingBuilding(TheRoute theRoute)
    {
        var a = theRoute.DestinyKey.Substring(theRoute.DestinyKey.Length - 2);

        return
            theRoute.DestinyKey.Contains("Dummy") ||
            theRoute.DestinyKey.Contains("Tree") ||
            theRoute.DestinyKey.Contains("Rock") ||
            Brain.GetStructureFromKey(theRoute.DestinyKey) != null ||
            theRoute.DestinyKey.Substring(theRoute.DestinyKey.Length-2) == ".D";//in Work Route (farmer, fisheerman)

    }

    int foresterStuck = 0;
    bool stuckedForester;
    /// <summary>
    /// If _workingNow = true this method will be called from derived class.
    ///  This is called once upon person is already on JobSite
    /// </summary>
    protected void WorkNow()
    {
        stuckedForester = _person.Body.Location == HPers.Work &&
            ProfDescription == Job.Forester && _person.Body.GoingTo == HPers.InWork
            && _workerTask == HPers.WalkingToJobSite && _person.Body.BodyAgent.IsStuck()
            ;

        if (stuckedForester)
        {
            foresterStuck++;
            _person.Body.Location = HPers.Work;
            _workerTask = HPers.DoneAtWork;
            _person.Body.GoingTo = HPers.Work;
            StillElementId = "";
            _person.OrderRedoWhenGetsHome();

            //_person.Body.Location = HPers.Work;
            //_workerTask = HPers.None;
            //_person.Body.GoingTo = HPers.Work; 

            Debug.Log("unstuck forester: " + _person.name);






            //if(foresterStuck > 1)
            //{
            //    _person.Work.ChangeMaxAmoutOfWorkers("Less");
            //}

            //_person.Body.Location = HPers.Work;
            //_workerTask = HPers.None;
        }

        //walking toward the job site for forester walking towards a tree 
        if (_person.Body.Location == HPers.Work && _workerTask == HPers.None)
        {
            if (_router!=null && _router.TheRoute.OriginKey != _router.TheRoute.DestinyKey
                && IsAnExistingBuilding(_router.TheRoute))
                //so doesnt go in and out in the same building
                //the is not booked to avoid people staying in the same House when grow older in same place 
            {
                _person.Body.WalkRoutine(_router.TheRoute, HPers.InWork);
                _workerTask = HPers.WalkingToJobSite;
            }
            //when is importing something
            //so the Work is the same as _sourceBuild
            //to avoid go in and out again in the Dock
            else
            {
                //Debug.Log("Person had same Destiny and OriginKey was sent back to office :"+_person.MyId);
                PreparePersonToGetBackToOffice();
            }
        }
        //called here so animation of iddle can be fully transitioned to
        else if ((_person.Body.Location == HPers.InWork && _workerTask == HPers.WalkingToJobSite && !_person.Body.MovingNow)
           // || stuckedForester
            )
        {
            Idle(HPers.AniFullyTrans, 1f);
        }
        //for forester //ChopWood
        else if (_workerTask == HPers.AniFullyTrans)
        {
            var forester = ProfDescription == Job.Forester && (ElementWasCut() || LoadedDifferentElement()
                //|| !_person.Work.CanTakeItOut(_person)
                );

            var builder = ProfDescription == Job.Builder && CurrentConstructionIsNullOrDone();
            if (forester || builder)
            {
                StillElementId = "";
                PreparePersonToGetBackToOffice();
                return;
            }

            //so its doesnt play 'isWheelBarrow' ani in the midle of nothing tht sometimes
            //wheel lead to wheelBarrowers that have not a WheelBarrpw to play the ani while carrying a box 
            if (ProfDescription!=Job.WheelBarrow && ProfDescription!=Job.Docker &&
                _person.Body.CurrentAni!= _myAnimation)//we need to pass it only once. dont keep doing it 
            {
                _person.Body.TurnCurrentAniAndStartNew(_myAnimation);
                //Debug.Log("_myAnimation sent on siteWork:"+_myAnimation+ " .profDesc:"+ProfDescription);
            }
            Idle(HPers.WorkingInPlaceNow, _workTime);
        }
        //called here so animation of iddle can be fully transitioned to
        else if (_workerTask == HPers.WorkingInPlaceNow)
        {
            PreparePersonToGetBackToOffice();
        }
        else if (_workerTask == HPers.WalkingBackToOffice)
        {
            _executeNow = true;
            _doneWorkNow = true;//set here once the ani is fully transioned to

            //so it will get right animation . bz homer walks first and then drop/gets load
            //there fore can have an animation of carrying with empty inv 
            if (ProfDescription==Job.Homer)
            {
                return;
            }
            ComingBackToOffice();
        }
        else if (_person.Body.Location == HPers.FoodSource && _workerTask == HPers.DoneAtFoodScr &&
                _person.Body.GoingTo == HPers.FoodSource)
        {
            //so in brain all gets retarted again 
            _person.Brain.CurrentTask = HPers.Walking;
            ResetMiniMindState();
        }
        else if (_person.Body.Location == HPers.Work && _workerTask == HPers.DoneAtWork &&
             _person.Body.GoingTo == HPers.Work)
        {
            DoneWork();
        }
        //for wheelbarrowers alone and dockers.. 
        else if (_person.Body.Location == HPers.WheelBarrow 
            && _workerTask == HPers.DoneAtWheelBarrow && _person.Body.GoingTo == HPers.WheelBarrow)
        {
            //so in brain all gets retarted again 
            _person.Brain.CurrentTask = HPers.WheelBarrow;
            WheelBarrowDropLoad();
            ConvertWheelBarrow();
        }    
        //for loading stuck Homer that was Farmer only
        else if (_person.Body.Location == HPers.WheelBarrow 
            && ProfDescription == Job.Homer && _person.PrevJob == Job.Farmer && _workerTask == HPers.None
            && _person.Body.GoingTo == HPers.WheelBarrow)
        {
            //so in brain all gets retarted again 
            _person.Brain.CurrentTask = HPers.WheelBarrow;
            ConvertToHomer();//called here bz need to restart 
        }
        //for homers so they can start all over again at home just as had finished Work
        else if (_person.Body.Location == HPers.Home && _workerTask == HPers.DoneAtHome &&
        _person.Body.GoingTo == HPers.Home)
        {
            _person.Brain.CurrentTask = HPers.Walking;
            _person.Body.Location = HPers.Home;
            _person.Body.GoingTo = HPers.Home;

            ResetMiniMindState();
        }
    }


    bool CurrentConstructionIsNullOrDone()
    {
        var constr = Brain.GetBuildingFromKey(ConstructingKey);
        return constr == null || constr.StartingStage == H.Done;
    }


    #region New Logic that all go back to closer Empty Storage to drop Load

    bool IsAHomerCreator()
    {
        if (ProfDescription == Job.WheelBarrow || ProfDescription == Job.Docker
            || IsNewHomerCreator() || IsNewHomerCreatorUsingAnInWorkRoute())
        {
            return true;
        }
        return false;
    }

    bool IsNewHomerCreator()
    {
        return ProfDescription == Job.Insider;
    }

    bool IsNewHomerCreatorUsingAnInWorkRoute()
    {
        return ProfDescription == Job.Farmer || ProfDescription == Job.SaltMiner || ProfDescription == Job.FisherMan;
    }

    protected void FakeWheelBarrowToRouteBack()
    {
        _person.Body.Location = HPers.WheelBarrow;
        _workerTask = HPers.DoneAtWheelBarrow;
        _person.Body.GoingTo = HPers.WheelBarrow;
    }

    protected void FakeRouter1ForNewProfThatUseHomer()
    {
        Router1 = new CryRouteManager();
        Router1.TheRoute = new TheRoute();
        Router1.IsRouteReady = true;
    }

    /// <summary>
    /// 
    /// Conditions so it works:
    /// and Router1 should be all set too. If never use Route1 can call FakeRouter1ForNewProfThatUseHomer()
    /// must be:  _routerActive = true;
    /// </summary>
    protected void RouteBackForNewProfThatUseHomer()
    {
        ////it will stay on limbo until redos profession again
        //if (_person.Brain.IsContainOnBlackList(_person.Work.MyId, _person.Work.PreferedStorage.MyId))
        //{
        //    Debug.Log("contained on Blaclist: "+_person.MyId);
        //    return;
        //}

        _routerActive = true;
        IsRouterBackUsed = true;
        RouterBack = new CryRouteManager(_person.Work, ReturnStorage(), _person, HPers.InWorkBack);
    }

    Structure ReturnStorage()
    {
        if (_person.Work.PreferedStorage == null)
        {
            return _person.FoodSource;
        }
        return _person.Work.PreferedStorage;
    }

#endregion

    private void ConvertWheelBarrow()
    {
        if (!IsAHomerCreator())
        {
            return;
        }
        var a = _person.Name;


        //so work Profession Mini States
        _person.Body.Location = HPers.Work;
        _workerTask = HPers.None;

        //_person.HomerFoodSrc = _sourceBuildKey;
        _person.CreateProfession(Job.Homer);
    }

    void ConvertToHomer()
    {
        //so work Profession Mini States
        _person.Body.Location = HPers.Work;
        _workerTask = HPers.None;
    }

    /// <summary>
    /// Wheel Barrower will drop load of Goods on Destiny
    /// </summary>
    private void WheelBarrowDropLoad()
    {
        //they just need to keep going to Final FoodSrc 
        if (ProfDescription != Job.WheelBarrow && ProfDescription != Job.Docker)
        {
            return;
        }

        var destinyBuild = GetStructureSrcAndDestiny(_destinyBuildKey, _person);

        if (destinyBuild == null)
        {
            //Debug.Log("destinyBuild null whelbarr:" + _person.MyId + "._destinyBuildKey:" + _destinyBuildKey);
            _person.Inventory.Delete();
            return;
        }

        if (_order == null)
        {
            Debug.Log("order null whelbarr:" + _person.MyId);
            _person.Inventory.Delete();

            return;
        }

        _person.ExchangeInvetoryItem(_person, destinyBuild, _order.Product, _order.Amount);

        //each time a wheelbarrow or docker uses a wheelBarrow dimish them a bit in the main storages
        //as the wheelbarrows get use they get destroy
        GameController.ResumenInventory1.Remove(P.WheelBarrow, .1f);

        Quest();
    }

    private void Quest()
    {
        if (_order.Product == P.Bean && _order.SourceBuild.Contains("FieldFarmSmall"))
        {
            Program.gameScene.QuestManager.QuestFinished("Transport");   
        }
    }

    /// <summary>
    /// Created so builders can used it if a building is destroy while they are working on it 
    /// 
    /// now foresrets use it too if they went to a tree that was recently cut and is growing now 
    /// and for stones,iron and gold recently removed too 
    /// </summary>
    protected void PreparePersonToGetBackToOffice()
    {
        _person.Body.TurnCurrentAniAndStartNew("isIdle");
        Idle(HPers.WalkingBackToOffice, 1f);
    }

    /// <summary>
    /// Will address the comming back to office action in where if _isRouterBackUsed will use a diff route 
    /// to go back
    /// 
    /// and if is not marked will use the _inverse of _router
    /// </summary>
    protected void ComingBackToOffice()
    {
        if (_isRouterBackUsed)
        {
            //bz in wheelbarrower the back is use to do the route Source to Destination
            if (IsAHomerCreator())
            {
                //they will just use a Homer to go home
                if (!IsNewHomerCreatorUsingAnInWorkRoute())
                {
                    _person.Body.WalkRoutine(_routerBack.TheRoute, HPers.WheelBarrow);
                }
                else
                {
                    _person.Body.WalkRoutine(_router.TheRoute, HPers.WheelBarrow, true);
                }
                _workerTask = HPers.DoneAtWheelBarrow;   
            }
            else  if (ProfDescription == Job.Homer)
            {
                _person.Body.WalkRoutine(_routerBack.TheRoute, HPers.Home);
                _workerTask = HPers.DoneAtHome; //so reset the cycle
            }
            else
            {
                _person.Body.WalkRoutine(_routerBack.TheRoute, HPers.FoodSource);
                _workerTask = HPers.DoneAtFoodScr; //so reset the cycle 
            }
        }
        else
        {
            //_person.Body.WalkRoutine(_router.TheRoute, HPers.Work, true);
            //_workerTask = HPers.DoneAtWork; //so reset the cycle                 
        }
    }

    void ResetMiniMindState()
    {
        _workingNow = false;
        _workerTask = HPers.None;
        //Debug.Log("resetMIniMindTstae:"+_person.MyId);
    }

    protected void CheckIfProfHasToBeReCreated()
    {
        if (_person.Work == null)
        {
            return;
        }

        if (ForesterHasNullEle() || ForesterCurrentStillEleIsBlackListed() || //|| LoadedDifferentElement()
            string.IsNullOrEmpty(StillElementId))
        {
            if (_person == null)
            {
                return;
            }
            //Debug.Log("foresetr recrete prof:"+_person.MyId);
            _person.CreateProfession();
            return;
        }

        var ele = Program.gameScene.controllerMain.TerraSpawnController.Find(StillElementId);
        if (ele !=null && !ele.Grown())
        {
            _person.CreateProfession();
        }
    }


    /// <summary>
    /// The id of an Still Element is not the same when loads again
    /// </summary>
    /// <returns></returns>
    private bool LoadedDifferentElement()
    {
        var ele =
        Program.gameScene.controllerMain.TerraSpawnController.Find(StillElementId);

        if (ele == null)
        {
            return true;
        }

        //if tht is over the amount on distance is not the same 
        return Vector3.Distance(ele.transform.position, FinRoutePoint) > 1f;
    }





    /// <summary>
    /// If Foresetrr has that Still Element blacklisted needs to Recreate Profession
    /// Otherwise wont look for new StillElements
    /// </summary>
    /// <returns></returns>
    private bool ForesterCurrentStillEleIsBlackListed()
    {
        if (ProfDescription != Job.Forester || _person == null)
        {
            return false;
        }

        return _person.Brain.BlackList.Contains(StillElementId);
    }

    /// <summary>
    /// If forester Elelemts was removed recently 
    /// </summary>
    /// <returns></returns>
    private bool ForesterHasNullEle()
    {
        if (ProfDescription != Job.Forester)
        {
            return false;
        }

        var ele =
                Program.gameScene.controllerMain.TerraSpawnController.Find(StillElementId);

        return ele == null;
    }

    /// <summary>
    /// Needs to check before cut if the element is not Grown
    /// </summary>
    /// <returns></returns>
    private bool ElementWasCut()
    {
        if (_person == null)
        {
            return true;
        }
        var ele =
        Program.gameScene.controllerMain.TerraSpawnController.Find(StillElementId);

        if (ele == null || !ele.Grown())
        {
            return true;
        }
        return false;
    }


    private float startIdleTime;
    /// <summary>
    /// Idle Action that is to perform the animation of work
    /// </summary>
    /// <param name="nextTask">The task will have after Idle is done</param>
    void Idle(HPers nextTask, float idleTime)
    {
        if (startIdleTime == 0)
        {
            var lookAtWork = DefineLookAt();

            //this is recreateing the initial point 
            var t = Vector3.MoveTowards(_finRoutePoint, lookAtWork, -_moveTowOrigin);

            if (_person.Work!=null && ProfDescription == Job.Builder
                && _constructing!=null)
            {
                _person.transform.LookAt(new Vector3(_constructing.MiddlePoint().x, _person.transform.position.y,
                    _constructing.MiddlePoint().z));
            }
            else
            {
                _person.transform.LookAt(t);
            }

            startIdleTime = Time.time;
        }

        if (IsGameSpeedIsZero())
        {return;}


        if (Time.time > startIdleTime + idleTime // / Program.gameScene.GameSpeed
            )
        {
            _workerTask = nextTask;
            startIdleTime = 0;
        }
    }

    /// <summary>
    /// Create to find out if look out was defined . If was not will use _person.Work.transform.position
    /// </summary>
    /// <returns></returns>
    Vector3 DefineLookAt()
    {
        if (LookAtWork == new Vector3() && _person.Work != null)
        {
            LookAtWork = _person.Work.transform.position;
        }
        return LookAtWork;
    }

    /// <summary>
    /// If game speed is zero wont allow to continue iddle . bz will consume the time there
    /// without playing the animation 
    /// </summary>
    /// <returns></returns>
    bool IsGameSpeedIsZero()
    {
        if (Program.gameScene.GameSpeed==0)
        {
            startIdleTime = Time.time;
            return true;
        }
        return false;
    }

    #region Save . File Writting

    /// <summary>
    /// Will lock on person control. So if saved is tryied will be delayed until is unlocked again 
    /// 
    /// This is to avoid things like saving a profession on the midddle of routing 
    /// </summary>
    protected void Lock()
    {
        PersonPot.Control.Locked = true;
    }

    protected void Unlock()
    {
        //they work under Locked, if gets unlocked will lead to bugg  
        if (ProfDescription == Job.ShackBuilder)
        {
            return;
        }

        PersonPot.Control.Locked = false;
    }

    #endregion

    /// <summary>
    /// Address to produce the selected prod in the work and the amount per shift defined in 'ProdXShift'
    /// </summary>
    public void Execute(string instruct = "", P prod = P.None)
    {
        //to address if work place is being destyo on the persons way
        if (_person.Work == null)
        {
            return;
        }

        SetProdXShift();
        Produce(instruct, prod);
        if (ReadyToWork)
        {
//            Debug.Log("workingNow:" + _person.MyId);
            WorkingNow = true;
        }
    }


    /// <summary>
    /// so I dont have to save load those
    /// </summary>
    private void FigureProdCarryingAndAmt()
    {
        if (_person == null || _person.Inventory.InventItems.Count == 0)
        {
            return;
        }
        amtCarrying = _person.Inventory.InventItems[0].Amount;
        prodCarrying = _person.Inventory.InventItems[0].Key;
    }

    private float amtCarrying;
    protected P prodCarrying = P.None;
    /// <summary>
    /// The action of producing goods 
    /// </summary>
    void Produce()
    {
        _person.Work.Produce(ProdXShift, _person);

        if (_person.Work.CanTakeItOut(_person))
        {
            amtCarrying = _person.HowMuchICanCarry();//ProdXShift

            _person.ExchangeInvetoryItem(_person.Work, _person, DefineProdWillCarry(), amtCarrying);
            prodCarrying = _person.Work.CurrentProd.Product;
            
            //people comsuming tools as they work 
            GameController.ResumenInventory1.Remove(P.Tool, 0.1f);
        }
    }

    /// <summary>
    /// So it handles what is the first item on inventory. otherwise will return CurrentProd 
    /// </summary>
    /// <returns></returns>
    P DefineProdWillCarry()
    {
        //as they have inputs and inputs are usualy in the first of the iNventory
        if (ProfDescription==Job.Insider)
        {
            return _person.Work.CurrentProd.Product;
        }

        if (_person.Work.Inventory.InventItems.Count>0)
        {
            //so removes the first item on Inventory 
            return _person.Work.Inventory.InventItems[0].Key;
        }
        return _person.Work.CurrentProd.Product;
    }
    
    /// <summary>
    /// The action of producing goods 
    /// </summary>
    void Produce(string instruct, P prod)
    {
        //if has not instructions is normal Produce()
        if (string.IsNullOrEmpty(instruct))
        {
            Produce();
            return;
        }

        //if has instructions is so far from a : Forester
        //they want to physically bring prod back and then drop it at they Storage 
        //dont want to added to building invetory he has to drop it there when he gets there 
        _person.Work.Produce(_person.HowMuchICanCarry(), _person, false, prod);

        //so foreseter show Wood , carry wood 
        _person.Body.UpdatePersonalObjAniSpeed();

        //means the prod was sent directly from Profession
        //Forestert is using this bz he Might mine ore or cut trees
        if (prod != P.None)
        {
            prodCarrying = prod;
        }
        else prodCarrying = _person.Work.CurrentProd.Product;

        amtCarrying = _person.HowMuchICanCarry();
    }

    /// <summary>
    /// Will drop all goods is carrying into FoodSource
    /// 
    /// Change bz I need workers that were fired drop input items when they go back to 
    /// Storage after being fired
    /// </summary>
    public void DropGoods()
    {
        if (_person == null || _person.FoodSource == null)
        {
            return;
        }

        DropAllMyGoods(_person.FoodSource);
        //_person.ExchangeInvetoryItem(_person, _person.FoodSource, prodCarrying, amtCarrying);
    }

    public void DropAllMyGoods(Structure st)
    {
        if (_person == null || st == null)
        {
            return;
        }

        _person.DropAllInvetoryItems(_person, st);
    }

    /// <summary>
    /// Used to find if something change and move on to a new task as a pRofessional
    /// 
    /// Ex. Builder finish building one building should find a new one to do tht
    /// </summary>
    public virtual void AnyChange()
    {
        
    }

    /// <summary>
    /// Will clean old profession objects
    /// </summary>
    internal void CleanOldProf()
    {
        CleanOldVars();
        ResetDummy();
    }


    /// <summary>
    /// Created to modularize the case in where is ship wht has to be returned is the Dock, _person.Work
    /// </summary>
    /// <returns></returns>
    public static Structure GetStructureSrcAndDestiny(string id, Person person)
    {
        //if (id == "Ship")
        //{
        //    return person.Work;
        //}
        return Brain.GetStructureFromKey(id);

      

    }



    #region Break Init 

    protected bool _breakInitNow;
    private float _breakInitDuration = 1f;
    private float _startInitBreakTime;
    protected bool _reInitNow;//will say if need to call Init() again

    protected bool ShouldITakeBreakInit()
    {
        if (_person.Work == null)
        {
            return true;
        }

        if (_person.Brain._workRoute.CheckPoints.Count == 0 ||
          _person.Brain._workRoute.DestinyKey != _person.Work.MyId)
        {
            //Debug.Log(ProfDescription + ": take break now:" + _person.MyId);
            _breakInitNow = true;
            _startInitBreakTime = Time.time;

            return true;
        }
        return false;
    }

    /// <summary>
    /// Called every birthday of this person
    /// </summary>
    internal void Birthday()
    {
        if (_person.Work.HType == H.LightHouse)
        {
            BuildingPot.Control.DockManager1.PortReputation += 0.025f;
        }
    }

    /// <summary>
    /// Used so a person is asking for bridges anchors takes a break and let brdige anchors complete then can 
    /// work on it
    /// </summary>
    void TakeInitBreak()
    {
        if (Time.time > _startInitBreakTime + _breakInitDuration)
        {
            _breakInitNow = false;
            _startInitBreakTime = 0;

            _reInitNow = true;
        }
    }

    #endregion





    internal void OnBuildDoneHandler(object sender, EventArgs e)
    {
        if (_person != null && _person.Body.Location == HPers.Work)
        {
            //Debug.Log("Called event to back to ooffice " + _person.name);
            //_person.Body.ReachDestinyAgentBody();
        }
    }
}
