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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Explorer 
{
    List<ExplorerUnit> _units = new List<ExplorerUnit>();
    public ExplorerUnit Result;//the one will contain the Unit for work for the CryRoute

    public Explorer() { }

    /// <summary>
    /// Adds a key to the explorer 
    /// </summary>
    /// <param name="key">Key of the Parent ID with intersect with</param>
    /// <param name="intersection">The point where we intersect</param>
    /// <param name="currPosition">the curr Position of the Crystal reaching Final</param>
    public void AddKey(string key, Vector3 intersection, Vector3 currPosition, Vector3 final)
    {
        ExplorerUnit doesExistKey = null;

        if (_units.Count > 0)
        {
            doesExistKey = _units.Find(a => a.Key == key);
        }
            
        //so it doesnt add duplicates keys
        if (doesExistKey == null)
        {
            _units.Add(new ExplorerUnit(key, intersection, currPosition, final));
        }
    }

    /// <summary>
    /// Will say if a building was hit and will return a ExplorerUnit object
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool WasABuildingHit()
    {
        _units = _units.OrderBy(a => a.Distance).ToList();

        for (int i = 0; i < _units.Count; i++)
        {
            if (_units[i].IsHasAValidBuilding)
            {
                _units[i].Create4Crystals();//so the crystals are ready
                Result = _units[i];
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// So its restarted so can be used again 
    /// </summary>
    public void Restart()
    {
        _units.Clear();
    }
}

public class ExplorerUnit
{
    public string Key;
    public Vector3 Intersection;
    public Building Building;
    //the 4 crystals to be eval in CryRoute
    public List<Crystal> Crystals = new List<Crystal>();

    //distance to currPosition of Crystal Reaching final and intersect 
    public float Distance; 
    public Vector3 Final;//the final point of the Route 
    public bool IsHasAValidBuilding;

    public ExplorerUnit(string key, Vector3 intersect, Vector3 currPosition, Vector3 final)//the curr Position of the Crystal reaching Final
    {
        Final = final;
        Key = key;
        Intersection = intersect;
        UVisHelp.CreateHelpers(Intersection, Root.yellowCube);

        Distance = Mathf.Abs(Vector3.Distance(intersect, currPosition));
        Building = Brain.GetBuildingFromKey(Key);

        if (Building != null)
        {
            IsHasAValidBuilding = true;
        }
        else
        {
            //is set to that so if never was calculated its really far 
            //since distance will be used for ordering 
            Distance = 10000;
        }
    }

    /// <summary>
    /// Will be call only if was the select building to be route trhu
    /// 
    /// Will set the Crystals
    /// will pick closest 3 points of the buildig to Final and will add the 4th point as the intersection
    /// </summary>
    public void Create4Crystals()
    {
        var anchorOrder = ReturnOrderedAnchors();
        Crystals = ReturnPriorityToFin(anchorOrder);
    }

    List<Crystal> ReturnOrderedAnchors()
    {
        List<Crystal> anchorOrdered = new List<Crystal>();
        anchorOrdered = MeshController.CrystalManager1.ReturnCrystalsThatBelongTo(Building, false);

        for (int i = 0; i < 4; i++)
        {
            anchorOrdered[i].Distance = Mathf.Abs(Vector3.Distance(U2D.FromV2ToV3(anchorOrdered[i].Position), Final));
        }
        anchorOrdered = anchorOrdered.OrderBy(a => a.Distance).ToList();

        return anchorOrdered;
    }

    /// <summary>
    /// Ordering to be closer to _fin
    /// 
    /// Pls interseection
    /// </summary>
    List<Crystal> ReturnPriorityToFin(List<Crystal> res)
    {
        //-1 bz only need the first 3 
        res.RemoveAt(res.Count-1);
        var last = new Crystal(Intersection, H.None, "", setIdAndName: false);
        res.Add(last);
        //res.Add(ReturnCrystalAwayFromBuild(last));

        return res;
    }

    /// <summary>
    /// Bz they needs to be moved a bit away from Buildign
    /// </summary>
    /// <returns></returns>
    Crystal ReturnCrystalAwayFromBuild(Crystal crystal)
    {
        var person = PersonPot.Control.All.FirstOrDefault();
        float moveBy = person.PersonDim * 3;

        var moved = Vector3.MoveTowards(U2D.FromV2ToV3(crystal.Position), Building.transform.position, -moveBy);
        crystal.Position = U2D.FromV3ToV2(moved);

        return crystal;
    }
}