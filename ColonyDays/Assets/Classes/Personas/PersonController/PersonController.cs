using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/* To Svae Load anytihng in this class see PersonSaveLoad.cs*/

//This class holds and controlls all persons
public class PersonController : PersonPot
{





	//initiating a game difficulty
    //0 insane, 4 newbie 
    private int _difficulty = 0;

    private List<Person> _all = new List<Person>();
    //same as above for GC reasons 
    Dictionary<string, Person> _allGC = new Dictionary<string, Person>(); 

    private StartingCondition[] conditions;

    //the counter to do the brainChecks.. BrainCheck is when looking to see if somethinglike a new Job is open
    private static int _univCounter=-1;

    private QueuesContainer _queues = new QueuesContainer();
	//the last gender person created 
    public static H GenderLast = H.Male;

    private bool _locked;//if is locked cant do CheckBrain

    //contains functionalities to manager the builders 
    BuildersManager _buildersManager = new BuildersManager();

    RoutesCache _routesCache = new RoutesCache();

    PeopleQueue _workersRoutingQueue=new PeopleQueue();

    EmigrateController _emigrateController = new EmigrateController();
    public EmigrateController EmigrateController1
    {
        get { return _emigrateController; }
        set { _emigrateController = value; }
    }

    public PeopleQueue WorkersRoutingQueue
    {
        get { return _workersRoutingQueue; }
        set { _workersRoutingQueue = value; }
    }

    public List<Person> All
    {
        get { return _all; }
        set
        {
            _all = value;
        }
    }

    public RoutesCache RoutesCache1
    {
        get { return _routesCache; }
        set { _routesCache = value; }
    }

    public BuildersManager BuildersManager1
    {
        get { return _buildersManager; }
        set { _buildersManager = value; }
    }

    public bool Locked
    {
        get { return _locked; }
        set { _locked = value; }
    }

    /// <summary>
    /// 
    ///game difficulty
    ///0 newbie, 1 easy, 2 med, 3 hard, 4 insane
    /// </summary>
    public int Difficulty
    {
        get { return _difficulty; }
        set { _difficulty = value; }
    }

    public static int UnivCounter
    {
        get { return _univCounter; }
        set { _univCounter = value; }
    }

    public StartingCondition[] Conditions
    {
        get { return conditions; }
        set { conditions = value; }
    }

    public QueuesContainer Queues
    {
        get { return _queues; }
        set { _queues = value; }
    }

    /// <summary>
    /// The current game Difficulty we are playing
    /// </summary>
    /// <returns></returns>
    public StartingCondition CurrentCondition()
    {return Conditions[Difficulty];}

    public static PersonController CreatePersonController(string root, int difficultyP, Transform container = null)
    {
        PersonController obj = null;
        obj = (PersonController)Resources.Load(root, typeof(PersonController));
        obj = (PersonController)Instantiate(obj, new Vector3(), Quaternion.identity);

        obj.Difficulty = difficultyP;

        if (container != null) { obj.transform.SetParent( container); }
        return obj;
    }

    void Map()
    {
        int multiplier = 100;
        int factor = 100;
        int ini = multiplier*factor;

        StartingCondition insane = new StartingCondition(4, 600, 600, 600, 600, 600, 600, 10000, 1 * factor, 1);
        StartingCondition hard = new StartingCondition(5, 700, 700, 700, 700, 700, 700, 20000, 2 * factor, 2);
        StartingCondition med = new StartingCondition(6, 800, 800, 800, 800, 800, 800, 30000, 3 * factor, 3);
        StartingCondition easy = new StartingCondition(7, 900, 900, 900, 900, 900, 900, 40000, 4 * factor, 4);
        StartingCondition newbie = new StartingCondition(8, ini, ini, ini, ini, ini, ini, 100000, 5 * factor, 5);

        Conditions = new StartingCondition[] { insane, hard, med, easy, newbie, };
    }

    private bool init;
    public void Initialize()
    {
        if (!MeshController.CrystalManager1.IsFullyLoaded() || !Program.gameScene.GameFullyLoaded())//making sure all build are fullt loaded
        {
            return;
        }
        init = false;

        PersonData pData = XMLSerie.ReadXMLPerson();
        CamControl.CAMRTS.InputRts.SaveFirstCamPos();

        //brand new game
        if (pData == null)
        {
            SpawnIniPersonas();
            GameController.LoadStartingConditions(conditions[Difficulty]);
            RestartController();
            
            //here bz trees all spawneed after buildings. 
            //so if tree is inside a building wont be deleted. bz the building
            //might be to big and wont collide with the building 
            BuildingPot.Control.Registro.DoLastStepOfTownLoaded();
        }
        //loading from file game
        else
        {
            LoadFromFile(pData);
            //game controller is loaded and Saved on BuildingSaveLoad.cs

            //called here for the first time after a Storage was build.
            //This is DEBUG
            GameController.LoadStartingConditions(conditions[Difficulty]);
            CamControl.CAMRTS.InputRts.LoadLastCamPos();
        }
        BuildingPot.CreateUnlockBuilds();
        MeshController.InitBuyRegions();

        Program.gameScene.GameController1.ReCheckWhatsOnStorage();
        Program.gameScene.ReleaseLoadingScreen();
    }


    /// <summary>
    /// Loads Person controller
    /// </summary>
    /// <param name="pData"></param>
    void LoadFromFile(PersonData pData)
    {
        //person controller vars
        Difficulty = pData.PersonControllerSaveLoad.Difficulty;

        UnivCounter = pData.PersonControllerSaveLoad.UnivCounter;
        Queues = pData.PersonControllerSaveLoad.Queues;
        GenderLast = pData.PersonControllerSaveLoad.GenderLast;

        Locked = pData.PersonControllerSaveLoad.Locked;
        BuildersManager1 = pData.PersonControllerSaveLoad.BuildersManager;

        RoutesCache1 = pData.PersonControllerSaveLoad.RoutesCache;
        RoutesCache1.LoadTheSave();

        //OnSystemNow1 = pData.PersonControllerSaveLoad.OnSystemNow1;
        EmigrateController1 = pData.PersonControllerSaveLoad.EmigrateController1;
        EmigrateController1.RecreateEmigratesGC();

        IsAPersonHomeLessNow = pData.PersonControllerSaveLoad.IsAPersonHomeLessNow;

        Program.MyScreen1.TownName = pData.PersonControllerSaveLoad.TownName;

        BulletinWindow.SubBulletinProduction1 = pData.PersonControllerSaveLoad.SubBulletinProduction;

        //persons
        for (int i = 0; i < pData.All.Count; i++)
        {
            Person t = Person.CreatePersonFromFile(pData.All[i]);
            MouseClick += t.MouseClickHandler;

            All.Add(t);
            _allGC.Add(t.MyId, t);
        }  
    }

    private Person tempPerson;
    public void SpawnIniPersonas(int amtP = 0, Vector3 iniPos = new Vector3())
    {
        if (amtP == 0)
        {
            amtP = Conditions[Difficulty].iniPerson;
        }

        for (int i = 0; i < amtP; i++)
        {
            Person t = Person.CreatePerson(iniPos);
            MouseClick += t.MouseClickHandler;
            All.Add( t);
            _allGC.Add(t.MyId, t);
        }
    }






    public void HaveNewKid(Vector3 iniPos)
    {
        Person t = Person.CreatePersonKid(iniPos);
        MouseClick += t.MouseClickHandler;

        All.Add(t);
        _allGC.Add(t.MyId, t);
    }



#region events

    public EventHandler<EventArgs> MouseClick;

    void OnMouseClick(EventArgs e)
    {
        if (MouseClick != null)
        {
            MouseClick(this, e);
        }
    }

#endregion





    public void RemovePerson(Person p)
    {
        _allGC.Remove(p.MyId);
        All.Remove(p);
    }

    internal Person FindPerson(string myIdP)
    {
        if (_allGC.ContainsKey(myIdP))
        {
            return _allGC[myIdP];
        }
        return null;
    }

    #region MovingToNewHome Related

    //means that a person is moving from one hose to another 
    //if this is on. No One can move 
    private string _isAPersonHomeLessNow;

    public string IsAPersonHomeLessNow
    {
        get { return _isAPersonHomeLessNow; }
        set { _isAPersonHomeLessNow = value; }
    }

    /// <summary>
    /// Will clean the homeless slot if the person asking for it is the one that
    /// ocupied 
    /// </summary>
    /// <param name="personId"></param>
    public void CleanHomeLessSlot(string personId)
    {
        if (personId == _isAPersonHomeLessNow)
        {
           //Debug.Log(personId + " clean homless now:");


            _isAPersonHomeLessNow = "";
        }
    }

    #endregion

    void Start()
    {
        Map();

        init = true;
        //Initialize();

        UVisHelp.CreateHelpers(Program.gameScene.controllerMain.MeshController.wholeMalla, Root.redSphereHelp);

        StartCoroutine("A2sUpdate");

    }

	// Update is called once per frame
	void Update ()
	{
	    DebugHere();
        Count();

        //needs to be here for efficiency as people wont add routes to it if is bein checkked
        //agaisnt new queues (new buildings added) here will get checked really fast.
        //So people will use Cache routes more and buidings will get Greenlit faster too. 
        RoutesCache1.Update();

        if (init)
        {
            Initialize();
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            OnMouseClick(EventArgs.Empty);
        }
	}


    private IEnumerator A2sUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.33f); // wait
            
            _buildersManager.Update();
            WorkersRoutingQueue.Update();
            SanitizeCurrent();
            EmigrateController1.Update();
        }
    }



    private void UpdateOnScreen()
    {
        if (!GameController.ResumenInventory1.IsItemOnInv(P.Wood))
        {
            return;
        }


        var msg =
            "Person:" + _all.Count + " | " +
            "Food:" + GameController.ResumenInventory1.ReturnAmountOnCategory(PCat.Food) + " | " +
            "Wood:" + GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Wood) + " | " +
            "Stone:" + GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Stone) + " | " +
            "Brick:" + GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Brick) + " | " +
            "Iron:" + GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Iron) + " | " +
            "Gold:" + GameController.ResumenInventory1.ReturnAmtOfItemOnInv(P.Gold) + " | " +
            "Dollar:" + Program.gameScene.GameController1.Dollars.ToString("C0", new CultureInfo(0x0816)) + 
            "\nSpeed:" + Program.gameScene.GameSpeed + "x"
                ;

        //CultureInfo
        // https://msdn.microsoft.com/en-us/goglobal/bb896001.aspx

        Program.gameScene.AddToMainScreen(msg);
    }

    void DebugHere()
    {
        if (!Developer.IsDev)
        {
            return;
        }

        //make sure when execute this a least oneempty house exst 
        if (Input.GetKeyUp(KeyCode.M))
        {
            DebugSpawnMorePeople(5 * AmtOfPeople());
        }
        //make sure when execute this a least oneempty house exst 
        if (Input.GetKeyUp(KeyCode.N))
        {
            DebugSpawnMorePeople(1 * AmtOfPeople());
        }
    }

    int AmtOfPeople()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt))
        {
            return 20;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            return 5;
        }
        return 1;
    }

    void DebugSpawnMorePeople(int amt, Vector3 iniPos = new Vector3())
    {
        //so acts like immigration arruve to scene
        //ShacksManager.Wave = true;   

        //so people can look for houses and stuff
        RestartController();
        BuildingPot.Control.IsNewHouseSpace = true;//this line will lead to bugg if not empty houses exits

        SpawnIniPersonas(amt, iniPos);
    }

    #region People Check

    //the people had check for current new stuff. like new house or work
    List<string> _peopleChecked = new List<string>();



    public void CheckPeopleIn(string newPeople)
    {
        if (!_peopleChecked.Contains(newPeople))
        {
           // print(newPeople+".Checked in");
            _peopleChecked.Add(newPeople);
        }
    }

    public bool PeopleHasCheck(string people)
    {
        if (_peopleChecked.Contains(people))
        {
            return true;
        }
        return false;
    }

    public bool IsPeopleCheckFull()
    {
        //needs to be the same otherwise Will left people 
        //without checking out
        if (_peopleChecked.Count >= All.Count)
        {
            return true;
        }

        //if (_peopleChecked.Count > All.Count)
        //{
        //    _peopleChecked.Clear();
        //}

        return false;
    }

    List<Person> ThatHasNotChecked()
    {
        List<Person> res = new List<Person>();

        for (int i = 0; i < All.Count; i++)
        {
            int personCount = 0;
            for (int j = 0; j < _peopleChecked.Count; j++)
            {
                if (All[i].MyId != _peopleChecked[i])
                {
                    personCount++;
                }

                if (personCount == _peopleChecked.Count)
                {
                    res.Add(All[i]);
                }
            }
        }
        return res;
    }

    /// <summary>
    /// Will set the controoler to Restart
    /// 
    /// makes:
    /// _univCounter = 0
    /// and Clears PeopleCheck List 
    /// </summary>
    public void RestartController()
    {
        _univCounter = 0;
        ClearPeopleCheck();
    }

    /// <summary>
    /// Will restar controller only for the persn. The person will be removed from the Checked People List
    /// </summary>
    public void RestartControllerForPerson(string p)
    {
        _univCounter = 0;
        _peopleChecked.Remove(p);
    }

    public void ClearPeopleCheck()
    {
        _peopleChecked.Clear();
    }

    /// <summary>
    /// call when person die
    /// </summary>
    /// <param name="p"></param>
    public void RemovePersonFromPeopleChecked(string p)
    {
        _peopleChecked.Remove(p);
    }

    #endregion
    
    #region CoolDown 

    //the amount of cooldown has to be wait to be able to a person check again 
    //.. measured on FixedUpdate Frames
    private int currentCoolDown;
    //the person locking this so cant be used by him again.. the person needs to 
    //be able to continue doing all so it doesnt get out of the building first 
    //without reachiung the new one for ex 
    private string whoLocked;
    //saids who had locked this already.. will get clear once all had lockeds once
    private Dictionary<string, string> peopleLocked = new Dictionary<string, string>(); 
    public bool IsGoodToCheck()
    {
        if (currentCoolDown > 0)
        {return false;}
        return true;
    }

    /// <summary>
    /// Will add more to the cool down. So the person has to wait more to check here 
    /// </summary>
    /// <param name="time">Time added to cooldown</param>
    public void AddToCoolDown(int time)
    {
        currentCoolDown += time;
    }

    void RemoveCurrentCoolDown()
    {
        if (currentCoolDown == 0)
        {
            whoLocked = "";//so can be locked by someone else
            return;
        }
        if (currentCoolDown < 0)
        {
            whoLocked = "";
            currentCoolDown =0;
            return;
        }
        currentCoolDown -= 1;
    }

    internal bool DidILocked(string p)
    {
        if (whoLocked == p)
        {
            return true;
        }
        return false;
    }

    #endregion
    
    #region Local Counter Will be Looping all the time so a person will check stuff at the time

    private int _peopleCounter;
    public int PeopleCounter
    {
        get { return _peopleCounter; }
    }

   


    void Count()
    {
        _peopleCounter++;
        if (PeopleCounter == All.Count)
        { _peopleCounter = 0; }
    }

    #endregion

    #region Immigrants




    private int debugCount ;
    /// <summary>
    /// Will emmigrate people to the Town Randomly. If town has at least one Dock 
    /// </summary>
    void CheckIfImmigrants()
    {
        if (debugCount > 2)
        {
            return;
        }

        var count = BuildingController.HowManyOfThisTypeAre(H.Dock);

        if (OverAllHappinesIsOk() && OverAllFoodIsOk() && ProsperitySense() &&  count > 0)
        {
            ImmigrateSome();
            debugCount++;
        }
    }

    /// <summary>
    /// The action of immigrating some people 
    /// </summary>
    private void ImmigrateSome()
    {
        var rand = Random.Range(5, 9);

        var dock = BuildingPot.Control.FindRandomBuildingOfThisType(H.Dock);
        var dockST = (Structure) dock;

        DebugSpawnMorePeople(rand, dockST.SpawnPoint.transform.position);
    }

    private bool OverAllHappinesIsOk()
    {
        return true;
    }

    private bool OverAllFoodIsOk()
    {
        return true;

    }

    private bool ProsperitySense()
    {
        return true;
    }

    #endregion

    private bool wasFullyLoaded;
    /// <summary>
    /// Will tell u if this Controller is fully loaded 
    /// 
    /// If you put speed on game and people still routing will give exception 
    /// </summary>
    /// <returns></returns>
    public bool IsFullyLoaded()
    {
        //so it doesnt stop in Game when people is ReRouting 
        if (wasFullyLoaded)
        {
            return true;
        }

        wasFullyLoaded =_onSystemNow.Count == 0 && _waitList.Count == 0
               && _workersRoutingQueue.OnSystemNow1.Count == 0 && _workersRoutingQueue.WaitList.Count == 0;

        return wasFullyLoaded;
    }
    
    #region People ReRouting System
    //People will reroute if they had not reroute already in this cycle and 
    //if queue has space. Other wise person should wait at home 

    private List<CheckedIn> _onSystemNow = new List<CheckedIn>();
    
    //the number is not inclusinve so if u put a 3 will alow 2
    private int _systemCap = 7;//5   2   4  //amt of person

    //people waiting to be pass to _onSystemNow
    List<CheckedIn>  _waitList = new List<CheckedIn>();
    
    /// <summary>
    /// This doesnt need to be SaveLoad. Will give probl
    /// </summary>
    public List<CheckedIn> OnSystemNow1
    {
        get { return _onSystemNow; }
        set { _onSystemNow = value; }
    }

    public List<CheckedIn> WaitList
    {
        get { return _waitList; }
        set { _waitList = value; }
    }




    public void CheckMeOnSystem(string id)
    {
        var find = _onSystemNow.Find(a => a.Id == id);

        if (find != null)
        {
            //was checked in already
            return;
        }

        _onSystemNow.Add(new CheckedIn(id, Time.time));
    }

    /// <summary>
    /// Will teel u if id is on system list now
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool OnSystemNow(string id)
    {
        for (int i = 0; i < _onSystemNow.Count; i++)
        {
            //time is up
            if (id == _onSystemNow[i].Id)
            {
                return true;
            }
        }
        return false;
    }

    public void DoneReRoute(string p)
    {
        for (int i = 0; i < _onSystemNow.Count; i++)
        {
            if (_onSystemNow[i].Id == p)
            {
                _onSystemNow.RemoveAt(i);
                TransferFirstInWaitingListToOnSystemNow();
                return;
            }
        }
    }

    /// <summary>
    /// Will tell u if can reRoute.
    /// </summary>
    /// <param name="pMyID"></param>
    /// <returns></returns>
    internal bool CanIReRouteNow(string pMyID)
    {
        //bz if he checked then dont need to try to get into system again
        return OnSystemNow1.Count < _systemCap && !PeopleHasCheck(pMyID);
    }

    internal bool AddMeToOnSystemWaitList(string id)
    {
        if (IAmOnSystemNow(id))
        {
            return false;
        }

        if (//WaitList.Count <= WaitingListCap() && 
            !PeopleHasCheck(id))
        {
           //Debug.Log("added to wait list:" + id);
            WaitList.Add(new CheckedIn(id, Time.time));
            return true;
        }
        return false;
    }

    int WaitingListCap()
    {
        var res = All.Count/5;

        if (res > 1)
        {
            return res;
        }
        return 1;
    }

    /// <summary>
    /// Called when DoneReRoute() is called 
    /// </summary>
    void TransferFirstInWaitingListToOnSystemNow()
    {
        if (WaitList.Count==0)
        {
            return;
        }


        var t = WaitList[0];
        WaitList.RemoveAt(0);
        OnSystemNow1.Add(t);

       //Debug.Log("transfer to System:"+t.Id);
    }

    internal bool OnWaitListNow(string id)
    {
        var find = WaitList.Find(a => a.Id == id);

        if (find != null)
        {
            //was added in already
            return true;
        }
        return false;
    }

    /// <summary>
    /// To bne call when person dies 
    /// </summary>
    /// <param name="id"></param>
    public void RemoveMeFromSystem(string id)
    {
        var wIndex = WaitList.FindIndex(a => a.Id == id);
        if (wIndex > -1)
        {
           //Debug.Log("remove from waitL:"+id);
            WaitList.RemoveAt(wIndex);
        }

        var sIndex = OnSystemNow1.FindIndex(a => a.Id == id);
        if (sIndex > -1)
        {
           //Debug.Log("remove from systemNow:" + id);
            OnSystemNow1.RemoveAt(sIndex);    
        }
    }

    /// <summary>
    /// Either on WaitList or SystemNow1
    /// </summary>
    /// <returns></returns>
    public bool IAmOnSystemNow(string id)
    {
        return OnSystemNow(id) || OnWaitListNow(id);
    }

    void SanitizeCurrent()
    {
        if (OnSystemNow1.Count==0)
        {
            return;
        }

        var p = OnSystemNow1[0];

        //if is being there for 10 sec we need to check 
        if(Time.time > p.Time + 10f)
        {
            if (OnSystemNow1.Contains(p) && Family.FindPerson(p.Id) == null)
            {
               //Debug.Log("remove bz was gone OnSystemNow1:" + p.Id);
                OnSystemNow1.Remove(p);
                TransferFirstInWaitingListToOnSystemNow();
            }
            if (WaitList.Contains(p) && Family.FindPerson(p.Id) == null)
            {
               //Debug.Log("remove bz was gone WaitList:" + p.Id);
                WaitList.Remove(p);
            }
        }
    }

#endregion
    
    /// <summary>
    /// Average of overall happiness
    /// </summary>
    /// <returns></returns>
    internal string OverAllHappiness()
    {
        double total = 0;

        for (int i = 0; i < _all.Count; i++)
        {
            total += _all[i].Happinnes;
        }

        return (total/_all.Count).ToString("n2") + " / 5";
    }

    public void SetIsBookedToPerson(string id, string isBooked)
    {
        var index = All.FindIndex(a => a.MyId == id);
        if (index == -1)
        {
            return;
        }
        //Debug.Log(string.Format("Booking: {0} to: {1}", id, isBooked));
        All[index].IsBooked = isBooked;
    }

    public void SetFamIDToPerson(string id, string famId)
    {
        var index = All.FindIndex(a => a.MyId == id);
        if (index == -1)
        {
            return;
        }
        All[index].FamilyId = famId;
    }






    /// <summary>
    /// Called when new game is gonna be created 
    /// </summary>
    internal void ClearAll()
    {
        for (int i = 0; i < _all.Count; i++)
        {
            _all[i].Destroy();
        }

        _all.Clear();
        _allGC.Clear();
        _onSystemNow.Clear();
    }
}

public class CheckedIn
{
    public string Id;
    public float Time;//the time checked in 

    public CheckedIn() { }

    public CheckedIn(string id, float time)
    {
        Id = id;
        Time = time;
    }
}



public class StartingCondition
{
    //the initial amount of person when start a brand new game
    public int iniPerson;
    public int iniWood;
    public int iniFood;
    public int iniStone;
    public int iniBrick;
    public int iniIron;
    //if adding a new prod for initial val also add in GameController.CreateInitialInv()
    public int iniGold, iniDollar, iniWheelBarrow, iniTool, iniCrate, iniCart;

    private int _thisDifficultyLevel;//    //0 insane, 4 newbie 


    public StartingCondition(int iniPersonP, int iniWoodP, int iniFoodP, int iniStoneP, int iniBrickP, int iniIronP,
        int iniGoldP, int iniDollarP, int iniWheelBarrowP, int thisDifficultyLevel)
    {
        iniPerson = iniPersonP;
        iniWood = iniWoodP;
        iniFood = iniFoodP;
        iniStone = iniStoneP;
        iniBrick = iniBrickP;
        iniIron = iniIronP;
        iniGold = iniGoldP;
        iniDollar = iniDollarP;
        iniWheelBarrow = iniWheelBarrowP;
        iniTool = iniWheelBarrowP/2;
        iniCrate = iniWheelBarrowP/2;
        iniCart = iniWheelBarrowP/2;
        _thisDifficultyLevel = thisDifficultyLevel;
    }
}

/// <summary>
/// Use to Save and Load PersonController vars 
/// </summary>
public class PersonControllerSaveLoad
{
    //    //0 insane, 4 newbie 
    public int Difficulty;

    //the counter to do the brainChecks.. BrainCheck is when looking to see if somethinglike a new Job is open
    public int UnivCounter;

    public QueuesContainer Queues = new QueuesContainer();
    //the last gender person created 
    public H GenderLast ;

    public bool Locked;//if is locked cant do CheckBrain

    //contains functionalities to manager the builders 
    public BuildersManager BuildersManager = new BuildersManager();

    public RoutesCache RoutesCache = new RoutesCache();

    public List<string> Waiting = new List<string>();
    //public List<CheckedIn> OnSystemNow1 = new List<CheckedIn>();


    public EmigrateController EmigrateController1 = new EmigrateController();

    public string IsAPersonHomeLessNow = "";

    public string GameVersion = "";

    public string TerrainName = "";


    public List<int> UnlockRegions = new List<int>();


    public string TownName { get; set; }

    public SubBulletinProduction SubBulletinProduction { get; set; }
}
