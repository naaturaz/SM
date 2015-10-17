using System.Collections.Generic;
using UnityEngine;

//they used to be the Prevoew Way now is only use to see how far this colliders go 
//so i can define bounds 

public class PreviewWay : Building {

    private bool _isActiveElement = true;//is the active element of a way
    private Way way;
    //this is the radues of the sphere that search thru to see what was collided when was fixed to terrain
    private float radius = 2f;

    public float Radius
    {
        get { return radius; }
        set { radius = value; }
    }

    public override void Destroy()
    {
        _isActiveElement = false;
        base.Destroy();
    }

    // Use this for initialization
	void Start ()
    {
        //InitializeMatColors();
	}
    
    /// <summary>
    /// Initilize the color of this obj InitialColor prop.
    /// Will keep it like that if all good or red if is not good.
    /// This is not needed bz this obj is not being rendered
    /// </summary>
    void InitializeMatColors()
    {
        way = (Way)BuildingPot.Control.CurrentSpawnBuild;
        InitialColor = Geometry.GetComponent<Renderer>().material.color;

        if (!way.IsWayOk)
        {
            Geometry.GetComponent<Renderer>().material.color = Color.red;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Will be useful if this class instances were rendered but thhere are not
        //if (_isActiveElement)
        //{
        //    CheckAndSwitchColor();
        //}
	}

    void CheckAndSwitchColor()
    {
        if (Geometry.GetComponent<Renderer>().material.color == InitialColor && !way.IsWayOk)
        {
            Geometry.GetComponent<Renderer>().material.color = Color.red;
        }
        else if (Geometry.GetComponent<Renderer>().material.color == Color.red && way.IsWayOk)
        {
            Geometry.GetComponent<Renderer>().material.color = InitialColor;
        }
    }

    /// <summary>
    /// Gets the bounds of the obj was created here bz we dont set the bounds here automatically
    /// </summary>
    public List<Vector3> GetBounds()
    {
        UpdateMinAndMaxVar();
        Bounds = FindBounds(Min, Max);
        return Bounds;
    }

    /// <summary>
    /// Gets the anchors of the obj was created here bz we dont set the anchors here automatically
    /// </summary>
    public List<Vector3> GetAnchors()
    {
        return FindAnchors(GetBounds());
    }
}
