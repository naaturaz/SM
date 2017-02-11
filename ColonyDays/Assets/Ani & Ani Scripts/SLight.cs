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



    float lastCheck;
    void Update()
    {
        if (Time.time < lastCheck + randomTime)
        {
            return;
        }

        lastCheck = Time.time;
        if (_stageManager.IsSunsetOrLater() && _light.intensity == 0 && GameController.AreThereWhaleOil)
        {
            _light.intensity = 1.1f;
            TurnThisOneOn(_nightGlass);
            randomTime = UMath.GiveRandom(5f, 10f);
        }
        else if (_stageManager.IsDawnOrLater() && _light.intensity > 0)
        {
            _light.intensity = 0f;
            TurnThisOneOn(_dayGlass);
            GameController.ResumenInventory1.Remove(P.WhaleOil, 1f);
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

