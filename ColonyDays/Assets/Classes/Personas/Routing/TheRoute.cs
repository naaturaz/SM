using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//This class is for keep all the info of the route
public class TheRoute
{
    private List<CheckPoint> _checkPoints = new List<CheckPoint>();

    //the square area the route cover ... all point contained in a rectangle..
    //the rectangles is defined by the maximun and minimun values of the rectangles
    //is defined using Registro.FromALotOfVertexToRect()
    private Rect _areaRect;

    private bool _passThruWater;//says if went trhu a water collider
    private string _bridgeKey;//Holds the key of a brdige if one was used to conform the route

    private string _originKey;//the key of the buildinng is the destiny is this route
    private string _destinyKey;//the key of the buildinng is the destiny is this route
    private DateTime _dateTime;//created to compare in Queues when A route is needed to be redone or not

    //created to avoid CLearing the checkpoints of a good route.
    //now will be marked as need new one so it can give a new one
    private H _instruction = H.None;

    public List<CheckPoint> CheckPoints
    {
        get { return _checkPoints; }
        set { _checkPoints = value; }
    }

    public Rect AreaRect
    {
        get { return _areaRect; }
        set { _areaRect = value; }
    }

    public bool PassThruWater
    {
        get { return _passThruWater; }
        set { _passThruWater = value; }
    }

    /// <summary>
    /// IF the route uses a river here is the Key
    /// </summary>
    public string BridgeKey
    {
        get { return _bridgeKey; }
        set { _bridgeKey = value; }
    }

    /// <summary>
    /// The destination Key Build of a Route
    /// </summary>
    public string DestinyKey
    {
        get { return _destinyKey; }
        set { _destinyKey = value; }
    }

    public DateTime DateTime1
    {
        get { return _dateTime; }
        set { _dateTime = value; }
    }

    public string OriginKey
    {
        get { return _originKey; }
        set { _originKey = value; }
    }

    public H Instruction
    {
        get { return _instruction; }
        set { _instruction = value; }
    }

    public TheRoute()
    {
    }

    public TheRoute(List<CheckPoint> checkPoints, string originKey = "", string destinyKey = "")
    {
        _destinyKey = destinyKey;
        _originKey = originKey;

        CheckPoints = checkPoints;
        DefineAreaRect();
        _dateTime = DateTime.Now;
    }

    public TheRoute(List<CheckPoint> checkPoints, string originKey, string bridgeKey, string destinyKey)
    {
        _destinyKey = destinyKey;
        _originKey = originKey;

        _bridgeKey = bridgeKey;
        CheckPoints = checkPoints;
        DefineAreaRect();
        _dateTime = DateTime.Now;
    }

    public TheRoute(TheRoute theRoute)
    {
        _destinyKey = theRoute.DestinyKey;
        _originKey = theRoute.OriginKey;
        _bridgeKey = theRoute.BridgeKey;

        CheckPoint[] array = new CheckPoint[theRoute.CheckPoints.Count];
        theRoute.CheckPoints.CopyTo(array);
        _checkPoints = array.ToList();

        _areaRect = theRoute.AreaRect;
        _dateTime = theRoute.DateTime1;
    }

    //Defines the Area Square of a Route
    private void DefineAreaRect()
    {
        List<Vector3> l = new List<Vector3>();
        for (int i = 0; i < CheckPoints.Count; i++)
        {
            l.Add(CheckPoints[i].Point);
        }
        _areaRect = Registro.FromALotOfVertexToRect(l);
    }

    //Says if param 'other' intersects the route area
    public bool IntersectMyRouteArea(Rect other)
    {
        if (AreaRect.Overlaps(other))
        { return true; }
        return false;
    }

    /// <summary>
    /// Will add a list of new Vector3 to be added as checkpoints
    /// </summary>
    /// <param name="brid1"></param>
    internal static void AddVector3s(List<Vector3> brid1)
    {
        List<CheckPoint> checks = new List<CheckPoint>();
        for (int i = 0; i < brid1.Count; i++)
        {
            checks.Add(new CheckPoint(brid1[i]));
        }
    }
}