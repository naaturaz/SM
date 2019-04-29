﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public class BuildingWindow : GUIElement
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

    private GameObject _ordBtn;//the btn for orders
    private GameObject _prdBtn;//the btn for 
    private GameObject _staBtn;//the btn for 

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
    //private GameObject _productionSelector;


    //upg btns
    private GameObject _upg_Mat_Btn;
    private GameObject _upg_Cap_Btn; //Upg_Mat_Btn

    private GameObject _demolish_Btn; //Upg_Mat_Btn
    private GameObject _cancelDemolish_Btn; //Upg_Mat_Btn


    //Texts
    private Text _currSalaryTxt;
    private Text _currPositionsTxt;
    private Text _maxPositionsTxt;

    Image _imageIcon;


    // Use this for initialization
    void Start()
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

    void InitObj()
    {
        iniPos = transform.position;

        _general = GetChildThatContains(H.General);
        _gaveta = GetChildThatContains(H.Gaveta);
        _orders = GetChildThatContains(H.Orders);
        _products = GetChildThatContains(H.Products);
        _upgrades = GetChildCalled(H.Upgrades);
        _stats = GetChildCalled("Stats");

        _salary = General.FindGameObjectInHierarchy("Salary", _general);
        _positions = General.FindGameObjectInHierarchy("Positions", _general);

        var currSalary = FindGameObjectInHierarchy("Current_Salary", _salary);
        _currSalaryTxt = currSalary.GetComponent<Text>();

        var currPos = FindGameObjectInHierarchy("Current_Positions", _positions);
        _currPositionsTxt = currPos.GetComponent<Text>();


        var maxPos = FindGameObjectInHierarchy("Max_Positions", _positions);
        _maxPositionsTxt = maxPos.GetComponent<Text>();


        //_productionSelector = FindGameObjectInHierarchy("Production_Selector", gameObject);

        _title = GetChildCalled(H.Title).GetComponent<Text>();


        _info = GetGrandChildCalled(H.Info).GetComponent<Text>();
        _inv = GetGrandChildCalled(H.Bolsa).GetComponent<Text>();//bolsa bz tht algorith has a bugg tht names cannot be the same or start with the same

        _displayProdInfo = GetGrandChildCalled(H.Display_Lbl).GetComponent<Text>();//bolsa bz tht algorith has a bugg tht names cannot be the same or start with the same

        _invIniPos = GetGrandChildCalled(H.Inv_Ini_Pos);
        _invIniPosSta = GetGrandChildCalled("Inv_Ini_Pos_Sta");

        _imageIcon = GetChildCalled("Image_Icon").GetComponent<Image>();


        var genBtn = GetChildThatContains(H.Gen_Btn).transform;
        var invBtn = GetChildThatContains(H.Inv_Btn).transform;
        _ordBtn = GetChildThatContains(H.Ord_Btn);

        var upgBtn = GetChildCalled(H.Upg_Btn).transform;
        var prdBtn = GetChildCalled(H.Prd_Btn).transform;
        _prdBtn = GetChildThatContains(H.Prd_Btn);

        _staBtn = GetChildCalled("Sta_Btn");
        var staBtn = GetChildCalled("Sta_Btn").transform;


        _genBtnRect = GetRectFromBoxCollider2D(genBtn);
        _invBtnRect = GetRectFromBoxCollider2D(invBtn);
        _ordBtnRect = GetRectFromBoxCollider2D(_ordBtn.transform);
        _upgBtnRect = GetRectFromBoxCollider2D(upgBtn.transform);
        _prdBtnRect = GetRectFromBoxCollider2D(prdBtn.transform);
        _staBtnRect = GetRectFromBoxCollider2D(staBtn.transform);


        _importIniPos = GetGrandChildCalled(H.IniPos_Import).transform.position;
        _exportIniPos = GetGrandChildCalled(H.IniPos_Export).transform.position;

        _importIniPosOnProcess = GetGrandChildCalled(H.IniPos_Import_OnProcess).transform.position;
        _exportIniPosOnProcess = GetGrandChildCalled(H.IniPos_Export_OnProcess).transform.position;


        _upg_Mat_Btn = GetGrandChildCalled(H.Upg_Mat_Btn);
        _upg_Cap_Btn = GetGrandChildCalled(H.Upg_Cap_Btn);


        _demolish_Btn = GetGrandChildCalled(H.Demolish_Btn);//Cancel_Demolish_Btn
        _cancelDemolish_Btn = GetGrandChildCalled(H.Cancel_Demolish_Btn);//Cancel_Demolish_Btn

        _plusBtn = FindGameObjectInHierarchy("More Positions", gameObject);
        _lessBtn = FindGameObjectInHierarchy("Less Positions", gameObject);


        _salary.SetActive(false);



    }

    /// <summary>
    /// The show of the menu in a building 
    /// </summary>
    /// <param name="val"></param>
    public void Show(Building val)
    {
        Building = val;
        Program.MouseListener.HideBuildingsMenu();

        if (Building.HType == H.Road)
        {
            return;
        }
        if (Building.HType == H.Dock)
        {
            Program.gameScene.TutoStepCompleted("SelectDock.Tuto");
        }


        LoadMenu();

        //so if last Window had the Inventory selected can be seen in this new builidng one too
        //MakeThisTabActive(oldTabActive);
        MakeThisTabActive(_general);

        transform.position = iniPos;
        HandleOrdBtn();
        HandlePrdBtn();

        //in case were inactive 


        //_upg_Mat_Btn.SetActive(true);
        //_upg_Cap_Btn.SetActive(true);

        CheckIfMatMaxOut();
        CheckIfCapMaxOut();

        HideStuff();

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
        if (Building.IsHouseType(Building.MyId) || Building.MyId.Contains("Storage") || Building.Category == Ca.Way ||
            Building.HType == H.Masonry || Building.HType == H.HeavyLoad
            || Building.HType == H.LightHouse
            || Building.IsNaval()
            || Building.HType == H.Church || Building.HType == H.Tavern || Building.HType == H.TownHouse
            || Building.HType == H.Library)
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

    bool isToHidePrdTab()
    {
        return Building.HType.ToString().Contains("House") || Building.Category == Ca.Way || Building.IsNaval();
    }

    /// <summary>
    /// Will hide it if not Dock or ...
    /// </summary>
    void HandleOrdBtn()
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
    void HideShowSalAndPositions()
    {
        bool fullyBuilt = Building.IsFullyBuilt();
        bool isAWorkPlace = isAWorkBuild(Building);
        //bool allowedTab = currentActiveTab != null && currentActiveTab != _gaveta;

        if (fullyBuilt && isAWorkPlace && Building.Instruction != H.WillBeDestroy //&& allowedTab
            )
        {
            //_salary.SetActive(true);
            _positions.SetActive(true);
        }
        else
        {
            _salary.SetActive(false);
            _positions.SetActive(false);
        }
    }

    //void HideShowProductionSelector()
    //{
    //    bool fullyBuilt = Building.IsFullyBuilt();
    //    bool isAWorkPlace = isAWorkBuild(Building);
    //    bool isAProdPlace = BuildingPot.Control.ProductionProp.IsAProductionPlace(Building.HType);
    //    //bool allowedTab = currentActiveTab != null && currentActiveTab != _gaveta;

    //    if (fullyBuilt && isAWorkPlace && isAProdPlace && Building.Instruction != H.WillBeDestroy //&& allowedTab
    //        )
    //    {
    //        _productionSelector.SetActive(true);
    //    }
    //    else
    //    {
    //        _productionSelector.SetActive(false);
    //    }
    //}



    private ShowAInventory _showAInventory;
    private int oldItemsCount;
    private string oldBuildID;
    private void LoadMenu()
    {
        HideShowSalAndPositions();
        //HideShowProductionSelector();
        LoadImageIcon();

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

        //_currSalaryTxt.text = _building.DollarsPay+"";
        //_currPositionsTxt.text = _building.MaxPeople + "";

        DemolishBtn();
    }

    private void LoadImageIcon()
    {
        var iconRoot = Root.RetBuildingIconRoot(_building.HType);
        var s = (Sprite)Resources.Load(iconRoot, typeof(Sprite));

        _imageIcon.sprite = s;

    }

    public void ResetShownInventory()
    {
        oldBuildID = "";
    }

    void Inventory()
    {
        if (Building.Inventory == null)
        {
            return;
        }

        if (_showAInventory == null)
        {
            _showAInventory = new ShowAInventory(Building.Inventory, _gaveta.gameObject, _invIniPos.transform.localPosition);
            ShowProductionReport();
        }
        else if (oldBuildID != Building.MyId)
        {
            oldBuildID = Building.MyId;
            oldItemsCount = Building.Inventory.InventItems.Count;

            _showAInventory.DestroyAll();
            _showAInventory = new ShowAInventory(Building.Inventory, _gaveta.gameObject, _invIniPos.transform.localPosition);

            //so when a new building is clicked changes
            ShowProductionReport();
        }
        else if (_showAInventory != null && (oldItemsCount != Building.Inventory.InventItems.Count
            || Building.IsToReloadInv()))
        {
            //if new items got in the inv needs to redo Stats in case is a new production 
            if (oldItemsCount != Building.Inventory.InventItems.Count)
            {
                ReloadStatsWhenNeeded(true);
                oldItemsCount = Building.Inventory.InventItems.Count;
            }
            _showAInventory.UpdateToThisInv(Building.Inventory);
        }

        ReloadStatsWhenNeeded();

        _showAInventory.ManualUpdate();
        _inv.text = BuildStringInv(Building);
    }

    /// <summary>
    /// Will redo Stats
    /// </summary>
    void ReloadStatsWhenNeeded(bool now = false)
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


    List<ShowAInventory> _reports = new List<ShowAInventory>();
    private void ShowProductionReport()
    {
        for (int i = 0; i < _reports.Count; i++)
        {
            _reports[i].DestroyAll();
        }
        _reports.Clear();

        var pastItems = 0;

        for (int i = 0; i < ShowLastYears(); i++)
        {
            var a = new ShowAInventory(Building.ProductionReport.ProduceReport[i], _stats.gameObject,
                _invIniPosSta.transform.localPosition + new Vector3(0, pastItems * -3.5f * i, 0));

            _reports.Add(a);
            pastItems = Building.ProductionReport.ProduceReport[i].InventItems.Count;
        }
    }

    /// <summary>
    /// so it only shows the last 5 years or less if less
    /// </summary>
    /// <returns></returns>
    int ShowLastYears()
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
        if (Building.MyId.Contains("Bridge"))
        {
            return "";
        }

        var st = (Structure)Building;
        return st.CoverageInfo();
    }


    public static bool isAWorkBuild(Building build)
    {
        var isAHouse = build.IsThisAHouseType();
        var isStorage = build.HType.ToString().Contains("Storage");
        var isRoadorBridge = build.HType.ToString().Contains("Road") || build.HType.ToString().Contains("Bridge");

        return !isAHouse && !isStorage && build.HType != H.StandLamp && !isRoadorBridge;
    }



    string BuildInfo()
    {
        CleanPeopleTile();
        string res = Languages.ReturnString(Building.HType + ".Desc") + "\n";

        res += IfInConstructionAddPercentageOfCompletion();

        var isAHouse = Building.IsThisAHouseType();

        //is not a house or bohio 
        if (!isAHouse || Building.HType == H.LightHouse)//must say lightHouse here bz actualkly contains House
        {
            //if is Storage
            if (Building.HType.ToString().Contains("Storage"))
            {
                //res += Languages.ReturnString("Users:") + Building.PeopleDict.Count + "\n";
            }
            //others
            else
            {
                //Workers list 
                //res += "\n  " + ReturnAvailablePositions() + "\n";

                //res += "Workers:" + Building.PeopleDict.Count + "\n";
                //for (int i = 0; i < Building.PeopleDict.Count; i++)
                //{
                //    res += "\n " + Family.GetPersonName(Building.PeopleDict[i]);
                //}
            }

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

                    //res += "\n" + Building.BuildersManager1.GreenLight[i].Key;
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

            res += Languages.ReturnString("People.Living") + amt + "";
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
    List<PersonTile> _tiles = new List<PersonTile>();
    Building _oldBuilding;

    void CleanPeopleTile()
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

    void TilesOfPeopleInAHouse()
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

    Vector3 ReturnIniPos(int i)
    {
        return new Vector3(_peopleTileIniPos.x, ReturnY(i) + _peopleTileIniPos.y, _peopleTileIniPos.z);
    }

    float ReturnY(int i)
    {
        return -3.9f * i;
    }
    #endregion




    string DestroyingBuilding(string current)
    {
        if (Building.Instruction == H.WillBeDestroy)
        {
            return Languages.ReturnString("Build.Destroy.Soon");
        }
        return current;
    }

    string ReturnAvailablePositions()
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
        StructureParent sP = Building.ReturnCurrentStructureParent();

        if (sP.CurrentStage != 4)
        {
            var percentage = sP.PercentageBuiltCured();
            return "Construction progress at: " + percentage + "%\n" +
                MaterialsGathered() + "\n" + MaterialsIsMissing() + "\n\n";
        }
        return "";
    }

    /// <summary>
    /// If a building is missing some materials/resources to get built will be shown here 
    /// </summary>
    /// <returns></returns>
    private string MaterialsIsMissing()
    {
        var pass = BuildersManager.CanGreenLight(Building.HType);

        if (!pass && !Building.WasGreenlit)
        {
            return "Warning: This building cannot be built now. Missing resource(s):\n" +
                BuildersManager.MissingResources(Building.HType);
        }
        return "";
    }

    string MaterialsGathered()
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





    #endregion

    // Update is called once per frame
    void Update()
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





    //private GameObject oldTabActive;
    GameObject currentActiveTab;
    /// <summary>
    /// Use to swith Tabs on Window. Will hide all and make the pass one as active
    /// </summary>
    /// <param name="g"></param>
    void MakeThisTabActive(GameObject g)
    {
        if (Building == null || _orders == null || _products == null)
        {
            return;
        }

        //first time loaded ever in game 
        if (g == null || (!Building.IsNaval() && g == _orders) || (g == _products && isToHidePrdTab()))
        {
            g = _general;
        }

        _general.SetActive(false);
        _gaveta.SetActive(false);
        _orders.SetActive(false);
        _upgrades.SetActive(false);
        _products.SetActive(false);
        _stats.SetActive(false);
        g.SetActive(true);
        currentActiveTab = g;
        //so the old tab is active from building to building
        //oldTabActive = g;

        //then orders need to be Pull from dispatch and shown on Tab
        if (g == _orders)
        {
            ShowOrders();
        }
        if (g == _products)
        {
            ShowProducts();
        }

    }



    /// <summary>
    /// Show Prod on Tab
    /// </summary>
    private void ShowProductDetail()
    {
        Building.CurrentProd.BuildDetails();//so they update if needed
        _displayProdInfo.text = Building.CurrentProd.Details;

        if (!_building.DoIHaveInput())
        {
            _displayProdInfo.text = "Product selected: " + Building.CurrentProd.Product + "\n"
                + _building.MissingInputs();
        }

        //Showing additional info for FieldFarms
        if (Building.HType.ToString().Contains("FieldFarm"))
        {
            var st = (Structure)Building;
            //add string with current crop status and past crop 

            if (st.FieldFarm1() != null && st.FieldFarm1().HarvestDate() != null)
            {
                _displayProdInfo.text += "\nHarvest date: " + st.FieldFarm1().HarvestDate().ToStringFormatMonYear();
                _displayProdInfo.text += "\nProgress: " + st.FieldFarm1().PercentageDone();
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

    void DisplayProducts(List<ProductInfo> list, string root)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Display1String(i, list[i], root);
        }
    }

    List<OrderShow> _showProducts = new List<OrderShow>();
    void Display1String(int i, ProductInfo pInfo, string root)
    {
        var orderShow = OrderShow.Create(root, _products.transform);
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
        //DestroyAndCleanShownOrders();
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

    void ShowExportOrdersOnProcess()
    {
        var expOrd = Building.Dispatch1.ReturnExportOrdersOnProcess();
        DisplayOrders(expOrd, _exportIniPosOnProcess, Root.orderShow, true);
    }

    /// <summary>
    /// Show import orders
    /// </summary>
    void ShowImportOrders()
    {
        var impOrd = Building.Dispatch1.ReturnImportOrdersOnProcess();
        DisplayOrders(impOrd, _importIniPos, Root.orderShowClose);
    }

    private void ShowImportOrdersOnProcess()
    {
        //var impOrd = _building.Dispatch1.ReturnEvacOrdersOnProcess();
        var impOrd = Building.Dispatch1.ReturnEvacuaOrders();
        DisplayOrders(impOrd, _importIniPosOnProcess, Root.orderShow, true);
    }

    /// <summary>
    /// Display the orders are passed on 'list'
    /// </summary>
    /// <param name="list"></param>
    /// <param name="iniPosP"></param>
    void DisplayOrders(List<Order> list, Vector3 iniPosP, string root, bool isOnProcess = false)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Display1Order(i, list[i], iniPosP, root, isOnProcess);
        }
    }


    List<ShowOrderTileWithIcons> _showOrders = new List<ShowOrderTileWithIcons>();
    /// <summary>
    /// Will display the order is pass as param. Bz 'i' will keep looping and puttin the towards the botton of the 
    /// _orders tab. Will make the orders Childs of _order tab
    /// </summary>
    /// <param name="i"></param>
    /// <param name="order"></param>
    void Display1Order(int i, Order order, Vector3 iniPosP, string root, bool isOnProcess = false)
    {
        var isOrderOnList = _showOrders.Find(a => a.OrderId == order.ID);

        //brand new tile 
        if (isOrderOnList == null)
        {
            var orderShow = ShowOrderTileWithIcons.Create(root, _orders.transform);
            orderShow.Show(order);
            orderShow.Reset(i, order.TypeOrder, isOnProcess);
            _showOrders.Add(orderShow);
        } 
        //update existing
        else
        {
            isOrderOnList.Show(order);
            isOrderOnList.Reset(i, order.TypeOrder, isOnProcess);
        }
    }

    void DestroyAllProducts()
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
    void DestroyAndCleanShownOrders()
    {
        for (int i = 0; i < _showOrders.Count; i++)
        {
             _showOrders[i].Destroy();
        }
        _showOrders.Clear();
    }

    void DestroyOrdersIfDone()
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
    void HideUpgMatBtn()
    {
        _upg_Mat_Btn.SetActive(false);
    }

    /// <summary>
    /// bz when a building is max out in capacity then the bttuon needs to be  hide 
    /// </summary>
    void HideUpgCapBtn()
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
    void CheckIfMatMaxOut()
    {
        if (Building.IsBuildingMaterialBest())
        {
            HideUpgMatBtn();
        }
    }

    /// <summary>
    /// if has the best material will hide tht button
    /// </summary>
    void CheckIfCapMaxOut()
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
    void SetCurrentProduct(string product)
    {
        Building.SetProductToProduce(product);

        ShowProductDetail();
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
        Building.Name = _titleInputField.text;
        _titleInputFieldGO.SetActive(false);
        _title.text = Building.NameBuilding();
        Program.UnLockInputSt();

        Program.gameScene.TutoStepCompleted("RenameBuild.Tuto");

        if (Building.HType == H.Dock && BuildingController.HowManyOfThisTypeAre(H.Dock) > 1)
        {
            Program.gameScene.QuestManager.QuestFinished("Rename2ndDock");
        }
    }













    #region plus and less sign on workers max

    GameObject _plusBtn;
    GameObject _lessBtn;



    private void CheckIfPlusIsActive()
    {
        if (MaxPeople() >= AbsMaxPeople() || MyText.Lazy() == 0)
        {
            MakeInactiveButton(_plusBtn);
        }
        else
        {
            MakeActiveButton(_plusBtn);
        }
    }

    private void CheckIfLessIsActive()
    {
        if (MaxPeople() == 0)
        {
            MakeInactiveButton(_lessBtn);
        }
        else
        {
            MakeActiveButton(_lessBtn);
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


    int MaxPeople()
    {
        if (Building == null)
        {
            return 0;
        }

        return Building.MaxPeople;
    }

    int AbsMaxPeople()
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
    #endregion
}
