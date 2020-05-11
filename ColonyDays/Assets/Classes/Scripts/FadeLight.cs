using UnityEngine;

public class FadeLight : MonoBehaviour
{
    private Light light;
    public bool isToFade;
    public F fadeDirection;
    public float speedFade = 40f;

    // Use this for initialization
    private void Start()
    {
        light = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isToFade)
        {
            light.intensity = UFade.FadeAction(fadeDirection.ToString(), light.intensity, speedFade);
            print(light.intensity);
        }
    }
}