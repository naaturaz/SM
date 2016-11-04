using System.Collections.Generic;
using Assets.Classes.Menu_GUI.GUI.Bulletin;
using UnityEngine;
using UnityEngine.UI;


public class BulletinWindow : GUIElement
{
    //bulletin fields
    private Text _body;

    private GameObject _content;
    private RectTransform _contentRectTransform;
    private GameObject _scroll_Ini_PosGO;
    private GameObject _scroll;
    private Scrollbar _verticScrollbar;


    //subBulletins
    private SubBulletinWorkers _workers;
    private static SubBulletinProduction _production;

    public Text Body1
    {
        get { return _body; }
        set { _body = value; }
    }

    public GameObject Content
    {
        get { return _content; }
        set { _content = value; }
    }

    public GameObject ScrollIniPosGo
    {
        get { return _scroll_Ini_PosGO; }
        set { _scroll_Ini_PosGO = value; }
    }


    void Start()
    {
        _body = GetChildCalled("Body_Lbl").GetComponent<Text>();
        iniPos = transform.position;

        Hide();

        _workers = new SubBulletinWorkers(this);
        _production = new SubBulletinProduction(this);
        //

        _scroll = GetChildCalled("Scroll_View");
        _content = GetGrandChildCalledFromThis("Content", _scroll);
        _contentRectTransform = _content.GetComponent<RectTransform>();
        _scroll_Ini_PosGO = GetChildCalledOnThis("Scroll_Ini_Pos", _content);
    }

    void Update()
    {

    }




    public void Show()
    {
        base.Show();
        ShowWorkers();
    }

    void HideAll()
    {
        _scroll.SetActive(false);
        _body.text = "";
        _production.Hide();
    }

    public void ShowScrool()
    {
        _scroll.SetActive(true);
    }

    public void ShowInBody(string text)
    {
        _body.text = text;
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowWorkers()
    {
        
        HideAll();
        _workers.Show();
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowProd()
    {
        HideAll();
        _production.ShowProdReport();
    } 
    
    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowConsume()
    {
        HideAll();
        _production.ShowConsumeReport();
    }


    public static void AddProduction(P p, float amt, string type)
    {
        if (type == "Prod")
        {
            _production.AddProductionThisYear(p, amt);
            
        }
        else if (type == "Consume")
        {
            _production.AddConsumeThisYear(p, amt);
        }
    }
}

