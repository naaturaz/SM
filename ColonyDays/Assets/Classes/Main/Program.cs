using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Main Class
public class Program : MonoBehaviour {

    //Audio
    private AudioPlayer _audioPlayer;
    private bool audioWas;
       
    //GUI
    static MyScreen _myScreen = new MyScreen();

    //Main objects
    CamControl mainCamera;
    public static GameScene gameScene;
    public static InputMain InputMain;

    //this is use to containg phisically all classes, so all class that have
    // a empty gameObj in game hieratchy are under the same obj
    public static General ClassContainer;

    //cointains all the buildings as childs 
    public static General BuildsContainer;

    //cointains all the buildings as childs 
    public static General PersonObjectContainer;


    public static General MeshBatchContainer;

    //statics vars
    public static Vector3 VIEWPOS;//use to grab mouse view position
    public static Transform MOUSEOVERTHIS = null;
    public static Player THEPlayer;
    public static Profile THEProfile;
    public static MenuHandler2DBtn twoDMHandler;
	
    //has to be static so the value keeps raising with new objects created
    //otherwise the variable is reset it to origial value in each new
    //object.. and it has to be universal so I define _id in 'General' only once 
    //and the universal value keeps raising
    public static int UNIVERSALID = 0;

    //Holds what mouse is hitting on.. works if is hitting over an obj with a collider
   // public static RaycastHit MOUSEHITTHIS;

    private static MouseListener _mouseListener = new MouseListener();

    public static MouseListener MouseListener
    {
        get { return _mouseListener; }
        set { _mouseListener = value; }
    }

    public static MyScreen MyScreen1
    {
        get { return _myScreen; }
        set { _myScreen = value; }
    }

    private static bool debugPrint;
    public static bool GameFullyLoaded()
    {
        if (!gameScene) return false;
        var res = gameScene.GameFullyLoaded();

        if (res && !debugPrint)
        {
            Debug.Log("FullyLoadedGame:" + Time.time);
            debugPrint = true;
        }
        return res;
    }
    
    #region Unity Voids
       
    // Use this for initialization
	public void Start ()
	{
        Load1();
        Load2();
        Load3(H.None);
    }

    public void ReloadAll()
    {
        Load1();
        Load2();
        Load3(H.Create);
    }

    void Load1()
    {
        Debug.Log("first load");
        //loads main menu
        MyScreen1.Start();

        DataController.Start();
        Application.targetFrameRate = 60;
        //ProfilerHere();

        ManagerReport.Start();
        MouseListener.Start();

        if (//Camera.main != null &&
            _audioPlayer == null && !audioWas)
        {
            audioWas = true;
            //bz camera needs to be initiated already
            _audioPlayer = new AudioPlayer();
        }

        //AudioCollector.RedoGame();
    }

    void Load2()
    {
        Debug.Log("second load");

        ClassContainer = General.Create(Root.classesContainer);

        BuildsContainer = General.Create(Root.classesContainer, name: "BuildsContainer");
        PersonObjectContainer = General.Create(Root.classesContainer, name: "PersonObjectsContainer");
        MeshBatchContainer = General.Create(Root.classesContainer, name: "MeshBatchContainer"); 
    }

    void Load3(H hType)
    {
        Debug.Log("third load");

        if (gameScene == null)
        {
            gameScene = (GameScene)General.Create(Root.gameScene, container: ClassContainer.transform, hType: hType);
            InputMain = (InputMain)General.Create(Root.inputMain, container: ClassContainer.transform);
        }
    }

    private void OnApplicationQuit()
    {
        ManagerReport.FinishAllReports();
        Settings.SaveToFile();
    }

    private void ProfilerHere()
    {
        // write FPS to "profilerLog.txt"
        UnityEngine.Profiling.Profiler.logFile = Application.persistentDataPath + "/profilerLog.txt";

        // write Profiler Data to "profilerLog.txt.data"                                                                                        
        UnityEngine.Profiling.Profiler.enableBinaryLog = true;
        UnityEngine.Profiling.Profiler.enabled = true;
    }
    
    bool _didLoad2;
    bool _didLoad3;
    void Update()
    {
        MouseListener.Update();
        MyScreen1.Update();
        ManagerReport.Update();

        //if (!_didLoad2 && Time.time > 4)
        //{
        //    _didLoad2 = true;
        //    Load2();
        //}

        //if (!_didLoad3 && Time.time > 6)
        //{
        //    _didLoad3 = true;
        //    Load3();
        //}

    }
    #endregion


    public void MouseClickListener(string type)
    {
        _mouseListener.DetectMouseClick(type);
        ManagerReport.AddInput(type);
    }

    public static void MouseClickListenerSt(string type)
    {
        _mouseListener.DetectMouseClick(type);
        ManagerReport.AddInput(type);
    }

    public static void KillGame()
    {
        if (PersonPot.Control != null)
        {
            PersonPot.Control.ClearAll();
        }

        //ManagerReport.FinishAllReports("NewMap");

        if (!ClassContainer) return;

        ClassContainer.Destroy();
        BuildsContainer.Destroy();
        PersonObjectContainer.Destroy();
        MeshBatchContainer.Destroy();

        if (!gameScene) return;

        gameScene.QuestManager.ResetNewGame();


        gameScene.Destroy();
        gameScene = null;
        InputMain.Destroy();

        GameController.ResumenInventory1.GameInventory.Delete();


    }

    /// <summary>
    /// Kills the whole game and then restarts all objects and GameObj
    /// </summary>
    internal static void RedoGame()
    {
        KillGame();
        var prog = FindObjectOfType<Program>();
        prog.ReloadAll();
    }




    //there are at least two types: 
    //lock: buildings are lock and user needs to unlock them 
    //unlock: all buildings are unlock
    private static H _typeOfGame = H.None;//bz traditional is the default 

    public static H TypeOfGame
    {
        get { return _typeOfGame; }
        set { _typeOfGame = value; }
    }

    static internal bool IsAnUnLockGame()
    {
        return TypeOfGame == H.Unlock;
    }

    //input locking
    static bool _isInputLocked;

    public static bool IsInputLocked
    {
        get { return Program._isInputLocked; }
        set { Program._isInputLocked = value; }
    }

    public void LockInput()
    {
        IsInputLocked = true;
    }

    /// <summary>
    /// Called  from INputFields when get inActive 
    /// </summary>
    public void UnLockInput()
    {
        IsInputLocked = false;
    }

    /// <summary>
    /// Called  from INputFields when get active 
    /// </summary>
    static public void LockInputSt()
    {
        IsInputLocked = true;
    }

    /// <summary>
    /// Called  from INputFields when get inActive 
    /// </summary>
    static public void UnLockInputSt()
    {
        IsInputLocked = false;
    }

    /// <summary>
    /// It has pirate threat
    /// </summary>
    public static bool IsPirate { get; set; }

    /// <summary>
    /// Does food expires 
    /// </summary>
    public static bool IsFood { get; set; }

    public static bool WasTutoPassed { get; set; }

    /// <summary>
    /// Means in this session reached the WeekDraw
    /// </summary>
    public static bool WeekDraw { get; internal set; }
}










class CarlosTest
{
    //carlos
    public static List<NewBuild> newBildList;
    public static List<Btn3D> menus;

    public static void AddObject<T>(T obj)
    {
        if (obj.GetType() == typeof(NewBuild))
        {
            newBildList.Add(obj as NewBuild);
        }
        List<T> array = new List<T>();
        array.Add(obj);
    }
}