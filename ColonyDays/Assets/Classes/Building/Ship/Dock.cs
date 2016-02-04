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
    //the building we are in
    private Building _building;

    private Vector3 _entry;

    private SeaRouter _seaRouter;

    private GameObject _spotsContainer;
    private GameObject _entryGO;
    private List<GameObject> _allSpots = new List<GameObject>();
    private List<GameObject> _allLookPoints= new List<GameObject>();

    private List<string> _busySpots = new List<string>();
    private string _buildKey;

    public Dock()
    {
    }

    public Dock(Building build)
    {
        BuildKey = build.MyId;
        _building = build;
        InitSpots();
        _seaRouter = new SeaRouter(_entry, build);
    }

    public List<string> BusySpots
    {
        get { return _busySpots; }
        set { _busySpots = value; }
    }

    public string BuildKey
    {
        get { return _buildKey; }
        set { _buildKey = value; }
    }


    private void InitSpots()
    {
        _allSpots.Clear();
        _allLookPoints.Clear();

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

    internal TheRoute CreateRoute(string shipGoMyId)
    {
        InitSpots();

        if (_seaRouter==null)
        {
            _seaRouter = new SeaRouter(_entry, _building);
        }


        return _seaRouter.PlotRoute(_entry, _allSpots, _allLookPoints, _building, shipGoMyId);
        //UVisHelp.CreateHelpers(route, Root.yellowSphereHelp);
    }

    public void Update()
    {

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

    public void AddToBusySpots( string whoIs,string nameSpot)
    {
        _busySpots.Add(whoIs+"."+nameSpot);
        BuildingPot.Control.Registro.ResaveOnRegistro(_building.MyId);
    }

    void ReInit()
    {
        if (_building == null)
        {
            _building = Brain.GetBuildingFromKey(BuildKey);
            InitSpots();
        } 
    }

    public void RemoveFromBusySpots(string whoIs)
    {
        ReInit();

        var index = _busySpots.FindIndex(a => a.Contains(whoIs));

        _busySpots.RemoveAt(index);
        BuildingPot.Control.Registro.ResaveOnRegistro(_building.MyId);
    }

    public bool ItHasAtLeastAFreeSpot()
    {
        ReInit();


        if (_allSpots.Count <= _busySpots.Count)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// looking for 'A 3' for example
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    internal bool IsSpotFree(string p)
    {
        var index = _busySpots.FindIndex(a => a.Contains(p));

        return index == -1;
    }
}
