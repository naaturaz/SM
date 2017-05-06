using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationArea
{
    bool _wasAdded;
    float _startTime;
    GameObject _go;

    public bool WasAdded
    {
        get
        {
            return _wasAdded;
        }

        set
        {
            _wasAdded = value;
        }
    }

    public float StartTime
    {
        get
        {
            return _startTime;
        }

        set
        {
            _startTime = value;
        }
    }

    public NavigationArea(GameObject go)
    {
        _go = go;
        StartTime = Time.time;
    }



    public void AddNavArea()
    {
        if (!WasAdded && Time.time > StartTime + 2)
        {
            var aa = _go.GetComponent<NavMeshSourceTag>();
            aa.enabled = true;

            //var aaa = _go.GetComponent<LocalNavMeshBuilder>();
            //aaa.enabled = true;

            WasAdded = true;
            Debug.Log("nav area added:" + _go.name);
        }
    }
}
