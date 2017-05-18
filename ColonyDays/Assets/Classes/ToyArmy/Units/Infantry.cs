using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : Commandable
{

    // Use this for initialization
    protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
        Walk();
    }

    float interval = 0.1f;
    float lastInterval;
    bool up;

    void Walk()
    {
        if (interval == 0)
        {
            interval = UMath.GiveRandom(0.08f, 0.15f);
        }

        if (Agent.enabled)
        {
            
        }
    }
   

    void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
