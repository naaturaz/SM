using System.Collections.Generic;
using UnityEngine;

public class UBuilding : MonoBehaviour
{
    /// <summary>
    /// Will return a list of buildings with
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<Building> ReturnBuildings(List<string> list)
    {
        List<Building> res = new List<Building>();

        for (int i = 0; i < list.Count; i++)
        {
            res.Add(Brain.GetBuildingFromKey(list[i]));
        }

        return res;
    }
}