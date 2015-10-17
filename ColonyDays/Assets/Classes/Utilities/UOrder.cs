using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UOrder : MonoBehaviour {


    public static List<Vector3> ReturnOrderedByDistance(Vector3 stone, List<Vector3> list)
    {
        List<Vector3> res = new List<Vector3>();
        List<VectorM> places = new List<VectorM>();

        for (int i = 0; i < list.Count; i++)
        {
            places.Add(new VectorM(list[i], stone));
        }
        places = places.OrderBy(a => a.Distance).ToList();

        for (int i = 0; i < places.Count; i++)
        {
            res.Add(places[i].Point);
        }
        return res;
    }


    public static List<VectorM> ReturnOrderedByDistance(Vector3 stone, List<VectorM> places)
    {
        var anchorOrdered = new List<VectorM>();
        for (int i = 0; i < places.Count; i++)
        {
            if (places[i] != null)
            {
                anchorOrdered.Add(new VectorM(places[i].Point, stone, places[i].LocMyId));
            }
        }
        return anchorOrdered.OrderBy(a => a.Distance).ToList();
    }
}
