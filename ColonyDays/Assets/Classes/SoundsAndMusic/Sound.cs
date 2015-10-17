using UnityEngine;
using System.Collections;

public class Sound : Audio {

    public int autoDestroyInSec = 3;
    public bool isAutoDestroy = true;


    float creationTime;

	// Use this for initialization
	void Start () 
    {
        creationTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Time.time > creationTime + autoDestroyInSec)
        {
            Destroy();
        }

        base.Update();
	}
}
