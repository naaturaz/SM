using UnityEngine;
using UnityEngine.UI;

public class PriceTile : GUIElement
{
    private ProdSpec _prodSpec;
    private Text _descText;
    private InputField _input;

    public ProdSpec Spec
    {
        get { return _prodSpec; }
        set { _prodSpec = value; }
    }

    private void Start()
    {
        _descText = FindGameObjectInHierarchy("Item_Desc", gameObject).GetComponent<Text>();
        _input = FindGameObjectInHierarchy("Input", gameObject).GetComponent<InputField>();

        Init();
    }

    private void Init()
    {
        _descText.text = _prodSpec.Product + "";
        _input.text = _prodSpec.Price.ToString("C");
    }

    private void Update()
    {
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ClickLessSign()
    {
        _prodSpec.Price -= 0.01f;

        if (_prodSpec.Price <= 0.01)
        {
            _prodSpec.Price = 0.01f;
        }

        Init();
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ClickPlusSign()
    {
        _prodSpec.Price += 0.01f;
        Init();
    }

    /// <summary>
    /// When leave Input fiield this is called from GUI
    /// </summary>
    public void SetNewPrice()
    {
        if (AddOrderWindow.IsTextAValidInt(_input.text))
        {
            _prodSpec.Price = float.Parse(_input.text);
        }
        //else wont do anything just will reload last price
        Init();
    }

    internal static PriceTile CreateTile(Transform container,
        ProdSpec spec, Vector3 iniPos)
    {
        PriceTile obj = null;

        var root = "";

        obj = (PriceTile)Resources.Load(Root.price_Tile, typeof(PriceTile));
        obj = (PriceTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var iniScale = obj.transform.localScale;
        obj.transform.SetParent(container);
        obj.transform.localPosition = iniPos;
        obj.transform.localScale = iniScale;

        obj.Spec = spec;

        return obj;
    }
}