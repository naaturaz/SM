using System.Collections.Generic;
using System.Linq;

public class Production  {

    Dictionary<List<H>, List<P>> _products = new Dictionary<List<H>, List<P>>();
    Food _food = new Food();

    public Production()
    {
        LoadProducts();
        LoadInputProductsStats();
    }

    public Food Food1
    {
        get { return _food; }
        set { _food = value; }
    }


    /// <summary>
    /// Loads all the products and their buildings 
    /// </summary>
    void LoadProducts()
    {
        //_products.Add(new List<H>(){H.Farm}, new List<P>(){P.Bean, P.Potato, P.SugarCane, P.Corn});

        _products.Add(new List<H>() { H.AnimalFarmSmall, H.AnimalFarmMed, H.AnimalFarmLarge, H.AnimalFarmXLarge }, 
            new List<P>() { P.Beef, P.Chicken, P.Egg, P.Pork });

        _products.Add(new List<H>() { H.FieldFarmSmall, H.FieldFarmMed, H.FieldFarmLarge, H.FieldFarmXLarge },
            new List<P>()
            {
                
                P.Corn,
                P.Bean,
                P.Coconut,
                P.Banana,
                 
                
                 P.Potato,  P.SugarCane, P.Cotton, P.Tobacco,
                 P.Henequen
                 
            });

        _products.Add(new List<H>() { H.Clay}, new List<P>() { P.Clay});

        _products.Add(new List<H>() { H.FishSmall, H.FishRegular }, new List<P>() { P.Fish });

        _products.Add(new List<H>() { H.Mine, H.MountainMine }, new List<P>() { P.Gold });

        _products.Add(new List<H>() { H.Resin }, new List<P>() { P.Resin });

        _products.Add(new List<H>() { H.Wood }, new List<P>() { P.Wood });

        _products.Add(new List<H>() { H.BlackSmith }, new List<P>() { P.Axe, P.Tool, P.Sword });

        _products.Add(new List<H>() { H.SaltMine }, new List<P>() { P.Salt});

        _products.Add(new List<H>() { H.Brick }, new List<P>() { P.Brick });

        _products.Add(new List<H>() { H.Carpintery }, new List<P>() { P.Tonel });

        _products.Add(new List<H>() { H.Cigars }, new List<P>() { P.Cigar });

        _products.Add(new List<H>() { H.Slat }, new List<P>() { P.Slat });

        _products.Add(new List<H>() { H.Tilery }, new List<P>() { P.Tile });

        _products.Add(new List<H>() { H.Cloth }, new List<P>() { P.Fabric });

        _products.Add(new List<H>() { H.GunPowder }, new List<P>() { P.GunPowder });

        _products.Add(new List<H>() { H.Paper }, new List<P>() { P.Paper });

        _products.Add(new List<H>() { H.PrinterBig, H.PrinterSmall }, new List<P>() { P.Books, P.PaperNews });

        _products.Add(new List<H>() { H.Silk }, new List<P>() { P.Silk });

        _products.Add(new List<H>() { H.SugarMill }, new List<P>() { P.Sugar });
    }

    /// <summary>
    /// Which will be the first element of the List 
    /// </summary>
    /// <param name="typeKey"></param>
    /// <returns></returns>
    public P ReturnDefaultProd(H typeKey)
    {
        for (int i = 0; i < _products.Count; i++)
        {
            var ele = _products.ElementAt(i);

            for (int j = 0; j < ele.Key.Count; j++)
            {
                var key = ele.Key[j];
                if (key == typeKey)
                {
                    //will return the first value om the values List
                    return ele.Value[0];
                }
            }
        }
        return P.None;
    }




    #region Inputs for Create a Product

    //contains which products are needed in the input to generate 1 unit of the final product
    List<ProductInfo> _inputProducts = new List<ProductInfo>();

    InputElement _eleWoodComb = new InputElement(P.Wood, 2);
    InputElement _eleCoalComb = new InputElement(P.Coal, 1);

    private void LoadInputProductsStats()
    {
        //Loading each prodcut input 
        Meats();

        GunPodwer();
        
        Axe();
        Sword();

        Brick();

        Tonel();

        Cigar();

        Slat();

        Tile();

        Fabric();

        PaperNewsAndBook();

        Sugar();

        
    }

    private void Meats()
    {
        //this should yeild 5 KG of meat(pork)
        InputElement salt = new InputElement(P.Salt, 0.25f);
        InputElement sugar = new InputElement(P.Sugar, 0.01f);


        List<InputElement> dryMeatFormu = new List<InputElement>() { salt, sugar};
        _inputProducts.Add(new ProductInfo(P.Pork, dryMeatFormu));
        _inputProducts.Add(new ProductInfo(P.Chicken, dryMeatFormu));
        _inputProducts.Add(new ProductInfo(P.Beef, dryMeatFormu));
    }

    //sulfur, charcoal, and potassium 
    private void GunPodwer()
    {
        InputElement sulfur = new InputElement(P.Sulfur, 1);
        InputElement coal = new InputElement(P.Coal, 1.5f);
        InputElement potassium = new InputElement(P.Potassium, 7.5f);
        List<InputElement> one = new List<InputElement>() { sulfur, coal, potassium };
        _inputProducts.Add(new ProductInfo(P.GunPowder, one));
    }



    void Axe()
    {
        //axe
        InputElement iron = new InputElement(P.Iron, 10);
        InputElement wood = new InputElement(P.Wood, 10);
        List<InputElement> axe = new List<InputElement>() { iron, wood };
        _inputProducts.Add(new ProductInfo(P.Axe, axe));
    }


    private void Sword()
    {
        InputElement iron = new InputElement(P.Iron, 15);
        List<InputElement> sword = new List<InputElement>() { iron };
        _inputProducts.Add(new ProductInfo(P.Sword, sword));
    }


    private void Brick()
    {
        InputElement element = new InputElement(P.Clay, 5);
        List<InputElement> prod = new List<InputElement>() { element };
        _inputProducts.Add(new ProductInfo(P.Brick, prod));
    }


    void Tonel()
    {
        //tonels
        InputElement wood = new InputElement(P.Wood, 10);
        List<InputElement> axe = new List<InputElement>() { wood };
        _inputProducts.Add(new ProductInfo(P.Axe, axe));
    }


    private void Cigar()
    {
        InputElement element = new InputElement(P.Tobacco, 15);
        List<InputElement> prodFormu1 = new List<InputElement>() { element, _eleWoodComb };
        List<InputElement> prodFormu2 = new List<InputElement>() { element, _eleCoalComb};
        _inputProducts.Add(new ProductInfo(P.Cigar, prodFormu1));
        _inputProducts.Add(new ProductInfo(P.Cigar, prodFormu2));
    }


    private void Slat()
    {
        InputElement element = new InputElement(P.Wood, 5);
        List<InputElement> prod = new List<InputElement>() { element };
        _inputProducts.Add(new ProductInfo(P.Slat, prod));
    }


    private void Tile()
    {
        InputElement element = new InputElement(P.Clay, 10);
        List<InputElement> prod = new List<InputElement>() { element };
        _inputProducts.Add(new ProductInfo(P.Tile, prod));
    }


    private void Fabric()
    {
        InputElement element = new InputElement(P.Cotton, 10);
        List<InputElement> prodForm1 = new List<InputElement>() { element, _eleWoodComb };
        List<InputElement> prodForm2 = new List<InputElement>() { element, _eleCoalComb };
        _inputProducts.Add(new ProductInfo(P.Fabric, prodForm1));
        _inputProducts.Add(new ProductInfo(P.Fabric, prodForm2));
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
        _inputProducts.Add(new ProductInfo(P.Paper, formula1));
        _inputProducts.Add(new ProductInfo(P.Paper, formula2));


        //paper news
        InputElement paper1 = new InputElement(P.Paper, 10);
        List<InputElement> paperForm1 = new List<InputElement>() { paper1, eleWoodComb};
        List<InputElement> paperForm2 = new List<InputElement>() { paper1, eleCoalComb};
        _inputProducts.Add(new ProductInfo(P.PaperNews, paperForm1));
        _inputProducts.Add(new ProductInfo(P.PaperNews, paperForm2));


        //books
        InputElement paper2 = new InputElement(P.Paper, 100);
        InputElement leather = new InputElement(P.Leather, 1);
        List<InputElement> bookForm1 = new List<InputElement>() { paper2, leather, eleWoodComb };
        List<InputElement> bookForm2 = new List<InputElement>() { paper2, leather, eleCoalComb };
        _inputProducts.Add(new ProductInfo(P.Books, bookForm1));
        _inputProducts.Add(new ProductInfo(P.Books, bookForm2));
    }



    private void Sugar()
    {
        InputElement element = new InputElement(P.SugarCane, 10);
        List<InputElement> prodFormu1 = new List<InputElement>() { element, _eleWoodComb };
        List<InputElement> prodFormu2 = new List<InputElement>() { element, _eleCoalComb };
        _inputProducts.Add(new ProductInfo(P.Sugar, prodFormu1));
        _inputProducts.Add(new ProductInfo(P.Sugar, prodFormu2));
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
    int ReturnAllInputsThisBuildingTake(H hTypeP)
    {
        //products that could be created in that vbuilding
        var products = _products.Where(a => a.Key.Contains(hTypeP)).ToList();
        int amt=0;
        List<InputElement> allInputIngredients = new List<InputElement>();

        for (int i = 0; i < products.Count(); i++)
        {
            for (int j = 0; j < products[i].Value.Count; j++)
            {
                var prodKey = products[i].Value[j];

                //the inputs for 1 product. 1 product could have many inputs like Axe
                var inputs = _inputProducts.Where(a => a.Product == prodKey).ToList();

                for (int k = 0; k < inputs.Count; k++)
                {
                    allInputIngredients.AddRange(inputs[k].Ingredients);
                }
            }
        }
        amt = ReturnTheAmountOfUniqueProducts(allInputIngredients);

        return amt;
    }

    int ReturnTheAmountOfUniqueProducts(List<InputElement> list)
    {
        List<P> prod = new List<P>();

        for (int i = 0; i < list.Count; i++)
        {
            prod.Add(list[i].Element);
        }

        return prod.Distinct().ToList().Count;
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

        //prod we create in this buildng 
        var products = _products.Where(a => a.Key.Contains(hTypeP)).ToList();
        var prodFound = products.Find(a => a.Value.Contains(prod));

        //the prod sent is something we manufactur un that place then can take half of building 
        if (prodFound.Value != null && prodFound.Value.Count > 0)
        {
            return 2;
        }

        //else is just an ingredint 
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
        var ingredients = ReturnIngredients(building.CurrentProd);

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



    #region GUI this 
    //is the Region tht will gather how much resources we have thur all the Storage and Will be called to be shown on GUI



    public void UpdateNumbers()
    {
        
    }



#endregion

}
