using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Way : Building
{
    #region Fields
    private int _wideSquare = 1;//how big  the submesh Cells are 1x1(1sumbesh poly) , 2x2 (4sumbesh poly) 

    //first corner where mouse is clicked
    Vector2 firstCorner = new Vector2();
    Vector2 secondCorner = new Vector2();

    protected float xLocStep ;
    protected float zLocStep;

    List<Vector3> onScreenPoly = new List<Vector3>();//the poly being dragged on screen, on terrain

    protected Vector3 _firstWayPoint = new Vector3();//the first Vector3 when draggin a way
    protected Vector3 _secondWayPoint = new Vector3();//the 2nd Vector3 when draggin a way

    protected List<Vector3> _subMeshPathVertic = new List<Vector3>();//the closest subMeshVert that are dragged
    protected List<Vector3> _subMeshPathHor = new List<Vector3>();//the closest subMeshVert that are dragged

    protected List<List<Vector3>> _planesDimVertic = new List<List<Vector3>>();//hold the dimensions of the planes that are the Way 
    protected List<List<Vector3>> _planesDimHor = new List<List<Vector3>>();//hold the dimensions of the planes that are the Way

    protected List<PreviewWay> _prevWayVertic = new List<PreviewWay>();
    protected List<PreviewWay> _prevWayHor = new List<PreviewWay>();

    //the generated paths
    List<Vector3> _verticPath = new List<Vector3>();
    List<Vector3> _horPath = new List<Vector3>();

    //the path we are always showing 
    protected List<Vector3> _verticPathNew = new List<Vector3>();
    protected List<Vector3> _horPathNew = new List<Vector3>();

    protected List<General> debuger = new List<General>();
    protected List<General> debuger2 = new List<General>();
    public static Dir _dir;

    private List<Vector3> _boundsVertic = new List<Vector3>();
    private List<Vector3> _boundsHoriz = new List<Vector3>();

    private int _maxStepsWay;//how many units can build in a way
    private bool _isWayColliding = false;
    private bool _isWayEven;
    private bool _isWayAboveWater;
    private bool _isWayOK;
    private bool _isBridgeTallEnough;
    private int _unevenTilesCount;//uneven Way tiles count. changed on Preview Way

    private string _previewRoot;//this is the preview object root has the previewWay.cs attached
    //in the PreviewWay this is the radues of the sphere that search thru to see what was collided when was fixed to terrain
    private float _previewCellRadius;

    private float _planeScale;//create Planes scaled up amount
    protected H _dominantSide = H.None;//the longer side of a bridge
    private H oldDominantSide;//USE TO toggle bix boxes preview

    //destroy cool variables
    protected float destroyCoolSpeed = 30f;
    protected float destroyCoolTime = 0.75f;

    #endregion

    #region Prop
    public List<Vector3> OnScreenPoly
    {
        get { return onScreenPoly; }
    }

    public List<Vector3> BoundsVertic
    {
        get { return _boundsVertic; }
        set { _boundsVertic = value; }
    }

    public List<Vector3> BoundsHoriz
    {
        get { return _boundsHoriz; }
        set { _boundsHoriz = value; }
    }

    public bool IsWayEven
    {
        get { return _isWayEven; }
        set { _isWayEven = value; }
    }

    public bool IsWayAboveWater
    {
        get { return _isWayAboveWater; }
        set { _isWayAboveWater = value; }
    }

    public bool IsWayOk
    {
        get { return _isWayOK; }
        set { _isWayOK = value; }
    }

    public bool IsWayColliding
    {
        get { return _isWayColliding; }
        set { _isWayColliding = value; }
    }

    public int WideSquare
    {
        get { return _wideSquare; }
        set { _wideSquare = value; }
    }

    public string PreviewRoot
    {
        get { return _previewRoot; }
        set { _previewRoot = value; }
    }

    public float PreviewCellRadius
    {
        get { return _previewCellRadius; }
        set { _previewCellRadius = value; }
    }

    public float PlaneScale
    {
        get { return _planeScale; }
        set { _planeScale = value; }
    }

    public int MaxStepsWay
    {
        get { return _maxStepsWay; }
        set { _maxStepsWay = value; }
    }

    //this will be set from the previewWay.cs
    public int UnevenTilesCount
    {
        get { return _unevenTilesCount; }
        set { _unevenTilesCount = value; }
    }

    #endregion

    BigBoxPrev verticBigBox;
    BigBoxPrev horizBigBox;

    public BigBoxPrev VerticBigBox
    {
        get { return verticBigBox; }
        set { verticBigBox = value; }
    }

    public BigBoxPrev HorizBigBox
    {
        get { return horizBigBox; }
        set { horizBigBox = value; }
    }

    static public Way CreateWayObj(string root, Vector3 origen, H hType, string previewObjRoot, string name = "", Transform container = null,
        int wideSquare = 1, float radius = 2f, float planeScale = 0.03f, int maxStepsWay = 50, string materialKey = "",
        bool isLoadingFromFile = false)
    {
        WAKEUP = true;
        Way obj = null;
        obj = (Way)Resources.Load(root, typeof(Way));
        obj = (Way)Instantiate(obj, origen, Quaternion.identity);
        obj.HType = hType;
        obj.MyId = obj.Rename(obj.transform.name, obj.Id, obj.HType, name);
        obj.transform.name = obj.MyId;
        
        obj.WideSquare = wideSquare;
        obj.PreviewRoot = previewObjRoot;//this is the obj that does the preview
        obj.PreviewCellRadius = radius;
        obj.PlaneScale = planeScale;
        obj.MaxStepsWay = maxStepsWay;

        obj.ClosestSubMeshVert = origen;
        if (container != null) { obj.transform.SetParent( container); }
        obj.MaterialKey = materialKey;

        obj.IsLoadingFromFile = isLoadingFromFile;

        return obj;
    }

    #region Big Boxes Prev

    void UpdateBigBoxesPrev()
    {
        InitializeBigBoxPrev();

        float diffYVertic = UMath.ReturnDiffBetwMaxAndMin(_verticPathNew, H.Y);
        float diffYHoriz = UMath.ReturnDiffBetwMaxAndMin(_horPathNew, H.Y);
        float biggestDiff = UMath.ReturnMax(diffYVertic, diffYHoriz);

        List<float> yS = UList.ReturnAxisList(_verticPathNew, H.Y);
        yS.AddRange(UList.ReturnAxisList(_horPathNew, H.Y));
        float maxY = UMath.ReturnMax(yS);

        if (_dominantSide == H.Vertic)
        {
            var locVertBound = MakeListYVal(BoundsVertic, maxY);
            verticBigBox.UpdatePos(locVertBound, biggestDiff + 0.5f);
            verticBigBox.CheckAndSwitchColor(_isWayOK);
        }
        else if (_dominantSide == H.Horiz)
        {
            var locHorBound = MakeListYVal(BoundsHoriz, maxY);
            horizBigBox.UpdatePos(locHorBound, biggestDiff + 0.5f);
            horizBigBox.CheckAndSwitchColor(_isWayOK);
        }
        //this is for all but bridges and DraggableSquare. Dominant Side here is None
        else if (_dominantSide == H.None && !HType.ToString().Contains("Bridge") && Category != Ca.DraggableSquare)
        {
            UpdateBigBoxesPrevForAllButBridges(maxY, biggestDiff);
        }

        TogglePrevBigBoxesVisible();
    }

    void UpdateBigBoxesPrevForAllButBridges(float maxY, float biggestDiff)
    {
        var locHorBound = MakeListYVal(BoundsHoriz, maxY);
        horizBigBox.UpdatePos(locHorBound, biggestDiff + 0.5f, corretMinimuScaleOnBigBoxP: true);
        horizBigBox.CheckAndSwitchColor(_isWayOK);

        var locVertBound = MakeListYVal(BoundsVertic, maxY);
        verticBigBox.UpdatePos(locVertBound, biggestDiff + 0.5f, corretMinimuScaleOnBigBoxP: true);
        verticBigBox.CheckAndSwitchColor(_isWayOK);
    }

    void TogglePrevBigBoxesVisible()
    {
        if (_dominantSide == H.Vertic && oldDominantSide != _dominantSide)
        {
            verticBigBox.PlaneGeometry.transform.GetComponent<Renderer>().enabled = true;
            horizBigBox.PlaneGeometry.transform.GetComponent<Renderer>().enabled = false;
            oldDominantSide = _dominantSide;
        }
        else if (_dominantSide == H.Horiz && oldDominantSide != _dominantSide)
        {
            verticBigBox.PlaneGeometry.transform.GetComponent<Renderer>().enabled = false;
            horizBigBox.PlaneGeometry.transform.GetComponent<Renderer>().enabled = true;
            oldDominantSide = _dominantSide;
        }
    }

    List<Vector3> MakeListYVal(List<Vector3> list, float newY)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Vector3 t = list[i];
            t.y = newY;
            list[i] = t;
        }
        return list;
    }

    void InitializeBigBoxPrev()
    {
        if (verticBigBox == null || horizBigBox == null)
        {
            verticBigBox = (BigBoxPrev)CreatePlane.CreatePlan(Root.bigBoxPrev, Root.matGreenSel2);
            verticBigBox.transform.name = "verticBigBox:" + MyId;
            horizBigBox = (BigBoxPrev)CreatePlane.CreatePlan(Root.bigBoxPrev, Root.matGreenSel2);
            horizBigBox.transform.name = "horizBigBox:" + MyId;
        }
    }

    #endregion

    void UpdateVerticBound()
    {
        if (_prevWayVertic.Count>0)
        _boundsVertic = FindBounds(_prevWayVertic[0].GetComponent<Collider>().bounds.min, _prevWayVertic[_prevWayVertic.Count - 1].GetComponent<Collider>().bounds.max);
        //ClearDebuger();
        //debuger2 = UVisHelp.CreateHelpers(_boundsVertic, Root.blueSphereHelp);
    }

    void UpdateHorizBound()
    {
        if (_prevWayHor.Count > 0)
        _boundsHoriz = FindBounds(_prevWayHor[0].GetComponent<Collider>().bounds.min, _prevWayHor[_prevWayHor.Count - 1].GetComponent<Collider>().bounds.max);
        //debuger = UVisHelp.CreateHelpers(_boundsHoriz, Root.yellowSphereHelp);
    }

    public void ClearPrevWay()
    {
        DestroyListGeneralObj(_prevWayVertic);
        DestroyListGeneralObj(_prevWayHor);
    }

    public void DestroyBigPrevBoxes()
    {
        if (VerticBigBox != null)
        {
            VerticBigBox.DestroyCoolMoveFirst(H.Y, destroyCoolSpeed, destroyCoolTime);
        }
        if (HorizBigBox != null)
        {
            HorizBigBox.DestroyCoolMoveFirst(H.Y, destroyCoolSpeed, destroyCoolTime);
        }
    }

    public void DestroyWayFromUserRightClick()
    {
        DestroyBigPrevBoxes();
        ClearPrevWay();
        _firstWayPoint = new Vector3();
        Destroy();
    }

    /// <summary>
    /// Destroys the list passed as arg
    /// </summary>
    void DestroyListGeneralObj(List<PreviewWay> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] != null)
            {
                list[i].Destroy();
            }
        }
        list.Clear();
    }

    protected void ClearDebuger()
    {
        for (int i = 0; i < debuger.Count; i++)
        {
            debuger[i].Destroy();
        }
        for (int i = 0; i < debuger2.Count; i++)
        {
            debuger2[i].Destroy();
        }
       
        debuger.Clear();
        debuger2.Clear();
    }

    /// <summary>
    /// Creates 2 vertcal and hor path will include the first and last point on the path too 
    /// </summary>
    void RetListOfWay(List<Vector3> selPoly, Dir dir)
    {
        //we add _firstWayPoint and _secondWayPoint so the first and last point when 
        //creating a way are always seen
        if (dir == Dir.SWtoNE  )
        {
            _verticPath = RetOneDirectionList(selPoly[3], selPoly[0], xLocStep, zLocStep);
            _horPath =  RetOneDirectionList(selPoly[1], selPoly[0], xLocStep, zLocStep);

            _verticPath.Insert(0, _firstWayPoint);
            _horPath.Add(_secondWayPoint);
        }
        else if (dir == Dir.SEtoNW )
        {
            _horPath = RetOneDirectionList(selPoly[2], selPoly[3], xLocStep, zLocStep);
            _verticPath =RetOneDirectionList(selPoly[0], selPoly[3], xLocStep, zLocStep);

            _horPath.Add(_firstWayPoint);
            _verticPath.Add(_secondWayPoint);
        }
        else if (dir == Dir.NEtoSW)
        {
            _verticPath = RetOneDirectionList(selPoly[3], selPoly[0], xLocStep, zLocStep);
            _horPath = RetOneDirectionList(selPoly[1], selPoly[0], xLocStep, zLocStep);

            _horPath.Add(_firstWayPoint);
            _verticPath.Insert(0, _secondWayPoint);
            
            //this is to correct a bugg where the path donest have the last
            //one there all the time
            _verticPath.Add(_horPath[0]);
        }
        else if(dir == Dir.NWtoSE)
        {
            _horPath = RetOneDirectionList(selPoly[2], selPoly[3], xLocStep, zLocStep);
            _verticPath = RetOneDirectionList(selPoly[0], selPoly[3], xLocStep, zLocStep);

            _horPath.Add(_secondWayPoint);
            _verticPath.Add(_firstWayPoint);
        }
    }

    /// <summary>
    /// Will return the list of subpoint from start to end ... 
    /// by default will use substep from MeshManager
    /// </summary>
    /// <param name="subStepX">substep in X, if is not provided will use substep from MeshManager</param>
    /// <param name="subStepZ">substep in Z, if is not provided will use substep from MeshManager</param>
    /// <returns></returns>
    List<Vector3> RetOneDirectionList(Vector3 start, Vector3 end, float subStepX , float subStepZ )
    {
        float epsilon = 0.001f;
        //find the common axis.
        H axis = RetCommonAxis(start, end, epsilon);
        //then we invert the axis bz that the one I will work on. The one is not common 
        axis = invertAxis(axis);

        float min = UMath.ReturnMinFromVector3(start, end, axis);
        float max = UMath.ReturnMaxFromVector3(start, end, axis);
        float subStep = Mathf.Abs(RetSubStep(axis, subStepX, subStepZ));

        //this is set only for X and Z
        List<Vector3> res = new List<Vector3>();
        res = ReturnFromMinToMax(axis, subStep, min, max, start);
        return res;
    }

    /// <summary>
    /// Returns a list from Min to Max of Vector3 positions.
    /// Used for create the Vertic and Horiz path
    /// </summary>
    List<Vector3> ReturnFromMinToMax(H axis, float subStep, float min, float max, Vector3 start)
    {
        List<Vector3> res = new List<Vector3>();
        if (axis == H.X)
        {
            for (float i = min; i < max; i += subStep * _wideSquare)
            {
                 res.Add(m.Vertex.BuildVertexWithXandZ(i, start.z));
            }
        }
        else if (axis == H.Z)
        {
            for (float i = min; i < max; i += subStep * _wideSquare)
            {
                res.Add(m.Vertex.BuildVertexWithXandZ(start.x, i));
            }
        }
        return res;
    }
         
    /// <summary>
    /// If the substeps passed are zero then we will use substep from SubMesh.
    /// Otherwise will be the one provide
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="subStepX"></param>
    /// <param name="subStepZ"></param>
    /// <returns></returns>
    float RetSubStep(H axis, float subStepX, float subStepZ)
    {
        float subStep = 0;
        if (subStepX == 0 && subStepZ == 0)
        {
            if (axis == H.X)
            {
                subStep = m.SubDivide.XSubStep;
            }
            else if (axis == H.Z)
            {
                subStep = Mathf.Abs(m.SubDivide.ZSubStep);
            }
        }
        else
        {
            if (axis == H.X)
            {
                subStep = subStepX;
            }
            else if (axis == H.Z)
            {
                subStep = subStepZ;
            }
        }
        return subStep;
    }

    /// <summary>
    /// Is use to invert the axis only btw X and Z..
    /// is needed to use the other axis is not common when creating Ret One Dire List
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    H invertAxis(H current)
    {
        H res = H.None;
        if (current == H.X)
        {
            res= H.Z;
        }
        if (current == H.Z)
        {
            res = H.X;
        }
        return res;
    }

    /// <summary>
    /// Returns the axis in where the Vector3 are in same position
    /// Doesnt evaluate Y by default
    /// </summary>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <param name="evalY">Will eval Y if is true</param>
    /// <returns></returns>
    H RetCommonAxis(Vector3 one, Vector3 two, float epsilon, bool evalY = false)
    {
        H res = H.None;
        
        bool x = UMath.nearlyEqual(one.x, two.x, epsilon);
        bool y = UMath.nearlyEqual(one.y, two.y, epsilon);
        bool z = UMath.nearlyEqual(one.z, two.z, epsilon);

        if (x){res = H.X;}
        else if (y && evalY) { res = H.Y; }
        else if (z) { res = H.Z; }
        return res;
    }

    /// <summary>
    /// The Drag action of the way
    /// </summary>
    public void Drag()
    {
        if (UMath.nearEqualByDistance(ClosestVertOld, ClosestSubMeshVert, 0.01f)) { return; }
        CreateWay();
        ClearPrevWay();

        if (onScreenPoly.Count == 0) { return; }
       
        //this is what creaetes both ways
        RetListOfWay(onScreenPoly, _dir);

        //this will remove duplicates and will in a road if the tile is overlapping other
        //will push it a bit down so in it renders fine
        //if is a bridge is not needed since we do exaclty this on Bridge.cs and brige needs
        //all tiles in the same height 
        if(HType != H.BridgeRoad && HType != H.BridgeTrail){ RemoveDupRectifyPath();}

        //if paths are less than the max amt allowed we will check coll and will update preview pos
        if (_verticPath.Count + _horPath.Count < MaxStepsWay)
        {
            CollideCheckRoutineUpdatePrev(_verticPath, _horPath);
            _verticPathNew = _verticPath;
            _horPathNew = _horPath;
        }
        else if (_verticPath.Count + _horPath.Count >= MaxStepsWay)
        {
            CollideCheckRoutineUpdatePrev(_verticPathNew, _horPathNew );
        }
        base.UpdateClosestVertexAndOld();//so ClosestSubMeshVert and ClosestVertOld are taken care of

        _isWayOK = CheckEvenTerraCollWater();
        UpdateBigBoxesPrev();
    }

    /// <summary>
    /// This is here to remove any duplicated point on the path 
    /// </summary>
    void RemoveDupRectifyPath()
    {
        _verticPath = _verticPath.OrderBy(a => a.z).ToList();
        _horPath = _horPath.OrderBy(a => a.x).ToList();

        _verticPath = RemoveDuplicatesFromOrderedList(_verticPath, 0.1f);
        _horPath = RemoveDuplicatesFromOrderedList(_horPath, 0.1f);
    }

    /// <summary>
    /// remove the overlapping corner in the paths checks if any corners is close and will remove it from 
    /// _hor path. This is here so the overlapping corners are gone
    /// </summary>
    /// <param name="minDiff"></param>
    protected void RemoveOverLapCorner(float minDiff)
    {
        if (HType == H.BridgeRoad || HType == H.BridgeTrail) { return;}
        float distance = Vector3.Distance(_verticPath[0], _horPath[0]);
        if (distance < minDiff)
        {
            _horPath.RemoveAt(0);
            return;
        }

        distance = Vector3.Distance(_verticPath[0], _horPath[_horPath.Count - 1]);
        if (distance < minDiff)
        {
            _horPath.RemoveAt(_horPath.Count - 1);
            return;
        }

        distance = Vector3.Distance(_verticPath[_verticPath.Count - 1], _horPath[0]);
        if (distance < minDiff)
        {
            _horPath.RemoveAt(0);
            return;
        }

        distance = Vector3.Distance(_verticPath[_verticPath.Count - 1], _horPath[_horPath.Count - 1]);
        if (distance < minDiff)
        {
            _horPath.RemoveAt(_horPath.Count - 1);
        }
    }

    /// <summary>
    /// Remove duplicatss value from a ordered list. If the list 
    /// is ordered says in X, Y or Z axis will remove duplicates
    /// </summary>
    /// <param name="minDiff">min Diff that a point can have</param>
    /// <returns>A list with out the duplicates</returns>
    List<Vector3> RemoveDuplicatesFromOrderedList(List<Vector3> list, float minDiff)
    {
        for (int i = 1; i < list.Count; i++)
        {
            float distance = Vector3.Distance(list[i - 1], list[i]);
            if (distance < minDiff)
            {
                list.RemoveAt(i);
                i--;
            }
        }
        return list;
    }

    /// <summary>
    /// returns True if both _verticPathNew and _horPathNew are even otherwise false
    /// </summary>
    public override bool CheckIfIsEvenRoutine()
    {
        bool even = false;
        //puts both path toghetehr in one list bz had a bugg before
        //that will be always even bz one path will have only one
        //tile in diff Y but bz was individial will be still fine
        //thats why both must me evaluated toghether
        List<Vector3> bothPaths = new List<Vector3>();
        bothPaths.AddRange(_verticPathNew);
        bothPaths.AddRange(_horPathNew);

        if (bothPaths.Count > 0)
        {
            even = AreAllPointsEven(bothPaths);
        }

        //if both are even then we will check if each anchor of each bound is even 
        //and if is not a DraggableSquare...
        if (even && Category != Ca.DraggableSquare)
        {
           even = CheckIfAnchorsAreEven();
        }

        return even;
    }

    /// <summary>
    /// Will find anchor for vert and hor bound and then will check if they are even
    /// </summary>
    bool CheckIfAnchorsAreEven()
    {
        var anchorsVertic = new List<Vector3>();
        var anchorsHor = new List<Vector3>();
        if(BoundsVertic !=null){anchorsVertic = FindAnchors(BoundsVertic);}
        if(BoundsHoriz !=null){anchorsHor = FindAnchors(BoundsHoriz);}
        
        List<Vector3> allAnchorPoints = new List<Vector3>();
        allAnchorPoints.AddRange(anchorsVertic);
        allAnchorPoints.AddRange(anchorsHor);
        if (allAnchorPoints.Count > 0)
        {
            return AreAllPointsEven(allAnchorPoints);
        }
        return false;
    }

    /// <summary>
    /// If the minimun of the both path is higher than _minHeightToSpawn is true
    /// </summary>
    bool FindIFWayAboveWater()
    {
        bool res = false;
        float minYVert = 0;
        float minYHor = 0;
        if (_verticPathNew.Count > 0)
        {
            var vertYs = UList.ReturnAxisList(_verticPathNew, H.Y);
            minYVert = UMath.ReturnMinimum(vertYs);
            if (minYVert > _minHeightToSpawn) { res = true; }
        }
        if (_horPathNew.Count > 0)
        {
            var horYs = UList.ReturnAxisList(_horPathNew, H.Y);
            minYHor = UMath.ReturnMinimum(horYs);
            if (minYHor > _minHeightToSpawn) { res = true; }
        }
        if (_verticPathNew.Count > 0 && _horPathNew.Count > 0)
        {
            var min = UMath.ReturnMinimum(minYVert, minYHor);
            if (min > _minHeightToSpawn) { res = true; }
        }

        return res;
    }

    /// <summary>
    /// Checks if Terrain below the build _isEven or _isColliding, and is tall enough
    /// </summary>
    /// <returns>True if terrain is even, not colliding and not on the sea. if is a bridge will evealuate
    /// if is the bridge tall enoguth</returns>
    public override bool CheckEvenTerraCollWater()
    {
        bool res = false;
        _isWayAboveWater = FindIFWayAboveWater();

        //if is not a Bridge ... 
        if (!HType.ToString().Contains(H.Bridge.ToString()))
        {
            _isWayEven = CheckIfIsEvenRoutine();
            res = _isWayEven && !IsWayColliding && _isWayAboveWater ;
        }
        //if is a bridge
        else if (HType.ToString().Contains(H.Bridge.ToString()))
        {
            _isWayEven = IsBridgeEven();
            //if (_isWayEven) { _isWayEven = CheckIfAllRealVerticesUnderneathBridgeAreEven(); }
            //CheckIfAllRealVerticesUnderneathBridgeAreEven();
            if (_isWayEven)
            {
                _isBridgeTallEnough = IsBrideTallEnought(3f);
            }
            res = _isWayEven && !IsWayColliding && _isWayAboveWater && _isBridgeTallEnough ;
        }

        //print("way: _even:" + _isWayEven+"._isColl: "+IsWayColliding + "._isAboveWater: "+ _isWayAboveWater +
        //   "._isBridgTall: " + _isBridgeTallEnough + "._areTheTilesEven:"+AreAllTilesEven());

        return res;
    }

    /// <summary>
    /// Collide check routine and update the previews lists too
    /// </summary>
    void CollideCheckRoutineUpdatePrev(List<Vector3> _verticPathP, List<Vector3> _horPathP)
    {
        if (!HType.ToString().Contains("Bridge"))
        {
            CollideUpdatePrevForAllWays(_verticPathP, _horPathP);
        }
        else CollideUpdatePrevForBridges( _verticPathP, _horPathP);
    }

    /// <summary>
    /// Collision with the rest of the obj and bounds update for only bridges)
    /// </summary>
    private void CollideUpdatePrevForBridges(List<Vector3> _verticPathP, List<Vector3> _horPathP)
    {
        if (_dominantSide == H.Vertic)
        {
            _prevWayVertic = UVisHelp.CreatePreviewWay(_verticPathP, _previewRoot, _previewCellRadius);

            UpdateVerticBound();
            IsWayColliding = CheckIfBoundsCollide(_boundsVertic);
            
            BoundsHoriz = null;
        }
        else if (_dominantSide == H.Horiz)
        {
             _prevWayHor = UVisHelp.CreatePreviewWay(_horPathP, _previewRoot, _previewCellRadius);

             UpdateHorizBound();
            IsWayColliding = CheckIfBoundsCollide(_boundsHoriz);
            
            BoundsVertic = null;
        }
    }

    /// <summary>
    /// Collision with the rest of the obj and bounds update for all types of ways (not including bridges)
    /// </summary>
    void CollideUpdatePrevForAllWays(List<Vector3> _verticPathP, List<Vector3> _horPathP)
    {
        if (_verticPathP.Count > 0)
        {
            //if is a DraggableSquare will not create the prev way 
            if (Category != Ca.DraggableSquare)
            {
                _prevWayVertic = UVisHelp.CreatePreviewWay(_verticPathP, _previewRoot, _previewCellRadius);
            }
            UpdateVerticBound();
            IsWayColliding = CheckIfBoundsCollide(_boundsVertic);
        }
        if (_horPathP.Count > 0)
        {
            //if is a DraggableSquare will not create the prev way 
            if (Category != Ca.DraggableSquare)
            {
                _prevWayHor = UVisHelp.CreatePreviewWay(_horPathP, _previewRoot, _previewCellRadius);
            }
            UpdateHorizBound();

            if (!IsWayColliding)
            {
                IsWayColliding = CheckIfBoundsCollide(_boundsHoriz);
            }
        }
    }
  
    /// <summary>
    /// Will return true if paramenter bounds collide with any existing bound in the Registro
    /// </summary>
    bool CheckIfBoundsCollide(List<Vector3> bounds)
    {
        if(bounds.Count > 0)
        if (BuildingPot.Control.Registro.IsCollidingWithExisting(bounds))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Find the closest submesh Vert to the list and returns them 
    /// </summary>
    protected Vector3 FindSubMeshVert(Vector3 current)
    {
        Vector3 res = new Vector3();
        res= m.Vertex.FindClosestVertex(current, Program.gameScene.controllerMain.MeshController.AllVertexs.ToArray(), 0.0001f);
        //if is a bridge Y will be  _firstWayPoint.y + _secondWayPoint.y) / 2
        if (HType.ToString().Contains(H.Bridge.ToString())) { res.y = (_firstWayPoint.y + _secondWayPoint.y) / 2; }

        //bz if a road i wanna keep the Y value bz the one that overlap want to be different on Y
        if (HType == H.Road)
        {res.y = current.y;};

        return res;
    }

    /// <summary>
    /// Returns planes dim from the Starting point of topleft
    /// </summary>
    protected List<Vector3> ReturnPlanesDim(Vector3 topLefts)
    {
        List<Vector3> res = new List<Vector3>();
        res = UPoly.RetSubMeshPoly(topLefts, Program.gameScene.controllerMain.MeshController.AllVertexs, WideSquare);

        //if is a bridge Y will be  _firstWayPoint.y + _secondWayPoint.y) / 2
        if (HType.ToString().Contains(H.Bridge.ToString()))
        {
            for (int i = 0; i < res.Count; i++)
            {
                Vector3 t = res[i];
                t.y = (_firstWayPoint.y + _secondWayPoint.y) / 2;
                res[i] = t;
            }
        }
        return res;
    }

    /// <summary>
    /// Defined a 3d  rectangle with current selection: onScreenPoly. Defines firstCorner and secondCorner too,
    /// _dir too
    /// </summary>
    /// <returns></returns>
    public void CreateWay()
    {
        //print("1st way p." + _firstWayPoint);
        //print("2st  way p corn." + _secondWayPoint);

        if (firstCorner == new Vector2())
        {
            firstCorner = new Vector2(UInput.TransformedMPos.x, UInput.TransformedMPos.y);
            _firstWayPoint = ClosestSubMeshVert;
        }
        else if (firstCorner != new Vector2())
        {
            secondCorner = new Vector2(UInput.TransformedMPos.x, UInput.TransformedMPos.y);
            _secondWayPoint = UPoly.RayCastTerrain(secondCorner).point;
            _secondWayPoint = m.Vertex.FindClosestVertex(_secondWayPoint, m.CurrentHoverVertices.ToArray());
            //direction was dragged on terrain
            _dir = UDir.ReturnDragDir(_firstWayPoint, _secondWayPoint);
            onScreenPoly = UPoly.RetTerrainPoly(_firstWayPoint, _secondWayPoint, _dir);
        }
    }

    // Use this for initialization
    protected void Start()
	{
        base.Start();
	}
	
	// Update is called once per frame
	protected void Update () 
    {
        base.Update();
        //means the obj was called to be destroy in  base  class
	    if (!PositionFixed)
	    {
            if (HType.ToString().Contains(H.Bridge.ToString()))
            {
                DefineBridgeDominantSide();
            }
	    }
	}

    /// <summary>
    /// Will destroy the current obj and will clear the debuger and _prevWay
    /// this is ordered from the base class
    /// </summary>
    protected void DestroyOrdered()
    {
        BuildingPot.InputU.IsDraggingWay = false;
        if (BuildingPot.Control.BuildWayCursor!=null){ BuildingPot.Control.BuildWayCursor.Destroy();}
        ClearPrevWay();

        DestroyBigPrevBoxes();
        DestroyProjector();

        //so people reroute 
        PersonPot.Control.Queues.AddToDestroyBuildsQueue(OnScreenPoly, MyId);

        Destroy();
    }





    #region Bridge Methods for even submesh that are not used



    List<int> _indexesOfEdgesShore = new List<int>();

    /// <summary>
    /// This is the initial routine that will find if all vertex below a bridge are even.
    /// This is not being used bz is too slow
    /// </summary>
    /// <returns></returns>
    public bool CheckIfAllRealVerticesUnderneathBridgeAreEven()
    {
        bool res = false;
        List<Vector3> realVertexUnderNeath = FindUnderNeathVertices();

        //UVisHelp.CreateHelpers(realVertexUnderNeath, Root.yellowSphereHelp);
        res = TellMeIfTopOnesAreEven(realVertexUnderNeath);

        string msg = "";
        for (int i = 0; i < realVertexUnderNeath.Count; i++)
        {
            msg += realVertexUnderNeath[i].ToString() + "\n";
        }
        print(msg + "Was Found Even? : " + res);

        return res;
    }

    bool TellMeIfTopOnesAreEven(List<Vector3> list)
    {
        List<float> yS = UList.ReturnAxisList(list, H.Y);
        float planesOnAirY = FindPlanesOnAirHeight();

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].y > planesOnAirY)
            {
                float diff = Mathf.Abs(planesOnAirY - list[i].y);
                if (diff > _maxDiffAllowOnTerrainForARoad)
                {
                    return false;
                }
            }
        }
        return true;
    }

    float FindPlanesOnAirHeight()
    {
        float res = 0;
        //im pickin the first one bz all are the same 
        if (_dominantSide == H.Vertic)
        {
            res = _verticPathNew[0].y;
        }
        else if (_dominantSide == H.Horiz)
        {
            res = _horPathNew[0].y;
        }
        return res;
    }

    List<Vector3> FindUnderNeathVertices()
    {
        print(_dominantSide + ">_dominantSide");
        List<Vector3> res = new List<Vector3>();
        if (_dominantSide == H.Vertic)
        {
            res = ReturnRealVerticesUnderNeath(_verticPathNew, _prevWayVertic);
        }
        else if (_dominantSide == H.Horiz)
        {
            res = ReturnRealVerticesUnderNeath(_horPathNew, _prevWayHor);
        }
        return res;
    }

    private List<Vector3> ReturnRealVerticesUnderNeath(List<Vector3> pathP, List<PreviewWay> prevP)
    {
        List<Lot> lots = FindTheLots(pathP);
        List<Vector3> res = new List<Vector3>();
        List<Rect> fourPointsRects = CreateRects(prevP);

        for (int i = 0; i < lots.Count; i++)
        {
            //will loop trhu all Lots Real Vertices that are in indexes
            for (int j = 0; j < lots[i].LotVertices.Count; j++)
            {
                //will make temp Vector2D to find if is contained on the rect
                Vector2 t = new Vector2(lots[i].LotVertices[j].x, lots[i].LotVertices[j].z);

                for (int k = 0; k < fourPointsRects.Count; k++)
                {
                    if (fourPointsRects[k].Contains(t))
                    {
                        res.Add(lots[i].LotVertices[j]);
                    }
                }
            }
        }
        return res;
    }

    List<Rect> CreateRects(List<PreviewWay> prevP)
    {
        List<Rect> res = new List<Rect>();

        for (int i = 0; i < _indexesOfEdgesShore.Count; i++)
        {
            List<Vector3> boundPoint = prevP[_indexesOfEdgesShore[i]].GetBounds();
            boundPoint = UPoly.ScalePoly(boundPoint, 0.3f);

            Rect boundRect = U2D.FromPolyToRect(boundPoint);
            boundRect = U2D.ReturnRectYInverted(boundRect);
            res.Add(boundRect);
        }
        return res;
    }

    private List<Lot> FindTheLots(List<Vector3> pathP)
    {
        List<Lot> lots = Program.gameScene.controllerMain.MeshController.Malla.Lots;
        List<Vector3> firstAndSecondPoints = ReturnFirstAndLastPointOnShore(pathP, 0.3f, pathP[0].y);
        UVisHelp.CreateHelpers(firstAndSecondPoints, Root.redSphereHelp);
        List<int> indexes = UMesh.ReturnIndexesContainDistinct(firstAndSecondPoints, lots);

        string msg = "";
        for (int i = 0; i < indexes.Count; i++)
        {
            msg += indexes[i] + "\n";
        }
        print(msg + "Count:" + indexes.Count);

        List<Lot> res = new List<Lot>();

        for (int i = 0; i < indexes.Count; i++)
        {
            for (int j = 0; j < lots.Count; j++)
            {
                if (lots[j].Index == indexes[i])
                {
                    res.Add(lots[j]);
                }
            }
        }
        return res;
    }

    /// <summary>
    /// This will return the closest point to the edge of a river on top of the terraiin.
    /// The bases for the bridge
    /// </summary>
    protected List<Vector3> ReturnFirstAndLastPointOnShore(List<Vector3> pathP, float minHeight, float groundHeight)
    {
        bool firstFound = false;
        bool secondFound = false;
        List<Vector3> res = new List<Vector3>();

        string msg = "";

        for (int i = 0; i < pathP.Count; i++)
        {
            float diff = Mathf.Abs(groundHeight - pathP[i].y);

            //the first instance is on the river was found
            if (diff > minHeight && !firstFound)
            {
                firstFound = true;
                //then i will add the previus one

                //left on 6 pos 
                //res.Add(pathP[i]);
                //_indexesOfEdgesShore.Add(i);
                //msg += i - 2 + ".|.";
                res.Add(pathP[i - 1]);
                _indexesOfEdgesShore.Add(i - 1);
            }
            //if the first one was found and diff is less minHeigh we are in the other shore already
            else if (diff < minHeight && firstFound && !secondFound)
            {
                secondFound = true;
                //res.Add(pathP[i + 1]);
                //_indexesOfEdgesShore.Add(i + 1);
                //msg += i + ".|.pathP.Count:"+ pathP.Count;
                res.Add(pathP[i]);
                _indexesOfEdgesShore.Add(i);
            }
        }
        print(msg);
        return res;
    }

    #endregion







    #region Brigde Methods That will stay

    /// <summary>
    /// Will add the first and last tile of the dominant side plus the other side path
    /// if those are even then will make even = CheckIfBothBoundsAnchorsAreEven()
    /// </summary>
    bool IsBridgeEven()
    {
        bool even = false;
        List<Vector3> pathPointsToBeCheckIfEven = new List<Vector3>();
        if (_dominantSide == H.Vertic)
        {
            //in this case first and last from _verticPathNew the dominant side
            //and hor list should be even too
            if (_verticPathNew.Count > 0)
            {
                pathPointsToBeCheckIfEven.Add(_verticPathNew[0]);
                pathPointsToBeCheckIfEven.Add(_verticPathNew[_verticPathNew.Count - 1]);
            }
        }
        else if (_dominantSide == H.Horiz)
        {
            if (_horPathNew.Count > 0)
            {
                pathPointsToBeCheckIfEven.Add(_horPathNew[0]);
                pathPointsToBeCheckIfEven.Add(_horPathNew[_horPathNew.Count - 1]);
            }
        }

        if (pathPointsToBeCheckIfEven.Count > 0)
        {even = AreAllPointsEven(pathPointsToBeCheckIfEven);}

        //if both are even then we will check if each anchor tile anchor is even 
        if (even)
        {
            even = CheckIfFirstAndLastTileEvenInBridge();
        }
        return even;
    }

    //will check if each anchor tile anchor is even
    bool CheckIfFirstAndLastTileEvenInBridge()
    {
        List<Vector3> temp = new List<Vector3>();
        if (_dominantSide == H.Vertic && _prevWayVertic.Count > 0 && _prevWayVertic[0] != null)
        {
            temp.AddRange(_prevWayVertic[0].GetAnchors());
            temp.AddRange(_prevWayVertic[_prevWayVertic.Count - 1].GetAnchors());
        }
        else if (_dominantSide == H.Horiz && _prevWayHor.Count > 0 && _prevWayHor[0] != null)
        {
            temp.AddRange(_prevWayHor[0].GetAnchors());
            temp.AddRange(_prevWayHor[_prevWayHor.Count - 1].GetAnchors());
        }
        if (temp.Count > 0)
        {
            return AreAllPointsEven(temp);
        }
        return false;
    }

    /// <summary>
    /// Define with path of a bridge is the longest used 
    /// </summary>
    void DefineBridgeDominantSide()
    {
        if (_verticPathNew.Count == 0 || _horPathNew.Count == 0) { return; }

        float verticYDiff = UMath.ReturnDiffBetwMaxAndMin(_verticPathNew, H.Y);
        float horYDiff = UMath.ReturnDiffBetwMaxAndMin(_horPathNew, H.Y);

        if (verticYDiff > horYDiff)
        {
            _dominantSide = H.Vertic;
        }
        else if (verticYDiff < horYDiff)
        {
            _dominantSide = H.Horiz;
        }
        else
        {
            _dominantSide = H.Same_Length_Both_Sides;
            IsWayEven = false;
            print("Unable to place bridge here Way.cs both side are the same or bridge is on the floor");
        }
    }

    /// <summary>
    /// Will let u knw if a bridge is tall enought so can be built. THi is to avoid bridge on flat terrain
    /// </summary>
    /// <returns></returns>
    bool IsBrideTallEnought(float minHeight)
    {
        bool res = false;
        if (_dominantSide == H.Vertic)
        {
            if (UMath.ReturnDiffBetwMaxAndMin(_verticPathNew, H.Y) > minHeight)
            {
                res = true;
            }
        }
        else if (_dominantSide == H.Horiz)
        {
            if (UMath.ReturnDiffBetwMaxAndMin(_horPathNew, H.Y) > minHeight)
            {
                res = true;
            }
        }
        return res;
    }
    

    #endregion
}
