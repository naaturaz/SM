using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Main Class
public class Program : MonoBehaviour {

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

    #region Unity Voids
    // Use this for initialization
	public void Start ()
	{
        //Settings.Load();

        DataController.Start();
        Application.targetFrameRate = 60;
	    //ProfilerHere();

	    ClassContainer = General.Create(Root.classesContainer);

	    BuildsContainer = General.Create(Root.classesContainer, name: "BuildsContainer");
	    PersonObjectContainer = General.Create(Root.classesContainer, name: "PersonObjectsContainer");
        MeshBatchContainer = General.Create(Root.classesContainer, name: "MeshBatchContainer");

        if (Application.loadedLevelName == "Lobby")
        {
            Settings.PlayMusic();
        }
        else
        {
            if (gameScene == null)
            {
                gameScene = (GameScene)General.Create(Root.gameScene, container: ClassContainer.transform);
                InputMain = (InputMain)General.Create(Root.inputMain, container: ClassContainer.transform);
            }
        }

        //loads main menu
        MyScreen1.Start();
        MouseListener.Start();
	}



    private void ProfilerHere()
    {
        // write FPS to "profilerLog.txt"
        Profiler.logFile = Application.persistentDataPath + "/profilerLog.txt";

        // write Profiler Data to "profilerLog.txt.data"                                                                                        
        Profiler.enableBinaryLog = true;
        Profiler.enabled = true;
    }

    
    void Update()
    {
        MouseListener.Update();
        MyScreen1.Update();
    }
    #endregion


    public void MouseClickListener(string type)
    {
        _mouseListener.DetectMouseClick(type);
    }

    public static void MouseClickListenerSt(string type)
    {
        _mouseListener.DetectMouseClick(type);
    }

    public static void KillGame()
    {
        if (PersonPot.Control != null)
        {
            PersonPot.Control.ClearAll();
        }

        ClassContainer.Destroy();
        BuildsContainer.Destroy();
        PersonObjectContainer.Destroy();
        MeshBatchContainer.Destroy();

        gameScene.Destroy();
        gameScene = null;
        InputMain.Destroy();

        GameController.ResumenInventory1.GameInventory.Delete();
    }

    internal static void RedoGame()
    {
        KillGame();
        var prog = FindObjectOfType<Program>();
        prog.Start();
    }
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