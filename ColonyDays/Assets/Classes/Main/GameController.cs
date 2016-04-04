﻿/*
 * This is a really omportatn class. This is the one the control the whole game in sense of inventory.
 * Holds the inventory of the game
 * 
 * 
 * Controls the game in the sense to which Scene with BUilding with Person Load.
 * Or create new game 
 */

using System.Collections;
using UnityEngine;

public class GameController  {

    //Main inventory of the game .. wht u see on the GUI 
    //will have all tht is in all Storages combined 
    //is a total inventory. Representing all tht is in those inventories 
    static ResumenInventory _inventory = new ResumenInventory();

    private float _dollars;//the dollars the player has 
    private static StartingCondition _startingCondition;

    private int _lastYearWorkersSalaryWasPaid;

    static public ResumenInventory Inventory1
    {
        get { return _inventory; }
        set
        {
            
            _inventory = value;
        }
    }

    public float Dollars
    {
        get { return _dollars; }
        set
        {
            _dollars = value;
        }
    }

    /// <summary>
    /// Says the last year the salaray was paid 
    /// </summary>
    public int LastYearWorkersSalaryWasPaid
    {
        get { return _lastYearWorkersSalaryWasPaid; }
        set { _lastYearWorkersSalaryWasPaid = value; }
    }


    static public void LoadStartingConditions(StartingCondition startingCondition)
    {
        _startingCondition = startingCondition;

        //if (!Inventory1.IsEmpty())// the inventory here is not empty means was loaded already)
        //{
        //    return;
        //}
        

        //var inv = CreateInitialInv(startingCondition);

        //LoadIntoInv(inv);
    }

    Inventory CreateInitialInv(StartingCondition startingCondition)
    {
        Dollars += startingCondition.iniDollar;
        Inventory inv = new Inventory();

        inv.Add(P.Wood, startingCondition.iniWood);
        inv.Add(P.Food, startingCondition.iniFood);

        inv.Add(P.Stone, startingCondition.iniStone);
        inv.Add(P.Brick, startingCondition.iniBrick);
        inv.Add(P.Iron, startingCondition.iniIron);

        inv.Add(P.Gold, startingCondition.iniGold);
        inv.Add(P.WheelBarrow, startingCondition.iniWheelBarrow);
        inv.Add(P.Tool, startingCondition.iniTool);

        //todo remove when release
        //inv.Add(P.Coal, 100000);
        //inv.Add(P.Sugar, 100000);

        return inv;
    }

    /// <summary>
    /// Will load the Starting conditions into the 1st Storage Type of building.
    /// This is done only at first when game is created
    /// </summary>
    static void LoadIntoInv(Inventory invt)
    {
        if (BuildingPot.Control.FoodSources.Count == 0 )
        {
            return;
        }

        var key = BuildingPot.Control.FoodSources[0];

        var storage = Brain.GetStructureFromKey(key);

        storage.Inventory.AddItems(invt.InventItems);
    }

    /// <summary>
    /// The first Storage will call this so the first Lote is get into there 
    /// </summary>
    public void SetInitialLote()
    {
        var inv = CreateInitialInv(_startingCondition);
        LoadIntoInv(inv);
    }




    private static bool _areThereWheelBarrowsOnStorage;
    /// <summary>
    /// updated every 60sec from GameScene
    /// </summary>
    public static bool AreThereWheelBarrowsOnStorage
    {
        get { return _areThereWheelBarrowsOnStorage; }
        set { _areThereWheelBarrowsOnStorage = value; }
    }

    public void ReCheckWheelBarrowsOnStorage()
    {
        AreThereWheelBarrowsOnStorage = ThereIsAtLeastOneOfThisOnStorage(P.WheelBarrow);
    }




    public void Start()
    {
        
    }

    public void Update()
    {
        CheckIfSalariesNeedToBePaid();
        CheckIfGameOverCondition();
    }

    private void CheckIfGameOverCondition()
    {
        if (Dollars<-100000)
        {
            GameOver();
        }
        if (BuildingPot.Control!=null &&  BuildingPot.Control.DockManager1!=null &&
            BuildingPot.Control.DockManager1.PirateThreat > 90)
        {
            GameOver();
        }
    }

    private bool _isGameOver;
    private void GameOver()
    {
        if (_isGameOver)
        {
            return;
        }

        //todo 
        //end game, show form 
        Debug.Log("Game over");
        _isGameOver = true;
    }

    private void CheckIfSalariesNeedToBePaid()
    {
        if (LastYearWorkersSalaryWasPaid < Program.gameScene.GameTime1.Year
            && Program.gameScene.GameTime1.Month1 == 1)
        {
            LastYearWorkersSalaryWasPaid = Program.gameScene.GameTime1.Year;
            SalariesPay();
            PirateThreat();
        }
    }



    void SalariesPay()
    {
        Dollars -= BuildingPot.Control.Registro.ReturnYearSalary();
    }

    private void PirateThreat()
    {
        var pts = 0f;
        if (Dollars > 10000)
        {
            pts = Dollars/10000;
        }
        if (Inventory1.ReturnAmtOfItemOnInv(P.Gold)> 1000)
        {
            pts += Inventory1.ReturnAmtOfItemOnInv(P.Gold)/1000;
        } 
        if (Inventory1.ReturnAmtOfItemOnInv(P.Silver) > 1000)
        {
            pts += Inventory1.ReturnAmtOfItemOnInv(P.Silver) / 2000;
        }
        if (Inventory1.ReturnAmtOfItemOnInv(P.Diamond) > 100)
        {
            pts += Inventory1.ReturnAmtOfItemOnInv(P.Diamond) / 100;
        }
        //add gold,silver,etc
        BuildingPot.Control.DockManager1.AddToPirateThreat(pts);
    }

    /// <summary>
    /// If not wheelBarrow objects are on game storages then . WheelBarrowers and DOcker will carry everytihng on 
    /// bare hands. (Crates)
    /// </summary>
    /// <returns></returns>
    static public bool ThereIsAtLeastOneOfThisOnStorage(P product)
    {
        if (GameController.Inventory1.ReturnAmtOfItemOnInv(product) > 0)
        {
            return true;
        }
        return false;
    }
}
