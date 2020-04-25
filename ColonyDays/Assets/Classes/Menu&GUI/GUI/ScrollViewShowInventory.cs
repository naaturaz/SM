using System.Collections;
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
    private Inventory _inv;
    private float _pad = 0;

    public string Which;//which inv needs to show and wht type of tile , need to be set in inspector

    private ShowAInventory _showAInv;
    private int _oldItemsAmt;
    private float _tileHeight = 3.5f;//3.48

    // Use this for initialization
    private void Start()
    {
        _content = FindGameObjectInHierarchy("Content", gameObject);
        _scroll_Ini_PosGO = GetChildCalledOnThis("Scroll_Ini_Pos", _content);

        RedoInvIfWhichIsEmpty();
        Show();
        StartCoroutine("ThirtySecUpdate");
    }

    private IEnumerator ThirtySecUpdate()
    {
        while (true)
        {
            var sec = Time.time < 30 ? 1 : 30;
            yield return new WaitForSeconds(sec); // wait
            ReAdjustIfDiff();
        }
    }

    private void ReAdjustIfDiff()
    {
        if (_showAInv != null && _showAInv.RedoItemsIfOldInvIsDiff())
        {
            AdjustContentHeight((_inv.InventItems.Count * _tileHeight) + _pad);
        }
    }

    /// <summary>
    /// So far used for ourInventories in the add import export order
    /// </summary>
    private void RedoInvIfWhichIsEmpty()
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
    private void Update()
    {
    }

    public void Show(float pad = 0)
    {
        if (_inv == null) return;

        _pad = _pad == 0 ? pad : _pad;
        if (_showAInv != null)
        {
            _showAInv.DestroyAll();
            RedoInvIfWhichIsEmpty();
        }

        _showAInv = new ShowAInventory(_inv, _content, _scroll_Ini_PosGO.transform.localPosition, Which);
        ResetScroolPos();
        AdjustContentHeight((_inv.InventItems.Count * _tileHeight) + _pad);
    }

    /// <summary>
    /// If true the mouse is on me right now
    /// </summary>
    static public bool IsMouseOnMe { get; set; }

    //Called from GUI
    //if mouse is on me will not scroll.
    //however this obj need to be set on Inspector with a trigger events  poinitng here
    public void CallMeWhenMouseEnter()
    {
        IsMouseOnMe = true;
    }

    public void CallMeWhenMouseExit()
    {
        IsMouseOnMe = false;
    }

    public void ReloadNewInventory(Inventory inv, float pad)
    {
        _inv = inv;
        AdjustContentHeight((_inv.InventItems.Count * _tileHeight) + _pad + pad);
        Show();
    }
}