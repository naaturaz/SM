using UnityEngine;

/// <summary>
/// This class spawns visible Ships with Geometry
///
/// Ship Game Object
/// </summary>
public class ShipGO : General
{
    private Ship _ship;
    private Building _building;

    public Building Building1
    {
        get { return _building; }
        set { _building = value; }
    }

    public Ship Ship1
    {
        get { return _ship; }
        set { _ship = value; }
    }

    static public ShipGO Create(string root, Vector3 origen, Building building, Ship ship, H hType = H.None, string myID = "")
    {
        WAKEUP = true;
        ShipGO obj = null;
        obj = (ShipGO)Resources.Load(root, typeof(ShipGO));
        obj = (ShipGO)Instantiate(obj, origen, Quaternion.identity);
        obj.HType = hType;
        obj.Building1 = building;
        obj.Ship1 = ship;

        obj.transform.SetParent(building.transform);
        obj.MyId = myID;

        return obj;
    }

    // Use this for initialization
    private void Start()
    {
        //new objetc
        if (Ship1.Position == new Vector3())
        {
            MyId = "Ship |" + HType + " | " + Id;
            Ship1.MoveThruPoints1 = new MoveThruPoints(Building1, gameObject, MyId);
            Ship1.MoveThruPoints1.WalkRoutine(Ship1.MoveThruPoints1.CurrTheRoute, HPers.Work);
        }
        //loading
        else
        {
            transform.position = Ship1.Position;
            transform.rotation = Ship1.Rotation;
        }

        transform.name = MyId;
    }

    // Update is called once per frame
    private void Update()
    {
        _ship.Update();

        if (Ship1.MoveThruPoints1.Location == HPers.Work && _ship.LeaveDate == null)
        {
            _ship.CheckDockOrders();

            _ship.SetLeaveDate();
        }

        CheckIfIsLeaveDate();
        CheckIfHome();
    }

    private void CheckIfHome()
    {
        if (_ship == null || _ship.LeaveDate == null)
        {
            return;
        }

        //thats is its back to its original point
        if (Ship1.MoveThruPoints1.Location == HPers.Home)
        {
            BuildingPot.Control.ShipManager1.RemoveMeFromShipsOnIsland(Ship1);
            Destroy();
        }
    }

    private void CheckIfIsLeaveDate()
    {
        if (_ship == null || _ship.LeaveDate == null)
        {
            return;
        }
        if (GameTime.IsPastOrNow(_ship.LeaveDate) && Ship1.MoveThruPoints1.Location == HPers.Work && !Ship1.MoveThruPoints1.MovingNow)
        {
            _ship.CheckDockOrders();

            Ship1.MoveThruPoints1.WalkRoutine(Ship1.MoveThruPoints1.CurrTheRoute, HPers.Home, true);
            _ship.Leaving(MyId);
        }
    }
}