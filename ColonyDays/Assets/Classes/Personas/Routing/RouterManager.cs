/* Create to address the Routing over Water...
 * 
 * Classes like 'Brain' wont access Router directly anymore. 
 * Will acess it thru the RouterManager
 * 
 * This class will determine if Route asked goes thru Water... 
 * if doesnt will return simple Route...
 * if does go trhu
 * 
 * 
 */

using System;
using UnityEngine;

public class RouterManager 
{
    private Router _router = new Router();
    private Structure _ini;
    private Structure _fin;
    private Person _person;

     DeltaCapsule _deltaCapsule;

    //both created for performance reason so the object are not created always and updated always
    //they are only created if needed and updated if being used 
    private bool _iAmDeltaRouting;
    private bool _iAmBridgeRouting;

    BridgeRouter _bridgeRouter ; //BridgeRouter _bridgeRouter = new BridgeRouter();

    HPers _routeType = HPers.None;

    private bool _useIniBehind;
    private bool _useFinBehind;

    private Vector3 _iniBehind;
    private Vector3 _finBehind;

    private bool _isRouteReady;
    private TheRoute _theRoute = new TheRoute();

    //the entity asking for the new Route...   
    //if is == new DateTime() then is a brand new... or not address it intance yet
    private DateTime _askDateTime = new DateTime();

    public RouterManager()
    {
        ClearOldVars();
        _router = new Router();
    }

    public RouterManager(Structure ini, Structure fin, Person person, HPers routeType, 
        bool useIniBehind = true, bool useFinBehind = true, DateTime askDateTime = new DateTime())
    {
        _iniBehind=new Vector3();
        _finBehind=new Vector3();
        _askDateTime = askDateTime;

        _ini = ini;
        _fin = fin;
        _routeType = routeType;
        _person = person;
        _useIniBehind = useIniBehind;
        _useFinBehind = useFinBehind;

        InitBehinds();
        ClearOldVars();

        Init();
    }

    void SetIsRouteReady(bool val)
    {
        _isRouteReady = val;
        _router.IsRouteReady = val;
    }

    void SetTheRoute(TheRoute val)
    {
        _router.TheRoute = val;
        _theRoute = val;
    }

    public bool IsRouteReady
    {
        get { return _isRouteReady; }
        set { SetIsRouteReady(value);}
    }

    public TheRoute TheRoute
    {
        get { return _theRoute; }
        set { SetTheRoute(value);}//so This class can be saved and loaded
    }

    public void ClearOldVars()
    {
        _isRouteReady = false; 

        if (TheRoute == null) { return; }
        _theRoute.BridgeKey = "";

        if (TheRoute.CheckPoints.Count > 0)
        {
            TheRoute.CheckPoints.Clear();
        }
    }

    #region Delta Router

    void Init()
    {
        _router.ClearOldVars();//bz for example has a brdige key set I wanna get rid of that 

        //if (PersonPot.Control.RoutesCache1.ContainANewerRoute(_ini.MyId, _fin.MyId, _askDateTime))
        //{
        //    WeHaveAnExisitingRoute();
        //}
        //else
        //{
            WeHaveToCreateTheRoute();
        //}
    }

    private TheRoute tempTheRoute;//will hold the route for a bit until is realeased on Fake()
    private void WeHaveAnExisitingRoute()
    {
       //GameScene.print("We have exisint route ");
        tempTheRoute = PersonPot.Control.RoutesCache1.GiveMeTheNewerRoute();

        time = Time.time;
        _iAmBridgeRouting = false;
        _iAmDeltaRouting = false;
    }

    void WeHaveToCreateTheRoute()
    {
        //GameScene.print("Brand new route ");

        //if is not water 
        if (!IsWaterOrMountainBtw(_ini.SpawnPoint.transform.position, _fin.SpawnPoint.transform.position))
        {
            //GameScene.print("Init() not water");
            _router = new Router(_ini.SpawnPoint.transform.position, _fin.SpawnPoint.transform.position, _person,
                _iniBehind, _finBehind, _ini.MyId, _fin.MyId);
        }
        else
        {    //try delta routing,//other wise will try bridge router, //other wise black list the building 
            _iAmDeltaRouting = true;
            if (!_useIniBehind && !_useFinBehind)
            {
                _deltaCapsule = new DeltaCapsule(_ini, _fin, _person);
            }
            else
            {
                _deltaCapsule = new DeltaCapsule(_ini.SpawnPoint.transform.position, _fin.SpawnPoint.transform.position,
                    _person, _iniBehind, _finBehind);
            }
        }
    }

    private void InitBehinds()
    {
        if (_useIniBehind)
        {
            _iniBehind = _ini.BehindMainDoorPoint;
        }
        if (_useFinBehind)
        {
            _finBehind = _fin.BehindMainDoorPoint;
        }
    }

    #endregion

    public void Update()
    {
        if (!_isRouteReady)
        {
            _router.Update();

            if (_iAmDeltaRouting)
            {
                _deltaCapsule.Update();
                PullRouteOfDeltaCapsule();
            }

            if (_iAmBridgeRouting)
            {
                _bridgeRouter.Update();
                PullRouteOfBridgeRouter();
            }

            PullRouteOfRouter();
        }

        FakeRealRoute();
    }

    private float time;
    /// <summary>
    /// Crated to fake the time of giving a route ready... bz the brain is set tht a router will take a bit
    /// too finish a route. This is to use it with the existing routes 
    /// </summary>
    private void FakeRealRoute()
    {
        if (time == 0)
        {
            return;
        }

        if (Time.time > time + 1f)
        {
            time = 0;
            SetIsRouteReady(true);
            SetTheRoute(tempTheRoute);//this route was defined on WeHaveAnExisitingRoute()
        }
    }

    void PullRouteOfRouter()
    {
        if (_router.IsRouteReady)
        {
            _isRouteReady = true;
            _theRoute = _router.TheRoute;
            _iAmBridgeRouting = false;
            _iAmDeltaRouting = false;
        }
    }

    private void PullRouteOfBridgeRouter()
    {
        //try pull bridge router
        if (_bridgeRouter.FinalRouter.IsRouteReady && _router == null)
        {
            _router = _bridgeRouter.FinalRouter;
        }
        //other wise black list the building 
    }

    void PullRouteOfDeltaCapsule()
    {
        //if birdge router was initiated then ...
        if (_iAmBridgeRouting || _router.TheRoute != null) { return; }

        //pulling the route out of delta capsule 
        if (_deltaCapsule.FinalRouter.IsRouteReady && _router.TheRoute == null)
        {
           //GameScene.print("Delta Capsule Assignment");
            _router = _deltaCapsule.FinalRouter;
        }
        else if (_router == null && _deltaCapsule.DeltaRoutingDone && !_deltaCapsule.FinalRouter.IsRouteReady)
        {
           //GameScene.print("Calling BridgeRouting");
            BridgeRouting();
        }
    }

    /// <summary>
    /// Will tell u if is water btw point A and B
    /// </summary>
    /// <returns>True if is water</returns>
    public static bool IsWaterOrMountainBtw(Vector3 origin, Vector3 target)
    {
        bool IsWaterInBtw = IsLayerInBtw(origin, target, 4);

        //Unity doesnt support RayCasting from the insede of a mesh
        //bool IsWaterInBtw = IsLayerInBtw(origin, target, 8, -1f);
        bool IsMountainInBtw = IsLayerInBtw(origin, target, 8);//8 is the Terrai Layer index
        return IsWaterInBtw || IsMountainInBtw;
    }

    /// <summary>
    /// Will return true if is obj in btw 'origin' and 'target' on the 'layer' specified 
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="target"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    static bool IsLayerInBtw(Vector3 origin, Vector3 target, int layer)
    {
        var hit = URayCast.FindObjOnMyWay(origin, target, layer, Color.magenta);

        if (hit.transform == null) { return false; }

        var distBtwOriAndTarg = Vector3.Distance(origin, target);
        var distBwtOriAndHit = Vector3.Distance(origin, hit.point);
        // UVisHelp.CreateHelpers(hit.point, Root.yellowSphereHelp);

        if (distBwtOriAndHit > distBtwOriAndTarg)
        { return false; }

        return true;
    }

    #region Bridge Routing

    private void BridgeRouting()
    {
        _iAmDeltaRouting = false;
        _iAmBridgeRouting = true;
       //GameScene.print("Bridge Routing init");
        _bridgeRouter = new BridgeRouter(_ini, _fin, _person, _routeType);
    }

    #endregion
}

class BridgeHelp
{
    private float _dist;
    private Bridge _bridge;

    public BridgeHelp(Bridge bridge, Vector3 iniPos)
    {
        _bridge = bridge;
        _dist = Vector3.Distance(iniPos, bridge.transform.position);
    }

    public float Dist
    {
        get { return _dist; }
        set { _dist = value; }
    }

    public Bridge Bridge
    {
        get { return _bridge; }
        set { _bridge = value; }
    }
}
