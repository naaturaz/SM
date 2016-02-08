using UnityEngine;
using UnityEngine.UI;


public class ShowInvetoryItem : GUIElement
{
    private GameObject _text;
    private GameObject _icon;

    private InvItem _invItem;

    private Text _textObj;
    private Image _iconImg;
    

    public InvItem InvItem1
    {
        get { return _invItem; }
        set { _invItem = value; }
    }

    // Use this for initialization
	void Start ()
	{
        _text = FindGameObjectInHierarchy("Text", gameObject);
	    _icon = FindGameObjectInHierarchy("Icon", gameObject);

        _textObj = _text.GetComponent<Text>();
        _iconImg = _icon.GetComponent<Image>();

        //so hover gets it
	    _text.transform.name = InvItem1.Key+"";

	    LoadIcon();
	}

    private void LoadIcon()
    {
        Sprite sp = (Sprite)Resources.Load(Root.iconBrick);

        _iconImg.sprite = sp;
    }

    static public ShowInvetoryItem Create(string root, Transform container, InvItem invItem, Vector3 iniPos)
    {
        ShowInvetoryItem obj = null;
        obj = (ShowInvetoryItem)Resources.Load(root, typeof(ShowInvetoryItem));
        obj = (ShowInvetoryItem)Instantiate(obj, new Vector3(), Quaternion.identity);

        obj.transform.parent = container;
        obj.InvItem1 = invItem;
        obj.transform.position = iniPos;

        return obj;
    }


    private int count;
	// Update is called once per frame
	void Update ()
	{
	    _textObj.text = InvItem1.Amount + "";
	}

    string Formatter(float amt)
    {
        
        return "";
    }
}
