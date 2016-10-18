using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Show a path to in 3d from a point A to B
/// </summary>
public class ShowPathTo
{
    private Vector3 _iniPos;
    private Vector3 _finPos;
    private Line _lineTo;
    private List<Vector3> _listToSpawn = new List<Vector3>();
    private List<GOLookAt> _spawns = new List<GOLookAt>();
    private int _updateIndex;
    private bool _isToShowNow;
    private bool _isToHideNow;
    private Person _person;
    private string _type;

    private string rootSpawnObj;//the obj will be spwan and show as the root visual helper

    private float _pointsInLineStep;//how apart are the steps of the points 

    private GameObject _finalGO;

    /// <summary>
    /// Path to Sea
    /// </summary>
    public ShowPathTo()
    {
        _iniPos = BuildingPot.Control.Registro.ReturnFirstThatContains("Storage").
    SpawnPoint.transform.position;
        _finPos = MeshController.CrystalManager1.GiveMeTheClosestSeaRegionToMe(_iniPos);

        _type = "Sea";

        rootSpawnObj = Root.ArrowLookAt;
        _pointsInLineStep = 3;

        Init();
    }

    public ShowPathTo(Person _person, string typeP)
    {
        this._person = _person;
        _type = typeP;

        rootSpawnObj = Root.CubeLookAt;
        _pointsInLineStep = .5f;
    }

    void PreInitForPerson()
    {
        if (DoesNeedToBeStop())
        {
            return;
        }

        _iniPos = _person.transform.position;
        if (_type == "Home")
        {
            _finPos = _person.Home.transform.position;
            _finalGO = _person.Home.gameObject;
        }
        else if (_type == "Work")
        {
            _finPos = _person.Work.transform.position;
            _finalGO = _person.Work.gameObject;

        }

        Init();
    }

    /// <summary>
    /// A null Structure 
    /// </summary>
    /// <returns></returns>
    bool DoesNeedToBeStop()
    {
        return (_type == "Work" && _person.Work == null) ||
               (_type == "Home" && _person.Home == null) ||
               (_type == "Food" && _person.FoodSource == null) ||
               (_type == "Religion" && _person.Religion == null) ||
               (_type == "Chill" && _person.Chill == null);
    }

    private void Init()
    {
        _lineTo = new Line(_iniPos, _finPos, 500f);
        _listToSpawn = _lineTo.ReturnPointsInLineEvery(_pointsInLineStep);
        SpawnPoints();
        SetPrevAndNextIfNeed();
    }

    private void SetPrevAndNextIfNeed()
    {
        if (_type == "Sea")
        {
            return;
        }

        _spawns[0].PrevGO = _person.gameObject;
        _spawns[0].NextGO = _spawns[1].gameObject;


        _spawns[_spawns.Count-1].PrevGO = _spawns[_spawns.Count-2].gameObject;
        _spawns[_spawns.Count-1].NextGO = _finalGO.gameObject;


        for (int i = 1; i < _spawns.Count-1; i++)
        {
            _spawns[i].PrevGO = _spawns[i - 1].gameObject;
            _spawns[i].NextGO = _spawns[i + 1].gameObject;
        }
    }



    private void SpawnPoints()
    {
        for (int i = 0; i < _listToSpawn.Count; i++)
        {
            var pos = _listToSpawn[i];
            if (_type != "Sea")
            {
                pos += new Vector3(0, .25f, 0);
            }

            var go = (GOLookAt)General.Create(rootSpawnObj, pos,
                container: Program.BuildsContainer.transform);

            _spawns.Add(go);
        }
    }


    /// <summary>
    /// Be aware that from person.cs is called from 64ms courutine
    /// </summary>
    public void Update()
    {
        //they show slowly
        if (_isToShowNow)
        {
            if (_updateIndex < _spawns.Count)
            {
                _spawns[_updateIndex].Show(_finPos);
                _updateIndex++;
            }
        }
        //they hide slowly
        else if (_isToHideNow)
        {
            if (_updateIndex < _spawns.Count)
            {
                _spawns[_updateIndex].Hide();
                if (_type != "Sea")
                {
                    _spawns[_updateIndex].Destroy();
                    //so it stays in the same one 
                    _spawns.RemoveAt(_updateIndex);
                    _updateIndex--;
                }
                _updateIndex++;
            }
        }
    }

    void ShowPath()
    {
        if (_type != "Sea")
        {
            //_spawns.Clear();
            //bz they where destroyed when hide it 
            PreInitForPerson();
        }

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

    internal void Toggle(string which)
    {
        //must be the same to toggle
        if (which != _type)
        {
            return;
        }
        Toggle();
    }
}

