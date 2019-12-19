using UnityEngine;
using UnityEngine.UI;

public class ButtonTile : GUIElement
{
    private Text _descText;
    private Text _priceText;

    public ProdSpec Value { get; set; }

    public AddOrderWindow OrderWindow { get; set; }

    void Start()
    {
        _descText = FindGameObjectInHierarchy("Item_Desc", gameObject).GetComponent<Text>();
        _priceText = FindGameObjectInHierarchy("Price_Desc", gameObject).GetComponent<Text>();
        
        Init();
    }

    private void Init()
    {
        _descText.text = Languages.ReturnString(Value.Product+"");
        _priceText.text = Unit.ProperPricedAndFormat(Value.Price) + Languages.ReturnString(" per ") + Unit.CurrentWeightUnitsString();
    }

    void Update()
    {
    }

    /// <summary>
    /// Calle from GUI
    /// </summary>
    public void ButtonClick()
    {
        OrderWindow.ProdSelected(_descText.text);
    }

    internal static ButtonTile CreateTile(Transform container,
        ProdSpec val, Vector3 iniPos, AddOrderWindow win)
    {
        ButtonTile obj = null;

        obj = (ButtonTile)Resources.Load(Root.button_Tile, typeof(ButtonTile));
        obj = (ButtonTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var iniScale = obj.transform.localScale;
        obj.transform.SetParent(container);
        obj.transform.localPosition = iniPos;
        obj.transform.localScale = iniScale;

        obj.Value = val;
        obj.OrderWindow = win;

        return obj;
    }

}
