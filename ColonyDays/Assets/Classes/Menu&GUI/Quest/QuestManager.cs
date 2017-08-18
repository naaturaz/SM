using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class QuestManager
{
    float _lastCompleted;

    QuestButton _questBtn;
    List<int> _currentQuests = new List<int>();
    List<int> _doneQuest = new List<int>();
    bool _wasLoaded;//whe is a loaded game

    float _timeToNextQuest = 90;


    public List<int> CurrentQuests
    {
        get { return _currentQuests; }
        set { _currentQuests = value; }
    }

    public List<int> DoneQuest
    {
        get { return _doneQuest; }
        set { _doneQuest = value; }
    }


    internal QuestButton QuestBtn
    {
        get
        {
            return _questBtn;
        }

        set
        {
            _questBtn = value;
        }
    }

    List<Quest> _bank = new List<Quest>()
    { 
        //need to mention reward still 


        new Quest("Shack.Quest", 500, 5.5f),

        new Quest("Lamp.Quest", 500, 5.5f),


        new Quest("SmallFarm.Quest", 550, 5.5f),
        new Quest("FarmHire.Quest", 600, 5.1f),
        new Quest("FarmProduce.Quest", 650, 5.1f, 100),
        //new Quest("Transport.Quest", 700, 5.5f),
        new Quest("Export.Quest", 750, 5.5f),
        new Quest("HireDocker.Quest", 800, 5.5f),
        new Quest("ImportOil.Quest", 950, 5.5f, 500),
        new Quest("MakeBucks.Quest", 850, 5.5f, 100),

        new Quest("HeavyLoad.Quest", 900, 5.5f),
        new Quest("HireHeavy.Quest", 900, 5.5f),

        new Quest("Production.Quest", 500, 5.5f),
        new Quest("ChangeProductToWeapon.Quest", 500, 5.5f),
        new Quest("BlackSmithHire.Quest", 500, 5.5f),
        new Quest("WeaponsProduce.Quest", 500, 5.5f, 100),
        new Quest("ExportWeapons.Quest", 500, 5.5f),



        new Quest("Population50.Quest", 900, 5.5f),

    };


    public List<Quest> CurrentPlsDone()
    {
        var res = _currentQuests.ToArray();

        var listRes = res.ToList();
        listRes.AddRange(_doneQuest);
        listRes = listRes.Distinct().ToList();

        List<Quest> fin = new List<Quest>();
        for (int i = 0; i < listRes.Count; i++)
        {
            fin.Add(_bank[listRes[i]]);
        }

        return fin;
    }

    public bool IsDone(Quest q)
    {
        var which = _bank.FindIndex(a => a.Key == q.Key);

        return _doneQuest.Contains(which);
    }




    public QuestManager()
    {

        if (76561198245800476 == SteamUser.GetSteamID().m_SteamID)
        {
            _timeToNextQuest = 10;
        }
    }


    bool IsAnyActiveQuestMatchThisKey(string key)
    {
        for (int i = 0; i < _currentQuests.Count; i++)
        {
            if (key == _bank[_currentQuests[i]].Key)
            {
                return true;
            }
        }
        return false;
    }

    Quest GetQuestWithKey(string key)
    {
        var ind = _bank.FindIndex(a => a.Key == key);
        return _bank[ind];
    }

    int GetIndexOfQuest(Quest q)
    {
        return _bank.FindIndex(a => a.Key == q.Key);
    }




    internal void HideQuestBtn()
    {
        if (_questBtn == null)
        {
            _questBtn = MonoBehaviour.FindObjectOfType<QuestButton>();
        }
        _questBtn.gameObject.SetActive(false);

    }

    private void ShowQuestBtn()
    {
        if (_questBtn == null)
        {
            _questBtn = MonoBehaviour.FindObjectOfType<QuestButton>();
        }
        _questBtn.gameObject.SetActive(true);
    }

    public void QuestFinished(string which)
    {
        ShowQuestBtn();//in case questBtn is null 

        which = which + ".Quest";
        _questBtn.HideArrow();

        if (IsAnyActiveQuestMatchThisKey(which))
        {
            var quest = GetQuestWithKey(which);
            var indexQ = GetIndexOfQuest(quest);

            ShowPrize(quest);

            Program.gameScene.GameController1.Dollars += quest.Prize;
            AudioCollector.PlayOneShot("BoughtLand", 0);
            BulletinWindow.SubBulletinFinance1.FinanceLogger.AddToAcct("Quests Completion", quest.Prize);

            _currentQuests.Remove(indexQ);//remove from _current list
            _doneQuest.Add(indexQ);//adds to done list 
        }
    }

    /// <summary>
    /// Some quest need progess and here is where it gets reported 
    /// </summary>
    /// <param name="which"></param>
    /// <param name="amt"></param>
    public void AddToQuest(string which, float amt)
    {
        if (IsAnyActiveQuestMatchThisKey(which + ".Quest"))
        {
            var quest = GetQuestWithKey(which + ".Quest");
            var indexQ = GetIndexOfQuest(quest);

            //adds
            quest.AddToProgress(amt);
            //checks if is completed
            if (quest.IsQuestCompleted())
            {
                QuestFinished(which);
            }
        }
    }

    public void SpawnDialog(string which)
    {
        //spawn dialog 
        Dialog.OKDialog(H.InfoKey, which);

        ShowQuestBtn();

        _questBtn.ShowNewQuestAvail();
    }

    void ShowPrize(Quest q)
    {
        Dialog.OKDialog(H.CompleteQuest, q.Prize + "");
        //AudioCollector.PlayOneShot("QUEST_COMPLETED_1", 0);
    }

    public void QuestCompletedAcknowled()
    {
        _lastCompleted = Time.time;


    }

    public void Update()
    {
        if (Program.MouseListener.IsAWindowShownNow() || CamControl.IsMainMenuOn() || _lastCompleted < 0)
        {
            return;
        }

        if (Program.gameScene.IsPassingTheTutoNow())
        {
            return;
        }

        //to show  others  and loaded 
        if (Time.time > _lastCompleted + _timeToNextQuest)
        {
            if (Dialog.IsActive() || BuildingPot.Control.CurrentSpawnBuild != null)
            {
                //so goes trhu again in 30s
                _lastCompleted = Time.time - 60;
                return;
            }

            if (_currentQuests.Count == 0)
            {
                AddANewQuest();
            }
            else
            {
                ShowQuestBtn();
            }
        }
    }

    private void AddANewQuest()
    {
        //1st time ever
        if (_currentQuests.Count == 0 && _doneQuest.Count == 0)
        {
            _currentQuests.Add(0);
            SpawnDialog(_bank[0].Key);
            return;
        }

        var highest = GetHighestQuestCompletedOrCurrent();
        if (highest + 1 >= _bank.Count)
        {
            _lastCompleted = -1;
        }
        else
        {
            _currentQuests.Add(highest + 1);
            SpawnDialog(_bank[highest + 1].Key);
        }
    }

    int GetHighestQuestCompletedOrCurrent()
    {
        var highestC = 0;
        var highestD = 0;

        if (_currentQuests.Count > 0)
        {
            highestC = UMath.ReturnMax(_currentQuests);
        }
        if (_doneQuest.Count > 0)
        {
            highestD = UMath.ReturnMax(_doneQuest);
        }

        List<int> temp = new List<int>() { highestC, highestD };

        return UMath.ReturnMax(temp);
    }

    public void ResetNewGame()
    {
        _lastCompleted = 0;

        _currentQuests.Clear();
        _doneQuest.Clear();
    }


    /// <summary>
    /// Called right after loaded meant to show current 
    /// </summary>
    internal void JustLoadedShowCurrent()
    {
        _wasLoaded = true;
    }


    public void TutoCallWhenDone()
    {
        _lastCompleted = Time.time;
    }






}






public class Quest
{
    string _key;

    public string Key
    {
        get { return _key; }
        set { _key = value; }
    }
    float _prize;

    public float Prize
    {
        get { return _prize; }
        set { _prize = value; }
    }
    float _secWait;

    public float SecWait
    {
        get { return _secWait; }
        set { _secWait = value; }
    }


    float _progress;
    /// <summary>
    /// How far the quest has progressed. This will be used for ex to see how much 
    /// money is being gained trhu exports until its goal is met, then the quest is completed 
    /// </summary>
    public float Progress
    {
        get { return _progress; }
        set { _progress = value; }
    }

    float _goal;
    /// <summary>
    /// The goal to reach for a quest. Used for ex when a $100 bucks are needed be gained to finish a quest
    /// this will hold that Goal value
    /// </summary>
    public float Goal
    {
        get { return _goal; }
        set { _goal = value; }
    }

    public Quest() { }

    public Quest(string key, float prize, float secWait, float goal = -1)
    {
        _key = key;
        _prize = prize;
        _secWait = secWait;
        _goal = goal;
    }

    public void AddToProgress(float add)
    {
        Progress += add;
    }

    public bool IsQuestCompleted()
    {
        if (_goal == -1)
        {
            return true;
        }
        return Progress >= Goal;
    }


    internal bool IsAPercetangeOne()
    {
        return _goal != -1;
    }

    /// <summary>
    /// Not formated: pls add ToString("0%") for string
    /// </summary>
    /// <returns></returns>
    internal float PercetageDone()
    {
        return _progress / _goal;
    }
}