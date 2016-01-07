using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * Script atteched to the AddOrderWindow
 * 
 */

public class AddOrderWindow : GUIElement {

    private Text _title;

    private Text _prodSelLbl;
    private Text _amtEnterLbl;
    private Text _frecuencyLbl;
    private Text _priceLbl;

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
        _frecuencyLbl = GetChildThatContains(H.Output_Lbl_Frec).GetComponent<Text>();
        _priceLbl = GetGrandChildCalled(H.Output_Lbl_Price).GetComponent<Text>();


        var addBtn = GetChildThatContains(H.Add_Btn).transform;
        var cancelBtn = GetChildThatContains(H.Cancel_Btn).transform;

        _addBtnRect = GetRectFromBoxCollider2D(addBtn);
        _cancelBtnRect = GetRectFromBoxCollider2D(cancelBtn);

        _inputAmt = GetChildThatContains(H.Input_Amt).GetComponent<InputField>();
        _inputPrice = GetGrandChildCalled(H.Input_Price).GetComponent<InputField>();

        _priceGroup = GetChildThatContains(H.PriceGroup);
    }

    public void Show(string val)
    {
        _orderType = val;

        LoadMenu();

        transform.position = iniPos;

        Display();
    }

    private void LoadMenu()
    {
        HandlePriceGroup();

        _title.text = "Add new " + _orderType + " order";
    }

    void HandlePriceGroup()
    {
        if (_orderType == "Import")
        {
            _priceGroup.SetActive(false);
        }
        else if (_orderType == "Export")
        {
            _priceGroup.SetActive(true);
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
        AddOrder();
        Hide();
    }

    public void CancelOrderClick()
    {
        Hide();
    }

    /// <summary>
    /// The action of adding an order to the Dock 
    /// </summary>
    private void AddOrder()
    {
        if (!IsOrderComplete())
        {
            OrderNotComplete();
            return;
        }


        var _dock = BuildingPot.Control.Registro.SelectBuilding;

        if (_orderType == "Export")
        {
            _dock.Export(new Order(_prodSelect, "Ship", _amt));
        }
        else if (_orderType == "Import")
        {
            Order order = new Order(_prodSelect, "", "Ship");
            order.Amount = _amt;
            _dock.Import(order);
        }

        //so orders are updated 
        Program.MouseListener.BuildingWindow1.ShowOrders();
    }

    /// <summary>
    /// notify at user tht the order is not complete yet
    /// </summary>
    private void OrderNotComplete()
    {
        //todo notify
        Debug.Log(_errorMsg);
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
        var dock = BuildingPot.Control.Registro.SelectBuilding;

        return dock.Inventory.HasEnoughtCapacityToStoreThis(_prodSelect, _amt);
    }

    /// <summary>
    /// Actions received from the Form 
    /// </summary>
    /// <param name="feed"></param>
    public void FeedFromForm(string feed)
    {
        //remove the 'AddOrder.'
        var sub = feed.Substring(9);

        if (sub.Contains("Prod."))
        {
            ProdSelected(sub);
        }
        else if (sub.Contains("Amt"))
        {
            AmtSelected();
        }
        else if (sub.Contains("Price"))
        {
            PriceSelected();
        }
        else if (sub.Contains("Frec."))
        {
            FrecSelected(sub);
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

    private void FrecSelected(string feed)
    {
        var sub = feed.Substring(5);

        H MyStatus = (H)Enum.Parse(typeof(H), sub, true);

        _frecuency = MyStatus;
    }

    private void PriceSelected()
    {
        if (IsTextValid(_inputPrice.text))
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
        if (IsTextValid(_inputAmt.text))
        {


            _amt = int.Parse(_inputAmt.text);
        }
        else _inputAmt.text = "";
    }



    void ProdSelected(string prod)
    {
        var sub = prod.Substring(5);

        P MyStatus = (P)Enum.Parse(typeof(P), sub, true);

        _prodSelect = MyStatus;
    }

    /// <summary>
    /// Will displya the final order how looks like 
    /// </summary>
    void Display()
    {
        _prodSelLbl.text = _prodSelect.ToString();
        _amtEnterLbl.text = _amt+"";
        _frecuencyLbl.text = _frecuency + "";

        if (_orderType == "Export")
        {
            _priceLbl.text = _price + "";
        }
    }

    //todo validation
    bool IsTextValid(string text)
    {
        int temp = 0;
        if (int.TryParse(text, out temp))
        {
            return true;
        }

        return false;
    }
}
