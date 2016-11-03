﻿using System;
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
        _containr = container;
        _iniPos = iniPos;

        //will show all items in all Storages this is for GUI
        if (specialInfo == "Main")
        {
            _invType = "Main";
            CreateMainInventory();
            ShowAllItems();
        }
        //will show the items will be exported, imported in DOck. without amt only name 
        else if (specialInfo == "Dock")
        {
            _invType = "Dock";
        }
    }

    /// <summary>
    /// Create a inventory with all the Products annd pull the infor from all Storages in game 
    /// </summary>
    private void CreateMainInventory()
    {
        _inv = new Inventory();
        var allProdSpec = Program.gameScene.ExportImport1.ProdSpecs;

        for (int i = 0; i < allProdSpec.Count; i++)
        {
            if (allProdSpec[i].Product.ToString().Contains("Random") || allProdSpec[i].Product.ToString().Contains("Coin")
                //foods wont be shown in main inventory
                || Inventory.CategorizeProd(allProdSpec[i].Product)==PCat.Food)
            {
                continue;
            }
            _inv.AddToSpecialInv(allProdSpec[i].Product);
        }
    }

    void ManualUpdateOfAllInvItems()
    {
        for (int i = 0; i < _inv.InventItems.Count; i++)
        {
            var amt = GameController.ResumenInventory1.ReturnAmtOfItemOnInv(_inv.InventItems[i].Key);
            _inv.SetToSpecialInv(_inv.InventItems[i].Key, amt);
        }
    }

    public Inventory Inv
    {
        get { return _inv; }
        set { _inv = value; }
    }

    private float _oldVolumeOccupied;
    private void ShowAllItems( )
    {
        //bridge for ex
        if (Inv == null)
        {
            return;
        }

        _oldVolumeOccupied = Inv.CurrentVolumeOcuppied();
        var iForSpwItem = 0;//so ReturnIniPos works nicely

        for (int i = 0; i < _inv.InventItems.Count; i++)
        {
            //> 0 for main so only show items tht have some 
            if (_inv.InventItems[i]!=null && _inv.InventItems[i].Amount>0)
            {
                _allItems.Add(ShowInvetoryItem.Create(_containr.transform, _inv.InventItems[i], ReturnIniPos(iForSpwItem),
                    this,_invType));

                iForSpwItem++;
            }
        }
    }

    Vector3 ReturnIniPos(int i)
    {
        return new Vector3(ReturnX(i) + _iniPos.x, ReturnY(i) + _iniPos.y, _iniPos.z);
    }

    private int _mainLines = 18;//24
    float ReturnX(int i)
    {
        if (_invType == "Main")
        {
            var columns = i/   _mainLines ;
            //filled out columns
            int columsInt = (int) columns;

            return 49*columsInt; //40
        }
        //string.IsNullOrEmpty(_invType)
        return 1;
    }

    float ReturnY(int i)
    {
        if (_invType=="Main")
        {
            //so Y is resested when a new Colum is reached 
            var lineNumber = (float)i / (float)_mainLines;
            var roundDown = int.Parse(lineNumber.ToString("F0"));
            var factor = lineNumber - roundDown;

            if (i==0)
            {
                return 0;
            }
            return -(ReturnRelativeYSpace(27, _allItems[0].transform.localScale.y))
                * _mainLines * factor;//32
        }
        return -3.5f*i;
    }

    public static float ReturnRelativeYSpace(float relative, float ySpaceOfTile)
    {
        return relative * ySpaceOfTile;
    }



    private int countMU;
    private bool loaded;
    public void ManualUpdate()
    {
        //countMU++;
        if (!loaded || (_allItems.Count==0 && _inv.InventItems.Count>0))
        {
            loaded = true;
            //DestroyAll();
            ShowAllItems();
            countMU = 0;
        }
    }

    public void DestroyAll()
    {
        for (int i = 0; i < _allItems.Count; i++)
        {
            if (_allItems[i]!=null)
            {
                _allItems[i].Destroy();
                _allItems.RemoveAt(i);
                i--;
            }
        }
    }

    public void Update()
    {
        if (Inv.InventItems.Count > 0 && _allItems.Count == 0)
        {
            RedoItemsIfOldInvIsDiff();
        }
    }

    //so far only called from myForm.cs
    public void UpdateEvery2Sec()
    {
        ManualUpdateOfAllInvItems();
    }

    public void UpdateEveryMinute()
    {
        RedoItemsIfOldInvIsDiff();
    }

    private void RedoItemsIfOldInvIsDiff()
    {
        if (!UMath.nearlyEqual(_oldVolumeOccupied, Inv.CurrentVolumeOcuppied(), 0.01f))//0.001
        {
            Debug.Log("Redone InvShow");
            DestroyAll();
            ShowAllItems();
        }
    }


    internal void Destroy(ShowInvetoryItem showInvetoryItem)
    {
        _allItems.Remove(showInvetoryItem);
        showInvetoryItem.Destroy();
    }
}
