using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class QuestButton : GUIElement
{
    GameObject _arrow;
    QuestWindow _questWin;
    GameObject _redCircle;
    GameObject _text;
    bool _shownArrow;


    void Start()
    {
        _arrow = GetChildCalled("Arrow");
        _arrow.gameObject.SetActive(false);

        _text = GetChildCalled("Text");
        _redCircle = GetChildCalled("Red_Circle");
        SetCircleAndTextTo(false);


        StartCoroutine("FiveSecUpd");

        _questWin = FindObjectOfType<QuestWindow>();
    }

    void SetCircleAndTextTo(bool active)
    {
        _redCircle.SetActive(active);
        _text.SetActive(active);
    }

    private IEnumerator FiveSecUpd()
    {
        while (true)
        {
            yield return new WaitForSeconds(5); // wait

        
        }
    }

    void Update()
    {
        if (Program.gameScene.QuestManager.CurrentQuests.Count == 0 && _redCircle.activeSelf)
        {
            SetCircleAndTextTo(false);

        }
    }

    private void ShowHere(GameObject gameObjectP, string which)
    {
        gameObjectP.SetActive(true);
        var button = gameObjectP.GetComponent<UnityEngine.UI.Button>();
    }






    private void HideButton(GameObject gameO)
    {
        gameO.SetActive(false);
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    /// <param name="which"></param>
    public void ClickOnButton()
    {
        _questWin.Show("");
    }

    internal void ShowNewQuestAvail()
    {
        SetCircleAndTextTo(true);
        AudioCollector.PlayOneShot("NEW_QUEST_1", 0);

        if (!_shownArrow)
        {
            _shownArrow = true;
            _arrow.SetActive(true);
        }
    }
}

