﻿/*
 * This is a really omportatn class. This is the one the control the whole game in sense of inventory.
 * Holds the inventory of the game
 *
 *
 * Controls the game in the sense to which Scene with BUilding with Person Load.
 * Or create new game
 */

using System;
using UnityEngine;

public class GameController
{
    public bool IsGameOver { get; set; }

    private static bool _areThereWheelBarrowsOnStorage;

    /// <summary>
    /// updated every 60sec from GameScene
    /// </summary>
    public static bool AreThereWheelBarrowsOnStorage
    {
        get { return _areThereWheelBarrowsOnStorage; }
        set { _areThereWheelBarrowsOnStorage = value; }
    }

    private static bool _areThereCratesOnStorage;

    public static bool AreThereCratesOnStorage
    {
        get { return _areThereCratesOnStorage; }
        set { _areThereCratesOnStorage = value; }
    }

    private static bool _areThereCartsOnStorage;

    public static bool AreThereCartsOnStorage
    {
        get { return _areThereCartsOnStorage; }
        set { _areThereCartsOnStorage = value; }
    }

    private static bool _areThereTonelsOnStorage;

    public static bool AreThereTonelsOnStorage
    {
        get { return _areThereTonelsOnStorage; }
        set { _areThereTonelsOnStorage = value; }
    }

    private static bool _areThereWhaleOil;

    public static bool AreThereWhaleOil
    {
        get { return GameController._areThereWhaleOil; }
        set { GameController._areThereWhaleOil = value; }
    }

    private static float _notificationFrec = 120;

    public static float NotificationFrec
    {
        get { return GameController._notificationFrec; }
        set { GameController._notificationFrec = value; }
    }

    private bool _wasNegativeAlerted;

    //Main inventory of the game .. wht u see on the GUI
    //will have all tht is in all Storages combined
    //is a total inventory. Representing all tht is in those inventories
    private static ResumenInventory _resumenInventory = new ResumenInventory();

    private float _dollars;//the dollars the player has
    private static StartingCondition _startingCondition;

    private int _lastYearWorkersSalaryWasPaid;

    private NotificationsManager _notificationsManager;

    private static int _capMaxPerson = 1100;

    /// <summary>
    /// tHE Max amt of person in a game
    /// </summary>
    public static int CapMaxPerson
    {
        get { return _capMaxPerson; }
        set { _capMaxPerson = value; }
    }

    public NotificationsManager NotificationsManager1
    {
        get { return _notificationsManager; }
        set { _notificationsManager = value; }
    }

    static public ResumenInventory ResumenInventory1
    {
        get { return _resumenInventory; }
        set
        {
            _resumenInventory = value;
        }
    }

    public float Dollars
    {
        get { return _dollars; }
        set
        {
            _dollars = value;
            AlertIfOnNegative();
        }
    }

    private void AlertIfOnNegative()
    {
        if (_wasNegativeAlerted)
        {
            return;
        }

        if (Dollars < 0 && Program.gameScene.GameWasFullyLoadedAnd10SecAgo())
        {
            _wasNegativeAlerted = true;
            Dialog.OKDialog(H.Negative);
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
    }

    private Inventory CreateInitialInv(StartingCondition startingCondition)
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
        inv.Add(P.Crate, startingCondition.iniCrate);

        inv.Add(P.Cart, startingCondition.iniCart);
        inv.Add(P.Barrel, startingCondition.iniTonel);

        inv.Add(P.Cloth, startingCondition.iniCloth);
        inv.Add(P.Utensil, startingCondition.iniUtensil);
        inv.Add(P.Crockery, startingCondition.iniCrockery);

        inv.Add(P.Furniture, startingCondition.iniFurniture);
        inv.Add(P.Nail, startingCondition.iniNail);
        inv.Add(P.Mortar, startingCondition.iniMortar);
        inv.Add(P.FloorTile, startingCondition.iniFloor);
        inv.Add(P.RoofTile, startingCondition.iniRoof);

        inv.Add(P.WhaleOil, startingCondition.iniWhaleOil);

        return inv;
    }

    /// <summary>
    /// Will load the Starting conditions into the 1st Storage Type of building.
    /// This is done only at first when game is created
    /// </summary>
    private static void LoadIntoInv(Inventory invt)
    {
        if (BuildingPot.Control.FoodSources.Count == 0)
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

    public void ReCheckWhatsOnStorage()
    {
        AreThereWheelBarrowsOnStorage = ThereIsAtLeastOneOfThisOnStorage(P.WheelBarrow);
        AreThereCratesOnStorage = ThereIsAtLeastOneOfThisOnStorage(P.Crate);
        AreThereCartsOnStorage = ThereIsAtLeastOneOfThisOnStorage(P.Cart);
        AreThereTonelsOnStorage = ThereIsAtLeastOneOfThisOnStorage(P.Barrel);
        AreThereWhaleOil = ThereIsAtLeastOneOfThisOnStorage(P.WhaleOil);
    }

    public void Start()
    {
    }

    public void UpdateOneSecond()
    {
        CheckIfSalariesNeedToBePaid();
        CheckIfGameOverCondition();

        if (_notificationsManager != null)
        {
            _notificationsManager.UpdateOneSecond();
        }
    }

    private void CheckIfGameOverCondition()
    {
        if (Dollars < -100000)
        {
            GameOver('m');
        }
        if (BuildingPot.Control != null && BuildingPot.Control.DockManager1 != null &&
            BuildingPot.Control.DockManager1.PirateThreat > 90 && Program.IsPirate)
        {
            GameOver('p');
        }
    }

    private void GameOver(char type)//p pirate ... m money
    {
        if (IsGameOver)
        {
            return;
        }

        Program.gameScene.GameSpeed = 0;
        Debug.Log("Game over");
        IsGameOver = true;

        if (type == 'p')
        {
            Dialog.OKDialog(H.GameOverPirate);
        }
        else if (type == 'm')
        {
            Dialog.OKDialog(H.GameOverMoney);
        }
    }

    private void CheckIfSalariesNeedToBePaid()
    {
        if (LastYearWorkersSalaryWasPaid < Program.gameScene.GameTime1.Year
            && Program.gameScene.GameTime1.Month1 == 1)
        {
            LastYearWorkersSalaryWasPaid = Program.gameScene.GameTime1.Year;
            PirateThreat();
        }
    }

    private void PirateThreat()
    {
        var pts = 0f;
        if (Dollars > 10000)
        {
            pts = Dollars / 10000;
        }
        if (ResumenInventory1.ReturnAmtOfItemOnInv(P.Gold) > 1000)
        {
            pts += ResumenInventory1.ReturnAmtOfItemOnInv(P.Gold) / 1000;
        }
        if (ResumenInventory1.ReturnAmtOfItemOnInv(P.Silver) > 1000)
        {
            pts += ResumenInventory1.ReturnAmtOfItemOnInv(P.Silver) / 2000;
        }
        if (ResumenInventory1.ReturnAmtOfItemOnInv(P.Diamond) > 100)
        {
            pts += ResumenInventory1.ReturnAmtOfItemOnInv(P.Diamond) / 100;
        }

        //from late 2016 to Dec 18, 2019 was:
        //to make game easier
        //pts /= 100;

        pts /= 10;

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
        if (GameController.ResumenInventory1.ReturnAmtOfItemOnInv(product) > 0)
        {
            return true;
        }
        return false;
    }

    internal void NotificationsManagerInit()
    {
        _notificationsManager = new NotificationsManager();
    }

    #region GameMode Could be Normal( Peace) or War

    private static H _gameMode = H.Normal;

    public static H GameMode
    {
        get { return GameController._gameMode; }
        set { GameController._gameMode = value; }
    }

    public static void ChangeGameModeTo(H to)
    {
        if (_gameMode != to && to == H.War)
        {
            //notify the user
            OnWar(EventArgs.Empty);
            Program.MouseListener.CurrForm.ShowWarMode();
        }
        else if (_gameMode != to && to == H.Normal)
        {
            Program.MouseListener.CurrForm.HideWarMode();
        }

        _gameMode = to;
    }

    public static EventHandler<EventArgs> War;

    private static void OnWar(EventArgs e)
    {
        if (War != null)
        {
            War("", e);
        }
    }

    #endregion GameMode Could be Normal( Peace) or War
}