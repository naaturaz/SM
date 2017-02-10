using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class SLight : General
{
    Light _light;
    StageManager _stageManager;

    GameObject _nightGlass;
    GameObject _dayGlass;

    private float randomTime = 5;

    void Start()
    {
        _light = GetComponent<Light>();
        _stageManager = FindObjectOfType<StageManager>();

        _nightGlass = GetChildCalled("Night_Glass");
        _dayGlass = GetChildCalled("Day_Glass");

        if (_nightGlass!= null)
        {
            _nightGlass.SetActive(false);
        }

        
    }


    void Update()
    {
        if (_stageManager.IsSunsetOrLater() && _light.intensity == 0)
        {
            _light.intensity = 1.1f;
            TurnThisOneOn(_nightGlass);
            randomTime = UMath.GiveRandom(5f, 10f);
        }

        if (_stageManager.IsDawnOrLater() && _light.intensity > 0)
        {
            _light.intensity = 0f;
            TurnThisOneOn(_dayGlass);
        }
    }

    private void TurnThisOneOn(GameObject gass)
    {
        //not a lamp
        if (gass == null)
        {
            return;
        }

        _nightGlass.SetActive(false);
        _dayGlass.SetActive(false);

        gass.SetActive(true);
    }

}

