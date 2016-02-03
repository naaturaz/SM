using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipManager
{
    private MDate _nextVisit;
    List<ShipGO> _shipGOs = new List<ShipGO>(); 

    public MDate NextVisit
    {
        get { return _nextVisit; }
        set { _nextVisit = value; }
    }

    public ShipManager() { }


    public void Update ()
    {
        CheckIfTimeToVisit();
	    CheckWhenNextVisit();
	}

    private void CheckIfTimeToVisit()
    {
        if (_nextVisit != null && IsTheVisitPastOrNow() && 
            BuildingPot.Control.DockManager1.HasSpaceForOneMore(_shipGOs.Count))
        {
            NewShipComingToUs();
        }
    }

    /// <summary>
    /// Instance a gameObject ship 
    /// </summary>
    private void NewShipComingToUs()
    {
        //Building build = BuildingPot.Control.DockManager1.GiveMeRandomBuilding();

    }

    private void CheckWhenNextVisit()
    {
        if (!BuildingPot.Control.DockManager1.HasAtLeastOneDockStructure())
        {
            return;
        }

        if (_nextVisit == null || IsTheVisitPastOrNow())
        {
            SetNextVisit();
        }
    }

    private void SetNextVisit()
    {
        //+1 in case is too low or zero
        //so if is 10 the PortRep 3600 / 10 is 1 year
        //so if is 100 the PortRep 3600 / 100 is 36 days
        var daysFromNow = 3600 / (BuildingPot.Control.DockManager1.PortReputation + 1);
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
}
