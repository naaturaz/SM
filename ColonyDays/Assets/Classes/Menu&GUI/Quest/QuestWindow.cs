using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class QuestWindow : GUIElement
{
    List<GameObject> _gOs = new List<GameObject>();
    GameObject _arrow;
    Quest _currentQ;
    private Text _text;

    void Start()
    {
        _gOs = new List<GameObject>()
        {
            GetChildCalled("Quest1"), GetChildCalled("Quest2"),
            GetChildCalled("Quest3"),GetChildCalled("Quest4")
        };

        _arrow = GetChildCalled("Arrow");
        _arrow.gameObject.SetActive(false);

        HideAll();
        //Hide();

        StartCoroutine("FiveSecUpd");
    }
    private IEnumerator FiveSecUpd()
    {
        while (true)
        {
            yield return new WaitForSeconds(5); // wait

            if (_currentQ != null && _currentQ.IsAPercetangeOne())
            {
                _text.text = RemoveLastPart(_currentQ.Key) + Percentage();
            }
        }
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

    public void SetAQuest(Quest quest)
    {
        _currentQ = quest;
        //1st
        if (quest.Key == "Bohio.Quest")
        {
            _arrow.gameObject.SetActive(true);
        }

        //Show();
        for (int i = 0; i < _gOs.Count; i++)
        {
            if (!_gOs[i].activeSelf)
            {
                ShowHere(_gOs[i], quest.Key);
                return;
            }
        }
    }

    private void ShowHere(GameObject gameObjectP, string which)
    {
        gameObjectP.SetActive(true);
        var button = gameObjectP.GetComponent<UnityEngine.UI.Button>();
        _text = GetChildCalledOnThis("Text", gameObjectP).GetComponent<Text>();
        _text.text = RemoveLastPart(which) + Percentage();
    }

    private string Percentage()
    {
        if (_currentQ.IsAPercetangeOne())
        {
            return " - " + _currentQ.PercetageDone().ToString("0%");
        }

        return "";
    }

    string RemoveLastPart(string pass)
    {
        var arr = pass.Split('.');
        return arr[0];
    }

    public void RemoveAQuest(Quest which)
    {
        for (int i = 0; i < _gOs.Count; i++)
        {
            var button = _gOs[i];//bz starts at 1 on GUI 
            if (!button.activeSelf)
            {
                continue;
            }

            var text = GetChildCalledOnThis("Text", button).GetComponent<Text>();
            if (_currentQ.Key == which.Key)
            {
                _currentQ = null;
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

        Program.gameScene.QuestManager.SpawnDialog(_currentQ.Key);
    }
}

