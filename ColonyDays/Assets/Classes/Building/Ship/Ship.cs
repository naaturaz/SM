using UnityEngine;

/// <summary>
/// The ship calss is the one that ask for Import and Exports on the Dock ExportImportDispath
/// </summary>
public class Ship
{
    private Building _building;
    private string _buildKey;

    Inventory _inventory = new Inventory();
    private float _size = 20f;

    private bool _didDockImport;
    private bool _didDockExport;
    
    private MDate _leaveDate;

    private string _root;
    private H _hType;
    private ShipGO _shipGo;

    private Quaternion _rotation;
    private Vector3 _position;

    private MoveThruPoints _moveThruPoints;

    private string _myID;


    private bool _isToRecreate;

    public Building Building()
    {
        return _building;
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

    public string BuildKey
    {
        get { return _buildKey; }
        set { _buildKey = value; }
    }

    public Quaternion Rotation
    {
        get { return _rotation; }
        set { _rotation = value; }
    }

    public Vector3 Position
    {
        get { return _position; }
        set { _position = value; }
    }

    public string Root1
    {
        get { return _root; }
        set { _root = value; }
    }

    public H HType
    {
        get { return _hType; }
        set { _hType = value; }
    }

    public bool DidDockImport
    {
        get { return _didDockImport; }
        set { _didDockImport = value; }
    }

    public bool DidDockExport
    {
        get { return _didDockExport; }
        set { _didDockExport = value; }
    }

    public MoveThruPoints MoveThruPoints1
    {
        get { return _moveThruPoints; }
        set { _moveThruPoints = value; }
    }

    public string MyId
    {
        get { return _myID; }
        set { _myID = value; }
    }

    public Ship(){}

    /// <summary>
    /// Instancing new Object 
    /// </summary>
    /// <param name="root"></param>
    /// <param name="build"></param>
    /// <param name="hType"></param>
    public Ship(string root, Building build, H hType)
    {
        _root = root;
        _building = build;
        _buildKey = build.MyId;
        _hType = hType;

        _shipGo = ShipGO.Create(_root, new Vector3(), _building, this, _hType);
    }

    internal void ReCreateShip()
    {
        _building = Brain.GetBuildingFromKey(BuildKey);

        if (_building == null) return;

        _shipGo = ShipGO.Create(_root, new Vector3(), _building, this, _hType, MyId);
        MoveThruPoints1.PassGameObject(_shipGo.gameObject);

        MoveThruPoints1.WalkRoutineLoad(MoveThruPoints1.CurrTheRoute, MoveThruPoints1.GoingTo,
            MoveThruPoints1.CurrentRoutePoint,
            MoveThruPoints1.Inverse, MoveThruPoints1.WhichRoute);
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

            if (_didDockExport)
            {
            }
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
        MoveThruPoints1.Update();

        if (Time.time > lastCheck + 10f)
        {
            lastCheck = Time.time;
        }

        if (_shipGo!=null)
        {
            Rotation = _shipGo.transform.rotation;
            Position = _shipGo.transform.position;
        }



        if (string.IsNullOrEmpty(MyId))
        {
            MyId = _shipGo.MyId;
        }
    }

    /// <summary>
    /// When arrive port wht to do
    /// </summary>
    public void CheckDockOrders()
    {
        CheckIfImportOrders();
        CheckIfExportOrders();
        _moveThruPoints.SailDown();
    }

    public void SetLeaveDate()
    {
        Program.gameScene.GameController1.NotificationsManager1.Notify("ShipArrived");

        var daysOnDock = UMath.GiveRandom(30, 60);
        LeaveDate = Program.gameScene.GameTime1.ReturnCurrentDatePlsAdded(daysOnDock);
    }


    public void Leaving(string myID)
    {
        Building().Dock1.RemoveFromBusySpots(myID);

        BuildingPot.Control.DockManager1.AddSurvey(Survey());
        _moveThruPoints.SailUp();

    }

    /// <summary>
    /// What will be added to Port reputation for each ship 
    /// </summary>
    /// <returns></returns>
    float Survey()
    {
        float expo = -0.1f;
        if (_didDockExport)
        {
            expo *= -1;
        }

        if (Building().HType == H.Shipyard || Building().HType == H.Supplier)
        {
            return expo*2;
        }
        if (Building().HType == H.Dock)
        {
            if(_didDockExport && _didDockImport)
            {
                return 0.6f;//0.2
            }
            else if (_didDockExport)
            {
                return 0.3f;//0.1
            }
            else if(_didDockImport)
            {
                return 0.3f;//0.1
            }
            else
            {
                return -.05f;//-0.1
            }
        }
        return 0f;
    }


}