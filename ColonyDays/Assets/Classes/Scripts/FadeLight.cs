using UnityEngine;
using System.Collections;



public class FadeLight : MonoBehaviour {

    Light light;
    public bool isToFade;
    public F fadeDirection;
    public float speedFade = 40f;

	// Use this for initialization
	void Start ()
    {
        light = gameObject.GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update ()
    {


	    if(isToFade)
        {
            light.intensity = UFade.FadeAction(fadeDirection.ToString(), light.intensity, speedFade);
            print(light.intensity);
        }
	}
}
