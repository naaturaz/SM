using UnityEngine;

public class OnScreen : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //HUDFPS.Message += ".vis:." + IsVisible();
    }

    private void OnWillRenderObject()
    {
        // print(gameObject.name + " is being rendered by " + Camera.current.name + " at " + Time.time);
    }

    private bool visible;

    public bool Visible()
    {
        return visible;
    }

    private void OnBecameVisible()
    {
        visible = true;
        //print("OnBecameVisible");
    }

    private void OnBecameInvisible()
    {
        visible = false;
        //print("OnBecameInvisible");
    }
}