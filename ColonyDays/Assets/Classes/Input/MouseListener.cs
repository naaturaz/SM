using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseListener : InputMain
{
    private Building _bullDozerGO;

    private MyForm _currForm = new MyForm();

    public MyForm CurrForm
    {
        get { return _currForm; }
        set { _currForm = value; }
    }

    public MyForm Main
    {
        get { return main; }
    }

    public BuildingWindow BuildingWindow1
    {
        get { return _buildingWindow; }
        set { _buildingWindow = value; }
    }

    public PersonWindow PersonWindow1
    {
        get { return _personWindow; }
        set { _personWindow = value; }
    }

    public BuildingsMenu BuildingsMenu1
    {
        get { return _buildingsMenu; }
        set { _buildingsMenu = value; }
    }

    public NotificationWindowGO NotificationWindow
    {
        get { return _notificationWindow; }
        set { _notificationWindow = value; }
    }

    private SteamStatsAndAchievements m_StatsAndAchievements;

    /// <summary>
    /// Accessing to Steam API Achivements and Stats
    /// </summary>
    public SteamStatsAndAchievements MStatsAndAchievements
    {
        get { return m_StatsAndAchievements; }
        set { m_StatsAndAchievements = value; }
    }

    // Use this for initialization
    public void Start()
    {
        if (m_StatsAndAchievements == null)
        {
            m_StatsAndAchievements = GameObject.FindObjectOfType<SteamStatsAndAchievements>();

            if (m_StatsAndAchievements != null)
            {
                m_StatsAndAchievements.OnGameStateChange(EClientGameState.k_EClientGameActive);
            }
        }
    }

    /// <summary>
    /// Loading  and Reloading Main Form
    /// </summary>
    private MyForm main;//the main GUI

    public void LoadMainGUI()
    {
        if (main == null)
        {
            main = (MyForm)Create(Root.mainGUI, new Vector2());
        }

        //can only be one on scene to work
        _buildingsMenu = FindObjectOfType<BuildingsMenu>();
        _descriptionWindow = FindObjectOfType<DescriptionWindow>();
        _personWindow = FindObjectOfType<PersonWindow>();
        _buildingWindow = FindObjectOfType<BuildingWindow>();
        _addOrderWindow = FindObjectOfType<AddOrderWindow>();
        _notificationWindow = FindObjectOfType<NotificationWindowGO>();

        BulletinWindow = FindObjectOfType<BulletinWindow>();
        QuestWindow = FindObjectOfType<QuestWindow>();
        _helpWindow = FindObjectOfType<HelpWindow>();
        TutoWindow1 = FindObjectOfType<TutoWindow>();

        //Debug.Log("LoadMainGUI() GUI");

        if (CamControl.IsMainMenuOn())
        {
            HideMainGUI();
        }
    }

    private Vector3 mainTempIniPos;

    public void HideMainGUI()
    {
        main.gameObject.SetActive(false);
        //Debug.Log("HideMainGUI() GUI");
    }

    public void ShowMainGUI()
    {
        if (!main.gameObject.activeSelf)
        {
            main.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// CAlled from Release loading screen, only needed for 2nd game loaded
    /// </summary>
    internal void ReActivateGUIIfNeeded()
    {
        main.Destroy();
        main = null;
        LoadMainGUI();

        if (main != null)
        {
            ShowMainGUI();
        }
    }

    public void ApplyChangeScreenResolution(bool promtToGame = false)
    {
        if (Program.gameScene.Fustrum1 != null)
            Program.gameScene.Fustrum1.RedoRect();

        Program.MyScreen1.ReLoadMainMenuIfActive();

        //being called before a game is loaded
        if (_personWindow == null)
        {
            return;
        }

        HidePersonBuildOrderNotiWindows();

        Program.gameScene.DoATempSave();

        main.Destroy();
        main = null;
        LoadMainGUI();

        //in case PersonWindow was not null. So main menu is last
        Program.MyScreen1.ReLoadMainMenuIfActive();
    }

    /// <summary>
    /// Mian  input method
    /// </summary>
    /// <param name="action"></param>
    public void DetectMouseClick(string action)
    {
        //print("DetectMouseClick() :" + type);
        if (action == "Outside")
        {
            //for when is clicked on Main menu gives NullRef
            if (_addOrderWindow == null)
            {
                return;
            }

            _addOrderWindow.Hide();
            _buildingWindow.Hide();
            BulletinWindow.Hide();
            HelpWindow.Hide();

            //try to select person first
            if (!SelectPerson())
            {
                //if coulndt then try to select build
                if (!SelectClickedBuild())
                {
                    //if was not posible to seelct a building
                    HidePersonBuildOrderNotiWindows();

                    var tryBuy = SelectSellRegion();
                }
            }
        }
        else if (action != "")
        {
            ActionFromForm(action);
        }
    }

    /// <summary>
    /// Will try to select a person. Person selection has more importantce than
    /// building selection and priority
    /// </summary>
    /// <returns>Will retrun true if a person was selected</returns>
    private bool SelectPerson()
    {
        Transform clicked = UPoly.RayCastLayer(Input.mousePosition, 11).transform;

        if (clicked != null)
        {
            ManagerReport.AddInput("Selected person: " + clicked.name);

            _personSelect = clicked.GetComponent<Person>();
            _personWindow.Show(_personSelect);

            UnselectingBuild();

            _buildingWindow.Hide();
            return true;
        }
        _personWindow.Hide();
        return false;
    }

    public void SelectPerson(Person pers)
    {
        ManagerReport.AddInput("Selected person: " + pers.name);
        _personSelect = pers;
        _personWindow.Show(_personSelect);
        UnselectingBuild();
        _buildingWindow.Hide();
    }

    /// <summary>
    /// Will select clicked building and ret true if one was seelected
    /// </summary>
    /// <returns></returns>
    private bool SelectClickedBuild()
    {
        if (!Input.GetMouseButtonUp(0))
        {
            return false;
        }

        List<string> names = new List<string>();
        var clicked = ReturnBuildinHit();

        //unselect if was click outise
        if (clicked != null)
        {
            names = UString.ExtractNamesUntilGranpa(clicked);
            Program.InputMain.InputMouse.UnSelectRoutine(names, clicked);
        }
        //select new Build
        if (names.Count > 0)
        {
            for (int i = 0; i < names.Count; i++)
            {
                H typeL = Program.InputMain.InputMouse.FindType(names[i]);
                Ca cat = DefineCategory(typeL);
                Program.InputMain.InputMouse.Select(cat, names[i]);
                ManagerReport.AddInput("Selected building: " + names[i]);

                //Address the click of a tile of a road
                //and SelectBuilding.name is contained in names, then return true
                if (BuildingPot.Control.Registro.SelectBuilding != null
                    && names.Contains(BuildingPot.Control.Registro.SelectBuilding.name)
                    )
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool SelectSellRegion()
    {
        if (!Input.GetMouseButtonUp(0))
        {
            return false;
        }

        List<string> names = new List<string>();
        var clicked = UPoly.RayCastLayer(Input.mousePosition, 10).transform;

        if (clicked != null && clicked.name.Contains("ForSaleRegion"))
        {
            var sell = clicked.GetComponent<ForSaleRegionGO>();
            sell.ClickOnMe();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Method tht address when a building is unseletced
    /// </summary>
    private void UnselectingBuild()
    {
        _buildingsMenu.Hide();
        _descriptionWindow.Hide();
        _dictSelection = -1;

        Program.InputMain.InputMouse.UnSelectCurrent();
    }

    /// <summary>
    /// Will return the build or way hitt by mouse position
    /// </summary>
    /// <returns></returns>
    private Transform ReturnBuildinHit()
    {
        //used to be UPoly.RayCastAll()
        Transform clicked = UPoly.RayCastLayer(Input.mousePosition, 10).transform;//10: personBlock

        //Removed bz was pulling the Previews of the Buildiggs and in that way will select that building
        ////try ways then
        //if (clicked == null)
        //{
        //    clicked = UPoly.RayCastLayer(Input.mousePosition, 12).transform;//12: way
        //}

        return clicked;
    }

    /// <summary>
    /// Actions to perform from form
    /// </summary>
    /// <param name="action"></param>
    public void ActionFromForm(string action)
    {
        //play click sound
        //AudioCollector.PlayOneShot("ClickMetal2", 0);

        //btn from main menu
        if (action.Contains("MainMenu."))
        {
            Program.MyScreen1.MouseListenAction(action);
            //play click sound
            AudioCollector.PlayOneShot("ClickMetal2", 0);
        }
        else if (action == "upgBuildMat")
        {
            _buildingWindow.ClickedUpdMatBtn();
        }
        else if (action == "upgBuildCap")
        {
            _buildingWindow.ClickedUpdCapBtn();
        }
        else if (action == "Close_Btn")
        {
            _personWindow.Hide();

            _buildingWindow.Hide();
            UnselectingBuild();
        }
        else if (action == "Demolish_Btn")
        {
            DemolishAction();

            _buildingWindow.Reload();
        }
        else if (action == "Cancel_Demolish_Btn")
        {
            CancelDemolishAction();
            _buildingWindow.Reload();
        }
        else if (action == "BullDozer")
        {
            BuildingPot.InputU.BuildNowNew(H.BullDozer);
            Debug.Log(action);
            //_bullDozerGO = Building.CreateBuild("Prefab/Building/BullDozer", m.HitMouseOnTerrain.point, H.BullDozer);
        }
        else if (action.Contains("Dialog."))
        {
            Dialog.Listen(action);
        }
        else if (action.Contains("GUIBtn."))
        {
            GUIBtnHandlers(action);
        }
        else if (action == H.Next_Stage_Btn.ToString())
        {
            if (BuildingPot.Control.Registro.SelectBuilding.HType.ToString().Contains("Bridge"))
            {
                Bridge b = BuildingPot.Control.Registro.SelectBuilding as Bridge;
                b.ShowNextStageOfParts();
            }
            else
            {
                Structure b = BuildingPot.Control.Registro.SelectBuilding as Structure;
                b.ShowNextStage();
            }
        }
        //Handling GUI : Aug 2015
        else
        {
            HandleGUIClicks(action);
        }
    }

    /// <summary>
    /// Handle actions from buttons in GUI
    /// </summary>
    /// <param name="action"></param>
    private void GUIBtnHandlers(string action)
    {
        action = action.Substring(7);

        if (action == "Menu")
        {
            Program.InputMain.EscapeKey();
        }
        else if (action == "QuickSave")
        {
            Program.InputMain.QuickSaveNow();
        }
        else if (action == "Share")
        {
        }
        else if (action == "MoreSpeed")
        {
            Program.InputMain.ChangeGameSpeedBy(1);
        }
        else if (action == "LessSpeed")
        {
            Program.InputMain.ChangeGameSpeedBy(-1);
        }
        else if (action == "Pause")
        {
            Program.InputMain.ChangeGameSpeedBy(-15);
        }
        else if (action == "Feedback")
        {
            //Dialog.InputFormDialog(H.Feedback);
            Application.OpenURL("https://steamcommunity.com/app/538990/discussions/1/");
        }
        else if (action == "BugReport")
        {
            Dialog.InputFormDialog(H.BugReport);
        }
        //invite a friend to the Closed beta
        else if (action == "Invitation")
        {
            Dialog.InputFormDialog(H.Invitation);
        }
        //Social Media
        else if (action == "Facebook")
        {
            Application.OpenURL("https://www.facebook.com/Aatlantis-455576847932828/");
        }
        else if (action == "Twitter")
        {
            Application.OpenURL("https://twitter.com/aatlantis_code/");
        }
        else if (action == "Youtube")
        {
            Application.OpenURL("https://www.youtube.com/channel/UCJ1VFhaQw6dYCYSlEjZ555Q/");
        }
        else if (action == "ShareFacebook")
        {
            //if (FB.IsInitialized)
            //{
            //    Application.OpenURL("https://www.facebook.com/Aatlantis-455576847932828/");
            //    Uri uri = new Uri("http://steamcommunity.com/sharedfiles/filedetails/?id=642347935");
            //    FB.ShareLink(uri, "Title", "Desc");
            //}

            //string facebookshare = "https://www.facebook.com/sharer/sharer.php?u=" + Uri.EscapeUriString("http://steamcommunity.com/sharedfiles/filedetails/?id=642347935");
            //Application.OpenURL(facebookshare);

            //string twittershare = "http://twitter.com/home?status=" + Uri.EscapeUriString(appStoreLink);
            //Application.OpenURL(twittershare);

            string fbFeed =
                "https://www.facebook.com/dialog/feed?app_id=145634995501895&display=popup&caption=An%20example%20caption&link=https%3A%2F%2Fdevelopers.facebook.com%2Fdocs%2Fdialogs%2F&redirect_uri=https://developers.facebook.com/tools/explorer";

            Application.OpenURL(fbFeed);
        }
        else if (action.Contains("Test."))
        {
            action = action.Substring(5);
            TestAchieve(action);
        }
    }

    private void TestAchieve(string cmd)
    {
        //m_StatsAndAchievements.Render();
        //GUILayout.Space(10);

        if (cmd == "100")
        {
            m_StatsAndAchievements.AddDistanceTraveled(100.0f);
            print("Added 100");
        }
        else if (cmd == "Reset")
        {
            m_StatsAndAchievements.ResetAll();
        }
        else if (cmd == "Render")
        {
            m_StatsAndAchievements.Render();
        }
    }

    #region GUI. Aug 2015

    //this is holding the index of _inputListDict on InputBuilding
    private int _dictSelection = -1;

    private BuildingsMenu _buildingsMenu;
    private DescriptionWindow _descriptionWindow;

    private PersonWindow _personWindow;
    private BuildingWindow _buildingWindow;
    private AddOrderWindow _addOrderWindow;
    private NotificationWindowGO _notificationWindow;

    private QuestWindow _questWindow;
    private HelpWindow _helpWindow;
    private TutoWindow _tutoWindow;

    public HelpWindow HelpWindow
    {
        get { return _helpWindow; }
        set { _helpWindow = value; }
    }

    internal TutoWindow TutoWindow1
    {
        get
        {
            return _tutoWindow;
        }

        set
        {
            _tutoWindow = value;
        }
    }

    /// <summary>
    /// Will handle all the inputs from the buttons on the GUI
    /// </summary>
    /// <param name="action"></param>
    private void HandleGUIClicks(string action)
    {
        //when adding a nw import or export . clicked on Dock orders tab
        if (action == "Add_Export_Btn" || action == "Add_Import_Btn")
        {
            AddExportImport(action);
        }
        //when selecting a prod on _addOrderForm
        else if (action.Contains("AddOrder."))
        {
            _addOrderWindow.FeedFromForm(action);
        }
        else if (action.Contains("BuildingForm."))
        {
            _buildingWindow.FeedFromForm(action);
        }
        //building new buildins
        else if (action.Contains("Slot"))
        {
            HandleSlot(action);
        }
        //clicking on the categories of buildings . ex Road
        else
        {
            HandleBuildCat(action);
        }
    }

    private void AddExportImport(string action)
    {
        HideBulletinHelp();
        if (action == "Add_Export_Btn")
        {
            _addOrderWindow.Show("Export");
        }
        else if (action == "Add_Import_Btn")
        {
            _addOrderWindow.Show("Import");
        }
    }

    void HideBulletinHelp()
    {
        _bulletinWindow.Hide();
        _helpWindow.Hide();
    }

    /// <summary>
    /// Will hide the Person, Building, and AddOrder Window
    /// </summary>
    public void HidePersonBuildOrderNotiWindows()
    {
        if (_miniHelper == null)
        {
            _miniHelper = FindObjectOfType<MiniHelper>();
        }
        if (_miniHelper != null)
        {
            _miniHelper.Hide();
        }

        _personWindow.Hide();
        UnselectingBuild();
        _addOrderWindow.Hide();
        _notificationWindow.Hide();
    }

    public void HidePersonBuildOrderNotiBulletinHelpWin()
    {
        HidePersonBuildOrderNotiWindows();

        _buildingsMenu.Hide();
        _descriptionWindow.Hide();

        _buildingWindow.Hide();
        BulletinWindow.Hide();
        _helpWindow.Hide();

        QuestWindow.Hide();
    }

    public void HidePersonBuildingOrderBulletin()
    {
        _personWindow.Hide();
        _buildingWindow.Hide();
        BulletinWindow.Hide();
        _addOrderWindow.Hide();
    }

    public void HideBuildingsMenu()
    {
        _buildingsMenu.Hide();
        _descriptionWindow.Hide();

        _dictSelection = -1;

        _descriptionWindow.Hide();
    }

    /// <summary>
    /// This is when click on , Road, or House
    /// </summary>
    /// <param name="action"></param>
    private void HandleBuildCat(string action)
    {
        HidePersonBuildOrderNotiWindows();

        _dictSelection = ReturnDictSelection(action);

        LoadIconsOnMenu();
    }

    /// <summary>
    /// Will load the Icons on the Menu
    /// </summary>
    private void LoadIconsOnMenu()
    {
        var dict = BuildingPot.InputU.InputListDict[_dictSelection];
        var list = ReturnHList(dict);
        _buildingsMenu.Show(list);
    }

    /// <summary>
    /// Will return the H values of the Dict
    /// </summary>
    /// <param name="dict"></param>
    /// <returns></returns>
    private List<H> ReturnHList(Dictionary<KeyCode, H> dict)
    {
        List<H> res = new List<H>();

        foreach (var item in dict)
        {
            res.Add(item.Value);
        }

        return res;
    }

    /// <summary>
    /// Depending on the Category of building selected will
    ///  return wichi index is for '_inputListDict' on InputBuilding.cs
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private int ReturnDictSelection(string action)
    {
        var res = -1;
        if (action == "Infrastructure")
        {
            res = 0;
        }
        else if (action == "House")
        {
            res = 1;
        }
        else if (action == "Food")
        {
            res = 2;
        }
        else if (action == "Raw")
        {
            res = 3;
        }
        else if (action == "Prod")
        {
            res = 4;
        }
        else if (action == "Ind")
        {
            res = 5;
        }
        else if (action == "Trade")
        {
            Program.gameScene.TutoStepCompleted("Trade.Tuto");
            res = 6;
        }
        else if (action == "Gov")
        {
            res = 7;
        }
        else if (action == "Other")
        {
            res = 8;
        }
        else if (action == "Militar")
        {
            res = 9;
        }
        else if (action == "Decoration")
        {
            res = 10;
        }
        return res;
    }

    /// <summary>
    /// Handles a Slot. This is where 1 would be HouseA, and 2 HouseB
    ///
    /// This is handlign the click action
    /// </summary>
    /// <param name="action"></param>
    private void HandleSlot(string action)
    {
        var dict = BuildingPot.InputU.InputListDict[_dictSelection];
        InputBuilding.InputMode = Mode.Building;
        var key = ReturnKeyCode(action);

        //means is click an empty slot
        if (!dict.ContainsKey(key))
        {
            return;
        }

        var val = dict[key];
        BuildingPot.InputU.BuildingSwitch(val);

        _buildingsMenu.Hide();
        _descriptionWindow.Hide();

        _dictSelection = -1;
    }

    /// <summary>
    /// With the Slot1 as 'action' will return wht keyCode is
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    private KeyCode ReturnKeyCode(string action)
    {
        if (action == "Slot1")
        {
            return KeyCode.Alpha1;
        }
        if (action == "Slot2")
        {
            return KeyCode.Alpha2;
        }
        if (action == "Slot3")
        {
            return KeyCode.Alpha3;
        }
        if (action == "Slot4")
        {
            return KeyCode.Alpha4;
        }
        if (action == "Slot5")
        {
            return KeyCode.Alpha5;
        }
        if (action == "Slot6")
        {
            return KeyCode.Alpha6;
        }
        if (action == "Slot7")
        {
            return KeyCode.Alpha7;
        }
        if (action == "Slot8")
        {
            return KeyCode.Alpha8;
        }
        if (action == "Slot9")
        {
            return KeyCode.Alpha9;
        }
        if (action == "Slot10")
        {
            return KeyCode.Alpha0;
        }
        return KeyCode.None;
    }

    /// <summary>
    /// This is to know which H is the one is clicked on slot
    ///
    /// Knowing already wht category was touched is easy
    ///
    ///
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public H ReturnThisSlotVal(string slot)
    {
        if (_dictSelection == -1)
        {
            return H.None;
        }

        var dict = BuildingPot.InputU.InputListDict[_dictSelection];

        for (int i = 0; i < dict.Count; i++)
        {
            KeyCode code = ReturnKeyCode(slot);

            if (!dict.ContainsKey(code))
            {
                //to avoid when Buildins category dont have the whole 10 alpha codes. which is majority
                return H.None;
            }

            return dict[code];
        }
        return H.None;
    }

    #endregion GUI. Aug 2015

    #region Person Selection Form

    private Person _personSelect;
    private MiniHelper _miniHelper;
    private BulletinWindow _bulletinWindow;

    public BulletinWindow BulletinWindow
    {
        get { return _bulletinWindow; }
        set { _bulletinWindow = value; }
    }

    #endregion Person Selection Form

    private void CancelDemolishAction()
    {
        if (BuildingPot.Control.Registro.SelectBuilding.Category == Ca.Structure)
        {
            Structure b = BuildingPot.Control.Registro.SelectBuilding as Structure;
            b.CancelDemolish();
        }
    }

    private void DemolishAction()
    {
        //print("Selected name:" + b.name);
        if (BuildingPot.Control.Registro.SelectBuilding.Category == Ca.Way)
        {
            Trail b = BuildingPot.Control.Registro.SelectBuilding as Trail;
            b.Demolish();
        }
        else if (BuildingPot.Control.Registro.SelectBuilding.Category == Ca.Structure
           || BuildingPot.Control.Registro.SelectBuilding.Category == Ca.Shore)
        {
            Structure b = BuildingPot.Control.Registro.SelectBuilding as Structure;

            if (BuildingPot.Control.IsThisTheLastFoodSrc(b))
            {
                Program.gameScene.GameController1.NotificationsManager1.MainNotify("LastFood");
                return;
            }
            if (BuildingPot.Control.IsThisTheLastOfThisType(H.Masonry, b))
            {
                Program.gameScene.GameController1.NotificationsManager1.MainNotify("LastMasonry");
                return;
            }
            if (BuildingPot.Control.Registro.AreUDemolishingOneAlready())
            {
                Program.gameScene.GameController1.NotificationsManager1.MainNotify("OnlyOneDemolish");
                return;
            }

            var wasDemolished = b.Demolish();
            if (wasDemolished)
            {
                OnDemolish(EventArgs.Empty);
            }
        }
        else if (BuildingPot.Control.Registro.SelectBuilding.Category == Ca.DraggableSquare)
        {
            DragSquare b = BuildingPot.Control.Registro.SelectBuilding as DragSquare;
            b.Demolish();
        }

        //Program.InputMain.InputMouse.UnSelectCurrent();
        //DestroyForm();
    }

    private EventHandler<EventArgs> _demolished;

    public EventHandler<EventArgs> Demolished
    {
        get { return _demolished; }
        set { _demolished = value; }
    }

    public QuestWindow QuestWindow
    {
        get
        {
            return _questWindow;
        }

        set
        {
            _questWindow = value;
        }
    }

    private void OnDemolish(EventArgs e)
    {
        if (Demolished != null)
        {
            Demolished(this, e);
        }
    }

    // Update is called once per frame
    public void Update()
    {
    }

    public void CreateNewForm(H type)
    {
        DestroyForm();

        if (type == H.Selection)
        {
            _buildingWindow.Show(BuildingPot.Control.Registro.SelectBuilding);
        }
    }

    public void DestroyForm()
    {
        if (_currForm != null)
        {
            _currForm.Destroy();
            _currForm = null;
        }
    }

    public static bool MouseOnWindowNow { get; set; }

    public bool IsAWindowShownNow()
    {
        if (_buildingsMenu == null)
        {
            return false;
        }

        return _buildingsMenu.IsShownNow() || _descriptionWindow.IsShownNow() ||
            _personWindow.IsShownNow() || _buildingWindow.IsShownNow() || _addOrderWindow.IsShownNow() ||
            BulletinWindow.IsShownNow()
            || QuestWindow.IsShownNow() || _helpWindow.IsShownNow();
    }

    public bool IsAWindowScrollableShownNow()
    {
        if (_buildingsMenu == null)
        {
            return false;
        }

        return _addOrderWindow.IsShownNow() || BulletinWindow.IsShownNow() //|| _notificationWindow.IsShownNow()
            || QuestWindow.IsShownNow() || _helpWindow.IsShownNow() || MouseOnWindowNow;
    }

    internal void ClickOnAnInvItem(InvItem _invItem)
    {
        HidePersonBuildOrderNotiWindows();
        BulletinWindow.Show();
        BulletinWindow.ShowSpecs();
    }
}