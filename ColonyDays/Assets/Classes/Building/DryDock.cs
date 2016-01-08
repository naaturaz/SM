using UnityEngine;
using System.Collections.Generic;

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

public class DryDock 
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

    public DryDock() { }

    public DryDock(Building build)
    {
        _building = build;
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
}
