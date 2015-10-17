using System.Collections.Generic;

/*
 * Here will be the price Base for all products and the variability they will have 
 * 
 * The Export and Import action happen in Dispatch.cs
 * 
 * 
 */

public class ExportImport
{
    List<ProdPrice> _prodPrices = new List<ProdPrice>(); 

    public ExportImport()
    {
        LoadBasePrices();
    }

    /// <summary>
    /// Will load the base price of each prod
    /// </summary>
    private void LoadBasePrices()
    {
        _prodPrices.Add(new ProdPrice(P.Bean, 900));
        _prodPrices.Add(new ProdPrice(P.Potato, 700));
        _prodPrices.Add(new ProdPrice(P.SugarCane, 500));
        _prodPrices.Add(new ProdPrice(P.Corn, 600));

        _prodPrices.Add(new ProdPrice(P.Chicken, 2000));
        _prodPrices.Add(new ProdPrice(P.Egg, 1500));
        _prodPrices.Add(new ProdPrice(P.Pork, 2000));
        _prodPrices.Add(new ProdPrice(P.Beef, 3000));

        _prodPrices.Add(new ProdPrice(P.Fish, 3000));

        _prodPrices.Add(new ProdPrice(P.Sugar, 500));

        _prodPrices.Add(new ProdPrice(P.Tobacco, 500));
        _prodPrices.Add(new ProdPrice(P.Cotton, 500));
        _prodPrices.Add(new ProdPrice(P.Leather, 500));




        _prodPrices.Add(new ProdPrice(P.Clay, 100));
        _prodPrices.Add(new ProdPrice(P.Gold, 5000));
        _prodPrices.Add(new ProdPrice(P.Stone, 500));
        _prodPrices.Add(new ProdPrice(P.Iron, 1500));


        _prodPrices.Add(new ProdPrice(P.Resin, 100));
        _prodPrices.Add(new ProdPrice(P.Wood, 100));
        _prodPrices.Add(new ProdPrice(P.Axe, 500));
        _prodPrices.Add(new ProdPrice(P.Tool, 1500));
        _prodPrices.Add(new ProdPrice(P.Sword, 1500));


        _prodPrices.Add(new ProdPrice(P.Brick, 500));
        _prodPrices.Add(new ProdPrice(P.Tonel, 600));
        _prodPrices.Add(new ProdPrice(P.Cigar, 2000));
        _prodPrices.Add(new ProdPrice(P.Slat, 400));
        _prodPrices.Add(new ProdPrice(P.Tile, 600));


        _prodPrices.Add(new ProdPrice(P.Fabric, 1000));
        _prodPrices.Add(new ProdPrice(P.GunPowder, 1000));
        _prodPrices.Add(new ProdPrice(P.Paper, 1500));
        _prodPrices.Add(new ProdPrice(P.Books, 3000));
        _prodPrices.Add(new ProdPrice(P.Silk, 1500));
    }


    /// <summary>
    /// The action of selling a Product and the amount
    /// 
    /// The sale happens when export
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="amt"></param>
    public void Sale(P prod, int amt)
    {
        var trans = CalculateTransaction(prod, amt);
        GameController.Dollars += trans;
    }

    /// <summary>
    /// The action of buying a Product and the amount
    /// 
    /// The buy happens when import 
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="amt"></param>
    public void Buy(P prod, int amt)
    {
        var trans = CalculateTransaction(prod, amt);
        GameController.Dollars -= trans;
    }

    int CalculateTransaction(P prod, int amt)
    {
        return ReturnPrice(prod) * amt;
    }

    int ReturnPrice(P prod)
    {
        return _prodPrices.Find(a => a.Product == prod).Price;
    }
}

/// <summary>
/// Will hold the product and its base price 
/// </summary>
public class ProdPrice
{
    public P Product;
    public int Price;

    public ProdPrice(){}

    public ProdPrice(P prod, int price)
    {
        Product = prod;
        Price = price;
    }
}
