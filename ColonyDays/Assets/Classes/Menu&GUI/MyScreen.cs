using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

/*
 * All Actions related to the the Screen. Including loading screen
 * 
 * Will pass contrll to MouseListener when MainGUI is created. That is created here 
 */
public class MyScreen : General
{
    private MainMenuWindow _mainMenuWindow;//the window of the main menu. so can be hidden and shw back
    private NewGameWindow _newGameWindow;
    private SaveLoadGameWindow _saveLoadGameWindow;
    
    private OptionsWindow _optionsWindow;
    private AchieveWindow _achieveWindow;
    private MyForm current;
    private MyForm mainMenuForm = new MyForm();

    private string _terraRoot;//the terrain root
    private string _diff;//the game difficulty

    private string _townName;//the town name 

    internal SaveLoadGameWindow SaveLoadGameWindow
    {
        get { return _saveLoadGameWindow; }
        set { _saveLoadGameWindow = value; }
    }

    public NewGameWindow NewGameWindow1
    {
        get { return _newGameWindow; }
        set { _newGameWindow = value; }
    }

    public string TerraRoot
    {
        get { return _terraRoot; }
        set { _terraRoot = value; }
    }

    public string TownName
    {
        get { return _townName; }
        set { _townName = value; }
    }

    public OptionsWindow OptionsWindow1
    {
        get { return _optionsWindow; }
        set { _optionsWindow = value; }
    }

    public MainMenuWindow MainMenuWindow1
    {
        get { return _mainMenuWindow; }
        set { _mainMenuWindow = value; }
    }

    #region Main Menu. Load Game. New Game

    public void Start()
    {
        Settings.LoadFromFile();
        
        //so is used only 1st time 
        if (current != null)
        {
            return;    
        }

        LoadMainMenu();

        var first = GameObject.Find("FirstScreen");
        Destroy(first);

        //can only be one on scene to work 
        RedifineWindows();
    }

    void RedifineWindows()
    {
        if (_mainMenuWindow == null)
        {
            _mainMenuWindow = FindObjectOfType<MainMenuWindow>();
        }
        if (_newGameWindow == null)
        {
            _newGameWindow = FindObjectOfType<NewGameWindow>();
        }
        if (SaveLoadGameWindow == null)
        {
            SaveLoadGameWindow = FindObjectOfType<SaveLoadGameWindow>();
        }
        if (_optionsWindow == null)
        {
            _optionsWindow = FindObjectOfType<OptionsWindow>();
        }
        if (_achieveWindow == null)
        {
            _achieveWindow = FindObjectOfType<AchieveWindow>();
        }
    }

    /// <summary>
    /// This is called when ESC key is pressed while player was plyaing 
    /// </summary>
    public void LoadMainMenuWithResumeBtn()
    {
        LoadMainMenu();
        Start();
    }
       
    /// <summary>
    /// Load the main menu
    /// </summary>
    public void LoadMainMenu()
    {
        current = (MyForm)General.Create(Root.mainMenu, new Vector2());
        mainMenuForm = current;
        CamControl.ChangeTo("Main");

        Debug.Log("Load Main Menu");
    }

    public bool IsMainMenuOn()
    {
        return current != null && current.name.Contains("Menu");
    }
    
    bool wasOptionalFeedbackShown;
    /// <summary>
    /// Depending on the btn was clicked will do action 
    /// </summary>
    /// <param name="action"></param>
    public void MouseListenAction(string action)
    {
        var sub = action.Substring(9);

        if (sub.Contains("New."))
        {
            _newGameWindow.MouseListen(sub);
        }
        else if (sub.Contains("Tutorial"))
        {
            _newGameWindow.MouseListen(sub);
        }
        else if (sub.Contains("Save."))
        {
            SaveLoadGameWindow.MouseListen(sub);
        }
        else if (sub == "Continue")
        {
            ContinueGameBtn();
        }
        else if (sub == "Resume")
        {
            Program.InputMain.EscapeKey();
        }
        else if (sub == "NewGame")
        {
            RedifineWindows();

            HideMainMakeWindActive(_newGameWindow);
        }
        else if(sub == "Exit")
        {
            if (!wasOptionalFeedbackShown)
            {
                Dialog.InputFormDialog(H.OptionalFeedback);
                wasOptionalFeedbackShown = true;
                return;
            }

             Application.Quit();
        }    
        else if(sub == "SaveGame")
        {
            RedifineWindows();
            SaveLoadGameWindow.Show("Save");
        }    
        else if(sub == "LoadGame")
        {
            RedifineWindows();
            SaveLoadGameWindow.Show("Load");
        }   
        else if (sub.Contains("Options"))
        {
            RedifineWindows();
            _optionsWindow.Listen(sub);
        }
        else if (sub.Contains("Achieve"))
        {
            //this is a call from Main
            if (sub == "Achieve")
            {
                RedifineWindows();
                _achieveWindow.Show("");
            }
                //the hit of the ok
            else
            {
                _achieveWindow.MouseListen(sub);
            }
           
            //Debug.Log("Achive");
        }
    }
    
    public void Update()
    {
        if (isNewGameCreated)
        {
            FirstPartOfNewGameCreated();
        }

        InputKeys();
    }

    private void InputKeys()
    {
        if (!IsMainMenuOn()) return;

        if(Input.GetKey(KeyCode.C))
        {
            if(_mainMenuWindow == null)
            _mainMenuWindow = FindObjectOfType<MainMenuWindow>();

            if(!_mainMenuWindow.IsContinueBtnInteractable())return;

            ContinueGameBtn();
        }
        else if (Input.GetKey(KeyCode.N))
        {
            RedifineWindows();
            HideMainMakeWindActive(_newGameWindow);
        }
    }

    private bool isNewGameCreated;
    private float timeClicked;
    /// <summary>
    /// Once the OK btn is clicked on the Create new game window
    /// </summary>
    public void NewGameCreated(string terraRoot, string diff, string townName)
    {
        XMLSerie.NewGame();

        isNewGameCreated = true;
        timeClicked = Time.time;
        DestroyCurrLoadLoading();

        if (string.IsNullOrEmpty(terraRoot))
        {
            _terraRoot = ReturnRandomTerraRoot();
        }
        else
        {
            _terraRoot = terraRoot;
        }

        if (string.IsNullOrEmpty(diff))
        {
            diff = "Easy";
        }
        _diff = diff;
        AssignDificulty();


        _townName = townName;
    }

    int _holdDifficulty = -1;
    public int HoldDifficulty
    {
        get { return _holdDifficulty; }
        set { _holdDifficulty = value; }
    }
    
    void AssignDificulty()
    {
        if (_diff == "Newbie")
        {
            HoldDifficulty = 4;
        }
        else if (_diff == "Easy")
        {
            HoldDifficulty = 3;
        }
        else if (_diff == "Moderate")
        {
            HoldDifficulty = 2;
        } 
        else if (_diff == "Hard")
        {
            HoldDifficulty = 1;
        }
        else if (_diff == "Insane")
        {
            HoldDifficulty = 0;
        }
    }

    internal int HoldDifficultyReal()
    {
        return HoldDifficulty + 1;
    }

    #region Random Terra Root
    //IMPORTANT : To add a new Terrain
    //To add a new Terrain the only thing needed is add the XML file to Application.dataPath;
    //Also in NewGameWindow
    //Need to be added in the button NewGameWindow.SetButtonsList()
    //the button also needs to be added to mainMenu manually
    //And need to be added on the Root.BigTerrains
    //In Dev Mode let it run and when finish then press, F12,
    //Then when XML is finish copy it to Assets

    /// <summary>
    /// bz this Xmls are all Prefabs tht gets call in game
    /// 
    /// "Prefab/Terrain/" + file
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    static public string AddPrefabTerrainRoot(string file)
    {
        return "Prefab/Terrain/" + file;
    }

    private string _dataPath;
    /// <summary>
    /// now user wont select terrain will be always at random
    /// 
    /// to remember would nt work in Standalone bz in StandAlone the root was:
    /// C:/GitHub/SM/10.05/SugarMill_Data\Bay_And_Mountain_1_River.Spawned.xml
    /// and the additional dot in '10.05' will mess below code
    /// 
    /// READ if is failing needs the Developer = true
    /// </summary>
    /// <returns></returns>
    string ReturnRandomTerraRoot()
    {
        _dataPath = Application.dataPath;

        //gets all xml files
        var xmls = Directory.GetFiles(_dataPath, "*.xml").ToList();
        List<string> validTerras = new List<string>();

        for (int i = 0; i < xmls.Count; i++)
        {
            var splitArray = xmls[i].Split('.');
            Debug.Log("xmls: "+xmls[i]);
            
            if (splitArray.Length > 1 && splitArray[1] == "Spawned")
            {
                //confirms that they have a terra file tht has the same name 
                if (ConfirmThisIsATerraFile(splitArray[0], xmls))
                {
                    validTerras.Add(splitArray[0]);
                    Debug.Log("validTerras: " + splitArray[0]);

                }
            }
        }

        var rand = validTerras[UMath.GiveRandom(0, validTerras.Count)];
        var cleanRand = RemoveDataPath(rand, _dataPath);
        return AddPrefabTerrainRoot(cleanRand);
    }

    /// <summary>
    /// Will loop true XML list and will find match for 'terra'
    /// </summary>
    /// <param name="terra"></param>
    /// <param name="xmls"></param>
    /// <returns></returns>
    bool ConfirmThisIsATerraFile(string terra, List<string> xmls)
    {
        for (int i = 0; i < xmls.Count; i++)
        {
            var cleaned = CleanRouteFile(xmls[i]);
            var terraClean = RemoveDataPath(terra, _dataPath);

            Debug.Log("terra:"+terra+"...dataPath:"+_dataPath);

            if (terraClean == cleaned)
            {
                return true;
            }
        }
        return false;
    }

    string CleanRouteFile(string fileName)
    {
        var splitArray = fileName.Split('.');
        return RemoveDataPath(splitArray[0], _dataPath);
    }

    public static string RemoveDataPath(string pathToClean, string dataPath)
    {
        var len = dataPath.Length;
        return pathToClean.Substring(len + 1);//bz the: \\
    }

#endregion

    /// <summary>
    /// bzloading screen appers after the terrain is loaded . so im goona wait until loading is loaded so will fire this 
    /// events after 
    /// </summary>
    void FirstPartOfNewGameCreated()
    {
        //and im waiting 1 second 
        if (current.name.Contains("Loading") && Time.time > timeClicked + 0)
        {
            isNewGameCreated = false;

            Program.RedoGame();

            BuildingPot.LoadBuildingsNow();
        }
    }


    public void ContinueGameBtn()
    {
        DataController.ContinueGame();
    }

    /// <summary>
    /// Will destroy current form and will load loading screebn
    /// </summary>
    public void DestroyCurrLoadLoading()
    {
        CamControl.ChangeTo("Game");


        current.Destroy();
        LoadLoadingScreen();
    }

    void LoadLoadingScreen()
    {
        current = (MyForm)General.Create(Root.loadingScreen, new Vector2());
    }

    public void LoadingScreenIsDone()
    {
        CamControl.CAMRTS.InputRts.CenterCam(true);


        Program.MouseListener.LoadMainGUI();
        Program.MouseListener.ApplyChangeScreenResolution();

        DestroyCurrentMenu();
        ManagerReport.AddFPS();
    }


    //public void ReloadGUI()
    //{
    //    Program.MouseListener.ReloadMainGUI();
    //    Program.MouseListener.ApplyChangeScreenResolution();
    //}

    public void DestroyCurrentMenu()
    {
        if (current == null)
        {
            return;
        }

        current.Destroy();
        current = null;
    }

    #endregion

    /// <summary>
    /// Will hide 'ele' windows and will show Main Menu Window
    /// </summary>
    /// <param name="wind"></param>
    public void HideWindowShowMain(GUIElement ele)
    {
        RedifineWindows();

        ele.Hide();
        _mainMenuWindow.Show();
    }

    void HideMainMakeWindActive(GUIElement window)
    {
        RedifineWindows();

        _mainMenuWindow.Hide();
        window.Show();
    }

    public void DeleteSavedGameCallBack()
    {
        RedifineWindows();
        SaveLoadGameWindow.DeleteCallBack();
    }

    /// <summary>
    /// Needed so MainGUI doesnt go on top of MainMenu
    /// </summary>
    internal void ReLoadMainMenuIfActive()
    {
        var forms = FindObjectsOfType<MyForm>();

        for (int i = 0; i < forms.Length; i++)
        {
            if (forms[i] != null && forms[i].MyId.Contains("MainMenu"))
            {
                RedifineWindows();
                _mainMenuWindow.Destroy();
                _mainMenuWindow = null;

                _newGameWindow.Destroy();
                _newGameWindow = null;

                SaveLoadGameWindow.Destroy();
                SaveLoadGameWindow = null;

                _optionsWindow.Destroy();
                _optionsWindow = null;

                if (_achieveWindow)
                {
                    _achieveWindow.Destroy();
                    _achieveWindow = null;
                }

                DestroyCurrentMenu();
                LoadMainMenu();
                Debug.Log("Reload Main Menu  ReLoadMainMenuIfActive");   
         
                RedifineWindows();
            }
        }
    }

}
