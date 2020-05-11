using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

internal class SaveLoadGameWindow : GUIElement
{
    private GameObject _content;
    private RectTransform _contentRectTransform;
    private InputField _inputName;
    private GameObject _inputNameGO;
    private Text _saveNameLbl;
    private GameObject _scroll_Ini_PosGO;
    private Vector3 _scrollIniPos;
    private Text _selectedToLoadNName;
    private string _tileNameSelected;
    private List<ShowSaveLoadTile> _tilesSpawn = new List<ShowSaveLoadTile>();
    private Text _title;
    private Scrollbar _verticScrollbar;
    private string _which;//is load or save dialog

    public void DeleteCallBack()
    {
        PopulateScrollView();
        ClearForm();
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void LockInput()
    {
        Program.LockInputSt();
    }

    public void Show(string which)
    {
        _which = which;
        ClearForm();

        if (which == "Save")
        {
            _saveNameLbl.text = Languages.ReturnString("NameToSave");
            _inputNameGO.SetActive(true);

            _inputName.Select();
            _inputName.ActivateInputField();

            _title.text = Languages.ReturnString("SaveGame.Dialog");
            _selectedToLoadNName.enabled = false;
        }
        else if (which == "Load")
        {
            _saveNameLbl.text = Languages.ReturnString("NameToLoad");
            _inputNameGO.SetActive(false);
            _title.text = Languages.ReturnString("LoadGame.Dialog");
            _selectedToLoadNName.enabled = true;
        }
        PopulateScrollView();
        Show();
        ResetScroolPos();
        LockInput();
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void UnLockInput()
    {
        Program.UnLockInputSt();
    }

    internal void MouseListen(string sub)
    {
        sub = sub.Substring(5);

        if (sub == "OKBtn" && _which == "Save")
        {
            DataController.SaveGame(_inputName.text);
            PopulateScrollView();
        }
        else if (sub == "OKBtn" && _which == "Load")
        {
            DataController.LoadGame(_tileNameSelected);
        }
        //Reloadd main menu
        else if (sub == "CancelBtn")
        {
            Hide();
            DestroyPrevTiles();
            Program.MyScreen1.HideWindowShowMain(this);
        }
        else if (sub == "DeleteBtn")
        {
            if (string.IsNullOrEmpty(_tileNameSelected))
            {
                //pls select a game to delete
                return;
            }
            DataController.DeleteGame(_tileNameSelected);
        }
        //clicked on a button of a SaveLoadTile
        else
        {
            _tileNameSelected = sub;

            if (_which == "Load")
            {
                _selectedToLoadNName.text = _tileNameSelected;
            }
            else
            {
                _inputName.text = _tileNameSelected;
            }
        }
    }

    private void ClearForm()
    {
        _tileNameSelected = "";
        _selectedToLoadNName.text = "";
        _inputName.text = "";

        if (_verticScrollbar != null)
        {
            _verticScrollbar.value = 1;
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

    private void Hide()
    {
        base.Hide();
        UnLockInput();
    }

    private void PopulateScrollView()
    {
        SetTileIniPos();

        DestroyPrevTiles();
        var saves = Directory.GetDirectories(DataController.SugarMillPath()).ToList();

        SetHeightOfContentRect(saves.Count);
        ShowAllItems(saves);

        TakeScrollVerticBar();
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

        var y = (Screen.height * 30) / 892;
        return -y * i;
    }

    private void SetHeightOfContentRect(int tiles)
    {
        //892
        var tileYSpace = 5.57f;

        //5.57f the space btw two of them
        var size = (tileYSpace * tiles) + tileYSpace;
        _contentRectTransform.sizeDelta = new Vector2(0, size);
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

    private void ShowAllItems(List<string> saves)
    {
        for (int i = 0; i < saves.Count; i++)
        {
            var iniPos = ReturnIniPos(i);
            var tile = ShowSaveLoadTile.Create(Root.saveLoadTile, _content.transform, iniPos, saves[i]);

            _tilesSpawn.Add(tile);
        }
    }

    private void Start()
    {
        iniPos = transform.position;
        Hide();

        _inputNameGO = GetChildCalled("Input_Name");
        _inputName = _inputNameGO.GetComponent<InputField>();

        var titleLbl = GetChildCalled("Title");
        _title = titleLbl.GetComponentInChildren<Text>();

        var selLoad = GetChildCalled("Selected_To_Load");
        _selectedToLoadNName = selLoad.GetComponentInChildren<Text>();

        var saveNameLbl = GetChildCalled("Save_Name_Lbl");
        _saveNameLbl = saveNameLbl.GetComponentInChildren<Text>();

        var scroll = GetChildCalled("Scroll_View");

        _content = GetGrandChildCalledFromThis("Content", scroll);
        _contentRectTransform = _content.GetComponent<RectTransform>();

        _scroll_Ini_PosGO = GetChildCalledOnThis("Scroll_Ini_Pos", _content);

        //pull the last Saved game if one
        _tileNameSelected = PlayerPrefs.GetString("Last_Saved");
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
            MouseListen("Save.OKBtn");
        }
    }
}