using UnityEngine;
using UnityEngine.UI;

/*
 * The hover window is wht is shown when hovering 
 */

public class HoverWindow : MonoBehaviour
{
    private string _msg;

    //windows props
    private Vector3 _min;
    private Vector3 _max;

	// Use this for initialization
	void Start ()
	{
	    Hide();
	}

    public void Hide()
    {
        _msg = "";
        transform.GetComponent<Text>().text = "";
    }

    public void Show(Vector3 pos, string msg)
    {
        _msg = msg;
        transform.GetComponent<RectTransform>().position = pos;
        
        transform.GetComponent<Text>().text = msg;
    }

    public string CurrentMsg()
    {
        return _msg;
    }


	
	// Update is called once per frame
	void Update () {
	
	}
}
