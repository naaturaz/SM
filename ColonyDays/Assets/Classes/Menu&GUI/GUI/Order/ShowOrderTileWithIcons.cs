﻿using System;
using UnityEngine;
using UnityEngine.UI;

internal class ShowOrderTileWithIcons : ShowInvetoryItem
{
    private Text _title;
    private P _prod;
    private float _amt;
    private bool _isDone;
    private Order _order;

    private GameObject _upBtn;
    private GameObject _downBtn;
    private GameObject _trashBtn;

    private DispatchManager _dispatchManager = new DispatchManager();
    private BuildingWindow _buildingWindow;

    private float oldAmt = -1000;

    public float xOff = 220;
    public float yOff = -22;
    private Vector3 _localPos;

    public string OrderId
    {
        get
        {
            return _order.ID;
        }
    }

    public Order Order { get { return _order; } }

    static public ShowOrderTileWithIcons Create(string root, Transform container)
    {
        ShowOrderTileWithIcons obj = null;
        root = Root.show_Order_Tile_With_Icons;

        obj = (ShowOrderTileWithIcons)Resources.Load(root, typeof(ShowOrderTileWithIcons));
        obj = (ShowOrderTileWithIcons)Instantiate(obj, new Vector3(), Quaternion.identity);

        obj.transform.SetParent(container);
        return obj;
    }

    private new void Start()
    {
        if (_icon != null)
            return;

        var container = FindGameObjectInHierarchy("Cont", gameObject);

        _upBtn = FindGameObjectInHierarchy("Up", gameObject);
        _downBtn = FindGameObjectInHierarchy("Down", gameObject);
        _trashBtn = FindGameObjectInHierarchy("Trash", gameObject);

        _icon = FindGameObjectInHierarchy("Icon", gameObject);

        _iconImg = _icon.GetComponent<Image>();

        _title = GetChildCalled(H.Title).GetComponent<Text>();

        LoadIcon(_prod);
        _icon.transform.name = _prod + "";

        _buildingWindow = FindObjectOfType<BuildingWindow>();
    }

    private new void Update()
    {
        //var rectT = GetComponent<RectTransform>();
        //rectT.localPosition = new Vector3(_localPos.x + xOff, _localPos.y + yOff, _localPos.z);
    }

    public void GoUp()
    {
        Debug.Log("Up");

        var dispatch = _dispatchManager.ReturnDispatchThatHostOrder(_order);
        if (dispatch == null) return;

        dispatch.IncreaseOrderPriority(_order);

        //order from all Heavy Loaders
        var heavys = BuildingController.FindAllStructOfThisType(H.HeavyLoad);
        for (int i = 0; i < heavys.Count; i++)
        {
            heavys[i].Dispatch1.IncreaseOrderPriority(_order);
        }

        //so it repositions
        oldAmt = -1000;
        _buildingWindow.ResetShownInventory();
    }

    public void GoDown()
    {
        Debug.Log("Down");

        var dispatch = _dispatchManager.ReturnDispatchThatHostOrder(_order);
        if (dispatch == null) return;

        dispatch.DecreaseOrderPriority(_order);

        //order from all Heavy Loaders
        var heavys = BuildingController.FindAllStructOfThisType(H.HeavyLoad);
        for (int i = 0; i < heavys.Count; i++)
        {
            heavys[i].Dispatch1.DecreaseOrderPriority(_order);
        }

        //so it repositions
        oldAmt = -1000;
        _buildingWindow.ResetShownInventory();
    }

    public void Trash()
    {
        Debug.Log("Trash");

        var dispatch = _dispatchManager.ReturnDispatchThatHostOrder(_order);
        if (dispatch == null) return;

        dispatch.DeleteOrder(_order);

        //remove order from all Heavy Loaders
        var heavys = BuildingController.FindAllStructOfThisType(H.HeavyLoad);
        for (int i = 0; i < heavys.Count; i++)
        {
            heavys[i].Dispatch1.DeleteOrder(_order);
        }

        //so it repositions
        oldAmt = -1000;
        _buildingWindow.ResetShownInventory();
    }

    public void ShowToSetCurrentProduct(ProductInfo pInfo)
    {
        Start();

        _title.text = Languages.ReturnString(pInfo.ProductLine);
        transform.position = IniPos;
        transform.name = _title.text + " | " + pInfo.Id;
    }

    /// <summary>
    /// Resets the position of the element
    /// </summary>
    /// <param name="i"></param>
    /// <param name="type"></param>
    internal void Reset(int i)
    {
        var rectT = GetComponent<RectTransform>();
        rectT.position = new Vector3();

        var x = 0;

        rectT.localPosition = new Vector3(x, (-4 * i), 0);
        transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Resets the position of the element
    /// </summary>
    /// <param name="i"></param>
    /// <param name="type"></param>
    internal void Reset(int i, H type, Vector3 iniPosition, bool isOnProcess = false)
    {
        var rectT = GetComponent<RectTransform>();
        rectT.position = new Vector3();

        var x = 0;
        if (type == H.None)
            x = ReturnPixelsToTheRight();

        float yPls = 0;
        ////bz if is on process goes down
        //if (isOnProcess)
        //yPls = AddYSpaceIfIsOnProcess();

        //rectT.localPosition = new Vector3(x, (-4 * i) - yPls, 0);

        _localPos = new Vector3(iniPosition.x + xOff, (iniPosition.y - (4 * i)) + yOff, 0);
        rectT.localPosition = _localPos;

        transform.localScale = new Vector3(1, 1, 1);
    }

    public void Show(Order order)
    {
        _amt = order.Left();

        //so it doesnt get updated every 0.1s
        if (oldAmt == _amt) return;
        oldAmt = _amt;

        _order = order;
        _prod = order.Product;

        Start();

        if (_amt == 0)
        {
            _title.text = Languages.ReturnString("Counting...");
            _isDone = true;

            _upBtn.SetActive(false);
            _downBtn.SetActive(false);
            _trashBtn.SetActive(false);
        }
        else
        {
            _title.text = Unit.WeightConverted(_amt).ToString("#");
        }

        transform.name = String.Format("{0} | {1} | {2}", _prod, _amt, Id);
    }

    private float AddYSpaceIfIsOnProcess()
    {
        //on Process order
        //var yDiff = Mathf.Abs(orderPos.y - onProcessOrderPos.y);
        return 45;//62
    }

    /// <summary>
    /// This is to address when is export should be moved to the right some pixesl
    ///
    /// Here have to address when screen is smaller. Or dif ratio
    /// </summary>
    /// <returns></returns>
    private int ReturnPixelsToTheRight()
    {
        return 233;
    }

    internal bool IsDone()
    {
        return _isDone;
    }
}