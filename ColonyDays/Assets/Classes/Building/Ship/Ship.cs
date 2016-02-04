using UnityEngine;

/// <summary>
/// The ship calss is the one that ask for Import and Exports on the Dock ExportImportDispath
/// </summary>
public class Ship
{
    private Building _building;
    Inventory _inventory = new Inventory();
    private float _size = 20f;

    private bool _didDockImport;
    private bool _didDockExport;


    private MDate _leaveDate;

    public Building Building
    {
        get { return _building; }
        set { _building = value; }
    }

    public Inventory Inventory1
    {
        get { return _inventory; }
        set { _inventory = value; }
    }

    public float Size
    {
        get { return _size; }
        set { _size = value; }
    }


    public MDate LeaveDate
    {
        get { return _leaveDate; }
        set { _leaveDate = value; }
    }

    public Ship(Building build)
    {
        _building = build;

    }

    void CheckIfImportOrders()
    {
        if (_building.Dispatch1.HasImportOrders() && HasInvAvail())
        {
            _didDockImport =_building.Dispatch1.Import(_building);
        }
    }



    private void CheckIfExportOrders()
    {
        if (_building.Dispatch1.HasExportOrders())
        {
            _didDockExport = _building.Dispatch1.Export(_building);
        }
    }


    private int random = -1;
    /// <summary>
    /// Will randomize if has inventory for this order
    /// </summary>
    /// <returns></returns>
    private bool HasInvAvail()
    {
        if (random!=-1)
        {
            return random > 0 && random < 8;
        }

        random = UMath.GiveRandom(0, 11);
        return random > 0 && random < 8;
    }

    private float lastCheck;
    public void Update()
    {
        if (Time.time > lastCheck + 10f)
        {
            lastCheck = Time.time;
        }
    }

    public void CheckDockOrders()
    {
        CheckIfImportOrders();
        CheckIfExportOrders();
    }

    public void SetLeaveDate()
    {
        var daysOnDock = UMath.GiveRandom(30, 60);
        LeaveDate = Program.gameScene.GameTime1.ReturnCurrentDatePlsAdded(daysOnDock);
    }

    public void Leaving(string myID)
    {
        Building.Dock1.RemoveFromBusySpots(myID);

        BuildingPot.Control.DockManager1.PortReputation += Survey();
    }

    /// <summary>
    /// What will be added to Port reputation for each ship 
    /// </summary>
    /// <returns></returns>
    float Survey()
    {
        float impo = -0.1f;
        float expo = -0.1f;

        if (_didDockImport)
        {
            impo *= -1;
        }
        if (_didDockExport)
        {
            expo *= -1;
        }

        if (Building.HType == H.DryDock || Building.HType == H.Supplier)
        {
            return expo*2;
        }
        if (Building.HType == H.Dock)
        {
            return expo + impo;
        }
        return 0f;
    }
}