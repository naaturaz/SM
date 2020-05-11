using System.Collections.Generic;
using UnityEngine;

/*
 * This is a queue of people . can be used to queue People to get a task done.
 * copyied and pasted from PersonController. just the _peopleChecked if statement was removed
 * as this class objects are independent of anything on game
 *
 * Created to be used by professionals to reroute. so they dont reroute all toghetehr
 * at the same time
 */

public class PeopleQueue
{
    //People will reroute if they had not reroute already in this cycle and
    //if queue has space. Other wise person should wait at home

    private List<CheckedIn> _onSystemNow = new List<CheckedIn>();

    //the number is not inclusinve so if u put a 3 will alow 2
    private int _systemCap = 7;//amt of person   //1

    //people waiting to be pass to _onSystemNow
    private List<CheckedIn> _waitList = new List<CheckedIn>();

    /// <summary>
    /// This doesnt need to be SaveLoad. Will give probl
    /// </summary>
    public List<CheckedIn> OnSystemNow1
    {
        get { return _onSystemNow; }
        set { _onSystemNow = value; }
    }

    public List<CheckedIn> WaitList
    {
        get { return _waitList; }
        set { _waitList = value; }
    }

    public PeopleQueue()
    {
    }

    public void CheckMeOnSystem(string id)
    {
        var find = _onSystemNow.Find(a => a.Id == id);

        if (find != null)
        {
            //was checked in already
            return;
        }

        _onSystemNow.Add(new CheckedIn(id, Time.time));
    }

    /// <summary>
    /// Will teel u if id is on system list now
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool OnSystemNow(string id)
    {
        for (int i = 0; i < _onSystemNow.Count; i++)
        {
            //time is up
            if (id == _onSystemNow[i].Id)
            {
                return true;
            }
        }
        return false;
    }

    public float DoneReRoute(string p)
    {
        for (int i = 0; i < _onSystemNow.Count; i++)
        {
            if (_onSystemNow[i].Id == p)
            {
                var timeOnSys = Time.time - _onSystemNow[i].Time;
                _onSystemNow.RemoveAt(i);
                TransferFirstInWaitingListToOnSystemNow();
                return timeOnSys;
            }
        }
        return -1;
    }

    internal bool CanIReRouteNow(string pMyID)
    {
        //bz if he checked then dont need to try to get into system again
        return OnSystemNow1.Count < _systemCap;
    }

    internal void AddMeToOnSystemWaitList(string id)
    {
        if (IAmOnSystemNow(id))
        {
            return;
        }

        if (_onSystemNow.Count == 0 && WaitList.Count == 0)
        {
            _onSystemNow.Add(new CheckedIn(id, Time.time));
            return;
        }
        if (_onSystemNow.Count == 0 && WaitList.Count > 0)
        {
            TransferFirstInWaitingListToOnSystemNow();
        }

        WaitList.Add(new CheckedIn(id, Time.time));
    }

    /// <summary>
    /// Called when DoneReRoute() is called
    /// </summary>
    private void TransferFirstInWaitingListToOnSystemNow()
    {
        if (WaitList.Count == 0)
        {
            return;
        }

        var t = WaitList[0];
        WaitList.RemoveAt(0);
        OnSystemNow1.Add(t);
    }

    internal bool OnWaitListNow(string id)
    {
        var find = WaitList.Find(a => a.Id == id);

        if (find != null)
        {
            //was added in already
            return true;
        }
        return false;
    }

    /// <summary>
    /// To bne call when person dies
    /// </summary>
    /// <param name="id"></param>
    public void RemoveMeFromSystem(string id)
    {
        var wIndex = WaitList.FindIndex(a => a.Id == id);

        if (wIndex > -1)
        {
            WaitList.RemoveAt(wIndex);
        }

        var sIndex = OnSystemNow1.FindIndex(a => a.Id == id);

        if (sIndex > -1)
        {
            OnSystemNow1.RemoveAt(sIndex);
        }
    }

    /// <summary>
    /// Either on WaitList or SystemNow1
    /// </summary>
    /// <returns></returns>
    private bool IAmOnSystemNow(string id)
    {
        return OnSystemNow(id) || OnWaitListNow(id);
    }

    public void Update()
    {
        SanitizeCurrent();
    }

    private void SanitizeCurrent()
    {
        if (OnSystemNow1.Count == 0)
        {
            return;
        }

        var p = OnSystemNow1[0];
        var person = Family.FindPerson(p.Id);

        //if is being there for 10 sec we need to check
        if (Time.time > p.Time + 10f)
        {
            //if the person is not RouterActive means he is not working somehow so can be removed from here
            if (OnSystemNow1.Contains(p) && (person == null || !person.ProfessionProp.RouterActive))
            {
                //Debug.Log("remove from WorkQueue OnSystemNow1 Prof:" + p.Id);
                OnSystemNow1.Remove(p);
                TransferFirstInWaitingListToOnSystemNow();
            }
            if (WaitList.Contains(p) && (person == null || !person.ProfessionProp.RouterActive))
            {
                //Debug.Log("remove from WorkQueue WaitList Prof:" + p.Id);
                WaitList.Remove(p);
            }
        }
    }
}