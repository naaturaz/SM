using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;


public class ShowInvetoryItem : GUIElement
{
    private GameObject _icon;
    private GameObject _back;
    private GameObject _back2;
    private GameObject _back3;

    private InvItem _invItem;

    private Text _textCol1;
    private Text _textCol2;
    private Text _textCol3;

    private Image _iconImg;

    private string _invType;

    private ShowAInventory _parent;

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

    public ShowAInventory Parent
    {
        get { return _parent; }
        set { _parent = value; }
    }

    // Use this for initialization
	void Start ()
	{
	    _icon = FindGameObjectInHierarchy("Icon", gameObject);
	    _back = FindGameObjectInHierarchy("Back", gameObject);
        _back2 = FindGameObjectInHierarchy("Back2", gameObject);
        _back3 = FindGameObjectInHierarchy("Back3", gameObject);

        _textCol1 = FindGameObjectInHierarchy("Text", gameObject).GetComponent<Text>();


        var col2 = FindGameObjectInHierarchy("Weight of product", gameObject);
	    if (col2!=null)
	    {
            _textCol2 = col2.GetComponent<Text>();
	    }

        var col3 = FindGameObjectInHierarchy("Volume occupied", gameObject);
	    if (col3!=null)
	    {
            _textCol3 = col3.GetComponent<Text>();
	    }




        _iconImg = _icon.GetComponent<Image>();

        if (_textCol1 != null && InvItem1!=null)
	    {
            //so hover gets it
            _textCol1.transform.name = InvItem1.Key + "";
            LoadIcon();

	    }

        //Debug.Log("loc:" + transform.localScale + " lossy"+transform.localScale);
	}

    private void LoadIcon()
    {
        var root = Program.gameScene.ExportImport1.ReturnIconRoot(_invItem.Key);
        Sprite sp = Resources.Load<Sprite>(root);

        //debug only bz all should have a root
        if (sp == new Sprite())
        {
            root = "Prefab/GUI/Inventory_Icons/Brick";
            sp = Resources.Load<Sprite>(root);
        }

        _iconImg.sprite = sp;
    }

    static public ShowInvetoryItem Create(Transform container, InvItem invItem, Vector3 iniPos, ShowAInventory parent,
        string invType="")
    {
        ShowInvetoryItem obj = null;
        var root = "";
        if (string.IsNullOrEmpty(invType))
        {
            root = Root.show_Invent_Item_Small_3_Text;
        }
        else
        {
            //for main
            root = Root.show_Invent_Item_Small_Med;
        }
        obj = (ShowInvetoryItem)Resources.Load(root, typeof(ShowInvetoryItem));
        obj = (ShowInvetoryItem)Instantiate(obj, new Vector3(), Quaternion.identity);

        obj.transform.SetParent( container);
        obj.transform.localPosition = iniPos;

        obj.Parent = parent;
        obj.InvItem1 = invItem;
        obj.InvType = invType;

        return obj;
    }




    private float oldAmt=-100;//the value so it gets started
	// Update is called once per frame
	void Update ()
	{
        if (InvItem1 == null || (InvItem1.Amount <= 0 && string.IsNullOrEmpty(InvType)))
        {
            Parent.Destroy(this);
	        return;
	    }

        if (oldAmt != InvItem1.Amount)
	    {
            //for the one tht oly has 1 text
	        if (_textCol2 == null)
	        {
                oldAmt = InvItem1.Amount;
                _textCol1.text = Formatter();
	        }
            //3 text used to builidings inventoryes 
	        else
	        {
	            Set3Text();
	        }


	    }
	}

    void Set3Text()
    {
        if (InvItem1.Key == P.Year)
        {
            Set3TextForReport();
            return;
        }

        _textCol1.text = InvItem1.Key+"";
        _textCol2.text = ReturnAmt();
        _textCol3.text = ReturnVol();
    }

    private void Set3TextForReport()
    {
        //-1 bz +1 is added when Year is added on invv
        _textCol1.text = InvItem1.Key+": "+ (InvItem1.Amount -1);
        _textCol2.text = "";
        _textCol3.text = "";

        _back.SetActive(false);
        _back2.SetActive(false);
        _back3.SetActive(false);
    }

    string ReturnAmt()
    {
        if (InvItem1.Amount <= 0)
        {
            return "-";
        }
        var amt = Unit.WeightConverted(InvItem1.Amount);
        return (int) amt + " " + Unit.WeightUnit();
    }

    string ReturnVol()
    {
        if (InvItem1.Amount <= 0)
        {
            return "-";
        }
        var vol = Unit.VolConverted(InvItem1.Volume);
        return vol.ToString("F0") + " " + Unit.VolumeUnit();
    }


    string Formatter()
    {
        if (InvItem1.Amount <= 0)
        {
            return "-";
        }

        var amt = Unit.WeightConverted(InvItem1.Amount);
        var vol = Unit.VolConverted(InvItem1.Volume);

        //Main GUI
        if (InvType=="Main")
        {
            //return StandardFormat();
            return ShortFormat(amt);
        }

        return PadThis(InvItem1.Key+"", 14, 'r') + " " + PadThis((int)amt+"", 8, 'l' )+  
            PadThis(vol.ToString("F1"), 8, 'l');
        //return InvItem1.Key + " " + (int)amt + BuildStringUnits() + vol.ToString("F1");
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pass">pass The string pass</param>
    /// <param name="max">amount of spaces</param>
    /// <param name="riOrLeft">padding on the right or left</param>
    /// <returns> Spaces depending on the pass.lenght and max </returns>
    private static string PadThis(string pass, int max, char riOrLeft, string pad = " ")
    {
        int spaces = max - pass.Length;

        for (int i = 0; i < spaces; i++)
        {
            if (riOrLeft == 'r')
            {
                pass += pad;
            }
            else if (riOrLeft == 'l')
            {
                pass = pad + pass;
            }
        }
        return pass;
    }


    

    string BuildStringUnits()
    {
        return " " + Unit.WeightUnit() + ". v(" + Unit.VolumeUnit() + "):";
    }

    private string StandardFormat()
    {
        if (InvItem1.Amount < 10)
        {
            return (InvItem1.Amount.ToString("n1"));
        }
        return  ((int)InvItem1.Amount)+"";
    }

    private string ShortFormat(float amt)
    {
        if (amt < 10)
        {
            return (amt.ToString("n1"));
        }

        if (amt > 1000000)
        {
            return (int)(amt / 1000000) + "M";
        }
        if (InvItem1.Amount > 1000)
        {
            return (int)(amt / 1000) + "K";
        }

        return (int)amt + "";
    }
}
