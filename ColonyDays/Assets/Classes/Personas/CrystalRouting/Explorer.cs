/*
 * This class helps with the routing will tell if reaching the Final 
 * a building was found or not and if was found will find closest building
 * will pick closest 3 points to Final and will add the 4 point as the intersection
 * 
 * Those 4 points will be the only ones the CryRoute will look into 
 * 
 * Explorer is used once a new _curr is set on CryRoute. A explorations needs to be done
 * to see what is on front 
 */

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Explorer 
{
    List<ExplorerUnit> _units = new List<ExplorerUnit>();
    public ExplorerUnit Result;//the one will contain the Unit for work for the CryRoute
    //is building routing if is true we can use 
    private bool _isBuildingRouting = true;

    //says if from curr to Final are only bulidings or still elemtents intersectin 
    private bool _isIntersectingOnlyObstacles = true;

    //use to say. if is intersecting only stills then IsBuildingRouting is false. So
    //way routing can work
    private bool _isIntersectingOnlyStills = true;

    /// <summary>
    /// is building routing if is true we can use 
    /// </summary>
    public bool IsBuildingRouting
    {
        //is calling WasABuildingHit() mainly to set the object Result
        //at the same time must have being intersecting only obstacles

        //and is not only intersecting stills. bz if so BuidingRouting is false
        //so way routing can happen 
        get
        {
            if (_isBuildingRouting && _isIntersectingOnlyObstacles && !_isIntersectingOnlyStills)
            {
                WasAObstacleHit();
                return true;
            }
            return false;
        }
    }

    public Explorer() { }

    /// <summary>
    /// Adds a key to the explorer 
    /// </summary>
    /// <param name="crystal">Key of the Parent ID with intersect with</param>
    /// <param name="intersection">The point where we intersect</param>
    /// <param name="currPosition">the curr Position of the Crystal reaching Final</param>
    public void AddKey(Crystal crystal, Vector3 intersection, Vector3 currPosition, Vector3 final)
    {
        ExplorerUnit doesExistKey = null;

        if (_units.Count > 0)
        {
            doesExistKey = _units.Find(a => a.Key == crystal.ParentId);
        }
            
        //so it doesnt add duplicates keys
        if (doesExistKey == null)
        {
            _units.Add(new ExplorerUnit(crystal, intersection, currPosition, final));
        }
        //bz a line can have diff intersections in a building. usually 2 
        //if exist will add Intersection 
        else
        {
            doesExistKey.AddIntersection(intersection, crystal);
        }

        SetIfIsIntersectingOnlyObstacles(crystal);
    }

    /// <summary>
    /// Will say if a building was hit and will return a ExplorerUnit object
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    bool WasAObstacleHit()
    {
        _units = _units.OrderBy(a => a.Distance).ToList();

        for (int i = 0; i < _units.Count; i++)
        {
            if (_units[i].IsHasAValidObstacle)
            {
                _units[i].Create4Crystals();//so the crystals are ready
                Result = _units[i];
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// sets for saying if from curr to Final are only bulidings or still elemtents intersectin 
    /// </summary>
    /// <param name="c"></param>
    void SetIfIsIntersectingOnlyObstacles(Crystal c)
    {
        if (_isIntersectingOnlyObstacles && c.Type1 != H.Obstacle)
        {        
            //intersected something was not a obstacle 
            _isIntersectingOnlyObstacles = false;
            _isIntersectingOnlyStills = false;
        }
        //is intersecting only obstacles 
        else
        {
            //if is intersecting only stills
            if (_isIntersectingOnlyStills)
            {
                Building val = Brain.GetBuildingFromKey(c.ParentId);

                //if one ParentId is a buiding then _isIntersectingOnlyStills = false
                if (val != null)
                {
                    _isIntersectingOnlyStills = false;
                }
            }
        }
    }

    /// <summary>
    /// So its restarted so can be used again 
    /// </summary>
    public void Restart()
    {
        Result = null;
        _units.Clear();
        _isBuildingRouting = true;
        _isIntersectingOnlyObstacles = true;
        _isIntersectingOnlyStills = true;
    }

    /// <summary>
    /// Adding the Crystals contain in RectC so if one Crystal is not obstacle then we cannnot use the 
    /// Building Routing system
    /// </summary>
    /// <param name="c"></param>
    public void AddCrystalOfRectC(Crystal c)
    {
        //as soon one is found that is not type obstacle then we cant use Building Routing 
        if (_isBuildingRouting && c.Type1 != H.Obstacle)
        {
            _isBuildingRouting = false;
        }
    }
}

public class ExplorerUnit
{
    public string Key;
    public List<Vector3> Intersections = new List<Vector3>();
    
    public Building Building;
    public StillElement StillElement;

    //the 4 crystals to be eval in CryRoute
    public List<Crystal> Crystals = new List<Crystal>();

    //distance to currPosition of Crystal Reaching final and intersect 
    public float Distance;

    //the current point on the route
    //the intersections should be moved towards this 
    public Vector3 Current;
    public Vector3 Final;//the final point of the Route 
    public bool IsHasAValidObstacle;

    public ExplorerUnit(Crystal crystal, Vector3 intersect, Vector3 currPosition, Vector3 final)//the curr Position of the Crystal reaching Final
    {
        Current = currPosition;
        Final = final;
        Key = crystal.ParentId;
        Intersections.Add(intersect);
        OrderIntersections();

        Distance = Mathf.Abs(Vector3.Distance(intersect, currPosition));
        Building = Brain.GetBuildingFromKey(Key);

        StillElement =
            Program.gameScene.controllerMain.TerraSpawnController.Find(crystal.ParentId);

        if (Building != null)
        {
            IsHasAValidObstacle = true;
        }
        else if (StillElement != null)
        {
            //Debug.Log("Hey hit random: " + StillElement.MyId);
            IsHasAValidObstacle = true;
        }
        else
        {
            //is set to that so if never was calculated its really far 
            //since distance will be used for ordering 
            Distance = 10000;
        }
    }

    /// <summary>
    /// So i know wich one is closer to Current 
    /// </summary>
    private void OrderIntersections()
    {
        
    }

    /// <summary>
    /// Will be call only if was the select building to be route trhu
    /// 
    /// Will set the Crystals
    /// will pick closest 3 points of the buildig to Final and will add the 4th point as the intersection
    /// </summary>
    public void Create4Crystals()
    {
        Crystals.Clear();
        var anchorOrder = ReturnOrderedAnchors();
     
        Crystals.AddRange(anchorOrder);
        Crystals.AddRange(ReturnIntersectionsPriorityToFin());
    }

    List<Crystal> ReturnOrderedAnchors()
    {
        List<Crystal> anchorOrdered = new List<Crystal>();

        anchorOrdered = ReturnScaledAnchorsFromBuildingOrStillElement();

        for (int i = 0; i < anchorOrdered.Count; i++)
        {
            anchorOrdered[i].Distance = Mathf.Abs(Vector3.Distance(U2D.FromV2ToV3(anchorOrdered[i].Position), Final));
        }
        anchorOrdered = anchorOrdered.OrderBy(a => a.Distance).ToList();

        return anchorOrdered;
    }

    float scale = 0.04f;//5
    List<Crystal> ReturnScaledAnchorsFromBuildingOrStillElement()
    {
        List<Crystal> res = new List<Crystal>();

        //for building
        if (Building != null)
        {
            res = ReturnScaledAnchors(Building.Anchors.ToArray(), Building.MyId);
        }
        //for still elemtnt
        else if (StillElement != null)
        {
            res = ReturnScaledAnchors(StillElement.Anchors.ToArray(), StillElement.MyId);
        }
        return res;
    }

    List<Crystal> ReturnScaledAnchors(Vector3[] anchors, string myIDP)
    {
        List<Crystal> res = new List<Crystal>();
        anchors = UPoly.ScalePoly(anchors, scale);

        for (int i = 0; i < anchors.Length; i++)
        {
            res.Add(new Crystal(anchors[i], H.Obstacle, myIDP, setIdAndName: false));
        }
        return res;
    }


        /// <summary>
    /// Ordering to be closer to _fin
    /// 
    /// Pls interseection
    /// </summary>
    //List<Crystal> ReturnPriorityToFin(List<Crystal> res)
    List<Crystal> ReturnIntersectionsPriorityToFin()
    {
        List<Crystal> res = new List<Crystal>();

        var obstaMidPos = ReturnTransformPosOfBuildingOrStillEle();
        for (int i = 0; i < Intersections.Count; i++)
        {
            var inter = new Crystal(Intersections[i], H.None, "", setIdAndName: false);
            //must be moved closer to Current/Origin so in tight towns can be reached
            //bz if is moved Away from center of the building can be too far to be 
            //reached 
            res.Add(ReturnCrystalFurtherTo(inter, obstaMidPos));
        }
        //UVisHelp.CreateHelpers(Intersections, Root.yellowCube);
        return res;
    }

    Vector3 ReturnTransformPosOfBuildingOrStillEle()
    {
        if (Building!=null)
        {
            return Building.transform.position;
        }
        if (StillElement!=null)
        {
            return StillElement.transform.position;
        }
        return new Vector3();
    }

    /// <summary>
    /// Bz they needs to be moved a bit away from Buildign
    /// </summary>
    /// <returns></returns>
    Crystal ReturnCrystalFurtherTo(Crystal crystal, Vector3 closerTo)
    {
        float moveBy = 0.1f;

        var moved = Vector3.MoveTowards(U2D.FromV2ToV3(crystal.Position), Current, moveBy);
        crystal.Position = U2D.FromV3ToV2(moved);

        return crystal;
    }

    /// <summary>
    /// ADds the intersection we hit and the crystal that belongs to
    /// </summary>
    /// <param name="intersection"></param>
    /// <param name="crystal"></param>
    internal void AddIntersection(Vector3 intersection, Crystal crystal)
    {
        Intersections.Add(intersection);
        Intersections = Intersections.Distinct().ToList();
    }
}