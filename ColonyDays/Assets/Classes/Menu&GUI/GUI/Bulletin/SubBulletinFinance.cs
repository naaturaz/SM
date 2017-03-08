﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SubBulletinFinance
{
    private BulletinWindow _bulletinWindow;


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
        

        //_bulletinWindow.ShowInBody("Finance B (Coming soon)");

        ShowBudgetAccts(_financeLogger.ResumenCurrentBudget());
    }



    List<AcctTile> _reportsBudget = new List<AcctTile>();
    private void ShowBudgetAccts(List<DisplayAccount> list)
    {
        Hide();
        _bulletinWindow.ShowScrool();

        for (int i = 0; i < list.Count; i++)
        {
            var iniPosHere = _bulletinWindow.ScrollIniPosGo.transform.localPosition +
                             new Vector3(0, -5f * i, 0);

            var a = AcctTile.CreateTile(_bulletinWindow.Content.gameObject.transform, list[i],
                iniPosHere);

            _reportsBudget.Add(a);
        }
    }

    #endregion







    #region Prices
    internal void ShowPrices()
    {
        ShowPrices(GetAllInInventories());
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

    }



    #region Specs

    internal void ShowSpecs()
    {
        ShowSpecs(GetAllSpecs());
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
    string _price;

    public string Price
    {
        get { return _price; }
        set { _price = value; }
    }

    public SpecData(ProductInfo pInfo)
    {
        _prodInfo = pInfo;
        _price = Program.gameScene.ExportImport1.ReturnPrice(_prodInfo.Product).ToString("C2");

        //title bar
        if (_prodInfo.Product == P.Product)
        {
            _price = "Price";
        }
    }

}
