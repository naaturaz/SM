using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Facebook.Unity;
using Steamworks;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class GameScene : General
{

    float _gameLoadedTime;//when this game fullyLoaded 
    public Terreno Terreno;

    private static Btn3D textMessage;
    public static string ScreenMsg;

    public ControllerMain controllerMain;
    private General _waterBody;

    private Book _book = new Book(); //keeps all the caps for each object of the game 

    //the scale of the small units in a farm
    private Vector3 _subDivideBlockScale = new Vector3();

    //how thick a block of way is
    private float _subDivideBlockYVal = 0.05f; //0.001f;
    private int _gameSpeed = 1; //the speed of the whole Game

    public static General dummyBlue; //for help everywhere()
    public static General dummyRed; //for help everywhere()

    public static Structure dummySpawnPoint; //for help everywhere()
    private List<Structure> dummiesSpwnPoint = new List<Structure>(); //for help everywhere()

    //load save in BuildingSaveLoad.cs
    private GameTime _gameTime = new GameTime();
    //people will grow ard 3x faster than anything else bz game is boring like is now 
    private GameTime _gameTimePeople;
    
    
    private GameController _gameController = new GameController();
    private ExportImport _exportImport = new ExportImport();


    private BatchManager _batchManager;
    private Culling _culling;
    private Fustrum _fustrum;
    private StaticBatch _staticBatch;
    private MeshBatch _meshBatch;

    private AudioPlayer _audioPlayer;


    public float SubDivideBlockYVal
    {
        get { return _subDivideBlockYVal; }
        set { _subDivideBlockYVal = value; }
    }

    private bool isScaleSmallRoadUnitDefined;

    public Vector3 ScaleSmallRoadUnitFarm
    {
        get
        {
            if (!isScaleSmallRoadUnitDefined)
            {
                if (_subDivideBlockScale.x < m.SubDivide.XSubStep)
                {
                    DefineSubDivideBlockScaleForFarm();
                    isScaleSmallRoadUnitDefined = true;
                }
            }
            return _subDivideBlockScale;
        }
        set { _subDivideBlockScale = value; }
    }

    public General WaterBody
    {
        get { return _waterBody; }
    }

    public Book Book
    {
        get { return _book; }
        set { _book = value; }
    }

    public int GameSpeed
    {
        get { return _gameSpeed; }
        set { _gameSpeed = value; }
    }

    public GameTime GameTime1
    {
        get { return _gameTime; }
        set { _gameTime = value; }
    }

    public GameController GameController1
    {
        get { return _gameController; }
        set { _gameController = value; }
    }

    public ExportImport ExportImport1
    {
        get { return _exportImport; }
        set { _exportImport = value; }
    }

    public Culling Culling1
    {
        get { return _culling; }
        set { _culling = value; }
    }

    public Fustrum Fustrum1
    {
        get { return _fustrum; }
        set { _fustrum = value; }
    }


#region SaveLoad Game General

    private string _gameVersion = "";
    public string GameVersion
    {
        get { return _gameVersion; }
        set { _gameVersion = value; }
    }

    public GameTime GameTimePeople
    {
        get { return _gameTimePeople; }
        set { _gameTimePeople = value; }
    }



    private void ProgramDataInit()
    {
        //only gets created if in Editor
#if UNITY_EDITOR
        XMLSerie.WriteXMLProgram(CreateProgramDataObjOrUpdate());
#endif
    }


    ProgramData CreateProgramDataObjOrUpdate()
    {
        //reads the Program.xls
        var pData = XMLSerie.ReadXMLProgram();

        if (pData == null)
        {
            ProgramData p = new ProgramData(Version());
            return p;
        }

        pData.GameVersion = Version();
        return pData;
    }

    /// <summary>
    /// version on botton of the game like 0.0.0.16.05.22
    /// </summary>
    /// <returns></returns>
    string Version()
    {
        if (Developer.IsDev)
        {
            return "Developer Version.";
        }

        var discl = 
            //"Legal: This is a Non Diclosure Agreement. By playing this game you " +
            //        "agree on not release, share or send any media about the game, nor talk about it. " +
            //        "You can not share any information about this game. " +
            //        "Thanks for your help. "  +
                    "Aatlantis Code Copyright 2016."
                    //+"Not for distribution, nor publicity. \n"
                    ;

        return discl +
               " Early Access \n v0.1.0." +
               //"Closed Beta \n v0.0.1." + 
               TimeStamp();
    }


    public static string TimeStamp()
    {
        return
            DateTime.Now.Year.ToString().Substring(2) + "." +
            AddZeroInFrontIfOneChar(DateTime.Now.Month) + "." +
            AddZeroInFrontIfOneChar(DateTime.Now.Day) + "d." +
            AddZeroInFrontIfOneChar(DateTime.Now.Hour) + "." +
            AddZeroInFrontIfOneChar(DateTime.Now.Minute) + "." +
            AddZeroInFrontIfOneChar(DateTime.Now.Second);
    }

    static string AddZeroInFrontIfOneChar(int v)
    {
        if (v.ToString().Length == 1)
        {
            return "0" + v;
        }
        return v+"";
    }




#endregion


    // Use this for initialization
    private void Start()
    {
        FB.Init();
//#if UNITY_EDITOR
//        Developer.IsDev = true;
//#endif

        Book.Start();

        LoadTerrain();

        GameController1.Start();
        StartCoroutine("SixtySecUpdate");

        StartCoroutine("OneSecUpdate");


        //Settings.PlayMusic();

        textMessage = (Btn3D) General.Create(Root.menusTextMiddle, new Vector3(0.85f, 0.3f, 0));
        textMessage.MoveSpeed = 40f; //so fade happens
        textMessage.FadeDirection = "FadeIn";

        dummyBlue = General.Create(Root.blueCubeBig, new Vector3());
        dummyRed = General.Create(Root.redCube, new Vector3());


        createDummySpawn = true;

        dummySpawnPoint = (Structure) Building.CreateBuild(Root.dummyBuildWithSpawnPointUnTimed, new Vector3(), H.Dummy,
            container: Program.ClassContainer.transform);

        //hudColor = textMessage.GetComponent<GUIText>().color;


        ProgramDataInit();

        if (GameTimePeople == null)
        {
            GameTimePeople = new GameTime(3f);
        }

    }

   


    #region BatchManager

    /// <summary>
    /// Is only gonna be called when CrystalManager is loaded 
    /// other wise Im creating a new Terrain 
    /// </summary>
    public void BatchManagerCreate()
    {
        _batchManager = new BatchManager("Semi");
    }

    /// <summary>
    /// call when all buildigns are loaded and 
    /// when all Spwaners are loaded.
    /// 
    /// Will pass only if both are done 
    /// </summary>
    public void BatchInitial()
    {
        //in case is not fully loaded the Spwaners or the buildings are not fully loaded
        if (!GameFullyLoaded())
        {
            return;
        }

        Debug.Log("BatchInitial() gameScene");
        _batchManager.BatchInitial();

    }

    /// <summary>
    /// call when all buildigns are loaded and 
    /// when all Spwaners are loaded.
    /// 
    /// Will pass only if both are done 
    /// 
    /// Called directly too when Spawners are rehuse 
    /// </summary>
    public void ReleaseLoadingScreen()
    {
        if (!GameFullyLoaded())
        {
            return;
        }
        //so the loading screen is kill and gui loaded 
        Program.MyScreen1.LoadingScreenIsDone();
        //so its loaded to the right Screen resolution 
        Program.MouseListener.ApplyChangeScreenResolution();

        RedoStuffWithLoadedData();
    }




    #region Tutorial
    private TutoWindow _tutoWindow;

    public void TutoStepCompleted(string step)
    {
        if (_tutoWindow == null)
        {
            _tutoWindow = FindObjectOfType<TutoWindow>();
        }

        if (_tutoWindow == null)
        {
            return;
        }

        _tutoWindow.Next(step);
    }

    public bool IsPassingTheTutoNow()
    {
        return _tutoWindow.IsPassingTheTutoNow();
    }

    #endregion


    /// <summary>
    /// Add the Object to the BatchMesh
    /// </summary>
    /// <param name="gen"></param>
    public void BatchAdd(General gen)
    {
        _batchManager.AddGen(gen);
    }

    internal void BatchRemove(General gen)
    {
        _batchManager.RemoveGen(gen);

    }

    /// <summary>
    /// Will not redo the region again
    /// </summary>
    /// <param name="stillElement"></param>
    internal void BatchRemoveNotRedo(General gen)
    {
        _batchManager.RemoveGen(gen, false);

    }

#endregion

    private IEnumerator SixtySecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(60); // wait
            GameController1.ReCheckWhatsOnStorage();
        }
    }


    private IEnumerator OneSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // wait
            GameController1.UpdateOneSecond();
            DataController.Update();

            HUDFPS.Message = " | " + MouseInBorderRTS.GlobalDir.ToString() + "| Dragged: " + Way._dir +
                             " | InputMode: " + BuildingPot.InputMode + "\n" + more + "\n"
                             + AddPersonControllerInfo() + " | " + AddLoadingInfo()
                //  +               Pull AddUnityStats()
                             + AddCachedReoutesCount();
        }
    }



    /// <summary>
    /// Defines the sub division block scale value this has to be executed after Mesh is submeshed
    /// </summary>
    public void DefineSubDivideBlockScaleForFarm()
    {
        CreatePlane cP = new CreatePlane();
        //the rectify are added so it looks seamles on terrain the farm
        _subDivideBlockScale.x = m.SubDivide.XSubStep + cP.RectifyOnX * 2 ;
        _subDivideBlockScale.y = _subDivideBlockYVal - 0.01f;//so it doesnt overlap ways like trail
        _subDivideBlockScale.z = Mathf.Abs(m.SubDivide.ZSubStep) + cP.RectifyOnZ * 2;
    }

    private bool audioWas;
	// Update is called once per frame
    void Update()
    {
        //means tht it has it loaded
        if (_gameTime == null)
        {
            return;
        }
        
        AudioCollector.Update();
        CreateDummySpawnPoint();
        
        DebugInput();
        DebugChangeScreenResolution();

        if (Camera.main != null && _culling == null)
        {
            //bz camera needs to be initiated already
            _culling = new Culling();
            _fustrum = new Fustrum();
        }

        if (Camera.main != null && _audioPlayer == null && !audioWas)
        {
            audioWas = true;
            //bz camera needs to be initiated already
            _audioPlayer = new AudioPlayer();
        }

        if (_fustrum!=null)
        {
            _fustrum.Update();
        }

        if (hud==null)
        {
            var hudGO = FindObjectOfType<HUDFPS>();
            if (hudGO != null)
            {
                hud = hudGO.GuiText;
                HideShowTextMsg();
            }
        }

        //if (Input.GetKeyUp(KeyCode.B))
        //{
        //    //_staticBatch = new StaticBatch();
        //    //_meshBatch = new MeshBatch();
        //}
    }

    void FixedUpdate()
    {
        //means tht it has it loaded
        if (_gameTime == null || _gameTimePeople == null)
        {
            return;
        }
        GameTime1.FixedUpdate();
        GameTimePeople.FixedUpdate();
    }

    string AddCachedReoutesCount()
    {
        if (PersonPot.Control == null || PersonPot.Control.RoutesCache1 == null)
        {
            return "";
        }

        return "\n Cached: " + PersonPot.Control.RoutesCache1.ItemsCount();
    }

    string AddPersonControllerInfo()
    {
        if (PersonPot.Control == null || PersonPot.Control.WorkersRoutingQueue == null)
        {
            return "";
        }


        string res = "";
        if (PersonPot.Control.WorkersRoutingQueue.OnSystemNow1.Count>0)
        {
            res += "on sysNow:"+ PersonPot.Control.WorkersRoutingQueue.OnSystemNow1[0].Id;
        }
        res += " waitList ct:"+PersonPot.Control.WorkersRoutingQueue.WaitList.Count;

        return res;
    }



    string AddLoadingInfo()
    {
        if (Program.gameScene.controllerMain != null 
                        && Program.gameScene.controllerMain.TerraSpawnController != null
                        && !Program.gameScene.controllerMain.TerraSpawnController.HasLoadedOrLoadedTreesAndRocks())
        {
            return "Loading";
        }
        return "Fully Loaded 100%";
    }

    /// <summary>
    /// To be called if the screen res was changed
    /// 
    /// This is to be called when u hit 'apply' on the Display Setup 
    /// </summary>
    void DebugChangeScreenResolution()
    {
        bool shouldWork = Developer.IsDev;

#if UNITY_EDITOR
        shouldWork = true;
#endif

        if (shouldWork && Input.GetKeyUp(KeyCode.Keypad1))
        {
           Program.MouseListener.ApplyChangeScreenResolution();
        }
    }

    private bool _hideText = true;
    private void DebugInput()
    {
        bool shouldWork = Developer.IsDev;

#if UNITY_EDITOR
        shouldWork = true;
#endif

        if (shouldWork && Input.GetKeyUp(KeyCode.Keypad0))
        {
            _hideText = !_hideText;
            HideShowTextMsg();

        }

    }

    private GUIText hud;
    private Color hudColor;
    //Wont change color bz is not in updte directly 
    void HideShowTextMsg()
    {
        Color col = Color.green;

        if (_hideText)
        {
            col.a = 0;
        }
        else col.a = 255;



        if (hud == null)
        {
            return;
        }

        hud.color = col;
    }

    private string more;
    /// <summary>
    /// Adds to the HUDFPS.Message
    /// </summary>
    /// <param name="moreP"></param>
    public void AddToMainScreen(string moreP)
    {more = moreP;}

    public static void ScreenPrint(string newA)
    {
        newA += "\n" + ScreenMsg;
        ScreenMsg = newA;
        textMessage.GetComponent<GUIText>().text = ScreenMsg;
    }

    public static void ResetDummyBlue()
    {
        dummyBlue.transform.position = new Vector3();
        dummyBlue.transform.rotation = Quaternion.identity;
    }

    public static void ResetDummySpawnPoint()
    {
        dummySpawnPoint.transform.position = new Vector3();
        dummySpawnPoint.transform.rotation = Quaternion.identity;
    }

    //public static void ResetDummyWithSpawnPoint(Structure dummPass)
    //{
    //    var ind = 

    //    dummyWithSpawnPoint.transform.position = new Vector3();
    //    dummyWithSpawnPoint.transform.rotation = Quaternion.identity;
    //}







    public void Destroy()
    {
        if (Terreno!=null)
        {
            Program.gameScene.controllerMain.TerraSpawnController.SendAllToPool();
            Terreno.Destroy();
            controllerMain.Destroy();

        }

        base.Destroy();
    }

        



    /// <summary>
    /// Load terrain and water 
    /// </summary>
    /// <param name="terrainRoot"></param>
    public void LoadTerrain()
    {
        //bz music 
        CamControl.CreateCam(H.CamRTS);

        if (string.IsNullOrEmpty(Program.MyScreen1.TerraRoot))
        {
            //the default terrain 
            Terreno = Terreno.CreateTerrain(Root.bayAndMountain1River, true);
        }
        else
        {
            //will create cvamera if is null
            Terreno = Terreno.CreateTerrain(Program.MyScreen1.TerraRoot);
        }
        
        if (WaterBody == null)
        {
            //at the Moment Water Small is not visible Apr1 2016. since the mirror was duplicating
            //the Draw calls
            _waterBody = General.Create(Root.waterSmall, new Vector3(0, 8, 0));
        }
        controllerMain = Create(Root.controllerMain, container: Program.ClassContainer.transform) as ControllerMain;
    }


    #region Dummy Pool

    private int poolSize = 30;
    private bool createDummySpawn = true;

    void CreateDummySpawnPoint()
    {
        if (!createDummySpawn || BuildingPot.Control == null || BuildingPot.Control.ProductionProp == null)
        {
            return;
        }
        createDummySpawn = false;
        InitDummyPool();
    }

    void InitDummyPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            dummiesSpwnPoint.Add((Structure)Building.CreateBuild(Root.dummyBuildWithSpawnPointUnTimed, new Vector3(), H.Dummy, 
                container: Program.ClassContainer.transform));
        }
    }

    internal Structure GimeMeUnusedDummy(string myIDP)
    {
        for (int i = 0; i < dummiesSpwnPoint.Count; i++)
        {
            if (dummiesSpwnPoint[i].transform.position == new Vector3() &&
                dummiesSpwnPoint[i].transform.rotation == Quaternion.identity &&
                string.IsNullOrEmpty(dummiesSpwnPoint[i].DummyIdSpawner) &&
                dummiesSpwnPoint[i].LandZone1.Count==0)
            {
               //Debug.Log("return dummy #:"+i);
                dummiesSpwnPoint[i].name = myIDP + ".Dummy";
                dummiesSpwnPoint[i].UsedAt = GameTime1.CurrentDate();
                return dummiesSpwnPoint[i];
            }
      }
      return null;
    }

    internal void ReturnUsedDummy(Structure usedDummy)
    {
        usedDummy.transform.position = new Vector3();
        usedDummy.transform.rotation = Quaternion.identity;
        usedDummy.name = usedDummy.Id+".Dummy";

        //bz is needed for next Routing 
        usedDummy.LandZone1.Clear();
        //usedDummy.DummyIdSpawner = "";
    }

    #endregion





    internal bool GameFullyLoaded()
    {
        if (controllerMain == null || p.TerraSpawnController == null)
        {
            return false;
        }

        var res = !p.TerraSpawnController.IsToLoadFromFile && BuildingPot.Control.Registro.IsFullyLoaded;

        if (res && _gameLoadedTime == 0)
        {
            _gameLoadedTime = Time.time;
        }

        return res;
    }




    public EventHandler<EventArgs> ChangeSpeed;
    void OnChangeSpeed(EventArgs e)
    {
        if (ChangeSpeed != null)
        {
            ChangeSpeed(this, e);
        }
    }

    private int oldSpeed;
    /// <summary>
    /// Will pause game. GameSpeed at 0
    /// </summary>
    internal void PauseGameSpeed()
    {
        oldSpeed = _gameSpeed;
        _gameSpeed = 0;
        OnChangeSpeed(EventArgs.Empty);
    }

    /// <summary>
    /// Will resume the game at the Paused Speed 
    /// </summary>
    internal void ResumeGameSpeed()
    {
        _gameSpeed = oldSpeed;
        OnChangeSpeed(EventArgs.Empty);
    }



    internal static string VersionLoaded()
    {
        var read = XMLSerie.ProgramData1;

        if (read != null)
        {
            return read.GameVersion;
        }
        else
        {
            return "Wait Loading";
        }
    }

    /// <summary>
    /// So the PlayerPref is saved. 
    /// </summary>
    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");

//#if UNITY_EDITOR
//        return;
//#endif

//        if (IsCurrentUserOnLogUploadList())
//        {
//            OpenLogHandler();
//        }
    }


    #region LogUploader
    
    //Logs will be uploaded only from people listed here 
    Dictionary<string, string> whiteList = new Dictionary<string, string>()
    {
        {"76561198245800476", "aatlantisstudios"},
    };

    bool IsCurrentUserOnLogUploadList()
    {
        return whiteList.ContainsKey(SteamUser.GetSteamID() + "");
    }

    /// <summary>
    /// Will open separate small .exe to upload and delete Log
    /// </summary>
    void OpenLogHandler()
    {
        // Prepare the process to run
        ProcessStartInfo start = new ProcessStartInfo();
        // Enter in the command line arguments, everything you would enter after the executable name itself
        start.Arguments = SteamUser.GetSteamID() + "." +SteamFriends.GetPersonaName();
        // Enter the executable to run, including the complete path
        start.FileName = Application.dataPath + "/Logs/LogsHandler.exe";
        // Do you want to show a console window?
        start.WindowStyle = ProcessWindowStyle.Hidden;
        start.CreateNoWindow = true;
        int exitCode;

        // Run the external process & wait for it to finish
        using (Process proc = Process.Start(start))
        {
            proc.WaitForExit();

            // Retrieve the app's exit code
            exitCode = proc.ExitCode;
        }
    }



#endregion


    
    internal bool IsDefaultTerreno()
    {
        return Terreno.Default;
    }



    /// <summary>
    /// Once data is loaded the Book has to be redo
    /// </summary>
    void RedoStuffWithLoadedData()
    {
        //means is a new game and this below is not needed 
        if (Program.MyScreen1.HoldDifficulty != -1)
        {
            return;
        }

        Program.MyScreen1.HoldDifficulty = PersonPot.Control.Difficulty;

        _book = new Book();
        _book.Start();

        PersonData pData = XMLSerie.ReadXMLPerson();

        Program.IsPirate = pData.PersonControllerSaveLoad.IsPirate;
        Program.IsFood = pData.PersonControllerSaveLoad.IsFood;
    }


    internal bool GameWasFullyLoadedAnd10SecAgo()
    {
        return Time.time > _gameLoadedTime + 10 && _gameLoadedTime != 0;
    }
}
