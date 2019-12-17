using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestWindow : GUIElement
{
    Text _rewardText;
    Text _contentText;

    private GameObject _content;
    private GameObject _scroll_Ini_PosGO;

    void Start()
    {
        iniPos = transform.position;

        _rewardText = GetChildCalled("Reward_Text").GetComponent<Text>();
        _contentText = GetChildCalled("Content_Text").GetComponent<Text>();

        var _scroll = GetChildCalled("Scroll_View");
        _content = GetGrandChildCalledFromThis("Content", _scroll);
        _scroll_Ini_PosGO = GetChildCalledOnThis("Scroll_Ini_Pos", _content);

        Hide();
    }

    public void Show(string val)
    {
        if (Program.gameScene.QuestManager.CurrentPlsDone().Count == 0)
        {
            return;
        }

        transform.position = iniPos;

        ResetScroolPos();

        PopulateScrollView();

        QuestSelected(Program.gameScene.QuestManager.CurrentPlsDone()[0]);

        Program.gameScene.QuestManager.QuestBtn.HideArrow();
    }

    #region Scroll

    private void PopulateScrollView()
    {
        ClearBtns();
        ShowButtons(Program.gameScene.QuestManager.CurrentPlsDone());
    }

    private void ClearBtns()
    {
        for (int i = 0; i < _btns.Count; i++)
        {
            _btns[i].Destroy();
        }
        _btns.Clear();
    }

    List<QuestTile> _btns = new List<QuestTile>();
    private void ShowButtons(List<Quest> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var iniPosHere = _scroll_Ini_PosGO.transform.localPosition +
                             new Vector3(0, -4.8f * i, 0);

            var a = QuestTile.CreateTile(_content.gameObject.transform, list[i],
                iniPosHere, this);

            _btns.Add(a);
        }
    }

    #endregion

    void Update()
    {
    }

    internal void QuestSelected(Quest q)
    {
        _rewardText.text = Languages.ReturnString("Reward: ") + q.Prize.ToString("C1") + "\n " + Languages.ReturnString("Status: ") + Status(q);
        _contentText.text = Languages.ReturnString(q.Key);
    }

    public string Status(Quest q)
    {
        if (Program.gameScene.QuestManager.IsDone(q))
        {
            return Languages.ReturnString("Done");    
        }
        else if (q.IsAPercetangeOne())
        {
            return q.PercetageDone().ToString("0%");
        }
        return Languages.ReturnString("Active");
    }
}
