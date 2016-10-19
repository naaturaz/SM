using System;
using UnityEngine;
using System.Collections.Generic;

public class QueuesContainer 
{
    //new buildings built
    private QueueTask _newBuildsQueue = new QueueTask();
    //building that are order to be destroyed
    private QueueTask _destroyBuildsQueue = new QueueTask();

    List<string> _peopleChecked = new List<string>();

    public List<string> PeopleChecked
    {
        get { return _peopleChecked; }
        set { _peopleChecked = value; }
    }

    public QueueTask NewBuildsQueue
    {
        get { return _newBuildsQueue; }
        set { _newBuildsQueue = value; }
    }

    public QueueTask DestroyBuildsQueue
    {
        get { return _destroyBuildsQueue; }
        set { _destroyBuildsQueue = value; }
    }


    /// <summary>
    /// Add the param 'objP' to _newBuildsAnchors and clears the _peopleChecked list
    /// </summary>
    public void AddToNewBuildsQueue(List<Vector3> objP, string key)
    {
        if (key.Contains("Bridge"))
        {
           //Debug.Log("Called:"+key);
        }

        _newBuildsQueue.AddToQueue(objP, key, "new");
        RestartPeopleChecked();
        PersonPot.Control.RoutesCache1.CheckQueuesNow();
    }
	
	/// <summary>
    /// Add the param 'objP' to _destroyedBuildsAnchors and clears the _peopleChecked list
    /// </summary>	
    public void AddToDestroyBuildsQueue(List<Vector3> objP, string key)
    {
        _destroyBuildsQueue.AddToQueue(objP, key, "old");
        RestartPeopleChecked();
        buidingsWhenStartChecking = 0;

        PersonPot.Control.RoutesCache1.CheckQueuesNow();
    }

    void RestartPeopleChecked()
    {
        _peopleChecked.Clear();
    }

    /// <summary>
    /// Will keep ading a new value to _univCounter each iteration
    /// If all pople check then will Clear the queues
    /// </summary>
    /// <returns>True if collide with any building annchor queue</returns>
    public bool ContainAnyBuild(TheRoute theRoute, string personMyID, HPers which = HPers.None)
    {
        InitVal();
        if (_peopleChecked.Contains(personMyID)){return false;}

        var onNewB = IsOnQueue(theRoute, _newBuildsQueue, personMyID);
        var onDesB = IsOnQueue(theRoute, _destroyBuildsQueue, personMyID);
        //print("onNewBlds:" + onNewB + ".elem.Count:" + _newBuildsAnchors.Elements.Count);
        //print("onDstBlds:" + onDesB + ".elem.Count:" + _destroyedBuildsAnchors.Elements.Count);

        if (which == HPers.Chill)//the last one
        {
            _peopleChecked.Add(personMyID);

            if (_peopleChecked.Count >= PersonPot.Control.All.Count)
            {
                ClearAllQueues();
            }
        }
        return onNewB || onDesB;
    }

    int buidingsWhenStartChecking;
    private void InitVal()
    {
        if (_peopleChecked.Count == 0 && buidingsWhenStartChecking== 0)
        {
            buidingsWhenStartChecking = _newBuildsQueue.Elements.Count + _destroyBuildsQueue.Elements.Count;
        }
    }

	/// <summary>
    /// Called everytime all the person checked.
	/// Will redo it again if everyone didnt check all buildings already
	///
    /// </summary>
    void ClearAllQueues()
    {
        PersonPot.Control.BuildersManager1.AddGreenLightKeys(_newBuildsQueue);
        FinalForcedDestroy();
        //then all where check
        if (buidingsWhenStartChecking == _newBuildsQueue.Elements.Count + _destroyBuildsQueue.Elements.Count)
        {
            _newBuildsQueue.Elements.Clear();
            _destroyBuildsQueue.Elements.Clear();
        }

        buidingsWhenStartChecking = 0;
        //always is clear here 
        _peopleChecked.Clear();
    }

    public void IWasCheckedByAllPeople(QueueElement qEle)
    {
        if (qEle.Type1=="new")
        {
            //NewBuildsQueue.Elements.Remove(qEle);
            PersonPot.Control.BuildersManager1.AddGreenLightKeys(qEle);
        }
        else if (qEle.Type1 == "old")
        {
            //DestroyBuildsQueue.Elements.Remove(qEle);
            FinalForceDestroyLastStp(qEle);
        }

    }

    /// <summary>
    /// Because some Structure dont get destroy bz UnivCounter is not in -1 
    /// when is called then here we finally destory them Bz all people checked on this 
    /// for rerouting purposes
    /// 
    /// this can be done bz once a building is in this list is that went tru everything
    /// the only thing stopped it to be deleted was the UnivCounter
    /// </summary>
    void FinalForcedDestroy()
    {
        for (int i = 0; i < _destroyBuildsQueue.Elements.Count; i++)
        {
            var build = Brain.GetBuildingFromKey(_destroyBuildsQueue.Elements[i].Key);

            //bz could have been destroyed already
            if (build != null && !_destroyBuildsQueue.Elements[i].WasUsedToGreenLightOrDestroy &&
                _destroyBuildsQueue.Elements[i].IsCheckedByAll())
            {
                FinalForceDestroyLastStp(_destroyBuildsQueue.Elements[i]);
            }
        }
    }

    void FinalForceDestroyLastStp(QueueElement qEle)
    {
        if (qEle.WasUsedToGreenLightOrDestroy)
        {
            return;
        }

        qEle.WasUsedToGreenLightOrDestroy = true;
        var build = Brain.GetBuildingFromKey(qEle.Key);

        //when loading a WillBeDestroy strucutre sometimes gets destroyed right away
        if (build!=null)
        {
            build.DestroyOrderedForced(); 
        }
    }


    DateTime _currenTime=new DateTime();
    /// <summary>
    /// Wioll return the last element that return true time if any
    /// </summary>
    /// <returns></returns>
    public DateTime GetLastCollisionTime()
    {
        return _currenTime;
    }

	/// <summary>
    /// Says if the param 'other' is contained in any _newBuildsAnchors
    /// 
    /// and if the element on the queue was added after the route was created 
    /// </summary>
    bool IsOnQueue(TheRoute theRoute, QueueTask queueTask, string personID = "")
    {
        for (int i = 0; i < queueTask.Elements.Count; i++)
        {
            DateTime date1 = queueTask.Elements[i].DateTime1;
            DateTime date2 = theRoute.DateTime1;

            int result = DateTime.Compare(date1, date2);
            //can be called here bz a person when call the Queues to check in goes trhu all of them 
            queueTask.Elements[i].CheckPersonIn(personID);

            //if they intersect and //the queue element was created later than the route then need to reroute
            if (queueTask.Contains(theRoute.AreaRect, i) && result > 0)
            {
                _currenTime = queueTask.Elements[i].DateTime1;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Created so chache will Check if anything new in the queue interfieres with thm
    /// 
    /// Needed bz Homers will keep using old Cached ROutes and they will never get updated
    /// </summary>
    /// <param name="cachedRoute"></param>
    /// <returns></returns>
    public bool IsThisRouteOnAnyQueue(TheRoute cachedRoute)
    {
        var onNewB = IsOnQueue(cachedRoute, _newBuildsQueue);
        var onDesB = IsOnQueue(cachedRoute, _destroyBuildsQueue);

        return onNewB || onDesB;
    }

    /// <summary>
    /// Manullay checks person in into all _newBuildsQueue and _destroyBuildsQueue Elements 
    /// </summary>
    /// <param name="personID"></param>
    public void CheckMeInToQueueElements(string personID)
    {
        for (int i = 0; i < _newBuildsQueue.Elements.Count; i++)
        {
            _newBuildsQueue.Elements[i].CheckPersonIn(personID);
        } 
        for (int i = 0; i < _destroyBuildsQueue.Elements.Count; i++)
        {
            _destroyBuildsQueue.Elements[i].CheckPersonIn(personID);
        }
    }

    /// <summary>
    /// So it decreses the AllCount in the Element 
    /// </summary>
    public void PersonDie()
    {
        for (int i = 0; i < _newBuildsQueue.Elements.Count; i++)
        {
            _newBuildsQueue.Elements[i].PersonDie();
        }
        for (int i = 0; i < _destroyBuildsQueue.Elements.Count; i++)
        {
            _destroyBuildsQueue.Elements[i].PersonDie();
        }
    }

	/// <summary>
    /// Indicates if the Queue is empty or not
    /// </summary>
    public bool IsEmpty()
    {
        if (_newBuildsQueue.Elements.Count == 0 && _destroyBuildsQueue.Elements.Count == 0)
        {
            return true;
        }
        return false;
    }    
}
