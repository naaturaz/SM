﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 * Script atteched to the AddOrderWindow
 * 
 */

public class AddOrderWindow : GUIElement {

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
    private int _amt;
    private H _frecuency = H.Once;
    private int _price;


    private InputField _inputAmt;
    private InputField _inputPrice;


    private GameObject _priceGroup;



    private GameObject _content;
    private GameObject _scroll_Ini_PosGO;

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
        //_contentRectTransform = _content.GetComponent<RectTransform>();
        _scroll_Ini_PosGO = GetChildCalledOnThis("Scroll_Ini_Pos", _content);
    }

    public void Show(string val)
    {
        _orderType = val;
        _inputAmt.text = "";

        LoadMenu();

        transform.position = iniPos;

        Display();
    }

    private void LoadMenu()
    {
        HandlePriceGroup();

        _title.text = "Add new " + _orderType + " order";

        PopulateScrollView();
    }


#region Scroll

    private void PopulateScrollView()
    {
        if (_btns.Count>0)
        {
            return;
        }

        ShowButtons(Program.gameScene.ExportImport1.ProdSpecsCured());
    }



    List<ButtonTile> _btns = new List<ButtonTile>();
    private void ShowButtons(List<ProdSpec> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var iniPosHere = _scroll_Ini_PosGO.transform.localPosition +
                             new Vector3(0, -3.5f * i, 0);

            var a = ButtonTile.CreateTile(_content.gameObject.transform, list[i].Product+"",
                iniPosHere, this);

            _btns.Add(a);
        }
    }


#endregion

    void HandlePriceGroup()
    {
        if (_orderType == "Import")
        {
            Program.gameScene.TutoStepCompleted("ImportOrder.Tuto");

            _priceGroup.SetActive(false);
        }
        else if (_orderType == "Export")
        {
            //_priceGroup.SetActive(true);
            _priceGroup.SetActive(false);

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

        if (_orderType == "Export")
        {
            dockBuild.Dock1.Export(new Order(_prodSelect, "Ship", _amt));


            if (_prodSelect == P.Bean && _amt == 300)
            {
                QuestManager.QuestFinished("Export");
            }
        }
        else if (_orderType == "Import")
        {
            Order order = new Order(_prodSelect, "", "Ship");
            order.Amount = _amt;
            dockBuild.Dock1.Import(order);

            if (_prodSelect == P.Wood && _amt == 100)
            {
                Program.gameScene.TutoStepCompleted("AddOrder.Tuto");
            }
        }

        //so orders are updated 
        Program.MouseListener.BuildingWindow1.ShowOrders();
        return true;
    }

    /// <summary>
    /// notify at user tht the order is not complete yet
    /// </summary>
    private void OrderNotComplete()
    {
        //todo notify
        //Debug.Log(_errorMsg);
        _errorMsgLbl.text = _errorMsg;
    }

    private string _errorMsg;

    bool IsOrderComplete()
    {
        if (!WasProdSelected())
        {
            _errorMsg = "Prd not select";
            return false;
        }
        if (_amt == 0)
        {
            _errorMsg = "amt cant be 0";
            return false;
        }
        if (!ThereIsSpaceRequiredAvail())
        {
            _errorMsg = "ur load wont fit in our storage area";
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

        //if (sub.Contains("Prod."))
        //{
        //    ProdSelected(sub);
        //}
        if (sub.Contains("Amt"))
        {
            AmtSelected();
        }
        else if (sub.Contains("Price"))
        {
            PriceSelected();
        }
        //else if (sub.Contains("Frec."))
        //{
        //    FrecSelected(sub);
        //}
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

    //private void FrecSelected(string feed)
    //{
    //    var sub = feed.Substring(5);

    //    H MyStatus = (H)Enum.Parse(typeof(H), sub, true);

    //    _frecuency = MyStatus;
    //}

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


            _amt = int.Parse(_inputAmt.text);
        }
        else _inputAmt.text = "";
    }



    public void ProdSelected(string prod)
    {
        P MyStatus = (P)Enum.Parse(typeof(P), prod, true);
        _prodSelect = MyStatus;
        Display();
    }

    /// <summary>
    /// Will displya the final order how looks like 
    /// </summary>
    void Display()
    {
        _prodSelLbl.text = _prodSelect.ToString();
        _amtEnterLbl.text = _amt+"";

        if (_orderType == "Export")
        {
            _priceLbl.text = _price + "";
        }
    }

    //todo validation
    static public bool IsTextAValidInt(string text)
    {
        int temp = 0;
        if (int.TryParse(text, out temp))
        {
            return true;
        }

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
