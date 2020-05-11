using System;

public class QuestBar : GUIElement
{
    private QuestWindow _questWin;
    private string _oldQuest;

    private void Start()
    {
        base.Start();
        _questWin = FindObjectOfType<QuestWindow>();
        Hide();
    }

    //_en.Add("Shack.Quest.Title", "Build a Shack");
    internal void QuestSelected(Quest q)
    {
        _text.text = Languages.ReturnString("Current Quest:");
        _text.text += String.Format("\n- {0}", Languages.ReturnString(q.Key + ".Title"));
        _text.text += "\n    " + Languages.ReturnString("Reward: ") + q.Prize.ToString("C1");
    }

    private void Update()
    {
        base.Update();
        CheckQuestManager();
    }

    private void CheckQuestManager()
    {
        var q = Program.gameScene.QuestManager.CurrentQuest();
        if (q == null)
        {
            _text.text = Languages.ReturnString("Have Fun");
            return;
        }

        if (_oldQuest != q.Key)
        {
            _oldQuest = q.Key;
            QuestSelected(q);
            Show();
        }
        else if (Program.gameScene.QuestManager.IsDone(q))
        {
            _text.text = Languages.ReturnString("Have Fun");
        }
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ClickOnButton()
    {
        _questWin.Show("");
    }
}