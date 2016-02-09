using System;
using UnityEngine;
using System.Collections.Generic;

public class CryBridgeRoute
{
    private Person _person;
    private VectorLand _one;
    private VectorLand _two;

    private BridgePsuedoPath _bridgePsuedoPath;


    private CryRoute _cryRoute1;
    private CryRoute _cryRoute2;
    private CryRoute _cryRoute3;

    List< CryRoute > _myRoutes = new List<CryRoute>(); 

    //legs to create Routes. They will be always, 1,2 sequenced
    List<VectorLand> _legs = new List<VectorLand>();
    private Structure _ini;
    private Structure _fin;

    private bool _isRouteReady;
    TheRoute _theRoute = new TheRoute();
    private string _destinyKey;
    private string _origenKey;

    public bool IsRouteReady
    {
        get { return _isRouteReady; }
        set { _isRouteReady = value; }
    }

    public TheRoute TheRoute
    {
        get { return _theRoute; }
        set { _theRoute = value; }
    }

    public CryBridgeRoute(Structure ini, Structure fin, Person person, string destinyKey)
    {
        _origenKey = ini.MyId;
        _destinyKey = destinyKey;
        _person = person;
        _one = ini.LandZone1[0];
        _two = fin.LandZone1[0];

        _ini = ini;
        _fin = fin;

        Init();
    }


    private void Init()
    {
        _bridgePsuedoPath = BuildingPot.Control.BridgeManager1.ReturnBestPath(_one, _two);

        if (_bridgePsuedoPath == null)
        {
            BlackList();
            return;
        }

        FindTheLegsPoints();
        InitIndividualRoutes();
    }

    /// <summary>
    /// Will define from _one to _two the points of the bridges in the botton icnluding _one to _two 
    /// </summary>
    private void FindTheLegsPoints()
    {
        var in1Bridge = FindPointInBridge(_one, H.Bottom);
        var in2Bridge = FindPointInBridge(_two, H.Bottom);
        in2Bridge.Reverse();//need to be reversed bz is ordered from the the second land point 

        for (int i = 0; i < in1Bridge.Count; i++)
        {
            CreateAndAddToLegs(in1Bridge[i], _bridgePsuedoPath.Bridges[0].BuildMyId);
        }

        if (_bridgePsuedoPath.Bridges.Count >1)
        {
            AddSecondBridgeLegs(in2Bridge);
        }

        var one = CreateAndAddToLegs(_one.Position, _ini.MyId, false);
        _legs.Insert(0, one);
        
        CreateAndAddToLegs(_two.Position, _fin.MyId);

        //Debug();
    }

    /// <summary>
    /// Add the second brdige legs
    /// </summary>
    /// <param name="list"></param>
    void AddSecondBridgeLegs(List<Vector3> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            //will not be added if is same tht upper. will happen if there is only1 brdige
            CreateAndAddToLegs(list[i], _bridgePsuedoPath.Bridges[1].BuildMyId);
        }
    }

    /// <summary>
    /// Will Create VectorLand with Building and will added to legs.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="bridgeId"></param>
    /// <param name="add">If is false will not added to legs</param>
    /// <returns>The new Vector Land</returns>
    VectorLand CreateAndAddToLegs(Vector3 pos, string bridgeId, bool add=true)
    {
        var bridge = Brain.GetBuildingFromKey(bridgeId);

        //todo
        if (bridge == null)
        {
            Debug.Log("Called with null brdige:" + bridgeId);
            throw new Exception("Fix");
        }

        VectorLand newVectorLand = new VectorLand();
        //if (bridge == null)
        //{
        //    newVectorLand = new VectorLand("", pos);
        //}
        //else 
            
        newVectorLand = new VectorLand("", pos, bridge);

        if (add)
        {
            _legs = AddIfsNotContain(_legs, newVectorLand);
        }

        return newVectorLand;
    }

    void DebugLoc()
    {
        //UVisHelp.CreateHelpers(_legs, Root.yellowCube);
        for (int i = 0; i < _legs.Count; i++)
        {
            UVisHelp.CreateText(_legs[i].Position, i + "");
        }
    }

    List<VectorLand> AddIfsNotContain(List<VectorLand> lis, VectorLand newV)
    {
        if (!lis.Contains(newV))
        {
            lis.Add(newV);
        }
        return lis;
    }

    /// <summary>
    /// Will find the point in the bridge from that zone. ordered from that zone point on brdige 
    /// </summary>
    /// <param name="from"></param>
    /// <param name="which"></param>
    /// <returns></returns>
    List<Vector3> FindPointInBridge(VectorLand from, H which)
    {
        var bridge = FindBridgeOnVectorLand(from);

        if (which == H.Bottom)
        {
            return bridge.ReturnBottonsBasedOnVectorLand(from);
        }
        else
        {
            return bridge.ReturnTopsBasedOnVectorLand(from);
        }
    }

    /// <summary>
    /// Will find the bridge tht is on that LandZone
    /// </summary>
    /// <param name="land"></param>
    /// <returns></returns>
    Bridge FindBridgeOnVectorLand(VectorLand land)
    {
        for (int i = 0; i < _bridgePsuedoPath.Bridges.Count; i++)
        {
            var link = _bridgePsuedoPath.Bridges[i];
            //if has 1 with the same leg. then this is the brdige im looking for 
            if (link.Has1OfSameType(land.LandZone))
            {
                var key = _bridgePsuedoPath.Bridges[i].BuildMyId;
                return FromKeyToBridge(key);
            }
        }
        return null;
    }

    public static Bridge FromKeyToBridge(string key)
    {
        return (Bridge) Brain.GetBuildingFromKey(key);
    }

    /// <summary>
    /// Will init the individual routes in the ground
    /// </summary>
    private void InitIndividualRoutes()
    {
        for (int i = 0; i < _legs.Count; i+=2)
        {
            VectorLand uno = _legs[i];
            VectorLand dos = _legs[i+ 1];
            
            _myRoutes.Add(new CryRoute(uno, dos, _person));
        }
    }

    /// <summary>
    /// Bz cant reach point two
    /// </summary>
    private void BlackList()
    {
        _person.Brain.BlackListBuild(ExtractRealId(_fin));
    }

    /// <summary>
    /// Used to detect if the _fin object is a dummy
    /// </summary>
    /// <returns></returns>
    public static string ExtractRealId(Structure fin)
    {
        if (fin.MyId.Contains("Dummy"))
        {
            var temp = fin.DummyIdSpawner;
            //fin.DummyIdSpawner = "";
            return temp;
        }
        return fin.MyId;
    }


    public void Update()
    {
        if (_myRoutes.Count==0)
        {
            return;
        }

        _myRoutes[0].Update();

        if (_myRoutes.Count > 1)
        {
            _myRoutes[1].Update();
        }
        if (_myRoutes.Count > 2)
        {
            _myRoutes[2].Update();
        }

        CheckIfAllDone();
    }

    /// <summary>
    /// Will tell u if routes are done 
    /// </summary>
    private void CheckIfAllDone()
    {
        var one = _myRoutes[0].IsRouteReady;

        //if (_myRoutes.Count == 1)
        //{
        //    if (one)
        //    {
        //        ConformRoute();
        //    }
        //}    
  
        if (_myRoutes.Count == 2)
        {
            if (one && _myRoutes[1].IsRouteReady)
            {
                ConformRoute();
            }

        }
        if (_myRoutes.Count ==3)
        {
            if (one && _myRoutes[1].IsRouteReady && _myRoutes[2].IsRouteReady)
            {
                ConformRoute();
            }
        }
    }


    void ConformRoute()
    {
        TheRoute.CheckPoints.AddRange(_myRoutes[0].TheRoute.CheckPoints);
        //get the tops of the brdige in same land zone as _one
        var topsBrid1 = FindPointInBridge(_one, H.Top);

        CorrectLastPointRotation(TheRoute, topsBrid1[0]);

        FirstBridge(topsBrid1, _myRoutes[1].TheRoute.CheckPoints[0].Point);
        TheRoute.CheckPoints.AddRange(_myRoutes[1].TheRoute.CheckPoints);

        //if has two bridges 
        if (_legs.Count == 6)
        {
            var topsBrid2 = FindPointInBridge(_two, H.Top);
            topsBrid2.Reverse();

            //CorrectLastPointRotation(TheRoute, topsBrid2[0]);

            FirstBridge(topsBrid2, _myRoutes[2].TheRoute.CheckPoints[0].Point);
            TheRoute.CheckPoints.AddRange(_myRoutes[2].TheRoute.CheckPoints);
        }

        TheRoute.OriginKey = _origenKey;
        TheRoute.DestinyKey = _destinyKey;
        IsRouteReady = true;
    }

    /// <summary>
    /// Will correct the last poinst of the route rotation
    /// 
    /// Needed bz the last point of a route doesnt have any rotation since we didnt know wht was next
    /// here I know wht is next so it can be added 
    /// </summary>
    /// <param name="cryRoute"></param>
    /// <param name="vector3"></param>
    private void CorrectLastPointRotation(TheRoute theRoute, Vector3 next)
    {
        var temp = theRoute.CheckPoints[theRoute.CheckPoints.Count - 1];
        var newCheck = ReturnFacingTo(temp.Point, next, temp.Point);

        //removes the last
        theRoute.CheckPoints.RemoveAt(theRoute.CheckPoints.Count - 1);
        //adds the new one 
        theRoute.CheckPoints.Add(newCheck);
    }






    /// <summary>
    /// Will Conform a new leg in the routing of bridges 
    /// </summary>
    /// <param name="brid">The points of top of the bridge </param>
    /// <param name="pointAfterBridge">The  exit point in the botton on the  bridge </param>
    void FirstBridge(List<Vector3> brid, Vector3 pointAfterBridge)
    {
        var brid1FirstPoint = ReturnFacingTo(brid[0], brid[1], brid[0]);
        var brid1SecPoint = ReturnFacingTo(brid[1], pointAfterBridge, brid[1]);

        TheRoute.CheckPoints.Add(brid1FirstPoint);
        TheRoute.CheckPoints.Add(brid1SecPoint);
    }

    /// <summary>
    /// Creates a CheckPoint
    /// </summary>
    /// <param name="from">Where will stan to look at from</param>
    /// <param name="facingTo">Point that will look at</param>
    /// <param name="iniPos">The Point of the CHeckPoint, which most of the time is the same as 'from'</param>
    /// <returns></returns>
    CheckPoint ReturnFacingTo(Vector3 from, Vector3 facingTo, Vector3 iniPos)
    {
        CheckPoint re = new CheckPoint(iniPos);
        GameScene.dummyBlue.transform.position = from;

        //so it doesnt tilt when going up or down the brdige hill 
        //im putting in the same height on Y as the next point 
        GameScene.dummyBlue.transform.position = new Vector3(GameScene.dummyBlue.transform.position.x, facingTo.y, GameScene.dummyBlue.transform.position.z);
        GameScene.dummyBlue.transform.LookAt(facingTo);

        re.QuaterniRotation = GameScene.dummyBlue.transform.rotation;

        GameScene.dummyBlue.transform.position = new Vector3();
        return re;
    }
}
