﻿using System.Collections.Generic;
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
        
        Debug.Log("Prod not found: ReturnExactProduct(). pls check");
        return null;
    }



    #region Inputs for Create a Product

    //contains which products are needed in the input to generate 1 unit of the final product
    List<ProductInfo> _inputProducts = new List<ProductInfo>();

    InputElement _eleWoodComb = new InputElement(P.Wood, 2);
    InputElement _eleCoalComb = new InputElement(P.Coal, 1);

    private void LoadInputProductsStats()
    {
        //Loading each prodcut input
        LoadAllGenerics();
        FieldFarm();

        Rum();
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

    void InputProdCheckAndAdd(ProductInfo pInfo)
    {
        _inputProducts.Add(pInfo);
    }

    //todo replace mine as Foundry, silk
    //todo check Resin
    private void LoadAllGenerics()
    {
        InputProdCheckAndAdd(new ProductInfo(P.Ceramic, null, H.Ceramic));

        InputProdCheckAndAdd(new ProductInfo(P.Fish, null, new List<H>() { H.FishSmall, H.FishRegular }));

        InputProdCheckAndAdd(new ProductInfo(P.Ore, null, H.MountainMine));
        InputProdCheckAndAdd(new ProductInfo(P.Coal, null, H.MountainMine));
        InputProdCheckAndAdd(new ProductInfo(P.Stone, null, H.MountainMine));

        InputProdCheckAndAdd(new ProductInfo(P.Wood, null, H.Wood));
        InputProdCheckAndAdd(new ProductInfo(P.Salt, null, H.SaltMine));
        InputProdCheckAndAdd(new ProductInfo(P.Tile, null, H.Tilery));
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
        InputElement element = new InputElement(P.Ceramic, 5);
        List<InputElement> prod = new List<InputElement>() { element };
        InputProdCheckAndAdd(new ProductInfo(P.Brick, prod, H.Brick));
    }


    void Tonel()
    {
        //tonels
        InputElement wood = new InputElement(P.Wood, 10);
        List<InputElement> tonel = new List<InputElement>() { wood };
        InputProdCheckAndAdd(new ProductInfo(P.Tonel, tonel, H.Carpintery));
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
        InputElement element = new InputElement(P.Ceramic, 10);
        List<InputElement> prod = new List<InputElement>() { element };
        InputProdCheckAndAdd(new ProductInfo(P.Tile, prod, H.Tile));
    }


    private void Fabric()
    {
        InputElement element = new InputElement(P.Cotton, 10);
        List<InputElement> prodForm1 = new List<InputElement>() { element, _eleWoodComb };
        List<InputElement> prodForm2 = new List<InputElement>() { element, _eleCoalComb };
        InputProdCheckAndAdd(new ProductInfo(P.Fabric, prodForm1, H.Cloth ));
        InputProdCheckAndAdd(new ProductInfo(P.Fabric, prodForm2, H.Cloth));
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
        InputProdCheckAndAdd(new ProductInfo(P.PaperNews, paperForm1, H.PrinterSmall));
        InputProdCheckAndAdd(new ProductInfo(P.PaperNews, paperForm2, H.PrinterSmall));


        //books
        InputElement paper2 = new InputElement(P.Paper, 100);
        InputElement leather = new InputElement(P.Leather, 1);
        List<InputElement> bookForm1 = new List<InputElement>() { paper2, leather, eleWoodComb };
        List<InputElement> bookForm2 = new List<InputElement>() { paper2, leather, eleCoalComb };
        InputProdCheckAndAdd(new ProductInfo(P.Books, bookForm1, H.PrinterSmall));
        InputProdCheckAndAdd(new ProductInfo(P.Books, bookForm2, H.PrinterSmall));
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



    #region GUI this 
    //is the Region tht will gather how much resources we have thur all the Storage and Will be called to be shown on GUI



    public void UpdateNumbers()
    {
        
    }



#endregion

}
