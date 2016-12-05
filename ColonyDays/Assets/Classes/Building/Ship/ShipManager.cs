using System.Collections.Generic;
using UnityEngine;

public class ShipManager
{
    private MDate _nextVisit;
    List<Ship> _ships = new List<Ship>();
    private int _isToLoadShips = -1;

    List<string> _shipsRoot = new List<string>()
    { "Prefab/Ship/ShipSmall1", "Prefab/Ship/ShipSmall2", "Prefab/Ship/ShipSmall3" }; 

    public MDate NextVisit
    {
        get { return _nextVisit; }
        set { _nextVisit = value; }
    }

    public List<Ship> Ships
    {
        get { return _ships; }
        set { _ships = value; }
    }

    public ShipManager() { }

    public void Update ()
    {
        CheckIfTimeToVisit();
	    CheckWhenNextVisit();

        if (_isToLoadShips == 1 && BuildingPot.Control.Registro.IsFullyLoaded)
        {
            _isToLoadShips = 0;
            LoadShips();
        }
	}

    private void CheckIfTimeToVisit()
    {
        if (_nextVisit != null && IsTheVisitPastOrNow() && 
            BuildingPot.Control.DockManager1.AtLeastOneDockHasSpace1More(_ships.Count))
        {
            _nextVisit = null;
            NewShipComingToUs();
        }
    }

    /// <summary>
    /// Instance a gameObject ship 
    /// </summary>
    private void NewShipComingToUs()
    {
        Building build = BuildingPot.Control.DockManager1.GiveMeRandomBuilding();

        if (build == null)
        {
            return;
        }

        //_ships.Add(ShipGO.Create(Root.shipSmall, new Vector3(), build, H.ShipSmall));
        _ships.Add(new Ship(_shipsRoot[UMath.GiveRandom(0,_shipsRoot.Count)], build, H.ShipSmall));
    }




    private void CheckWhenNextVisit()
    {
        if (!BuildingPot.Control.DockManager1.HasAtLeastOneDockStructure())
        {
            return;
        }

        if (_nextVisit == null)
        {
            SetNextVisit();
        }
    }

    private void SetNextVisit()
    {
        //+1 in case is too low or zero
        //so if is 10 the PortRep 3600 / 10 is 1 year
        //so if is 100 the PortRep 3600 / 100 is 36 days
        //with 6000 is every 60 days, 2 month with 100 as PortRep
        var daysFromNow = 6000 / (BuildingPot.Control.DockManager1.PortReputation + 1);
        _nextVisit = Program.gameScene.GameTime1.ReturnCurrentDatePlsAdded(daysFromNow);
    }

    private bool IsTheVisitPastOrNow()
    {
        if (_nextVisit.Month1 <= Program.gameScene.GameTime1.Month1 && _nextVisit.Year <= Program.gameScene.GameTime1.Year)
        {
            return true;
        }
        return false;
    }

    internal void RemoveMeFromShipsOnIsland(Ship shipGO)
    {
        _ships.Remove(shipGO);

    }

    internal void MarkToLoadShips()
    {
        if (_isToLoadShips==-1)
        {
            _isToLoadShips = 1;
           //Debug.Log("_isToLoadShips tru:");
        }
    }

    void LoadShips()
    {
        for (int i = 0; i < Ships.Count; i++)
        {
            Ships[i].ReCreateShip();
           //Debug.Log("recreate ship:" + Ships[i].MyId);
        }
    }
}
