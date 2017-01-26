using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class QuestWindow : GUIElement
{
    List<GameObject> _gOs = new List<GameObject>();

    void Start()
    {
        _gOs = new List<GameObject>()
        {
            GetChildCalled("Quest1"), GetChildCalled("Quest2"),
            GetChildCalled("Quest3"),GetChildCalled("Quest4")
        };

        HideAll();
    }

    void HideAll()
    {
        for (int i = 0; i < _gOs.Count; i++)
        {
            _gOs[i].SetActive(false);
        }
    }

    void Update()
    {

    }

    public void SetAQuest(string which)
    {
        for (int i = 0; i < _gOs.Count; i++)
        {
            if (!_gOs[i].activeSelf)
            {
                ShowHere(_gOs[i], which);
                return;
            }
        }
    }

    private void ShowHere(GameObject gameObjectP, string which)
    {
        gameObjectP.SetActive(true);
        var button = gameObjectP.GetComponent<UnityEngine.UI.Button>();
        var text = GetChildCalledOnThis("Text", gameObjectP).GetComponent<Text>();
        text.text = RemoveLastPart(which);
    }

    string RemoveLastPart(string pass)
    {
        var arr = pass.Split('.');
        return arr[0];
    }

    public void RemoveAQuest(string which)
    {
        for (int i = 0; i < _gOs.Count; i++)
        {
            var button = _gOs[i];//bz starts at 1 on GUI 
            if (!button.activeSelf)
            {
                continue;
            }

            var text = GetChildCalledOnThis("Text", button).GetComponent<Text>();
            if (text.text == which)
            {
                HideButton(_gOs[i]);
                return;
            }
        }
    }

    private void HideButton(GameObject gameO)
    {
        gameO.SetActive(false);
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    /// <param name="which"></param>
    public void ClickOnButton(string which)
    {
        var index = int.Parse(which);

        var button = _gOs[index-1];//bz starts at 1 on GUI 
        var text = GetChildCalledOnThis("Text", button).GetComponent<Text>();

        Program.gameScene.QuestManager.SpawnDialog(text.text+".Quest");
    }
}

