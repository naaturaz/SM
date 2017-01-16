﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


/// <summary>
/// Meant for a 3d gameobject not GUI
/// </summary>

public class Hover3d : General
{
    //private HoverWindow hoverWindow;//the window tht will pop up msg
    HoverWindowMed hoverWindowMed;

    void Start()
    {
        if (hoverWindowMed == null)
        {
            //hoverWindow = FindObjectOfType<HoverWindow>();
            hoverWindowMed = FindObjectOfType<HoverWindowMed>();

        }
    }

    void OnMouseEnter()
    {
        //so dont show on top of other while placing a building 
        if (BuildingPot.InputMode == Mode.Placing)
        {
            return;
        }

        PublicSpawnHelp();

    }

    void OnMouseExit()
    {
        PublicDestroyHelp();

    }

    /// <
    /// summary>
    /// For unity eventTrigger
    /// 
    /// </summary>
    void PublicSpawnHelp()
    {
        var pos = MoveItTowardsScreenCenter(CorrectMouseCenterPos());
        //hoverWindow.Show(pos, MyMsg());
        hoverWindowMed.Show(pos, MyMsg());
    }

    /// <summary>
    /// If mouse is too close to center on the screen want to add a bit
    /// on Y so it doesnt keep entering and exiting bz the hover window is
    /// being spawned on it 
    /// </summary>
    /// <returns></returns>
    Vector3 CorrectMouseCenterPos()
    {
        var half = Screen.height / 2;
        var difference = Input.mousePosition.y - half;

        //means is in the middle of the screen
        if (Math.Abs(difference) < 220)
        {
            return Input.mousePosition + new Vector3(0, 100, 0);
        }
        return Input.mousePosition;
    }

    private Vector2 MoveItTowardsScreenCenter(Vector3 v3)
    {
        var w = Screen.width / 2;
        var h = Screen.height / 2;

        //so its depending on the screen size. roughly +45 px
        var howFar = h / 7;//9



        Vector2 center = new Vector2(w, h);
        var moved = Vector2.MoveTowards(v3, center, howFar);
        return moved;
    }

    /// <summary>
    /// For unity event
    /// </summary>
    void PublicDestroyHelp()
    {
        hoverWindowMed.Hide();
    }

    string MyMsg()
    {
        return transform.name;
    }
}

