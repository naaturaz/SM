using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpecTile : GUIElement
{
    private SpecData _spec;
    ExportData _export;

    private Text _prodLbl;
    private Text _input1Lbl;
    private Text _input2Lbl;
    private Text _input3Lbl;

    private Text _buildingLbl;

    private Text _priceLbl;

    List<Text> _inputs = new List<Text>();

    public SpecData Spec
    {
        get { return _spec; }
        set { _spec = value; }
    }

    public ExportData Export
    {
        get
        {
            return _export;
        }

        set
        {
            _export = value;
        }
    }

    void Start()
    {
        _prodLbl = GetChildCalled("Prod_Lbl").GetComponent<Text>();
        _prodLbl.text = "-";

        _input1Lbl = GetChildCalled("Input1_Lbl").GetComponent<Text>();
        _input1Lbl.text = "-";
        _input2Lbl = GetChildCalled("Input2_Lbl").GetComponent<Text>();
        _input2Lbl.text = "-";

        _input3Lbl = GetChildCalled("Input3_Lbl").GetComponent<Text>();
        _input3Lbl.text = "-";

        _buildingLbl = GetChildCalled("Building_Lbl").GetComponent<Text>();
        _buildingLbl.text = "-";

        _priceLbl = GetChildCalled("Price_Lbl").GetComponent<Text>();
        _priceLbl.text = "-";

        if (Spec != null)
            InitSpec();
        if (Export != null)
            InitExport();
    }

    private void InitExport()
    {
        //HandleTitleBar
        if (Export.MDate == null)
        {
            _prodLbl.text = Languages.ReturnString("Date");
            _input1Lbl.text = Languages.ReturnString("Building");
            _input2Lbl.text = Languages.ReturnString("Product");
            _input3Lbl.text = Languages.ReturnString("Amount");
            _priceLbl.text = Languages.ReturnString("Transaction");
            return;
        }

        _prodLbl.text = Export.MDate.ToStringFormatMonDayYear();
        _input1Lbl.text = Languages.ReturnString(Export.Building);
        _input2Lbl.text = Languages.ReturnString(Export.Prod);
        _input3Lbl.text = Unit.ConvertFromKGToCurrent(Export.Amt).ToString("N0") + " " 
            + Unit.CurrentWeightUnitsString();
        _priceLbl.text = MyText.DollarFormat(Export.Money);
    }

    private void InitSpec()
    {
        _inputs.Add(_input1Lbl);
        _inputs.Add(_input2Lbl);
        _inputs.Add(_input3Lbl);

        _prodLbl.text = _spec.ProdInfo.Product + "";

        if (_spec.ProdInfo.Ingredients != null)
        {
            for (int i = 0; i < _spec.ProdInfo.Ingredients.Count; i++)
            {
                _inputs[i].text = Languages.ReturnString(_spec.ProdInfo.Ingredients[i].Element + "");
            }
        }

        _buildingLbl.text = "-";

        if (_spec.ProdInfo.HType.Count > 0)
            _buildingLbl.text = Languages.ReturnString(_spec.ProdInfo.HType[0].ToString());

        _priceLbl.text = Unit.ProperPricedAndFormat(_spec.Price);  //_spec.Price

        if (_spec.Price == -100)
            _priceLbl.text = Languages.ReturnString("Price");
    }

    void Update()
    {
    }

    internal static SpecTile CreateTile(Transform container, SpecData spec, Vector3 iniPos)
    {
        SpecTile obj = null;

        obj = (SpecTile)Resources.Load(Root.spec_Tile, typeof(SpecTile));
        obj = (SpecTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var iniScale = obj.transform.localScale;
        obj.transform.SetParent(container);
        obj.transform.localPosition = iniPos;
        obj.transform.localScale = iniScale;

        obj.Spec = spec;

        return obj;
    }

    internal static SpecTile CreateTile(Transform container,
    ExportData export, Vector3 iniPos)
    {
        SpecTile obj = null;

        obj = (SpecTile)Resources.Load(Root.spec_Tile, typeof(SpecTile));
        obj = (SpecTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var iniScale = obj.transform.localScale;
        obj.transform.SetParent(container);
        obj.transform.localPosition = iniPos;
        obj.transform.localScale = iniScale;

        obj.Export = export;

        return obj;
    }
}

