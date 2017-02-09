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

    void Start()
    {
        _arrow = GetChildCalled("Arrow");
        _arrow.gameObject.SetActive(false);

        _redCircle = GetChildCalled("Red_Circle");
        _redCircle.SetActive(false);

        StartCoroutine("FiveSecUpd");

        _questWin = FindObjectOfType<QuestWindow>();
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

    }

    private void ShowHere(GameObject gameObjectP, string which)
    {
        gameObjectP.SetActive(true);
        var button = gameObjectP.GetComponent<UnityEngine.UI.Button>();
    }



    string RemoveLastPart(string pass)
    {
        var arr = pass.Split('.');
        return arr[0];
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
        _redCircle.SetActive(false);
    }

    internal void ShowNewQuestAvail()
    {
        _redCircle.SetActive(true);
        AudioCollector.PlayOneShot("NEW_QUEST_1", 0);
    }
}

