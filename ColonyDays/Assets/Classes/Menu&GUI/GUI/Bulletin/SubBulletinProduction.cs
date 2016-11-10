﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SubBulletinProduction
{
    private ProductionReport _productionReport;
    private ProductionReport _expirationReport;
    private BulletinWindow _bulletinWindow;

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

    public void ShowProdReport()
    {
        if (_productionReport == null)
        {
            _bulletinWindow.ShowInBody("Nothing");
            return;
        }

        ShowProductionReport(_productionReport.ProduceReport);
    }  
    
    public void ShowConsumeReport()
    {
        if (_productionReport == null)
        {
            _bulletinWindow.ShowInBody("Nothing");
            return;
        }

        ShowProductionReport(_productionReport.ConsumeReport);
    }   
    
    public void ShowExpirationReport()
    {
        if (_expirationReport == null)
        {
            _bulletinWindow.ShowInBody("Nothing");
            return;
        }

        ShowProductionReport(_expirationReport.ProduceReport);
    }





    List<ShowAInventory> _reports = new List<ShowAInventory>();
    private void ShowProductionReport(List<Inventory> list)
    {
        Hide();
        _bulletinWindow.ShowScrool();

        var pastItems = 0;
        for (int i = 0; i < ShowLastYears(list); i++)
        {
            var a = new ShowAInventory(list[i], _bulletinWindow.Content.gameObject,
                _bulletinWindow.ScrollIniPosGo.transform.localPosition + new Vector3(0, pastItems * -3.5f * i, 0));
            
            _reports.Add(a);
            pastItems = list[i].InventItems.Count;
        }
    }

    int ShowLastYears(List<Inventory> list)
    {
        if (list == null)
        {
            return 0;
        }

        if (list.Count < 6)
        {
            return list.Count;
        }
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



