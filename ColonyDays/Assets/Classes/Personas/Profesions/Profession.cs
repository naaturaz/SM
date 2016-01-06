﻿using System.Collections.Generic;
using UnityEngine;
/*
 * 
 * on Brain.cs, void GoWork() there is interactin with Docker, WheelBarrow, Homer, Forester 
 * 
 * 
 */

public class Profession  {

    protected Person _person;
    protected static float radius = 200f;//20f, how far will go to cut a tree 

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
    protected Structure dummy;

    //Will execute the action it came to do in code... 
    //for ex will load inventory with Lumber from cutted tree 
    protected bool _executeNow;

    //says the exact moment when the person finished the work in the site 
    protected bool _doneWorkNow;
    protected float _workTime = 1f;//how long will execute the animation of work
    protected Job _profDescription = Job.None;

    //ShackBuilder and Builders
    protected Building _constructing;//need to implement  to be saved and loaded 
    public string ConstructingKey;//need to implement  to be saved and loaded 

    private int _prodXShift = 1000;//100 //Wht a person will produce or carry in a shift 

    //The pos to look at while working if is = new Vector3 the pos of the Work will be used then 
    private Vector3 _lookAtWork;

    //WheelBarrowe
    protected Order _order;//the order of a wheelBarrower
    protected string _sourceBuildKey;//from where taking the load 
    protected string _destinyBuildKey;//where taking load 

    protected Structure _destinyBuild;
    protected Structure _sourceBuild;

    public Job ProfDescription
    {
        get { return _profDescription; }
        set { _profDescription = value; }
    }

    public static float Radius
    {
        get { return radius; }
        set { radius = value; }
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

    public int ProdXShift
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
        set { _order = value; }
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

    public Profession()
    {
        CleanOldProf();
        CleanOldVars();
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
    }

    private void CleanOldVars()
    {
        _workerTask = HPers.None;

        _profDescription = Job.None;
        _person = null;
        _workTime = 1f;
        _readyToWork = false;
        _workingNow = false;
        _isRouterBackUsed = false;
        _routerActive = false;
        _finRoutePoint=new Vector3();

        _router = null;
        _routerBack = null;
    }

    /// <summary>
    /// Work Action is called from brain when person is actually in job site 
    /// </summary>
    public virtual void WorkAction(HPers p)
    {
        if (_readyToWork)
        {
            _workingNow = true;
        }
        else
        {
            _person.Brain.CurrentTask = p;
        }
    }

    /// <summary>
    /// Meant to be called when work is done 
    /// 
    /// Once is called will promt the brain to continue to next StateMind which is Idle
    public virtual void DoneWork()
    {
        _person.Brain.CurrentTask = HPers.None;
        GameScene.print("Done work:" + _person.MyId);

        ////bz job foreseter drops things when gets at work
        //if (ProfDescription == Job.Forester)
        //{
        //    DropGoods(_person.Work);
        //}
    }

    /// <summary>
    /// This needs to be called from every child and to work must be called too from Person.Update()
    /// </summary>
    public virtual void Update() 
    {
        RouterDealear();

        if (_workingNow)
        {
            WorkNow();
        }
        //  GameScene.print("Update on Profession");
	}

    void RouterDealear()
    {
        if (_routerActive)
        {
            if (_isRouterBackUsed)
            {
                BackRouterUpdate();
            }   
            else SingleRouterUpdate();
        }
    }

    /// <summary>
    /// Decisions  on the update when the Back Routers is used
    /// </summary>
    void BackRouterUpdate()
    {
        if (!_router.IsRouteReady || !_routerBack.IsRouteReady)
        {
            _router.Update();
            _routerBack.Update();
        }
        else if (_router.IsRouteReady && _routerBack.IsRouteReady)
        {
            _readyToWork = true;
            _routerActive = false;
            Unlock();
            DestroyDummy();
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
            DestroyDummy();
        }
    }

    /// <summary>
    /// Created to address that sometimes derived classs dont even spawn the dummy helper 
    /// </summary>
    protected void DestroyDummy()
    {
        if (dummy!=null)
        {
         //   Program.gameScene.ReturnUsedDummy(dummy);
            dummy.DestroySafe();
        }
    }

    /// <summary>
    /// If _workingNow = true this method will be called from derived class.
    ///  This is called once upon person is already on JobSite
    /// </summary>
    protected void WorkNow()
    {
        //walking toward the job site for forester walking towards a tree 
        if (_person.Body.Location == HPers.Work && _workerTask == HPers.None)
        {

            if (_router.TheRoute.OriginKey != _router.TheRoute.DestinyKey)//so doesnt go in and out in the same building
            {
                _person.Body.WalkRoutine(_router.TheRoute, HPers.InWork, hideThisTime: false);
                _workerTask = HPers.WalkingToJobSite;
            }
            //when is importing something
            //so the Work is the same as _sourceBuild
            //to avoid go in and out again in the Dock
            else
            {
                PreparePersonToGetBackToOffice();
            }

        }
        //called here so animation of iddle can be fully transitioned to
        else if (_person.Body.Location == HPers.InWork && _workerTask == HPers.WalkingToJobSite && !_person.Body.MovingNow)
        {
            Idle(HPers.AniFullyTrans, 1f);
        }
        //for forester //ChopWood
        else if (_workerTask == HPers.AniFullyTrans)
        {
            _person.Body.TurnCurrentAniAndStartNew(_myAnimation);
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

            //they dont need to return anywhere... 
            if (_profDescription == Job.ShackBuilder)
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
            //_person.Brain.CurrentTask = HPers.None;
            ResetMiniMindState();
        }
        //for wheelbarrowers alone
        else if (_person.Body.Location == HPers.WheelBarrow 
            && _workerTask == HPers.DoneAtWheelBarrow && _person.Body.GoingTo == HPers.WheelBarrow)
        {
            //so in brain all gets retarted again 
            _person.Brain.CurrentTask = HPers.WheelBarrow;
            WheelBarrowDropLoad();
            ConvertWheelBarrow();
        }
        //for homers so they can start all over again at home just as had finished Work
        else if (_person.Body.Location == HPers.Home && _workerTask == HPers.DoneAtHome &&
        _person.Body.GoingTo == HPers.Home)
        {
            //so in brain all gets retarted again . 
            _person.Brain.CurrentTask = HPers.IdleInHome;
            _person.Brain.PreviousTask = HPers.IdleSpot;

            ResetMiniMindState();
        }
    }

    private void ConvertWheelBarrow()
    {
        if (ProfDescription != Job.WheelBarrow && ProfDescription != Job.Docker)
        {
            return;
        }

        //so work Profession Mini States
        _person.Body.Location = HPers.Work;
        _workerTask = HPers.None;

        _person.HomerFoodSrc = _sourceBuildKey;
        _person.CreateProfession(Job.Homer);
    }

    /// <summary>
    /// Wheel Barrower will drop load of Goods on Destiny
    /// </summary>
    private void WheelBarrowDropLoad()
    {
        if (ProfDescription != Job.WheelBarrow && ProfDescription != Job.Docker)
        {
            return;
        }

        var destinyBuild = GetStructureSrcAndDestiny(_destinyBuildKey, _person);

    //    Debug.Log(_person.MyId + " " + ProfDescription + " Dropped load in:" + destinyBuild.MyId +
    //".prod:" + _order.Product);

        _person.ExchangeInvetoryItem(_person, destinyBuild, _order.Product, _order.Amount);
    }

    /// <summary>
    /// Created so builders can used it if a building is destroy while they are working on it 
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
    void ComingBackToOffice()
    {
        if (_isRouterBackUsed)
        {
            //bz in wheelbarrower the back is use to do the route Source to Destination
            if (ProfDescription == Job.WheelBarrow || ProfDescription == Job.Docker)
            {
                _person.Body.WalkRoutine(_routerBack.TheRoute, HPers.WheelBarrow);
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
            _person.Body.WalkRoutine(_router.TheRoute, HPers.Work, true);
            _workerTask = HPers.DoneAtWork; //so reset the cycle                 
        }
    }

    void ResetMiniMindState()
    {
        _workingNow = false;
        _workerTask = HPers.None;    
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
            _person.transform.LookAt(t);
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
        if (LookAtWork==new Vector3())
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
    public void Execute(string instruct = "")
    {
        if (UPerson.IsWorkingAtSchool(_person))
        {
            return;
        }

        //to address if work place is being destyo on the persons way
        if (_person.Work == null)
        {
            return;
        }

        Produce(instruct);
    }

    private int amtCarrying;
    P prodCarrying = P.None;
    /// <summary>
    /// The action of producing goods 
    /// </summary>
    void Produce()
    {
        _person.Work.Produce(ProdXShift, _person);

        if (_person.Work.CanTakeItOut(_person))
        {
            _person.ExchangeInvetoryItem(_person.Work, _person, _person.Work.CurrentProd, ProdXShift);
            prodCarrying = _person.Work.CurrentProd;
            amtCarrying = ProdXShift;
        }
    }  
    
    /// <summary>
    /// The action of producing goods 
    /// </summary>
    void Produce(string instruct)
    {
        //if has not instructions is normal Produce()
        if (string.IsNullOrEmpty(instruct))
        {
            Produce();
            return;
        }

        //if has instructions is so far from a : Forester
        //they want to physically bring prod back and then drop it at they place 
        //dont want to added to building invetory he has to drop it there when he gets there 
        _person.Work.Produce(ProdXShift, _person, false);
        prodCarrying = _person.Work.CurrentProd;
        amtCarrying = ProdXShift;
    }

    /// <summary>
    /// Will drop goods produced in its respective Work into a Storage 
    /// </summary>
    public void DropGoods()
    {
        if (_person == null || _person.FoodSource == null)
        {
            return;
        }
 
        _person.ExchangeInvetoryItem(_person, _person.FoodSource, prodCarrying, amtCarrying);
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
        DestroyDummy();
    }


    /// <summary>
    /// Created to modularize the case in where is ship wht has to be returned is the Dock, _person.Work
    /// </summary>
    /// <returns></returns>
    public static Structure GetStructureSrcAndDestiny(string id, Person person)
    {
        if (id == "Ship")
        {
            return person.Work;
        }
        return Brain.GetStructureFromKey(id);
    }
}
