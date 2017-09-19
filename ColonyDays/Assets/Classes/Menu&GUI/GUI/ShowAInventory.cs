using System;
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



         //if (allProdSpec[i].Product.ToString().Contains("Random") || allProdSpec[i].Product.ToString().Contains("Coin")
         //       //foods wont be shown in main inventory
         //       || Inventory.CategorizeProd(allProdSpec[i].Product)==PCat.Food)

    List<P> allowOnMain = new List<P>() 
    { P.Wood, P.Stone, P.Brick, P.Iron, P.Nail, P.Furniture, P.Mortar, P.RoofTile, P.FloorTile
    };
    /// <summary>
    /// Create a inventory with all the Products annd pull the infor from all Storages in game 
    /// </summary>
    private void CreateMainInventory()
    {
        _inv = new Inventory();
        var allProdSpec = Program.gameScene.ExportImport1.ProdSpecs;

        for (int i = 0; i < allProdSpec.Count; i++)
        {
            if (allowOnMain.Contains(allProdSpec[i].Product))
            {
                _inv.AddToSpecialInv(allProdSpec[i].Product);
            }
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
            return -(ReturnRelativeYSpace(26, _allItems[0].transform.localScale.y))
                * _mainLines * factor;//32
        }

        //var screenY = Screen.height / 3.5f;
        //Debug.Log("Screen.height / 3.5f= " + screenY);//254.8571

        //return -3.5f*i;
        //return -(Screen.height / 254.8571f) * i;
        return -(ReturnRelativeYSpace(33, ReturnTileYScale())) * i;
    }

    float ReturnTileYScale()
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



    private int countMU;
    private bool loaded;
    public void ManualUpdate()
    {
        //countMU++;
        if (!loaded || (_allItems.Count==0 && _inv.InventItems.Count>0))
        {
            loaded = true;
            //DestroyAll();
            //ShowAllItems();
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


        UpdateToThisInv(Inv);
    }

    public void UpdateEveryMinute()
    {
        RedoItemsIfOldInvIsDiff();
    }

    private void RedoItemsIfOldInvIsDiff()
    {
        if (!UMath.nearlyEqual(_oldVolumeOccupied, Inv.CurrentVolumeOcuppied(), 0.01f))//0.001
        {
            //Debug.Log("Redone InvShow");
            DestroyAll();
            ShowAllItems();
        }
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

        var iForSpwItem = 0;//so ReturnIniPos works nicely
        for (int i = 0; i < _inv.InventItems.Count; i++)
        {
            //> 0 for main so only show items tht have some 
            if (_inv.InventItems[i] != null && _inv.InventItems[i].Amount > 0)
            {
                //is a brand new item
                if (_allItems.Count <= i && !DoWeHaveThatKeyAlready(_inv.InventItems[i].Key))
                {
                    _allItems.Add(ShowInvetoryItem.Create(_containr.transform, _inv.InventItems[i], ReturnIniPos(iForSpwItem),
                        this, _invType));
                }
                else if (_allItems[i].InvItem1.Key != _inv.InventItems[i].Key)
                {
                    //updates the item
                    _allItems[i].UpdateToThis(_inv.InventItems[i], ReturnIniPos(iForSpwItem));
                }

                iForSpwItem++;
            }
        }
        FinalReposition();
    }

    bool DoWeHaveThatKeyAlready(P prod)
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
