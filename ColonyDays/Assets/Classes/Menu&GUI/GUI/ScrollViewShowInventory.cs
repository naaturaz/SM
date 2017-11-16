using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script must be attached to a ScrollView element
/// 
/// must have a content and inside a Scroll_Ini_Pos gameobject
/// </summary>
public class ScrollViewShowInventory : GUIElement
{
    private GameObject _content;
    private GameObject _scroll_Ini_PosGO;
    Inventory _inv;

    public string Which;//which inv needs to show and wht type of tile 

    ShowAInventory _showAInv;

    // Use this for initialization
    void Start()
    {
        _content = FindGameObjectInHierarchy("Content", gameObject);
        _scroll_Ini_PosGO = GetChildCalledOnThis("Scroll_Ini_Pos", _content);

        RedoInvIfIfWhichIsEmpty();

        Show();
    }

    /// <summary>
    /// So far used for ourInventories in the add import export order 
    /// </summary>
    void RedoInvIfIfWhichIsEmpty()
    {
        if (string.IsNullOrEmpty(Which))
        {
            _inv = GameController.ResumenInventory1.GameInventory;
        }
        if (Which == "Main")
        {
            _inv = GameController.ResumenInventory1.GameInventory;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {
        if (_showAInv!=null)
        {
            _showAInv.DestroyAll();
            RedoInvIfIfWhichIsEmpty();
        }

        _showAInv = new ShowAInventory(_inv, _content, _scroll_Ini_PosGO.transform.localPosition, Which);
        ResetScroolPos();
        AdjustContentHeight(_inv.InventItems.Count * 3.5f);
    }


    static bool _isMouseOnMe;

    static public bool IsMouseOnMe
    {
        get
        {
            return _isMouseOnMe;
        }

        set
        {
            _isMouseOnMe = value;
        }
    }

    //Called from GUI
    //if mouse is on me will not scroll. 
    //however this obj need to be set on Inspector with a trigger events  poinitng here 
    public void CallMeWhenMouseEnter()
    {
        _isMouseOnMe = true;
    }

    public void CallMeWhenMouseExit()
    {
        _isMouseOnMe = false;

    }

}
