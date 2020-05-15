using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildingWindow : Window
{
    private Text _title;
    private Text _info;
    private Text _inv;
    private Text _displayProdInfo;

    private Building _building;

    public Building Building
    {
        get { return _building; }
        set { _building = value; }
    }

    private Vector3 iniPos;

    private Rect _genBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    private Rect _invBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    private Rect _ordBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    private Rect _prdBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    private Rect _upgBtnRect;
    private Rect _staBtnRect;

    private GameObject _genBtn;//the btn
    private GameObject _invBtn;//the btn
    private GameObject _ordBtn;//the btn
    private GameObject _prdBtn;//the btn
    private GameObject _staBtn;//the btn

    //tabs
    private GameObject _general;

    private GameObject _gaveta;
    private GameObject _upgrades;
    private GameObject _products;
    private GameObject _orders;
    private GameObject _stats;
    private GameObject _invIniPos;
    private GameObject _invIniPosSta;

    private Vector3 _importIniPos;
    private Vector3 _exportIniPos;

    private Vector3 _importIniPosOnProcess;
    private Vector3 _exportIniPosOnProcess;
    private GameObject _salary;
    private GameObject _positions;

    //upg btns
    private GameObject _upg_Mat_Btn;

    private GameObject _upg_Cap_Btn; //Upg_Mat_Btn

    private GameObject _demolish_Btn; //Upg_Mat_Btn
    private GameObject _cancelDemolish_Btn; //Upg_Mat_Btn

    //Texts
    private Text _currSalaryTxt;

    private Text _currPositionsTxt;
    private Text _maxPositionsTxt;

    //Image _imageIcon;

    //Scrool
    //private ScrollViewShowInventory _scrollInventory;

    private GameObject _scrollParent;

    //Priority Rank
    private GameObject _priorityControls;

    private Text _currRankTxt;

    // Use this for initialization
    private void Start()
    {
        base.Start();
        InitObj();

        Hide();

        StartCoroutine("ThreeSecUpdate");
        StartCoroutine("FiveSecUpdate");
    }

    private IEnumerator FiveSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // wait

            if (Building != null && Building.CurrentProd != null && _products.activeSelf)
            {
                //in case is a Field Farm updates the progress
                ShowProductDetail();
            }
        }
    }

    private IEnumerator ThreeSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // wait

            var samePos = UMath.nearEqualByDistance(transform.position, iniPos, 1);
            var buildNull = Building == null;

            //means is showing
            if (samePos && !buildNull && BuildingPot.Control.Registro.SelectBuilding != null)
            {
                LoadMenu();

                //then orders need to be reloaded from dispatch and shown on Tab
                if (_orders.gameObject.activeSelf == _orders)
                {
                    ShowOrders();
                }
            }
        }
    }

    private void InitObj()
    {
        iniPos = transform.position;

        _general = GetChildThatContains(H.General);
        _gaveta = GetChildThatContains(H.Gaveta);
        _orders = GetChildThatContains(H.Orders);
        _products = GetChildThatContains(H.Products);
        _upgrades = GetChildCalled(H.Upgrades);
        _stats = GetChildCalled("Stats");

        _scrollParent = FindGameObjectInHierarchy("Inventory_Scroll", gameObject);
        //_scrollInventory = FindGameObjectInHierarchy("Scroll_View", gameObject).GetComponent<ScrollViewShowInventory>();

        _priorityControls = FindGameObjectInHierarchy("PriorityControl", _general);
        var currRank = FindGameObjectInHierarchy("Current_Rank", _priorityControls);
        _currRankTxt = currRank.GetComponent<Text>();

        _salary = General.FindGameObjectInHierarchy("Salary", _general);
        _positions = General.FindGameObjectInHierarchy("Positions", _general);

        var currSalary = FindGameObjectInHierarchy("Current_Salary", _salary);
        _currSalaryTxt = currSalary.GetComponent<Text>();

        var currPos = FindGameObjectInHierarchy("Current_Positions", _positions);
        _currPositionsTxt = currPos.GetComponent<Text>();

        var maxPos = FindGameObjectInHierarchy("Max_Positions", _positions);
        _maxPositionsTxt = maxPos.GetComponent<Text>();

        _title = GetChildCalled(H.Title).GetComponent<Text>();

        _info = GetGrandChildCalled(H.Info).GetComponent<Text>();
        _inv = GetGrandChildCalled(H.Bolsa).GetComponent<Text>();//bolsa bz tht algorith has a bugg tht names cannot be the same or start with the same

        _displayProdInfo = GetGrandChildCalled(H.Display_Lbl).GetComponent<Text>();//bolsa bz tht algorith has a bugg tht names cannot be the same or start with the same

        _invIniPos = GetGrandChildCalled(H.Inv_Ini_Pos);
        _invIniPosSta = GetGrandChildCalled("Inv_Ini_Pos_Sta");

        //_imageIcon = GetChildCalled("Image_Icon").GetComponent<Image>();

        _genBtn = GetChildThatContains(H.Gen_Btn);
        _invBtn = GetChildThatContains(H.Inv_Btn);
        _ordBtn = GetChildThatContains(H.Ord_Btn);

        var upgBtn = GetChildCalled(H.Upg_Btn).transform;
        var prdBtn = GetChildCalled(H.Prd_Btn).transform;
        _prdBtn = GetChildThatContains(H.Prd_Btn);

        _staBtn = GetChildCalled("Sta_Btn");
        var staBtn = GetChildCalled("Sta_Btn").transform;

        _genBtnRect = GetRectFromBoxCollider2D(_genBtn.transform);
        _invBtnRect = GetRectFromBoxCollider2D(_invBtn.transform);
        _ordBtnRect = GetRectFromBoxCollider2D(_ordBtn.transform);
        _upgBtnRect = GetRectFromBoxCollider2D(upgBtn.transform);
        _prdBtnRect = GetRectFromBoxCollider2D(prdBtn.transform);
        _staBtnRect = GetRectFromBoxCollider2D(staBtn.transform);

        _importIniPos = GetGrandChildCalled(H.IniPos_Import).transform.localPosition;
        _exportIniPos = GetGrandChildCalled(H.IniPos_Export).transform.localPosition;

        _importIniPosOnProcess = GetGrandChildCalled(H.IniPos_Import_OnProcess).transform.localPosition;
        _exportIniPosOnProcess = GetGrandChildCalled(H.IniPos_Export_OnProcess).transform.localPosition;

        _upg_Mat_Btn = GetGrandChildCalled(H.Upg_Mat_Btn);
        _upg_Cap_Btn = GetGrandChildCalled(H.Upg_Cap_Btn);

        _demolish_Btn = GetGrandChildCalled(H.Demolish_Btn);//Cancel_Demolish_Btn
        _cancelDemolish_Btn = GetGrandChildCalled(H.Cancel_Demolish_Btn);//Cancel_Demolish_Btn

        _plusBtn = FindGameObjectInHierarchy("More Positions", gameObject);
        _lessBtn = FindGameObjectInHierarchy("Less Positions", gameObject);

        _hireAllBtn = FindGameObjectInHierarchy("Hire All", gameObject);
        _fireAllBtn = FindGameObjectInHierarchy("Fire All", gameObject);

        _salary.SetActive(false);

        var img = _genBtn.GetComponent<Image>();
        _initialTabColor = img.color;
    }

    /// <summary>
    /// The show of the menu in a building
    /// </summary>
    /// <param name="val"></param>
    public void Show(Building val)
    {
        oldBuildID = Building == null ? "" : Building.MyId;
        Building = val;
        Program.MouseListener.HideBuildingsMenu();

        if (Building.HType == H.Dock)
        {
            Program.gameScene.TutoStepCompleted("SelectDock.Tuto");
        }

        LoadMenu();

        LoadInitialTabDependingOnBuilding();

        transform.position = iniPos;
        HandleOrdBtn();
        HandlePrdBtn();

        //CheckIfMatMaxOut();
        CheckIfCapMaxOut();

        HideStuff();
    }

    private void LoadInitialTabDependingOnBuilding()
    {
        if (Building.CurrentStage() != 4)
        {
            MakeThisTabActive(_general);
            return;
        }

        if (Building.HType.ToString().Contains("Storage"))
        {
            MakeThisTabActive(_gaveta);
        }
        else if (Building.HType == H.Dock)
        {
            MakeThisTabActive(_orders);
        }
        else MakeThisTabActive(_general);
    }

    private void DemolishBtn()
    {
        _cancelDemolish_Btn.SetActive(false);

        if (Building.Instruction == H.WillBeDestroy //|| !fullyBuilt
            )
        {
            _demolish_Btn.SetActive(false);
            //_cancelDemolish_Btn.SetActive(true);
        }
        else
        {
            //todo uncomment so it active
            _demolish_Btn.SetActive(true);
        }
    }

    private void HideStuff()
    {
        _invBtn.SetActive(true);

        if (Building.IsHouseType(Building.MyId) || Building.MyId.Contains("Storage") || Building.Category == Ca.Way ||
            Building.HType == H.Masonry || Building.HType == H.HeavyLoad
            || Building.HType == H.LightHouse
            || Building.IsNaval()
            || Building.HType == H.Church || Building.HType == H.Tavern || Building.HType == H.TownHouse
            || Building.HType == H.Library
            || Building.HType == H.School || Building.HType == H.TradesSchool)
        {
            _salary.SetActive(false);
            _staBtn.SetActive(false);
            _prdBtn.SetActive(false);
        }
        else
        {
            _salary.SetActive(true);
            _staBtn.SetActive(true);
            _prdBtn.SetActive(true);
        }

        if (Building.HType == H.Masonry || Building.HType == H.HeavyLoad || Building.HType == H.LightHouse
            || Building.IsNaval()
            || Building.HType == H.Church || Building.HType == H.Tavern || Building.HType == H.TownHouse
            || Building.HType == H.Library)
        {
            _salary.SetActive(true);
        }

        if (Building.Instruction == H.WillBeDestroy)
        {
            _salary.SetActive(false);
            _staBtn.SetActive(false);
            _prdBtn.SetActive(false);
        }

        if (isADecorationBuilding(_building) || _building.IsMilitar() || _building.HType == H.Road)
        {
            _salary.SetActive(false);
            _staBtn.SetActive(false);
            _prdBtn.SetActive(false);
            _invBtn.SetActive(false);
        }

        _salary.SetActive(false);
    }

    /// <summary>
    /// Bz Houses, Gov, Trade Structures, Ways , etc dont have a prod the prod tab must be
    /// hidden
    ///
    ///
    /// </summary>
    private void HandlePrdBtn()
    {
        //todo add all houses, trade, gov as Cointned in their Enum Cat
        if (isToHidePrdTab())
        {
            _prdBtn.SetActive(false);
        }
        else
        {
            _prdBtn.SetActive(true);
        }
    }

    private bool isToHidePrdTab()
    {
        return Building.HType.ToString().Contains("House") || Building.Category == Ca.Way || Building.IsNaval();
    }

    /// <summary>
    /// Will hide it if not Dock or ...
    /// </summary>
    private void HandleOrdBtn()
    {
        if (Building.HType != H.Dock || Building.HType != H.Shipyard || Building.HType != H.Supplier)
        {
            _ordBtn.SetActive(false);
        }
        if (Building.HType == H.Dock || Building.HType == H.Shipyard || Building.HType == H.Supplier)
        {
            _ordBtn.SetActive(true);
        }
    }

    /// <summary>
    /// Will hide salary and positions if is not fully built
    /// </summary>
    private void HideShowSalAndPositions()
    {
        bool fullyBuilt = Building.IsFullyBuilt();
        bool isAWorkPlace = isAWorkBuilding(Building);

        if (fullyBuilt && isAWorkPlace && Building.Instruction != H.WillBeDestroy)
        {
            _positions.SetActive(true);
        }
        else
        {
            _salary.SetActive(false);
            _positions.SetActive(false);
        }
    }

    private ShowAInventory _showAInventory;
    private int oldItemsCount;
    private string oldBuildID;

    private void LoadMenu()
    {
        HidePriorityControls();

        HideShowSalAndPositions();
        //LoadImageIcon();

        HideShow();

        _title.text = Building.NameBuilding();
        _info.text = BuildInfo() + BuildCover();

        Inventory();

        //breaks with the TUto
        if (BuildingPot.Control.Registro.SelectBuilding == null)
        {
            BuildingPot.Control.Registro.SelectBuilding = Building;
        }

        _currSalaryTxt.text = BuildingPot.Control.Registro.SelectBuilding.DollarsPay + "";
        _currPositionsTxt.text = BuildingPot.Control.Registro.SelectBuilding.MaxPeople + "";
        _maxPositionsTxt.text = Book.GiveMeStat(Building.HType).MaxPeople + "";

        _currRankTxt.text = PersonPot.Control.BuildersManager1.CurrentPriorityRank(Building.MyId);

        DemolishBtn();
    }

    private void HideShow()
    {
        _priorityControls.SetActive(false);
    }

    private void LoadImageIcon()
    {
        var iconRoot = Root.RetBuildingIconRoot(_building.HType);
        var s = (Sprite)Resources.Load(iconRoot, typeof(Sprite));

        //_imageIcon.sprite = s;
    }

    public void ResetShownInventory()
    {
        oldBuildID = "";
    }

    private void Inventory()
    {
        if (Building.Inventory == null) return;

        if (oldBuildID != Building.MyId || Building.IsToReloadInv())
        {
            ShowProductionReport();
            oldBuildID = Building.MyId;
            //_scrollInventory.ReloadNewInventory(Building.Inventory, 0);//pad at 1.7 works fine but it cuts the first item
        }

        ReloadStatsWhenNeeded();
        _inv.text = BuildStringInv(Building);
    }

    /// <summary>
    /// Will redo Stats
    /// </summary>
    private void ReloadStatsWhenNeeded(bool now = false)
    {
        if (now)
        {
            ShowProductionReport();
            return;
        }

        //only updated when a production happened Building.IsToReloadInv()
        if (Building.IsToReloadInv())
        {
            ShowProductionReport();
            Building.InvWasReloaded();
        }
    }

    private List<ShowAInventory> _reports = new List<ShowAInventory>();

    private void ShowProductionReport()
    {
        for (int i = 0; i < _reports.Count; i++)
        {
            _reports[i].DestroyAll();
        }
        _reports.Clear();

        var pastItems = 0;
        var lastPos = _invIniPosSta.transform.localPosition;
        for (int i = 0; i < ShowLastYears(); i++)
        {
            var margin = i > 0 ? -4f : -1f;
            var yPos = (pastItems * -3.75f) + margin;
            var a = new ShowAInventory(Building.ProductionReport.ProduceReport[i], _stats.gameObject,
                lastPos + new Vector3(0, yPos, 0));

            lastPos = lastPos + new Vector3(0, yPos, 0);

            _reports.Add(a);
            pastItems = Building.ProductionReport.ProduceReport[i].InventItems.Count;
        }
    }

    /// <summary>
    /// so it only shows the last 5 years or less if less
    /// </summary>
    /// <returns></returns>
    private int ShowLastYears()
    {
        if (Building.ProductionReport == null)
        {
            return 0;
        }
        if (Building.ProductionReport.ProduceReport.Count < 6)
        {
            return Building.ProductionReport.ProduceReport.Count;
        }
        return 5;
    }

    /// <summary>
    /// Schools, Church, and Tavern have coverage
    /// </summary>
    /// <returns></returns>
    private string BuildCover()
    {
        if (Building.HType == H.Road || Building.MyId.Contains("Bridge"))
        {
            return "";
        }

        var st = (Structure)Building;
        return st.CoverageInfo();
    }

    public static bool isADecorationBuilding(Building build)
    {
        var isDecoration =
            build.HType == H.Fountain ||
            build.HType == H.WideFountain ||
            build.HType == H.PalmTree ||
            build.HType == H.FloorFountain ||
            build.HType == H.FlowerPot ||
            build.HType == H.PradoLion;

        return isDecoration;
    }

    public static bool isAWorkBuilding(Building build)
    {
        var isAHouse = build.IsThisAHouseType();
        var isStorage = build.HType.ToString().Contains("Storage");
        var isRoadorBridge = build.HType.ToString().Contains("Road") || build.HType.ToString().Contains("Bridge");

        var isDecoration = isADecorationBuilding(build);

        return !isAHouse && !isStorage && build.HType != H.StandLamp && !isDecoration && !isRoadorBridge;
    }

    private string BuildInfo()
    {
        CleanPeopleTile();
        string res = Languages.ReturnString(Building.HType + ".Desc") + "\n";

        res += IfInConstructionAddPercentageOfCompletion();

        var isAHouse = Building.IsThisAHouseType();

        //is not a house or bohio
        if (!isAHouse || Building.HType == H.LightHouse)//must say lightHouse here bz actualkly contains House
        {
            if (Building.HType == H.Masonry)
            {
                res += Languages.ReturnString("Buildings.Ready");
                for (int i = 0; i < Building.BuildersManager1.GreenLight.Count; i++)
                {
                    var st = Brain.GetStructureFromKey(Building.BuildersManager1.GreenLight[i].Key);
                    if (st != null)
                    {
                        res += "\n" + st.Name;
                    }
                }
            }
        }
        //is a house or bohio
        else
        {
            var amt = 0;
            for (int i = 0; i < Building.Families.Count(); i++)
            {
                amt += Building.Families[i].MembersOfAFamily();
            }

            res += Languages.ReturnString("People.Living") + " " + amt;
            TilesOfPeopleInAHouse();
        }

        res = DestroyingBuilding(res);

        return res
#if UNITY_EDITOR
            //+ DebugInfo()
#endif
            ;
    }

    #region PeopleTile

    private int iForSpwItem;
    private Vector3 _peopleTileIniPos;

    //this is the items they are a Key and a Value
    private List<PersonTile> _tiles = new List<PersonTile>();

    private Building _oldBuilding;

    private void CleanPeopleTile()
    {
        var oldBuild = _oldBuilding != null && _oldBuilding == _building;
        var sameTiles = Building.Families != null && Building.Families[0].MembersOfAFamily() == _tiles.Count;

        if (oldBuild && sameTiles)
        {
            return;
        }

        for (int i = 0; i < _tiles.Count; i++)
        {
            _tiles[i].Destroy();
        }
        _tiles.Clear();
        iForSpwItem = 0;
    }

    private void TilesOfPeopleInAHouse()
    {
        if (_tiles.Count > 0)
        {
            return;
        }

        _oldBuilding = _building;
        var iniGO = FindGameObjectInHierarchy("IniPos_Person_Tile", gameObject);
        _peopleTileIniPos = iniGO.transform.localPosition;

        for (int i = 0; i < Building.Families[0].MembersOfAFamily(); i++)
        {
            var personP = Building.Families[0].MemberAt(i);

            _tiles.Add(PersonTile.Create(_general.transform, ReturnIniPos(iForSpwItem), personP));
            iForSpwItem++;
        }
    }

    private Vector3 ReturnIniPos(int i)
    {
        return new Vector3(_peopleTileIniPos.x, ReturnY(i) + _peopleTileIniPos.y, _peopleTileIniPos.z);
    }

    private float ReturnY(int i)
    {
        return -3.9f * i;
    }

    #endregion PeopleTile

    private string DestroyingBuilding(string current)
    {
        if (Building.Instruction == H.WillBeDestroy)
        {
            return Languages.ReturnString("Build.Destroy.Soon");
        }
        return current;
    }

    private string ReturnAvailablePositions()
    {
        var res = "Available positions:";
        var availPos = Building.MaxPeople - Building.PeopleDict.Count;

        if (availPos < 0)
        {
            availPos = 0;
        }
        return res + " " + availPos;
    }

    /// <summary>
    /// If is in construction will add percentage of completion
    /// </summary>
    /// <returns></returns>
    private string IfInConstructionAddPercentageOfCompletion()
    {
        if (_building.HType == H.Road) return "";

        StructureParent sP = Building.ReturnCurrentStructureParent();

        if (sP.CurrentStage != 4)
        {
            ShowPriorityControls();

            var percentage = sP.PercentageBuiltCured();
            return "\n\n" + Languages.ReturnString("Construction.Progress") + percentage + "%\n" +
                MaterialsGathered() + "\n" + MaterialsIsMissing() + "\n\n";
        }
        HidePriorityControls();

        return "";
    }

    /// <summary>
    /// If a building is missing some materials/resources to get built will be shown here
    /// </summary>
    /// <returns></returns>
    private string MaterialsIsMissing()
    {
        var pass = BuildersManager.CanGreenLight(Building.HType);
        bool wasGreen = PersonPot.Control.BuildersManager1.WasIGreenLight(Building.MyId);

        if (!pass && !wasGreen)
        {
            return Languages.ReturnString("Warning.This.Building") +
                BuildersManager.MissingResources(Building.HType) +
                PersonPot.Control.BuildersManager1.PriorityInfo(Building.MyId);
        }

        return "";
    }

    private void ShowPriorityControls()
    {
        _priorityControls.SetActive(true);
    }

    private void HidePriorityControls()
    {
        _priorityControls.SetActive(false);
    }

    public void ClickOnGoUpOnRankPriority()
    {
        Debug.Log("up");
        PersonPot.Control.BuildersManager1.ChangePriority(Building.MyId, 1);
        LoadMenu();
    }

    public void ClickOnGoDownOnRankPriority()
    {
        Debug.Log("down");
        PersonPot.Control.BuildersManager1.ChangePriority(Building.MyId, -1);
        LoadMenu();
    }

    private string MaterialsGathered()
    {
        return DescriptionWindow.CostOfABuilding(Building.HType, 1);
    }

    private string DebugInfo()
    {
        string res = "\n___________________\n";

        //is not a house or bohio
        if (!Building.HType.ToString().Contains("House"))
        {
            res += "Type:" + Building.HType
             + "\n ID:" + Building.MyId
            + "\n Recommended max workers:" + Book.GiveMeStat(Building.HType).MaxPeople;
        }
        else
        {
            res += "Type:" + Building.HType +
                 " ID:" + Building.MyId
                 ;

            if (Building.BookedHome1 != null)
            {
                res += " IsBooked:" + Building.BookedHome1.IsBooked();
            }
            else
            {
                res += " IsBooked: no";
            }
        }

        return res;
    }

    #region Salary

    /// <summary>
    /// When the use clicks to change the salary on a building
    /// </summary>
    /// <param name="action"></param>
    public void ClickedOnChangeSalaryCheckBox(string action)
    {
        //change salary
        _currSalaryTxt.text = BuildingPot.Control.Registro.SelectBuilding.ChangeSalary(action);
    }

    public void ClickedOnChangeMaxAmtOfWorkers(string action)
    {
        _currPositionsTxt.text = BuildingPot.Control.Registro.SelectBuilding.ChangeMaxAmoutOfWorkers(action);
    }

    #endregion Salary

    // Update is called once per frame
    private void Update()
    {
        //if click gen
        if (_genBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0))
        {
            MakeThisTabActive(_general);
        }
        //ig click inv
        else if (_invBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0))
        {
            MakeThisTabActive(_gaveta);
        }
        //if click ord
        else if (Building != null && _ordBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0)
            && Building.IsNaval())
        {
            MakeThisTabActive(_orders);
            Program.gameScene.TutoStepCompleted("OrderTab.Tuto");
        }
        else if (_upgBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0))
        {
            MakeThisTabActive(_upgrades);
        }
        else if (_prdBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0))
        {
            MakeThisTabActive(_products);
        }
        else if (_staBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0))
        {
            MakeThisTabActive(_stats);
        }

        CheckIfLessIsActive();
        CheckIfPlusIsActive();
    }

    private GameObject currentActiveTab;

    /// <summary>
    /// Use to swith Tabs on Window. Will hide all and make the pass one as active
    /// </summary>
    /// <param name="g"></param>
    private void MakeThisTabActive(GameObject g)
    {
        DestroyAllProducts();

        if (Building == null || _orders == null || _products == null)
        {
            return;
        }

        //first time loaded ever in game
        if (g == null || (!Building.IsNaval() && g == _orders) || (g == _products && isToHidePrdTab()))
        {
            g = _general;
        }

        ResetPanelsAndTabs();

        g.SetActive(true);
        currentActiveTab = g;

        //then orders need to be Pull from dispatch and shown on Tab
        if (g == _orders)
        {
            ColorTabActive(_ordBtn);
            ShowOrders();
        }
        else if (g == _products)
        {
            ColorTabActive(_prdBtn);
            ShowProducts();
        }
        else if (g == _gaveta)
        {
            ColorTabActive(_invBtn);
        }
        else if (g == _stats)
        {
            ColorTabActive(_staBtn);
        }
        else
            ColorTabActive(_genBtn);
    }

    private void ResetPanelsAndTabs()
    {
        _general.SetActive(false);
        _gaveta.SetActive(false);
        _orders.SetActive(false);
        _upgrades.SetActive(false);
        _products.SetActive(false);
        _stats.SetActive(false);

        ColorTabInactive(_genBtn);
        ColorTabInactive(_invBtn);
        ColorTabInactive(_ordBtn);
        ColorTabInactive(_prdBtn);
        ColorTabInactive(_staBtn);
    }

    /// <summary>
    /// Show Prod on Tab
    /// </summary>
    private void ShowProductDetail()
    {
        MarkProductAsSelected(Building.CurrentProd.Id);

        Building.CurrentProd.BuildDetails();//so they update if needed
        _displayProdInfo.text = Building.CurrentProd.Details;

        if (!_building.DoIHaveInput())
        {
            _displayProdInfo.text = Languages.ReturnString("Product.Selected") + Building.CurrentProd.Product + "\n"
                + _building.MissingInputs();
        }

        //Showing additional info for FieldFarms
        if (Building.HType.ToString().Contains("FieldFarm"))
        {
            var st = (Structure)Building;
            //add string with current crop status and past crop

            if (st.FieldFarm1() != null && st.FieldFarm1().HarvestDate() != null)
            {
                _displayProdInfo.text += Languages.ReturnString("Harvest.Date") + st.FieldFarm1().HarvestDate().ToStringFormatMonYear();
                _displayProdInfo.text += Languages.ReturnString("Progress") + st.FieldFarm1().PercentageDone();
            }
        }
    }

    private void ShowProducts()
    {
        DestroyAndCleanShownOrders();

        var list = Building.ShowProductsOfBuild();
        DisplayProducts(list, Root.showGenBtnLarge);

        ShowProductDetail();
    }

    private void DisplayProducts(List<ProductInfo> list, string root)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Display1String(i, list[i], root);
        }
    }

    private List<OrderShow> _showProducts = new List<OrderShow>();

    private void Display1String(int i, ProductInfo pInfo, string root)
    {
        var orderShow = OrderShow.Create(root, _products.transform, pInfo);
        orderShow.ShowToSetCurrentProduct(pInfo);

        orderShow.Reset(i);
        _showProducts.Add(orderShow);
    }

    ///Show  Orders on tab
    /// <summary>
    /// Show orders routine
    /// </summary>
    public void ShowOrders()
    {
        DestroyAllProducts();
        DestroyOrdersIfDone();

        ShowImportOrders();
        ShowImportOrdersOnProcess();

        ShowExportOrders();
        ShowExportOrdersOnProcess();
    }

    private void ShowExportOrders()
    {
        var expOrd = Building.Dispatch1.ReturnRegularOrders();
        DisplayOrders(expOrd, _exportIniPos, Root.orderShowClose);
    }

    private void ShowExportOrdersOnProcess()
    {
        var expOrd = Building.Dispatch1.ReturnExportOrdersOnProcess();
        DisplayOrders(expOrd, _exportIniPosOnProcess, Root.orderShow, true);
    }

    /// <summary>
    /// Show import orders
    /// </summary>
    private void ShowImportOrders()
    {
        var impOrd = Building.Dispatch1.ReturnImportOrdersOnProcess();
        DisplayOrders(impOrd, _importIniPos, Root.orderShowClose);
    }

    private void ShowImportOrdersOnProcess()
    {
        var impOrd = Building.Dispatch1.ReturnEvacuaOrders();
        DisplayOrders(impOrd, _importIniPosOnProcess, Root.orderShow, true);
    }

    /// <summary>
    /// Display the orders are passed on 'list'
    /// </summary>
    /// <param name="list"></param>
    /// <param name="iniPosP"></param>
    private void DisplayOrders(List<Order> list, Vector3 iniPosP, string root, bool isOnProcess = false)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Display1Order(i, list[i], iniPosP, root, isOnProcess);
        }
    }

    private List<ShowOrderTileWithIcons> _showOrders = new List<ShowOrderTileWithIcons>();

    /// <summary>
    /// Will display the order is pass as param. Bz 'i' will keep looping and puttin the towards the botton of the
    /// _orders tab. Will make the orders Childs of _order tab
    /// </summary>
    /// <param name="i"></param>
    /// <param name="order"></param>
    private void Display1Order(int i, Order order, Vector3 iniPosP, string root, bool isOnProcess = false)
    {
        var isOrderOnList = _showOrders.Find(a => a.OrderId == order.ID);

        //brand new tile
        if (isOrderOnList == null)
        {
            var orderShow = ShowOrderTileWithIcons.Create(root, _orders.transform);
            orderShow.Show(order);
            orderShow.Reset(i, order.TypeOrder, iniPosP, isOnProcess);
            _showOrders.Add(orderShow);
        }
        //update existing
        else
        {
            isOrderOnList.Show(order);
            isOrderOnList.Reset(i, order.TypeOrder, iniPosP, isOnProcess);
        }
    }

    private void DestroyAllProducts()
    {
        for (int i = 0; i < _showProducts.Count; i++)
        {
            _showProducts[i].Destroy();
        }
        _showProducts.Clear();
    }

    /// <summary>
    /// Will destoy all orders Shown
    /// </summary>
    private void DestroyAndCleanShownOrders()
    {
        for (int i = 0; i < _showOrders.Count; i++)
        {
            _showOrders[i].Destroy();
        }
        _showOrders.Clear();
    }

    private void DestroyOrdersIfDone()
    {
        for (int i = 0; i < _showOrders.Count; i++)
        {
            var impOrd = Building.Dispatch1.ReturnEvacuaOrders();

            var tile = _showOrders[i];
            var isStillOnDispatch = Building.Dispatch1.DoYouHaveThisOrderInCurrentLists(tile.Order);

            if (tile.IsDone() || !isStillOnDispatch)
            {
                _showOrders[i].Destroy();
                _showOrders.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// bz when a building is max out in material then the bttuon will hide
    /// </summary>
    private void HideUpgMatBtn()
    {
        _upg_Mat_Btn.SetActive(false);
    }

    /// <summary>
    /// bz when a building is max out in capacity then the bttuon needs to be  hide
    /// </summary>
    private void HideUpgCapBtn()
    {
        //_upg_Cap_Btn.SetActive(false);
    }

    /// <summary>
    /// Once the Upgrate mat bottuon is clicked .
    /// </summary>
    public void ClickedUpdMatBtn()
    {
        Building.UpgradeMatToNext();
        CheckIfMatMaxOut();
    }

    /// <summary>
    /// Upgradint capacity
    /// </summary>
    internal void ClickedUpdCapBtn()
    {
        Building.UpgradeCapToNext();
        CheckIfCapMaxOut();

        _inv.text = BuildStringInv(Building);
    }

    /// <summary>
    /// if has the best material will hide tht button
    /// </summary>
    private void CheckIfMatMaxOut()
    {
        if (Building.IsBuildingMaterialBest())
        {
            HideUpgMatBtn();
        }
    }

    /// <summary>
    /// if has the best material will hide tht button
    /// </summary>
    private void CheckIfCapMaxOut()
    {
        if (Building.IsBuildingCapAtMax())
        {
            HideUpgCapBtn();
        }
    }

    /// <summary>
    /// Called if Mouse listener is called with this : 'BuildingForm.'
    /// </summary>
    /// <param name="action"></param>
    internal void FeedFromForm(string action)
    {
        //remove the 'BuildingForm.'
        var sub = action.Substring(13);

        if (sub.Contains("Set.Current.Prod"))
        {
            //remove the 'Set.Current.Prod.'
            var rem = sub.Substring(17);
            SetCurrentProduct(rem);
        }
    }

    /// <summary>
    /// Will set current prod on selected buildng
    /// </summary>
    /// <param name="product"></param>
    private void SetCurrentProduct(string product)
    {
        Building.SetProductToProduce(product);
        ShowProductDetail();

        MarkProductAsSelected(int.Parse(product.Split('.')[1]));
    }

    private void MarkProductAsSelected(int productId)
    {
        foreach (var shown in _showProducts)
        {
            if (shown.ProductId() == productId)
            {
                shown.MarkAsSelected();
            }
            else shown.MarkAsUnSelected();
        }
    }

    internal void Reload()
    {
        Show(Building);
    }

    /// <summary>
    /// called from gui
    /// </summary>
    public void UpdateInputTitle()
    {
        _titleInputField.text = Building.NameBuilding();
        Program.LockInputSt();
    }

    /// <summary>
    /// called from gui
    /// </summary>
    public void NewAlias()
    {
        var oldName = Building.Name;
        Building.Name = _titleInputField.text;
        _titleInputFieldGO.SetActive(false);
        _title.text = Building.NameBuilding();
        Program.UnLockInputSt();

        if (oldName != Building.Name)
            Program.gameScene.TutoStepCompleted("RenameBuild.Tuto");

        if (Building.HType == H.Dock && BuildingController.HowManyOfThisTypeAre(H.Dock) > 1)
        {
            Program.gameScene.QuestManager.QuestFinished("Rename2ndDock");
        }
    }

    #region plus and less sign on workers max

    private GameObject _plusBtn;
    private GameObject _lessBtn;
    private GameObject _hireAllBtn;
    private GameObject _fireAllBtn;

    private void CheckIfPlusIsActive()
    {
        if (MaxPeople() >= AbsMaxPeople() || MyText.Lazy() == 0)
        {
            MakeInactiveButton(_plusBtn);
            MakeInactiveButton(_hireAllBtn);
        }
        else
        {
            MakeActiveButton(_plusBtn);
            MakeActiveButton(_hireAllBtn);
        }
    }

    private void CheckIfLessIsActive()
    {
        if (MaxPeople() == 0)
        {
            MakeInactiveButton(_lessBtn);
            MakeInactiveButton(_fireAllBtn);
        }
        else
        {
            MakeActiveButton(_lessBtn);
            MakeActiveButton(_fireAllBtn);
        }
    }

    private void MakeInactiveButton(GameObject btn)
    {
        btn.SetActive(false);
    }

    private void MakeActiveButton(GameObject btn)
    {
        btn.SetActive(true);
    }

    private int MaxPeople()
    {
        if (Building == null)
        {
            return 0;
        }

        return Building.MaxPeople;
    }

    private int AbsMaxPeople()
    {
        if (Building == null)
        {
            return 0;
        }

        return Building.AbsMaxPeople;
    }

    internal void HideIfSameBuilding(Building building)
    {
        if (_building != null && building == _building)
        {
            Hide();
        }
    }

    #endregion plus and less sign on workers max
}