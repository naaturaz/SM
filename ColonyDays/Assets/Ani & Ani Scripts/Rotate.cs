using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 1f;
    public bool onX;
    public bool onY;
    public bool onZ;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    

	    if (onX)
	    {
	        gameObject.transform.Rotate(speed,0  , 0);
	    }
        if (onY)
        {
            gameObject.transform.Rotate(0 ,speed , 0);
        }
        if (onZ)
        {
            gameObject.transform.Rotate(0, 0, speed);
        }
    }
}
