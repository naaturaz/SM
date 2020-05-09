using UnityEngine;
using UnityEngine.AI;

public class BodyAgent
{
    private float _speedInitial;
    private NavMeshAgent _agent;
    private Person _person;

    private Vector3 _destiny;
    private Vector3 _afterDestiny;
    private bool _destWasSet;

    private Vector3 _nextDest;

    private float _initRadius;

    private MDate _startDate;

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
        if (UPerson.IsThisPersonTheSelectedOne(_person) || Program.Debugger.IsThisOneTarget(_person))
        {
            var a = 1;
        }

        //correcting bug where kids stay in front of storage with path completed
        if ((_destWasSet && _agent.isOnNavMesh && _agent.enabled && !_person.IsMajor
            && !UMath.nearEqualByDistance(_agent.destination, Destiny, 0.1f) &&
            _agent.pathStatus == NavMeshPathStatus.PathComplete)
            ||
            (_agent.pathStatus == NavMeshPathStatus.PathInvalid && _agent.enabled && _agent.isOnNavMesh)
            )
        {
            if (UPerson.IsThisPersonTheSelectedOne(_person) || Program.Debugger.IsThisOneTarget(_person))
            {
                //UVisHelp.CreateHelpers(Destiny, Root.yellowSphereHelp);
                var a = 1;
            }

            //so i set the destination again to the real one so they move towards it
            _agent.SetDestination(Destiny);
        }

        if (_nextDest != new Vector3() && !_destWasSet && _agent.isOnNavMesh && _agent.enabled)
        {
            if (UPerson.IsThisPersonTheSelectedOne(_person) || Program.Debugger.IsThisOneTarget(_person))
            {
                //UVisHelp.CreateHelpers(Destiny, Root.redSphereHelp);
                var a = 1;
            }

            _destWasSet = true;
            _agent.SetDestination(Destiny);
            _startDate = Program.gameScene.GameTime1.CurrentDate();
        }
        CheckIfGoingIntoBuild();
        RadiusForHeavyLoaders();

        CheckVelocity();
        CheckIfTempSpeed();
        CheckIfPathPending();

        CheckIfStuck();
    }

    /// <summary>
    /// For public questions
    /// </summary>
    /// <returns></returns>
    public bool IsStuck()
    {
        //had 28 since I put the Body Agent
        return _startDate != null && Program.gameScene.GameTime1.ElapsedDateInDaysToDate(_startDate) > 28;
    }


    private void CheckIfStuck()
    {
        if (_startDate != null && Program.gameScene.GameTime1.ElapsedDateInDaysToDate(_startDate) > 30)
        {
            //so restarts the routing
            Debug.Log(_person.Name + " - stuck. restarts the routing");
            _destWasSet = false;
            _startDate = null;
        }
    }

    private string savedAni = "";
    private bool hidden;

    /// <summary>
    /// In version Unity 2017.1 and above it seems they have an internal queue so at 10x speed
    /// with over 160 agents they take a while to start walking
    /// </summary>
    private void CheckVelocity()
    {
        if (Program.gameScene.GameSpeed == 0)
        {
            return;
        }

        var onIdleSpot = (_person.Brain.CurrentTask == HPers.IdleSpot || _person.Brain.CurrentTask == HPers.Praying) &&
                _person.Body.Location == HPers.IdleSpot &&
                (_person.Body.GoingTo == HPers.IdleSpot || _person.Body.GoingTo == HPers.Home);

        if (_person.Body.IsNearBySpawnPointOfInitStructure() || onIdleSpot)
        {
            //right where stops for idle
            if (onIdleSpot && savedAni == "" && !_person.Body.MovingNow)
            {
                savedAni = _person.Body.CurrentAni;
                _person.Body.Show();
                _person.Body.TurnCurrentAniAndStartNew("isIdle");
                return;
            }
            //add the other places cant be hidden
            else if (!onIdleSpot && (!_person.IsAroundHouseSpawnPoint() || !_person.Body.MovingNow || _agent.pathPending))
            {
                hidden = true;
                _person.Body.HideNoQuestion();
            }
        }
        else if (hidden && !_person.Body.IsNearBySpawnPointOfInitStructure() && _person.Body.MovingNow)//is walking already
        {
            hidden = false;
            _person.Body.Show();
        }
        //will restart 'isWalk' as soon it moves
        if (savedAni != "" && (_agent.velocity != new Vector3() || _person.Body.MovingNow))
        {
            savedAni = "";
        }
    }

    private string _savedAniPathPending = "";

    /// <summary>
    /// When at 10x, and over 100ppl may take a while for the agent get the requested path
    /// </summary>
    private void CheckIfPathPending()
    {
        //if is waiting for a path and suppose to be movung already, then will be promt to Iddle
        if (_savedAniPathPending == "" && _agent.pathPending && _person.Body.IAmShown())
        {
            _savedAniPathPending = _person.Body.CurrentAni;
            _person.Body.TurnCurrentAniAndStartNew("isIdle");
            _startDate = null;
        }
        //once is ready will retake its ani
        else if (_savedAniPathPending != "" && !_agent.pathPending)
        {
            _person.Body.TurnCurrentAniAndStartNew(_savedAniPathPending);
            _savedAniPathPending = "";
            _startDate = Program.gameScene.GameTime1.CurrentDate();
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

    private General deb;

    private void Debugg(Vector3 point)
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

        //if person is:
        //hidden
        //close enough to the destiny spawnpoint
        //destiny is a:
        //Dock
        //Library
        //Dont show
        if (_person.Body != null)
        {
            if (UPerson.IsThisPersonTheSelectedOne(_person))
            {
                var a = 1;
            }

            if (_person.Body.IsHidden() && _person.Body.IsNearBySpawnPointOfInitStructure() &&
                (_person.Body.IsDestinyOrOrigin(H.Library) || _person.Body.IsDestinyOrOrigin(H.Dock)))
            {
            }
            else
                _person.Body.Show();
        }

        if (goingTo == HPers.InWork && _person.ProfessionProp != null
            && _person.ProfessionProp.ProfDescription == Job.Builder)
        {
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
        else
        {
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;//low
        }
    }

    private void RadiusForHeavyLoaders()
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

    private void CartRideRadius()
    {
        _initRadius = _agent.radius;
        _agent.radius *= 4;//2
    }

    #region Speed

    //bz wheelbarrows spin at 10x
    private float _tempSpeedSetAt;

    internal void NewSpeed()
    {
        if (_person.Name == "Barry")
        {
            var a = 1;
        }

        _agent.speed = NewSpeedValue();
        CheckOnAnimation();
    }

    private float NewSpeedValue()
    {
        return _speedInitial * AgeSpeedCorrection() * Program.gameScene.GameSpeed;
    }

    /// <summary>
    /// Everytime a new animation is set should call this. So if is WheelBarrow will slow down
    /// </summary>
    public void CheckOnAnimation()
    {
        if (Program.gameScene.GameSpeed >= 5 && _person.Body != null && _person.Body.CurrentAni == "isWheelBarrow")
        {
            //so they dont spin
            _agent.speed = _speedInitial * AgeSpeedCorrection() * Program.gameScene.GameSpeed / 5;
            _tempSpeedSetAt = Time.time;
            _person.Body.SetAnimatorSpeed(Program.gameScene.GameSpeed / 5);
        }
        if (_person != null && _person.Body != null && _person.Body.CurrentAni == "isCarry")
        {
            _agent.speed /= IsCarryAgeSpeedCorrection();
        }
    }

    //2.2f perfect for >21 yr ...  //1.8f is perfect for 6 years old
    private float IsCarryAgeSpeedCorrection()
    {
        var result = 4.2f;

        var yearDiff = 21 - 6;
        var speedDiff = 2.2f - 1.8f;
        var eachYearWorth = speedDiff / yearDiff;

        var yearsTo21 = _person.Age >= 21 ? 0 : 21 - _person.Age;
        var toRemoveFrom21YearSpeed = eachYearWorth * yearsTo21;

        result -= toRemoveFrom21YearSpeed;

        return result;
    }

    private void CheckIfTempSpeed()
    {
        if (_tempSpeedSetAt == 0)
        {
            return;
        }

        //will restablish normal speed
        if (Time.time > _tempSpeedSetAt + 4f)
        {
            _tempSpeedSetAt = 0;
            _agent.speed = _speedInitial * AgeSpeedCorrection() * Program.gameScene.GameSpeed;
            _person.Body.SetAnimatorSpeed(Program.gameScene.GameSpeed);
        }
    }

    private float AgeSpeedCorrection()
    {
        var factor = (_person.Age / 10) + .6f;

        if (_person.Age > 21 || factor <= 0 || factor > 1)
        {
            return 1;
        }
        return factor;
    }

    #endregion Speed

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

            "\npathPending: " + _agent.pathPending +
            "\nhasPath: " + _agent.hasPath +

        "\nEnabled: " + _agent.enabled +
        "\nNextDest: " + _nextDest +
        "\nVelocity: " + _agent.velocity +
                "\nCurr Task: " + _person.Brain.CurrentTask +
        "\nGoingTo: " + _person.Body.GoingTo +
        "\nLoc: " + _person.Body.Location +
        "\nIsNearBySpawnPointOfInitStructure: " + _person.Body.IsNearBySpawnPointOfInitStructure() +
        "\nProf: " + _person.ProfessionProp.ProfDescription;
    }

    internal void OneSecondUpdate()
    {
    }

    internal bool IsMoving()
    {
        return _agent.speed > 0;
    }

    public void DebugWasDestSetToFalse()
    {
        _destWasSet = false;
    }
}