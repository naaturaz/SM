using System.Collections.Generic;
using System.Linq;

/*
 * Here will be the price Base for all products and the variability they will have
 *
 * The Export and Import action happen in Dispatch.cs
 */

public class ExportImport
{
    private List<ProdSpec> _prodSpecs = new List<ProdSpec>();
    private List<ProdSpec> _townProdSpecs = new List<ProdSpec>();

    //craeted for GC reasons
    private Dictionary<P, int> _prodSpecsGC = new Dictionary<P, int>();

    private Dictionary<P, int> _townProdSpecsGC = new Dictionary<P, int>();

    public ExportImport()
    {
        LoadBasePrices();

        _townProdSpecs.AddRange(_prodSpecs);
        _townProdSpecs.RemoveAt(0);
        _townProdSpecs.RemoveAt(0);
        _townProdSpecs.RemoveAt(0);

        MapDict();
    }

    public List<ProdSpec> TownProdSpecs
    {
        get { return _townProdSpecs; }
        set { _townProdSpecs = value; }
    }

    public List<ProdSpec> ProdSpecs
    {
        get { return _prodSpecs; }
        set { _prodSpecs = value; }
    }

    private void MapDict()
    {
        _prodSpecs = _prodSpecs.OrderBy(a => a.Product).ToList();

        for (int i = 0; i < _prodSpecs.Count; i++)
        {
            _prodSpecsGC.Add(_prodSpecs[i].Product, i);
        }

        //town
        _townProdSpecs = _townProdSpecs.OrderBy(a => a.Product).ToList();
        for (int i = 0; i < _townProdSpecs.Count; i++)
        {
            _townProdSpecsGC.Add(_townProdSpecs[i].Product, i);
        }
    }

    //
    private List<ProdSpec> _prodSpecsCured = new List<ProdSpec>();

    /// <summary>
    /// Is just a called for one that doesnt have the Random and YEar
    /// </summary>
    /// <returns></returns>
    internal List<ProdSpec> ProdSpecsCured()
    {
        if (_prodSpecsCured.Count == 0)
        {
            _prodSpecsCured.AddRange(_townProdSpecs);

            _prodSpecsCured = _prodSpecsCured.OrderBy(a => a.ProductPriority()).ThenBy(a => a.ProductLang()).ToList();
        }
        return _prodSpecsCured;
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
    ///
    /// The Sprite Icion should be added on Prefab/GUI/Inventory_Items/Brick and just named same as Prod name
    ///
    /// </summary>
    private void LoadBasePrices()
    {
        _prodSpecs.Add(new ProdSpec(P.RandomMineOutput, 150, 4100, 100));
        _prodSpecs.Add(new ProdSpec(P.RandomFoundryOutput, 150, 4100, 100));

        //for report purposes, and needed here only  for the icon root
        _prodSpecs.Add(new ProdSpec(P.Year, 150, 4100, 100));

        _prodSpecs.Add(new ProdSpec(P.Bean, 90, 368, 100));
        _prodSpecs.Add(new ProdSpec(P.CoffeeBean, 100, 308, 100, 30 * 6));
        _prodSpecs.Add(new ProdSpec(P.Cacao, 110, 250, 100, 360 * 3));
        _prodSpecs.Add(new ProdSpec(P.Potato, 70, 380, 100, 30 * 8));
        _prodSpecs.Add(new ProdSpec(P.Banana, 100, 350, 100));
        _prodSpecs.Add(new ProdSpec(P.Coconut, 100, 550, 100));

        //new foods
        _prodSpecs.Add(new ProdSpec(P.CornFlower, 100, 550, 100, 30 * 12));
        _prodSpecs.Add(new ProdSpec(P.Bread, 100, 550, 100, 30 * 1));
        _prodSpecs.Add(new ProdSpec(P.Carrot, 100, 550, 100, 7 * 5));
        _prodSpecs.Add(new ProdSpec(P.Tomato, 100, 550, 100, 30 * 1));
        _prodSpecs.Add(new ProdSpec(P.Cucumber, 100, 550, 100, 30 * 1));
        _prodSpecs.Add(new ProdSpec(P.Cabbage, 100, 550, 100, 30 * 1));
        _prodSpecs.Add(new ProdSpec(P.Lettuce, 100, 550, 100, 30 * 1));
        _prodSpecs.Add(new ProdSpec(P.SweetPotato, 100, 550, 100, 30 * 8));
        _prodSpecs.Add(new ProdSpec(P.Cassava, 100, 550, 100, 30 * 8));
        _prodSpecs.Add(new ProdSpec(P.Pineapple, 100, 550, 100, 30 * 1));
        _prodSpecs.Add(new ProdSpec(P.Papaya, 100, 550, 100, 30 * 1));

        //foods
        _prodSpecs.Add(new ProdSpec(P.Corn, 60, 540, 120, 6 * 30));
        _prodSpecs.Add(new ProdSpec(P.CornMeal, 90, 700, 130, 12 * 30));

        _prodSpecs.Add(new ProdSpec(P.Chicken, 200, 881, 80, 30 * 1));
        _prodSpecs.Add(new ProdSpec(P.Egg, 150, 400, 100, 30 * 1));
        _prodSpecs.Add(new ProdSpec(P.Pork, 200, 881, 70, 30 * 1));
        _prodSpecs.Add(new ProdSpec(P.Beef, 300, 881, 60, 30 * 1));

        _prodSpecs.Add(new ProdSpec(P.Fish, 300, 932, 70, 30 * 1));
        _prodSpecs.Add(new ProdSpec(P.Chocolate, 900, 600, 70, 30 * 12));
        _prodSpecs.Add(new ProdSpec(P.Salt, 30, 900, 85));

        _prodSpecs.Add(new ProdSpec(P.Sugar, 800, 900, 70));
        _prodSpecs.Add(new ProdSpec(P.SugarCane, 50, 200.2f, 100, 30 * 12));

        //procesed foods
        _prodSpecs.Add(new ProdSpec(P.Candy, 1600, 200, 60));

        //frutas
        //_prodSpecs.Add(new ProdSpec(P.Mango, 100, 550, 100));
        //_prodSpecs.Add(new ProdSpec(P.Avocado, 100, 550, 100));
        //_prodSpecs.Add(new ProdSpec(P.Guava, 100, 550, 100));
        //_prodSpecs.Add(new ProdSpec(P.Orange, 100, 550, 100));

        //liquids
        _prodSpecs.Add(new ProdSpec(P.Water, 1, 1000, 100));
        _prodSpecs.Add(new ProdSpec(P.Honey, 50, 1000, 100));
        _prodSpecs.Add(new ProdSpec(P.Beer, 150, 1000, 100));
        _prodSpecs.Add(new ProdSpec(P.Rum, 500, 1000, 100));
        _prodSpecs.Add(new ProdSpec(P.Wine, 450, 990, 100));
        _prodSpecs.Add(new ProdSpec(P.Ink, 100, 990, 100));
        _prodSpecs.Add(new ProdSpec(P.WhaleOil, 200, 1090, 100));

        _prodSpecs.Add(new ProdSpec(P.Coal, 20, 180, 70));
        _prodSpecs.Add(new ProdSpec(P.Sulfur, 20, 1960, 70));
        _prodSpecs.Add(new ProdSpec(P.Potassium, 30, 862, 60));

        _prodSpecs.Add(new ProdSpec(P.TobaccoLeaf, 50, 300, 90));

        _prodSpecs.Add(new ProdSpec(P.Henequen, 15, 400, 90));
        _prodSpecs.Add(new ProdSpec(P.Cotton, 40, 360, 120));
        _prodSpecs.Add(new ProdSpec(P.Leather, 70, 570, 20));

        _prodSpecs.Add(new ProdSpec(P.Crockery, 100, 100, 10));//ceramic
        _prodSpecs.Add(new ProdSpec(P.Gold, 1000, 19300, 5));
        _prodSpecs.Add(new ProdSpec(P.Diamond, 2000, 3539, 5));
        _prodSpecs.Add(new ProdSpec(P.Silver, 900, 10490, 8));
        _prodSpecs.Add(new ProdSpec(P.Jewel, 3000, 8490, 8));

        //_prodSpecs.Add(new ProdSpec(P.CrystalCoin, 1000, 8120, 5));
        //_prodSpecs.Add(new ProdSpec(P.CaribbeanCoin, 1500, 11441, 5));
        _prodSpecs.Add(new ProdSpec(P.Coin, 1500, 14550, 5));

        _prodSpecs.Add(new ProdSpec(P.Stone, 50, 2515, 20));
        _prodSpecs.Add(new ProdSpec(P.Ore, 70, 4200, 20));
        _prodSpecs.Add(new ProdSpec(P.Iron, 150, 7874, 15));
        _prodSpecs.Add(new ProdSpec(P.Machinery, 1050, 2487, 5));

        _prodSpecs.Add(new ProdSpec(P.Steel, 200, 7850, 15));
        _prodSpecs.Add(new ProdSpec(P.Nail, 200, 4874, 20));//for ship repairs

        //militar
        _prodSpecs.Add(new ProdSpec(P.CannonBall, 500, 6874, 20));
        _prodSpecs.Add(new ProdSpec(P.CannonPart, 1200, 5944, 20));
        _prodSpecs.Add(new ProdSpec(P.Weapon, 1000, 5000, 8));
        //_prodSpecs.Add(new ProdSpec(P.Sword, 150, 6000, 8));
        //_prodSpecs.Add(new ProdSpec(P.Axe, 50, 2500, 10));
        _prodSpecs.Add(new ProdSpec(P.GunPowder, 500, 1281, 60));

        _prodSpecs.Add(new ProdSpec(P.Clay, 10, 30, 100));
        _prodSpecs.Add(new ProdSpec(P.PalmLeaf, 10, 30));

        _prodSpecs.Add(new ProdSpec(P.Wood, 5, 800, 100));
        _prodSpecs.Add(new ProdSpec(P.Tool, 500, 3000, 15));
        _prodSpecs.Add(new ProdSpec(P.Utensil, 200, 3000, 15));

        _prodSpecs.Add(new ProdSpec(P.Brick, 150, 2000, 100));
        _prodSpecs.Add(new ProdSpec(P.Mortar, 350, 2500, 100));
        _prodSpecs.Add(new ProdSpec(P.QuickLime, 25, 1800, 100));
        _prodSpecs.Add(new ProdSpec(P.Sand, 20, 1500, 100));

        _prodSpecs.Add(new ProdSpec(P.Barrel, 300, 100, 50));
        _prodSpecs.Add(new ProdSpec(P.Bucket, 300, 90, 60));
        _prodSpecs.Add(new ProdSpec(P.Crate, 300, 80, 50));
        _prodSpecs.Add(new ProdSpec(P.WheelBarrow, 1400, 40, 25, weightPerUnit: 12));
        _prodSpecs.Add(new ProdSpec(P.Cart, 1400, 20, 20, weightPerUnit: 120));
        _prodSpecs.Add(new ProdSpec(P.Furniture, 840, 10, 15, weightPerUnit: 20));

        _prodSpecs.Add(new ProdSpec(P.Cigar, 450, 700, 50));
        _prodSpecs.Add(new ProdSpec(P.CigarBox, 200, 200, 60));
        //_prodSpecs.Add(new ProdSpec(P.Slat, 40, 600, 70));
        _prodSpecs.Add(new ProdSpec(P.FloorTile, 600, 2100, 90));
        _prodSpecs.Add(new ProdSpec(P.RoofTile, 600, 2100, 90));

        _prodSpecs.Add(new ProdSpec(P.Fabric, 300, 400, 20));
        _prodSpecs.Add(new ProdSpec(P.Cloth, 120, 380, 20));
        _prodSpecs.Add(new ProdSpec(P.Wool, 100, 300, 20));

        _prodSpecs.Add(new ProdSpec(P.Sail, 350, 200, 15));
        _prodSpecs.Add(new ProdSpec(P.String, 200, 321, 15));
        _prodSpecs.Add(new ProdSpec(P.Shoe, 220, 100, 10));

        _prodSpecs.Add(new ProdSpec(P.Paper, 150, 192, 30));
        _prodSpecs.Add(new ProdSpec(P.Map, 300, 292, 50));
        _prodSpecs.Add(new ProdSpec(P.Book, 500, 502, 5));
        _prodSpecs.Add(new ProdSpec(P.Rubber, 500, 502, 50));

        // _prodSpecs.Add(new ProdSpec(P.Silk, 150, 1300, 5));
    }

    /// <summary>
    /// Will calculate the volume of a Product given the mass in KG
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="mass"></param>
    /// <returns></returns>
    public float CalculateVolume(P prod, float mass)
    {
        var prodLo = FindProdSpec(prod);

        if (prodLo == null)
        {
            //Debug.Log("prod not found!:"+prod);
            return 0;
        }

        var dens = prodLo.Density;

        return mass / dens;
    }

    /// <summary>
    /// For a Cubic Meter . How much can be store in a Cubic Meter
    /// Returing Weitg KG
    /// </summary>
    /// <param name="prod"></param>
    /// <returns></returns>
    public float CalculateMass(P prod, float cubicMetersVol)
    {
        var prodLo = FindProdSpec(prod);

        if (prodLo == null)
        {
            //Debug.Log("prod not found!:" + prod);
            return 0;
        }

        //returns the Mass
        return prodLo.Density * cubicMetersVol;
    }

    /// <summary>
    /// The action of selling a Product and the amount
    ///
    /// The sale happens when export
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="amt"></param>
    public void Sale(P prod, float amt, string building)
    {
        var trans = CalculateTransaction(prod, amt);
        Program.gameScene.GameController1.Dollars += trans;
        Program.gameScene.GameController1.NotificationsManager1.Notify("ShipPayed", trans.ToString("N0"));

        if (trans == 0 || BulletinWindow.SubBulletinFinance1 == null)
        {
            return;
        }

        BulletinWindow.SubBulletinFinance1.FinanceLogger.AddToAcct("Exports", trans);
        BulletinWindow.SubBulletinFinance1.AddNewExportOrImport(building, prod, amt, trans, "Export");
        Quest(prod, trans);
    }

    private void Quest(P prod, float amt)
    {
        Program.gameScene.QuestManager.AddToQuest("MakeBucks", amt);
    }

    /// <summary>
    /// The action of buying a Product and the amount
    ///
    /// The buy happens when import
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="amt"></param>
    public void Buy(P prod, float amt, string building)
    {
        var trans = CalculateTransaction(prod, amt);
        Program.gameScene.GameController1.Dollars -= trans;

        //Program.gameScene.GameController1.NotificationsManager1.Notify("ShipPayed", trans.ToString("N0"));

        BulletinWindow.SubBulletinFinance1.FinanceLogger.AddToAcct("Imports", trans);
        BulletinWindow.SubBulletinFinance1.AddNewExportOrImport(building, prod, amt, trans, "Import");
    }

    public float CalculateTransaction(P prod, float amt)
    {
        return ReturnPrice(prod) * amt;
    }

    public float ReturnDensityKGM3(P prod)
    {
        var prodFound = FindProdSpec(prod);

        if (prodFound == null)
        {
            //Debug.Log("ReturnDensityKGM3 asked of not found prod:" + prod);
            return 0;
        }

        return _prodSpecs.Find(a => a.Product == prod).Density;
    }

    //todo GC . pass Index of List. map index to Prod while creating the Dict
    public ProdSpec FindProdSpec(P prod)
    {
        if (_prodSpecsGC.ContainsKey(prod))
        {
            var index = _prodSpecsGC[prod];
            return _prodSpecs[index];
        }
        return null;
    }

    public float ReturnProduceFactor(P prod)
    {
        var prodFound = FindProdSpec(prod);

        if (prodFound == null)
        {
            //Debug.Log("Prod Factor asked of not found prod:" + prod);
            return 0;
        }
        return prodFound.ProduceFactor;
    }

    public float ReturnExpirationInDays(P prod)
    {
        var prodFound = FindProdSpec(prod);

        if (prodFound == null)
        {
            //Debug.Log("Prod Factor asked of not found prod:" + prod);
            return 0;
        }
        return prodFound.ExpireDays;
    }

    public int ReturnExpireDays(P prod)
    {
        var prodFound = FindProdSpec(prod);

        if (prodFound == null)
        {
            //Debug.Log("Prod Factor asked of not found prod:" + prod);
            return 0;
        }
        return prodFound.ExpireDays;
    }

    public float ReturnPrice(P prod)
    {
        var prodFound = FindProdSpec(prod);

        if (prodFound == null)
        {
            //Debug.Log("ReturnPrice asked of not found prod:" + prod);
            return 0;
        }
        return prodFound.Price;
    }

    public string ReturnIconRoot(P prod)
    {
        var prodFound = FindProdSpec(prod);

        if (prodFound == null)
        {
            return "";
        }
        return prodFound.IconRoot;
    }

    //Town Prices
    internal float ReturnPriceTown(P prod)
    {
        var prodFound = FindTownProdSpec(prod);

        if (prodFound == null)
        {
            //Debug.Log("ReturnPrice asked of not found prod:" + prod);
            return 0;
        }
        return prodFound.Price;
    }

    //todo GC . pass Index of List. map index to Prod while creating the Dict
    private ProdSpec FindTownProdSpec(P prod)
    {
        if (_townProdSpecsGC.ContainsKey(prod))
        {
            var index = _townProdSpecsGC[prod];
            return _townProdSpecs[index];
        }
        return null;
    }
}

/// <summary>
/// Will hold the product and its base price
/// Also the Density of the product
/// and the produce factor:  Producing item factor. Can produce more KG of rice than Ceramic
///
/// The Sprite Icion should be added on Prefab/GUI/Inventory_Items/Brick
///
/// </summary>
public class ProdSpec
{
    public P Product;
    public float Price;

    private float _density;
    private float _produceFactor;

    private string _iconRoot;

    //how many days can be used before is expired
    private int _expireDays;

    /// <summary>
    /// how muach weight a unit of this
    /// Useful for WheelBarrow, Furniture, etc
    /// </summary>
    private float _weightPerUnit;

    public float WeightPerUnit
    {
        get { return _weightPerUnit; }
        set { _weightPerUnit = value; }
    }

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

    /// <summary>
    /// The root of the icon
    /// </summary>
    public string IconRoot
    {
        get { return _iconRoot; }
        set { _iconRoot = value; }
    }

    public int ExpireDays
    {
        get { return _expireDays; }
        set { _expireDays = value; }
    }

    public ProdSpec()
    {
    }

    /// <summary>
    /// The price is divided by 100 in Constructor
    /// </summary>
    /// <param name="prod"></param>
    /// <param name="price"></param>
    /// <param name="density"></param>
    /// <param name="produceFactor"></param>
    public ProdSpec(P prod, float price, float density = 1, float produceFactor = 1, int expireDays = -1,
        float weightPerUnit = -1)
    {
        Product = prod;
        Price = price / 100;//500  900 1000
        Density = density;
        ProduceFactor = produceFactor;

        _iconRoot = "Prefab/GUI/Inventory_Icons/" + prod;
        _expireDays = expireDays;
        _weightPerUnit = weightPerUnit;
    }

    internal object ProductLang()
    {
        return Languages.ReturnString(Product + "");
    }

    internal object ProductPriority()
    {
        return Product == P.Year ? 0 : 1;
    }
}