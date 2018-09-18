using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Buildings, Persons, and StillElements will be able to show their name 
/// 
/// Also Signs will show information
/// 
/// The object calling this must have a box collider or a collider for this to work
/// </summary>
public class Hoverable : General
{
    private HoverWindow hoverWindow;//the window tht will pop up msg
    HoverWindowMed hoverWindowMed;

    void Start()
    {
        if (hoverWindowMed == null)
        {
            hoverWindow = FindObjectOfType<HoverWindow>();
            hoverWindowMed = FindObjectOfType<HoverWindowMed>();

        }
    }

    protected void OnMouseEnter()
    {
        //so dont show on top of other while placing a building 
        if (BuildingPot.InputMode == Mode.Placing)
        {
            return;
        }
        if (Program.MouseListener.IsAWindowShownNow() || CamControl.IsMainMenuOn())
        {
            return;
        }

        PublicSpawnHelp();
    }

    protected void OnMouseExit()
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
        if (hoverWindowMed == null)
        {
            hoverWindow = FindObjectOfType<HoverWindow>();
            hoverWindowMed = FindObjectOfType<HoverWindowMed>();
        }

        if (hoverWindowMed == null)
        {
            return;
        }

        var pos = MoveItTowardsScreenCenter(CorrectMouseCenterPos());

        if (HType == H.Person ||
            Category == Ca.Structure || Category == Ca.Shore || Category == Ca.Way ||
            //Category == Ca.Spawn ||
            HType == H.BuyRegion)
        {
            hoverWindow.ShowExplicitThis(pos, Name);
        }
        //Construction Sign
        //bz if more than 6 he know how to build already 
        else if (transform.name == "Construction" && BuildingPot.Control.Registro.AllBuilding.Count < 10)
        {
            hoverWindowMed.Show(pos, transform.name);
        }
        //Demolition Sign
        else if (transform.name == "Demolition")
        {
            hoverWindowMed.Show(pos, transform.name);
        }
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
        if (hoverWindowMed == null)
        {
            return;
        }
        hoverWindow.Hide();
        hoverWindowMed.Hide();
    }


    private void OnDestroy()
    {
        //if (hoverWindowMed == null)
        //{
        //    return;
        //}

        //hoverWindowMed.Hide();
    }
}

