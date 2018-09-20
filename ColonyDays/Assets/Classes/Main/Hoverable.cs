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
        HideHelp();
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

        if (HType == H.Person ||
            Category == Ca.Structure || Category == Ca.Shore || Category == Ca.Way ||
            //Category == Ca.Spawn ||
            HType == H.BuyRegion)
        {
            hoverWindow.ShowExplicitThis(Name);
        }
        //Construction Sign
        //bz if more than 6 he know how to build already 
        else if (transform.name == "Construction" && BuildingPot.Control.Registro.AllBuilding.Count < 10)
        {
            hoverWindowMed.Show(MousePositionTowardsScreenCenter(), transform.name);
        }
        //Demolition Sign
        else if (transform.name == "Demolition")
        {
            hoverWindowMed.Show(MousePositionTowardsScreenCenter(), transform.name);
        }
    }
    
    public static Vector2 MousePositionTowardsScreenCenter()
    {
        var dir = ReturnScreenQuadrantMousePosition();

        int far = 50;
        Vector2 add = new Vector2();
        if(dir == Dir.NW)
        {
            add = new Vector2(far, -far);
        }
        else if (dir == Dir.NE)
        {
            add = new Vector2(-far, -far);
        }
        else if (dir == Dir.SE)
        {
            add = new Vector2(-far, far);
        }
        else if (dir == Dir.SW)
        {
            add = new Vector2(far, far);
        }

        var mp = (Vector2)Input.mousePosition;
        return mp + add;
    }

    //x:0 and y:0 are in the bottom left of the screen
    static Dir ReturnScreenQuadrantMousePosition()
    {
        var w = Screen.width / 2;
        var h = Screen.height / 2;
        Vector2 center = new Vector2(w, h);
        Vector2 mp = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        if (mp.x < center.x && mp.y > center.y) return Dir.NW;
        if (mp.x > center.x && mp.y > center.y) return Dir.NE;
        if (mp.x > center.x && mp.y < center.y) return Dir.SE;
        if (mp.x < center.x && mp.y < center.y) return Dir.SW;

        return Dir.None;
    }

    /// <summary>
    /// For unity event
    /// </summary>
    void HideHelp()
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

