﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkerTile : GUIElement
{
    private string _workType;

    public string WorkType
    {
        get { return _workType; }
        set { _workType = value; }
    }

    public TextMeshProUGUI _descText;
    public TextMeshProUGUI _totalText;
    public TextMeshProUGUI _currentText;

    private List<Structure> _buildings;
    private int _employ = -1;//total employ by this types of works
    private int _oldEmploy = -1;
    private Guid _idL;
    private GameObject _plusBtn;
    private GameObject _lessBtn;

    private GameObject _hireAllBtn;
    private GameObject _fireAllBtn;
    private string _hireFireAllAction = "";


    internal static WorkerTile CreateTile(Transform container, string workType, Vector3 iniPos, WorkerTile workerTile = null)
    {
        WorkerTile obj = workerTile;

        if (workerTile == null)
            obj = (WorkerTile)Resources.Load(Root.worker_Tile, typeof(WorkerTile));

        obj = (WorkerTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var iniScale = obj.transform.localScale;
        obj.transform.SetParent(container);
        obj.transform.localPosition = iniPos;
        obj.transform.localScale = iniScale;

        obj.WorkType = workType;

        return obj;
    }

    private void Start()
    {
        _idL = Guid.NewGuid();

        _plusBtn = GetChildCalled("More");
        _lessBtn = GetChildCalled("Less");

        _hireAllBtn = GetChildCalled("Hire All");
        _fireAllBtn = GetChildCalled("Fire All");

        _descText.text = Languages.ReturnString(_workType);

        Init();
    }

    private void Init()
    {
        _buildings = BuildingController.FindAllStructOfThisTypeAndFullyBuilt(_workType);

        if (_employ == -1)
            _employ = MaxPeople();

        if (_oldEmploy != _employ && _oldEmploy != -1)
        {
            TakeActionOnNewNumber();
            _employ = MaxPeople();

            //update _employ and _oldEmply in my sibling
            UpdateEmploysOnMySibling();
        }
        _oldEmploy = _employ;

        _currentText.text = _employ + "";
        _totalText.text = "" + AbsMaxPeople() + "";
    }

    private void UpdateEmploysOnMySibling()
    {
        var workerTiles = FindObjectsOfType<WorkerTile>();

        for (int i = 0; i < workerTiles.Length; i++)
        {
            workerTiles[i].UpdateEmploysOnlyOnMyUniqueSibling(_employ, WorkType, _idL);
        }
    }

    //used when a user is updating on the bulleting window
    private void UpdateEmploysOnlyOnMyUniqueSibling(int employ, string workType, Guid idL)
    {
        if (_idL == idL || WorkType != workType) return;
        _employ = employ;
        _oldEmploy = employ;
    }

    private void TakeActionOnNewNumber()
    {
        if (_oldEmploy > _employ)
        {
            FirePeople();
        }
        else
        {
            HirePeople();
        }
    }

    private void HirePeople()
    {
        var newHires = _employ - _oldEmploy;
        ChangeEmployesBy(newHires);
    }

    private void ChangeEmployesBy(int newEmploys)
    {
        if (newEmploys == 0)
        {
            return;
        }

        var indexBuild = UMath.GiveRandom(0, _buildings.Count);
        var build = _buildings[indexBuild];
        newEmploys = build.CanYouChangeOne(newEmploys);

        ChangeEmployesBy(newEmploys);
    }

    private void FirePeople()
    {
        var firePeople = _oldEmploy - _employ;
        ChangeEmployesBy(-firePeople);
    }

    private int MaxPeople()
    {
        int res = 0;
        for (int i = 0; i < _buildings.Count; i++)
        {
            res += _buildings[i].MaxPeople;
        }
        return res;
    }

    private int AbsMaxPeople()
    {
        int res = 0;
        for (int i = 0; i < _buildings.Count; i++)
        {
            res += _buildings[i].AbsMaxPeople;
        }
        return res;
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ClickLessSign()
    {
        _employ--;
        Init();
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ClickPlusSign()
    {
        if (WorkType == "Masonry")
            Program.gameScene.TutoStepCompleted("AddWorkers.Tuto");

        _employ++;
        Init();
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ClickFireAllSign()
    {
        _hireFireAllAction = "Fire All";
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ClickHireAllSign()
    {
        _hireFireAllAction = "Hire All";
    }

    private void CheckIfPlusIsActive()
    {
        if (MaxPeople() >= AbsMaxPeople() || MyText.Lazy() == 0)
        {
            MakeInactiveButton(_plusBtn);
            MakeInactiveButton(_hireAllBtn);
        }
        else
        {
            MakeActiveButton(_plusBtn);
            MakeActiveButton(_hireAllBtn);
        }
    }

    private void CheckIfLessIsActive()
    {
        if (MaxPeople() == 0)
        {
            MakeInactiveButton(_lessBtn);
            MakeInactiveButton(_fireAllBtn);
        }
        else
        {
            MakeActiveButton(_lessBtn);
            MakeActiveButton(_fireAllBtn);
        }
    }

    private void MakeInactiveButton(GameObject btn)
    {
        btn.SetActive(false);
    }

    private void MakeActiveButton(GameObject btn)
    {
        btn.SetActive(true);
    }

    private void Update()
    {
        if (_buildings == null)
        {
            return;
        }
        Init();

        CheckIfLessIsActive();
        CheckIfPlusIsActive();
        HireFireAll();
    }

    private void HireFireAll()
    {
        if (_hireFireAllAction == "Fire All")
        {
            if (_lessBtn.activeSelf)
            {
                ClickLessSign();
            }
            else _hireFireAllAction = "";
        }
        else if (_hireFireAllAction == "Hire All")
        {
            if (_plusBtn.activeSelf)
            {
                ClickPlusSign();
            }
            else _hireFireAllAction = "";
        }
    }
}