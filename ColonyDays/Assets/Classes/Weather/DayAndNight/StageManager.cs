﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class StageManager : General
{
    List<GameObject> _stages = new List<GameObject>();
    Light _main;
    Light _west;
    Light _east;

    int _startStage = 2;
    int _currentStage = 2;



    float _daySpeed = .5f;
    float _nightSpeed = .5f;//.6

    bool _isOnTransition;

    Vector3 _center;

    ColorManager _colorManager;

    H _currentCycle = H.Day;//day or night

    /// <summary>
    /// times to move from a stage
    /// Fox ex
    /// in stage 0 will move at 1sec
    /// in stage 1 will move at 20sec
    /// 
    /// At night time is the same but divided / 10
    /// </summary>
    List<float> _times = new List<float>() { 1, 20, 120, 120, 20, 5 };

    float _startedCycleAt = 0;

    void Start()
    {
        _stages.Add(GetChildCalled("Stage0"));
        _stages.Add(GetChildCalled("Stage1"));
        _stages.Add(GetChildCalled("Stage2"));
        _stages.Add(GetChildCalled("Stage3"));
        _stages.Add(GetChildCalled("Stage4"));
        _stages.Add(GetChildCalled("Stage5"));

        var mainLig = GameObject.Find("Directional light Main 1.8");
        _main = mainLig.GetComponent<Light>();

        var bLigW = GameObject.Find("Directional light Back 1.1 West");
        _west = bLigW.GetComponent<Light>();

        var bLigE = GameObject.Find("Directional light Back 1.1 East");
        _east = bLigE.GetComponent<Light>();

        _startedCycleAt = Time.time;

        _center = GetChildCalled("Center").transform.position;

        _colorManager = FindObjectOfType<ColorManager>();
    }

    void Update()
    {
        if (Program.gameScene.GameSpeed == 0)
        {
            return;
        }

        CheckIfMoveStages();

        CheckIfInTrans();
    }

    /// <summary>
    /// Checks if is in transition
    /// </summary>
    private void CheckIfInTrans()
    {
        if (!_isOnTransition)
        {
            return;
        }

        if (FindStateOfLight() != _currentStage)
        {
            BlendIntoNextStage();
        }
        else
        {
            _isOnTransition = false;
        }
    }

    /// <summary>
    /// Blends the curretn position and state into the next one
    /// </summary>
    private void BlendIntoNextStage()
    {
        var transfTo = _stages[_currentStage].transform.position;
        float step = ReturnSpeed() * Time.deltaTime;

        _main.transform.position = Vector3.MoveTowards(_main.transform.position, transfTo, step);
        _main.transform.LookAt(_center);

        ReachNextColors();
        ReachNextIntensities();
    }

    private void ReachNextIntensities()
    {
        float step = ReturnSpeed() * Time.deltaTime;

        //
        var mainCurr = _main.intensity;
        var newMain = Mathf.Lerp(mainCurr,
            _colorManager.GetMeMainIntensity(_currentStage, _currentCycle), step);
        _main.intensity = newMain;


        // 
        var wCurr = _west.intensity;
        var newW = Mathf.Lerp(wCurr,
            _colorManager.GetMeWestIntensity(_currentStage, _currentCycle), step);
        _west.intensity = newW;

        //
        var eCurr = _east.intensity;
        var newE = Mathf.Lerp(eCurr,
            _colorManager.GetMeEastIntensity(_currentStage, _currentCycle), step);
        _east.intensity = newE;
    }




    private void ReachNextColors()
    {
        float step = ReturnSpeed() * Time.deltaTime;//bz color doest finish blending

        var mainCurr = _main.color;
        var newMain = Color.Lerp(mainCurr,
            _colorManager.GetMeMainColor(_currentStage, _currentCycle), step);
        _main.color = newMain;


        var mainAmbience = RenderSettings.ambientLight;
        var newAmbience = Color.Lerp(mainAmbience,
            _colorManager.GetMeAmbienceColor(_currentStage, _currentCycle), step);
        RenderSettings.ambientLight = newAmbience;
    }

    /// <summary>
    /// Checks if needs to move to next stage 
    /// </summary>
    private void CheckIfMoveStages()
    {
        if (Time.time > _startedCycleAt + WaitTime() && !_isOnTransition)
        {
            MoveToNextStage();
            _startedCycleAt = Time.time;

            Debug.Log("Moving to:" + _currentStage);
        }
    }
    
    float ReturnSpeed()
    {
        if (_currentCycle == H.Day)
        {
            return _daySpeed;
        }
        //night time is a fifth of the day
        return _nightSpeed;
    }

    /// <summary>
    /// Wait time in current stage 
    /// </summary>
    /// <returns></returns>
    float WaitTime()
    {
        if (_currentCycle == H.Day)
        {
            return _times[_currentStage];
        }
        //night time is a fifth of the day
        return _times[_currentStage]/10;
    }

    /// <summary>
    /// Moves to next stage 
    /// </summary>
    private void MoveToNextStage()
    {
        _currentStage = UMath.GoAround(1, _currentStage, 0, 5);

        if (_currentStage == 0)
        {
            //toggle day , night
            ToggleDayCycle();
        }

        _isOnTransition = true;
    }

    private void ToggleDayCycle()
    {
        if (_currentCycle == H.Day)
        {
            _currentCycle = H.Night;
        }
        else
        {
            _currentCycle = H.Day;

        }
    }

    /// <summary>
    /// Wil return current exact pos of light, if found in one stage, otherwisw
    /// ret -1
    /// </summary>
    /// <returns></returns>
    int FindStateOfLight()
    {
        for (int i = 0; i < _stages.Count; i++)
        {
            if (UMath.nearEqualByDistance(_main.transform.position, _stages[i].transform.position, 0.02f))
            {
                return i;
            }
        }
        return -1;
    }

    internal bool IsSunsetOrLater()
    {
        return _currentCycle == H.Night || (_currentCycle == H.Day && _currentStage > 4);
    }

    internal bool IsMidNightOrLater()
    {
        return _currentCycle == H.Night  && _currentStage > 2;
    }

    internal bool IsDawnOrLater()
    {
        return _currentCycle == H.Day && _currentStage > 0;
    }
}
