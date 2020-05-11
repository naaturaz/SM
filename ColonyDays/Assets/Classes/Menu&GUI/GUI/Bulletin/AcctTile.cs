using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcctTile : GUIElement
{
    private DisplayAccount _acct;

    private Text _leftText;
    private Text _rightText;
    private Text _centerText;

    private List<Text> _allText;

    private GameObject _nextBtn;
    private GameObject _prevBtn;

    private Sprite _back;

    private SubBulletinFinance _finance;

    public SubBulletinFinance Finance
    {
        get { return _finance; }
        set { _finance = value; }
    }

    public DisplayAccount Acct
    {
        get { return _acct; }
        set { _acct = value; }
    }

    private void Start()
    {
        _leftText = FindGameObjectInHierarchy("Left_Lbl", gameObject).GetComponent<Text>();
        _rightText = FindGameObjectInHierarchy("Right_Lbl", gameObject).GetComponent<Text>();
        _centerText = FindGameObjectInHierarchy("Center_Lbl", gameObject).GetComponent<Text>();

        _prevBtn = FindGameObjectInHierarchy("Prev_Btn", gameObject);
        _nextBtn = FindGameObjectInHierarchy("Next_Btn", gameObject);

        _back = FindGameObjectInHierarchy("PriceBackImg", gameObject).GetComponent<Sprite>();

        _allText = new List<Text>();

        _allText.Add(_leftText);
        _allText.Add(_rightText);
        _allText.Add(_centerText);

        Init();
    }

    private void Init()
    {
        _leftText.text = "";
        _rightText.text = "";
        _centerText.text = "";

        if (Acct.Balance == -1)
        {
            _centerText.text = Acct.Name;
        }
        else
        {
            _leftText.text = Acct.Name;
            _rightText.text = MyText.DollarFormat(Acct.Balance);
        }

        Year();
        Fonts();
    }

    private void Year()
    {
        //uncoomment to deal with the Next, Prev year featuree

        if (Acct.Name != "Year")
        {
            _prevBtn.SetActive(false);
            _nextBtn.SetActive(false);
        }

        if (Acct.Name == "Year")
        {
            if (!BulletinWindow.SubBulletinFinance1.FinanceLogger.ThereIsANextYear((int)Acct.Balance))
            {
                _nextBtn.SetActive(false);
            }
            if (!BulletinWindow.SubBulletinFinance1.FinanceLogger.ThereIsAPrevYear((int)Acct.Balance))
            {
                _prevBtn.SetActive(false);
            }

            _leftText.text = "";
            _rightText.text = "";
            _centerText.text = "Year: " + Acct.Balance;
        }
    }

    private void Fonts()
    {
        if (Acct.BoldFont)
        {
            for (int i = 0; i < _allText.Count; i++)
            {
                //_allText[i].font.fontSize += Acct.AddSizeFont;
            }
        }

        for (int i = 0; i < _allText.Count; i++)
        {
            //_allText[i].font.fontSize += Acct.AddSizeFont;
        }
    }

    private void Update()
    {
    }

    internal static AcctTile CreateTile(Transform container,
        DisplayAccount acct, Vector3 iniPos, SubBulletinFinance finance)
    {
        AcctTile obj = null;

        var root = "";

        obj = (AcctTile)Resources.Load(Root.acct_Tile, typeof(AcctTile));
        obj = (AcctTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var iniScale = obj.transform.localScale;
        obj.transform.SetParent(container);
        obj.transform.localPosition = iniPos;
        obj.transform.localScale = iniScale;

        obj.Acct = acct;
        obj.Finance = finance;
        return obj;
    }

    public void NextYear()
    {
        _finance.BulletinWindow1.ShowNextYearBudget();
    }

    public void PrevYear()
    {
        _finance.BulletinWindow1.ShowPrevYearBudget();
    }
}