using UnityEngine;
using System.Collections;

public class OnScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    //HUDFPS.Message += ".vis:." + IsVisible();
	}

    void OnWillRenderObject()
    {
       // print(gameObject.name + " is being rendered by " + Camera.current.name + " at " + Time.time);
    }

    private bool visible;
    public bool Visible()
    {
        return visible;
    }

    void OnBecameVisible()
    {
        visible = true;
        //print("OnBecameVisible");
    }

    void OnBecameInvisible()
    {
        visible = false;
        //print("OnBecameInvisible");
    }
}
