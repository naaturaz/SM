using System.Collections.Generic;
using UnityEngine;
/*
 * Here will be the price Base for all products and the variability they will have 
 * 
 * The Export and Import action happen in Dispatch.cs
 * 
 * 
 */

public class ExportImport
{
    List<ProdSpec> _prodSpecs = new List<ProdSpec>(); 

    public ExportImport()
    {
        LoadBasePrices();
    }

    //Densities
    //http://www.engineeringtoolbox.com/foods-materials-bulk-density-d_1819.html
    //http://www.sugartech.co.za/density/
    //http://go.key.net/rs/key/images/Bulk%20Density%20Averages%20100630.pdf
    //http://www.simetric.co.uk/si_materials.htm
    //http://www.engineeringtoolbox.com/density-materials-d_1652.html

    //Glass; Density kg/m3, 2400-2800

    /// <summary>
    /// Will load the base price of each prod
    /// </summary>
    private void LoadBasePrices()
    {
        _prodSpecs.Add(new ProdSpec(P.Bean, 90, 368, 100));
        _prodSpecs.Add(new ProdSpec(P.Potato, 70, 380, 100));
        _prodSpecs.Add(new ProdSpec(P.SugarCane, 50, 200.2f, 100));
        _prodSpecs.Add(new ProdSpec(P.Corn, 60, 540, 120));

        _prodSpecs.Add(new ProdSpec(P.Chicken, 200, 881, 80));
        _prodSpecs.Add(new ProdSpec(P.Egg, 150, 400, 100));
        _prodSpecs.Add(new ProdSpec(P.Pork, 200, 881, 70));
        _prodSpecs.Add(new ProdSpec(P.Beef, 300, 881, 60));

        _prodSpecs.Add(new ProdSpec(P.Fish, 300, 932, 70));
        _prodSpecs.Add(new ProdSpec(P.Salt, 30, 900, 85));

        _prodSpecs.Add(new ProdSpec(P.Sugar, 50, 900, 70));

        _prodSpecs.Add(new ProdSpec(P.Tobacco, 50, 300, 90));
        _prodSpecs.Add(new ProdSpec(P.Cotton, 40, 360, 120));
        _prodSpecs.Add(new ProdSpec(P.Leather, 70, 570, 20));




        _prodSpecs.Add(new ProdSpec(P.Clay, 10, 100, 10));//ceramic
        _prodSpecs.Add(new ProdSpec(P.Gold, 500, 19300, 5));
        _prodSpecs.Add(new ProdSpec(P.Stone, 50, 2515, 20));
        _prodSpecs.Add(new ProdSpec(P.Iron, 150, 7874, 15));


        _prodSpecs.Add(new ProdSpec(P.Resin, 10, 30));
        _prodSpecs.Add(new ProdSpec(P.Wood, 10, 500, 90));
        _prodSpecs.Add(new ProdSpec(P.Axe, 50, 2500, 10));
        _prodSpecs.Add(new ProdSpec(P.Tool, 150, 3000, 15));
        _prodSpecs.Add(new ProdSpec(P.Sword, 150, 6000, 8));


        _prodSpecs.Add(new ProdSpec(P.Brick, 50, 2000, 100));
        _prodSpecs.Add(new ProdSpec(P.Tonel, 60, 50, 50));
        _prodSpecs.Add(new ProdSpec(P.Cigar, 200, 700, 50));
        _prodSpecs.Add(new ProdSpec(P.Slat, 40, 600, 70));
        _prodSpecs.Add(new ProdSpec(P.Tile, 60, 2100, 90));


        _prodSpecs.Add(new ProdSpec(P.Fabric, 100, 400, 20));
        _prodSpecs.Add(new ProdSpec(P.GunPowder, 100, 1281, 60));
        _prodSpecs.Add(new ProdSpec(P.Paper, 150, 192, 30));
        _prodSpecs.Add(new ProdSpec(P.Books, 300, 500, 5));
        _prodSpecs.Add(new ProdSpec(P.Silk, 150, 1300, 5));
    }

    /// <summary>
    /// Will calculate the volume of a Product given the mass in KG 
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="mass"></param>
    /// <returns></returns>
    public float CalculateVolume(P prod, float mass)
    {
        var prodLo = _prodSpecs.Find(a => a.Product == prod);

        if (prodLo == null)
        {
            Debug.Log("prod not found!:"+prod);
            return 0;
        }

        var dens = prodLo.Density;

        return mass/dens;
    }

    /// <summary>
    /// For a Cubic Meter . How much can be store in a Cubic Meter
    /// </summary>
    /// <param name="prod"></param>
    /// <returns></returns>
    public float CalculateMass(P prod, float cubicMeters)
    {
        var prodLo = _prodSpecs.Find(a => a.Product == prod);

        if (prodLo == null)
        {
            Debug.Log("prod not found!:" + prod);
            return 0;
        }

        //returns the Mass
        return prodLo.Density*cubicMeters;
    }


    /// <summary>
    /// The action of selling a Product and the amount
    /// 
    /// The sale happens when export
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="amt"></param>
    public void Sale(P prod, float amt)
    {
        var trans = CalculateTransaction(prod, amt);
        Program.gameScene.GameController1.Dollars += trans;
    }

    /// <summary>
    /// The action of buying a Product and the amount
    /// 
    /// The buy happens when import 
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="amt"></param>
    public void Buy(P prod, float amt)
    {
        var trans = CalculateTransaction(prod, amt);
        Program.gameScene.GameController1.Dollars -= trans;
    }

    float CalculateTransaction(P prod, float amt)
    {
        return ReturnPrice(prod) * amt;
    }

    float ReturnPrice(P prod)
    {
        return _prodSpecs.Find(a => a.Product == prod).Price;
    }
}

/// <summary>
/// Will hold the product and its base price
/// Also the Density of the product 
/// and the produce factor:  Producing item factor. Can produce more KG of rice than Ceramic
/// </summary>
public class ProdSpec
{
    public P Product;
    public float Price;

    private float _density;
    private float _produceFactor;

    /// <summary>
    /// The amount of Cubic Meters Needed to fit one KG of this Product
    /// 
    /// Density in kg/m3
    /// </summary>
    public float Density
    {
        get { return _density; }
        set { _density = value; }
    }

    /// <summary>
    /// Producing item factor. Can produce more KG of rice than Ceramic
    /// This Factor is the one that reflects that . For ex Rice might be 10 and ceramic 5
    /// </summary>
    public float ProduceFactor
    {
        get { return _produceFactor; }
        set { _produceFactor = value; }
    }

    public ProdSpec(){}

    public ProdSpec(P prod, float price, float density = 1, float produceFactor = 1)
    {
        Product = prod;
        Price = price;
        Density = density;
        ProduceFactor = produceFactor;
    }
}
