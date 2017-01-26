using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Floating : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;

    public float minInterval;
    public float maxInterval;


    float changedTime;
    bool didChange;
    int sign = 1;

    float definedSpeed;
    float definedInterval;



    public bool isFloating;
    public bool isScaling;


    void Start()
    {
        definedSpeed = UMath.GiveRandom(minSpeed, maxSpeed);
        definedInterval = UMath.GiveRandom(minInterval, maxInterval);
    }


    void Update()
    {
        //define
        if (Time.time > changedTime + definedInterval && !didChange)
        {
            changedTime = Time.time;
            didChange = true;
            definedSpeed *= -1;
        }

        //the moving when changed
        if (didChange)
        {
            if (isFloating)
            {
                var newY = transform.position.y + definedSpeed;
                transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            }

            if (isScaling)
            {
                transform.localScale += new Vector3(definedSpeed, definedSpeed, definedSpeed);
            }
        }

        //too long changed
        if (Time.time > changedTime + definedInterval && didChange)
        {
            didChange = false;
        }
    }


}

