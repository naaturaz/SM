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

    void Start()
    {
        _light = GetComponent<Light>();
        _stageManager = FindObjectOfType<StageManager>();

        StartCoroutine("RandomUpdate5to10");

    }

    private float randomTime;
    private IEnumerator RandomUpdate5to10()
    {
        while (true)
        {
            yield return new WaitForSeconds(randomTime); // wait
            randomTime = UMath.GiveRandom(5f, 10f);

            LightUpdate();
        }
    }

    void Update()
    {
    }

    void LightUpdate()
    {
        if (_stageManager.IsSunsetOrLater() && _light.intensity == 0)
        {
            _light.intensity = 1.1f;
        }

        if (_stageManager.IsDawnOrLater() && _light.intensity > 0)
        {
            _light.intensity = 0f;
        }
    }

}

