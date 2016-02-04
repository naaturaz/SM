using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SeaRouter  {

    SMe m = new SMe();
    List<VectorM> _map8entries = new List<VectorM>();
    private Vector3 _entry;

    private Vector3 _closerMapEntryReachable;

    private Building _building;

    public SeaRouter(Vector3 entry, Building building)
    {
        _entry = entry;
        _building = building;
        DefineMap8Entries();
    }

    private void DefineMap8Entries()
    {
        Add4MapCorners();
        Define4MidPointsOfCorners();
    }

    private void Add4MapCorners()
    {
        for (int i = 0; i < m.MeshController.wholeMalla.Count; i++)
        {
            _map8entries.Add(new VectorM(m.MeshController.wholeMalla[i], _entry));
        }
    }

    private void Define4MidPointsOfCorners()
    {
        for (int i = 0; i < m.MeshController.wholeMalla.Count; i++)
        {
            var v1 = m.MeshController.wholeMalla[i];
            var v2 = m.MeshController.wholeMalla[UMath.GoAround(1,i,0,3)];

            var dist = Vector3.Distance(v1, v2)/2;
            var newV = Vector3.MoveTowards(v1, v2, dist);

            _map8entries.Add(new VectorM(newV, _entry));
        }
        _map8entries = _map8entries.OrderBy(a => a.Distance).ToList();
        //UVisHelp.CreateHelpers(_map8entries, Root.largeBlueCube);
    }

    /// <summary>
    /// Expecting 'A 2' will return 'A'
    /// </summary>
    /// <param name="nameGO"></param>
    /// <returns></returns>
    private string FindMyLetter(string nameGO)
    {
        var splt = nameGO.Split(' ');

        if (splt.Count() == 0)
        {
            return null;
        }

        return splt[0];
    }

    /// <summary>
    /// Will tell u if can route 
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public bool CanRoute(Vector3 entry)
    {
        for (int i = 0; i < _map8entries.Count; i++)
        {
            //if a can reach 1 then I can use that one 
            if (!MeshController.CrystalManager1.IntersectAnyLine(_map8entries[i].Point, entry))
            {
                _closerMapEntryReachable = _map8entries[i].Point;
                return true;
            }
        }
        
        return false;
    }


    public TheRoute PlotRoute(Vector3 entry, List<GameObject> spots, List<GameObject> finalLookPoint, 
        Building build, string shipGOMyId)
    {
        var spot = FindRandomSpot(spots, build, shipGOMyId);

        string spotFinLetter = FindMyLetter(spot.transform.name);

        var fin = new Vector3();

        for (int i = 0; i < finalLookPoint.Count; i++)
        {
            if (finalLookPoint[i].transform.name == spotFinLetter)
            {
                fin = finalLookPoint[i].transform.position;
            }
        }

        //correct Y  
        _closerMapEntryReachable = 
            new Vector3(_closerMapEntryReachable.x, spot.transform.position.y, _closerMapEntryReachable.z);

        var lis = new List<Vector3>() {_closerMapEntryReachable, entry, spot.transform.position};
        return ConformInBuildRouteAnimal(lis);
    }

    GameObject FindRandomSpot(List<GameObject> spots, Building build, string shipGOMyId)
    {
        for (int i = 0; i < spots.Count; i++)
        {
            if (build.Dock1.IsSpotFree(spots[i].transform.name))
            {
                build.Dock1.AddToBusySpots(shipGOMyId, spots[i].transform.name);
                return spots[i];
            }
        }

        return null;
    }

    /// <summary>
    /// For an animal farm 
    /// </summary>
    TheRoute ConformInBuildRouteAnimal(List<Vector3> points)
    {
        var TheRoute = ReachBean.RouteVector3s(points);

        //the .O is to pass the profession or brain reurn 
        TheRoute.OriginKey = _building.MyId + ".O";
        TheRoute.DestinyKey = _building.MyId + ".D";

        return TheRoute;
    }

}
