using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/*
 * This class is used by DryDock building 
 * and supplier building
 * 
 * They bigger diff as on now is the diff amount of products each
 * one can add to they inventory . This is restricted on the AddExport Window
 * for DryDock and Supplier
 * 
 * 
 * 
 */

public class Dock 
{
    private Ship _currShip;
    //where the ship is shown on Screen. 
    //Ship will be removed moved from sea to here
    private Vector3 _shipDockPoint;

    //the place where ship was on sea 
    //so once we are done with it we can place it back there
    private Vector3 _shipSeaPoint;

    //the building we are in
    private Building _building;

    private Vector3 _entry;

    private SeaRouter _seaRouter;

    private GameObject _spotsContainer;
    private GameObject _entryGO;
    private List<GameObject> _allSpots = new List<GameObject>();
    private List<GameObject> _allLookPoints= new List<GameObject>();
 

    public Dock() { }

    public Dock(Building build)
    {
        _building = build;
        InitSpots();
        _seaRouter = new SeaRouter(_entry, build);
    }

    private void InitSpots()
    {
        _spotsContainer = General.FindGameObjectInHierarchy("SpotsContainer", _building.gameObject);

        var allChild = General.FindAllChildsGameObjectInHierarchy(_spotsContainer);

        for (int i = 0; i < allChild.Count(); i++)
        {
            string nameGO = allChild[i].transform.name;

            if (nameGO == "Entry")
            {
                _entryGO = allChild[i];
                _entry = allChild[i].transform.position;
            }
            else if (nameGO.Length==3)
            {
                _allSpots.Add(allChild[i]);
            }
            else if (nameGO.Length == 1)
            {
                _allLookPoints.Add(allChild[i]);
            }
        }
    }

    public bool CanIReachRoute()
    {
        UpdateEntry();   
        return _seaRouter.CanRoute(_entry);
    }

    private void UpdateEntry()
    {
        _entry = _entryGO.transform.position;
    }

    internal void CreateRoute()
    {
        InitSpots();

        var route = _seaRouter.PlotRoute(_entry, _allSpots, _allLookPoints);

        //UVisHelp.CreateHelpers(route, Root.yellowSphereHelp);
    }

    /// <summary>
    /// After ship asked if he can DryDock then can be placed here
    /// </summary>
    /// <param name="newShip"></param>
    public void ServiceMe(Ship newShip)
    {
        _currShip = newShip;
        //
        //_shipSeaPoint = newShip.pos
        //_currShip.pos = _shipDockPoint;

        PrepareOrder();
    }

    private void PrepareOrder()
    {
        var items = _building.Inventory.ReturnInvItemsForSize(_currShip.Size);
        Bill(items);
    }

    /// <summary>
    /// The action of Billing  a ship
    /// </summary>
    void Bill(List<InvItem> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Program.gameScene.ExportImport1.Sale(list[i].Key, list[i].Amount);
        }

        Program.gameScene.GameController1.Dollars += WorkersSalary();
    }

    float WorkersSalary()
    {
        return _building.PeopleDict.Count * _building.DollarsPay;//+time
    }

    /// <summary>
    /// A Ship asking if can DryDock
    /// </summary>
    /// <param name="newShip"></param>
    /// <returns></returns>
    public bool CanIBeService(Ship newShip)
    {
        if (_currShip == null)
        {
            return true;
        }
        return false;
    }

    public void Update()
    {
        if (_currShip != null)
        {
            _currShip.Update();
        } 
    }

    /// <summary>
    /// ACtion from the user tht need an 'item' to be import 
    /// </summary>
    /// <param name="item"></param>
    public void Import(Order order)
    {
        _building.Dispatch1.AddToExpImpOrders(order);
    }

    /// <summary>
    /// ACtion from the User when needs Export and order 
    /// </summary>
    /// <param name="order"></param>
    public void Export(Order order)
    {
        Order exp = new Order();
        exp = order;
        _building.Dispatch1.AddToExpImpOrders(exp);


        //so Dockers starts looking for this in the Storage Buildings 
        Order local = new Order();
        local = order;
        _building.Dispatch1.AddToOrdersToDock(local);
    }


}
