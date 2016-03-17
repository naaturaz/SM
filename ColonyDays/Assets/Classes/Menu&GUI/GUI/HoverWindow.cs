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

    private GameObject _geometry;
    private Text _text;

	// Use this for initialization
	void Start ()
	{
	    _geometry = General.FindGameObjectInHierarchy("Geometry", gameObject);
	    var textGO = General.FindGameObjectInHierarchy("Text", gameObject);

	    if (textGO!= null)
	    {
	        _text = textGO.GetComponent<Text>();
	    }

	    Hide();
	}

    public void Hide()
    {
        _msg = "";
        _text.text = "";
        _geometry.SetActive(false);
    }

    public void Show(Vector3 pos, string msg)
    {
        _msg = msg;
        transform.GetComponent<RectTransform>().position = pos;
        
        _text.text = msg;
        _geometry.SetActive(true);
    }

    public string CurrentMsg()
    {
        return _msg;
    }


	
	// Update is called once per frame
	void Update () {
	
	}
}
