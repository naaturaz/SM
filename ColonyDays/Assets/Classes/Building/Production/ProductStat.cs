using System.Collections.Generic;

public class ProductInfo
{
    private P _product;
    List<InputElement> _ingredients = new List<InputElement>();

    public ProductInfo(P product, List<InputElement> ingredients)
    {
        _product = product;
        _ingredients = ingredients;
    }

    public ProductInfo(P product, InputElement ingredient)
    {
        _product = product;
        _ingredients.Add(ingredient);        
    }

    public ProductInfo()
    {
    }

    public ProductInfo(P finalProd)
    {
        _product = finalProd;
    }

    public P Product
    {
        get { return _product; }
        set { _product = value; }
    }

    public List<InputElement> Ingredients
    {
        get { return _ingredients; }
        set { _ingredients = value; }
    }
}



public class InputElement
{
    public P Element;//the element
    public int Units;//how many units

    public InputElement(P element, int units)
    {
        Element = element;
        Units = units;
    }
}


