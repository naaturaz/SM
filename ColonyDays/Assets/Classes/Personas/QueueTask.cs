using System.Collections.Generic;
using System;
using UnityEngine;

public class QueueTask : IComparable {
    
    List<QueueElement> _elements = new List<QueueElement>();

    public List<QueueElement> Elements
    {
        get { return _elements; }
        set { _elements = value; }
    }

    internal void AddToQueue(List<Vector3> objP, string key)
    {
        _elements.Add(new QueueElement(objP, key));
    }
    
    public bool Contains(Rect other, int index)
    {
        return IntersectMyRouteArea(other, index);
    }

    bool IntersectMyRouteArea(Rect other, int index)
    {
        //bool isLi3V = Elements is List<List<Vector3>>;
        //if (!isLi3V)
        //{
        //    throw new NotImplementedException();
        //    return false;
        //}

        List<Vector3> e = Elements[index].Poly;

        //means doesnt have Anchors set.. for ex Ways like road
        if (e.Count ==0){return false;}
        
        Rect a = U2D.ReturnRectYInverted(U2D.FromPolyToRect(e));

        if (a.Overlaps(other))
        { return true; }
        return false;
    }

    public int CompareTo(object obj)
    {
        throw new NotImplementedException();
    }

  
}

public class QueueElement
{
    public List<Vector3> Poly = new List<Vector3>();
    public DateTime DateTime1;//created to compare in Queues when A route is needed to be redone or not 

    private string _key;

    public QueueElement(List<Vector3> eList, string key)
    {
        _key = key;
        Poly = eList;
        DateTime1=DateTime.Now;
    }

    public QueueElement() { }

    public string Key
    {
        get { return _key; }
        set { _key = value; }
    }
}
