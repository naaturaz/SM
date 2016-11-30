using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Horse : Animal
{
    // Use this for initialization
    void Start()
    {
        base.Start();

        MoveToRandomSpot();
        RotateRandomly();
        SetRandomIdleStart();
    }

    /// <summary>
    /// Intended to be used For the first load of people spawned
    /// </summary>
    static public Horse Create(Vector3 iniPos, Building spawner)
    {
        Horse obj = null;

        obj = (Horse)Resources.Load(Root.horse, typeof(Horse));

        obj = (Horse)Instantiate(obj, iniPos, Quaternion.identity);
        obj.gameObject.transform.SetParent(spawner.transform);
        obj.Spawner = spawner;

        return obj;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        CheckIfYield();
    }
}
