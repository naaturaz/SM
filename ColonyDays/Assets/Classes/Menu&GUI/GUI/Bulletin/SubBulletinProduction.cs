using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class SubBulletinProduction
{
    private ProductionReport _productionReport;
    private ProductionReport _expirationReport;
    private BulletinWindow _bulletinWindow;
    List<ShowAInventory> _reports = new List<ShowAInventory>();

    [XmlIgnore]
    public BulletinWindow BulletinWindow1
    {
        get { return _bulletinWindow; }
        set { _bulletinWindow = value; }
    }

    public SubBulletinProduction() { }

    public SubBulletinProduction(BulletinWindow bulletinWindow)
    {
        _bulletinWindow = bulletinWindow;
    }

    public ProductionReport ProductionReport1
    {
        get { return _productionReport; }
        set { _productionReport = value; }
    }

    public ProductionReport ExpirationReport
    {
        get { return _expirationReport; }
        set { _expirationReport = value; }
    }

    internal void AddProductionThisYear(P p, float amt)
    {
        if (_productionReport == null)
        {
            _productionReport = new ProductionReport();
        }
        _productionReport.AddProductionThisYear(p, amt);
    }

    internal void AddConsumeThisYear(P p, float amt)
    {
        if (_productionReport == null)
        {
            _productionReport = new ProductionReport();
        }
        _productionReport.AddConsumeThisYear(p, amt);
    }  
    
    internal void AddToExpirationThisYear(P p, float amt)
    {
        if (_expirationReport == null)
        {
            _expirationReport = new ProductionReport();
        }
        _expirationReport.AddProductionThisYear(p, amt);
    }

    /// <summary>
    /// Created to find how many items are in each report 
    /// needed to ajust the Scrool View Content height 
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    int ItemsOnReport(List<Inventory> list)
    {
        var res = 0;
        for (int i = 0; i < list.Count; i++)
        {
            res += list[i].InventItems.Count;
        }
        return res;
    }

    public void ShowProdReport()
    {
        if (_productionReport == null)
        {
            _bulletinWindow.ShowInBody("");
            return;
        }
        ShowProductionReport(_productionReport.ProduceReport);
    }

    public void ShowConsumeReport()
    {
        if (_productionReport == null)
        {
            _bulletinWindow.ShowInBody("");
            return;
        }
        ShowProductionReport(_productionReport.ConsumeReport);
    }

    public void ShowExpirationReport()
    {
        if (_expirationReport == null)
        {
            _bulletinWindow.ShowInBody("");
            return;
        }
        ShowProductionReport(_expirationReport.ProduceReport);
    }

    private void ShowProductionReport(List<Inventory> list)
    {
        Hide();
        var height = ItemsOnReport(list) * 3.75f + (list.Count - 1) * 4f;
        _bulletinWindow.AdjustContentHeight(height);
        _bulletinWindow.ShowScrool();

        var lastPos = _bulletinWindow.ScrollIniPosGo.transform.localPosition;
        var pastItems = 0;
        for (int i = 0; i < list.Count; i++)
        {
            var margin = i > 0 ? -4f : 0f;
            var yPos = (pastItems * -3.75f) + margin;

            var a = new ShowAInventory(list[i], _bulletinWindow.Content.gameObject, lastPos + new Vector3(0, yPos, 0), "Bulletin.Prod.Report");

            lastPos = lastPos + new Vector3(0, yPos, 0);

            _reports.Add(a);
            pastItems = list[i].InventItems.Count;
        }
    }

    int ShowLastYears(List<Inventory> list)
    {
        if (list == null)
            return 0;

        if (list.Count < 6)
            return list.Count;

        return 5;
    }

    internal void Hide()
    {
        for (int i = 0; i < _reports.Count; i++)
        {
            _reports[i].DestroyAll();
        }
        _reports.Clear();
    }
}
