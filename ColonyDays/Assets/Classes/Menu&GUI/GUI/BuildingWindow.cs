using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class BuildingWindow : GUIElement {

    private Text _title;
    private Text _info;
    private Text _inv;
    private Text _displayProdInfo;

    private Building _building;

    private Vector3 iniPos;

    private Rect _genBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    private Rect _invBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    private Rect _ordBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    private Rect _prdBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    private Rect _upgBtnRect;

    private GameObject _ordBtn;//the btn for orders
    private GameObject _prdBtn;//the btn for 

    //tabs
    private GameObject _general;
    private GameObject _gaveta;
    private GameObject _upgrades;
    private GameObject _products;
    private GameObject _orders;


    private Vector3 _importIniPos;
    private Vector3 _exportIniPos;    
    
    private Vector3 _importIniPosOnProcess;
    private Vector3 _exportIniPosOnProcess;

    //private Vector3 _iniPosForProdList;
    private GameObject _salary;
    List<Toggle> toggles=new List<Toggle>() ; 

    //upg btns
    private GameObject _upg_Mat_Btn;
    private GameObject _upg_Cap_Btn; //Upg_Mat_Btn

    // Use this for initialization
    void Start()
    {
        InitObj();

        Hide();

        StartCoroutine("ThreeSecUpdate");
    }

    private float waitTime = 2;
    private IEnumerator ThreeSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime); // wait

            //means is showing 
            if (transform.position == iniPos)
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


        _salary = General.FindGameObjectInHierarchy("Salary", _general);



        _title = GetChildCalled(H.Title).GetComponent<Text>();
        _info = GetGrandChildCalled(H.Info).GetComponent<Text>();
        _inv = GetGrandChildCalled(H.Bolsa).GetComponent<Text>();//bolsa bz tht algorith has a bugg tht names cannot be the same or start with the same
        
        _displayProdInfo = GetGrandChildCalled(H.Display_Lbl).GetComponent<Text>();//bolsa bz tht algorith has a bugg tht names cannot be the same or start with the same


        var genBtn = GetChildThatContains(H.Gen_Btn).transform;
        var invBtn = GetChildThatContains(H.Inv_Btn).transform;
        _ordBtn = GetChildThatContains(H.Ord_Btn);

        var upgBtn = GetChildCalled(H.Upg_Btn).transform;
        var prdBtn = GetChildCalled(H.Prd_Btn).transform;
        _prdBtn = GetChildThatContains(H.Prd_Btn);


        _genBtnRect = GetRectFromBoxCollider2D(genBtn);
        _invBtnRect = GetRectFromBoxCollider2D(invBtn);
        _ordBtnRect = GetRectFromBoxCollider2D(_ordBtn.transform);
        _upgBtnRect = GetRectFromBoxCollider2D(upgBtn.transform);
        _prdBtnRect = GetRectFromBoxCollider2D(prdBtn.transform);


        _importIniPos = GetGrandChildCalled(H.IniPos_Import).transform.position;
        _exportIniPos = GetGrandChildCalled(H.IniPos_Export).transform.position;

        _importIniPosOnProcess = GetGrandChildCalled(H.IniPos_Import_OnProcess).transform.position;
        _exportIniPosOnProcess = GetGrandChildCalled(H.IniPos_Export_OnProcess).transform.position;

   //     _iniPosForProdList = GetGrandChildCalled(H.Prd_Btns_Pos).transform.position;



        _upg_Mat_Btn = GetGrandChildCalled(H.Upg_Mat_Btn);
        _upg_Cap_Btn = GetGrandChildCalled(H.Upg_Cap_Btn);

    }




    public void Show(Building val)
    {
        _building = val;

        LoadMenu();

        MakeThisTabActive(_general);

        transform.position = iniPos;
        HandleOrdBtn();
        HandlePrdBtn();

        //in case were inactive 
        _upg_Mat_Btn.SetActive(true);
        _upg_Cap_Btn.SetActive(true);

        CheckIfMatMaxOut();
        CheckIfCapMaxOut();

        InitToggles();//in case they have not been set yet
        HideSalaryIfHouseOrStorage();
        Mark1stCheckBox();
    }

    private void HideSalaryIfHouseOrStorage()
    {
        if (_building.MyId.Contains("House") || _building.MyId.Contains("Storage") || _building.Category == Ca.Way)
        {
           _salary.SetActive(false);
        }
        else
        {
            _salary.SetActive(true);
        }
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
        if (_building.HType.ToString().Contains("House") || _building.Category == Ca.Way)
        {
            _prdBtn.SetActive(false);
        }
        else
        {
            _prdBtn.SetActive(true);
        }
    }

    /// <summary>
    /// Will hide it if not Dock or ...
    /// </summary>
    void HandleOrdBtn()
    {
        if (_building.HType != H.Dock || _building.HType != H.DryDock || _building.HType != H.Supplier)
        {
            _ordBtn.SetActive(false);
        }
        if (_building.HType == H.Dock || _building.HType == H.DryDock || _building.HType == H.Supplier)
        {
            _ordBtn.SetActive(true);
        }
    }

    private void LoadMenu()
    {
        _title.text = _building.HType + "";
        _info.text = BuildInfo();
        _inv.text = BuildStringInv(_building);
    }

    string BuildInfo()
    {
        string res = "";
        
        //is not a house 
        if (!_building.HType.ToString().Contains("House"))
        {
            res = "Type:" + _building.HType + " Workers:" + _building.PeopleDict.Count
                  + " ID:" + _building.MyId
                  + "\n Workers:";

            for (int i = 0; i < _building.PeopleDict.Count; i++)
            {
                res += "\n" + _building.PeopleDict[i];
            }
        }
        else
        {
            var amt = 0;
            for (int i = 0; i < _building.Families.Count(); i++)
            {
                amt += _building.Families[i].MembersOfAFamily();
            }

            res = "Type:" + _building.HType + " In House:" + amt
                + " ID:" + _building.MyId;

            if (_building.BookedHome1 != null)
            {
                res += " IsBooked:" + _building.BookedHome1.IsBooked();
            }
            else
            {
                res += " IsBooked: no";
            }

            for (int i = 0; i < _building.Families.Count(); i++)
            {
                res += _building.Families[i].InfoShow();
            }
        }

        return res;
    }


    #region CheckBoxes / Toggles

    private bool ignore;//will ignore toggling so it doestn doo and infinity loop 

    private void InitToggles()
    {
        if (toggles.Count > 0)
        {
            UnMarkAllCheckBoxes();
            return;
        }

        toggles = new List<Toggle>()
        {
            GetGrandChildCalledFromThis("Toggle_1", _general).GetComponent<Toggle>(),
            GetGrandChildCalledFromThis("Toggle_2", _general).GetComponent<Toggle>(),
            GetGrandChildCalledFromThis("Toggle_3", _general).GetComponent<Toggle>(),
            GetGrandChildCalledFromThis("Toggle_4", _general).GetComponent<Toggle>(),
            GetGrandChildCalledFromThis("Toggle_5", _general).GetComponent<Toggle>(),
        };

        UnMarkAllCheckBoxes();
    }


    /// <summary>
    /// When the use clicks to change the salary on a building 
    /// </summary>
    /// <param name="action"></param>
    public void ClickedOnChangeSalaryCheckBox(string action)
    {
        if (ignore)
        {//will ignore the toggle so the event listener wont do anyting
            return;
        }

        //change salary
        UnMarkAllCheckBoxes();
        MarkCheckBox(action);
        BuildingPot.Control.Registro.SelectBuilding.ChangeSalary(action);
    }

    void UnMarkAllCheckBoxes()
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            ToggleThisOne(i, false);
        }
    }

    void MarkCheckBox(string action)
    {
        //Toggle_1
        var subSt = action.Substring(action.Length - 1);
        var index = int.Parse(subSt);
        //-1 bz the toggles are from 1-5

        ToggleThisOne(index - 1, true);
    }


    void ToggleThisOne(int index, bool val)
    {
        ignore = true;//will ignore the toggle so the event listener wont do anyting
        toggles[index].isOn = val;
        ignore = false;
    }


    /// <summary>
    /// Happens when the first time the check box is loaded 
    /// </summary>
    void Mark1stCheckBox()
    {
        var diff = BuildingPot.Control.Registro.SelectBuilding.WhichIsTheBuildingSalaryStatus();
        var index = -1;//the index toggle is gonna be mark

        if (diff == -2)
        {
            index = 0;
        }    
        else if (diff == -1)
        {
            index = 1;
        }   
        else if (diff == 0)
        {
            index = 2;
        }     
        else if (diff == 1)
        {
            index = 3;
        }     
        else if (diff == 2)
        {
            index = 4;
        }

        ToggleThisOne(index, true);
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
        //ig click inv
        else if (_ordBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0) && 
            (_building.HType == H.Dock || _building.HType == H.DryDock ||_building.HType == H.Supplier))
        {
            MakeThisTabActive(_orders);
        }
        else if (_upgBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0))
        {
            MakeThisTabActive(_upgrades);
        }
        else if (_prdBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0))
        {
            MakeThisTabActive(_products);
        }
    }

    /// <summary>
    /// Use to swith Tabs on Window. Will hide all and make the pass one as active
    /// </summary>
    /// <param name="g"></param>
    void MakeThisTabActive(GameObject g)
    {
        _general.SetActive(false);
        _gaveta.SetActive(false);
        _orders.SetActive(false);
        _upgrades.SetActive(false);
        _products.SetActive(false);

        g.SetActive(true);

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


    //Show Prod on Tab
    private void ShowProductDetail()
    {
        _displayProdInfo.text = _building.CurrentProd.Details;
    }

    private void ShowProducts()
    {
        DestroyAndCleanShownOrders();

        var list = _building.ShowProductsOfBuild();
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

    void Display1String(int i, ProductInfo pInfo, string root)
    {
        var orderShow = OrderShow.Create(root, _products.transform);
        orderShow.ShowToSetCurrentProduct(pInfo);

        orderShow.Reset(i);

        _showOrders.Add(orderShow);
    }






    ///Show  Orders on tab

    /// <summary>
    /// Show orders routine
    /// </summary>
    public void ShowOrders()
    {
        DestroyAndCleanShownOrders();

        ShowImportOrders();
        ShowImportOrdersOnProcess();

        ShowExportOrders();
        ShowExportOrdersOnProcess();
    }



    private void ShowExportOrders()
    {
        var expOrd = _building.Dispatch1.ReturnRegularOrders();
        DisplayOrders(expOrd, _exportIniPos, Root.orderShowClose);
    }

    void ShowExportOrdersOnProcess()
    {
        var expOrd  =  _building.Dispatch1.ReturnRegularOrdersOnProcess();
        DisplayOrders(expOrd, _exportIniPosOnProcess, Root.orderShow);
    }

    /// <summary>
    /// Show import orders
    /// </summary>
    void ShowImportOrders()
    {
        //todo not show orders to cancel when on Dock Inventory
        //var impOrd = _building.Dispatch1.ReturnEvacuaOrders();
        var impOrd = _building.Dispatch1.ReturnEvacOrdersOnProcess();
        DisplayOrders(impOrd, _importIniPos, Root.orderShowClose);
    }

    private void ShowImportOrdersOnProcess()
    {
        //var impOrd = _building.Dispatch1.ReturnEvacOrdersOnProcess();
        var impOrd = _building.Dispatch1.ReturnEvacuaOrders();
        DisplayOrders(impOrd, _importIniPosOnProcess, Root.orderShow);
    }

    /// <summary>
    /// Display the orders are passed on 'list'
    /// </summary>
    /// <param name="list"></param>
    /// <param name="iniPosP"></param>
    void DisplayOrders(List<Order> list, Vector3 iniPosP, string root)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Display1Order(i, list[i], iniPosP, root);
        }
    }


    List<OrderShow> _showOrders = new List<OrderShow>(); 
    /// <summary>
    /// Will display the order is pass as param. Bz 'i' will keep looping and puttin the towards the botton of the 
    /// _orders tab. Will make the orders Childs of _order tab
    /// </summary>
    /// <param name="i"></param>
    /// <param name="order"></param>
    /// <param name="iniPosP"></param>
    void Display1Order(int i, Order order, Vector3 iniPosP, string root)
    {
        var orderShow = OrderShow.Create(root, _orders.transform);
        orderShow.Show(order);

        orderShow.Reset(i, order.TypeOrder, _importIniPos, _importIniPosOnProcess);

        _showOrders.Add(orderShow);
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
        _upg_Cap_Btn.SetActive(false);
    }


    /// <summary>
    /// Once the Upgrate mat bottuon is clicked .
    /// </summary>
    public void ClickedUpdMatBtn()
    {
        _building.UpgradeMatToNext();
        CheckIfMatMaxOut();
    }


    /// <summary>
    /// Upgradint capacity
    /// </summary>
    internal void ClickedUpdCapBtn()
    {
        _building.UpgradeCapToNext();
        CheckIfCapMaxOut();

        _inv.text = BuildStringInv(_building);
    }

    /// <summary>
    /// if has the best material will hide tht button
    /// </summary>
    void CheckIfMatMaxOut()
    {
        if (_building.IsBuildingMaterialBest())
        {
            HideUpgMatBtn();
        }
    }

    /// <summary>
    /// if has the best material will hide tht button
    /// </summary>
    void CheckIfCapMaxOut()
    {
        if (_building.IsBuildingCapAtMax())
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
        _building.SetProductToProduce(product);

        ShowProductDetail();
    }
}
