using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BodyAgent  {

    NavMeshAgent _agent;
    Person _person;
    private Vector3 _destiny;

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
    }

















    ///////
    public void WalkRoutine(TheRoute route, HPers goingTo, bool inverse = false, HPers whichRouteP = HPers.None)
    {
        Destiny = route.CheckPoints[route.CheckPoints.Count - 1].Point;
        return;

    }

    // Update is called once per frame
    public void Update()
    {
        if (_nextDest != new Vector3()  && Input.GetKeyUp(KeyCode.U))
        {
            _agent.SetDestination(Destiny);
        }

        CheckIfGoingIntoBuild();

    }

    private void CheckIfGoingIntoBuild()
    {
        if (_person == null) { return; }

        if (UMath.nearEqualByDistance(Destiny, _person.transform.position, 0.5f))
        {
            _nextDest = new Vector3();
            _agent.enabled = false;

        }
    }

    internal void Walk(Vector3 point)
    {
        Destiny = point;
        _nextDest = point;
        _agent.enabled = true;
    }
}
