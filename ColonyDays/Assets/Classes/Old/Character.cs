using UnityEngine;
using System.Collections;

public class Character : General {

    //Properties
    private int _lives;
    private bool _showBlockedArea = false;

    public int Lives
    {
        get { return _lives; }
        set { _lives = value; }
    }

    public bool ShowBlockedArea
    {
        get { return _showBlockedArea; }
        set { _showBlockedArea = value; }
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
    internal new void Update() 
    {
        //print(ShowBlockedArea);
        base.Update();//execute the update method in base

        if (ShowBlockedArea)
        {
            ActivateChildObject(true, "Show_Blocked_Area");
        }
        else if (!ShowBlockedArea)
        {
            ActivateChildObject(false, "Show_Blocked_Area");
        }
	}

    //activate or deactivae a child gameobject...
    void ActivateChildObject(bool boolPass, string whichChild)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == whichChild)
            {
                transform.GetChild(i).gameObject.SetActive(boolPass);
            }
        }
    }
}
