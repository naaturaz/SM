using System.Collections.Generic;
using UnityEngine;

public class GameScene : General {


    public Terreno Terreno;

    private static Btn3D textMessage;
    public static string ScreenMsg;

    public ControllerMain controllerMain;
    private General _waterBody;

    private Book _book = new Book();//keeps all the caps for each object of the game 

    //the scale of the small units in a farm
    Vector3 _subDivideBlockScale = new Vector3();
    
    //how thick a block of way is
    float _subDivideBlockYVal = 0.05f;//0.001f;
    private int _gameSpeed = 0;//the speed of the whole Game

    public static General dummyBlue;//for help everywhere()
    public static General dummyRed;//for help everywhere()
    List<Structure> dummiesSpwnPoint = new List<Structure>();//for help everywhere()


    private GameTime _gameTime = new GameTime();
    GameController _gameController = new GameController();
    ExportImport _exportImport = new ExportImport();

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

    // Use this for initialization
	void Start ()
	{
        Book.Start();

        LoadTerrain();
      



        Settings.PlayMusic();

        textMessage = (Btn3D)General.Create(Root.menusTextMiddle, new Vector3(0.85f, 0.3f, 0));
        textMessage.MoveSpeed = 40f; //so fade happens
        textMessage.FadeDirection = "FadeIn";

        dummyBlue = General.Create(Root.blueCubeBig, new Vector3());
        dummyRed = General.Create(Root.redCube, new Vector3());


	    createDummySpawn = true;

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

        CreateDummySpawnPoint();

        //var onS = PersonPot.PersonController.All.ElementAt(0).Value.Geometry.GetComponent<OnScreen>();
        //var vis = onS.Visible();

        HUDFPS.Message = " | " + MouseInBorderRTS.GlobalDir.ToString() + " | Dragged: " + Way._dir +
                " | InputMode: " + BuildingPot.InputMode + "\n" + more + "\n"
            //+ " Vis: " + PersonPot.PersonController.All.ElementAt(0).Value.IsVisible()
            //+ " | Can see: " + PersonPot.PersonController.All.ElementAt(0).Value.I_Can_See()
            //+ " | Became: " + vis
                ;

        DebugInput();
        DebugChangeScreenResolution();
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
        }

        HideShowTextMsg();
    }

    void HideShowTextMsg()
    {
        Color col = Color.green;

        if (_hideText)
        {
            col.a = 0;
        }
        else col.a = 255;

        var hud = FindObjectOfType<HUDFPS>().GuiText;

        if (hud == null)
        {
            return;
        }

        hud.color = col;
        textMessage.GetComponent<GUIText>().color = col;
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
            dummiesSpwnPoint.Add((Structure)Building.CreateBuild(Root.dummyBuildWithSpawnPoint, new Vector3(), H.Dummy, 
                container: Program.ClassContainer.transform));
        }
    }

    internal Structure GimeMeUnusedDummy()
    {
        for (int i = 0; i < dummiesSpwnPoint.Count; i++)
        {
            if (dummiesSpwnPoint[i].transform.position == new Vector3())
            {
                return dummiesSpwnPoint[i];
            }
        }
        return null;
    }

    internal void ReturnUsedDummy(Structure usedDummy)
    {
        usedDummy.transform.position = new Vector3();
    }




    #endregion


}
