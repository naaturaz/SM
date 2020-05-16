using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowInvetoryItem : GUIElement
{
    public TextMeshProUGUI _textCol1;
    public TextMeshProUGUI _textCol2;
    public TextMeshProUGUI _textCol3;
    public Transform InitialPositionPoint;

    protected GameObject _icon;

    private InvItem _invItem;

    protected Image _iconImg;
    private string _invType;
    private ShowAInventory _parent;

    private ProdSpec _pSpec;

    private float oldAmt = -100;//the value so it gets started
                                // Update is called once per frame

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

    private void Awake()
    {
        if (info == "For Dock") return;

        Start();
    }

    // Use this for initialization
    private void Start()
    {
        if (_icon != null)
            return;

        var container = FindGameObjectInHierarchy("Cont", gameObject);
        _icon = FindGameObjectInHierarchy("Icon", gameObject);

        _iconImg = _icon.GetComponent<Image>();
    }

    protected void LoadIcon(P key = P.None)
    {
        if (key == P.None) key = _invItem.Key;

        var root = Program.gameScene.ExportImport1.ReturnIconRoot(key);
        Sprite sp = Resources.Load<Sprite>(root);

        //debug only bz all should have a root
        if (sp == null)//new Sprite()
        {
            root = "Prefab/GUI/Inventory_Icons/EmptyPNG";
            sp = Resources.Load<Sprite>(root);
        }

        _iconImg.sprite = sp;
    }

    static public ShowInvetoryItem Create(Transform container, InvItem invItem, Vector3 iniPos, ShowAInventory parent,
        string invType = "")
    {
        ShowInvetoryItem obj = null;
        var root = "";
        if (string.IsNullOrEmpty(invType))
        {
            root = Root.show_Invent_Item_Small_3_Text + " 2";
        }
        else if (invType == "Bulletin.Prod.Report")
        {
            root = Root.show_Invent_Item_Small_3_Text + " 4";
        }
        else
        {
            //for 
            root = Root.show_Invent_Item_Small_Med + " 2";
        }

        obj = (ShowInvetoryItem)Resources.Load(root, typeof(ShowInvetoryItem));
        obj = Instantiate(obj, container.transform);

        if (obj.InitialPositionPoint != null && obj.InitialPositionPoint.position != new Vector3())
        {
            obj.transform.SetParent(container);
            obj.transform.localPosition = obj.InitialPositionPoint.localPosition;
        }

        obj.Parent = parent;
        obj.InvItem1 = invItem;
        obj.InvType = invType;

        return obj;
    }

    private void Update()
    {
        if (InvItem1 == null || (InvItem1.Amount <= 0 && string.IsNullOrEmpty(InvType)))
        {
            //Parent.Destroy(this);
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

    private void Set3Text()
    {
        if (InvItem1.Key == P.Year)
        {
            LoadIcon();
            Set3TextForReport();
            return;
        }

        _textCol1.text = Languages.ReturnString(InvItem1.Key + "");
        _textCol2.text = ReturnAmt();
        if (_textCol3 != null)
            _textCol3.text = ReturnVol();

        LoadIcon();
    }

    private void Set3TextForReport()
    {
        //-1 bz +1 is added when Year is added on invv
        _textCol1.text = Languages.ReturnString(InvItem1.Key.ToString()) + ": " + (InvItem1.Amount - 1);
        _textCol2.text = "";
        _textCol3.text = "";
    }

    private string ReturnAmt()
    {
        if (_pSpec == null)
            _pSpec = Program.gameScene.ExportImport1.FindProdSpec(InvItem1.Key);

        if (InvItem1.Amount <= 0)
            return "-";

        var amt = Unit.WeightConverted(InvItem1.Amount);

        if (InvType == "Main")
            return (int)amt + " ";

        return (int)amt + " " + Unit.WeightUnit();
    }

    private string ReturnAmtOnUnit()
    {
        return (InvItem1.Amount / _pSpec.WeightPerUnit).ToString("n0") + " u";
    }

    private string ReturnVol()
    {
        if (InvItem1.Amount <= 0)
        {
            return "-";
        }
        var vol = Unit.VolConverted(InvItem1.Volume);
        return vol.ToString("F0") + " " + Unit.VolumeUnit();
    }

    protected string Formatter()
    {
        if (InvItem1.Amount <= 0)
            return "-";

        var amt = Unit.WeightConverted(InvItem1.Amount);
        var vol = Unit.VolConverted(InvItem1.Volume);

        //Main GUI
        if (InvType == "Main")
        {
            return amt.ToString("N0");
        }

        return UString.PadThis(InvItem1.Key + "", 14, 'r') + " " + UString.PadThis((int)amt + "", 8, 'l')
            + UString.PadThis(vol.ToString("F1"), 8, 'l');
    }

    private string BuildStringUnits()
    {
        return " " + Unit.WeightUnit() + ". v(" + Unit.VolumeUnit() + "):";
    }

    private string StandardFormat()
    {
        if (InvItem1.Amount < 10)
            return (InvItem1.Amount.ToString("n1"));
        return ((int)InvItem1.Amount) + "";
    }

    private string ShortFormat(float amt)
    {
        if (amt < 10)
            return (amt.ToString("n1"));
        if (amt > 1000000)
            return (int)(amt / 1000000) + "M";
        if (InvItem1.Amount > 1000)
            return (int)(amt / 1000) + "K";

        return (int)amt + "";
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ClickOnIt()
    {
        Program.MouseListener.ClickOnAnInvItem(_invItem);
    }
}