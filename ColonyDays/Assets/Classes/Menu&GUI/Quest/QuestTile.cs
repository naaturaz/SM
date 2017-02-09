using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class QuestTile : GUIElement
{
    private Text _descText;
    private Text _priceText;

    private QuestWindow _window;

    public Quest Value { get; set; }

    public QuestWindow Window
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
        _descText.text = Value.Key;
        _priceText.text = _window.Status(Value);
    }

    void Update()
    {

    }

    /// <summary>
    /// Calle from GUI
    /// </summary>
    public void ButtonClick()
    {
        _window.QuestSelected(Value);
    }

    internal static QuestTile CreateTile(Transform container,
        Quest val, Vector3 iniPos, QuestWindow win)
    {
        QuestTile obj = null;

        var root = "";

        obj = (QuestTile)Resources.Load(Root.quest_Tile, typeof(QuestTile));
        obj = (QuestTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var iniScale = obj.transform.localScale;
        obj.transform.SetParent(container);
        obj.transform.localPosition = iniPos;
        obj.transform.localScale = iniScale;

        obj.Value = val;
        obj.Window = win;

        return obj;
    }



}

