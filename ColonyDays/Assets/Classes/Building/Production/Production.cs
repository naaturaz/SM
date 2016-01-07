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


    private void LoadInputProductsStats()
    {
        //Loading each prodcut input 
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
        List<InputElement> prod = new List<InputElement>() { element };
        _inputProducts.Add(new ProductInfo(P.Cigar, prod));
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
        List<InputElement> prod = new List<InputElement>() { element };
        _inputProducts.Add(new ProductInfo(P.Fabric, prod));
    }




    private void PaperNewsAndBook()
    {
        //paper
        InputElement ele = new InputElement(P.Wood, 10);
        List<InputElement> inp = new List<InputElement>() { ele };
        _inputProducts.Add(new ProductInfo(P.Paper, inp));


        //paper news
        InputElement paper1 = new InputElement(P.Paper, 10);
        List<InputElement> paper = new List<InputElement>() { paper1 };
        _inputProducts.Add(new ProductInfo(P.PaperNews, paper));

        //books
        InputElement paper2 = new InputElement(P.Paper, 100);
        List<InputElement> book = new List<InputElement>() { paper2 };
        _inputProducts.Add(new ProductInfo(P.Books, book));
    }



    private void Sugar()
    {
        InputElement element = new InputElement(P.SugarCane, 10);
        List<InputElement> prod = new List<InputElement>() { element };
        _inputProducts.Add(new ProductInfo(P.Sugar, prod));
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
