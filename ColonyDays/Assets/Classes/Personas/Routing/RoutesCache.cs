using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoutesCache
{

    Dictionary<string, TheRoute> _items = new Dictionary<string, TheRoute>();
    TheRoute _current = new TheRoute();//current route we are comparing to
    List<TheRoute> _itemsLoadSave = new List<TheRoute>();

    //so they are saveLoad
    public List<TheRoute> ItemsLoadSave
    {
        get { return _itemsLoadSave; }
        set { _itemsLoadSave = value; }
    }

    public TheRoute Current
    {
        get { return _current; }
        set { _current = value; }
    }


    /// <summary>
    /// Needs to be called when saving this. so the List 'ItemsLoadSave' is created and is ready to save
    /// </summary>
    public void CreateSave()
    {
        for (int i = 0; i < _items.Count; i++)
        {
            _itemsLoadSave.Add(_items.ElementAt(i).Value);
        }
    }

    /// <summary>
    /// Needs to be called so the dict '_items' is populated
    /// </summary>
    public void LoadTheSave()
    {
        for (int i = 0; i < _itemsLoadSave.Count; i++)
        {
            var key = ReturnKey(_itemsLoadSave[i]);

            if (!_items.ContainsKey(key))
            {
                _items.Add(key, _itemsLoadSave[i]);
            }
        }
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
        //needs to validate all the current routes first
        if (_checkOnQueues)
        {
            return false;
        }

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
            if (_items[key].CheckPoints.Count > 0)
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

    bool OriginDestinyContains(TheRoute theRoute, string word)
    {
        return theRoute.OriginKey.Contains(word) || theRoute.DestinyKey.Contains(word);
    }

    public void AddReplaceRoute(TheRoute theRoute)
    {
        //if _checkOnQueues is b is checking we need to keep pure that revision.
        //after is done this will accept routes again
        if (_checkOnQueues || theRoute == null || theRoute.CheckPoints.Count == 0 
            || theRoute.CheckPoints.Count == 0 || OriginDestinyContains(theRoute, "Dummy"))
        {
            return;
        }

        var key = theRoute.OriginKey + "." + theRoute.DestinyKey;
        var haveIt = DoWeHaveThatRoute(theRoute.OriginKey, theRoute.DestinyKey);

        if (haveIt)
        {
            var isNewer = IsNewerOrSame(theRoute.DateTime1);
            if (isNewer)
            {
                _items[key] = theRoute;
            }
            //other wise we have the lastest one 
        }
        else
        {
            _items.Add(key, theRoute);
            //Debug.Log("added to cache:" + key + " ct:" + _items.Count);
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
        CheckIfARouteIsTooOld();
        CheckIfNewQueues();
    }


    /// <summary>
    /// Bz routes will stay there forever. really old and not useful 
    /// </summary>
    void CheckIfARouteIsTooOld()
    {
        
    }
    
    /// <summary>
    /// Will remove all RoutesCahched related to this building 
    /// </summary>
    /// <param name="MyId"></param>
    internal void RemoveAllMine(string MyId)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items.ElementAt(i).Key.Contains(MyId))
            {
                var key = _items.ElementAt(i).Key;
                var removed = _items.Remove(key);

                if (removed)
                {
                    //Debug.Log("cachedRoute removed:"+key);
                    i--;
                }
            }
        }
    }

    internal string ItemsCount()
    {
        return _items.Count + "";
    }


    #region Queues

    private bool _checkOnQueues;
    private int _qCount;

    /// <summary>
    /// So all Routes in cache can be checked to see if one is colliding 
    /// </summary>
    public void CheckQueuesNow()
    {
        _checkOnQueues = true;
        _qCount = 0;//in case was running already
    }

    private void CheckIfNewQueues()
    {
        if (!_checkOnQueues)
        {
            return;
        }
        CheckRouteOnQueue();
    }

    private void CheckRouteOnQueue()
    {
        if (_qCount < _items.Count)
        {
            var theRoute = _items.ElementAt(_qCount).Value;
            _qCount++;
            var isCollided = PersonPot.Control.Queues.IsThisRouteOnAnyQueue(theRoute);
            var askTime = PersonPot.Control.Queues.GetLastCollisionTime();

            //if collided then we need to see if can be removed bz we might have and old version of the route
            if (isCollided)
            {
                RemoveRoute(theRoute, askTime);
            }
        }
        else
        {
            _checkOnQueues = false;
            _qCount = 0;
        }
    }


#endregion
}
