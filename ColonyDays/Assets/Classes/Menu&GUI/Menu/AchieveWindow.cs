using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class AchieveWindow : GUIElement
{
    private Text _title;

    private GameObject _content;
    private RectTransform _contentRectTransform;

    private GameObject _scroll_Ini_PosGO;

    private List<AchieveTile> _tilesSpawn = new List<AchieveTile>();
    private Vector3 _scrollIniPos;

    private Scrollbar _verticScrollbar;

    private SteamStatsAndAchievements _steamStatsAndAchievements;

    private void Start()
    {
        iniPos = transform.position;
        Hide();

        _steamStatsAndAchievements = FindObjectOfType<SteamStatsAndAchievements>();

        var titleLbl = GetChildCalled("Title");
        _title = titleLbl.GetComponentInChildren<Text>();

        var selLoad = GetChildCalled("Selected_To_Load");

        var saveNameLbl = GetChildCalled("Save_Name_Lbl");

        var scroll = GetChildCalled("Scroll_View");

        _content = GetGrandChildCalledFromThis("Content", scroll);
        _contentRectTransform = _content.GetComponent<RectTransform>();

        _scroll_Ini_PosGO = GetChildCalledOnThis("Scroll_Ini_Pos", _content);
    }

    public void Show(string which)
    {
        ClearForm();
        PopulateScrollView();
        Show();
    }

    private void ClearForm()
    {
        if (_verticScrollbar != null)
        {
            _verticScrollbar.value = 1;
        }
    }

    /// <summary>
    /// So as changes size will be available or not.
    /// We need this ref ' _verticScrollbar ' to set it to defauitl value
    /// </summary>
    private void TakeScrollVerticBar()
    {
        var vert = GetGrandChildCalled("Scrollbar Vertical");
        if (vert != null)
        {
            _verticScrollbar = vert.GetComponent<Scrollbar>();
        }
        else
        {
            _verticScrollbar = null;
        }
    }

    private void Update()
    {
        if (transform.position == iniPos && Input.GetKeyUp(KeyCode.Return))
        {
            MouseListen("Achieve.OKBtn");
        }
    }

    internal void MouseListen(string sub)
    {
        sub = sub.Substring(8);

        if (sub == "OKBtn")
        {
            Hide();
            DestroyPrevTiles();
            Program.MyScreen1.HideWindowShowMain(this);
        }
    }

    private void DestroyPrevTiles()
    {
        for (int i = 0; i < _tilesSpawn.Count; i++)
        {
            _tilesSpawn[i].Destroy();
        }
        _tilesSpawn.Clear();
    }

    private void PopulateScrollView()
    {
        SetTileIniPos();

        DestroyPrevTiles();

        SetHeightOfContentRect(_steamStatsAndAchievements.Achievements_t.Length);
        ShowAllItems();

        TakeScrollVerticBar();
    }

    private void ShowAllItems()
    {
        for (int i = 0; i < _steamStatsAndAchievements.Achievements_t.Length; i++)
        {
            var iniPos = ReturnIniPos(i);
            var tile = AchieveTile.Create(Root.achieveTile, _content.transform, iniPos,
                _steamStatsAndAchievements.Achievements_t[i]);

            _tilesSpawn.Add(tile);
        }
    }

    /// <summary>
    /// need to be called to set the Ini POs everytime
    /// </summary>
    private void SetTileIniPos()
    {
        var ySpace = Screen.height / 59.46667f;    //15 on editor

        _scrollIniPos = _scroll_Ini_PosGO.transform.position;
        _scrollIniPos = new Vector3(_scrollIniPos.x, _scrollIniPos.y - ySpace, _scrollIniPos.z);
    }

    private void SetHeightOfContentRect(int tiles)
    {
        //892
        var tileYSpace = 5.57f * 3.3f;
        //var tileYSpace = Screen.height / 160.1436f;//5.57f on editor

        //5.57f the space btw two of them
        var size = (tileYSpace * tiles) + tileYSpace;
        _contentRectTransform.sizeDelta = new Vector2(0, size);
    }

    private Vector3 ReturnIniPos(int i)
    {
        var xAddVal = Screen.width / 5.87407f;
        return new Vector3(xAddVal + _scrollIniPos.x, ReturnY(i) + _scrollIniPos.y, _scrollIniPos.z);
    }

    private float ReturnY(int i)
    {
        if (i == 0)
        {
            return 0;
        }
        //30
        var y = (Screen.height * 100) / 892;
        return -y * i;
    }
}