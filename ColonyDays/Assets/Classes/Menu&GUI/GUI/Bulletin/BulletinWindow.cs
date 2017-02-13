﻿using System.Collections.Generic;
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

    //subBulletins
    private SubBulletinGeneral _workers;
    private static SubBulletinProduction _production;
    private SubBulletinFinance _finance;

    Text _help;//help in this window

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

    public static SubBulletinProduction SubBulletinProduction1
    {
        get { return _production; }
        set { _production = value; }
    }


    void Start()
    {
        _body = GetChildCalled("Body_Lbl").GetComponent<Text>();
        iniPos = transform.position;

        Hide();

        _workers = new SubBulletinGeneral(this);
        _production = new SubBulletinProduction(this);
        _finance = new SubBulletinFinance(this);
        
        //
        _scroll = GetChildCalled("Scroll_View");
        _content = GetGrandChildCalledFromThis("Content", _scroll);
        _contentRectTransform = _content.GetComponent<RectTransform>();
        _scroll_Ini_PosGO = GetChildCalledOnThis("Scroll_Ini_Pos", _content);

        var h = GetChildCalled("Help");
        _help = h.GetComponent<Text>();
        _help.text = "";

    }

    void Update()
    {

    }

    /// <summary>
    /// Called from GUI 
    /// </summary>
    public void Show()
    {
        base.Show();
        ShowWorkers();

        Program.gameScene.TutoStepCompleted("ShowWorkersControl.Tuto");
    }

    void HideAll()
    {
        _help.text = "";
        _scroll.SetActive(false);
        _body.text = "";
        _production.Hide();
        _finance.Hide();
        _workers.Hide();
    }

    public void ShowScrool()
    {
        _scroll.SetActive(true);
        //all the way up 

        base.ResetScroolPos();


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
        ClickAndHideAll();

        _workers.ShowWorkers();

        _help.text = "Bulletin/General/Workers \n" + Languages.ReturnString("Help.Bulletin/General/Workers");

    }  
    
    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowBuildings()
    {
        ClickAndHideAll();

        _workers.ShowBuildings();

        //""
        _help.text = "Bulletin/General/Buildings \n" + Languages.ReturnString("Help.Bulletin/General/Buildings");

    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowProd()
    {
        ClickAndHideAll();

        _production.ShowProdReport();
        _help.text = "Bulletin/Prod/Produce \n" + Languages.ReturnString("Help.Bulletin/Prod/Produce");

    } 
    
    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowConsume()
    {
        ClickAndHideAll();

        _production.ShowConsumeReport();
        _help.text = "Bulletin/Prod/Consume \n" + Languages.ReturnString("Help.Bulletin/Prod/Consume");

    } 
    
    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowExpiration()
    {
        ClickAndHideAll();
        _production.ShowExpirationReport();
        _help.text = "Bulletin/Prod/Expire \n" + Languages.ReturnString("Help.Bulletin/Prod/Expire");

    }

    /// <summary>
    /// Called from GUI
    /// 
    /// Also from GUI on Show_Invent_Item_Small_Med_3_Text
    /// </summary>
    public void ShowSpecs()
    {
        ClickAndHideAll();
        _finance.ShowSpecs();
        Program.gameScene.TutoStepCompleted("Spec.Tuto");
        _help.text = "Bulletin/Prod/Spec \n" + Languages.ReturnString("Help.Bulletin/Prod/Spec");
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
        else if (type == "Expire")
        {
            _production.AddToExpirationThisYear(p, amt);
        }
    }

    void ClickAndHideAll()
    {
        AudioCollector.PlayOneShotFullAudio("ClickMetal2");
        HideAll();
    }

#region Finance

    public void ShowFinanceResume()
    {
        ClickAndHideAll();
        _finance.ShowResume();
    }

    public void ShowFinancePrices()
    {
        ClickAndHideAll();
        _finance.ShowPrices();
    }

#endregion
}

