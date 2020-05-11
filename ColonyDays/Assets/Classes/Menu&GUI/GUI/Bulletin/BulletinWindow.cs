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
    private static SubBulletinFinance _finance;

    private Text _help;//help in this window

    public static SubBulletinProduction SubBulletinProduction1
    {
        get { return _production; }
        set { _production = value; }
    }

    public static SubBulletinFinance SubBulletinFinance1
    {
        get { return _finance; }
        set { _finance = value; }
    }

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

    private void Start()
    {
        _body = GetChildCalled("Body_Lbl").GetComponent<Text>();
        iniPos = transform.position;

        Hide();

        _workers = new SubBulletinGeneral(this);
        _production = new SubBulletinProduction(this);
        _finance = new SubBulletinFinance(this);

        _scroll = GetChildCalled("Scroll_View");
        _content = GetGrandChildCalledFromThis("Content", _scroll);
        _contentRectTransform = _content.GetComponent<RectTransform>();
        _scroll_Ini_PosGO = GetChildCalledOnThis("Scroll_Ini_Pos", _content);

        var h = GetChildCalled("Help");
        _help = h.GetComponent<Text>();
        _help.text = "";

        //bz GUI Loades like 4 times
        PersonData pData = XMLSerie.ReadXMLPerson();

        var tempData = Program.gameScene.ProvideMeWithTempData();
        //means is reloading from a change in GUI a
        if (tempData != null)
            pData = tempData;

        //loading
        if (pData != null)
        {
            SubBulletinProduction1 = pData.PersonControllerSaveLoad.SubBulletinProduction;
            SubBulletinProduction1.BulletinWindow1 = this;

            if (pData.PersonControllerSaveLoad.SubBulletinFinance != null)
            {
                SubBulletinFinance1 = pData.PersonControllerSaveLoad.SubBulletinFinance;
                SubBulletinFinance1.BulletinWindow1 = this;
            }
        }

        //means is brand new game
        if (_finance.FinanceLogger.Budgets.Count == 0)
            _finance.FinanceLogger.AddYearBudget();
    }

    private void Update()
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
        Program.MouseListener.TutoWindow1.HideArrow();
        Program.MouseListener.HelpWindow.Hide();
    }

    private void HideAll()
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

        _help.text = Languages.ReturnString("Help.Bulletin/General/Workers");
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowBuildings()
    {
        ClickAndHideAll();
        _workers.ShowBuildings();

        _help.text = Languages.ReturnString("Help.Bulletin/General/Buildings");
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowProd()
    {
        ClickAndHideAll();

        _production.ShowProdReport();
        _help.text = Languages.ReturnString("Help.Bulletin/Prod/Produce");

        Program.gameScene.TutoStepCompleted("Prod.Tuto");
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowConsume()
    {
        ClickAndHideAll();

        _production.ShowConsumeReport();
        _help.text = Languages.ReturnString("Help.Bulletin/Prod/Consume");
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowExpiration()
    {
        ClickAndHideAll();
        _production.ShowExpirationReport();
        _help.text = Languages.ReturnString("Help.Bulletin/Prod/Expire");
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
        _help.text = Languages.ReturnString("Help.Bulletin/Prod/Spec");
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

    private void ClickAndHideAll()
    {
        AudioCollector.PlayOneShotFullAudio("ClickMetal2");
        HideAll();
    }

    #region Finance

    public void ShowFinanceBudget()
    {
        ClickAndHideAll();
        _finance.ShowBudget();

        _help.text = Languages.ReturnString("Help.Bulletin/Finance/Ledger");
        Program.gameScene.TutoStepCompleted("Budget.Tuto");
    }

    /// <summary>
    /// called from GUI  Dollars
    /// </summary>
    public void ShowFinanceBudgetGUI()
    {
        Program.MouseListener.HidePersonBuildOrderNotiBulletinHelpWin();
        base.Show();
        ShowFinanceBudget();
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowNextYearBudget()
    {
        HideAll();
        _finance.FinanceLogger.SetResumenToNextYear();
        ShowFinanceBudget();
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowPrevYearBudget()
    {
        HideAll();
        _finance.FinanceLogger.SetResumenToPrevYear();
        ShowFinanceBudget();
    }

    public void ShowFinancePrices()
    {
        ClickAndHideAll();
        _body.text = "Coming Soon";
        //_finance.ShowPrices();

        //Program.gameScene.TutoStepCompleted("Spec.Tuto");
        _help.text = Languages.ReturnString("Help.Bulletin/Finance/Prices");
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowExports()
    {
        ClickAndHideAll();
        _finance.ShowExports();
        Program.gameScene.TutoStepCompleted("Exports.Tuto");
        _help.text = Languages.ReturnString("Help.Bulletin/Finance/Exports");
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ShowImports()
    {
        ClickAndHideAll();
        _finance.ShowImports();
        _help.text = Languages.ReturnString("Help.Bulletin/Finance/Imports");
    }

    public void ShowWindowAndThenExports()
    {
        Program.MouseListener.HidePersonBuildOrderNotiBulletinHelpWin();
        base.Show();
        ShowExports();
    }

    /// <summary>
    /// called from GUI  Dollars
    /// </summary>
    public void ShowProducedReportGUI()
    {
        Program.MouseListener.HidePersonBuildOrderNotiBulletinHelpWin();
        base.Show();
        ShowProducedReport();
    }

    private void ShowProducedReport()
    {
        ClickAndHideAll();
        _production.ShowProdReport();

        _help.text = Languages.ReturnString("Help.Bulletin/Prod/Produce");
    }

    #endregion Finance
}