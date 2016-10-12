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

    private RectTransform _rectTransform;

    //the Key that is showing this HoverWindow. 
    //needed to pull the string from Languages.cs
    private string _key;

    //the medium hover window
    private HoverWindowMed _hoverWindowMed;

    public string Key
    {
        get { return _key; }
        set { _key = value; }
    }

    // Use this for initialization
	void Start ()
	{
	    _geometry = General.FindGameObjectInHierarchy("Geometry", gameObject);
	    var textGO = General.FindGameObjectInHierarchy("Text", gameObject);

	    if (textGO!= null)
	    {
	        _text = textGO.GetComponent<Text>();
	    }

	    _hoverWindowMed = FindObjectOfType<HoverWindowMed>();
	    _rectTransform = transform.GetComponent<RectTransform>();
	    Hide();
	}

    public void Hide()
    {
        if (_rectTransform == null)
        {
            return;
        }

        _rectTransform.position = new Vector3(500,500);
        _msg = "";
        _text.text = "";
        _geometry.SetActive(false);
    }

    public void Show(Vector3 pos, string key)
    {
        _key = key;
        _msg = Languages.ReturnString(key + ".HoverSmall");
        _rectTransform.position = pos;

        _text.text = _msg;
        _geometry.SetActive(true);

        diffToMouse = pos - Input.mousePosition;
        showedAt = Time.time;
    }  
    
    /// <summary>
    /// This one is a simple message
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="msg"></param>
    public void ShowMsg(Vector3 pos, string msg)
    {
        _key = "";
        _msg = Languages.ReturnString(msg);
        _rectTransform.position = pos;

        _text.text = _msg;
        _geometry.SetActive(true);

        diffToMouse = pos - Input.mousePosition;
        showedAt = Time.time;
    }

    //so mouse never touches this one. so it doesnt trigger a OnMouseExit on the spawer
    //of this window
    private Vector3 diffToMouse;

    //when was showed
    private float showedAt;

	// Update is called once per frame
	void Update ()
	{
        //means is hiding.
        //and wont let any other click happen if is on front of everything 
        if (string.IsNullOrEmpty(_msg))
	    {
	        return;
	    }

	    _rectTransform.position = Input.mousePosition + diffToMouse;

        //after 3 seconds of being show
        //if key = "" is a simple msg with out key
	    if (Time.time > showedAt + .2 && !string.IsNullOrEmpty(_key))
	    {
	        SpawnMedHover();
	    }
	}

    //Hover Med

    /// <summary>
    /// Will hide this one and will spawn a Medium size hover window
    /// with more explanation
    /// </summary>
    private void SpawnMedHover()
    {
        if (!Languages.DoIHaveHoverMed(_key))
        {
            return;
        }

        Hide();
        //*3 to make it abit further from mouse pointer
        _hoverWindowMed.ShowSemiTut(_key);
    }





    internal string Message()
    {
        return _msg;
    }
}
