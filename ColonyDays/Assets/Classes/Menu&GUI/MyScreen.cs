using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
    private MyForm current;
    private MyForm mainMenuForm = new MyForm();




    private string _terraRoot;//the terrain root
    private string _diff;//the game difficulty
    private string _townName;//the town name 



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

    public string Diff
    {
        get { return _diff; }
        set { _diff = value; }
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

    #region Main Menu. Load Game. New Game

    public void Start()
    {
        //so is used only 1st time 
        if (current != null)
        {
            return;    
        }

        LoadMainMenu();

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
        if (_saveLoadGameWindow == null)
        {
            _saveLoadGameWindow = FindObjectOfType<SaveLoadGameWindow>();
        }
        if (_optionsWindow == null)
        {
            _optionsWindow = FindObjectOfType<OptionsWindow>();
        }
    }

    /// <summary>
    /// This is called when ESC key is pressed while player was plyaing 
    /// </summary>
    public void LoadMainMenuWithResumeBtn()
    {
        LoadMainMenu();
        Start();

        //_mainMenuWindow.MakeResumeActive();
    }



    /// <summary>
    /// Load the main menu
    /// </summary>
    public void LoadMainMenu()
    {
        current = (MyForm)General.Create(Root.mainMenu, new Vector2());
        mainMenuForm = current;
    }

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
        else if (sub.Contains("Save."))
        {
            _saveLoadGameWindow.MouseListen(sub);
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
             Application.Quit();
        }    
        else if(sub == "SaveGame")
        {
            RedifineWindows();
            _saveLoadGameWindow.Show("Save");
        }    
        else if(sub == "LoadGame")
        {
            RedifineWindows();
            _saveLoadGameWindow.Show("Load");
        }   
        else if (sub.Contains("Options"))
        {
            RedifineWindows();
            _optionsWindow.Listen(sub);
        }
    }


    public void Update()
    {
        if (isNewGameCreated)
        {
            FirstPartOfNewGameCreated();
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
        
        _townName = townName;
    }



    #region Random Terra Root

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
            if (splitArray.Length > 0 && splitArray[1] == "Spawned")
            {
                //confirms that they have a terra file tht has the same name 
                if (ConfirmThisIsATerraFile(splitArray[0], xmls))
                {
                    validTerras.Add(splitArray[0]);
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
        current.Destroy();
        LoadLoadingScreen();
    }

    void LoadLoadingScreen()
    {
        current = (MyForm)General.Create(Root.loadingScreen, new Vector2());
    }

    public void LoadingScreenIsDone()
    {
        Program.MouseListener.LoadMainGUI();
        Program.MouseListener.ApplyChangeScreenResolution();


        DestroyCurrentMenu();
    }


    public void DestroyCurrentMenu()
    {
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
        _saveLoadGameWindow.DeleteCallBack();
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

                _saveLoadGameWindow.Destroy();
                _saveLoadGameWindow = null;

                _optionsWindow.Destroy();
                _optionsWindow = null;

                DestroyCurrentMenu();
                LoadMainMenu();
                Debug.Log("Reload Main Menu");   
         
                RedifineWindows();
            }
        }
    }
}
