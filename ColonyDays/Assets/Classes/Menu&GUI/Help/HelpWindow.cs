using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HelpWindow : GUIElement
{
    Text _contentText;

    private GameObject _content;
    private GameObject _scroll_Ini_PosGO;

    InputField _searchInput;
    GameObject _f1forHelp;

    //helps available. u can add anything here, but need to be add on Langugaes.cs
    List<string> _helps = new List<string>()
    {
        "Construction.Help",
        "Camera.Help",
        "Sea Path.Help",
        "People Range.Help",
        "Pirate Threat.Help",
        "Port Reputation.Help",
        //"Emigrate.Help",
        "Food.Help",
        "Weight.Help",
        "More.Help",

        "Products Expiration.Help",
        "Horse Carriages.Help",
        "Usage of goods.Help",
        "Happiness.Help",
        "Line production.Help",
        "Bulletin.Help",
        "Trading.Help",
        //"Combat Mode.Help",

        "Population.Help",
        "Inputs.Help",
        "WheelBarrows.Help",
        "What is Ft3 and M3?.Help",
        "Production Tab.Help",
        "Our Inventories.Help",
        "Inventories Explanation.Help",
    };

    void Start()
    {
        iniPos = transform.position;

        _contentText = GetChildCalled("Content_Text").GetComponent<Text>();

        var _scroll = GetChildCalled("Scroll_View");
        _content = GetGrandChildCalledFromThis("Content", _scroll);
        _scroll_Ini_PosGO = GetChildCalledOnThis("Scroll_Ini_Pos", _content);

        Hide();

        _helps = _helps.OrderBy(a => a.ToString()).ToList();

        _searchInput = GetChildCalled("SearchInputField").GetComponent<InputField>();
    }

    public void Show(string val)
    {
        HelpSelected("Construction.Help");

        Program.gameScene.TutoStepCompleted("ShowHelp.Tuto");

        transform.position = iniPos;
        ResetScroolPos();
        PopulateScrollView();

        Program.MouseListener.HidePersonBuildOrderNotiWindows();

        if (_f1forHelp == null)
        {
            //F1 for help
            _f1forHelp = GameObject.Find("F1 for help");
        }

        if (_f1forHelp!=null)
        {
            _f1forHelp.SetActive(false);
            PlayerPrefs.SetString("F1", "Used");
        }
    }

    #region Scroll

    private void PopulateScrollView()
    {
        ResetScroolPos();
        AdjustContentHeight(_helps.Count * 4.9f);

        ClearBtns();
        ShowButtons(_helps);
    }

    private void ClearBtns()
    {
        for (int i = 0; i < _btns.Count; i++)
        {
            _btns[i].Destroy();
        }
        _btns.Clear();
    }

    List<HelpTile> _btns = new List<HelpTile>();
    private void ShowButtons(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var iniPosHere = _scroll_Ini_PosGO.transform.localPosition +
                             new Vector3(0, -4.8f * i, 0);

            var a = HelpTile.CreateTile(_content.gameObject.transform, list[i],
                iniPosHere, this);

            _btns.Add(a);
        }
    }

    #endregion

    void Update()
    {
    }

    public string Status(Quest q)
    {
        if (Program.gameScene.QuestManager.IsDone(q))
        {
            return "Done";
        }
        else if (q.IsAPercetangeOne())
        {
            return q.PercetageDone().ToString("0%");
        }
        return "Active";
    }

    internal void HelpSelected(string Key)
    {
        _contentText.text = Languages.ReturnString(Key);
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    /// <param name="Key"></param>
    public void ShowSpecificItem(string Key)
    {
        Program.MouseListener.HidePersonBuildOrderNotiBulletinHelpWin();
        Show("");
        HelpSelected(Key);
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    /// <param name="Key"></param>
    public void ShowSpecificItemDontHideWindows(string Key)
    {
        Show("");
        HelpSelected(Key);
    }

    public void SuggestChange()
    {
        Dialog.InputFormDialog(H.Feedback);
    }

    public void OpenWiki()
    {
        Application.OpenURL("http://sugarmill.wikia.com/wiki/SugarMill_Wiki");
    }

    public void Search()
    {
        var arr = _helps.Where
            (a => a.Contains(_searchInput.text.ToLower()) || a.Contains(_searchInput.text.ToUpper()))
            .ToArray();
        ClearBtns();
        ShowButtons(arr.ToList());
    }

}
