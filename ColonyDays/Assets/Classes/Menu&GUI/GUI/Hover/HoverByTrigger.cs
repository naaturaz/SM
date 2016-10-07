using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;


class HoverByTrigger : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    private HoverWindow hoverWindow;//the window tht will pop up msg

    void Start()
    {
        if (hoverWindow == null)
        {
            hoverWindow = FindObjectOfType<HoverWindow>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("enter:"+eventData);
        PublicSpawnHelp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("exitt:" + eventData);
        PublicDestroyHelp();
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.KeyUp || Event.current.type == EventType.MouseUp)
        {
            PublicDestroyHelp();
        }
    }





    /// <
    /// summary>
    /// For unity eventTrigger
    /// 
    /// </summary>
    void PublicSpawnHelp()
    {

        var pos = MoveItTowardsScreenCenter(Input.mousePosition);
        hoverWindow.Show(pos, MyMsg());
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
        hoverWindow.Hide();
    }

    string MyMsg()
    {
        return transform.name;
    }
}

