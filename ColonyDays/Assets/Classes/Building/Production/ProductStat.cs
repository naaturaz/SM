using System.Collections.Generic;

public class ProductInfo
{
    private int _id;//the id of the product 
    private P _product;
    List<InputElement> _ingredients = new List<InputElement>();
    private List <H> _hType = new List<H>();

    //the line name of the product will be shown on Building window
    private string _productLine;
    //the details of a prduct
    private string _details;

    public ProductInfo(P product, List<InputElement> ingredients, H buildHType)
    {
        _product = product;
        _ingredients = ingredients;
        Id = General.GiveMeAutoNumber();
        _hType.Add(buildHType);

        BuildStrings();
    }

    public ProductInfo(P product, List<InputElement> ingredients, List<H> buildHType)
    {
        _product = product;
        _ingredients = ingredients;
        Id = General.GiveMeAutoNumber();
        _hType = buildHType;

        BuildStrings();
    }

    private void BuildStrings()
    {
        BuildProductLine();
        BuildDetails();
    }

    private void BuildDetails()
    {
        var dens = Program.gameScene.ExportImport1.ReturnDensityKGM3(_product);
        var prodF = Program.gameScene.ExportImport1.ReturnProduceFactor(_product);
        var price = Program.gameScene.ExportImport1.ReturnPrice(_product);

        _details = "Product " + _product + " details: \n";

        if (Ingredients != null)
        {
            for (int i = 0; i < Ingredients.Count; i++)
            {
                _details = _details + "Input: " + Ingredients[i].Element + " Units: " +
                    Ingredients[i].Units + " " + Settings.WeightUnit() + " \n";
            }
        }
       
        _details = _details + "Density: " +dens + "\n";
        _details = _details + "Produce Factor: " + prodF + "\n";
        _details = _details + "Base Price: " + price + "\n";
    }

    void BuildProductLine()
    {
        _productLine = _product + " (";

        if (Ingredients != null)
        {
            for (int i = 0; i < Ingredients.Count; i++)
            {
                _productLine = _productLine + Ingredients[i].Element + ", ";
            }
        }

        //remove last 2 chars
        _productLine = _productLine.Substring(0, _productLine.Length - 2);

        if (Ingredients != null)
        {
            _productLine = _productLine + ")";
        }
    }

    public ProductInfo()
    {
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

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public List<H> HType
    {
        get { return _hType; }
        set { _hType = value; }
    }

    public string ProductLine
    {
        get { return _productLine; }
        set { _productLine = value; }
    }

    public string Details
    {
        get { return _details; }
        set { _details = value; }
    }
}



public class InputElement
{
    public P Element;//the element
    public float Units;//how many units

    public InputElement(P element, float units)
    {
        Element = element;
        Units = units;
    }
}


