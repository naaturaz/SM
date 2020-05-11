using UnityEngine;

/*
 * Game obj tht have this script wioll pop up small form with help as is being hover
 *
 * This GObj it spawns it doesnt move wit mouse
 */

public class HoverBtn : MonoBehaviour
{
    private HoverWindow hoverWindow;//the window tht will pop up msg
    private Rect screenRect;

    // Use this for initialization
    private void Start()
    {
        InitObjects();
    }

    private void InitObjects()
    {
        //for this to work only one gameObj can have the HoverWindow attached
        if (hoverWindow == null)
        {
            hoverWindow = FindObjectOfType<HoverWindow>();
        }

        screenRect = DefineScreenRect();
    }

    private Rect DefineScreenRect()
    {
        Rect res = new Rect();
        res.min = new Vector2();
        res.max = new Vector2(Screen.width, Screen.height);

        return res;
    }

    private Rect GetRectFromBoxCollider2D()
    {
        var res = new Rect();
        var min = transform.GetComponent<BoxCollider2D>().bounds.min;
        var max = transform.GetComponent<BoxCollider2D>().bounds.max;

        res = new Rect();
        res.min = min;
        res.max = max;

        return res;
    }

    // Update is called once per frame
    private void Update()
    {
        if (hoverWindow == null)
        {
            if (Time.time % 2 == 0)
            {
                hoverWindow = FindObjectOfType<HoverWindow>();
            }
            return;
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    private string MyMsg()
    {
        return transform.name;
    }

    private static string oldMsg = "";

    private void SpawnHelp()
    {
        if (oldMsg == MyMsg())
            return;

        oldMsg = MyMsg();

        //hoverWindow.ShowMsg(ReturnHoverPos(), MyMsg());
        hoverWindow.ShowMsg(MyMsg());
    }

    private Vector2 MoveItTowardsScreenCenter(Vector3 iniPos)
    {
        var w = Screen.width / 2;
        var h = Screen.height / 2;

        //so its depending on the screen size. roughly 45 px
        var howFar = h / 9;

        Vector2 center = new Vector2(w, h);
        var moved = Vector2.MoveTowards(iniPos, center, howFar);
        return moved;
    }

    private void DestroyHelp()
    {
        hoverWindow.Hide();
    }

    //Called from GUI

    public void CallMeWhenMouseEnter()
    {
        SpawnHelp();
    }

    public void CallMeWhenMouseExit()
    {
        DestroyHelp();
    }
}