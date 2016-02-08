using UnityEngine;
using UnityEngine.UI;


public class ShowInvetoryItem : GUIElement
{
    private GameObject _text;
    private GameObject _icon;

    private InvItem _invItem;

    private Text _textObj;
    private Image _iconImg;

    private string _invType;

    public InvItem InvItem1
    {
        get { return _invItem; }
        set { _invItem = value; }
    }

    public string InvType
    {
        get { return _invType; }
        set { _invType = value; }
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
        Sprite sp = Resources.Load<Sprite>(Root.iconBrick);
        _iconImg.sprite = sp;
    }

    static public ShowInvetoryItem Create(Transform container, InvItem invItem, Vector3 iniPos, string invType="")
    {
        ShowInvetoryItem obj = null;

        var root = "";
        if (string.IsNullOrEmpty(invType))
        {
            root=Root.show_Invent_Item_Med;
        }
        else
        {
            root = Root.show_Invent_Item;
        }

        obj = (ShowInvetoryItem)Resources.Load(root, typeof(ShowInvetoryItem));
        obj = (ShowInvetoryItem)Instantiate(obj, new Vector3(), Quaternion.identity);


        obj.transform.parent = container;
        obj.transform.localPosition = iniPos;

        obj.InvItem1 = invItem;

        return obj;
    }


    private float oldAmt;
	// Update is called once per frame
	void Update ()
	{
        if (oldAmt != InvItem1.Amount)
	    {
            oldAmt = InvItem1.Amount;

            _textObj.text = Formatter();
	    }

	}

    string Formatter()
    {
        //buildign invneotyr 
        if (string.IsNullOrEmpty(InvType))
        {
            return InvItem1.Amount.ToString("F1") + "kg. v(m3):" + InvItem1.Volume.ToString("F1");
        }

        return "";
    }
}
