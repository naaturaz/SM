﻿using UnityEngine;
using System.Collections;
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
    private MyForm current;





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
        _mainMenuWindow = FindObjectOfType<MainMenuWindow>();
        _newGameWindow = FindObjectOfType<NewGameWindow>();
        _saveLoadGameWindow = FindObjectOfType<SaveLoadGameWindow>();

        //DecideWhichBtnShow();
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

        _terraRoot = terraRoot;
        _diff = diff;
        _townName = townName;
    }

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
    }

    public void DeleteSavedGameCallBack()
    {
        RedifineWindows();
        _saveLoadGameWindow.DeleteCallBack();
    }
}
