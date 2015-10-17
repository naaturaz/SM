using System;
using UnityEngine;
using System.Collections.Generic;

public class QueuesContainer 
{
    private QueueTask _newBuildsQueue = new QueueTask();
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
    public void AddToNewBuildsQueue(List<Vector3> objP)
    {
        _newBuildsQueue.AddToQueue(objP);
        RestartPeopleChecked();
    }
	
	/// <summary>
    /// Add the param 'objP' to _destroyedBuildsAnchors and clears the _peopleChecked list
    /// </summary>	
    public void AddToDestroyBuildsQueue(List<Vector3> objP)
    {
        _destroyBuildsQueue.AddToQueue(objP);
        RestartPeopleChecked();
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

        var onNewB = IsOnQueue(theRoute, _newBuildsQueue);
        var onDesB = IsOnQueue(theRoute, _destroyBuildsQueue);

        //print("onNewBlds:" + onNewB + ".elem.Count:" + _newBuildsAnchors.Elements.Count);
        //print("onDstBlds:" + onDesB + ".elem.Count:" + _destroyedBuildsAnchors.Elements.Count);

        if (which == HPers.Chill)//the last one
        {
            _peopleChecked.Add(personMyID);

            if (_peopleChecked.Count == PersonPot.Control.All.Count)
            { ClearAllQueues();}
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
    bool IsOnQueue(TheRoute theRoute, QueueTask queueTask)
    {
        for (int i = 0; i < queueTask.Elements.Count; i++)
        {
            DateTime date1 = queueTask.Elements[i].DateTime1;
            DateTime date2 = theRoute.DateTime1;

            int result = DateTime.Compare(date1, date2);

            //if they intersect and 
            //the queue element was created later than the route then need to reroute
            if (queueTask.Contains(theRoute.AreaRect, i) 
                && result > 0
                )
            {
                _currenTime = queueTask.Elements[i].DateTime1;
                return true;
            }
        }
        return false;
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
