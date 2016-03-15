using System;
using System.Collections.Generic;
using UnityEngine;

public class RoutesCache {

    Dictionary<string, TheRoute> _items = new Dictionary<string, TheRoute>();
    TheRoute _current = new TheRoute();//current route we are comparing to

    public TheRoute Current
    {
        get { return _current; }
        set { _current = value; }
    }


    /// <summary>
    /// Will tell if the cachec contians a newer route that actually is newer thatn the ask so it can be used no proble 
    /// </summary>
    /// <param name="OriginKey"></param>
    /// <param name="DestinyKey"></param>
    /// <param name="askDateTime"></param>
    /// <returns></returns>
    public bool ContainANewerOrSameRoute(string OriginKey, string DestinyKey, DateTime askDateTime)
    {

        //return false;

        var haveIt = DoWeHaveThatRoute(OriginKey, DestinyKey);

        if (!haveIt)
        {return false;}

        return IsNewerOrSame(askDateTime);
    }

    /// <summary>
    /// Says if _current is newer tatn 'askDateTime'
    /// </summary>
    /// <param name="askDateTime"></param>
    /// <returns></returns>
    bool IsNewerOrSame(DateTime askDateTime)
    {
        DateTime date1 = _current.DateTime1;
        DateTime date2 = askDateTime;

        int result = DateTime.Compare(date1, date2);

        // _current.DateTime1 is after 
        if (result > 0 || result == 0 )
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Says if we have a route with same Oring and Destiyny key
    /// 
    /// sets '_current'
    /// </summary>
    /// <param name="theRoute"></param>
    /// <returns></returns>
    bool DoWeHaveThatRoute(string OriginKey, string DestinyKey)
    {
        var key = CreateRouteKey(OriginKey, DestinyKey);

        if (_items.ContainsKey(key))
        {
            if (_items[key].CheckPoints.Count >0)
            {
                //only if has more than 0 bz they can reference clear the routes 
                _current = new TheRoute(_items[key]);
                return true;
            }
            _items.Remove(key);
        }
        return false;
    }

    /// <summary>
    /// Creates a Stardard key for a route
    /// OriginKey + "." + DestinyKey;
    /// </summary>
    /// <param name="OriginKey"></param>
    /// <param name="DestinyKey"></param>
    /// <returns></returns>
    public static string CreateRouteKey(string OriginKey, string DestinyKey)
    {
        return OriginKey + "." + DestinyKey;
    }

    /// <summary>
    /// Will reutn _current the newer route found when asked 'bool ContainANewerRoute()'
    /// </summary>
    /// <returns></returns>
    public TheRoute GiveMeTheNewerRoute()
    {
        //so replace if had any instruction. If is current and asked is bz is proper route
        _current.Instruction = H.None;
        return _current;
    }

    public void AddReplaceRoute(TheRoute theRoute)
    {
        if (theRoute.CheckPoints.Count==0)
        {
            return;
        }

        var key = theRoute.OriginKey + "." + theRoute.DestinyKey;
        var haveIt = DoWeHaveThatRoute(theRoute.OriginKey, theRoute.DestinyKey);

        if (haveIt)
        {
            if (theRoute.DateTime1 == _current.DateTime1)
            {
                //we have the latest one already
            }
            else
            {
                _items[key]=theRoute;
            }
        }
        else
        {
            _items.Add(key, theRoute);
        }
    }

    /// <summary>
    /// Bz a route that was collided for Profession needs to be removed from here 
    /// </summary>
    /// <param name="theRoute"></param>
    public void RemoveRoute(TheRoute theRoute, DateTime askTime)
    {
        var key = ReturnKey(theRoute);
        if (!_items.ContainsKey(key))
        {
            return;
        }
        
        var item = _items[key];
        if (item==null)
        {
            return;
        }

        DateTime date1 = item.DateTime1;
        DateTime date2 = askTime;
        int result = DateTime.Compare(date1, date2);
        //if the Current ITem date is bigger that askTime means we have the latest verstion of it
        if (result > 0)
        {
            return;
        }
        
        var was = _items.Remove(key);
        Debug.Log("was remove:"+was+"."+key);
    }

    string ReturnKey(TheRoute theRoute)
    {
        
        return theRoute.OriginKey + "." + theRoute.DestinyKey;
    }

    public void Update()
    {
        
    }

    /// <summary>
    /// Bz routes will stay there forever. really old and not useful 
    /// </summary>
    void CheckIfARouteIsTooOld()
    {
        
    }
}
