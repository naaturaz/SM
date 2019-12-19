using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 * Script atteched to the AddOrderWindow
 * 
 */
public class AddOrderWindow : GUIElement
{
    private Text _title;

    private Text _prodSelLbl;
    private Text _amtEnterLbl;
    private Text _priceLbl;
    private Text _errorMsgLbl;

    private string _orderType;//will say if import or export

    private Rect _addBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    private Rect _cancelBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D

    private GameObject _ordBtn;//the btn for orders

    //order elements
    private P _prodSelect = P.None;
    private float _amt;
    private H _frecuency = H.Once;
    private int _price;

    private InputField _inputAmt;
    private InputField _inputPrice;

    private GameObject _priceGroup;

    private GameObject _content;
    private GameObject _scroll_Ini_PosGO;

    ScrollViewShowInventory _ourInventories;

    // Use this for initialization
    private void Start()
    {
        InitObj();
        Hide();
    }

    void InitObj()
    {
        iniPos = transform.position;

        _title = GetChildThatContains(H.Title).GetComponent<Text>();

        _prodSelLbl = GetChildThatContains(H.Output_Lbl_Prod).GetComponent<Text>();
        _amtEnterLbl = GetChildThatContains(H.Output_Lbl_Amt).GetComponent<Text>();
        _priceLbl = GetGrandChildCalled(H.Output_Lbl_Price).GetComponent<Text>();

        _errorMsgLbl = GetChildCalled(H.Output_Error_Msg).GetComponent<Text>();

        CleanDisplay();

        var addBtn = GetChildThatContains(H.Add_Btn).transform;
        var cancelBtn = GetChildThatContains(H.Cancel_Btn).transform;

        _addBtnRect = GetRectFromBoxCollider2D(addBtn);
        _cancelBtnRect = GetRectFromBoxCollider2D(cancelBtn);

        _inputAmt = GetChildThatContains(H.Input_Amt).GetComponent<InputField>();
        _inputPrice = GetGrandChildCalled(H.Input_Price).GetComponent<InputField>();

        _priceGroup = GetChildThatContains(H.PriceGroup);

        var _scroll = GetChildCalled("Scroll_View");
        _content = GetGrandChildCalledFromThis("Content", _scroll);
        _scroll_Ini_PosGO = GetChildCalledOnThis("Scroll_Ini_Pos", _content);

        _ourInventories = FindGameObjectInHierarchy("Scroll_View_Inv_Resume", gameObject).GetComponent<ScrollViewShowInventory>();
    }

    public void Show(string val)
    {
        _orderType = val;
        _inputAmt.text = "";

        LoadMenu();
        transform.position = iniPos;

        ResetScroolPos();
        Display();

        _ourInventories.Show(5f);
    }

    private void LoadMenu()
    {
        HandlePriceGroup();
        _title.text = Languages.ReturnString("Add.New") + Languages.ReturnString(_orderType) + Languages.ReturnString("Order");
        PopulateScrollView();
    }

    #region Scroll

    private void PopulateScrollView()
    {
        if (_btns.Count > 0)
            return;

        ShowButtons(Program.gameScene.ExportImport1.ProdSpecsCured());
    }

    List<ButtonTile> _btns = new List<ButtonTile>();
    private void ShowButtons(List<ProdSpec> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var iniPosHere = _scroll_Ini_PosGO.transform.localPosition + new Vector3(0, -3.5f * i, 0);
            var a = ButtonTile.CreateTile(_content.gameObject.transform, list[i], iniPosHere, this);

            _btns.Add(a);
        }
    }

    #endregion

    void HandlePriceGroup()
    {
        if (_orderType == "Import")
        {
            Program.gameScene.TutoStepCompleted("ImportOrder.Tuto");
        }
        else if (_orderType == "Export")
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        //so rects dont work when hidden 
        if (transform.position != InitialPosition)
        {
            return;
        }

        //if click gen
        if (_addBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0))
        {

        }
        //ig click inv
        else if (_cancelBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0))
        {

        }
    }

    public void AddOrderClick()
    {
        if (AddedOrder())
        {
            Hide();
            CleanDisplay();
        }
    }

    public void CancelOrderClick()
    {
        Hide();
        CleanDisplay();
    }

    void CleanDisplay()
    {
        _errorMsgLbl.text = "";
        _priceLbl.text = "";
    }

    /// <summary>
    /// The action of adding an order to the Dock 
    /// </summary>
    private bool AddedOrder()
    {
        if (!IsOrderComplete())
        {
            OrderNotComplete();
            return false;
        }

        var dockBuild = BuildingPot.Control.Registro.SelectBuilding;

        //used in case the conversion has something off. 
        int locAmt = -1;
        if (int.TryParse(_inputAmt.text, out locAmt))
        {
        }

        if (_orderType == "Export")
        {
            dockBuild.Dock1.Export(new Order(_prodSelect, "Ship", _amt));


            if (_prodSelect == P.Bean && locAmt == 300)
            {
                Program.gameScene.QuestManager.QuestFinished("Export");
            }
            //
            else if (_prodSelect == P.Weapon && locAmt == 100)
            {
                Program.gameScene.QuestManager.QuestFinished("ExportWeapons");
            }
            //
            else if (_prodSelect == P.GunPowder && locAmt == 100)
            {
                Program.gameScene.QuestManager.QuestFinished("ExportGunPowder");
            }
        }
        else if (_orderType == "Import")
        {
            Order order = new Order(_prodSelect, "", "Ship");
            order.Amount = _amt;
            dockBuild.Dock1.Import(order);

            if (_prodSelect == P.Wood && locAmt == 100)
            {
                Program.gameScene.TutoStepCompleted("AddOrder.Tuto");
            }
            if (_prodSelect == P.WhaleOil && locAmt == 500)
            {
                Program.gameScene.QuestManager.QuestFinished("ImportOil");
            }
            //
            if (_prodSelect == P.Sulfur && locAmt == 1000)
            {
                Program.gameScene.QuestManager.QuestFinished("ImportSulfur");
            }
            if (_prodSelect == P.Potassium && locAmt == 1000)
            {
                Program.gameScene.QuestManager.QuestFinished("ImportPotassium");
            }
            if (_prodSelect == P.Coal && locAmt == 1000)
            {
                Program.gameScene.QuestManager.QuestFinished("ImportCoal");
            }
            if (_prodSelect == P.Wood && locAmt == 2000)
            {
                Program.gameScene.QuestManager.QuestFinished("Import2000Wood");
            }
            if (_prodSelect == P.Coal && locAmt == 2000)
            {
                Program.gameScene.QuestManager.QuestFinished("Import2000Coal");
            }
        }

        //so orders are updated 
        Program.MouseListener.BuildingWindow1.ShowOrders();

        ClearForm();
        return true;
    }

    void ClearForm()
    {
        _amt = 0;
        _prodSelect = P.None;
        _errorMsgLbl.text = "";
        _errorMsg = "";
    }

    /// <summary>
    /// notify at user tht the order is not complete yet
    /// </summary>
    private void OrderNotComplete()
    {
        //todo notify
        _errorMsgLbl.text = _errorMsg;
        _priceLbl.text = "";
    }

    private string _errorMsg;

    bool IsOrderComplete()
    {
        if (!WasProdSelected())
        {
            _errorMsg = Languages.ReturnString("Prod.Not.Select");
            return false;
        }
        if (_amt == 0)
        {
            _errorMsg = Languages.ReturnString("Amt.Cant.Be.0");
            return false;
        }
        if (!ThereIsSpaceRequiredAvail())
        {
            _errorMsg = Languages.ReturnString("LoadWontFit");
            return false;
        }
        _errorMsg = "";
        return true;
    }

    bool WasProdSelected()
    {
        if (_prodSelect == P.None)
        {
            return false;
        }
        return true;
    }

    bool ThereIsSpaceRequiredAvail()
    {
        //breaks with the TUto
        if (BuildingPot.Control.Registro.SelectBuilding == null)
        {
            BuildingPot.Control.Registro.SelectBuilding = Program.MouseListener.BuildingWindow1.Building;
        }

        var dock = BuildingPot.Control.Registro.SelectBuilding;
        return dock.Inventory.HasEnoughtCapacityToStoreThis(_prodSelect, _amt);
    }

    /// <summary>
    /// Actions received from the Form 
    /// </summary>
    /// <param name="feed"></param>
    public void FeedFromForm(string feed)
    {
        if (!IsShownNow())//bz amt is being called all the time from the keyboard 
        {
            return;
        }

        //remove the 'AddOrder.'
        var sub = feed.Substring(9);

        if (sub.Contains("Amt"))
        {
            AmtSelected();
        }
        else if (sub.Contains("Price"))
        {
            PriceSelected();
        }
        else if (sub.Contains("Remove."))
        {
            RemoveOrderFromDispatch(sub);
        }

        Display();
    }

    /// <summary>
    /// Will remove order from dispatch
    /// </summary>
    /// <param name="sub"></param>
    private void RemoveOrderFromDispatch(string feed)
    {
        var id = feed.Substring(7);
        var dock = BuildingPot.Control.Registro.SelectBuilding;

        dock.Dispatch1.RemoveOrderByIDExIm(id);
        //so orders are updated 
        Program.MouseListener.BuildingWindow1.ShowOrders();
    }

    private void PriceSelected()
    {
        if (IsTextAValidInt(_inputPrice.text))
        {
            _price = int.Parse(_inputPrice.text);
        }
        else _inputPrice.text = "";
    }

    /// <summary>
    /// Amt selected 
    /// </summary>
    private void AmtSelected()
    {
        if (IsTextAValidInt(_inputAmt.text))
        {
            var loc = int.Parse(_inputAmt.text);
            _amt = Unit.ConvertWeightFromCurrentToKG((float)loc);
            _errorMsgLbl.text = "";
        }
        else _inputAmt.text = "";
    }

    public void ProdSelected(string prod)
    {
        P MyStatus = (P)Enum.Parse(typeof(P), prod, true);
        _prodSelect = MyStatus;
        Display();
        _errorMsgLbl.text = "";
    }

    /// <summary>
    /// Will displya the final order how looks like 
    /// </summary>
    void Display()
    {
        if (_prodSelect != P.None)
        {
            var key = _prodSelect.ToString();
            _prodSelLbl.text = Languages.ReturnString(key);
        }
        else
        {
            _prodSelLbl.text = "-";
        }

        if (_inputAmt.text != "")
            _amtEnterLbl.text = _inputAmt.text + " " + Unit.CurrentWeightUnitsString();
        else
            _amtEnterLbl.text = "-"; ;

        if (_prodSelect != P.None && _inputAmt.text != "")
        {
            var trans = Program.gameScene.ExportImport1.CalculateTransaction(_prodSelect, _amt);
            _priceLbl.text = MyText.DollarFormat(trans);
        }
    }

    //todo validation
    static public bool IsTextAValidInt(string text)
    {
        int temp = 0;
        if (int.TryParse(text, out temp))
            return true;

        return false;
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void LockInput()
    {
        Program.LockInputSt();
    }
}
