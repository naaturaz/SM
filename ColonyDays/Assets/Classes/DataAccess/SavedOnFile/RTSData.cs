using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;
using System.Collections.Generic;

public class RTSData //: General //,ISerializable
{
    /// <summary>
    /// Camera Data
    /// </summary>


    public KeyCode saveKeyC;
    public KeyCode loadKeyC;
    public Vector3 pos;
    public Quaternion rot;
    public float FOV;
    public Vector3 CenterTargetPos;

    private KeyCode keyCode1;
    private KeyCode keyCode2;
    private Transform TransformCam;
    private Transform CenterTarget;

    //use for all 5 save cameras
    public RTSData(KeyCode save, KeyCode load, Transform transformCam, Transform centerTarget)
    {
        saveKeyC = save;
        loadKeyC = load;
        pos = transformCam.position;
        rot = transformCam.rotation;
        FOV = transformCam.transform.GetComponent<Camera>().fieldOfView;
        CenterTargetPos = centerTarget.position;

    }

    //use for last camera seen on screen
    public RTSData(Transform transformCam, Transform centerTarget)
    {
        pos = transformCam.position;
        rot = transformCam.rotation;
        FOV = transformCam.transform.GetComponent<Camera>().fieldOfView;
        CenterTargetPos = centerTarget.position;
    }

    public RTSData() { }






}
