using UnityEngine;

/// <summary>
/// This calss instantiate visible Ships with Geomertry
/// 
/// Ship Game Object
/// </summary>
public class ShipGO : General {

    Ship _ship;
    private Building _building;
    private MoveThruPoints _moveThruPoints;

    public Building Building1
    {
        get { return _building; }
        set { _building = value; }
    }

    static public ShipGO Create(string root, Vector3 origen, Building building, H hType = H.None)
    {
        WAKEUP = true;
        ShipGO obj = null;
        obj = (ShipGO)Resources.Load(root, typeof(ShipGO));
        obj = (ShipGO)Instantiate(obj, origen, Quaternion.identity);
        obj.HType = hType;
        obj.Building1 = building;

        obj.transform.parent = building.transform;
        return obj;
    }

	// Use this for initialization
	void Start ()
	{
	    MyId = "Ship |" + HType + " | " + Id;
	    transform.name = MyId;
        _ship = new Ship(Building1);
        
        _moveThruPoints=new MoveThruPoints(Building1, this);
        _moveThruPoints.WalkRoutine(_moveThruPoints.CurrTheRoute, HPers.Work);
	}
	
    
	// Update is called once per frame
	void Update ()
    {
	    _moveThruPoints.Update();

        if (_moveThruPoints.Location == HPers.Work)
	    {
	        
	    }
	}
}
