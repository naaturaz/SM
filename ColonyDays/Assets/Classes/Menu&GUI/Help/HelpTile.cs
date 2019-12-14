using UnityEngine;
using UnityEngine.UI;

public class HelpTile : GUIElement
{
    private Text _descText;
    private Text _priceText;
    private HelpWindow _window;
    string _key;

    public string Key
    {
        get { return _key; }
        set { _key = value; }
    }

    public HelpWindow Window
    {
        get { return _window; }
        set { _window = value; }
    }

    void Start()
    {
        _descText = FindGameObjectInHierarchy("Item_Desc", gameObject).GetComponent<Text>();
        _priceText = FindGameObjectInHierarchy("Price_Desc", gameObject).GetComponent<Text>();
        Init();
    }

    private void Init()
    {
        var key = UString.RemoveLastPart(Key);
        _descText.text = Languages.ReturnString(key);

        _priceText.text = "";
    }

    void Update()
    {
    }

    /// <summary>
    /// Calle from GUI
    /// </summary>
    public void ButtonClick()
    {
        _window.HelpSelected(Key);
    }

    internal static HelpTile CreateTile(Transform container,
        string key, Vector3 iniPos, HelpWindow win)
    {
        HelpTile obj = null;

        obj = (HelpTile)Resources.Load(Root.help_Tile, typeof(HelpTile));
        obj = (HelpTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var iniScale = obj.transform.localScale;
        obj.transform.SetParent(container);
        obj.transform.localPosition = iniPos;
        obj.transform.localScale = iniScale;

        obj.Key = key;
        obj.Window = win;

        return obj;
    }

}
