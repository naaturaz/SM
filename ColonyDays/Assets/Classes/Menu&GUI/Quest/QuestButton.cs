using UnityEngine;

public class QuestButton : GUIElement
{
    private GameObject _arrow;
    private QuestWindow _questWin;
    private GameObject _redCircle;
    private GameObject _text;
    private bool _shownArrow;
    private bool _wasHidden;

    private void Start()
    {
        GatherAllGO();

        //_arrow.gameObject.SetActive(false);

        SetCircleAndTextTo(false);

        _questWin = FindObjectOfType<QuestWindow>();

        if (!_wasHidden)
        {
            Program.gameScene.QuestManager.HideQuestBtn();
        }
    }

    private void GatherAllGO()
    {
        _arrow = GetChildCalled("Arrow");
        _text = GetChildCalled("Text");
        _redCircle = GetChildCalled("Red_Circle");
    }

    private void SetCircleAndTextTo(bool active)
    {
        _redCircle.SetActive(active);
        //_text.SetActive(active);
    }

    private void Update()
    {
        if (Program.gameScene.QuestManager.CurrentQuests.Count == 0 && _redCircle.activeSelf)
        {
            SetCircleAndTextTo(false);
        }
        else if (Program.gameScene.QuestManager.CurrentQuests.Count > 0 && !_redCircle.activeSelf)
        {
            SetCircleAndTextTo(true);
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

        //so it not shown anymore
        _shownArrow = true;
    }

    internal void ShowNewQuestAvail()
    {
        _wasHidden = true;
        GatherAllGO();

        SetCircleAndTextTo(true);
        //AudioCollector.PlayOneShot("NEW_QUEST_1", 0);

        if (!_shownArrow)
        {
            _shownArrow = true;
            _arrow.SetActive(true);
        }
    }
}