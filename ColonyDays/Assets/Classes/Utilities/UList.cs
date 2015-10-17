using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UList : MonoBehaviour {

    public static List<Vector3> FindVectorsOnSameHeight(List<Vector3> list, float height, float epsilon)
    {
        List<Vector3>res=new List<Vector3>();
        for (int i = 0; i < list.Count; i++)
        {
            if (UMath.nearlyEqual(list[i].y, height, epsilon))
            {
                res.Add(list[i]);
            }
        }
        return res;
    }

    public static List<Vector3> FindVectorsOnSameRange(List<Vector3> list, float compare, H axis, float epsilon)
    {
        List<Vector3> res = new List<Vector3>();
        for (int i = 0; i < list.Count; i++)
        {
            if (axis == H.X)
            {
                if (UMath.nearlyEqual(list[i].x, compare, epsilon))
                {
                    res.Add(list[i]);
                }
            }
            else if (axis == H.Y)
            {
                if (UMath.nearlyEqual(list[i].y, compare, epsilon))
                {
                    res.Add(list[i]);
                }
            }
            else if (axis == H.Z)
            {
                if (UMath.nearlyEqual(list[i].z, compare, epsilon))
                {
                    res.Add(list[i]);
                }
            }

    
        }
        return res;
    }

    /// <summary>
    /// Find the common values of the 'list' on the 'axis' passed.
    /// For ex: is used to find all the values of Y in the mesh
    /// 
    /// Will order by if 'order' was specified
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<float> FindYAxisCommonValues(List<Vector3> list, H order)
    {
        if (order == H.Descending)
        {
            list = list.OrderByDescending(a => a.y).ToList();
        }
        else if (order == H.Ascending)
        {
            list = list.OrderBy(a => a.y).ToList();
        }


        var res = UList.ReturnAxisList(list, H.Y);
        res = res.Distinct().ToList();


        return res;
    }

    /// <summary>
    /// Find the common values of the 'list' on the 'axis' passed.
    /// For ex: is used to find all the values of Y in the mesh
    /// 
    /// Will order by if 'order' was specified
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<float> FindXAxisCommonValues(List<Vector3> list, H order)
    {
        if (order == H.Descending)
        {
            list = list.OrderByDescending(a => a.x).ToList();
        }
        else if (order == H.Ascending)
        {
            list = list.OrderBy(a => a.x).ToList();
        }


        var res = UList.ReturnAxisList(list, H.X);
        res = res.Distinct().ToList();


        return res;
    }

    /// <summary>
    /// Find the common values of the 'list' on the 'axis' passed.
    /// For ex: is used to find all the values of Y in the mesh
    /// 
    /// Will order by if 'order' was specified
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<float> FindZAxisCommonValues(List<Vector3> list, H order)
    {
        if (order == H.Descending)
        {
            list = list.OrderByDescending(a => a.z).ToList();
        }
        else if (order == H.Ascending)
        {
            list = list.OrderBy(a => a.z).ToList();
        }


        var res = UList.ReturnAxisList(list, H.Z);
        res = res.Distinct().ToList();


        return res;
    }

    /// <summary>
    /// For this to work fine the 'list' passed most be ordered Descending 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="below"></param>
    /// <returns></returns>
    public static float FindFirstYBelow(List<float> list, float below)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] < below)
            {
                return list[i];
            }
        }
        return 0;
    }

    public static float FindFirstYAbove(List<float> list, float val)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] > val)
            {
                return list[i];
            }
        }
        return 0;
    }

    public static float FindMostCommonValue(H axis, List<Vector3> list)
    {

        var ordered = list.OrderBy(a => a.y).ToList();
        return ordered[ordered.Count / 2].y;
    }


    public static List<H> ConvertToList(System.Array array)
    {
        List<H> res = new List<H>();
        foreach (var item in array)
        {
            //casting from string to H
            var content = (H)Enum.Parse(typeof(H), item.ToString());
            res.Add(content);
        }
        return res;
    }

    /// <summary>
    /// Eliminates duplicates ... robust code.. 
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<Vector3> EliminateDuplicates(List<Vector3> list)
    {
        List<Vector3> duplicateList = new List<Vector3>();
        List<Vector3> a = list;
        List<Vector3> newList = new List<Vector3>();
        int sameValCounter = 0;
        int duplicateIndex = 0;

        for (int i = 1; i < list.Count; i++)
        {
            for (int j = 0; j < a.Count; j++)
            {
                if (list[i] == a[j])
                {
                    sameValCounter++;
                }
            }
            if (sameValCounter < 2)
            {
                newList.Add(list[i]);
            }
            else if (sameValCounter > 1)
            {
                //will added to the dupl list right away
                duplicateList.Add(list[i]);
                for (int j = 0; j < duplicateList.Count; j++)
                {
                    if (duplicateList[j] == list[i])
                    {
                        duplicateIndex++;
                    }
                }
                //if the duplicate val is only 1 then we will added bz is not being added ever 
                //yet to newList
                if (duplicateIndex < 2)
                {
                     newList.Add(list[i]);
                }
            }
            sameValCounter = 0;
            duplicateIndex = 0;
        }
        return newList;
    }

    /// <summary>
    /// Eliminates duplicates ... robust code.. 
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<Vector3> EliminateDuplicatesByDist(List<Vector3> list, float minDist)
    {
        List<Vector3> duplicateList = new List<Vector3>();
        List<Vector3> a = list;
        List<Vector3> newList = new List<Vector3>();
        int sameValCounter = 0;
        int duplicateIndex = 0;

        for (int i = 1; i < list.Count; i++)
        {
            for (int j = 0; j < a.Count; j++)
            {
                float dist = Vector3.Distance(list[i], a[j]);
                //if the distance is less thn the minimun distance allowed
                //then is the same Vector3 
                if (dist < Mathf.Abs(minDist))
                {
                    sameValCounter++;
                }
            }
            if (sameValCounter < 2)
            {
                newList.Add(list[i]);
            }
            else if (sameValCounter > 1)
            {
                //will added to the dupl list right away
                duplicateList.Add(list[i]);
                for (int j = 0; j < duplicateList.Count; j++)
                {
                    float dist = Vector3.Distance(list[i], duplicateList[j]);
                    if (dist < Mathf.Abs(minDist))
                    {
                        duplicateIndex++;
                    }
                }
                //if the duplicate val is only 1 then we will added bz is not being added ever 
                //yet to newList
                if (duplicateIndex < 2)
                {
                    newList.Add(list[i]);
                }
            }
            sameValCounter = 0;
            duplicateIndex = 0;
        }
        return newList;
    }

    public static List<Vector3> AddManyListToList(List<Vector3> current,
        List<Vector3> toAdd1, List<Vector3> toAdd2, List<Vector3> toAdd3 = null,
        List<Vector3> toAdd4 = null)
    {
        current = AddOneListToList(current, toAdd1);
        if(toAdd2!=null)
        { current = AddOneListToList(current, toAdd2); }
        if (toAdd3 != null)
        { current = AddOneListToList(current, toAdd3); }
        if (toAdd4 != null)
        { current = AddOneListToList(current, toAdd4); }
        return current;
    }

    //carlos
    public static List<NewBuild> newBildList;
    public static List<Btn3D> menus;

    public static void AddObject<T>(T obj)
    {
        if (obj.GetType() == typeof(NewBuild))
        {
            newBildList.Add(obj as NewBuild);
        }
        List<T> array = new List<T>();
        array.Add(obj);
    }

    //make this <T>
    public static List<Vector3> AddOneListToList(List<Vector3> current, 
    List<Vector3> toAdd)
    {
        for (int i = 0; i < toAdd.Count; i++)
        {
            current.Add(toAdd[i]);
        }
        return current;
    }

    public static List<T> AddOneListToList<T>(List<T> current, List<T> toAdd)
    {
        for (int i = 0; i < toAdd.Count; i++)
        {
            current.Add(toAdd[i]);
        }
        return current;
    }
    
    public static List<float> ReturnAxisList(List<Vector3> list, H axis)
    {
        List<float> res = new List<float>();
        if (axis == H.X)
        {
            for (int i = 0; i < list.Count; i++)
            {
                res.Add(list[i].x);
            }
        }        
        else if (axis == H.Y)
        {
            for (int i = 0; i < list.Count; i++)
            {
                res.Add(list[i].y);
            }
        }
        else if (axis == H.Z)
        {
            for (int i = 0; i < list.Count; i++)
            {
                res.Add(list[i].z);
            }
        }
        return res;
    }


    public static List<TreeVeget> UpdateAList(List<TreeVeget> list, TerrainRamdonSpawner newObj)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].IndexAllVertex == newObj.IndexAllVertex)
            {
                list[i] = newObj as TreeVeget;
            }
            
        }
        return list;
    }

    public static List<StoneRock> UpdateAList(List<StoneRock> list, TerrainRamdonSpawner newObj)
    {
        //print(list.Count+" count");
        //print(newObj.name+" name");

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].IndexAllVertex == newObj.IndexAllVertex)
            {
                list[i] = newObj as StoneRock;
            }

        }
        return list;
    }

    public static List<IronRock> UpdateAList(List<IronRock> list, TerrainRamdonSpawner newObj)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].IndexAllVertex == newObj.IndexAllVertex)
            {
                list[i] = newObj as IronRock;
            }

        }
        return list;
    }

    public static List<StillElement> UpdateAList(List<StillElement> list, TerrainRamdonSpawner newObj)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].IndexAllVertex == newObj.IndexAllVertex)
            {
                list[i] = newObj as StillElement;
            }

        }
        return list;
    }

    public static List<Vector3> ReturnTheVector3List(List<PreviewWay> list)
    {
        List<Vector3> res = new List<Vector3>(); 
        for (int i = 0; i < list.Count; i++)
        {
            res.Add(list[i].transform.position);
        }
        return res;
    }


    public static List<Vector3> ReturnTrunckedList(List<Vector3> list, int much)
    {
            List<Vector3 > res= new List<Vector3>();

        for (int i = 0; i < much; i++)
        {
res.Add(list[i]);
        }
        return res;
    }

}
