using System;
using System.Collections.Generic;
using UnityEngine;

public class VisualConstructionProgress
{
    private Building _building;
    private Structure _structure;
    float _amtNeeded;
    float _currAmt;

    List<String> _roots = new List<string>() {
        "Prefab/Building/VisualProgress/Cube1",
        "Prefab/Building/VisualProgress/Cube2",
    };

    List<String> _rootsMed = new List<string>() {
        "Prefab/Building/VisualProgress/Cube1Med",
        "Prefab/Building/VisualProgress/Cube2Med",
    };

    List<General> _cubes = new List<General>();

    float _height;
    float _width;
    float _length;
    int _cubesShown;

    float _long;
    float _tall;
    float _wide;
    float _cubesTtl;
    float _amtPerCube;

    float _cubeLength = .4f;
    float _cubeHeight = .4f;
    float _cubeWidth = .4f;

    Vector3 _cubePos;
    private bool _destroyCubes;

    public VisualConstructionProgress(Building building, float _amtNeeded, float amt)
    {
        this._building = building;
        _structure = (Structure)building;
        this._amtNeeded = _amtNeeded;
        AddAmount(amt);
        CalcDim();
    }

    void CalcDim()
    {
        // if (_structure.IsDoubleBound())
        // {
        //     _cubeLength = .8f;
        //     _cubeHeight = .8f;
        //     _cubeWidth = .8f;
        //     _roots = _rootsMed;
        // }

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
        _currAmt += amt;

        if (_structure.CurrentStage == 4) return;

        if (_amtPerCube == 0) _amtPerCube = 1;

        if (_currAmt > _cubesShown * _amtPerCube){
            var newCubes = amt / _amtPerCube;

            for (int i = 0; i < newCubes; i++)
            {
                ShowCube();
            }
        }
    }

    void ShowCube()
    {
        if (_cubePos == new Vector3()) _cubePos = GetInitPos();

        var cube = General.Create(_roots[UMath.GiveRandom(0, 2)], _cubePos);
        _cubes.Add(cube);

        //define next cubePos
        _cubePos.x += _cubeWidth;
        if(_cubePos.x > _building.MaxVcp.x - _cubeWidth)
        {
            //reset to init
            _cubePos.x = _building.MinVcp.x;
            _cubePos.z += _cubeLength;

            if(_cubePos.z > _building.MaxVcp.z - _cubeLength)
            {
                _cubePos.y += _cubeHeight;
                //reset to init
                _cubePos.x = _building.MinVcp.x;
                _cubePos.z = _building.MinVcp.z;
            }
        }
    }

    Vector3 GetInitPos()
    {
        return Vector3.MoveTowards(_building.MinVcp, _building.MaxVcp, _cubeHeight);
    }

    public void Update()
    {
        if (_currAmt >= _amtNeeded || _structure.CurrentStage == 4)
        {
            if (_cubes.Count == 0) return;
            _cubes[_cubes.Count - 1].Destroy();
            _cubes.RemoveAt(_cubes.Count - 1);
        }
    }

}
