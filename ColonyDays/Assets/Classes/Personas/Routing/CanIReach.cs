using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/*Class that the only purpose is to tell u if is a Brdige Reacheable from Point 'a'
 * and at the same time will tell u wich end is reacheable
 * 
 * And its Asycn takes a bit to find if is reachable or not sicne if is not 
 * reachlble directly will try to DeltaRoute it
 */
public class CanIReach : MonoBehaviour
{
    private Bridge _bridge;
    private Vector3 _endABot;
    private Vector3 _endBBot;
    private Vector3 _endATop;
    private Vector3 _endBTop;
    
    private Vector3 _from;
    private Vector3 _to;

    //to try delta routing btw points 
    DeltaCapsule _deltaCapsule = new DeltaCapsule();
    private Person _person;

    //this is a ASync flag that if is being used will tell if a delta route is routable or not
    /* values. -100 not initited. -1 false. 0 still processing. 1 true
     */
    private int _isRoutable = -100;
    private Vector3 _currentTo;//for delta .. this is the bottom of the Bridge
    private Vector3 _currentToTop;//for saving when delta  .. this is the Top of the Bridge

    ReachBean _reachBean = new ReachBean();//final result
    private bool _isStarted;

    //will keep store temp the pair used to create Routes on Brdiges.. so when asking brdige for 
    //bot and top always gives the same 
    private int _pairUsed = -1;//restarted only when SetStarttoFalse() called
    private static Person oldPerson;//to keep track of last person so if not the same will reset pair usd

    private bool _inverse;//if is being started for the end position of a route 
    

    public ReachBean Bean
    {
        get { return _reachBean; }
        set { _reachBean = value; }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
        set { _isStarted = value; }
    }

    public int PairUsed
    {
        get { return _pairUsed; }
        set { _pairUsed = value; }
    }

    public CanIReach() { }

    private static int debug;

    public CanIReach(Vector3 from, Bridge bridge, Person person, bool inverse = false)
    {
        RestartVar();
        _bridge = bridge;
        _from = from;
        _person = person;
        _inverse = inverse;
        FindInitPointOnBridge();
        FindTopPointOnBridge();
        TryDirectBoth();
    }

    void RestartVar()
    {
        OldPersonMatters();

        _isRoutable = 0;//is processing 
        _isStarted = true;
        _endABot=new Vector3();
        _endBBot = new Vector3();
        _currentTo = new Vector3();
    }

    /// <summary>
    /// Will take care of restart 'PairUsed' of the person used now is diff than last one 
    /// </summary>
    void OldPersonMatters()
    {
        if (oldPerson == null || oldPerson != _person)
        {
            oldPerson = _person;
            PairUsed = -1;
        }
    }

    public bool IsActive()
    {
        if (_isRoutable == -100)
        {
            return false;
        }
        return true;
    }
    
    /// <summary>
    /// Makes _isDeltaRoutable = -100
    /// </summary>
    public void Restart()
    {
        _isRoutable = -100;
    }

    public void SetIsStartedToFalse()
    {
        IsStarted = false;//so can assign again
        //_pairUsed = -1;//so the new bridge will use reamdon new cval when pick bottom and top in the bridge
    }

    /// <summary>
    /// Defines the Two ends of a Brdige 
    /// </summary>
    void FindInitPointOnBridge()
    {
        List<StructureParent> currentBridgeParts12 = _bridge.GiveTheTwoEndsParts10and12();
        var part12ABotton = currentBridgeParts12[0].BottomTop(this)[0].transform.position;
        var part12BBottom = currentBridgeParts12[1].BottomTop(this)[0].transform.position;

        var posit = new List<Vector3>() { part12ABotton, part12BBottom };
        _endABot = UMath.ReturnClosestVector3(posit, _from);
        _endBBot = UMath.ReturnFarestVector3(posit, _from);
    }

    /// <summary>
    /// Defines the Two ends of a Brdige 
    /// </summary>
    void FindTopPointOnBridge()
    {
        List<StructureParent> currentBridgeParts12 = _bridge.GiveTheTwoEndsParts10and12();
        var part12ATop = currentBridgeParts12[0].BottomTop(this)[1].transform.position;
        var part12BTop = currentBridgeParts12[1].BottomTop(this)[1].transform.position;

        var posit = new List<Vector3>() { part12ATop, part12BTop };
        _endATop = UMath.ReturnClosestVector3(posit, _from);
        _endBTop = UMath.ReturnFarestVector3(posit, _from);
    }

    private void TryDirectBoth()
    {
        if (!RouterManager.IsWaterOrMountainBtw(_from, _endABot))
        {
            ConformValidResult(_from, _endABot);
        }
        else if (!RouterManager.IsWaterOrMountainBtw(_from, _endBBot))
        {
            ConformValidResult(_from, _endBBot);
        }
        else
        {
            _currentTo = _endABot;
            TryDeltaRoute(_from, _currentTo);
        }
    }

    /// <summary>
    /// Address if _inverse is flagged . This is when calling this class an ini point is the end one
    /// 
    /// 
    /// 
    /// 
    /// Needs to be called in Router Creation on Bean 
    /// 
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static void InvertIfNeeded(ref Vector3 a, ref Vector3 b, bool inverse)
    {
        if (inverse)
        {
            Vector3 t = a;
            a = b;
            b = t;
        }
    }

    void TryDirect(Vector3 fromP, Vector3 toP)
    {
        if (!RouterManager.IsWaterOrMountainBtw(fromP, toP))
        {
            ConformValidResult(fromP, toP);
        }
        else
        {
            _currentTo = toP;
            TryDeltaRoute(_from, _currentTo);
        }
    }

    void TryDeltaRoute(Vector3 fromP, Vector3 toP)
    {
        InvertIfNeeded(ref fromP, ref toP, _inverse);

        _deltaCapsule = new DeltaCapsule(fromP, toP, _person);
    }

    /// <summary>
    /// Conform valid result and passes the DeltaCapsule too
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to">the end from the bridge , the bottom </param>
    /// <param name="deltaCapsule"></param>
    void ConformValidResult(Vector3 from, Vector3 to, DeltaCapsule deltaCapsule)
    {
        Vector3[] f4Points = new Vector3[4]{_endABot, _endATop, _endBTop, _endBBot};

        _reachBean = new ReachBean(from, f4Points, deltaCapsule, _person, _inverse);
        Restart();
    }

    void ConformValidResult(Vector3 from, Vector3 to)
    {
        Vector3[] f4Points = new Vector3[4] { _endABot, _endATop, _endBTop, _endBBot };

        _reachBean = new ReachBean(from, f4Points, _person, _inverse);
        Restart();
    }

    /// <summary>
    /// Will return the closest top from 'bottom' in the current bridge 
    /// </summary>
    /// <param name="bottom"></param>
    Vector3 ClosestTop(Vector3 bottom)
    {
        List<Vector3> tops = new List<Vector3>() { _endATop, _endBTop };
        return UMath.ReturnClosestVector3(tops, bottom);
    }

	// Update is called once per frame
	public void Update ()
    {
        _deltaCapsule.Update();
        CheckOnDeltaCapsule();
        _reachBean.Update();
	}

    /// <summary>
    /// If Delta Capsule was initiated will keep checking to see the outcome of it
    /// </summary>
    private void CheckOnDeltaCapsule()
    {
        //means is not used
        if (_isRoutable == -100)
        { return; }

        DecideOnRouteOfDeltaCapsule();
    }

    /// <summary>
    /// Will only be executed when _deltaCapsule is Done and then will tell if is routable or not...
    /// then will call RecurseRoutine()
    /// </summary>
    void DecideOnRouteOfDeltaCapsule()
    {
        if (_deltaCapsule.FinalRouter.IsRouteReady && _deltaCapsule.DeltaRoutingDone)
        {
            _isRoutable = 1;
            //needs to keep the delta capsule to conform route 
            ConformValidResult(_from, _currentTo, _deltaCapsule);
        }
        else if (_deltaCapsule.DeltaRoutingDone && !_deltaCapsule.FinalRouter.IsRouteReady)
        {
            _isRoutable = -1;
            CheckIf1stPointOnBridge();
        }
    }

    void CheckIf1stPointOnBridge()
    {
        if (_currentTo == _endABot && _currentTo != new Vector3())
        {
            _currentTo = _endBBot;
            TryDeltaRoute(_from, _currentTo);
        }
        else
        {
            Restart();
        }
    }
}

/// <summary>
/// Will contain the result of CanIReach.cs 
/// 
/// Will conform its partial route too.
/// 
/// If has delta route will added with the Brdige points.
/// If doesnt will Route it with Router.cs and then will add the bridge points
/// </summary>
public class ReachBean
{
    private bool _isReacheable;
    //here bz if u pass a bridge needs to know which point on the brdige is the accessible one 
    private Vector3 _validFrom;
    private Vector3 _validToBot;
    private Vector3 _validToTop;
    private int _result = -5;//-5 virgin or still proceessing, 1 true, -1 flase
    private int _keepResult;
    DeltaCapsule _deltaCapsule = new DeltaCapsule();

    Router _router = new Router();

    TheRoute _theRoute = new TheRoute();//from '_validFrom' to '_validToBot'
    //the 4 points on the route in a brdige.. Will only need to gather this in the first pass of a Bridge 
    //so when conforming route on BridgeRouter this is onlyy needed once out of the first CanIReach Instance 
    TheRoute _theRouteInBridge = new TheRoute();
    private Person _person;
    List<Structure> _debugList = new List<Structure>();
    private Vector3[] _f4Points;

    private bool _inverse;

    public int Result
    {
        get { return _result; }
        set { _result = value; }
    }

    public TheRoute Route
    {
        get { return _theRoute; }
        set { _theRoute = value; }
    }

    public TheRoute TheRouteInBridge
    {
        get { return _theRouteInBridge; }
        set { _theRouteInBridge = value; }
    }

    public ReachBean() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="f4Points">The 4 points of a brdige to see orginze pls refer to the caller of constructir</param>
    public ReachBean(Vector3 from, Vector3[] f4Points, Person person, bool inverse)
    {
        _validFrom = from;
        _validToBot = f4Points[0];
        _validToTop = f4Points[1];
        _isReacheable = true;
        _keepResult = 1;
        _f4Points = f4Points;
        _person = person;
        _inverse = inverse;
        StartRoute();
    }

    public ReachBean(Vector3 from, Vector3[] f4Points, DeltaCapsule deltaCapsule, Person person, bool inverse)
    {
        _validFrom = from;
        _validToBot = f4Points[0];
        _validToTop = f4Points[1];
        _isReacheable = true;
        _keepResult = 1;
        _f4Points = f4Points;
        _person = person;
        _inverse = inverse;
        _deltaCapsule = deltaCapsule;
        ConformFinalRoute(_deltaCapsule.FinalRouter.TheRoute.CheckPoints);
    }

    private void ConformFinalRoute(List<CheckPoint> checkPoints)
    {
        _theRoute = new TheRoute(checkPoints);

        OrderBridgePoints();
        _theRouteInBridge =  RouteVector3s(_f4Points.ToList());

        //so the Bean gets reaad it and keeps going on the BrdigeRouter .. 
        //so no Async dealing to conform route on BrdigeRouter
        _result = _keepResult;
    }

    /// <summary>
    /// Will organizee brdige points by distance from last point on '_theRoute' here
    /// so it will address the instance where sometimes birge points come backward
    /// </summary>
    private void OrderBridgePoints()
    {
        var lastPointOnTerraRoute = _theRoute.CheckPoints[_theRoute.CheckPoints.Count - 1].Point;
        _f4Points = UOrder.ReturnOrderedByDistance(lastPointOnTerraRoute, _f4Points.ToList()).ToArray();
    }

    /// <summary>
    /// Will create the Route from a ordered list of Vector3s will add the angles too 
    /// </summary>
    public static TheRoute RouteVector3s(List<Vector3> Vector3s)
    {
        List<CheckPoint> list = new List<CheckPoint>();
        for (int i = 0; i < Vector3s.Count; i++)
        {
            list.Add(new CheckPoint(Vector3s[i]));
        }
        list = AddAnglesToRoute(list);
        return new TheRoute(list);
    }

    /// <summary>
    /// Will pass point by point and will find wht is the angle facing the next one
    /// </summary>
    public static List<CheckPoint> AddAnglesToRoute(List<CheckPoint> checkPoints)
    {
        General dumm = General.Create(Root.blueCubeBig, checkPoints[0].Point);
        for (int i = 0; i < checkPoints.Count - 1; i++)
        {
            dumm.transform.position = checkPoints[i].Point;
            //so it doesnt tilt when going up or down the brdige hill 
            //im putting in the same height on Y as the next point 
            dumm.transform.position = new Vector3(dumm.transform.position.x, checkPoints[i + 1].Point.y, dumm.transform.position.z);
            dumm.transform.LookAt(checkPoints[i + 1].Point);

            checkPoints[i].QuaterniRotation = dumm.transform.rotation;
        }
        dumm.Destroy();
        return checkPoints;
    }

    public void CleanBean()
    {
        _isReacheable = false;
        _result = -5;
        _validFrom = new Vector3();
        _validToBot = new Vector3();
        _deltaCapsule=new DeltaCapsule();
        _router = new Router();
        _theRoute = new TheRoute();
    }

    private void StartRoute()
    {
        CanIReach.InvertIfNeeded(ref _validFrom, ref _validToBot, _inverse);

        _router = new Router(_validFrom, _validToBot, _person);
    }

    public void Update()
    {
        //means is done 
        if (_result ==_keepResult)
        {
            return;
        }

        _router.Update();
        PullRouteOfRouter();
    }


    private void PullRouteOfRouter()
    {
        if (_router.IsRouteReady && _theRoute.CheckPoints.Count == 0)
        {
            ConformFinalRoute(_router.TheRoute.CheckPoints);
        }
    }
}
























///// <summary>
///// Will start the delta routing 
///// </summary>
//void StartDeltaRoute(Vector3 iniDelta, Vector3 finDelta)
//{
//    if (_isRoutable == -100)
//    {
//        _isRoutable = 0;//sets to still processing
//        _deltaCapsuleA = new DeltaCapsule(iniDelta, finDelta, _person);
//    }
//}

///// <summary>
///// This is a ASycn method bz If has water on the middle I will try to delta Route it
///// so if has water on the middle and cant delta route then returns -1 that means false...
///// 
///// If can see it directly or can delta routed will return 1
///// 
///// If is processing returns 0
///// </summary>
///// <returns>returns -1 that means false, 1 means true, 0 is still processing </returns>
//int CanIWalkToPos(Vector3 from, Vector3 to)
//{
//    //can i see it directly?
//    //if is water on middle will try to delta route 
//    if (RouterManager.IsWaterOrMountainBtw(from, to))
//    {
//        StartDeltaRoute(to, from);
//        return _isRoutable;
//    }
//    //otherwise is true... nothing is on midle so I can walk there 
//    return 1;
//}