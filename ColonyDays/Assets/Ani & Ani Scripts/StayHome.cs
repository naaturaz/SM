using UnityEngine;

public class StayHome : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Click()
    {
        Application.OpenURL("https://twitter.com/search?q=%23StayHome&src=typeahead_click");
    }
}
