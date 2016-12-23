using System;
using System.Collections.Generic;

public class DockManager
{
    private float _portReputation = 10;
    private float _pirateThreat = 0;//0

    private int _maxAmountSpots = 3;

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

    public List<string> DockStructures
    {
        get { return _dockStructures; }
        set { _dockStructures = value; }
    }

    public DockManager()
    {

    }

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
        if (HType == H.Shipyard || HType == H.Supplier || HType == H.Dock)
        {
            return true;
        }
        return false;
    }

    internal bool AtLeastOneDockHasSpace1More(int shipsOnIsland)
    {
        // how many ships each structure can hold 
        if (_dockStructures.Count * _maxAmountSpots <= shipsOnIsland)
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

    public void AddSurvey(float survey)
    {
        PortReputation = UMath.Clamper(survey, PortReputation, 0, 100);

        if (PortReputation < 0)
        {
            PortReputation = 0;
        }
    }

    internal void AddToPirateThreat(float amtChange)
    {
        if (!Program.IsPirate)
        {
            return;
        }

        PirateThreat = UMath.Clamper(amtChange, PirateThreat, 0, 100);
    }

    public void Update()
    {
        
    }
}
