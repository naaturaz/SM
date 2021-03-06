﻿using System.Collections.Generic;
using UnityEngine;

public class StageManager : General
{
    private List<GameObject> _stages = new List<GameObject>();
    private Light _main;
    private Light _west;
    private Light _east;

    private int _currentStage = 2;//_startStage = 2

    /// <summary>
    /// This is the shared material for all AlphaAtlasShake instances . here I change the main
    /// color so at night is not white
    /// </summary>
    private Material _waveMat;

    //this speeds both limit how quick they go from point to point. They are a bottle
    //neck if a time is set to 1sec for ex
    private float _daySpeed = .5f;

    private float _nightSpeed = 1f;

    private bool _isOnTransition;
    private Vector3 _center;

    private ColorManager _colorManager;
    private H _currentCycle = H.Day;//day or night

    /// <summary>
    /// times to move from a stage
    /// Fox ex
    /// in stage 0 will move at 1sec
    /// in stage 1 will move at 20sec
    ///
    /// At night time is the same but divided / 10
    /// </summary>
    private List<float> _times = new List<float>() { 1, 20, 120, 120, 5, 5 };

    //List<float> _times = new List<float>() { 1, 5, 5, 5, 1, 5 };

    private float _startedCycleAt = 0;

    private void Start()
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

        _waveMat = GetChildCalled("WaveMaterial").GetComponent<Renderer>().sharedMaterial;
        _waveMat.color = _colorManager.DayGrass;
    }

    private void Update()
    {
        if (!Program.gameScene || Program.gameScene.GameSpeed == 0) return;

        //will block further progression if was turned off
        if (_currentStage == 2 && !Settings.ISDay && _currentCycle == H.Day) return;

        //#if UNITY_EDITOR
        //        return;
        //#endif

        CheckIfMoveStages();
        CheckIfInTrans();
    }

    /// <summary>
    /// Checks if is in transition
    /// </summary>
    private void CheckIfInTrans()
    {
        if (!_isOnTransition)
            return;

        if (FindStateOfLight() != _currentStage)
            BlendIntoNextStage();
        else
            _isOnTransition = false;
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

        if (CamControl.CAMRTS != null)
        {
            var currentColBack = CamControl.CAMRTS.GetCameraBackColor();

            var newColBack = Color.Lerp(currentColBack, _colorManager.GetMeCameraBackGroundColor(_currentCycle), step);
            CamControl.CAMRTS.AssignBackGroundColor(newColBack);
        }

        ReachNewColorForWaveGrass();
    }

    #region Grass Wave

    private void ReachNewColorForWaveGrass()
    {
        float step = ReturnSpeed() * Time.deltaTime;//bz color doest finish blending
        var newAmbience = Color.Lerp(_waveMat.color, ColorWaveColorTarget(), step);
        _waveMat.color = newAmbience;
    }

    private Color ColorWaveColorTarget()
    {
        if (_currentCycle == H.Day && (_currentStage == 2 || _currentStage == 3))
        {
            return _colorManager.DayGrass;
        }
        return RenderSettings.ambientLight;
    }

    #endregion Grass Wave

    /// <summary>
    /// Checks if needs to move to next stage
    /// </summary>
    private void CheckIfMoveStages()
    {
        if (Time.time > _startedCycleAt + WaitTime() && !_isOnTransition)
        {
            MoveToNextStage();
            _startedCycleAt = Time.time;
            //Debug.Log("Moving to:" + _currentStage);
        }
    }

    private float ReturnSpeed()
    {
        if (_currentCycle == H.Day)
            return _daySpeed;
        //night time is a quicker
        return _nightSpeed;
    }

    /// <summary>
    /// Wait time in current stage
    /// </summary>
    /// <returns></returns>
    private float WaitTime()
    {
        if (_currentCycle == H.Day)
        {
            return _times[_currentStage] / 1;//1
        }
        //night time
        return _times[_currentStage] / 10;//10
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
            _currentCycle = H.Night;
        else
            _currentCycle = H.Day;
    }

    /// <summary>
    /// Wil return current exact pos of light, if found in one stage, otherwisw
    /// ret -1
    /// </summary>
    /// <returns></returns>
    private int FindStateOfLight()
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
        return _currentCycle == H.Night && _currentStage > 2;
    }

    internal bool IsDawnOrLater()
    {
        return _currentCycle == H.Day && _currentStage > 0;
    }

    public void OptionsDayCycleWasToggled()
    {
        if (!Settings.ISDay)
        {
            _isOnTransition = false;
            _currentCycle = H.Day;
            _currentStage = 2;
            _main.transform.position = _stages[_currentStage].transform.position;
            _main.transform.LookAt(_center);

            _main.intensity = _colorManager.GetMeMainIntensity(_currentStage, _currentCycle);
            _west.intensity = _colorManager.GetMeWestIntensity(_currentStage, _currentCycle);
            _east.intensity = _colorManager.GetMeEastIntensity(_currentStage, _currentCycle);

            _main.color = _colorManager.GetMeMainColor(_currentStage, _currentCycle);
            RenderSettings.ambientLight = _colorManager.GetMeAmbienceColor(_currentStage, _currentCycle);

            if (CamControl.CAMRTS != null)
            {
                var newColBack = _colorManager.GetMeCameraBackGroundColor(_currentCycle);
                CamControl.CAMRTS.AssignBackGroundColor(newColBack);
            }

            _waveMat.color = ColorWaveColorTarget();
        }
    }
}