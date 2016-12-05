using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Production  {

    //if product doesnt have ingredients must be added here alone 
    //if has ingredients must be added here and in _inputProducts
    Food _food = new Food();

    public Production()
    {
        LoadInputProductsStats();
    }

    public Food Food1
    {
        get { return _food; }
        set { _food = value; }
    }




    /// <summary>
    /// Which will be the first element of the List 
    /// </summary>
    /// <param name="typeKey"></param>
    /// <returns></returns>
    public ProductInfo ReturnDefaultProd(H typeKey)
    {
        var prod = _inputProducts.Find(a => a.HType.Contains(typeKey));

        if (prod != null)
        {
            return prod;
        }
        //so i dont have to check for Null Ref in a lot of places 
        return new ProductInfo(P.None, null, H.None);
    }

    /// <summary>
    /// Will return the prod the building can produce
    /// </summary>
    /// <param name="typeKey"></param>
    /// <returns></returns>
    public List<ProductInfo> ReturnProducts(H typeKey)
    {
        var prods = _inputProducts.Where(a => a.HType.Contains(typeKey)).ToList();

        return prods;
    }

    /// <summary>
    /// Will return the prod the building can produce
    /// </summary>
    /// <param name="typeKey"></param>
    /// <returns></returns>
    public ProductInfo ReturnExactProduct(int IdP)
    {
        var prod = _inputProducts.Find(a => a.Id == IdP);

        if (prod != null)
        {
            return prod;
        }

        //the Stop ID
        if (IdP == 1000)
        {
            return new ProductInfo(P.Stop, null, H.None);
        }
        
       //Debug.Log("Prod not found: ReturnExactProduct(). pls check");
        return null;
    }

    /// <summary>
    /// To work the element u are passing as param mus be unique!
    /// </summary>
    /// <param name="output"></param>
    /// <returns></returns>
    public ProductInfo ReturnProdInfoWithOutput(P output)
    {
        var prod = _inputProducts.Find(a => a.Product == output);

        if (prod != null)
        {
            return prod;
        }

       //Debug.Log("Prod not found: ReturnProdInfoWithOutput(). pls check");
        return null;
    }

    #region Inputs for Create a Product

    //contains which products are needed in the input to generate 1 unit of the final product
    List<ProductInfo> _inputProducts = new List<ProductInfo>();

    InputElement _eleWoodComb = new InputElement(P.Wood, 0.1f);
    InputElement _eleCoalComb = new InputElement(P.Coal, 0.1f);


    void InputProdCheckAndAdd(ProductInfo pInfo)
    {
        _inputProducts.Add(pInfo);
    }

    private void LoadInputProductsStats()
    {
        //Loading each prodcut input

        LoadAllGenerics();
        Mine();
        
        FieldFarm();

        CannonParts();
        Rum();
        Chocolate();
        Ink();
        SteelFoundry();
        Foundry();

        //Meats();

        GunPodwer();

        Clay();
        Brick();
        Ceramic();

        Carpintery();
        BlackSmith();

        Cigar();

        Tailor();

        Tile();

        Fabric();

        Coins();
        PaperNewsAndBook();
        Sugar();

        Houses();
    }

    /// <summary>
    /// just bz they need water as input so inventory works fine
    /// </summary>
    void Houses()
    {
        List<H> listH = new List<H>()
        {
            H.Bohio, H.WoodHouseA, H.WoodHouseB,
            H.BrickHouseA ,
            H.BrickHouseB ,
            H.BrickHouseC ,
            H.WoodHouseC,
        };

        InputElement water = new InputElement(P.Water, 1f);
        List<InputElement> waterForm = new List<InputElement>() { water };
        InputProdCheckAndAdd(new ProductInfo(P.None, waterForm, listH));
    }

    private void CannonParts()
    {
        InputElement iron = new InputElement(P.Iron, 10);
        List<InputElement> i1 = new List<InputElement>() { iron, _eleWoodComb };
        List<InputElement> i2 = new List<InputElement>() { iron, _eleCoalComb };
        InputProdCheckAndAdd(new ProductInfo(P.CannonPart, i1, H.Armory));
        InputProdCheckAndAdd(new ProductInfo(P.CannonPart, i2, H.Armory));

        InputProdCheckAndAdd(new ProductInfo(P.CannonBall, i1, H.Armory));
        InputProdCheckAndAdd(new ProductInfo(P.CannonBall, i2, H.Armory));
    }

    private void Rum()
    {
        InputElement element = new InputElement(P.SugarCane, 10);
        InputElement tonelEle = new InputElement(P.Tonel, 1);

        List<InputElement> prodFormu1 = new List<InputElement>() { element, _eleWoodComb, tonelEle };
        List<InputElement> prodFormu2 = new List<InputElement>() { element, _eleCoalComb, tonelEle };
        InputProdCheckAndAdd(new ProductInfo(P.Rum, prodFormu1, H.Distillery));
        InputProdCheckAndAdd(new ProductInfo(P.Rum, prodFormu2, H.Distillery));
    }
    private void Chocolate()
    {
        InputElement elementS = new InputElement(P.Sugar, 5);
        InputElement elementC = new InputElement(P.Cacao, 5);

        List<InputElement> prodFormu1 = new List<InputElement>() { elementS, _eleWoodComb, elementC };
        List<InputElement> prodFormu2 = new List<InputElement>() { elementS, _eleCoalComb, elementC };
        InputProdCheckAndAdd(new ProductInfo(P.Chocolate, prodFormu1, H.Chocolate));
        InputProdCheckAndAdd(new ProductInfo(P.Chocolate, prodFormu2, H.Chocolate));
    }
    private void Ink()
    {
        List<InputElement> prodFormu1 = new List<InputElement>() { _eleCoalComb};
        InputProdCheckAndAdd(new ProductInfo(P.Ink, prodFormu1, H.Ink));
    }

    private void SteelFoundry()
    {
        InputElement elementI = new InputElement(P.Iron, 5);
        InputElement elementC = new InputElement(P.Coal, 5);

        List<InputElement> prodFormu1 = new List<InputElement>() { elementI, elementC, _eleWoodComb };
        List<InputElement> prodFormu2 = new List<InputElement>() {  elementI, elementC, _eleCoalComb };
        InputProdCheckAndAdd(new ProductInfo(P.Steel, prodFormu1, H.SteelFoundry));
        InputProdCheckAndAdd(new ProductInfo(P.Steel, prodFormu2, H.SteelFoundry));
    }

    private void Foundry()
    {
        //doesnt have Coal as Combustion element bz then leaves a lot of Coal in the inventory
        //and gets full 

        InputElement elementS = new InputElement(P.Ore, 5);

        List<InputElement> prodFormu1 = new List<InputElement>() { elementS, _eleWoodComb };
        //List<InputElement> prodFormu2 = new List<InputElement>() { elementS, _eleCoalComb };

        ProductInfo productInfo1 = new ProductInfo(P.RandomFoundryOutput, prodFormu1, H.Foundry);
        //ProductInfo productInfo2 = new ProductInfo(P.RandomFoundryOutput, prodFormu2, H.Foundry);

        productInfo1.AddRandomOutput(new ElementWeight(P.Iron, 1.5f));
        productInfo1.AddRandomOutput(new ElementWeight(P.Coal, 1));
        productInfo1.AddRandomOutput(new ElementWeight(P.Stone, 1));
        productInfo1.AddRandomOutput(new ElementWeight(P.Sulfur, .5f));
        productInfo1.AddRandomOutput(new ElementWeight(P.Potassium, .5f));
        productInfo1.AddRandomOutput(new ElementWeight(P.Gold, .05f));
        productInfo1.AddRandomOutput(new ElementWeight(P.Silver, 0.1f));
        productInfo1.AddRandomOutput(new ElementWeight(P.Diamond, 0.01f));

        //productInfo2.RandomWeightsOutput = productInfo1.RandomWeightsOutput;

        InputProdCheckAndAdd(productInfo1);
        //InputProdCheckAndAdd(productInfo2);
    }

    private void LoadAllGenerics()
    {


        InputProdCheckAndAdd(new ProductInfo(P.Fish, null, new List<H>() { H.FishingHut,  }));


        //animal farms
        List<H> listAnimalFarm = new List<H>() { H.AnimalFarmSmall, H.AnimalFarmMed, H.AnimalFarmLarge, H.AnimalFarmXLarge };
        //InputProdCheckAndAdd(new ProductInfo(P.Pork, null, listAnimalFarm));
        //InputProdCheckAndAdd(new ProductInfo(P.Chicken, null, listAnimalFarm));
        InputProdCheckAndAdd(new ProductInfo(P.Beef, null, listAnimalFarm));



        InputProdCheckAndAdd(new ProductInfo(P.Wood, null, H.LumberMill));
        InputProdCheckAndAdd(new ProductInfo(P.Salt, null, H.SaltMine));
       // InputProdCheckAndAdd(new ProductInfo(P.FloorTile, null, H.Tilery));
    }

    private void Mine()
    {
        ProductInfo productInfo = new ProductInfo(P.RandomMineOutput, null, H.MountainMine);
        productInfo.AddRandomOutput(new ElementWeight(P.Ore, 2));
        //productInfo.AddRandomOutput(new ElementWeight(P.Coal, 1));
        productInfo.AddRandomOutput(new ElementWeight(P.Stone, 2));

        InputProdCheckAndAdd(productInfo);
    }


    private void FieldFarm()
    {
        List<H> listH = new List<H>() { H.FieldFarmSmall, H.FieldFarmMed, H.FieldFarmLarge, H.FieldFarmXLarge };

        InputProdCheckAndAdd(new ProductInfo(P.Corn, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Bean, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Coconut, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Banana, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Potato, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.SugarCane, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Cotton, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.TobaccoLeaf, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Henequen, null, listH));

        //new
        InputProdCheckAndAdd(new ProductInfo(P.Carrot, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Tomato, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Cabbage, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Lettuce, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.SweetPotato, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Cassava, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Pineapple, null, listH));
        //InputProdCheckAndAdd(new ProductInfo(P.Mango, null, listH));
        //InputProdCheckAndAdd(new ProductInfo(P.Avocado, null, listH));
        //InputProdCheckAndAdd(new ProductInfo(P.Guava, null, listH));
        //InputProdCheckAndAdd(new ProductInfo(P.Orange, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Papaya, null, listH));

    }

   

    private void DryMeats()
    {
        //this should yeild 5 KG of meat(pork)
        InputElement salt = new InputElement(P.Salt, 0.25f);
        InputElement sugar = new InputElement(P.Sugar, 0.01f);

        List<H> listH = new List<H>(){H.AnimalFarmSmall, H.AnimalFarmMed, H.AnimalFarmLarge, H.AnimalFarmXLarge};

        List<InputElement> dryMeatFormu = new List<InputElement>() { salt, sugar};
        InputProdCheckAndAdd(new ProductInfo(P.Pork, dryMeatFormu, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Chicken, dryMeatFormu, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Beef, dryMeatFormu, listH));
    }

    //sulfur, charcoal, and potassium 
    private void GunPodwer()
    {
        InputElement sulfur = new InputElement(P.Sulfur, 1);
        InputElement coal = new InputElement(P.Coal, 1.5f);
        InputElement potassium = new InputElement(P.Potassium, 7.5f);
        List<InputElement> one = new List<InputElement>() { sulfur, coal, potassium };
        InputProdCheckAndAdd(new ProductInfo(P.GunPowder, one, H.GunPowder));
    }






    private void Clay()
    {
        InputProdCheckAndAdd(new ProductInfo(P.Clay, null, H.Clay));
    }


    private void Brick()
    {
        InputElement element = new InputElement(P.Clay, 2);
        List<InputElement> prod = new List<InputElement>() { element, _eleWoodComb };
        List<InputElement> prod2 = new List<InputElement>() { element, _eleCoalComb };
        
        InputProdCheckAndAdd(new ProductInfo(P.Brick, prod, H.Brick));
        InputProdCheckAndAdd(new ProductInfo(P.Brick, prod2, H.Brick));
        InputProdCheckAndAdd(new ProductInfo(P.RoofTile, prod, H.Brick));
        InputProdCheckAndAdd(new ProductInfo(P.RoofTile, prod2, H.Brick));
        InputProdCheckAndAdd(new ProductInfo(P.FloorTile, prod, H.Brick));
        InputProdCheckAndAdd(new ProductInfo(P.FloorTile, prod2, H.Brick));
    }   
    
    private void Ceramic()
    {
        InputElement element = new InputElement(P.Clay, 5);
        List<InputElement> prod = new List<InputElement>() { element, _eleWoodComb };
        List<InputElement> prod2 = new List<InputElement>() { element, _eleCoalComb };

        InputProdCheckAndAdd(new ProductInfo(P.Ceramic, prod, H.Ceramic));
        InputProdCheckAndAdd(new ProductInfo(P.Ceramic, prod2, H.Ceramic));
    }

    private void Tile()
    {
        //InputElement element = new InputElement(P.Ceramic, 10);
        //List<InputElement> prod = new List<InputElement>() { element };
        //InputProdCheckAndAdd(new ProductInfo(P.Tile, prod, H.Tile));
        //InputProdCheckAndAdd(new ProductInfo(P.FloorTile, null, H.Tilery));
    }


    void Carpintery()
    {
        InputElement wood = new InputElement(P.Wood, 10);
        InputElement wood2 = new InputElement(P.Wood, 20);
        InputElement iron = new InputElement(P.Iron, 1);
        InputElement iron2 = new InputElement(P.Iron, 3);



        List<InputElement> tonel = new List<InputElement>() { wood };
        List<InputElement> wheelBar = new List<InputElement>() { wood,iron };
        List<InputElement> cart = new List<InputElement>() { wood2,iron2 };


        List<InputElement> cigarBox = new List<InputElement>() { new InputElement(P.Wood, 1), new InputElement(P.Iron, 0.2f) };

        InputProdCheckAndAdd(new ProductInfo(P.Crate, tonel, H.Carpintery));
        InputProdCheckAndAdd(new ProductInfo(P.WheelBarrow, wheelBar, H.Carpintery));
        InputProdCheckAndAdd(new ProductInfo(P.Cart, cart, H.Carpintery));
        InputProdCheckAndAdd(new ProductInfo(P.Tonel, tonel, H.Carpintery));
        InputProdCheckAndAdd(new ProductInfo(P.CigarBox, cigarBox, H.Carpintery));
        InputProdCheckAndAdd(new ProductInfo(P.Bucket, cigarBox, H.Carpintery));


    }

    void BlackSmith()
    {
        InputElement wood = new InputElement(P.Wood, 10);
        InputElement iron = new InputElement(P.Iron, 10);
        
        List<InputElement> tool = new List<InputElement>() { wood, iron };

        InputProdCheckAndAdd(new ProductInfo(P.Tool, tool, H.BlackSmith));
        InputProdCheckAndAdd(new ProductInfo(P.Weapon, tool, H.BlackSmith));
        InputProdCheckAndAdd(new ProductInfo(P.Nail, tool, H.BlackSmith));
        InputProdCheckAndAdd(new ProductInfo(P.Utensil, tool, H.BlackSmith));




        //InputProdCheckAndAdd(new ProductInfo(P.Axe, tool, H.BlackSmith));
        //InputProdCheckAndAdd(new ProductInfo(P.Sword, tool, H.BlackSmith));
    }


    private void Cigar()
    {
        //H.Cigar
        InputElement element = new InputElement(P.TobaccoLeaf, 15);
        InputElement cigarBox = new InputElement(P.CigarBox, 2);
        
        List<InputElement> prodFormu1 = new List<InputElement>() { element,cigarBox, _eleWoodComb };
        List<InputElement> prodFormu2 = new List<InputElement>() { element,cigarBox, _eleCoalComb};
        
        InputProdCheckAndAdd(new ProductInfo(P.Cigar, prodFormu1, H.Cigars));
        InputProdCheckAndAdd(new ProductInfo(P.Cigar, prodFormu2, H.Cigars));
    }


    private void Tailor()
    {
        InputElement wool = new InputElement(P.Wool, 5);
        List<InputElement> cloth = new List<InputElement>() { wool };
        InputProdCheckAndAdd(new ProductInfo(P.Cloth, cloth, H.Tailor));

        InputElement fabric = new InputElement(P.Fabric, 5);
        List<InputElement> sail = new List<InputElement>() { fabric };
        InputProdCheckAndAdd(new ProductInfo(P.Sail, sail, H.Tailor));

        InputElement leather = new InputElement(P.Leather, 5);
        List<InputElement> shoe = new List<InputElement>() { leather };
        InputProdCheckAndAdd(new ProductInfo(P.Shoe, shoe, H.Tailor));
    }





    private void Fabric()
    {
        InputElement cotton = new InputElement(P.Cotton, 10);
        InputElement henequen = new InputElement(P.Henequen, 10);
        
        List<InputElement> cloth1 = new List<InputElement>() { cotton, _eleWoodComb };
        List<InputElement> cloth2 = new List<InputElement>() { cotton, _eleCoalComb };

        List<InputElement> string1 = new List<InputElement>() { henequen, _eleWoodComb };
        List<InputElement> string2 = new List<InputElement>() { henequen, _eleCoalComb };


        InputProdCheckAndAdd(new ProductInfo(P.Fabric, cloth1, H.Cloth ));
        InputProdCheckAndAdd(new ProductInfo(P.Fabric, cloth2, H.Cloth));

        InputProdCheckAndAdd(new ProductInfo(P.String, string1, H.Cloth));
        InputProdCheckAndAdd(new ProductInfo(P.String, string2, H.Cloth));
    }


    /// <summary>
    /// My Secret Weapon
    /// 
    /// Will have to have a lot of Req to be able to build
    /// </summary>
    private void Coins()
    {
        InputElement go = new InputElement(P.Gold, 1);
        InputElement si = new InputElement(P.Silver, 1);

        InputElement go2 = new InputElement(P.Gold, 0.5f);

        List<InputElement> prodForm1 = new List<InputElement>() { si, _eleCoalComb };
        List<InputElement> prodForm2 = new List<InputElement>() { si, go2, _eleCoalComb };
        List<InputElement> prodForm3 = new List<InputElement>() { go, _eleCoalComb };

        InputProdCheckAndAdd(new ProductInfo(P.Coin, prodForm3, H.CoinStamp));
    }

    private void PaperNewsAndBook()
    {
        //good sample Paper:
        //paper
        InputElement eleInputWood = new InputElement(P.Wood, 10);
        InputElement eleWoodComb = new InputElement(P.Wood, 2);
        InputElement eleCoalComb = new InputElement(P.Coal, 1);

        //1 formula that do paper
        List<InputElement> formula1 = new List<InputElement>() { eleInputWood, eleWoodComb };
        //2nd forumala that do paper 
        List<InputElement> formula2 = new List<InputElement>() { eleInputWood, eleCoalComb };
        InputProdCheckAndAdd(new ProductInfo(P.Paper, formula1, H.Paper_Mill));
        InputProdCheckAndAdd(new ProductInfo(P.Paper, formula2, H.Paper_Mill));


        //paper news
        InputElement paper1 = new InputElement(P.Paper, 10);
        List<InputElement> paperForm1 = new List<InputElement>() { paper1, eleWoodComb};
        List<InputElement> paperForm2 = new List<InputElement>() { paper1, eleCoalComb};
        InputProdCheckAndAdd(new ProductInfo(P.Map, paperForm1, H.Printer));
        InputProdCheckAndAdd(new ProductInfo(P.Map, paperForm2, H.Printer));


        //books
        InputElement paper2 = new InputElement(P.Paper, 100);
        InputElement leather = new InputElement(P.Leather, 1);
        List<InputElement> bookForm1 = new List<InputElement>() { paper2, leather, eleWoodComb };
        List<InputElement> bookForm2 = new List<InputElement>() { paper2, leather, eleCoalComb };
        InputProdCheckAndAdd(new ProductInfo(P.Book, bookForm1, H.Printer));
        InputProdCheckAndAdd(new ProductInfo(P.Book, bookForm2, H.Printer));
    }



    private void Sugar()
    {
        InputElement element = new InputElement(P.SugarCane, 10);
        List<InputElement> prodFormu1 = new List<InputElement>() { element, _eleWoodComb };
        List<InputElement> prodFormu2 = new List<InputElement>() { element, _eleCoalComb };
        InputProdCheckAndAdd(new ProductInfo(P.Sugar, prodFormu1, H.SugarMill));
        InputProdCheckAndAdd(new ProductInfo(P.Sugar, prodFormu2, H.SugarMill));
    }




    ///Functions

    ///// <summary>
    ///// Will tell u wich product take from an Inventoryy to take to ur current factory/work
    ///// </summary>
    ///// <param name="inventory"></param>
    ///// <param name="itemProducing"></param>
    ///// <returns></returns>
    //public P WhichProdIShouldTake(Person person)
    //{
    //    var invList = person.FoodSource.Inventory.InventItems;
    //    var inputsNeeded = person.Work.OrderedListOfInputNeeded();

    //    //Will loop first one by one thru the inputs needed to see if I can find the most needed
    //    //in the inventory and from there will keep looping to the least needed
    //    for (int i = 0; i < inputsNeeded.Count; i++)
    //    {
    //        //will loop thru all the invList looking for the inputsNeeded
    //        for (int j = 0; j < invList.Count; j++)
    //        {
    //            if (inputsNeeded[i] == invList[i].Key)
    //            {
    //                return inputsNeeded[i];
    //            }
    //        }
    //    }
   
    //    //if none is found then nothing should be taken from this FoodSrc to Work
    //    return P.None;
    //}

    /// <summary>
    /// Will tell u how many ingredients this building will take maximun
    /// needs to split the storage area of an Building so storage is not fillOut
    /// only with one ingredient(input)
    /// </summary>
    /// <param name="hTypeP"></param>
    /// <returns></returns>
    int ReturnAllInputsThisBuildingTake(H typeKey)
    {
        var prods = _inputProducts.Where(a => a.HType.Contains(typeKey)).ToList();
        return prods.Sum(t => t.Ingredients.Count);
    }

    /// <summary>
    /// Will return a list of all the prodcuts this building can use as an input
    /// </summary>
    /// <param name="typeKey"></param>
    /// <returns></returns>
    public List<P> ReturnAllInputsThisBuildingTakeListOfProd(H typeKey)
    {
        List<P> res = new List<P>();
        var prods = _inputProducts.Where(a => a.HType.Contains(typeKey)).ToList();
        for (int i = 0; i < prods.Count; i++)
        {
            //res.Add(prods[i].Product);
            res.AddRange(ReturnAllIngridients(prods[i]));
        }
        return res;
    }

    private List<P> ReturnAllIngridients(ProductInfo productInfo)
    {
        List<P> res = new List<P>();

        if (productInfo.Ingredients==null)
        {
            return res;
        }

        for (int i = 0; i < productInfo.Ingredients.Count; i++)
        {
            res.Add(productInfo.Ingredients[i].Element);
        }
        return res;
    }



    /// <summary>
    /// Given a Inventory and HType will tell u which products are not input
    /// </summary>
    /// <param name="inv"></param>
    /// <param name="hType"></param>
    /// <returns></returns>
    public List<P> ReturnProductsOnInvThatAreNotInput(Inventory inv, H hType)
    {
        List<P> res = new List<P>();
        var inputs = ReturnAllInputsThisBuildingTakeListOfProd(hType);

        for (int i = 0; i < inv.InventItems.Count; i++)
        {
            //if nnot imput like Brick. just can add all items inventory to res 
            if (inputs.Count == 0)
            {
                res.Add(inv.InventItems[i].Key);
            }

            int notEqual = 0;
            for (int j = 0; j < inputs.Count; j++)
            {
                if (inv.InventItems[i].Key != inputs[j])
                {
                    notEqual++;
                }
            }
            //if all inputs are not equal to inv.InventItems[i].Key then that is a product produced in that building 
            if (notEqual == inputs.Count)
            {
                res.Add(inv.InventItems[i].Key);
            }
        }
        return res.Distinct().ToList();
    }

    /// <summary>
    /// bz if is an input will get half of the storage divided by the amount of ingredients
    /// for ex for Axe: the input is wood and iron.
    /// 
    /// If axe is pass will return a 2 so half of the storage
    /// if wood is pass will return a 4 so a quater of the storage
    /// if iron is pass will return a 4 ...
    /// </summary>
    /// <param name="hTypeP"></param>
    /// <param name="prod"></param>
    /// <returns></returns>
    public int ReturnPartOfStorageThatBelongsToThisProd(H hTypeP, P prod)
    {
        var inputsAmt = ReturnAllInputsThisBuildingTake(hTypeP);

        //a building without inputs then 
        if (inputsAmt == 0)
        {
            //the whole storage
            //ex: Dock, Wood, SaltMine
            return 1;
        }
        return inputsAmt*2;
    }



    /// <summary>
    /// Given a product will return the list of ingrediets needed 
    /// </summary>
    /// <param name="product"></param>
    /// <returns></returns>
    public List<InputElement> ReturnIngredients(P product)
    {
        for (int i = 0; i < _inputProducts.Count; i++)
        {
            if (product == _inputProducts[i].Product)
            {
                return _inputProducts[i].Ingredients;
            }
        }
        return null;
    }

    /// <summary>
    /// Will tell u if param 'building' has enough on inventory to produce a Unit of its current product
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    public bool DoIHaveEnoughOnInvToProdThis(Building building)
    {
        var ingredients = ReturnIngredients(building.CurrentProd.Product);

        //this is for buildings tht Produce and dont need any input 
        if (ingredients == null)
        {
            return true;
        }

        int covered = 0;

        for (int i = 0; i < ingredients.Count; i++)
        {
            if (building.Inventory.IsHasEnoughToCoverThisIngredient(ingredients[i]))
            {
                covered++;
            }
        }

        if (covered == ingredients.Count)
        {
            return true;
        }
        return false;
    }  
    
    public bool DoIHaveEnoughOnInvToProdThis(Building building, P prod)
    {
        var ingredients = ReturnIngredients(prod);

        //this is for buildings tht Produce and dont need any input 
        if (ingredients == null)
        {
            return true;
        }

        int covered = 0;

        for (int i = 0; i < ingredients.Count; i++)
        {
            if (building.Inventory.IsHasEnoughToCoverThisIngredient(ingredients[i]))
            {
                covered++;
            }
        }

        if (covered == ingredients.Count)
        {
            return true;
        }
        return false;
    }

#endregion
}
