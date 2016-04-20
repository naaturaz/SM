using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameScene : General
{


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
    private int _gameSpeed = 0; //the speed of the whole Game

    public static General dummyBlue; //for help everywhere()
    public static General dummyRed; //for help everywhere()

    public static Structure dummySpawnPoint; //for help everywhere()
    private List<Structure> dummiesSpwnPoint = new List<Structure>(); //for help everywhere()


    private GameTime _gameTime = new GameTime();
    private GameController _gameController = new GameController();
    private ExportImport _exportImport = new ExportImport();


    private BatchManager _batchManager;
    private Culling _culling;
    private Fustrum _fustrum;
    private StaticBatch _staticBatch;
    private MeshBatch _meshBatch;



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

    // Use this for initialization
    private void Start()
    {
        Book.Start();

        LoadTerrain();

        GameController1.Start();
        StartCoroutine("SixtySecUpdate");

#if UNITY_EDITOR

        StartCoroutine("OneSecUpdate");

#endif

        Settings.PlayMusic();

        textMessage = (Btn3D) General.Create(Root.menusTextMiddle, new Vector3(0.85f, 0.3f, 0));
        textMessage.MoveSpeed = 40f; //so fade happens
        textMessage.FadeDirection = "FadeIn";

        dummyBlue = General.Create(Root.blueCubeBig, new Vector3());
        dummyRed = General.Create(Root.redCube, new Vector3());


        createDummySpawn = true;

        dummySpawnPoint = (Structure) Building.CreateBuild(Root.dummyBuildWithSpawnPointUnTimed, new Vector3(), H.Dummy,
            container: Program.ClassContainer.transform);

        //hudColor = textMessage.GetComponent<GUIText>().color;
    }


    #region BatchManager

    /// <summary>
    /// Is only gonna be called when CrystalManager is loaded 
    /// other wise Im creating a new Terrain 
    /// </summary>
    public void BatchManagerCreate()
    {
        _batchManager = new BatchManager();
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

#endregion

    private IEnumerator SixtySecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(60); // wait
            GameController1.ReCheckWheelBarrowsOnStorage();
        }
    }


    private IEnumerator OneSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1); // wait

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

	// Update is called once per frame
    void Update()
    {
        //means tht it has it loaded
        if (_gameTime == null)
        {
            return;
        }
        
        GameController1.Update();
        CreateDummySpawnPoint();


        DebugInput();
        DebugChangeScreenResolution();


        if (Camera.main != null && _culling == null //&& PersonPot.Control!= null 
            //&& PersonPot.Control.All.Count > 0
            //&& BuildingPot.Control!=null && BuildingPot.Control.Registro.AllBuilding.Count>1
            )
        {
            _culling = new Culling();
            _fustrum = new Fustrum();
        }

        if (_fustrum!=null)
        {
            _fustrum.Update();
        }

        if (hud==null)
        {
            hud = FindObjectOfType<HUDFPS>().GuiText;
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
        if (_gameTime == null)
        {
            return;
        }
        GameTime1.FixedUpdate();
        
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
        if (Input.GetKeyUp(KeyCode.Keypad1))
        {
           Program.MouseListener.ApplyChangeScreenResolution();
        }
    }

    private bool _hideText = true;
    private void DebugInput()
    {
        if (Input.GetKeyUp(KeyCode.Keypad0))
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
        //hudColor = col;
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
        Terreno.Destroy();
        controllerMain.Destroy();

        base.Destroy();
    }

        



    /// <summary>
    /// Load terrain and water 
    /// </summary>
    /// <param name="terrainRoot"></param>
    public void LoadTerrain()
    {
        //will create cvamera if is null
        CamControl.CreateCam(H.CamRTS);

        if (string.IsNullOrEmpty(Program.MyScreen1.TerraRoot))
        {
            //the default terrain 
            Terreno = Terreno.CreateTerrain(Root.bayAndMountain1River);
        }
        else Terreno = Terreno.CreateTerrain(Program.MyScreen1.TerraRoot);
        //Terreno.name += "." + Time.time;

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
        return !p.TerraSpawnController.IsToLoadFromFile && BuildingPot.Control.Registro.IsFullyLoaded;
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

    public void ClearOldSpeed()
    {
        oldSpeed = 0;
    }
}
