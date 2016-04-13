using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class StaticBatch
{
    public StaticBatch()
    {
        Debug.Log("StaticBatch");
        Init();
    }

    void Init()
    {
        // find the corresponding objects e.g. with loading into cache
        GameObject batchmaster = Program.BuildsContainer.gameObject;
        GameObject[] array_gameobjects = GameObject.FindGameObjectsWithTag("HouseMed");
                                              
        // parent the array under batchmaster
        for(int i = 0; i< array_gameobjects.Length;i++)
        {
            array_gameobjects[i].transform.parent = batchmaster.transform;
        }
 
        StaticBatchingUtility.Combine (array_gameobjects, batchmaster);
                                              
        //optionally, in case it does not batch on runtime (don’t ask me why, I found it in a forum, when it didn’t work J)
        batchmaster.SetActive(false);
        batchmaster.SetActive(true);
    }
}

