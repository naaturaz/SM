using UnityEngine;

public class MyForm : General 
{
    GameObject _canvas;
    GameObject _panel;

    public GameObject Canvas
    {
        get
        {
            if (_canvas == null)
            {
                return GetChildCalled(H.Canvas, gameObject);
            }
            return _canvas;
        }
        set
        {
            if (_canvas == null)
            {
                _canvas = GetChildCalled(H.Canvas, gameObject);
            }
            _canvas = value;
        }
    }

    public GameObject Panel
    {
        get
        {
            if (_panel == null)
            {
                return GetChildCalled(H.Panel, Canvas);
            }
            return _panel;
        }
        set
        {
            if (_panel == null)
            {
                _panel = GetChildCalled(H.Canvas, Canvas);
            }
            _panel = value;
        }
    }

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {}

    protected GameObject GetChildCalled(H childName, GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).name == childName.ToString())
            {
                return parent.transform.GetChild(i).gameObject;
            }
        }
        print("Obj doesnt have a child called " + childName);
        return null;
    }

    //Returns true if is overlapping the Panel 2dCollider
    //Panel must have a 2d collidaer attached
    public bool IsOverLapingPanel(Vector2 pos)
    {
        Collider2D c = Panel.GetComponent<Collider2D>();
        
        if(c.OverlapPoint(pos))
        {
            return true;
        }
        return false;
    }
}
