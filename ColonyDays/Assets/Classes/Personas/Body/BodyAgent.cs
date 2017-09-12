﻿using System;
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

    float _initRadius;



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

    public float ReachRadius { get; internal set; }

    public BodyAgent(Person person)
    {
        ReachRadius = .5f;

        _person = person;
        _agent = _person.GetComponent<NavMeshAgent>();
        _speedInitial = _agent.speed;
        //so they get up to speed 
        NewSpeed();

        _agent.enabled = false;

        //_agent.SetAreaCost(3, 1);
    }









    // Update is called once per frame
    public void Update()
    {
        //correcting bug where kids stay in front of storage with path completed
        if ((_destWasSet && _agent.isOnNavMesh && _agent.enabled && !_person.IsMajor
            && !UMath.nearEqualByDistance(_agent.destination, Destiny, 0.1f) &&
            _agent.pathStatus == NavMeshPathStatus.PathComplete)
            ||
            (_agent.pathStatus == NavMeshPathStatus.PathInvalid && _agent.enabled && _agent.isOnNavMesh)
            )
        {
            //so i set the destination again to the real one so they move towards it 
            _agent.SetDestination(Destiny);

            //#if UNITY_EDITOR
            //            Debug.Log("Corrected pathCompleted: " + _person.name + " :pathStatus: " + _agent.pathStatus);
            //#endif
            //            _agent.radius = 0.08f;
            //_agent.height = 3;
        }

        if (_nextDest != new Vector3() && !_destWasSet && _agent.isOnNavMesh && _agent.enabled)
        {
            _destWasSet = true;
            _agent.SetDestination(Destiny);
        }
        CheckIfGoingIntoBuild();
        RadiusForHeavyLoaders();

        CheckVelocity();
    }


    string savedAni = "";
    /// <summary>
    /// In version Unity 2017.1 and above it seems they have an internal queue so at 10x speed
    /// with over 160 agents they take a while to start walking 
    /// </summary>
    private void CheckVelocity()
    {
        var notWalkingYet = _agent.enabled && _nextDest != new Vector3() && _agent.velocity == new Vector3();

        if ((notWalkingYet || _person.Body.IsNearBySpawnPointOfInitStructure())
            && savedAni == "" && _person.Body.IAmShown() && _person.Body.MovingNow)
        {
            savedAni = _person.Body.CurrentAni;
            _person.Body.TurnCurrentAniAndStartNew("isIdle");

            //if is not idling will be hidden 
            if (!_person.Brain.CurrentTask.ToString().Contains("Idle"))
            {
                _person.Body.HideNoQuestion();
            }
        }
        else if (savedAni != "" && !notWalkingYet && !_person.Body.IsNearBySpawnPointOfInitStructure())//is walking already
        {
            _person.Body.Show();
            _person.Body.TurnCurrentAniAndStartNew(savedAni);
            savedAni = "";
        }
    }

    private void CheckIfGoingIntoBuild()
    {
        if (_person == null) { return; }

        if (UMath.nearEqualByDistance(Destiny, _person.transform.position, ReachRadius))
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

    void RadiusForHeavyLoaders()
    {
        if (_person == null || _person.Body == null)
        {
            return;
        }

        if (_person.Body.CurrentAni.Contains("Cart") && _initRadius == 0)
        {
            CartRideRadius();
        }
        //rezising the radius down to original size 
        else if (_initRadius > 0 && !_person.Body.CurrentAni.Contains("Cart"))
        {
            _agent.radius = _initRadius;
            _initRadius = 0;
        }
    }

    void CartRideRadius()
    {
        _initRadius = _agent.radius;
        _agent.radius *= 4;//2
    }




    internal void NewSpeed()
    {
        _agent.speed = _speedInitial * AgeSpeedCorrection() * Program.gameScene.GameSpeed;
    }

    float AgeSpeedCorrection()
    {
        var factor = (_person.Age / 10) + .6f;

        if (_person.Age > 21 || factor <= 0 || factor > 1)
        {
            return 1;
        }
        return factor;
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

    internal string DebugInfo()
    {
        return "isOnNavMesh: " + _agent.isOnNavMesh +
            "\npathStatus: " + _agent.pathStatus +
        "\nEnabled: " + _agent.enabled +
        "\nNextDest: " + _nextDest +
        "\nVelocity: " + _agent.velocity;

    }
}
