using UnityEngine;
using System.Collections.Generic;

/* Encapsulates DeltaRouter and do the logic to find the delta route
 * and do the recursion too
 * 
 */

public class DeltaCapsule
{
    private DeltaRouter _deltaRouter = new DeltaRouter();
    private Router _finalRouter = new Router();
    private Vector3 _iniPos;
    private Vector3 _finPos;
    private Person _person;
    private Structure _ini;
    private Structure _fin;
    private Vector3 _iniBehindDoor;
    private Vector3 _finBehindDoor;

    List<Structure> _debugList = new List<Structure>();
    private bool _deltaRoutingDone;//says when the delta routing is done 

    private Router _router1 = new Router();
    private Router _router2 = new Router();
    private Router _router3 = new Router();
    private bool deltaRouted;

    public DeltaCapsule() { }

    /// <summary>
    /// Used to do from Structure A to B
    /// </summary>
    /// <param name="ini"></param>
    /// <param name="fin"></param>
    /// <param name="person"></param>
    public DeltaCapsule(Structure ini, Structure fin, Person person)
    {
        _ini = ini;
        _fin = fin;
        _iniPos = _ini.SpawnPoint.transform.position;
        _iniBehindDoor = _ini.BehindMainDoorPoint;
        _finPos = _fin.transform.position;

        _person = person;

        Init();
    }

    public DeltaCapsule(Vector3 ini, Vector3 fin, Person person,
        Vector3 iniBehind = new Vector3(), Vector3 finBehind = new Vector3())
    {
        _iniPos = ini;
        _finPos = fin;

        _iniBehindDoor = iniBehind;
        _finBehindDoor = finBehind;

        _person = person;
        Init();
    }

    public bool DeltaRoutingDone
    {
        get { return _deltaRoutingDone; }
        set { _deltaRoutingDone = value; }
    }

    public Router FinalRouter
    {
        get { return _finalRouter; }
        set { _finalRouter = value; }
    }

    private void Init()
    {
       //GameScene.print("Init()  DeltaRouting ");
        _finalRouter = new Router();
        _deltaRouter = new DeltaRouter(_iniPos, _finPos, _person);
    }

    /// <summary>
    /// Creates the routes btw the 4 points of the Delta Route
    /// </summary>
    void DeltaRoute()
    {
        Structure dummy = (Structure)Building.CreateBuild(Root.dummyBuildWithSpawnPoint, new Vector3(), H.Dummy);
        dummy.transform.position = _deltaRouter.DeltaRoute.Points[1];

        _router1 = new Router(_iniPos, dummy.SpawnPoint.transform.position, _person, _iniBehindDoor);

        Structure dummy2 = (Structure)Building.CreateBuild(Root.dummyBuildWithSpawnPoint, new Vector3(), H.Dummy);
        dummy2.transform.position = _deltaRouter.DeltaRoute.Points[2];
        _router2 = new Router(dummy.SpawnPoint.transform.position, dummy2.SpawnPoint.transform.position, _person);

        Vector3 lastPos = DefineFinalPoint();
        Vector3 lastPosBehingDoor = DefineFinalPointBehingDoor();
        _router3 = new Router(dummy2.SpawnPoint.transform.position, lastPos, _person, new Vector3(), lastPosBehingDoor);

        deltaRouted = true;
        _debugList.Add(dummy);
        _debugList.Add(dummy2);
    }

    /// <summary>
    /// Created to address when the last point of a route is passed with Structure or not 
    /// 
    /// If is strucutre we use :_fin.SpawnPoint.transform.position
    /// other wise _finPos
    /// </summary>
    /// <returns></returns>
    Vector3 DefineFinalPoint()
    {
        if (_fin != null && _fin.SpawnPoint != null)
        {
            return _fin.SpawnPoint.transform.position;
        }
        return _finPos;
    }

    /// <summary>
    /// To address when the _fin structure is null 
    /// </summary>
    /// <returns></returns>
    Vector3 DefineFinalPointBehingDoor()
    {
        if (_fin != null && _fin.BehindMainDoorPoint != null)
        {
            return _fin.BehindMainDoorPoint;
        }
        else if (_finBehindDoor != new Vector3())
        {
            return _finBehindDoor;
        }
        return new Vector3();
    }

    private List<CheckPoint> bucketCheckPoints = new List<CheckPoint>();//checks points to be validated 
    List<CheckPoint> validCheckPoints = new List<CheckPoint>();//checkpoint validated they will be used for last result
    private void DeltaQualityCheck()
    {
        for (int i = 0; i < bucketCheckPoints.Count - 1; i++)
        {
            //if is not the same 
            if (!UMath.nearEqualByDistance(bucketCheckPoints[i].Point, bucketCheckPoints[i + 1].Point, 0.1f))
            {
                if (!RouterManager.IsWaterOrMountainBtw(bucketCheckPoints[i].Point, bucketCheckPoints[i + 1].Point))
                {
                    validCheckPoints.Add(bucketCheckPoints[i]);
                }
                //if is in water wont add the current one
                else
                {
                    RestartDeltaRouter(bucketCheckPoints[i].Point);
                    Init();
                    deltaRouted = false;
                    return;
                }
            }
        }
        //add the last one 
        validCheckPoints.Add(bucketCheckPoints[bucketCheckPoints.Count - 1]);
        DoneDelta();
    }

    private void DoneDelta()
    {
       //GameScene.print("Done Delta Routing ");
        DestroyDebuger();

        var t = CreateTheRouteObj();

        _finalRouter.TheRoute = t;
        _finalRouter.IsRouteReady = true;
        _deltaRoutingDone = true;
    }

    /// <summary>
    /// Created for modularity 
    /// </summary>
    TheRoute CreateTheRouteObj()
    {
        var ori = "";
        var dest = "";

        if (_ini!=null)
        {
            ori = _ini.MyId;
        }
        if (_fin!=null)
        {
            dest = _fin.MyId;
        }

        return new TheRoute(validCheckPoints, ori, dest);
    }

    void InitDeltaQualityCheck()
    {
        bucketCheckPoints.AddRange(_router1.TheRoute.CheckPoints);
        bucketCheckPoints.AddRange(_router2.TheRoute.CheckPoints);
        bucketCheckPoints.AddRange(_router3.TheRoute.CheckPoints);
    }

    void RestartDeltaRouter(Vector3 newIni = new Vector3())
    {
        bucketCheckPoints.Clear();
       //GameScene.print("One Recursion on Delta Routimng");

        //will change _iniPos value if newIni was specified
        if (newIni != new Vector3())
        {
            _iniPos = newIni;
        }

        _router1 = new Router();
        _router2 = new Router();
        _router3 = new Router();
        _deltaRouter = null;
        _deltaRouter = new DeltaRouter();

        //since was already used for the initial structure for recursion is set to Default value so is not consider
        //for router.cs
        _iniBehindDoor = new Vector3();

        DestroyDebuger();
    }

    void DestroyDebuger()
    {
        for (int i = 0; i < _debugList.Count; i++)
        {
            _debugList[i].Destroy();
        }
        _debugList.Clear();
    }

    public void Update()
    {
        if (DeltaRoutingDone) { return; }

        _finalRouter.Update();
        _deltaRouter.Update();
        _router1.Update();
        _router2.Update();
        _router3.Update();

        if (!deltaRouted && _deltaRouter.DeltaRoute.Points.Count == 4)
        {
            DeltaRoute();
        }
        //////////////////
        else if (!deltaRouted && _deltaRouter.DeltaRoute.Points.Count == 0 && !_deltaRouter.IsDeltaRoutable())
        {
            DeltaRoutingDone = true;
           //GameScene.print("delta Routing Done for Not routable ");
        }

        if (_router1.IsRouteReady && _router2.IsRouteReady && _router3.IsRouteReady
            && deltaRouted && bucketCheckPoints.Count == 0)
        {
            InitDeltaQualityCheck();
            DeltaQualityCheck();
        }
    }
}
