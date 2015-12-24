using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

/* To Svae Load anytihng in this class see PersonSaveLoad.cs*/

//This class holds and controlls all persons
public class PersonController : PersonPot
{
	//initiating a game difficulty
    //0 newbie, 1 easy, 2 med, 3 hard, 4 insane
    private int _difficulty = 0;

    private List<Person> _all = new List<Person>();
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

    

    public List<Person> All
    {
        get { return _all; }
        set
        {
            MyText.ManualUpdate();
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


    public StartingCondition CurrentCondition()
    {return Conditions[Difficulty];}

    public static PersonController CreatePersonController(string root, int difficultyP, Transform container = null)
    {
        PersonController obj = null;
        obj = (PersonController)Resources.Load(root, typeof(PersonController));
        obj = (PersonController)Instantiate(obj, new Vector3(), Quaternion.identity);

        obj.Difficulty = difficultyP;

        if (container != null) { obj.transform.parent = container; }
        return obj;
    }

    void Map()
    {
        int multiplier = 1000;
        int factor = 100;
        int ini = multiplier*factor;

        StartingCondition newbie = new StartingCondition(1, ini, ini, ini, ini, ini, ini, 100000);
        StartingCondition easy = new StartingCondition(18, 900, 900, 900, 900, 900, 900, 900);
        StartingCondition med = new StartingCondition(16, 800, 800, 800, 800, 800, 800, 800);
        StartingCondition hard = new StartingCondition(14, 700, 700, 700, 700, 700, 700, 700);
        StartingCondition insane = new StartingCondition(12, 600, 600, 600, 600, 600, 600, 600);

        Conditions = new StartingCondition[] {newbie, easy, med, hard, insane};
    }

    private bool init;
    public void Initialize()
    {
        if (!MeshController.CrystalManager1.IsFullyLoaded())
        {
            return;
        }

        init = false;

        PersonData pData = XMLSerie.ReadXMLPerson();
        //brand new game
        if (pData == null)
        {
            SpawnIniPersonas();
            GameController.LoadStartingConditions(conditions[Difficulty]);
        }
        //loading from file 
        else
        {
            LoadFromFile(pData);
            //game controller is loaded and Saved on BuildingSaveLoad.cs

            //called here for the first time after a Storage was build.
            //This is DEBUG
            GameController.LoadStartingConditions(conditions[Difficulty]);
        }

        //so the loading screen is kill and gui loaded 
        Program.MyScreen1.LoadingScreenIsDone();
    }

    void LoadFromFile(PersonData pData)
    {
        //persons
        for (int i = 0; i < pData.All.Count; i++)
        {
            Person t = Person.CreatePersonFromFile(pData.All[i]);
            All.Add( t);
        }  

        //person controller vars
        Difficulty = pData.PersonControllerSaveLoad.Difficulty;

        UnivCounter = pData.PersonControllerSaveLoad.UnivCounter;
        Queues = pData.PersonControllerSaveLoad.Queues;
        GenderLast = pData.PersonControllerSaveLoad.GenderLast;

        Locked = pData.PersonControllerSaveLoad.Locked;
        BuildersManager1 = pData.PersonControllerSaveLoad.BuildersManager;

        RoutesCache1 = pData.PersonControllerSaveLoad.RoutesCache;

        Waiting = pData.PersonControllerSaveLoad.Waiting;
        OnSystemNow1 = pData.PersonControllerSaveLoad.OnSystemNow1;
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
            All.Add( t);
        }
    }

    public void HaveNewKid(Vector3 iniPos)
    {
        Person t = Person.CreatePersonKid(iniPos);
        All.Add(t); 
    }

    #region MovingToNewHome Related

    //means that a person is moving from one hose to another 
    //if this is on. No One can move 
    //TODO needs to be SaveLoad
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

        StartCoroutine("RandomUpdate1020");
    }

	// Update is called once per frame
	void Update ()
	{
	    Debug();
        Count();
	    UpdateOnScreen();

        _buildersManager.Update();

        CheckIfSystemHasRoom();
        //CheckIfPersonIsBeingOnSystemTooLong();

        if (init)
        {
            Initialize();
        }
	}

    private void UpdateOnScreen()
    {
        if (!GameController.Inventory1.IsItemOnInv(P.Wood))
        {
            return;
        }


        var msg =
            "Person:" + _all.Count + " | " +
            "Food:" + GameController.Inventory1.ReturnAmountOnCategory(PCat.Food) + " | " +
            "Wood:" + GameController.Inventory1.ReturnAmtOfItemOnInv(P.Wood) + " | " +
            "Stone:" + GameController.Inventory1.ReturnAmtOfItemOnInv(P.Stone) + " | " +
            "Brick:" + GameController.Inventory1.ReturnAmtOfItemOnInv(P.Brick) + " | " +
            "Iron:" + GameController.Inventory1.ReturnAmtOfItemOnInv(P.Iron) + " | " +
            "Gold:" + GameController.Inventory1.ReturnAmtOfItemOnInv(P.Gold) + " | " +
            "Dollar:" + GameController.Dollars.ToString("C0", new CultureInfo(0x0816)) + 
            "\nSpeed:" + Program.gameScene.GameSpeed + "x"
                ;

        //CultureInfo
        // https://msdn.microsoft.com/en-us/goglobal/bb896001.aspx

        Program.gameScene.AddToMainScreen(msg);
    }

    void Debug()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {
            foreach (var ite in All)
            {
                //ite.Value.Brain.Router.DebugDestroy();
                ite.Destroy();
                All.Remove(ite);
            }
            SpawnIniPersonas();
        }

        //make sure when execute this a least oneempty house exst 
        if (Input.GetKeyUp(KeyCode.M))
        {
            DebugSpawnMorePeople(25);
        }
        //make sure when execute this a least oneempty house exst 
        if (Input.GetKeyUp(KeyCode.N))
        {

            DebugSpawnMorePeople(10);
        }
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
    Dictionary<string, string> _peopleChecked = new Dictionary<string, string>();



    public void CheckPeopleIn(string newPeople)
    {
        if (!_peopleChecked.ContainsKey(newPeople))
        {
           // print(newPeople+".Checked in");
            _peopleChecked.Add(newPeople, newPeople);
        }
    }

    public bool PeopleHasCheck(string people)
    {
        if (_peopleChecked.ContainsKey(people))
        {
            return true;
        }
        return false;
    }

    public bool IsPeopleCheckFull()
    {
        var t = ThatHasNotChecked();

        if (_peopleChecked.Count >= All.Count)
        {
            return true;
        }
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
                if (All[i].MyId != _peopleChecked.ElementAt(j).Value)
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

    private float random1020Time;
    private IEnumerator RandomUpdate1020()
    {
        while (true)
        {
            yield return new WaitForSeconds(random1020Time); // wait
            random1020Time = Random.Range(5, 10);

            //CheckIfImmigrants();
        }
    }


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
    
    #region People ReRouting System
    //People will reroute if they had not reroute already in this cycle and 
    //if queue has space. Other wise person should wait at home 

    List<string> _waiting = new List<string>();
    private List<CheckedIn> _onSystemNow = new List<CheckedIn>();
    private int _systemCap = 4;//2//4
    private int _allowOnSystem = 8;//seconds

    public List<string> Waiting
    {
        get { return _waiting; }
        set { _waiting = value; }
    }

    public List<CheckedIn> OnSystemNow1
    {
        get { return _onSystemNow; }
        set { _onSystemNow = value; }
    }

  

    public void CheckMeOnSystem(string id)
    {
        _onSystemNow.Add(new CheckedIn(id, Time.time));
    }

    /// <summary>
    /// Will teel u if id is on system list now
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    bool OnSystemNow(string id)
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

    public void AddToWaiting(string id)
    {
        if (!_waiting.Contains(id) && !OnSystemNow(id))
        {
            _waiting.Add(id);
        }
    }

    public void DoneReRoute(string p)
    {
        for (int i = 0; i < _onSystemNow.Count; i++)
        {
            if (_onSystemNow[i].Id == p)
            {
                _onSystemNow.RemoveAt(i);
                _waiting.Remove(p);//in case was there duplicated 
                return;
            }
        }
        CheckIfSystemHasRoom();
    }

    /// <summary>
    /// If system Has room will pull person from _waiting list
    /// </summary>
    private void CheckIfSystemHasRoom()
    {
        if (_onSystemNow.Count < _systemCap)
        {
            for (int i = 0; i < _waiting.Count; i++)
            {
                if (i < _systemCap && _onSystemNow.Count < _systemCap)
                {
                    CheckMeOnSystem(_waiting[i]);
                    PersonTurn(_waiting[i]);

                    if (i >= _waiting.Count)
                    {
                        return;
                    }

                    _waiting.RemoveAt(i);
                    i--;
                }
                else return;
            }
        }
    }

    ///// <summary>
    ///// Will be taken out and put inwaiting list. to address person taking forever to ReDoRoutes
    ///// </summary>
    //void CheckIfPersonIsBeingOnSystemTooLong()
    //{
    //    for (int i = 0; i < _onSystemNow.Count; i++)
    //    {
    //        if (Time.time > _onSystemNow[i].Time  +_allowOnSystem)
    //        {
    //            var idP = _onSystemNow[i].Id;
    //            var pers = Family.FindPerson(idP);

    //            if (!pers.Brain.IAmHomeNow())
    //            {
    //                _onSystemNow.RemoveAt(i);
    //                Family.FindPerson(idP).Brain.PlaceMeOnWaiting();
    //            }

    //            return;
    //        }
    //    }
    //}


    /// <summary>
    /// Will call the Person.Brain to Reroute 
    /// </summary>
    /// <param name="idP"></param>
    void PersonTurn(string idP)
    {
        var pers = Family.FindPerson(idP);

        if (pers!=null)
        {
            pers.Brain.YourTurnToReRoute();
        }
    }

    internal bool CanIReRouteNow(string p)
    {
        return OnSystemNow(p);
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
    public int iniGold, iniDollar;

    public StartingCondition(int iniPersonP, int iniWoodP, int iniFoodP, int iniStoneP, int iniBrickP, int iniIronP,
        int iniGoldP, int iniDollarP)
    {
        iniPerson = iniPersonP;
        iniWood = iniWoodP;
        iniFood = iniFoodP;
        iniStone = iniStoneP;
        iniBrick = iniBrickP;
        iniIron = iniIronP;
        iniGold = iniGoldP;
        iniDollar = iniDollarP;
    }
}

/// <summary>
/// Use to Save and Load PersonController vars 
/// </summary>
public class PersonControllerSaveLoad
{
    //0 newbie, 1 easy, 2 med, 3 hard, 4 insane
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
    public List<CheckedIn> OnSystemNow1 = new List<CheckedIn>();

}
