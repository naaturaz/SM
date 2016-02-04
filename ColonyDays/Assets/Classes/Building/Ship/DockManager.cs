﻿using System;
using System.Collections.Generic;

public class DockManager
{
    private float _portReputation = 50;
    private float _pirateThreat;

    List<string> _dockStructures = new List<string>(); 

    public float PortReputation
    {
        get { return _portReputation; }
        set { _portReputation = value; }
    }

    public float PirateThreat
    {
        get { return _pirateThreat; }
        set { _pirateThreat = value; }
    }

    public DockManager()
    { }

    internal bool HasAtLeastOneDockStructure()
    {
        return _dockStructures.Count > 0;
    }

    /// <summary>
    /// Called from Registro to keep record of Dock structures 
    /// </summary>
    /// <param name="MyIDP"></param>
    /// <param name="hType"></param>
    public void AddToDockStructure(string MyIDP, H hType)
    {
        if (IsDockType(hType))
        {
            if (!_dockStructures.Contains(MyIDP))
            {
                _dockStructures.Add(MyIDP);
            }
        }
    }

    /// <summary>
    /// Called from Registro to keep record of Dock structures 
    /// </summary>
    /// <param name="MyIDP"></param>
    /// <param name="hType"></param>
    public void RemoveFromDockStructure(string MyIDP, H hType)
    {
        if (IsDockType(hType))
        {
            if (_dockStructures.Contains(MyIDP))
            {
                _dockStructures.Remove(MyIDP);
            }
        }
    }

    bool IsDockType(H HType)
    {
        if (HType == H.DryDock || HType == H.Supplier || HType == H.Dock)
        {
            return true;
        }
        return false;
    }

    internal bool AtLeastOneDockHasSpace1More(int shipsOnIsland)
    {
        //15 how many ships each structure can hold 
        if (_dockStructures.Count * 15 <= shipsOnIsland)
        {
            return false;
        }
        return true;
    }

    private int recuCount;
    internal Building GiveMeRandomBuilding()
    {
        var key = _dockStructures[UMath.GiveRandom(0, _dockStructures.Count)];
        var build = Brain.GetBuildingFromKey(key);

        if (build.Dock1.ItHasAtLeastAFreeSpot())
        {
            recuCount = 0;
            return build;
        }
        recuCount++;

        if (recuCount>1000)
        {
            throw new Exception("inf loopp dock manag");
        }

        //will recurse until finds one tht has a free spot 
        return GiveMeRandomBuilding();
    }
}