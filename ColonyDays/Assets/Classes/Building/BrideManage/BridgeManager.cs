using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BridgeManager 
{
    SMe m = new SMe();
  
    //types of LandZoneLinkers they have the 2 ends equalls, disreagardin the order of them 
    List<LandZoneLinker> _types = new List<LandZoneLinker>(); 
    
    List<Connection> _connections = new List<Connection>();
    private bool load;


    public BridgeManager()
    {
        load = true;
    }

    public void Update()
    {
        if (load)
        {
            CreateTypes(); 
        }
    }


    /// <summary>
    /// Depending on the amount of landZones on this Terrain will create the types:
    /// 
    /// One type is from Shore A to Shore B, the next one :
    /// from Shore B to C. then
    /// from Shore C to A,
    /// 
    /// in a 3 LandZone Terrains those are all the combinations posible 
    /// </summary>
    private void CreateTypes()
    {
        if (m.MeshController.LandZoneManager1.LandZones.Count == 0)
        {
            return;
        }
        load = false;

        var landZones = m.MeshController.LandZoneManager1.LandZones;
        //2 zones
        if (landZones.Count == 2)
        {
            _types.Add(new LandZoneLinker(landZones[0].LandZoneName, landZones[1].LandZoneName));
        }
        //3 zones
        else if (landZones.Count == 3)
        {
            _types.Add(new LandZoneLinker(landZones[0].LandZoneName, landZones[1].LandZoneName));
            _types.Add(new LandZoneLinker(landZones[1].LandZoneName, landZones[2].LandZoneName));
            _types.Add(new LandZoneLinker(landZones[2].LandZoneName, landZones[0].LandZoneName));
        }
        else if (landZones.Count > 3)
        {
            throw new Exception("Game only support a Max of 3 LandZones ");
        }
    }












    /// <summary>
    /// The action of adding a new Bridge when is fully built
    /// </summary>
    /// <param name="zone1"></param>
    /// <param name="zone2"></param>
    /// <param name="b"></param>
    public void AddBridge(string zone1, string zone2, Bridge b)
    {
        LandZoneLinker land = new LandZoneLinker(zone1, zone2, b.transform.position, b.MyId);

        CreateNewConnection(zone1, zone2, land);
    }

    /// <summary>
    /// This will create the new conenction of the new Bridge and will evaluate if are new connections
    /// </summary>
    /// <param name="zone1"></param>
    /// <param name="zone2"></param>
    private void CreateNewConnection(string zone1, string zone2, LandZoneLinker link)
    {
        Connection connection = new Connection(zone1, zone2, link);
        connection = ReturnOrderedConn(connection);

        _connections.Add(connection);

        ThereAreMoreNewConnections(connection);
    }

    /// <summary>
    /// Will try to see if new connections can be done with the new Bridge 
    /// </summary>
    /// <param name="connection"></param>
    private void ThereAreMoreNewConnections(Connection connection)
    {
        var list = ReturnAllConnectionThatAreDiffTypeAndShareAtLeastOneZone(connection);

        for (int i = 0; i < list.Count; i++)
        {
            CreateOneConnectionFromTwo(list[i], connection);
        }
    }

    void CreateOneConnectionFromTwo(Connection one, Connection two)
    {
        var list = FindUncommonZones(one, two);

        if (list.Count > 1)
        {
            Connection c = new Connection(list[0], list[1]);
            c = ReturnOrderedConn(c);

            c.ThruBridge.AddRange(one.ThruBridge);
            c.ThruBridge.AddRange(two.ThruBridge);

            _connections.Add(c);
        }
    }

    /// <summary>
    /// Will return the two zone that are uncommom here 
    /// </summary>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <returns></returns>
    List<string> FindUncommonZones(Connection one, Connection two)
    {
        List<string>res = new List<string>();

        res = AddToListAndRemoveIfDuplicate(res, one.Zone1);
        res = AddToListAndRemoveIfDuplicate(res, one.Zone2);
        res = AddToListAndRemoveIfDuplicate(res, two.Zone1);
        res = AddToListAndRemoveIfDuplicate(res, two.Zone2);

        return res;
    }

    private List<string> AddToListAndRemoveIfDuplicate(List<string>list,string p)
    {
        if (list.Contains(p))
        {
            list.Remove(p);
        }
        else list.Add(p);

        return list;
    }



    /// <summary>
    /// Will return all connections ttht do not have the same ends but and share only 1 leg zone 
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    List<Connection> ReturnAllConnectionThatAreDiffTypeAndShareAtLeastOneZone(Connection connection)
    {
        List<Connection> res = new List<Connection>();

        for (int i = 0; i < _connections.Count; i++)
        {
            if (_connections[i].ShareOnlyOneZone(connection))
            {
                res.Add(_connections[i]);
            }
        }
        return res;
    }










    /// <summary>
    /// Will clasifi it depending on the zones it has and will added to the list tht contains same 
    /// type of elements bz they all have the same 2 ends 
    /// </summary>
    /// <param name="land"></param>
    int Classify(LandZoneLinker land)
    {
        for (int i = 0; i < _types.Count; i++)
        {
            if (_types[i].IsSameType(land))
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// This will return the Type of Connection in the right order correspoding
    /// to the connections we have on file
    /// </summary>
    /// <param name="conn"></param>
    /// <returns></returns>
    Connection ReturnOrderedConn(Connection conn)
    {
        for (int i = 0; i < _types.Count; i++)
        {
            if (_types[i].IsSameType(conn))
            {
                return conn;
            }
        }
        return null;
    }




    /// <summary>
    /// Will return the closest combitnation of bridges to reach from pos1 to pos2
    /// 
    /// Before the method will evalute all the posibilities
    /// </summary>
    /// <returns></returns>
    public BridgePsuedoPath ReturnBestPath(VectorLand one, VectorLand two)
    {
        var conn = new Connection(one.LandZone, two.LandZone);
        conn = ReturnOrderedConn(conn);

        var sameEndsLis = FindAllConnWithSameEnds(conn);

        return ShorterPath(sameEndsLis, one, two);
    }

    /// <summary>
    /// Will create path for each connection and will return the shorterst
    /// </summary>
    /// <param name="sameEndsLis"></param>
    /// <returns></returns>
    private BridgePsuedoPath ShorterPath(List<Connection> sameEndsLis, VectorLand one, VectorLand two)
    {
        List<BridgePsuedoPath> list = new List<BridgePsuedoPath>();

        for (int i = 0; i < sameEndsLis.Count; i++)
        {
            var conn = sameEndsLis[i];

            list.Add(new BridgePsuedoPath(conn.ThruBridge, one, two));
        }

        list = list.OrderBy(a => a.Distance).ToList();

        if (list.Count == 0)
        {
            return null;
        }

        return list[0];
    }







    /// <summary>
    /// Will find all connections with same end as the param
    /// </summary>
    /// <param name="connection"></param>
    /// <returns></returns>
    List<Connection> FindAllConnWithSameEnds(Connection connection)
    {
        List<Connection> res = new List<Connection>();

        for (int i = 0; i < _connections.Count; i++)
        {
            if (_connections[i].HasSameZones(connection))
            {
                res.Add(_connections[i]);
            }
        }
        return res;
    }






}


/// <summary>
/// Everytime a new bridge is built will open up new connection, (s)
/// </summary>
public class Connection
{
    private string _zone1;
    private string _zone2;
    private List<LandZoneLinker> _thruBridge = new List<LandZoneLinker>();
    private string p1;
    private string p2; 

    public string Zone1
    {
        get { return _zone1; }
        set { _zone1 = value; }
    }

    public string Zone2
    {
        get { return _zone2; }
        set { _zone2 = value; }
    }

    public List<LandZoneLinker> ThruBridge
    {
        get { return _thruBridge; }
        set { _thruBridge = value; }
    }


    public Connection(string zone1, string zone2, LandZoneLinker bridge)
    {
        _zone1 = zone1;
        _zone2 = zone2;
        _thruBridge.Add(bridge);
    }

    public Connection(string zone1, string zone2)
    {
        _zone1 = zone1;
        _zone2 = zone2;
    }

    public bool HasSameZones(Connection connection)
    {
        int same = 0;
        if (_zone1 == connection.Zone1 || _zone1 == connection.Zone2)
        {
            same++;
        }
        if (_zone2 == connection.Zone1 || _zone2 == connection.Zone2)
        {
            same++;
        }

        if (same == 2)
        {
            return true;
        }
        return false;
    }

    bool ShareOneZone(Connection connection)
    {
        int same = 0;
        if (_zone1 == connection.Zone1 || _zone1 == connection.Zone2)
        {
            same++;
        }
        if (_zone2 == connection.Zone1 || _zone2 == connection.Zone2)
        {
            same++;
        }

        if (same == 1)
        {
            return true;
        }
        return false;
    }

    public bool ShareOnlyOneZone(Connection connection)
    {
        return !HasSameZones(connection) && ShareOneZone(connection);
    }
}


/// <summary>
/// Use to keep the Bridge combination 
/// </summary>
public class BridgePsuedoPath
{
    List<LandZoneLinker> _bridges = new List<LandZoneLinker>();
    private float _distance;



    public float Distance
    {
        get { return _distance; }
        set { _distance = value; }
    }

    public List<LandZoneLinker> Bridges
    {
        get { return _bridges; }
        set { _bridges = value; }
    }

    public BridgePsuedoPath(List<LandZoneLinker> bridges, VectorLand pos1, VectorLand pos2)
    {
        _bridges = bridges;

        //more thn 1 bridge
        if (bridges.Count > 1)
        {
            Distance = CalcDistance(pos1, pos2);
        }
        else if (bridges.Count == 1)
        {
            Distance = CalcDistance(pos1, pos2, bridges[0].Pos);
        }
    }


    /// <summary>
    /// If is only one bridge 
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <param name="theBridgePos"></param>
    /// <returns></returns>
    private float CalcDistance(VectorLand pos1, VectorLand pos2, Vector3 theBridgePos)
    {
        return Vector3.Distance(pos1.Position, theBridgePos) + Vector3.Distance(pos2.Position, theBridgePos);
    }




    private float CalcDistance(VectorLand pos1, VectorLand pos2)
    {
        List<Vector3> brPositions = new List<Vector3>();

        var firstPos = ReturnInZonePos(pos1, pos2, Bridges[0]);
        var secPos = ReturnInZonePos(pos1, pos2, Bridges[Bridges.Count - 1]);

        float dist = 0;
        for (int i = 0; i < Bridges.Count; i++)
        {
            var pos = Bridges[i].Pos;
            brPositions.Add(pos);

            if (i > 0)
            {
                var prevPos = Bridges[i - 1].Pos;

                dist += Vector3.Distance(prevPos, pos);
            }
        }

        //distance from 1st pos to first bridge
        dist += Vector3.Distance(firstPos, brPositions[0]);

        //distance from pos2 to last brdige 
        dist += Vector3.Distance(secPos, brPositions[brPositions.Count - 1]);

        return dist;
    }


    /// <summary>
    /// Because when is more tht 1 brdige I need to know wich VectorLand is the one in land with the bridge
    /// so Distance is calculated properly
    /// </summary>
    /// <param name="pos1"></param>
    /// <param name="pos2"></param>
    /// <param name="bridge"></param>
    /// <returns></returns>
    Vector3 ReturnInZonePos(VectorLand pos1, VectorLand pos2, LandZoneLinker bridge)
    {
        List<string > temp = new List<string>(){pos1.LandZone, pos2.LandZone, bridge.Zone1, bridge.Zone2};
        var common = UString.ReturnMostCommonName(temp);

        //if the common is the one is pos1.LandZone
        if (pos1.LandZone == common)
        {
            return pos1.Position;
        }
        return pos2.Position;
    }









}

/// <summary>
/// So a Position asked for and a LandZone are kept tpghetejhr this is important to calculate distance 
/// properluy
/// </summary>
public class VectorLand
{
    public string LandZone;
    public Vector3 Position;
    public Building MyBuild;//use to legs in the CryBridgeRoute. Bz I need to know tht for CryRoute




    public VectorLand(string landzon, Vector3 pos)
    {
        LandZone = landzon;
        Position = pos;
    }  
    
    public VectorLand(string landzon, Vector3 pos, Building st)
    {
        LandZone = landzon;
        Position = pos;
        MyBuild = st;
    }
}