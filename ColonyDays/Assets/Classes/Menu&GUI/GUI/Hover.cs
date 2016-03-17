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
    }

    private Vector3 ReturnHoverPos()
    {
        return MoveItTowardsScreenCenter(myRect);
    }

    private Vector2 MoveItTowardsScreenCenter(Rect myRectP)
    {
        var w = Screen.width / 2;
        var h = Screen.height / 2;

        //so its depending on the screen size. roughly 45 px
        var howFar = h / 9;

        Vector2 center = new Vector2(w, h);
        var moved = Vector2.MoveTowards(myRectP.center, center, howFar);
        return moved;
    }

    void DestroyHelp()
    {
        hoverWindow.Hide();
    }
}
