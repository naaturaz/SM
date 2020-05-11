/*
 * WARNINGS:
 * 1 - cant move the order now in the childs of camera BZ We are refencing those pos by index only can add more
 *
 * Documentation
 *
        //using this ColiU.SetColiState(camChildBtnSlots, H.Disable); because :
        //we need to do tht bz if htey are ignored like we do to the heart and other GUI elements
        //doesnt work well bz there is places in the screen tht wont spawn a model bz the camera child btn slots
        //are blocking them from do it
 */

using System.Collections.Generic;
using UnityEngine;

public class MenuHandler2DBtn : MenuHandler
{
    private int howManyBtns;//holds how many btns are to be use 3 or 5 so far

    //holds the dinamic  btns so we can access it any time
    private Btn2D[] btnArray = new Btn2D[10];

    //the transform that are childs of the camera will be assigned here
    private Transform[] setOfBtn;

    //still buttons
    private Btn2D pauseBtn;

    private List<Btn2D> hearts;
    private Btn2D pauseBackGround;

    private int amountOfCamBtnSlots = 10;
    private int amountOfCamChildObj = 24;
    private Transform[] camChild;
    private Transform[] camChildBtnSlots;

    //hold all the Btn2d that are on screen
    private List<Btn2D> onScreen = new List<Btn2D>();

    private const int MAX_MODEL_SPAWN = 10;

    //list to hold the models
    public List<Model> allModels = new List<Model>();

    private void GetAllCamChilds()
    {
        camChild = new Transform[amountOfCamChildObj];
        camChildBtnSlots = new Transform[amountOfCamBtnSlots];
        if (Camera.main.transform != null)
        {
            for (int i = 0; i < Camera.main.transform.childCount; i++)
            {
                camChild[i] = Camera.main.transform.GetChild(i);
                if (i < amountOfCamBtnSlots)
                {
                    camChildBtnSlots[i] = Camera.main.transform.GetChild(i);
                }
            }
            //DISABLEs the col the slots in cam child // + doc in the top comments
            UColi.SetColiState(camChildBtnSlots, H.Disable);
        }
    }

    //this list is used to make the hovering work
    public void UpdateList()
    {
        onScreen.Clear();
        //print("onScreen.Count." + onScreen.Count);

        for (int i = 0; i < btnArray.Length; i++)
        {
            if (btnArray[i] != null)
            {
                if (!btnArray[i].DestroyNow)
                {
                    onScreen.Add(btnArray[i]);
                }
            }
        }
        if (pauseBtn != null)
        {
            if (!pauseBtn.DestroyNow)
            {
                onScreen.Add(pauseBtn);
            }
        }
        for (int i = 0; i < hearts.Count; i++)
        {
            if (hearts[i] != null)
            {
                if (!hearts[i].DestroyNow)
                {
                    onScreen.Add(hearts[i]);
                }
            }
        }
        //print("onScreen.Count.end." + onScreen.Count);
    }

    public void DestroyIndividualHeart(int currentLives)
    {
        hearts[currentLives].Destroy2dBtn();
        UpdateList();
    }

    // Use this for initialization
    private void Start()
    {
        base.Start();
        hearts = new List<Btn2D>();
        CheckOnStillBtns();
    }

    // Update is called once per frame
    private void Update()
    {
        base.Start();

        ActOnRightClick();
        if (Program.MOUSEOVERTHIS != null)
        {
            Hovering(Program.MOUSEOVERTHIS);
        }
        LeftClick();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActionOnPauseBtn();
        }

        if (UKeys.FindBtnKeyUP() != "" && UKeys.FindBtnKeyUP() != null)
        {
            if (!Program.THEPlayer.isSpawningModel)
            {
                UponLeftClickOnMenu(UKeys.FindBtnKeyUP());
            }
        }
    }

    private void CheckOnStillBtns()
    {
        if (Camera.main != null && pauseBtn == null)
        {
            GetAllCamChilds();
            FireUpGUIButtons();
        }
    }

    //Creates hearts acoording to lives of player and pause btn
    private void FireUpGUIButtons(float speedFadePass = 50f)
    {
        pauseBtn = CreateStillBtn(Root.pauseButton, camChild[amountOfCamBtnSlots + 3]);
        pauseBtn.FadeSpeedProp = speedFadePass;

        for (int i = 0; i < Program.THEPlayer.Lives; i++)
        {
            Btn2D temp = CreateStillBtn(Root.heart, camChild[i + amountOfCamBtnSlots]);
            temp.FadeSpeedProp = speedFadePass;
            hearts.Add(temp);
            temp = null;
        }
        UpdateList();
    }

    //Based ont the paramenter will create geometry... the parameter format always have to be the same
    //used for Raw and Elements
    private void UponLeftClickOnMenu(string menuClicked)
    {
        CloseCurrentMenu();
        Model temp = null;
        //will split the string in '_',
        //word we are looking for is the second one: "Cone", "Cube", etc
        //the 4th word : "Raw", "Element"
        string[] split = menuClicked.Split('_');
        Vector3 ini = new Vector3();

        //used to store the initial pos of new models if Player is exisitng
        if (Program.THEPlayer != null)
        {
            ini = Vector3.MoveTowards(Program.THEPlayer.transform.position,
                -Camera.main.transform.position, 1f);
        }

        if (split[0] == "Actionable")
        {
            //this method() is in the base class
            ActionableBtnClick(btnArray);
        }
        else if (split[3] == "Raw")
        {
            temp = (Raw)General.Create(Root.ReturnFullPath(split[1]), ini);
        }
        else if (split[3] == "Element")
        {
            if (split[1] == "Spring")
            {
                temp = (Spring)General.Create(Root.ReturnFullPath(split[1]), ini);
            }
            else if (split[1] == "Mine")
            {
                temp = (Mine)General.Create(Root.ReturnFullPath(split[1]), ini);
            }
            else if (split[1] == "Bomb")
            {
                temp = (Bomb)General.Create(Root.ReturnFullPath(split[1]), ini);
            }
            else
                temp = (Element)General.Create(Root.ReturnFullPath(split[1]), ini);
        }
        else if (split[3] == "Main" || split[3] == "NewMenu" || split[3] == "Click"
            || menuClicked == "RightClicked")
        {
            if (split[4].Contains("PauseMenu"))
            {
                ActionOnPauseBtn(split[1]);
            }
            else
            {
                string menuToPull = split[1] + "_Menu_Spawner";
                SpawnSetMenu(menuToPull);
            }
        }
        else print("Error in naming should be: Select_Bomb_Btn_Element_3dMenu");

        ModelsManager(temp);
    }

    /// <summary>
    /// Manages how many models the player has spwaned
    /// </summary>
    /// <param name="temp"></param>
    private void ModelsManager(Model temp)
    {
        if (temp != null)
        {
            if (allModels.Count < MAX_MODEL_SPAWN)
            {
                allModels.Add(temp);
            }
            else
            {
                allModels[0].Destroy();
                allModels.RemoveAt(0);
                allModels.Add(temp);
            }
        }
    }

    private void ActionOnPauseBtn(string action = "")
    {
        //had to include this one here !Settings.ISPAUSE bz due to fade time will change it
        //back to false and then will disapper the pause menu and the still btns will not be back
        if ((!Settings.ISPAUSED && action == "") || (!Settings.ISPAUSED && action == "Pause"))
        {
            float tempSpeed = 150f;
            Settings.ISPAUSED = Settings.MecanicSwitcher(Settings.ISPAUSED);
            DestroyStillBtns();
            SpawnSetMenu("Pause_Menu_Spawner", tempSpeed);
            pauseBackGround = CreateStillBtn(Root.backGroundLabelPauseMenu, camChild[amountOfCamBtnSlots + 4]);
            pauseBackGround.FadeSpeedProp = tempSpeed;
        }
        else if ((Settings.ISPAUSED && action == "") || (Settings.ISPAUSED && action == "Resume"))
        {
            Settings.ISPAUSED = Settings.MecanicSwitcher(Settings.ISPAUSED);
            FireUpGUIButtons();
            CloseCurrentMenu();
            pauseBackGround.Destroy2dBtn();
        }
    }

    private void DestroyStillBtns()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].Destroy2dBtn();
        }
        pauseBtn.Destroy2dBtn();
        UpdateList();
    }

    private void LeftClick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //if MOUSEOVERTHIS not null
            if (Program.MOUSEOVERTHIS != null)
            {
                //print(Program.MOUSEOVERTHIS.name);
                //audioPlayer.PlayAudio(RootSound.clickMenuSound, H.Sound);
                //will pop up new menus
                //will pop up new Raws and Elements
                if ((Program.MOUSEOVERTHIS.name.Contains("Select_") || Program.MOUSEOVERTHIS.name.Contains("Actionable_"))
                    && Application.loadedLevelName != "Lobby")
                {
                    UponLeftClickOnMenu(Program.MOUSEOVERTHIS.name);
                }
                else if (Application.loadedLevelName != "Lobby")
                {
                    CloseCurrentMenu();
                }
            }
            else if (Application.loadedLevelName != "Lobby")
            {
                CloseCurrentMenu();
            }
        }
    }

    /// <summary>
    /// will close current menu .. if was in pause the game will fire up the still btns
    /// </summary>
    /// <param name="forcedP">need to be forced only if the game is paused</param>
    private void CloseCurrentMenu(bool forcedP = false)
    {
        if (!Settings.ISPAUSED || forcedP)
        {
            for (int i = 0; i < btnArray.Length; i++)
            {
                if (btnArray[i] != null)
                {
                    btnArray[i].Destroy2dBtn();
                }
            }
            SetBtnsAndNameThem(howManyBtns);
            UpdateList();
            MenuHandler.CREATEMENU = true;
            //DISABLEs the col the slots in cam child // + doc in the top comments
            UColi.SetColiState(camChildBtnSlots, H.Disable);
        }
    }

    //act on right click
    private void ActOnRightClick()
    {
        if (Input.GetMouseButtonUp(1) && Application.loadedLevelName != "Lobby")
        {
            SpawnSetMenu(S.RightClickedMenu.ToString());
        }
    }

    private Btn2D CreateStillBtn(string root, Transform slotWhereIsRenderOnScreen)
    {
        Btn2D temp = null;
        temp = (Btn2D)General.Create(root);
        temp.MyTransform = slotWhereIsRenderOnScreen;
        temp.StartMaterial();
        return temp;
    }

    private new void ActionableBtnClick(Button[] setOfBtns)//actionable buttons
    {
        base.ActionableBtnClick(setOfBtns);

        //when we click in the back to menu main btn in the pause screen
        if (currentBtn.btnAction == Btn.BackToMain)
        {
            int[] slots = { 6, 8 };//the slots we want to render this obj in the camChild slots
            CloseCurrentMenu(true);//we force to close current menu
            SpawnSetMenu(S.Confirm_Menu_Spawner.ToString(), 150f, slotsP: slots);
            Vector3 msgSpawn = USearch.FindTransfInArray(camChild, "Message_Spawner").transform.position;
            SpawnText(Camera.main.WorldToViewportPoint(msgSpawn), "If you exit will lose all the progress.\n"
                + "Are you sure you want to exit the game now?");
        }
        //when we click in the ok btn in the pause screen
        else if (currentBtn.btnAction == Btn.OkPause)
        {
            ActionOnPauseBtn();
            Application.LoadLevel("Lobby");
        }
        //when we click in the cancel btn in the pause screen
        else if (currentBtn.btnAction == Btn.CancelPause)
        {
            ActionOnPauseBtn();
            Destroy(textMessage.gameObject);
            textMessage = null;
        }
        //when we click in the SETTINGS btn in the pause screen
        else if (currentBtn.btnAction == Btn.SettingsPause)
        {
            CloseCurrentMenu(true);
            SpawnSetMenu(S.Settings_Pause_Menu_Spawner.ToString());
        }
        //when we click in the MUSIC btn in the settings pause screen
        else if (currentBtn.btnAction == Btn.SwitchMusic)
        {
            ChangeAudioSettings(H.Music.ToString());
        }
        //when we click in the SOUND btn in the settings pause screen
        else if (currentBtn.btnAction == Btn.SwitchSound)
        {
            ChangeAudioSettings(H.Sound.ToString());
        }
        //when we click in the BACK TO PAUSE MENU btn in the settings pause screen
        else if (currentBtn.btnAction == Btn.BackToPauseMenu)
        {
            CloseCurrentMenu(true);
            SpawnSetMenu("Pause_Menu_Spawner", 150f);
        }
        currentBtn = null;
    }

    /// <summary>
    /// spawns Set of buttons based on which menu was passed
    /// </summary>
    /// <param name="whichMenu">Which menu will be spawn </param>
    /// <param name="fadeSpeedPass"></param>
    /// <param name="slotsP">if we are sending an array of slotP[] then we will assign it mannually the
    /// slots in the camera to the new 2d Btns</param>
    private void SpawnSetMenu(string whichMenu, float fadeSpeedPass = 25f, int[] slotsP = null)
    {
        //we are looking for the a set of obj tht are stored in the list myMenuSets as a list as well
        int listIndex = Root.ReturnListNumber(whichMenu);
        howManyBtns = Root.myMenuSets[listIndex].Count - 1; /*bz last one is spawner*/
        int max = Root.myMenuSets[listIndex].Count - 2;//use it for find the middle one

        SetBtnsAndNameThem(howManyBtns);

        if (MenuHandler.CREATEMENU)
        {
            for (int i = 0; i < howManyBtns; i++)
            {
                btnArray[i] = (Btn2D)General.Create(Root.myMenuSets[listIndex][i]);

                //if slots is null we will assign slots in the camera child btn automatically
                //defined in SetBtnsAndNameThem() only useful for 3 or 5
                if (slotsP == null) btnArray[i].MyTransform = setOfBtn[i];
                //if we are sending an array of slotP[] then we will assign it mannually the
                //slots in the camera to the new 2d Btns
                else btnArray[i].MyTransform = camChildBtnSlots[slotsP[i]];

                btnArray[i].StartMaterial();
                btnArray[i].FadeSpeedProp = fadeSpeedPass;
            }
            UpdateList();
            MenuHandler.CREATEMENU = false;
            //enables the col the slots in cam child// + doc in the top comments
            UColi.SetColiState(camChildBtnSlots, H.Enable);
        }
    }

    /// <summary>
    /// Depening in the parameter howMany we choose the btn slots in the cam child and name them same as
    /// the obj will use them to render tthem selves
    /// </summary>
    /// <param name="howMany"></param>
    private void SetBtnsAndNameThem(int howMany)
    {
        setOfBtn = new Transform[howMany];
        int st = 0;
        int index = 0;
        //print("howMany." + howMany);
        //if is 3 then...
        if (howMany == 3)
        {
            //we do this so if is 3 btns will use the 3 central
            st = 1;
            howMany = 4;
        }
        for (int i = st; i < howMany; i++)
        {
            setOfBtn[index] = Camera.main.transform.GetChild(i);
            setOfBtn[index].name = (i + 1).ToString();
            index++;
        }
    }

    /// <summary>
    /// Will find which obj in onScreen List is hovered at the moment
    /// </summary>
    /// <param name="hovering">The transform the mouse is hovering at the moment</param>
    private void Hovering(Transform hovering)
    {
        int counter = 0;
        for (int i = 0; i < onScreen.Count; i++)
        {
            if (onScreen[i] != null)
            {
                if (hovering == onScreen[i].MyTransform.transform)
                {
                    onScreen[i].IsNowHovered = true;

                    Vector3 tipsSpawn = USearch.FindTransfInArray(camChild, "Tips_Spawner").transform.position;
                    SpawnText(Camera.main.WorldToViewportPoint(tipsSpawn));
                }
                else
                {
                    onScreen[i].IsNowHovered = false;
                    counter++;
                }
            }
        }
        //if the counter is being added as many times as the onScreen.Count means tht is nothing hovered at the
        //moment
        if (counter == onScreen.Count)
        {
            DestroyTipMenu();
        }
    }
}