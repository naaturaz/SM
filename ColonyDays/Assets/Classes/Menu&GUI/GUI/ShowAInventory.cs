using UnityEngine;
using System.Collections.Generic;

/*
 * 
 * Given an inventory will display all inventory Items 
 */ 

public class ShowAInventory  
{
    List<ShowInvetoryItem> _allItems = new List<ShowInvetoryItem>();
    private GameObject _containr;
    private Vector3 _iniPos;
    private Inventory _inv;

    private string _invType;

    /// <summary>
    /// Reuglar inventory
    /// </summary>
    /// <param name="inv"></param>
    /// <param name="container"></param>
    /// <param name="iniPos"></param>
    public ShowAInventory(Inventory inv, GameObject container, Vector3 iniPos)
    {
        _iniPos = iniPos;
        _inv = inv;
        _containr = container;

        ShowAllItems();
    }


    /// <summary>
    /// Reuglar inventory
    /// </summary>
    /// <param name="inv"></param>
    /// <param name="container"></param>
    /// <param name="iniPos"></param>
    public ShowAInventory(string specialInfo, GameObject container, Vector3 iniPos)
    {
        //will show all items in all Storages this is for GUI
        if (specialInfo == "Main")
        {
            _invType = "Main";
        }
        //will show the items will be exported, imported in DOck. without amt only name 
        else if (specialInfo == "Dock")
        {
            _invType = "Dock";

        }
    }

    public Inventory Inv
    {
        get { return _inv; }
        set { _inv = value; }
    }

    


    private void ShowAllItems()
    {
        for (int i = 0; i < _inv.InventItems.Count; i++)
        {
            _allItems.Add(ShowInvetoryItem.Create(_containr.transform, _inv.InventItems[i], ReturnIniPos(i)));
        }
    }

    Vector3 ReturnIniPos(int i)
    {
        return new Vector3(ReturnX(i) + _iniPos.x, ReturnY(i) + _iniPos.y, _iniPos.z);
    }

    private int _mainLines = 12;
    float ReturnX(int i)
    {
        if (_invType == "Main")
        {
            var columns = i/   _mainLines ;
            //filled out columns
            int columsInt = (int) columns;

            return 20*columsInt;
        }
        //string.IsNullOrEmpty(_invType)
        return 1;
    }

    float ReturnY(int i)
    {
        return -4 * i;
    }

    public void ManualUpdate()
    {
        if (_allItems.Count != _inv.InventItems.Count)
        {
            Destroy();
            ShowAllItems();
        }
    }

    public void Destroy()
    {
        for (int i = 0; i < _allItems.Count; i++)
        {
            _allItems[i].Destroy();
        }
    }
}
