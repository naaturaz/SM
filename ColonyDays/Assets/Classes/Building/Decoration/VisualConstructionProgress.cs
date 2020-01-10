using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class VisualConstructionProgress
{
    private Building _building;
    float _amtNeeded;
    float _currAmt;
    List<String> roots = new List<string>() {
        "Prefab/Building/VisualProgress/Cube1",
        "Prefab/Building/VisualProgress/Cube2",
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
        this._amtNeeded = _amtNeeded;
        AddAmount(amt);
        CalcDim();
    }

    void CalcDim()
    {
        _height = _building.Max.y - _building.Min.y;
        _length = _building.Max.z - _building.Min.z;
        _width = _building.Max.x - _building.Min.x;

        _long = _length / _cubeLength;
        _tall = _height / _cubeHeight;
        _wide = _width / _cubeWidth;

        _cubesTtl = _long * _tall * _wide;
        _amtPerCube = _amtNeeded / _cubesTtl;
    }

    internal void AddAmount(float amt)
    {
        _currAmt += amt;
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
        if (_cubePos == new Vector3()) _cubePos = _building.Min;

        var cube = General.Create(roots[UMath.GiveRandom(0, 2)], _cubePos);
        _cubes.Add(cube);

        //define next cubePos
        _cubePos.x += _cubeWidth;
        if(_cubePos.x > _building.Max.x - _cubeWidth)
        {
            //reset to init
            _cubePos.x = _building.Min.x;
            _cubePos.z += _cubeLength;

            if(_cubePos.z > _building.Max.z - _cubeLength)
            {
                _cubePos.y += _cubeHeight;
                //reset to init
                _cubePos.x = _building.Min.x;
                _cubePos.z = _building.Min.z;
            }
        }
    }

    public void Update()
    {
        if (_currAmt >= _amtNeeded)
        {
            if (_cubes.Count == 0) return;
            _cubes[_cubes.Count - 1].Destroy();
            _cubes.RemoveAt(_cubes.Count - 1);
        }
    }

}
