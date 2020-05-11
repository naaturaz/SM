using System;
using System.Collections.Generic;
using UnityEngine;

public class VisualConstructionProgress
{
    private Building _building;
    private Structure _structure;
    private float _amtNeeded;
    private float _currAmt;

    private List<String> _roots = new List<string>() {
        "Prefab/Building/VisualProgress/Cube1",
        "Prefab/Building/VisualProgress/Cube2",
    };

    private List<General> _cubes = new List<General>();

    private float _height;
    private float _width;
    private float _length;
    private int _cubesShown;

    private float _long;
    private float _tall;
    private float _wide;
    private float _cubesTtl;
    private float _amtPerCube;

    private float _cubeLength = .4f;
    private float _cubeHeight = .4f;
    private float _cubeWidth = .4f;

    private Vector3 _cubePos;
    private bool _buildingSetToBeDestroyed;

    public VisualConstructionProgress(Building building, float _amtNeeded, float amt)
    {
        amt = amt > 0f ? amt : 10f;//adding a few boxes right away
        Debug.Log("VisualConstructionProgress init amt:" + amt);

        this._building = building;
        _structure = (Structure)building;
        this._amtNeeded = _amtNeeded;

        CalcDim();
        AddAmount(amt);
    }

    private void CalcDim()
    {
        _height = _building.MaxVcp.y - _building.MinVcp.y;
        _length = _building.MaxVcp.z - _building.MinVcp.z;
        _width = _building.MaxVcp.x - _building.MinVcp.x;

        _long = _length / _cubeLength;
        _tall = _height / _cubeHeight;
        _wide = _width / _cubeWidth;

        _cubesTtl = _long * _tall * _wide;
        _amtPerCube = _amtNeeded / _cubesTtl;
    }

    internal void AddAmount(float amt)
    {
        if (amt == 100000)
        {
            _buildingSetToBeDestroyed = true;
            return;
        }

        _currAmt += amt;
        if (_currAmt >= _amtNeeded)
            _currAmt = _amtNeeded;

        if (_structure.CurrentStage == 4) return;

        if (_amtPerCube == 0) _amtPerCube = 1;

        if (_currAmt > _cubesShown * _amtPerCube)
        {
            var newCubes = amt / _amtPerCube;

            for (int i = 0; i < newCubes; i++)
                CreateCube();
        }
    }

    private void CreateCube()
    {
        if (_cubePos == new Vector3()) _cubePos = GetInitPos();

        var cube = General.Create(_roots[UMath.GiveRandom(0, 2)], _cubePos);
        _cubes.Add(cube);

        //define next cubePos
        _cubePos.x += _cubeWidth;
        if (_cubePos.x > _building.MaxVcp.x - _cubeWidth)
        {
            //reset to init
            _cubePos.x = _building.MinVcp.x;
            _cubePos.z += _cubeLength;

            if (_cubePos.z > _building.MaxVcp.z - _cubeLength)
            {
                _cubePos.y += _cubeHeight;
                //reset to init
                _cubePos.x = _building.MinVcp.x;
                _cubePos.z = _building.MinVcp.z;
            }
        }
    }

    private Vector3 GetInitPos()
    {
        return Vector3.MoveTowards(_building.MinVcp, _building.MaxVcp, _cubeHeight);
    }

    public void Update()
    {
        //destroy them
        if (_currAmt >= _amtNeeded || _structure.CurrentStage == 4 || _buildingSetToBeDestroyed)
        {
            if (_cubes.Count == 0) return;
            _cubes[_cubes.Count - 1].Destroy();
            _cubes.RemoveAt(_cubes.Count - 1);
        }
    }
}