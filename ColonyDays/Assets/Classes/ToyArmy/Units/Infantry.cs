using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : Commandable
{

    // Use this for initialization
    protected void Start()
    {
        if (name.Contains("Enemy"))
        {
            IsGood = false;
        }

        base.Start();

        Program.InputMain.ChangeSpeed += ChangedSpeedHandler;
        Program.gameScene.ChangeSpeed += ChangedSpeedHandler;
        GameController.War += WarHandler;
    }

    private void WarHandler(object sender, EventArgs e)
    {

    }

    private void ChangedSpeedHandler(object sender, EventArgs e)
    {
        MilitarBody.NewSpeed();
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
    }

    void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
