using System;
using System.Collections.Generic;
using UnityEngine;

/* Address the Route case in where the person needs to go around a bay
 * or the Delta of a River
 *
 * Will return 4 points like this shape: /--\ to go around a delta river or bay
 *
 * When is here is bz somehow the route is colliding with Water btw Point A and B
 */

public class DeltaRouter
{
    private Person _person;

    private Vector3 _a;
    private Vector3 _b;
    private Vector3 _mid;

    private Vector3 _a1;//a prime
    private Vector3 _b1;//b prime

    //the last good _a1 position I can reach with without hitting a water body
    private Vector3 _lastGoodA1FromA;

    private Vector3 _lastGoodA1FromB;

    private Vector3 _lastGoodB1FromA;
    private Vector3 _lastGoodB1FromB;

    private bool recurseRoutineNow;

    private H currentCheck = H.None;
    private Vector3 currentLast;
    private Vector3 currentPrime;
    private Vector3 currentOrigin;

    //indicates if this is  Delta routable
    //is initiated at true bz until is found otherwise is
    private bool _isDeltaRoutable = true;

    //the route//
    private DeltaRoute _deltaRoute = new DeltaRoute();

    public DeltaRoute DeltaRoute
    {
        get { return _deltaRoute; }
        set { _deltaRoute = value; }
    }

    //temporary stores the positions where the algorithm is checking
    //if can reach it withoput water on the middle
    private List<Vector3> positionsToCheck = new List<Vector3>();

    public DeltaRouter()
    {
    }

    public DeltaRouter(Vector3 a, Vector3 b, Person person)
    {
        _person = person;
        _a = a;
        _b = b;
        _mid = (_a + _b) / 2;
        InitPrimes();

        recurseRoutineNow = true;
        ClearOldVars();
    }

    private void ClearOldVars()
    {
        DebugDestroy();

        _isDeltaRoutable = true;
        currentCheck = H.None;
        _deltaRoute = new DeltaRoute();
        _deltaRoute.Points.Clear();

        _lastGoodA1FromA = new Vector3();
        _lastGoodA1FromB = new Vector3();

        _lastGoodB1FromA = new Vector3();
        _lastGoodB1FromB = new Vector3();
    }

    /// <summary>
    /// Find the initial position of prime _a1 and _b1
    /// </summary>
    private void InitPrimes()
    {
        var midHelp = UVisHelp.CreateHelpers(_mid, Root.yellowCube);
        var a1Help = UVisHelp.CreateHelpers(_a, Root.yellowCube);
        var b1Help = UVisHelp.CreateHelpers(_b, Root.yellowCube);

        a1Help.transform.SetParent(midHelp.transform);
        b1Help.transform.SetParent(midHelp.transform);
        midHelp.transform.Rotate(0, 90, 0);

        _a1 = a1Help.transform.position;
        _b1 = b1Help.transform.position;

        var dist = Vector3.Distance(_mid, _a1);

        var times = 3;//8 for mountains was working ok
        //pushe them aaway another step as big as the dist btw them initialiliy and middle * times
        _a1 = Vector3.MoveTowards(_a1, _mid, -dist * times);
        _b1 = Vector3.MoveTowards(_b1, _mid, -dist * times);

        midHelp.Destroy();
        a1Help.Destroy();
        b1Help.Destroy();
    }

    /// <summary>
    /// Main Method on this Class this is the one that is recursed 4 times as the 'currentCheck' changes
    /// </summary>
    private void RecurseRoutine()
    {
        currentLast = new Vector3();
        DefineCurrent();
        DefinePositionsToCheck(currentPrime);
        ThrowRays();
        SaveCurrent();

        if (currentCheck == H.Done)
        {
            recurseRoutineNow = false;
        }
    }

    /// <summary>
    /// Defines the last good position from the origin to that prime
    /// in this case the prime is not seeing or use directly bz 'positionsToCheck' was created
    /// using the prime position already
    /// </summary>
    private void ThrowRays()
    {
        int lastI = -1;
        for (int i = 0; i < positionsToCheck.Count; i++)
        {
            if (!RouterManager.IsWaterOrMountainBtw(currentOrigin, positionsToCheck[i]))
            {
                lastI = i;
                break;
            }
        }
        DefineLastCurrent(lastI);
    }

    /// <summary>
    /// Created so the player doesnt pass thru river terrain tht is unenven
    /// </summary>
    /// <param name="lastI">Last index was good</param>
    private void DefineLastCurrent(int lastI)
    {
        if (lastI != -1)
        {
            currentLast = positionsToCheck[lastI];
            if (lastI + 1 < positionsToCheck.Count)
            {
                //IMPORTANT here am checking if the one before is visible
                if (!RouterManager.IsWaterOrMountainBtw(positionsToCheck[lastI + 1], currentOrigin))
                {
                    //the middle btw the last good and the one before
                    currentLast = (positionsToCheck[lastI] + positionsToCheck[lastI + 1]) / 2;
                }
            }
        }
    }

    /// <summary>
    /// Define the current variables to use
    /// </summary>
    private void DefineCurrent()
    {
        if (currentCheck == H.None)
        {
            //from a to _a1
            currentCheck = H.AtoA1;
            currentLast = _lastGoodA1FromA;
            currentPrime = _a1;
            currentOrigin = _a;
        }
        else if (currentCheck == H.AtoA1)
        {
            //from b to _a1
            currentCheck = H.BtoA1;
            currentLast = _lastGoodA1FromB;
            currentOrigin = _b;
        }
        else if (currentCheck == H.BtoA1)
        {
            //from a to _b1
            currentCheck = H.AtoB1;
            currentLast = _lastGoodB1FromA;
            currentPrime = _b1;
            currentOrigin = _a;
        }
        else if (currentCheck == H.AtoB1)
        {
            //from b to _b1
            currentCheck = H.BtoB1;
            currentLast = _lastGoodB1FromB;
            currentOrigin = _b;
        }
        else
        {
            AfterChecks();
        }
    }

    /// <summary>
    /// Saves the 'currentLast' on the actually current Vector 3 holding that
    /// </summary>
    private void SaveCurrent()
    {
        if (currentCheck == H.AtoA1)
        {
            //from a to _a1
            _lastGoodA1FromA = currentLast;
        }
        else if (currentCheck == H.BtoA1)
        {
            //from b to _a1
            _lastGoodA1FromB = currentLast;
        }
        else if (currentCheck == H.AtoB1)
        {
            //from a to _b1
            _lastGoodB1FromA = currentLast;
        }
        else if (currentCheck == H.BtoB1)
        {
            //from b to _b1
            _lastGoodB1FromB = currentLast;
        }
    }

    private void AfterChecks()
    {
        currentCheck = H.Done;
        DefineDeltaRoute();

        if (_isDeltaRoutable)
        {
            MoveEachOneAloneAndThrowRayAndConformRoute();
        }
        else
        {
            Stop();
        }
    }

    private void Stop()
    {
        recurseRoutineNow = false;//so stops the recursion
                                  //throw new Exception("Not Posible routeing btw points on DeltaRouter");
                                  //GameScene.print("Not Posible routeing btw points on DeltaRouter");
    }

    //step to move along towards middle and then towards last good position
    private float step = 10f;//3f was working ok for mountain

    private void DefinePositionsToCheck(Vector3 primeVector)
    {
        positionsToCheck.Clear();
        float dist = Vector3.Distance(primeVector, _mid);//sp it goes twice as far to check
        Vector3 t = _mid;

        for (float i = 0; i < dist; i = i + step)
        {
            t = Vector3.MoveTowards(t, primeVector, step);
            //if is not on Terrain will break loop so wont add antyhing else towards that Prime
            //bz sim,ply after that is just out of terrain. In this way I will never RayCast a point
            //out of terrain
            if (!UTerra.IsOnTerrain(t))
            {
                //Debug.Log("not on terrain Delta DefinePositionsToCheck()");
                break;
            }
            step *= 2f;
            positionsToCheck.Add(t);
        }
        //_person.DebugList.AddRange(UVisHelp.CreateHelpers(positionsToCheck, Root.blueSphereHelp));
        step = 10f;
    }

    private void DefineDeltaRoute()
    {
        //if a pair of them towards A1 or B1 are set that means yeas we found a middle point
        //for each that can ponteatlly be used to create route
        if ((_lastGoodA1FromA != new Vector3() && _lastGoodA1FromB != new Vector3()))
        {
            _deltaRoute = new DeltaRoute(_a, _b, _lastGoodA1FromA, _lastGoodA1FromB);

            //  _person.DebugList.Add(UVisHelp.CreateText(_lastGoodA1FromA, "_lastGoodA1FromA"));
            // _person.DebugList.Add(UVisHelp.CreateText(_lastGoodA1FromB, "_lastGoodA1FromB"));
        }
        else if (_lastGoodB1FromA != new Vector3() && _lastGoodB1FromB != new Vector3())
        {
            /*DefineDeltaRoute() somehting is not working fine.
            //if a river is on middle should never have 2 points with good on the same side
             *
             *The problem with river freezing will give one point on first if. and 2 in this one.
             *Whit a river onmiddle ... so tht is bugg... I fixed with addressing the infinite loop.
             *But this still need to be fixed
             */
            _deltaRoute = new DeltaRoute(_a, _b, _lastGoodB1FromA, _lastGoodB1FromB);

            //_person.DebugList.Add(UVisHelp.CreateText(_lastGoodB1FromA, "_lastGoodB1FromA"));
            //  _person.DebugList.Add(UVisHelp.CreateText(_lastGoodB1FromB, "_lastGoodB1FromB"));
        }
        else _isDeltaRoutable = false;
    }

    private int loopCounts;

    /// <summary>
    /// Move each point towards is target and throw Ray agaisnt the other to see if can
    /// see it already
    /// </summary>
    private void MoveEachOneAloneAndThrowRayAndConformRoute()
    {
        Vector3 aMove = _deltaRoute.AnyA;
        Vector3 bMove = _deltaRoute.AnyB;

        //will keep mpving them towards target until they can see eachother without water on midle
        while (RouterManager.IsWaterOrMountainBtw(aMove, bMove))
        {
            aMove = Vector3.MoveTowards(aMove, _deltaRoute.TargetA, step / 2);
            bMove = Vector3.MoveTowards(bMove, _deltaRoute.TargetB, step / 2);

            loopCounts++;

            //this address probblem of inifitnie loop
            //for some reason in  void DefineDeltaRoute() somehting is not working fine.
            //if a river is on middle should never have 2 points with good on the same side
            if (loopCounts > 100)
            {
                loopCounts = 0;
                _isDeltaRoutable = false;
                Stop();
                break;
            }
        }

        //push away a bit towards target. To avoid infinite recursion on certain spots
        aMove = Vector3.MoveTowards(aMove, _deltaRoute.TargetA, step);
        bMove = Vector3.MoveTowards(bMove, _deltaRoute.TargetB, step);

        _deltaRoute.BestA = AssignIniPositionIfNotInBuild(aMove, _deltaRoute.TargetA);
        _deltaRoute.BestB = AssignIniPositionIfNotInBuild(bMove, _deltaRoute.TargetB);
        _deltaRoute.ConformRoute();

        // _person.DebugList.Add( UVisHelp.CreateHelpers(_deltaRoute.BestA, Root.redSphereHelp));
        // _person.DebugList.Add(UVisHelp.CreateHelpers(_deltaRoute.BestB, Root.redSphereHelp));
    }

    /// <summary>
    /// Says if is Routable or not btw point A and B
    /// </summary>
    public bool IsDeltaRoutable()
    {
        return _isDeltaRoutable;
    }

    public void Update()
    {
        if (recurseRoutineNow)
        {
            RecurseRoutine();
        }
    }

    /// <summary>
    /// Will return position if 'oringin' was not contained in a building ...
    /// other wise will recurse keep moving towards target... until a spot is find it that
    /// is not contained in a building or 'origin' is equals 'target'
    /// </summary>
    public Vector3 AssignIniPositionIfNotInBuild(Vector3 origin, Vector3 target)
    {
        var personBounds = UPoly.CreatePolyFromVector3(origin, _person.PersonDim, _person.PersonDim);
        //if bound collide will recurse
        if (BuildingPot.Control.Registro.IsCollidingWithExisting(personBounds))
        {
            //if both are the same they the origin was moved all the way untli the target
            //so no more recursion is needed... i couldnt find a point where was not building
            //this is covering a infinite loop, should nt happen ever
            if (UMath.nearEqualByDistance(origin, target, 0.1f))
            {
                throw new Exception("origin reach target on AssignIniPositionIfNotInBuild() DeltaRouter");
            }

            origin = Vector3.MoveTowards(origin, target, 0.04f);
            origin = AssignIniPositionIfNotInBuild(origin, target);
        }
        return origin;
    }

    private List<Vector3> debugPos = new List<Vector3>();

    public void DebugDestroy()
    {
        for (int i = 0; i < _person.DebugList.Count; i++)
        {
            _person.DebugList[i].Destroy();
        }
        _person.DebugList.Clear();
    }

    private void DebugRender()
    {
        for (int i = 0; i < debugPos.Count; i++)
        {
            // _person.DebugList.Add(UVisHelp.CreateHelpers(debugPos[i], Root.blueCube));
            //  _person.DebugList.Add(UVisHelp.CreateText(debugPos[i], i.ToString(), 55));
        }
    }
}

public class DeltaRoute
{
    private Vector3 _anyA;
    private Vector3 _targetA;
    private Vector3 _anyB;
    private Vector3 _targetB;

    private Vector3 _bestA;//this is the clostst theyu can see each other on A
    private Vector3 _bestB;//this is the clostst theyu can see each other on B

    private List<Vector3> _points = new List<Vector3>();

    public DeltaRoute()
    {
    }

    public DeltaRoute(Vector3 anyA, Vector3 anyB, Vector3 targetA, Vector3 targetB)
    {
        _anyA = anyA;
        _anyB = anyB;
        _targetA = targetA;
        _targetB = targetB;
    }

    public void ConformRoute()
    {
        _points.Add(_anyA);
        _points.Add(_bestA);
        _points.Add(_bestB);
        _points.Add(_anyB);
        //UVisHelp.CreateHelpers(_points, Root.blueCubeBig);
    }

    public Vector3 AnyA
    {
        get { return _anyA; }
        set { _anyA = value; }
    }

    public Vector3 AnyB
    {
        get { return _anyB; }
        set { _anyB = value; }
    }

    public Vector3 TargetA
    {
        get { return _targetA; }
        set { _targetA = value; }
    }

    public Vector3 TargetB
    {
        get { return _targetB; }
        set { _targetB = value; }
    }

    public Vector3 BestA
    {
        get { return _bestA; }
        set { _bestA = value; }
    }

    public Vector3 BestB
    {
        get { return _bestB; }
        set { _bestB = value; }
    }

    public List<Vector3> Points
    {
        get { return _points; }
        set { _points = value; }
    }
}