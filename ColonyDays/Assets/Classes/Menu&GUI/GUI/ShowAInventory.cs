using System.Collections.Generic;
using UnityEngine;

/*
 * Given an inventory will display all inventory Items
 */

public class ShowAInventory
{
    private List<ShowInvetoryItem> _allItems = new List<ShowInvetoryItem>();
    private GameObject _container;
    private Vector3 _iniPos;
    private string _invType;

    private float _oldVolumeOccupied;
    private int _oldItemsAmt;

    public Inventory Inv { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="inv"></param>
    /// <param name="container"></param>
    /// <param name="iniPos"></param>
    /// <param name="specialInfo">Will only make a diff if param 'Main' is passed </param>
    public ShowAInventory(Inventory inv, GameObject container, Vector3 iniPos, string specialInfo = "")
    {
        _invType = specialInfo;

        _iniPos = iniPos;
        Inv = inv;
        Inv.OrderItemsAlpha();
        _container = container;

        ShowAllItems();
    }

    private void ManualUpdateOfAllInvItems()
    {
        for (int i = 0; i < Inv.InventItems.Count; i++)
        {
            var amt = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(Inv.InventItems[i].Key);
            Inv.SetToSpecialInv(Inv.InventItems[i].Key, amt);
        }
    }

    private void ShowAllItems()
    {
        //bridge for ex
        if (Inv == null)
            return;

        _oldVolumeOccupied = Inv.CurrentVolumeOcuppied();
        var iForSpwItem = 0;//so ReturnIniPos works nicely

        for (int i = 0; i < Inv.InventItems.Count; i++)
        {
            //> 0 for main so only show items tht have some
            if (Inv.InventItems[i] != null && Inv.InventItems[i].Amount > 0)
            {
                _allItems.Add(ShowInvetoryItem.Create(_container.transform, Inv.InventItems[i], ReturnIniPos(iForSpwItem),
                    this, _invType));

                iForSpwItem++;
            }
        }
    }

    private Vector3 ReturnIniPos(int i)
    {
        return new Vector3(ReturnX(i) + _iniPos.x, ReturnY(i) + _iniPos.y, _iniPos.z);
    }

    private int _mainLines = 18;//24

    private float ReturnX(int i)
    {
        return 1;
    }

    private float ReturnY(int i)
    {
        if (_invType == "Main")
        {
            return -(ReturnRelativeYSpace(22f, ReturnTileYScale())) * i;
        }
        return -(ReturnRelativeYSpace(25, ReturnTileYScale())) * i;
    }

    private float ReturnTileYScale()
    {
        if (_allItems.Count > 0)
        {
            return _allItems[0].transform.localScale.y;
        }
        return 0;
    }

    public static float ReturnRelativeYSpace(float relative, float ySpaceOfTile)
    {
        return relative * ySpaceOfTile;
    }

    public void ManualUpdate()
    {
        if (!IsInvItemsSameThatShown())
        {
            ManualUpdateOfAllInvItems();
            UpdateToThisInv(Inv);
        }
    }

    private bool IsInvItemsSameThatShown()
    {
        if (_allItems.Count != Inv.InventItems.Count)
        {
            return false;
        }

        for (int i = 0; i < _allItems.Count; i++)
        {
            if (_allItems[i].InvItem1.Key != Inv.InventItems[i].Key)
                return false;
        }

        return true;
    }

    public void DestroyAll()
    {
        for (int i = 0; i < _allItems.Count; i++)
        {
            if (_allItems[i] != null)
            {
                _allItems[i].Destroy();
                _allItems.RemoveAt(i);
                i--;
            }
        }
    }

    public bool RedoItemsIfOldInvIsDiff()
    {
        if (!UMath.nearlyEqual(_oldVolumeOccupied, Inv.CurrentVolumeOcuppied(), 0.01f) ||
            _oldItemsAmt != Inv.InventItems.Count)//0.001
        {
            _oldItemsAmt = Inv.InventItems.Count;
            UpdateToThisInv(Inv);
            return true;
        }
        return false;
    }

    internal void Destroy(ShowInvetoryItem showInvetoryItem)
    {
        _allItems.Remove(showInvetoryItem);
        showInvetoryItem.Destroy();

        //if a tile is destroyed needs to update
        UpdateToThisInv(Inv);
    }

    internal void UpdateToThisInv(Inventory inventory)
    {
        Inv = inventory;
        Inv.OrderItemsAlpha();

        var iForSpwItem = 0;//so ReturnIniPos works nicely
        for (int i = 0; i < Inv.InventItems.Count; i++)
        {
            //> 0 for main so only show items tht have some
            if (Inv.InventItems[i] != null && Inv.InventItems[i].Amount > 0)
            {
                //is a brand new item
                if (_allItems.Count <= i && !DoWeHaveThatKeyAlready(Inv.InventItems[i].Key))
                {
                    _allItems.Add(ShowInvetoryItem.Create(_container.transform, Inv.InventItems[i], ReturnIniPos(iForSpwItem),
                        this, _invType));
                }
                else if (_allItems[i].InvItem1.Key != Inv.InventItems[i].Key)
                {
                    //updates the item
                    _allItems[i].UpdateToThis(Inv.InventItems[i], ReturnIniPos(iForSpwItem));
                }

                iForSpwItem++;
            }
        }
        FinalReposition();
    }

    private bool DoWeHaveThatKeyAlready(P prod)
    {
        var index = _allItems.FindIndex(a => a.InvItem1.Key == prod);
        return index != -1;
    }

    /// <summary>
    /// Final position all Tiles
    /// Bz sometimes overlaps tiles
    /// </summary>
    private void FinalReposition()
    {
        for (int i = 0; i < _allItems.Count; i++)
        {
            _allItems[i].UpdateToThis(ReturnIniPos(i));
        }
    }
}