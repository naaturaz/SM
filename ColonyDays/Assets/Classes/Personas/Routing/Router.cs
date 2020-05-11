using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Will create a root between ini point and destination point.
//Then the person will keep that root loaded on memory so will use it again
//and again until a new route is needed

//the objects to be detected with the RayCast need to be on Layer 10

/* Second hardedst class ever created after Brain... bz it has a Recursive Algorith
 * that finds the route trhowing Raycast
 *
 * If wanna change the class is gonna be hard. Make sure to backup often
 *
 * Make sure the object to be detected by the router have the same collider as the gameObject.
 * If gameObject is rotated make sure collision to detect is squeare and not rotated
 */

public class Router : MonoBehaviour
{
    private SMe m = new SMe();

    //how far rounded center points will go towards end and origin
    private float step = 0.2f;//.3f

    private Vector3 _current;

    private Vector3 nextGoal;
    private Vector3 _ini;
    private Vector3 _fin;

    private List<CheckPoint> _checkPoints = new List<CheckPoint>();
    private Dir dir;

    private bool routePlotted;

    private string _currentObjHitMyIdOld;
    private Building currBuilding;
    private RaycastHit _currentHit;

    private Dir[] polyMap = new[] { Dir.NW, Dir.NE, Dir.SE, Dir.SW };
    private int[] polyMapXSing = new[] { -1, 1, 1, -1 };
    private int[] polyMapZSing = new[] { 1, 1, -1, -1 };

    public event EventHandler<EventArgs> RoutePlotted;

    //The hit Vector 3 position and contains name
    private VectorHit vectorHit;

    //anchor moved away from real anchor
    private Vector3 destinyAwayFromBuild;

    ///this is the other anchor on the side the hit landed on building
    private Vector3 anchorOnSideClosestToFin;

    //they are set in SetAnchors()
    private Vector3 closestAnchorToOrigin;

    private Vector3 closest1stAnchorToFin;
    private Vector3 closest2ndAnchorToFin;
    private Vector3 closest3rdAnchorToFin;

    private Person _person;
    private bool _isRouteReady;//will tell if route is ready to be used

    //created to put the recursive algo PlotRoute() being called from update. so dont block the game FrameRate
    private bool isToPlot;

    private TheRoute _theRoute;//this is the final product of the work of the router

    private string _originKey;//used to create the TheRoute obj
    private string _destinyKey;

    public bool IsRouteReady
    {
        get { return _isRouteReady; }
        set { _isRouteReady = value; }
    }

    public TheRoute TheRoute
    {
        get { return _theRoute; }
        set { _theRoute = value; }
    }

    private Vector3 _iniBehingDoor;
    private Vector3 _finBehingDoor;

    public Router()
    {
    }

    public Router(Vector3 iniSpawnPoint, Vector3 finSpawnPoint, Person person,
        Vector3 iniBehinDoor = new Vector3(), Vector3 finBehinDoor = new Vector3(),
        string originKey = "", string destinyKey = "")
    {
        _originKey = originKey;
        _destinyKey = destinyKey;

        _ini = iniSpawnPoint;
        _fin = finSpawnPoint;

        _iniBehingDoor = iniBehinDoor;
        _finBehingDoor = finBehinDoor;

        _person = person;

        Init();
    }

    private void Init()
    {
        AssignInitVars();
        ClearOldVars();

        //DebugDestroy();
        PlotRoute();
    }

    public void ClearOldVars()
    {
        IsRouteReady = false;

        if (TheRoute == null) { return; }
        TheRoute.BridgeKey = "";

        if (TheRoute.CheckPoints.Count > 0)
        {
            TheRoute.CheckPoints.Clear();
        }
    }

    private void AssignInitVars()
    {
        _current = _ini;
        nextGoal = _fin;
    }

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
        for (int i = 0; i < _checkPoints.Count; i++)
        {
            //_person.DebugList.Add(UVisHelp.CreateHelpers(_checkPoints[i].Point, Root.blueCube));
            //_person.DebugList.Add(UVisHelp.CreateText(_checkPoints[i].Point, i.ToString(), 55));
        }
        //DebugDestroy();
    }

    /// <summary>
    /// Will return the route to follow a Person from point ini to fin
    /// When this is call make sure u validated both points are not insede
    /// any building
    ///
    /// This is recursive is called and called again from its methods until _fin is reached
    /// </summary>
    private void PlotRoute()
    {
        if (_checkPoints.Count == 0) { AddFirst(); }
        vectorHit = FindGeneralBuildingOnMyWay(_current, _fin);

        if (vectorHit == null)
        {
            AddLastAndDone();
            return;
        }
        if (vectorHit.HitMyId != "")
        {
            SetCurrentBuilding();
            if (IsCollidingWithBuildingThatBlockNear(vectorHit, _current, _fin))
            {
                SetAnchors();
                HandleCollision();
            }
            else AddLastAndDone();
        }
        else AddLastAndDone();
    }

    /// <summary>
    /// The collision handler is called everytime the routerr collides
    /// </summary>
    private void HandleCollision()
    {
        if (IsWayClear(_current, closest1stAnchorToFin))
        {
            AddToRouteCurrent();
            // _person.DebugList.Add(UVisHelp.CreateText(_current, "1st", 60));
        }
        else if (IsWayClear(_current, closest2ndAnchorToFin))
        {
            AddToRouteCurrent();
            //_person.DebugList.Add(UVisHelp.CreateText(_current, "2nd", 60));
        }
        else if (IsWayClear(_current, closest3rdAnchorToFin))
        {
            AddToRouteCurrent();
            //_person.DebugList.Add(UVisHelp.CreateText(_current, "3rd", 60));
        }
        else
        {
            AddCurrentHitRoutine();
            //_person.DebugList.Add(UVisHelp.CreateText(_current, "4th", 60));
        }
        _currentObjHitMyIdOld = vectorHit.HitMyId;
        //so it makes it recursive. used to be calling directylt to PlotRoute();
        //but for performance was placed on Update() and set true below so it will be called

        isToPlot = true;

        //DestroyCurrentBuildingIfDummy();
    }

    private void AddCurrentHitRoutine()
    {
        AddCurrentHit();
        IsWayClear(_current, anchorOnSideClosestToFin);//so the other acnhor is selected to keep moving
        AddToRouteCurrent();
    }

    private void AddToRouteCurrent()
    {
        _current = destinyAwayFromBuild;
        //_person.DebugList.Add(UVisHelp.CreateHelpers( _current,Root.redCube));
        _checkPoints.Add(new CheckPoint(_current));
        CheckAndSmoothCornerIfNeeded();
    }

    private int hitCurrentAdded;

    /// <summary>
    /// Add Current hit point but first needs to move it away from buiding
    /// </summary>
    private void AddCurrentHit()
    {
        Dir[] dirsSimpleMap = new Dir[] { Dir.N, Dir.E, Dir.S, Dir.W };
        float moveBy = _person.PersonDim * 2;
        Vector3[] moveAway = new Vector3[]
        {
            new Vector3(0, 0, moveBy), new Vector3(moveBy, 0, 0),
            new Vector3(0, 0, -moveBy), new Vector3(-moveBy, 0, 0)
        };
        Dir buildWasHitOn = UDir.TellMeWhenHitLanded(currBuilding.Anchors, vectorHit.HitPoint);

        //is needed here so it moves foward along the side and not to the cloeset that coluld be accross the biuilding
        Vector3 otherAnchorOnSide = UPoly.FindOtherCornerOnSide(currBuilding.Anchors, buildWasHitOn, closestAnchorToOrigin);
        var sideAnchors = ReturnFirstAndLast(_fin, otherAnchorOnSide, closestAnchorToOrigin);
        anchorOnSideClosestToFin = sideAnchors[0];//the anchor on the side is closer to _fin

        for (int i = 0; i < dirsSimpleMap.Length; i++)
        {
            if (dirsSimpleMap[i] == buildWasHitOn)
            {
                //is moving the vector hit away given the direction
                Vector3 t = vectorHit.HitPoint + moveAway[i];
                t.y = m.IniTerr.MathCenter.y;//so is on same Y value

                t = Sanitize(t);
                //_person.DebugList.Add(UVisHelp.CreateText(t, hitCurrentAdded.ToString()));
                _checkPoints.Add(new CheckPoint(t));
                return;
            }
        }
    }

    /// <summary>
    /// Tells if a point is inside a buildig.
    ///
    /// Will loop until is out of a building
    ///
    /// This will give infinite loop if all building are really close to each other but
    /// tht should never happen
    /// </summary>
    private Vector3 Sanitize(Vector3 t)
    {
        while (IAmInsideABuildNow(t))
        {
            t = Vector3.MoveTowards(t, inside.transform.position, -0.05f);
            //UVisHelp.CreateHelpers(t, Root.blueCube);

            inside = null;
            return t;
        }

        //means is outside already
        Program.gameScene.AddToMainScreen("Fixed Router");
        return t;
    }

    private Structure inside;//the structure current point is inside

    /// <summary>
    /// Returns true if inside a Registro.Structures... sets 'inside' as the building the 't' is in
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private bool IAmInsideABuildNow(Vector3 t)
    {
        var pos = t;
        var p2 = new Vector2(pos.x, pos.z);
        var allBuild = BuildingPot.Control.Registro.Structures.ToArray();
        bool res = false;
        Rect rect = new Rect();
        for (int i = 0; i < allBuild.Length; i++)
        {
            rect = U2D.ReturnRectYInverted(U2D.FromPolyToRect(allBuild[i].Value.Anchors));
            if (rect.Contains(p2))
            {
                inside = allBuild[i].Value;
                res = true;
                Program.gameScene.AddToMainScreen("Router point inside building:" + allBuild[i].Key);

                //Debug.Log("Router point inside building:" + allBuild[i].Key);
                //throw new Exception("Router point inside building:" + allBuild[i].Key);
            }
        }
        return res;
    }

    /// <summary>
    /// Will find out if the last 3 points need smoothing. Will find the angle btw 3 beeing the 2nd point the middel
    /// and if less than 157.5 degres will smooth ny removing the second point and its place inserting two points
    /// one towards origin other closer destiny
    /// </summary>
    private void CheckAndSmoothCornerIfNeeded()
    {
        if (_checkPoints.Count < 3) { return; }//needs trhee at least

        Vector3 newPoint = _checkPoints[_checkPoints.Count - 1].Point;
        Vector3 centPoint = _checkPoints[_checkPoints.Count - 2].Point;
        Vector3 oldestPoint = _checkPoints[_checkPoints.Count - 3].Point;
        var angle = AngleFrom3PointsInDegrees(oldestPoint, centPoint, newPoint);
        //print(Mathf.Abs((float)angle) + " angle" + (TheRoute.Count - 1) + "." +
        //   (TheRoute.Count - 2) + "." + (TheRoute.Count - 3));
        if (Mathf.Abs((float)angle) > 22.5f) { SmoothCenterPoint(); }
    }

    /// <summary>
    /// Will smooth center point of last 3.
    /// Will remove middle one and then will inserrt 2 on its place
    /// </summary>
    private void SmoothCenterPoint()
    {
        Vector3 newPoint = _checkPoints[_checkPoints.Count - 1].Point;
        Vector3 centPoint = _checkPoints[_checkPoints.Count - 2].Point;
        Vector3 oldestPoint = _checkPoints[_checkPoints.Count - 3].Point;

        Vector3 centerToNew = Vector3.MoveTowards(centPoint, newPoint, step);
        Vector3 centerToOld = Vector3.MoveTowards(centPoint, oldestPoint, step);

        AddAndRemoveCenterPoint(centerToOld, centerToNew);
    }

    private void AddAndRemoveCenterPoint(Vector3 centerToOld, Vector3 centerToNew)
    {
        _checkPoints.Insert(_checkPoints.Count - 2, new CheckPoint(centerToOld));
        _checkPoints.Insert(_checkPoints.Count - 2, new CheckPoint(centerToNew));
        _checkPoints.RemoveAt(_checkPoints.Count - 2);
    }

    public double AngleFrom3PointsInDegrees(Vector3 oldPoint, Vector3 centerPoint, Vector3 newPoint)
    {
        double a = centerPoint.x - oldPoint.x;
        double b = centerPoint.z - oldPoint.z;
        double c = newPoint.x - centerPoint.x;
        double d = newPoint.z - centerPoint.z;

        double atanA = Math.Atan2(a, b);
        double atanB = Math.Atan2(c, d);

        return (atanA - atanB) * (-180 / Math.PI);
        // if Second line is counterclockwise from 1st line angle is
        // positive, else negative
    }

    /// <summary>
    /// Will tell u if te way btw 2 points is clear. Of buidllings or obj that block, like house,
    /// </summary>
    private bool IsWayClear(Vector3 origin, Vector3 destinyRealAnchor, Building buildP = null)
    {
        if (buildP == null) { buildP = currBuilding; }//by default will compare to cuurrentBuilding

        float howFar = _person.PersonDim / 4; //* 1.5f;
        destinyAwayFromBuild = MoveAnchorAwayFromBuild(destinyRealAnchor, buildP.Anchors, howFar, howFar);

        VectorHit vH = FindGeneralBuildingOnMyWay(origin, destinyAwayFromBuild);
        bool firstStep = false;
        if (vH == null) { firstStep = true; }
        else firstStep = !IsCollidingWithBuildingThatBlockNear(vH, origin, destinyAwayFromBuild);

        return firstStep;
    }

    private Vector3[] ReturnFirstAndLast(Vector3 stonePoint, Vector3 p1, Vector3 p2)
    {
        float dist1 = Vector3.Distance(stonePoint, p1);
        float dist2 = Vector3.Distance(stonePoint, p2);

        if (dist1 < dist2)
        {
            return new[] { p1, p2 };
        }
        return new[] { p2, p1 };
    }

    /// <summary>
    /// Set the the closest anchor to origin, then calls SetAnchorsPriorityToFin()
    /// </summary>
    private void SetAnchors()
    {
        //_person.DebugList.AddRange(UVisHelp.CreateHelpers(currBuilding.Anchors, Root.yellowCube));
        VectorM[] anchorOrdered = new VectorM[4];
        for (int i = 0; i < currBuilding.Anchors.Count; i++)
        {
            anchorOrdered[i] = new VectorM(currBuilding.Anchors[i], _current);
        }
        anchorOrdered = anchorOrdered.OrderBy(a => a.Distance).ToArray();
        closestAnchorToOrigin = anchorOrdered[0].Point;

        SetAnchorsPriorityToFin(anchorOrdered);
    }

    /// <summary>
    /// Ordering to be closer to _fin
    /// </summary>
    private void SetAnchorsPriorityToFin(VectorM[] anchorOrdered)
    {
        //now will need the distances towards the ordered end
        for (int i = 0; i < currBuilding.Anchors.Count; i++)
        {
            anchorOrdered[i] = new VectorM(currBuilding.Anchors[i], _fin);
        }
        anchorOrdered = anchorOrdered.OrderBy(a => a.Distance).ToArray();
        closest1stAnchorToFin = anchorOrdered[0].Point;
        closest2ndAnchorToFin = anchorOrdered[1].Point;
        closest3rdAnchorToFin = anchorOrdered[2].Point;
    }

    private void AddFirst()
    {
        _checkPoints.Add(new CheckPoint(_ini));
        //_person.DebugList.Add(UVisHelp.CreateHelpers(_ini, Root.yellowSphereHelp));
    }

    private void AddLastAndDone()
    {
        //_fin was reached
        _checkPoints.Add(new CheckPoint(_fin));
        //_person.DebugList.Add(UVisHelp.CreateHelpers(_fin, Root.yellowSphereHelp));
        CheckAndSmoothCornerIfNeeded();
        AddBehingDoors();

        EliminateDupConsecutive();
        //DebugRender();
        AddAnglesToRoute();
        IsRouteReady = true;

        EliminateHelperBuilds();

        CreateTheRouteObj();
        //DebugDestroy();
    }

    private void AddBehingDoors()
    {
        if (_iniBehingDoor != new Vector3())
        {
            _checkPoints.Insert(0, new CheckPoint(_iniBehingDoor));
        }
        if (_finBehingDoor != new Vector3())
        {
            _checkPoints.Add(new CheckPoint(_finBehingDoor));
        }
    }

    private void CreateTheRouteObj()
    {
        TheRoute = new TheRoute(_checkPoints, _originKey, _destinyKey);
    }

    /// <summary>
    /// Eliminates the buildings created for help when collide with Tree
    /// </summary>
    private void EliminateHelperBuilds()
    {
        for (int i = 0; i < helperBuilds.Count; i++)
        {
            helperBuilds[i].Destroy();
        }
        helperBuilds.Clear();
    }

    /// <summary>
    /// Will pass point by point and will find wht is the angle facing the next one
    /// </summary>
    private void AddAnglesToRoute()
    {
        GameScene.dummyBlue.transform.position = _checkPoints[0].Point;

        for (int i = 0; i < _checkPoints.Count - 1; i++)
        {
            GameScene.dummyBlue.transform.position = _checkPoints[i].Point;
            GameScene.dummyBlue.transform.LookAt(_checkPoints[i + 1].Point);
            _checkPoints[i].QuaterniRotation = GameScene.dummyBlue.transform.rotation;
        }

        //GameScene.dummyBlue.transform.position = new Vector3();
        GameScene.ResetDummyBlue();
    }

    /// <summary>
    /// Need to elimianted duplicated in rare ocasions will show duplicates
    /// </summary>
    private void EliminateDupConsecutive()
    {
        for (int i = 0; i < _checkPoints.Count - 1; i++)
        {
            if (UMath.nearEqualByDistance(_checkPoints[i].Point, _checkPoints[i + 1].Point, 0.001f))
            {
                _checkPoints.RemoveAt(i);
                i--;
            }
        }
    }

    private void DestroyCurrentBuildingIfDummy()
    {
        if (currBuilding != null && currBuilding.MyId.Contains("Dummy"))
        {
            currBuilding.Destroy();
            currBuilding = null;
        }
    }

    /// <summary>
    /// Set the current currBuilding
    /// </summary>
    private void SetCurrentBuilding()
    {
        if (BuildingPot.Control.Registro.AllBuilding.ContainsKey(vectorHit.HitMyId))
        { currBuilding = BuildingPot.Control.Registro.AllBuilding[vectorHit.HitMyId]; }

        if (currBuilding == null || currBuilding.MyId.Contains("Dummy"))
        {
            SetCurrentBuildingVariations();
        }

        if (currBuilding == null)
        {
            throw new Exception("currBuilding null:" + _person.MyId + ".from:" + _originKey + ".to:" + _destinyKey +
            ".hit:" + vectorHit.HitMyId);
            //RestartRouter();
        }

        currBuilding.GetAnchors();//will set the anchors for the currentBuilding in case was not done before
        //UVisHelp.CreateHelpers(currBuilding.GetAnchors(), Root.yellowCube);
    }

    private void RestartRouter()
    {
        //Debug.Log("Router Restarted:" + _person.MyId+"."+_originKey);
        GameScene.ScreenMsg = "Router Restarted:" + _person.MyId + "." + _originKey;
        isToPlot = false;
        Init();
    }

    private void SetCurrentBuildingVariations()
    {
        //called here bz I need to see if current hit obj belongs to a stockPile

        //bool isADiffBuild = SetCurrentBuildingOfType(vectorHit.HitMyId, H.StockPile);

        //if (currBuilding == null)
        //{isADiffBuild = SetCurrentBuildingOfType(vectorHit.HitMyId, H.BridgeTrail);}

        //if (currBuilding == null)
        //{ isADiffBuild = SetCurrentBuildingOfType(vectorHit.HitMyId, H.BridgeRoad); }

        //if (!isADiffBuild)
        //{
        SetCurrentBuildingTerraSpawn(vectorHit.HitMyId);
        //}
    }

    //if current Building was created on this class for Terra Spanwer for ex, then it needs to be destroyed
    private List<Building> helperBuilds = new List<Building>(); //this list collect the helpers to be destoyed later

    /// <summary>
    /// Created to see if we are colliding with a Teeraa Spawner
    /// </summary>
    private void SetCurrentBuildingTerraSpawn(string myIdP)
    {
        if (Program.gameScene.controllerMain.TerraSpawnController.AllRandomObjList.Contains(myIdP))
        {
            TerrainRamdonSpawner t = Program.gameScene.controllerMain.TerraSpawnController.AllRandomObjList[myIdP];

            CreateIfInADiffPos(t);
        }
    }

    private void CreateIfInADiffPos(TerrainRamdonSpawner t)
    {
        //var isNull = currBuilding == null;
        //var samePos = false;

        //if (!isNull)
        //{
        //    samePos = t.transform.position == currBuilding.transform.position;
        //}

        /////if is null or
        ///// is not null and the position is not the same as the current building
        //if (isNull || (!isNull && !samePos))
        //{
        currBuilding = Building.CreateBuild(Root.dummyBuild, new Vector3(), H.Dummy);
        currBuilding.transform.position = t.transform.position;

        currBuilding.PositionFixed = true;

        AddBoxColliderToCurrent(t);

        helperBuilds.Add(currBuilding);
        //}
    }

    public void AddBoxColliderToCurrent(General g)
    {
        currBuilding.gameObject.AddComponent<BoxCollider>();

        //BoxCollider b = currBuilding.gameObject.GetComponent<BoxCollider>();

        //b = g.GetComponent<BoxCollider>();

        //currBuilding.transform.rotation = g.transform.rotation;
    }

    /// <summary>
    /// True if is colliding with a building of same type 'HTypeP'...
    ///
    /// I need to do this bz StockPile doesnt have a big collider. has small ones instead
    /// Brdige has the same situation
    /// </summary>
    private bool SetCurrentBuildingOfType(string myIdP, H HTypeP)
    {
        if (_currentHit.transform == null)
        { return false; }

        var names = UString.ExtractNamesUntilGranpa(_currentHit.transform);

        for (int i = 0; i < names.Count; i++)
        {
            //will loop thrue names only one on a transform is registered that why i check only on the one
            //is contained on AllBuildings
            if (BuildingPot.Control.Registro.AllBuilding.ContainsKey(names[i]))
            {
                Building b = BuildingPot.Control.Registro.AllBuilding[names[i]];
                if (b.HType == HTypeP)
                {
                    currBuilding = BuildingPot.Control.Registro.AllBuilding[names[i]];
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Was created so we can descrimanate if a buildings is far or close enougnt that we care,
    /// if is far enought we dont carre
    ///
    /// If builidg is further than end then is false,, if builidg is closer than end
    /// then I called here IsCollidingWithBuildingThatBlock()
    /// </summary>
    private bool IsCollidingWithBuildingThatBlockNear(VectorHit vectorHit, Vector3 start, Vector3 end)
    {
        var distBtwStartAndEnd = Vector3.Distance(start, end);

        General g = null;
        if (BuildingPot.Control.Registro.AllBuilding.ContainsKey(vectorHit.HitMyId))
        { g = BuildingPot.Control.Registro.AllBuilding[vectorHit.HitMyId]; }
        if (g == null)
        {
            if (Program.gameScene.controllerMain.TerraSpawnController.AllRandomObjList.Contains(vectorHit.HitMyId))
            { g = Program.gameScene.controllerMain.TerraSpawnController.AllRandomObjList[vectorHit.HitMyId]; }
        }
        if (g == null) { return false; }

        //needs to compared with the hit point  of that building.. since if is a big building will cause bugg
        //since transform.position could be further than distBtwStartAndEnd...
        var distBtwStartAndBuild = Vector3.Distance(start, vectorHit.HitPoint);

        if (distBtwStartAndBuild > distBtwStartAndEnd)
        { return false; }

        return IsCollidingWithBuildingThatBlock(vectorHit.HitMyId, g);
    }

    /// <summary>
    /// Says true if is colliding with a biulding that doesnt let person pass trhu it.
    /// Like a house
    /// If currentRegFile is not null means we are colliding with something
    /// </summary>
    private bool IsCollidingWithBuildingThatBlock(string myIdP, General gen)
    {
        if (gen.Category == Ca.Structure || gen.Category == Ca.Shore
            || gen.HType == H.StockPile || gen.HType == H.BridgeTrail || gen.HType == H.BridgeRoad
            || gen.Category == Ca.Spawn)
        { return true; }
        return false;
    }

    /// <summary>
    /// Move an vertex of a poly aways from the Poly dependeing on which one is .
    ///
    /// Needs to be called bz we need to push person away from building other wise
    /// when checking for collisions is always in bz algorith will do it on the edge
    /// So wehen i call this i push the edge away from Building
    /// </summary>
    private Vector3 MoveAnchorAwayFromBuild(Vector3 anchorToMoveAndRet, List<Vector3> anchors, float inX, float inZ)
    {
        int anchorIndex = -1;
        for (int i = 0; i < anchors.Count; i++)
        {
            if (UMath.nearEqualByDistance(anchors[i], anchorToMoveAndRet, 0.01f))
            { anchorIndex = i; }
        }

        //this is to addres when the Anchor was sanitize... will nt be tht close to an anchor anymore
        //and wont find which one is closer ... therefore will return the save value pased
        if (anchorIndex == -1)
        {
            return anchorToMoveAndRet;
        }

        float newX = anchorToMoveAndRet.x + inX * polyMapXSing[anchorIndex];
        float newZ = anchorToMoveAndRet.z + inZ * polyMapZSing[anchorIndex];

        return new Vector3(newX, anchorToMoveAndRet.y, newZ);
    }

    public void OnRoutePlotted(EventArgs e)
    {
        if (RoutePlotted != null)
        {
            RoutePlotted(this, e);
        }
    }

    /// <summary>
    /// Will find the building on its way btw ini and end param
    /// </summary>
    public VectorHit FindGeneralBuildingOnMyWay(Vector3 ini, Vector3 end)
    {
        ini.y += 0.2f;//so we are not in flloor raso
        end.y += 0.2f;

        GameScene.dummyRed.transform.position = ini;

        GameScene.dummyRed.transform.LookAt(end);
        //_person.DebugList.Add(dummyRayCaster);//so it gets deleted later

        // This would cast rays only against colliders in layer 10.
        int layerMask = 1 << 10;

        Debug.DrawRay(GameScene.dummyRed.transform.position, GameScene.dummyRed.transform.forward * 33, Color.yellow, 5f);
        RaycastHit hit;
        if (Physics.Raycast(GameScene.dummyRed.transform.position,
            GameScene.dummyRed.transform.TransformDirection(Vector3.forward) * 100, out hit, Mathf.Infinity, layerMask))
        {
            _currentHit = hit;
        }
        else
        {
            GameScene.dummyRed.transform.position = new Vector3();
            return null;
        }
        GameScene.dummyRed.transform.position = new Vector3();
        return new VectorHit(hit.transform.name, hit.point);
    }

    public void Update()
    {
        if (isToPlot)
        {
            isToPlot = false;
            PlotRoute();
        }
    }
}

public class CheckPoint
{
    private Vector3 _point;
    private Quaternion _quaterniRotation;//this is the rotation needed to be in the exact good position
    private Quaternion _quaterniRotationInv;
    private bool _inverseWasSet;
    private float _speed = 1f;

    public Vector3 Point
    {
        get { return _point; }
        set { _point = value; }
    }

    public Quaternion QuaterniRotation
    {
        get { return _quaterniRotation; }
        set { _quaterniRotation = value; }
    }

    public Quaternion QuaterniRotationInv
    {
        get { return _quaterniRotationInv; }
        set { _quaterniRotationInv = value; }
    }

    public bool InverseWasSet
    {
        get { return _inverseWasSet; }
        set { _inverseWasSet = value; }
    }

    /// <summary>
    /// The speed of a checkPoint. Ways are faster than grass
    /// So far not used in this version as Apr11 2016
    /// </summary>
    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    public CheckPoint()
    {
    }

    public CheckPoint(Vector3 point)
    {
        Point = point;
    }

    /// <summary>
    /// This one is use to set the Speed of the CheckPoint right away
    /// </summary>
    /// <param name="point"></param>
    /// <param name="crystalType"></param>
    public CheckPoint(Vector3 point, H crystalType)
    {
        Point = point;
        Speed = SetSpeed(crystalType);
    }

    /// <summary>
    /// Sets the speed of the CheckPoint base on the Type of Crystal we are using
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    private float SetSpeed(H _type)
    {
        if (_type == H.Way3)//best way
        {
            return 1.4f;
        }
        else if (_type == H.Way2)
        {
            return 1.2f;
        }
        else if (_type == H.Way1)//worst way
        {
            return 1.1f;
        }

        return 1f;
    }
}

public class VectorM
{
    private Vector3 _point;
    private float _distance;
    private string _locMyId;
    private H _hType;

    public VectorM()
    {
    }

    public VectorM(Vector3 pos, Vector3 comparationPoint)
    {
        Point = pos;
        Distance = Vector3.Distance(pos, comparationPoint);
    }

    public VectorM(Vector3 pos, Vector3 comparationPoint, string locMyId)
    {
        Point = pos;
        Distance = Vector3.Distance(pos, comparationPoint);
        _locMyId = locMyId;
    }

    public VectorM(Vector3 pos, Vector3 comparationPoint, string locMyId, H hType)
    {
        _hType = hType;
        Point = pos;
        Distance = Vector3.Distance(pos, comparationPoint);
        _locMyId = locMyId;
    }

    public Vector3 Point
    {
        get { return _point; }
        set { _point = value; }
    }

    //used mainly to order data strauctures
    public float Distance
    {
        get { return _distance; }
        set { _distance = value; }
    }

    public string LocMyId
    {
        get { return _locMyId; }
        set { _locMyId = value; }
    }

    public H HType
    {
        get { return _hType; }
        set { _hType = value; }
    }
}

public class VectorHit
{
    private Vector3 _hitPoint;
    private string _hitMyId;

    public VectorHit(string hitMyId, Vector3 hitPoint)
    {
        HitPoint = hitPoint;
        HitMyId = hitMyId;
    }

    public Vector3 HitPoint
    {
        get { return _hitPoint; }
        set { _hitPoint = value; }
    }

    public string HitMyId
    {
        get { return _hitMyId; }
        set { _hitMyId = value; }
    }
}