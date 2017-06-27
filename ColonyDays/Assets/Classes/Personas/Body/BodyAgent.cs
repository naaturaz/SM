using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BodyAgent
{

    float _speedInitial;
    NavMeshAgent _agent;
    Person _person;

    private Vector3 _destiny;
    Vector3 _afterDestiny;
    bool _destWasSet;

    Vector3 _nextDest;

    ////if a route was interrupt
    //private bool _wasInterrupt;



    public Vector3 Destiny
    {
        get
        {
            return _destiny;
        }

        set
        {
            _destiny = value;
        }
    }

    public BodyAgent(Person person)
    {
        _person = person;
        _agent = _person.GetComponent<NavMeshAgent>();
        _speedInitial = _agent.speed;
        //so they get up to speed 
        NewSpeed();

        _agent.enabled = false;
    }









    // Update is called once per frame
    public void Update()
    {
        //correcting bug where kids stay in front of storage with path completed
        if ((_destWasSet && _agent.isOnNavMesh && _agent.enabled && !_person.IsMajor 
            && !UMath.nearEqualByDistance(_agent.destination ,Destiny, 0.1f) &&
            _agent.pathStatus == NavMeshPathStatus.PathComplete) 
            || 
            (_agent.pathStatus == NavMeshPathStatus.PathInvalid && _agent.enabled && _agent.isOnNavMesh)
            )
        {
            //so i set the destination again to the real one so they move towards it 
            _agent.SetDestination(Destiny);
            //Debug.Log("Corrected pathCompleted: " + _person.name + " :pathStatus: " + _agent.pathStatus);
        }

        if (_nextDest != new Vector3() && !_destWasSet && _agent.isOnNavMesh && _agent.enabled)
        {
            _destWasSet = true;
            _agent.SetDestination(Destiny);
        }
        CheckIfGoingIntoBuild();
    }

    private void CheckIfGoingIntoBuild()
    {
        if (_person == null) { return; }

        if (UMath.nearEqualByDistance(Destiny, _person.transform.position, 0.3f))
        {
            if (_nextDest != new Vector3())
            {
                //Debug.Log("Point reached:" + _person.Name);
                _nextDest = new Vector3();
                _agent.enabled = false;
            }
        }
    }


    General deb;

    void Debugg(Vector3 point)
    {
        if (deb != null)
        {
            deb.Destroy();
        }

        deb = UVisHelp.CreateHelpers(point, Root.yellowCube);
        deb.name = "Yellow > " + _person.MyId;
    }


    internal void Walk(Vector3 point, Vector3 afterDest, Vector3 moveNowTo, HPers goingTo)
    {
        _agent.enabled = false;


        _person.transform.position = moveNowTo;



        _destWasSet = false;
        Destiny = point;
        _afterDestiny = afterDest;
        _nextDest = point;
        _agent.enabled = true;

        //Debugg(point);

        if (_person.Body != null)
        {
            _person.Body.Show();
        }

        if (goingTo == HPers.InWork && _person.ProfessionProp != null
            && _person.ProfessionProp.ProfDescription == Job.Builder)
        {
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
        else
        {
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        }
    }

    internal void NewSpeed()
    {
        _agent.speed = _speedInitial * Program.gameScene.GameSpeed;
    }

    internal void PutOnNavMeshIfNeeded(Vector3 vector3)
    {
        if (!_agent.isOnNavMesh)
        {
            _agent.enabled = false;

            _person.transform.position = vector3;
            _destWasSet = false;
            _agent.enabled = true;
        }
    }

    /// <summary>
    /// Called after  WalkDone() from body.cs
    /// </summary>
    internal void CleanDestiny()
    {
        _destiny = new Vector3();

        if (_afterDestiny == new Vector3())
        {
            return;
        }
        //will positioned there on after destiny tthat is a door 
        _person.transform.position = _afterDestiny;
    }

    ///// <summary>
    ///// Used when a person is building a building and was completly done before he got there . Needs to go back 
    ///// </summary>
    //internal void ReachDestiny()
    //{
    //    Destiny = _person.transform.position;
    //    _wasInterrupt = true;
    //}
}
