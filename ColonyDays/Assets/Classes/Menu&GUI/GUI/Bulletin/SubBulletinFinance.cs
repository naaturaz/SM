using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class SubBulletinFinance
{
    private BulletinWindow _bulletinWindow;

    [XmlIgnore]
    public BulletinWindow BulletinWindow1
    {
        get { return _bulletinWindow; }
        set { _bulletinWindow = value; }
    }


    FinanceLogger _financeLogger;
    public FinanceLogger FinanceLogger
    {
        get { return _financeLogger; }
        set { _financeLogger = value; }
    }



    public SubBulletinFinance() { }

    public SubBulletinFinance(BulletinWindow bulletinWindow)
    {
        _bulletinWindow = bulletinWindow;

        if (_financeLogger == null)
        {
            _financeLogger = new FinanceLogger(true);
        }
    }


    #region Budget
    internal void ShowBudget()
    {
        ShowBudgetAccts(_financeLogger.ResumenCurrentBudget(Program.gameScene.GameTime1.Year));
        _bulletinWindow.AdjustContentHeight(_reportsBudget.Count * 4.7f);

    }



    List<AcctTile> _reportsBudget = new List<AcctTile>();
    private void ShowBudgetAccts(List<DisplayAccount> list)
    {
        Hide();
        _bulletinWindow.ShowScrool();

        for (int i = 0; i < list.Count; i++)
        {
            var iniPosHere = _bulletinWindow.ScrollIniPosGo.transform.localPosition +
                             new Vector3(0, -4.2f * i, 0);

            var a = AcctTile.CreateTile(_bulletinWindow.Content.gameObject.transform, list[i],
                iniPosHere, this);

            _reportsBudget.Add(a);
        }
    }

    #endregion







    #region Prices
    internal void ShowPrices()
    {
        ShowPrices(GetAllInInventories());
        _bulletinWindow.AdjustContentHeight(_reports.Count * 3.2f);

    }

    List<ProdSpec> GetAllInInventories()
    {
        List<ProdSpec> res = Program.gameScene.ExportImport1.TownProdSpecs;

        res = res.OrderBy(a => a.Product.ToString()).ToList();

        return res;
    }


    List<PriceTile> _reports = new List<PriceTile>();
    private void ShowPrices(List<ProdSpec> list)
    {
        Hide();
        _bulletinWindow.ShowScrool();

        for (int i = 0; i < list.Count; i++)
        {
            var iniPosHere = _bulletinWindow.ScrollIniPosGo.transform.localPosition +
                             new Vector3(0, -3.5f * i, 0);

            var a = PriceTile.CreateTile(_bulletinWindow.Content.gameObject.transform, list[i],
                iniPosHere);

            _reports.Add(a);
        }
    }
    #endregion



    internal void Hide()
    {
        for (int i = 0; i < _reports.Count; i++)
        {
            _reports[i].Destroy();
        }
        _reports.Clear();


        //specs
        for (int i = 0; i < _reportsSpec.Count; i++)
        {
            _reportsSpec[i].Destroy();
        }
        _reportsSpec.Clear();


        //budget
        for (int i = 0; i < _reportsBudget.Count; i++)
        {
            _reportsBudget[i].Destroy();
        }
        _reportsBudget.Clear();
        //_financeLogger.Clean();

        //exports
        for (int i = 0; i < _reportsExports.Count; i++)
        {
            //means this GUI was reloaded
            if (_reportsExports[i] == null || _reportsExports[i].gameObject == null)
            {
                break;
            }
            _reportsExports[i].Destroy();
        }
        _reportsExports.Clear();
    }



    #region Specs

    internal void ShowSpecs()
    {
        ShowSpecs(GetAllSpecs());
        _bulletinWindow.AdjustContentHeight(_reportsSpec.Count * 3.5f);
    }

    List<ProductInfo> GetAllSpecs()
    {
        var res = BuildingPot.Control.ProductionProp.InputProducts();
        res = res.OrderBy(a => a.Product.ToString()).ToList();

        res = AddTitleBar(res);

        res = RemoveRandomsProd(res);
        return res;
    }

    List<ProductInfo> AddTitleBar(List<ProductInfo> res)
    {
        //Title bar
        var list = new List<InputElement>()
            {
                new InputElement(P.Input1, 0),
                new InputElement(P.Input2, 0),
                new InputElement(P.Input3, 0),

            };
        ProductInfo v = new ProductInfo(P.Product, list, H.Building);
        res.Insert(0, v);


        return res;
    }

    List<ProductInfo> RemoveRandomsProd(List<ProductInfo> res)
    {
        var index = res.Find(a => a.Product.ToString().Contains("Random"));
        res.Remove(index);

        index = res.Find(a => a.Product.ToString().Contains("Random"));
        res.Remove(index);

        return res;
    }


    List<SpecTile> _reportsSpec = new List<SpecTile>();
    private void ShowSpecs(List<ProductInfo> list)
    {
        Hide();
        _bulletinWindow.ShowScrool();

        for (int i = 0; i < list.Count; i++)
        {
            var iniPosHere = _bulletinWindow.ScrollIniPosGo.transform.localPosition +
                             new Vector3(0, -3.5f * i, 0);

            var spec = new SpecData(list[i]);

            var a = SpecTile.CreateTile(_bulletinWindow.Content.gameObject.transform, spec,
                iniPosHere);

            _reportsSpec.Add(a);
        }
    }





    //Exports

    List<ExportData> _exports = new List<ExportData>();
    public List<ExportData> Exports
    {
        get
        {
            return _exports;
        }

        set
        {
            _exports = value;
        }
    }

    public void AddNewExport(string building, P prod, float amt, float money)
    {
        var data = new ExportData(Program.gameScene.GameTime1.CurrentDate(),
            building, prod + "", amt, money);

        _exports.Insert(0, data);//1
    }

    internal void ShowExports()
    {
        ExportData[] arr = new ExportData[_exports.Count + 1];
        arr[0] = AddTitleBarExports();

        for (int i = 0; i < _exports.Count; i++)
        {
            arr[i + 1] = _exports[i];
        }

        ShowExports(arr);
        _bulletinWindow.AdjustContentHeight(arr.Length * 3.75f);

    }


    List<SpecTile> _reportsExports = new List<SpecTile>();
    private void ShowExports(ExportData[] list)
    {
        Hide();
        _bulletinWindow.ShowScrool();

        for (int i = 0; i < list.Length; i++)
        {
            var iniPosHere = _bulletinWindow.ScrollIniPosGo.transform.localPosition +
                             new Vector3(0, -3.5f * i, 0);

            var a = SpecTile.CreateTile(_bulletinWindow.Content.gameObject.transform, list[i],
                iniPosHere);

            _reportsExports.Add(a);
        }
    }

    ExportData AddTitleBarExports()
    {
        return new ExportData(null, "Building", "Product", 0, 0);
    }

    #endregion
}


/// <summary>
/// For modularity
/// </summary>
public class SpecData
{
    ProductInfo _prodInfo;

    public ProductInfo ProdInfo
    {
        get { return _prodInfo; }
        set { _prodInfo = value; }
    }
    float _price;

    public float Price
    {
        get { return _price; }
        set { _price = value; }
    }

    public SpecData(ProductInfo pInfo)
    {
        _prodInfo = pInfo;
        _price = Program.gameScene.ExportImport1.ReturnPrice(_prodInfo.Product);

        //title bar
        if (_prodInfo.Product == P.Product)
        {
            _price = -100;
        }
    }

}

public class ExportData
{
    MDate _mDate;
    string _building;
    string _prod;
    float _amt;
    float _money;

    public MDate MDate
    {
        get
        {
            return _mDate;
        }

        set
        {
            _mDate = value;
        }
    }

    public string Building
    {
        get
        {
            return _building;
        }

        set
        {
            _building = value;
        }
    }

    public string Prod
    {
        get
        {
            return _prod;
        }

        set
        {
            _prod = value;
        }
    }

    public float Amt
    {
        get
        {
            return _amt;
        }

        set
        {
            _amt = value;
        }
    }

    public float Money
    {
        get
        {
            return _money;
        }

        set
        {
            _money = value;
        }
    }

    public ExportData() { }

    public ExportData(MDate mDate, string building, string prod, float amt, float money)
    {
        MDate = mDate;
        Building = building;
        Prod = prod;
        Amt = amt;
        Money = money;
    }
}
