using UnityEngine;

public class MyForm : General 
{
    GameObject _canvas;
    GameObject _panel;
    GameObject _resources;

    private GameObject _startPosIni;

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

    public GameObject Resources
    {
        get
        {
            if (_resources == null)
            {
                return GetChildCalled(H.Resources, Canvas);
            }
            return _resources;
        }
        set
        {
            if (_resources == null)
            {
                _resources = GetChildCalled(H.Resources, Canvas);
            }
            _resources = value;
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
                _panel = GetChildCalled(H.Panel, Canvas);
            }
            _panel = value;
        }
    }

	// Use this for initialization
	void Start ()
	{
        _canvas = GetChildCalled(H.Canvas, gameObject);
        _resources = FindGameObjectInHierarchy("Resources", Canvas);


        //if resource is null is another Form. The MainMenu Form 
        if (_resources != null)
	    {
            _startPosIni = GetChildCalled(H.Start, Resources);
	        LoadMainInventory();   
	    }

	}
	
	// Update is called once per frame
    void Update()
    {
        if (_showAInventory!=null)
        {
            //_showAInventory.Update();
        } 
    }

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

    private ShowAInventory _showAInventory;
    internal void LoadMainInventory()
    {
        _showAInventory = new ShowAInventory("Main", Resources, _startPosIni.transform.localPosition);
    }
}
