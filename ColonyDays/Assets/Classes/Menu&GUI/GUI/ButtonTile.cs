using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class ButtonTile : GUIElement
{
    private Text _descText;
    private AddOrderWindow _addOrderWindow;
    
    public string Value { get; set; }

    public AddOrderWindow OrderWindow
    {
        get { return _addOrderWindow; }
        set { _addOrderWindow = value; }
    }

    void Start()
    {
        _descText = FindGameObjectInHierarchy("Item_Desc", gameObject).GetComponent<Text>();
        
        Init();
    }

    private void Init()
    {
        _descText.text = Value;
    }

    void Update()
    {

    }

    /// <summary>
    /// Calle from GUI
    /// </summary>
    public void ButtonClick()
    {
        _addOrderWindow.ProdSelected(_descText.text);
    }

    internal static ButtonTile CreateTile(Transform container,
        string val, Vector3 iniPos, AddOrderWindow win)
    {
        ButtonTile obj = null;

        var root = "";

        obj = (ButtonTile)Resources.Load(Root.button_Tile, typeof(ButtonTile));
        obj = (ButtonTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var iniScale = obj.transform.localScale;
        obj.transform.SetParent(container);
        obj.transform.localPosition = iniPos;
        obj.transform.localScale = iniScale;

        obj.Value = val;
        obj.OrderWindow = win;

        return obj;
    }




}

