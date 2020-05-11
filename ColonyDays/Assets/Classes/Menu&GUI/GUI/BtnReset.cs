using UnityEngine;

public class BtnReset : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Reset()
    {
        PlayerPrefs.SetString("Tuto", "");
        PlayerPrefs.SetInt("Rate", 0);
        PlayerPrefs.SetInt("Reward", 0);
        PlayerPrefs.SetInt("Week", -1);

        PlayerPrefs.SetString("F1", "");
        PlayerPrefs.SetString("QuestArrow", "");
        PlayerPrefs.SetString("TutoArrow", "");
    }
}