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

    InputElement _eleWoodComb = new InputElement(P.Wood, 2);
    InputElement _eleCoalComb = new InputElement(P.Coal, 1);


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
        
        Axe();
        Sword();

        Brick();

        Tonel();

        Cigar();

        Slat();

        Tile();

        Fabric();

        Coins();
        PaperNewsAndBook();
        Sugar();
    }



    private void CannonParts()
    {
        InputElement iron = new InputElement(P.Iron, 10);
        List<InputElement> i1 = new List<InputElement>() { iron, _eleWoodComb };
        List<InputElement> i2 = new List<InputElement>() { iron, _eleCoalComb };
        InputProdCheckAndAdd(new ProductInfo(P.CannonPart, i1, H.CannonParts));
        InputProdCheckAndAdd(new ProductInfo(P.CannonPart, i2, H.CannonParts));

        InputProdCheckAndAdd(new ProductInfo(P.CannonBall, i1, H.CannonParts));
        InputProdCheckAndAdd(new ProductInfo(P.CannonBall, i2, H.CannonParts));
    }

    private void Rum()
    {
        InputElement element = new InputElement(P.SugarCane, 10);
        InputElement tonelEle = new InputElement(P.Tonel, 1);

        List<InputElement> prodFormu1 = new List<InputElement>() { element, _eleWoodComb, tonelEle };
        List<InputElement> prodFormu2 = new List<InputElement>() { element, _eleCoalComb, tonelEle };
        InputProdCheckAndAdd(new ProductInfo(P.Rum, prodFormu1, H.Rum));
        InputProdCheckAndAdd(new ProductInfo(P.Rum, prodFormu2, H.Rum));
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
        InputElement elementS = new InputElement(P.Ore, 5);

        List<InputElement> prodFormu1 = new List<InputElement>() { elementS, _eleWoodComb };
        List<InputElement> prodFormu2 = new List<InputElement>() { elementS, _eleCoalComb };

        ProductInfo productInfo1 = new ProductInfo(P.RandomFoundryOutput, prodFormu1, H.MountainMine);
        ProductInfo productInfo2 = new ProductInfo(P.RandomFoundryOutput, prodFormu2, H.MountainMine);

        productInfo1.AddRandomOutput(new ElementWeight(P.Iron, 2));
        productInfo1.AddRandomOutput(new ElementWeight(P.Coal, 1));
        productInfo1.AddRandomOutput(new ElementWeight(P.Stone, 1));
        productInfo1.AddRandomOutput(new ElementWeight(P.Sulfur, .5f));
        productInfo1.AddRandomOutput(new ElementWeight(P.Gold, .05f));
        productInfo1.AddRandomOutput(new ElementWeight(P.Silver, 0.1f));
        productInfo1.AddRandomOutput(new ElementWeight(P.Diamond, 0.01f));

        productInfo2.RandomWeightsOutput = productInfo1.RandomWeightsOutput;

        InputProdCheckAndAdd(productInfo1);
        InputProdCheckAndAdd(productInfo2);
    }

    private void LoadAllGenerics()
    {
        InputProdCheckAndAdd(new ProductInfo(P.Ceramic, null, H.Ceramic));

        InputProdCheckAndAdd(new ProductInfo(P.Fish, null, new List<H>() { H.FishSmall, H.FishRegular }));


        //animal farms
        List<H> listAnimalFarm = new List<H>() { H.AnimalFarmSmall, H.AnimalFarmMed, H.AnimalFarmLarge, H.AnimalFarmXLarge };
        InputProdCheckAndAdd(new ProductInfo(P.Pork, null, listAnimalFarm));
        InputProdCheckAndAdd(new ProductInfo(P.Chicken, null, listAnimalFarm));
        InputProdCheckAndAdd(new ProductInfo(P.Beef, null, listAnimalFarm));



        InputProdCheckAndAdd(new ProductInfo(P.Wood, null, H.Wood));
        InputProdCheckAndAdd(new ProductInfo(P.Salt, null, H.SaltMine));
        InputProdCheckAndAdd(new ProductInfo(P.Tile, null, H.Tilery));
    }

    private void Mine()
    {
        //this should yeild 5 KG of meat(pork)
        ProductInfo productInfo = new ProductInfo(P.RandomMineOutput, null, H.MountainMine);
        productInfo.AddRandomOutput(new ElementWeight(P.Ore, 2));
        productInfo.AddRandomOutput(new ElementWeight(P.Coal, 1));
        productInfo.AddRandomOutput(new ElementWeight(P.Stone, 1));

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
        InputProdCheckAndAdd(new ProductInfo(P.Yucca, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Pineapple, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Mango, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Avocado, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Guava, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Orange, null, listH));
        InputProdCheckAndAdd(new ProductInfo(P.Papaya, null, listH));

    }

   

    private void Meats()
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



    void Axe()
    {
        //axe
        InputElement iron = new InputElement(P.Iron, 10);
        InputElement wood = new InputElement(P.Wood, 10);
        List<InputElement> axe = new List<InputElement>() { iron, wood };
        InputProdCheckAndAdd(new ProductInfo(P.Axe, axe, H.BlackSmith));
    }


    private void Sword()
    {
        InputElement iron = new InputElement(P.Iron, 15);
        List<InputElement> sword = new List<InputElement>() { iron };
        InputProdCheckAndAdd(new ProductInfo(P.Sword, sword, H.BlackSmith));
    }


    private void Brick()
    {
        //InputElement element = new InputElement(P.Ceramic, 5);
        //List<InputElement> prod = new List<InputElement>() { element };
        //InputProdCheckAndAdd(new ProductInfo(P.Brick, prod, H.Brick));
        InputProdCheckAndAdd(new ProductInfo(P.Brick, null, H.Brick));
    }


    void Tonel()
    {
        //tonels
        InputElement wood = new InputElement(P.Wood, 10);
        List<InputElement> tonel = new List<InputElement>() { wood };
        InputProdCheckAndAdd(new ProductInfo(P.Tonel, tonel, H.Carpintery));
        InputProdCheckAndAdd(new ProductInfo(P.Crate, tonel, H.Carpintery));
    }


    private void Cigar()
    {
        //H.Cigar
        InputElement element = new InputElement(P.TobaccoLeaf, 15);
        List<InputElement> prodFormu1 = new List<InputElement>() { element, _eleWoodComb };
        List<InputElement> prodFormu2 = new List<InputElement>() { element, _eleCoalComb};
        InputProdCheckAndAdd(new ProductInfo(P.Cigar, prodFormu1, H.Cigars));
        InputProdCheckAndAdd(new ProductInfo(P.Cigar, prodFormu2, H.Cigars));
    }


    private void Slat()
    {
        InputElement element = new InputElement(P.Wood, 5);
        List<InputElement> prod = new List<InputElement>() { element };
        InputProdCheckAndAdd(new ProductInfo(P.Slat, prod, H.Slat));
    }


    private void Tile()
    {
        //InputElement element = new InputElement(P.Ceramic, 10);
        //List<InputElement> prod = new List<InputElement>() { element };
        //InputProdCheckAndAdd(new ProductInfo(P.Tile, prod, H.Tile));
        InputProdCheckAndAdd(new ProductInfo(P.Tile, null, H.Tilery));
    }


    private void Fabric()
    {
        InputElement element = new InputElement(P.Cotton, 10);
        List<InputElement> prodForm1 = new List<InputElement>() { element, _eleWoodComb };
        List<InputElement> prodForm2 = new List<InputElement>() { element, _eleCoalComb };
        InputProdCheckAndAdd(new ProductInfo(P.Fabric, prodForm1, H.Cloth ));
        InputProdCheckAndAdd(new ProductInfo(P.Fabric, prodForm2, H.Cloth));
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

        InputProdCheckAndAdd(new ProductInfo(P.CrystalCoin, prodForm1, H.CoinStamp));
        InputProdCheckAndAdd(new ProductInfo(P.CaribbeanCoin, prodForm2, H.CoinStamp));
        InputProdCheckAndAdd(new ProductInfo(P.SugarCoin, prodForm3, H.CoinStamp));
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
        InputProdCheckAndAdd(new ProductInfo(P.Paper, formula1, H.Paper));
        InputProdCheckAndAdd(new ProductInfo(P.Paper, formula2, H.Paper));


        //paper news
        InputElement paper1 = new InputElement(P.Paper, 10);
        List<InputElement> paperForm1 = new List<InputElement>() { paper1, eleWoodComb};
        List<InputElement> paperForm2 = new List<InputElement>() { paper1, eleCoalComb};
        InputProdCheckAndAdd(new ProductInfo(P.PaperNews, paperForm1, H.Printer));
        InputProdCheckAndAdd(new ProductInfo(P.PaperNews, paperForm2, H.Printer));


        //books
        InputElement paper2 = new InputElement(P.Paper, 100);
        InputElement leather = new InputElement(P.Leather, 1);
        List<InputElement> bookForm1 = new List<InputElement>() { paper2, leather, eleWoodComb };
        List<InputElement> bookForm2 = new List<InputElement>() { paper2, leather, eleCoalComb };
        InputProdCheckAndAdd(new ProductInfo(P.Books, bookForm1, H.Printer));
        InputProdCheckAndAdd(new ProductInfo(P.Books, bookForm2, H.Printer));
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

    /// <summary>
    /// Will tell u wich product take from an Inventoryy to take to ur current factory/work
    /// </summary>
    /// <param name="inventory"></param>
    /// <param name="itemProducing"></param>
    /// <returns></returns>
    public P WhichProdIShouldTake(Person person)
    {
        var invList = person.FoodSource.Inventory.InventItems;
        var inputsNeeded = person.Work.OrderedListOfInputNeeded();

        //Will loop first one by one thru the inputs needed to see if I can find the most needed
        //in the inventory and from there will keep looping to the least needed
        for (int i = 0; i < inputsNeeded.Count; i++)
        {
            //will loop thru all the invList looking for the inputsNeeded
            for (int j = 0; j < invList.Count; j++)
            {
                if (inputsNeeded[i] == invList[i].Key)
                {
                    return inputsNeeded[i];
                }
            }
        }
   
        //if none is found then nothing should be taken from this FoodSrc to Work
        return P.None;
    }

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

    List<P> ReturnAllInputsThisBuildingTakeListOfProd(H typeKey)
    {
        List<P> res = new List<P>();
        var prods = _inputProducts.Where(a => a.HType.Contains(typeKey)).ToList();
        for (int i = 0; i < prods.Count; i++)
        {
            res.Add(prods[i].Product);
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
            for (int j = 0; j < inputs.Count; j++)
            {
                if (inv.InventItems[i].Key != inputs[j])
                {
                    res.Add(inv.InventItems[i].Key);
                }
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
