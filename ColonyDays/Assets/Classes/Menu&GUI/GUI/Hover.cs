using UnityEngine;

/*
 * Game obj tht have this script wioll pop up small form with help as is being hover 
 */

public class Hover : MonoBehaviour
{
    private Rect myRect;//the rect area of my element. Must have attached a BoxCollider2D
    private HoverWindow hoverWindow;//the window tht will pop up msg
    private Rect screenRect;

	// Use this for initialization
	void Start ()
	{
	    InitObjects();
	}

    void InitObjects()
    {       
        //for this to work only one gameObj can have the HoverWindow attached
        if (hoverWindow == null)
        {
            hoverWindow = FindObjectOfType<HoverWindow>();
        }

        myRect = GetRectFromBoxCollider2D();
        screenRect = DefineScreenRect();
    }

    Rect DefineScreenRect()
    {
        Rect res = new Rect();
        res.min = new Vector2();
        res.max = new Vector2(Screen.width, Screen.height);


        return res;
    }

    Rect GetRectFromBoxCollider2D()
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
	void Update ()
    {
        //if got in my area
	    if (myRect.Contains(Input.mousePosition))
	    {
            SpawnHelp();
	    }
        //ig got out 
        else if (!myRect.Contains(Input.mousePosition) && MyMsg() == hoverWindow.CurrentMsg())
	    {
	        DestroyHelp();  
	    }
	}

    string MyMsg()
    {
        return transform.name;
    }

    void SpawnHelp()
    {
        hoverWindow.Show(ReturnHoverPos(), MyMsg());
        CorrectHoverWindowPos();
    }

    Vector3 ReturnHoverPos()
    {
        Vector3 res = myRect.center;
        Vector2 twoD = new Vector2(res.x, res.y + 20f);

        ////if is too far on top
        //if (!screenRect.Contains(twoD))
        //{
        //    twoD.y -= 40f;
        //}

        return twoD;
    }

    /// <summary>
    /// Will correct the position if is a off screen
    /// </summary>
    void CorrectHoverWindowPos()
    {
        
    }

    void DestroyHelp()
    {
        hoverWindow.Hide();
    }
}
