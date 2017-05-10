using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BodyAgent  {

    float _speedInitial;
    NavMeshAgent _agent;
    Person _person;

    private Vector3 _destiny;
    bool _destWasSet;

    Vector3 _nextDest;

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
        if (_nextDest != new Vector3() && !_destWasSet && _agent.isOnNavMesh)
        {
            _destWasSet = true;
            _agent.SetDestination(Destiny);
        }
        CheckIfGoingIntoBuild();
    }

    private void CheckIfGoingIntoBuild()
    {
        if (_person == null) { return; }

        if (UMath.nearEqualByDistance(Destiny, _person.transform.position, 0.1f))
        {
            if (_nextDest != new Vector3())
            {
                Debug.Log("Point reached:" + _person.Name);
            }

            _nextDest = new Vector3();
            _agent.enabled = false;
        }
    }

    General deb;
    internal void Walk(Vector3 point)
    {
        if (deb !=null)
        {
            deb.Destroy();
        }

        _agent.enabled = false;

        _destWasSet = false;
        Destiny = point;
        _nextDest = point;
        _agent.enabled = true;
        deb = UVisHelp.CreateHelpers(point, Root.yellowCube);
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

    internal void CleanDestiny()
    {
        _destiny = new Vector3();
    }
}
