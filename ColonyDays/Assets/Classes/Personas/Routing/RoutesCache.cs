using System;
using System.Collections.Generic;

public class RoutesCache {

    List<TheRoute> _items = new List<TheRoute>();
    TheRoute _current = new TheRoute();//current route we are comparing to

    public List<TheRoute> Items
    {
        get { return _items; }
        set { _items = value; }
    }

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
    public bool ContainANewerRoute(string OriginKey, string DestinyKey, DateTime askDateTime)
    {
        var haveIt = DoWeHaveThatRoute(OriginKey, DestinyKey);

        if (!haveIt 
            //|| askDateTime == new DateTime()
            )
        {return false;}

        return IsNewer(askDateTime);
    }

    /// <summary>
    /// Says if _current is newer tatn 'askDateTime'
    /// </summary>
    /// <param name="askDateTime"></param>
    /// <returns></returns>
    bool IsNewer(DateTime askDateTime)
    {
        DateTime date1 = _current.DateTime1;
        DateTime date2 = askDateTime;

        int result = DateTime.Compare(date1, date2);

        // _current.DateTime1 is after 
        if (result > 0 )
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
        for (int i = 0; i < _items.Count; i++)
        {
            var sameOri = _items[i].OriginKey == OriginKey;
            var sameDest = _items[i].DestinyKey == DestinyKey;

            if (sameOri && sameDest)
            {
                _current = new TheRoute(_items[i]);
                return true;
            }
        }
        return false;
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
        //if inverse was not set yet not has to be considered
        if (!IsInverseSet(theRoute))
        {
            return;
        }

        //just to set _current
        var haveIt = DoWeHaveThatRoute(theRoute.OriginKey, theRoute.DestinyKey);

        if (haveIt)
        {
            if (theRoute.DateTime1 == _current.DateTime1)
            {
                //we have the latest one already
            }
            else
            {
                RemoveItem();
                _items.Add(theRoute);
                RemoveAllWithZeroCount();
            }
        }
        else
        {
            _items.Add(theRoute);
        }
    }

    bool IsInverseSet(TheRoute theRoute)
    {
        return theRoute.CheckPoints[0].InverseWasSet;
    }

    /// <summary>
    /// Will remove _current a Route from list of items that has same  origin and destiny key
    /// </summary>
    void RemoveItem()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            var sameOri = _items[i].OriginKey == _current. OriginKey;
            var sameDest = _items[i].DestinyKey == _current.DestinyKey;

            if (sameOri && sameDest)
            {
                _items.RemoveAt(i);
            }
        }
    }

    void RemoveAllWithZeroCount()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].CheckPoints.Count==0)
            {
                _items.RemoveAt(i);
                i--;
            }
        }
    }
}
