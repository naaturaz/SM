using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : General
{

    GameObject _healthBarGO;
    Shooter _shot;
    int _oldHealth;
    int _fullHealth;

    float _xWorthXHealthUnit;

    public void PassShooter(Shooter shot)
    {
        _shot = shot;
        _oldHealth = _shot.Health;
        _fullHealth = _oldHealth;

        _xWorthXHealthUnit = 1 / (float)_fullHealth;
    }

    // Use this for initialization
    void Start()
    {
        _healthBarGO = GetChildCalled("Health_Bar_Green");
    }

    // Update is called once per frame
    void Update()
    {
        if (_shot == null)
        {
            return;
        }

        if (_oldHealth != _shot.Health)
        {
            _oldHealth = _shot.Health;

            var newXVal = _xWorthXHealthUnit * _shot.Health;
            if (newXVal > 1)
            {
                newXVal = 1;
            }
            _healthBarGO.transform.localScale = new Vector3(newXVal, 1, 1);
        }
    }
}
