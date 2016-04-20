using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


class SaveLoadGameWindow : GUIElement
{
    private GameObject _inputNameGO;
    private InputField _inputName;
    
    private Text _title;
    private GameObject _saveNameLbl;

    private GameObject _content;
    private RectTransform _contentRectTransform;

    private GameObject _scroll_Ini_Pos;

    void Start()
    {
        iniPos = transform.position;
        Hide();

        _scroll_Ini_Pos = GetChildCalled("Scroll_Ini_Pos"); 


        _inputNameGO = GetChildCalled("Input_Name"); 
        _inputName = _inputNameGO.GetComponent<InputField>();

        var titleLbl = GetChildCalled("Title");
        _title = titleLbl.GetComponentInChildren<Text>();

        _saveNameLbl = GetChildCalled("Save_Name_Lbl");
        
        var scroll = GetChildCalled("Scroll_View");


        _content = GetGrandChildCalledFromThis("Content", scroll);
        _contentRectTransform = _content.GetComponent<RectTransform>();
    }

    public void Show(string which)
    {
        if (which == "Save")
        {
            _saveNameLbl.SetActive(true);
            _inputNameGO.SetActive(true);
            _title.text = Languages.ReturnString("SaveGame.Dialog");
        }
        else if (which == "Load")
        {
            _saveNameLbl.SetActive(false);

            _inputNameGO.SetActive(false);
            _title.text = Languages.ReturnString("LoadGame.Dialog");
        }
        PopulateScrollView();
        Show();
    }

    void Update()
    {
        
    }

    internal void MouseListen(string sub)
    {
        sub = sub.Substring(5);

        if (sub == "OKBtn")
        {
            DataController.SaveGame(_inputName.text);
        }
        //Reloadd main menu
        else if (sub == "CancelBtn")
        {
            Hide();
            DestroyPrevTiles();
            Program.MyScreen1.HideWindowShowMain(this);
        }

        Display();
    
    }



    private List<ShowSaveLoadTile> _tilesSpawn = new List<ShowSaveLoadTile>();
    private Vector3 _iniPos;

    void DestroyPrevTiles()
    {
        for (int i = 0; i < _tilesSpawn.Count; i++)
        {
            _tilesSpawn[i].Destroy();
        }
        _tilesSpawn.Clear();
    }

    void PopulateScrollView()
    {
        SetTileIniPos();

        DestroyPrevTiles();
        var saves = Directory.GetDirectories(DataController.SugarMillPath()).ToList();

        SetHeightOfContentRect(saves.Count);


        ShowAllItems(saves);
    }

    private void ShowAllItems(List<string> saves)
    {
        var iForSpwItem = 0;//so ReturnIniPos works nicely

        for (int i = 0; i < saves.Count; i++)
        {
            var tile = ShowSaveLoadTile.Create(Root.saveLoadTile, _content.transform, ReturnIniPos(iForSpwItem), saves[i]);

            _tilesSpawn.Add(tile);
            iForSpwItem++;
        }

    }

    void SetTileIniPos()
    {
        if (_iniPos == new Vector3())
        {
            _iniPos = _scroll_Ini_Pos.transform.position;
            _iniPos = new Vector3(_iniPos.x, _iniPos.y - 15, _iniPos.z);

        }
    }


    private void SetHeightOfContentRect(int tiles)
    {
        //throw new NotImplementedException();
    }



    Vector3 ReturnIniPos(int i)
    {
        return new Vector3(270 + _iniPos.x, ReturnY(i) + _iniPos.y, _iniPos.z);
    }

    float ReturnY(int i)
    {
        return -30f * i;
    }


    private void Display()
    {

    }
}

