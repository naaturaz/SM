/*
 * IMPORTANT: IF LANDZONES ARE NOT SET THE ROUTING SYSTEM WONT WORK
 * 
 * 
 */

public class CryRouteManager
{
    private HPers _routeType;
    private Person _person;
    private VectorLand _one;
    private VectorLand _two;

    private CryRoute _cryRoute = new CryRoute();
    CryBridgeRoute _cryBridgeRoute ;
    
    private bool _isRouteReady;
    private TheRoute _theRoute = new TheRoute();
    private Structure _ini;
    private Structure _fin;

    private bool _iniDoor;
    private bool _finDoor;

    private string _destinyKey;//to be added to TheRoute obj

    void SetIsRouteReady(bool val)
    {
        _isRouteReady = val;
        _cryRoute.IsRouteReady = val;
    }

    void SetTheRoute(TheRoute val)
    {
        _cryRoute.TheRoute = val;
        _theRoute = val;
    }

    public bool IsRouteReady
    {
        get { return _isRouteReady; }
        set { SetIsRouteReady(value); }
    }

    public TheRoute TheRoute
    {
        get { return _theRoute; }
        set { SetTheRoute(value); }//so This class can be saved and loaded
    }

    public CryRouteManager(){}

    public CryRouteManager(Structure ini, Structure fin, Person person,
        HPers routeType = HPers.None, bool iniDoor = true, bool finDoor = true)
    {
        _destinyKey = fin.MyId;
        
        _iniDoor = iniDoor;
        _finDoor = finDoor;

        _routeType = routeType;
        _person = person;

        DefineOneAndTwo(ini, fin);
        
        _ini = ini;
        _fin = fin;

        ClearOldVars();
        Init();
    }

    /// <summary>
    /// Soe when idle dummy is created doesnt send people to Vector3.zero
    /// </summary>
    /// <param name="fin"></param>
    void CloneFin(Structure fin)
    {
        
    }

    void DefineOneAndTwo(Structure ini, Structure fin)
    {
        if (ini != null && ini.LandZone1.Count > 0)
        {
            _one = ini.LandZone1[0];
        }
        else
        {
            _one = new VectorLand("", ini.transform.position);
        }        
        
        if (fin != null && fin.LandZone1.Count > 0)
        {
            _two = fin.LandZone1[0];
        }
        else
        {
            _two = new VectorLand("", fin.transform.position);
        }
    }

    private void ClearOldVars()
    {
        _isRouteReady = false;

        if (_theRoute != null)
        {
            _theRoute.CheckPoints.Clear();
        }
        _theRoute = null;

        _cryRoute = null;
        _cryBridgeRoute = null;
    }

    private void Init()
    {
        //will stop a lot of instances where the landzone is not being initiated
        if (_one.LandZone == "" || _two.LandZone == "")
        {
            return;
        }


        if (_one.LandZone != _two.LandZone)
        {
            _cryBridgeRoute = new CryBridgeRoute(_ini, _fin, _person, _destinyKey);
        }
        else _cryRoute = new CryRoute(_ini, _fin, _person, _destinyKey, _iniDoor, _finDoor);
    }

    public void Update () 
    {
        if (_cryRoute != null && !_isRouteReady)
        {
            _cryRoute.Update();

            if (_cryRoute.IsRouteReady)
            {
                _isRouteReady = true;
                _theRoute = _cryRoute.TheRoute;
            }
        }

        if (_cryBridgeRoute != null && !_isRouteReady)
        {
            _cryBridgeRoute.Update();

            if (_cryBridgeRoute.IsRouteReady)
            {
                _isRouteReady = true;
                _theRoute = _cryBridgeRoute.TheRoute;
            }
        }
	}
}
