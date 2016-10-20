using System;
using UnityEngine;
using System.Collections.Generic;

public class InputMain : InputParent {


    public EventHandler<EventArgs> ChangeSpeed;
    /// <summary>
    /// This event is called everytime speed is changed on game
    /// </summary>
    /// <param name="e"></param>
    void OnChangeSpeed(EventArgs e)
    {
        if (ChangeSpeed != null)
        {
            ChangeSpeed(this, e);
            Rotate.SpeedChanged();
            AudioContainer.SpeedChanged();
        }
    }


    public static InputMeshSpawn InputMeshSpawnObj;
    public BuildingPot BuilderPot;
    private InputMouse inputMouse = new InputMouse();
    private PersonPot _personPot;

    public InputMouse InputMouse
    {
        get { return inputMouse; }
        set { inputMouse = value; }
    }

    public PersonPot PersonPot
    {
        get { return _personPot; }
        set { _personPot = value; }
    }

    int localDebugCounter;
    private List<General> debuger = new List<General>();

    public void CreatePersonPot()
    {
        PersonPot = (PersonPot)Create(Root.personPot, container: Program.ClassContainer.transform); 
    }

    void Start()
    {
        inputMouse.Start();
        InputMeshSpawnObj = (InputMeshSpawn)Create(Root.inputMeshSpawn, container: Program.ClassContainer.transform);
    }

    void Update()
    {
        UpdateCaller();

        if (BuilderPot == null)
        {
            BuilderPot = (BuildingPot)Create(Root.builderPot, container: Program.ClassContainer.transform);
        }

        GeneralSwitch();
        ModeSwitcher();
        ChangeGameSpeed();
        //AddressPointerOutOfScreen();
    }

    void AddressPointerOutOfScreen()
    {
        if (!Screen.fullScreen)
        {
            if (!IsGameFullyLoaded())
            {
                return;
            }

            //if mouse ppointer gets out of screen 
            Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
            if (!screenRect.Contains(Input.mousePosition))
            {
                CancelCurrentAction();
            }
        }
    }

    /// <summary>
    /// Will tell if game is fully loaded 
    /// 
    /// Improve: Might need to add more stuff to include person Loading
    /// </summary>
    /// <returns></returns>
    public bool IsGameFullyLoaded()
    {
        if (Program.gameScene == null || Program.gameScene.controllerMain == null)
        {
            return false;
        }

        if (Program.gameScene.controllerMain.MeshController == null) { return false; }
        if (Program.gameScene.controllerMain.MeshController.IsLoading) { return false; }

        if (BuildingPot.SaveLoad.IsToRecreateNow) { return false; }

        return true;
    }

    void RightClickRoutine()
    {
        if (BuildingPot.InputMode == Mode.None)
        {
            RightClickDebugShowHelp();
        }
        else if (BuildingPot.InputMode != Mode.None)
        {
            if (!BuildingPot.Control.CurrentSpawnBuild.PositionFixed)
            {
                CancelBuilding();
            }
        }
    }


    void GeneralSwitch()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            InputReport.Add(KeyCode.Escape+"");
            EscapeKey();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            RightClickRoutine();
        }
        else if (!Dialog.IsActive() && Developer.IsDev && Input.GetKeyUp(KeyCode.J))
        {
            Program.gameScene.controllerMain.MeshController.ForcedTerraScanning();
        }
        else if (!Dialog.IsActive() && Input.GetKeyUp(KeyCode.F))
        {
            InputReport.Add("QuickSaveNow()");
            QuickSaveNow();
        }
    }

    public void QuickSaveNow()
    {
        DataController.SaveGame(NowGameName(), true);
    }

    string NowGameName()
    {
        return Program.MyScreen1.TownName + " " + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " " +
               DateTime.Now.Hour + "h" + DateTime.Now.Minute + "m" + DateTime.Now.Second + "s";
    }



    public void EscapeKey()
    {
        var mainMenu = FindObjectOfType<MainMenuWindow>();

        //means is playing
        //&& Program.gameScene.GameFullyLoaded() is to not allow touch ESC while is loadig
        if (mainMenu == null && Program.gameScene.GameFullyLoaded())
        {
            Program.gameScene.PauseGameSpeed();
            
            Program.MouseListener.HideMainGUI();
            Program.MyScreen1.LoadMainMenuWithResumeBtn();

            CamControl.CAMRTS.StopReportingAudioNow();
        }
        //is on main Menu
        else if (mainMenu != null && Program.gameScene.GameFullyLoaded())
        {
            Program.gameScene.ResumeGameSpeed();

            Program.MyScreen1.DestroyCurrentMenu();
            Program.MouseListener.ShowMainGUI();

            CamControl.CAMRTS.ReportAudioNow();
        }
    }

 

    /// <summary>
    /// Says if game is unlock and can be saved now 
    /// </summary>
    /// <returns></returns>
    bool IsGameUnLock()
    {
        var personLock = PersonPot.Control.Locked;

        return !personLock;
    }

    void CancelCurrentAction()
    {
        CancelBuilding();
        //print("game is paused now, mouse out of game windows");
    }

    void CancelBuilding()
    {
        HandleWayDestroy();

        if (BuildingPot.Control.CurrentSpawnBuild != null)
        {
            BuildingPot.Control.CurrentSpawnBuild.FinishPlacingMode(H.Cancel);

        }
        //BuilderPot.InputU.BuildNowNew(BuilderPot.DoingNow);
        BuildingPot.InputMode = Mode.None;
        //_audioPlayer.PlaySoundOneTime(RootSound.hoverMenuSound);
    }

    /// <summary>
    /// This is for the roads farm when they are canceled neeed to to all this 
    /// </summary>
    void HandleWayDestroy()
    {
        //in case is a dragable square
        BuildingPot.InputU.KillCursor();
        BuildingPot.InputU.IsDraggingWay = false;


        DragSquare farm = BuildingPot.Control.CurrentSpawnBuild as DragSquare;
        if (farm != null)
        {
            farm.DestroyPreviews();
            return;
        }

        Way fWay = BuildingPot.Control.CurrentSpawnBuild as Way;
        if (fWay != null)
        {
            fWay.DestroyWayFromUserRightClick();
        }
    }

    /// <summary>
    /// Switch between modes 
    /// </summary>
    void ModeSwitcher()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            InputReport.Add("Mode.Building");
            BuildingPot.InputMode = Mode.Building;
            //_audioPlayer.PlaySoundOneTime(RootSound.hoverMenuSound);
            DestroyCurrentSpawnBuild();
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            //BuildingPot.InputMode = Mode.Cutting;
            //_audioPlayer.PlaySoundOneTime(RootSound.hoverMenuSound);
        }
        //if (BuilderPot.InputMode != Mode.Placing) { Cursor.visible = true; }
    }

    /// <summary>
    /// To avoid the bugg where once a new building was spawned the Unfixed one will saty around 
    /// </summary>
    void DestroyCurrentSpawnBuild()
    {
        if (BuildingPot.Control.CurrentSpawnBuild != null)
        {
            BuildingPot.Control.CurrentSpawnBuild.DestroySafe();
        }
    }

    public void RightClickDebugShowHelp()
    {
        DeleteAllDrawDebug();
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            debuger = UVisHelp.CreateHelpers(m.CurrentHoverVertices, Root.blueCube);
        }
        else if(Input.GetKey(KeyCode.LeftControl))
        {
            debuger = UVisHelp.CreateHelpers(m.Malla.Lots[localDebugCounter + 0].LotVertices, Root.blueCube);
            debuger.AddRange(UVisHelp.CreateHelpers(m.Malla.Lots[localDebugCounter + 10].LotVertices,
                Root.yellowSphereHelp));
            debuger.AddRange(UVisHelp.CreateHelpers(m.Malla.Lots[localDebugCounter + 20].LotVertices,
                Root.blueSphereHelp));

            localDebugCounter += 1;
        }
    }

    void DeleteAllDrawDebug()
    {
        if (debuger != null)
        {
            for (int i = 0; i < debuger.Count; i++)
            {
                debuger[i].Destroy();
            }
        }
        debuger.Clear();
    }


    #region This calls Updates on Obj that needed but dont have a real obj on Scene

    /// <summary>
    /// Calls updates 
    /// </summary>
    void UpdateCaller()
    {
        inputMouse.Update();
        UInput.Update();//needs to be call
    }

    #endregion

    /// <summary>
    /// Will say if game has fully loaded 
    /// 
    /// Still might be missing few stuff
    /// Mar17 2015 most recent 
    /// </summary>
    /// <returns></returns>
    bool HasGameAllLoaded()
    {
        return PersonPot.Control != null && PersonPot.Control.IsFullyLoaded()
                        && Program.gameScene.controllerMain != null 
                        && Program.gameScene.controllerMain.TerraSpawnController != null
                        && Program.gameScene.controllerMain.TerraSpawnController.HasLoadedOrLoadedTreesAndRocks();
    }


    void ChangeGameSpeed()
    {
        if (!Developer.IsDev)
        {
            return;
        }


        //if there are foresters for example they wil cut trees while the TerrainController is still loading. so
        //its not a good idea . all TerrainContrroller must be loaded before it can be played the game 
        if (!HasGameAllLoaded() || Program.gameScene.GameController1.IsGameOver)
        {
            return;
        }

        //1x
        if (Input.GetKeyUp(KeyCode.PageUp))
        {
            Program.gameScene.GameSpeed++;
            OnChangeSpeed(EventArgs.Empty);
        }
        if (Input.GetKeyUp(KeyCode.PageDown))
        {
            Program.gameScene.GameSpeed--;
            OnChangeSpeed(EventArgs.Empty);
        }
        //10x
        if (Input.GetKeyUp(KeyCode.PageUp) && Input.GetKey(KeyCode.LeftControl))
        {
            Program.gameScene.GameSpeed += 10;
            OnChangeSpeed(EventArgs.Empty);
        }
        if (Input.GetKeyUp(KeyCode.PageDown) && Input.GetKey(KeyCode.LeftControl))
        {
            Program.gameScene.GameSpeed -= 10;
            OnChangeSpeed(EventArgs.Empty);
        }
        //prevent negative
        if (Program.gameScene.GameSpeed < 0)
        {
            Program.gameScene.GameSpeed = 0;
            OnChangeSpeed(EventArgs.Empty);
        }

    }




    public void ChangeGameSpeedBy(int val)
    {
        if (Program.gameScene.GameController1.IsGameOver)
        {
            return;
        }

        Program.gameScene.GameSpeed += val;

        if (Program.gameScene.GameSpeed < 0)
        {
            Program.gameScene.GameSpeed = 0;
        }
        else if (Program.gameScene.GameSpeed > 10)
        {
            Program.gameScene.GameSpeed = 10;
        }
        //needs to be call for Body.cs
        OnChangeSpeed(EventArgs.Empty);
    }
}