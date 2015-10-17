using UnityEngine;
using System.Collections;

//The Fade Utility
public class UFade : MonoBehaviour
{
    /// <summary>
    /// Will fade value btw 0-1 or 1-0 depedning the direction
    /// </summary>
    /// <param name="fadeDirectionPass">will indicate if is going towards 0 or 1 the fade value</param>
    /// <param name="targetMatchValue">this is the value we want to match</param>
    /// <param name="speedPass">the speed will be matched</param>
    /// <returns>from zero to one </returns>
    public static float FadeAction(string fadeDirectionPass, float targetMatchValue, float speedPass = 30f)
    {
        if (fadeDirectionPass == "FadeIn")
        {
            if (targetMatchValue < 1f)
            {
                targetMatchValue = targetMatchValue + 0.1f * Time.deltaTime * speedPass;
            }
            else if (targetMatchValue >= 1f)
            {
                //FadeDirection = "";
                //FadeState = "FadedIn";
            }
        }
        else if (fadeDirectionPass == "FadeOut")
        {
            if (targetMatchValue > 0)
            {
                targetMatchValue = targetMatchValue - 0.1f * Time.deltaTime * speedPass;
            }
            else if (targetMatchValue <= 0)
            {
                //FadeDirection = "";
                //FadeState = "FadedOut";
                //Destroyer();
            }
        }
        return targetMatchValue;
    }

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}
}
