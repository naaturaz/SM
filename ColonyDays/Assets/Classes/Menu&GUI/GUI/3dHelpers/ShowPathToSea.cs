using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ShowPathToSea
{
    private Vector3 _iniPos;
    private Vector3 _seaPos;
    private Line _lineToSea;
    private List<Vector3> _listToSpawn = new List<Vector3>();
    private List<GOLookAt> _spawns = new List<GOLookAt>();
    private int _updateIndex;
    private bool _isToShowNow;
    private bool _isToHideNow;

    public ShowPathToSea()
    {
        Init();
    }

    private void Init()
    {
        _iniPos = BuildingPot.Control.Registro.ReturnFirstThatContains("Storage").
            SpawnPoint.transform.position;
        _seaPos = MeshController.CrystalManager1.GiveMeTheClosestSeaRegionToMe(_iniPos);

        //UVisHelp.CreateText(_iniPos, "iniPOs");
        //UVisHelp.CreateText(_seaPos, "_seaPos");

        _lineToSea = new Line(_iniPos, _seaPos, 500f);
        _listToSpawn = _lineToSea.ReturnPointsInLineEvery(3);
        SpawnPoints();
    }

    private void SpawnPoints()
    {
        //UVisHelp.CreateHelpers(_listToSpawn, Root.blueCube);
        for (int i = 0; i < _listToSpawn.Count; i++)
        {
            _spawns.Add((GOLookAt)General.Create(Root.ArrowLookAt, _listToSpawn[i], 
                container: Program.BuildsContainer.transform));
        }
    }

    public void Update()
    {
        //they show slowly
        if (_isToShowNow)
        {
            if (_updateIndex < _spawns.Count)
            {
                _spawns[_updateIndex].Show(_seaPos);
                _updateIndex++;
            }
        }
        //they hide slowly
        else if (_isToHideNow)
        {
            if (_updateIndex < _spawns.Count)
            {
                _spawns[_updateIndex].Hide();
                _updateIndex++;
            }
         }
    }

    void ShowPath()
    {
        _isToShowNow = true;
        _updateIndex = 0;
        _isToHideNow = false;
    }

    void HidePath()
    {
        _isToHideNow = true;
        _updateIndex = 0;
        _isToShowNow = false;
    }

    internal void Toggle()
    {
        //fisrt time
        if (!_isToShowNow && !_isToHideNow)
        {
            ShowPath();
        }
            //being shown at call of this
        else if (_isToShowNow)
        {
            HidePath();
        }
            //being hide at call of this 
        else if (_isToHideNow)
        {
            ShowPath();
        }
    }
}

