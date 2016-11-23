﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class RandomUV
{
    private GameObject _main;
    private H _hType = H.None;

    private float _xOffset = 0;
    private float _yOffset = 409.6f;

    private int _yMulti = -1;
    private int _yMaxSteps;

    public int YMulti
    {
        get { return _yMulti; }
        set { _yMulti = value; }
    }

    public H HType
    {
        get { return _hType; }
        set { _hType = value; }
    }

    public RandomUV() { }

    public RandomUV(GameObject main, H hType)
    {
        _main = main;
        _hType = hType;
        InitUVMap();
    }

    public void Load(GameObject main)
    {
        _main = main;
        InitUVMap();
    }

    void InitUVMap()
    {
        SetYs();
        if (_yMulti == -1)
        {
            _yMulti = UMath.GiveRandom(0, _yMaxSteps);
        }

        Mesh myMesh = null;
        Vector2[] uvs = null;
        if (IsA10Stripes())
        {
            uvs = (_main.GetComponent<MeshFilter>()).mesh.uv; // Take the existing UVs
        }
        else if (HType == H.Person)
        {
            myMesh = _main.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            uvs = myMesh.uv; // Take the existing UVs
        }

        //so it moves down randomwly 
        _yOffset = _yOffset * _yMulti;
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(uvs[i].x + _xOffset, uvs[i].y + _yOffset);
        }

        //assign
        if (IsA10Stripes())
        {
            _main.GetComponent<MeshFilter>().mesh.uv = uvs;
        }
        else if (HType == H.Person)
        {
            myMesh.uv = uvs;
        }
    }

    private void SetYs()
    {
        if (IsA10Stripes())
        {
            _yOffset = 409.6f;
            _yMaxSteps = 10;
        }
        else if(HType == H.Person)
        {
            _yOffset = 1024f;
            _yMaxSteps = 6;
        }
    }

    bool IsA10Stripes()
    {
        return HType.ToString().Contains("WoodHouse") ||
               HType.ToString().Contains("BrickHouse");
    }
}

