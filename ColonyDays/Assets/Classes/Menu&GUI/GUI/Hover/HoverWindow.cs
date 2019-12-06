using UnityEngine;
using UnityEngine.UI;

/*
 * The hover window is wht is shown when hovering 
 */

public class HoverWindow : MonoBehaviour
{
    private string _msg;

    private GameObject _geometry;
    private Text _text;

    private RectTransform _rectTransform;

    //the Key that is showing this HoverWindow. 
    //needed to pull the string from Languages.cs
    private string _key;

    //the medium hover window
    private HoverWindowMed _hoverWindowMed;

    int hideVal = -90000;

    //when was showed
    private float showedAt;

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
            return;

        _msg = "";
        _text.text = "";
        _rectTransform.position = new Vector3(-90000, -90000, 0);
    }

    public void Show(Vector3 pos, string key)
    {
        AudioCollector.PlayOneShot("ClickWoodSubtle", 0);
        _key = key;
        _msg = Languages.ReturnString(key + ".HoverSmall");

        _rectTransform.position = Hoverable.MousePositionTowardsScreenCenter();

        _text.text = _msg;
        showedAt = Time.time;
    }  
    
    /// <summary>
    /// This one is a simple message
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="msg"></param>
    public void ShowMsg(Vector3 pos, string msg)
    {
        AudioCollector.PlayOneShot("ClickWoodSubtle", 0);

        _key = "";
        _msg = Languages.ReturnString(msg);

        _rectTransform.position = Hoverable.MousePositionTowardsScreenCenter();

        _text.text = _msg;
        showedAt = Time.time;
    }

	// Update is called once per frame
	void Update ()
	{
        //means is hiding.
        if (_rectTransform.position.x < -80000)
            return;

        _rectTransform.position = Hoverable.MousePositionTowardsScreenCenter();

        //after 3 seconds of being show
        //if key = "" is a simple msg with out key
        if (Time.time > showedAt + .7 && !string.IsNullOrEmpty(_key))
	        SpawnMedHover();
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

    public void ShowExplicitThis(string key)
    {
        AudioCollector.PlayOneShot("ClickWoodSubtle", 0);

        _rectTransform.position = Hoverable.MousePositionTowardsScreenCenter();

        _text.text = key;
        showedAt = Time.time;
    }
}
