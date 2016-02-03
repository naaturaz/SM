using UnityEngine;

/// <summary>
/// The ship calss is the one that ask for Import and Exports on the Dock ExportImportDispath
/// </summary>
public class Ship
{
    private Building _dock;
    Inventory _inventory = new Inventory();
    private float _size = 20f;

    public Building Dock
    {
        get { return _dock; }
        set { _dock = value; }
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

    public Ship(Building dock)
    {
        _dock = dock;
        DebugInit();
    }

    /// <summary>
    /// To emulate the user entering orders 
    /// </summary>
    void DebugInit()
    {
        //_dock.Export(new Order(P.Gold, "Ship", 1));

        Order order = new Order(P.Axe, "", "Ship");
        order.Amount = 1;
        _dock.Dock1.Import(order);
    }

    void CheckIfImportOrders()
    {
        if (_dock.Dispatch1.HasImportOrders())
        {
            _dock.Dispatch1.Import(_dock);
        }
    }

    private void CheckIfExportOrders()
    {
        if (_dock.Dispatch1.HasExportOrders())
        {
            _dock.Dispatch1.Export(_dock);
        }
    }

    private float lastCheck;
    public void Update()
    {
        if (Time.time > lastCheck + 10f)
        {
            CheckIfImportOrders();
            CheckIfExportOrders();

            lastCheck = Time.time;
        }
    }
}