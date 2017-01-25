using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class QuestManager
{
    static int _current;
    static float _lastCompleted;
    static bool _showed;
    static QuestWindow _questWindow;

    static List<Quest> _bank = new List<Quest>()
    { 
        //need to mention reward still 
        new Quest("Bohio.Quest", 500, 7),
        new Quest("SmallFarm.Quest", 550, 7),
        new Quest("FarmHire.Quest", 600, 5),
        new Quest("FarmProduce.Quest", 650, 7, 300),
        new Quest("Transport.Quest", 700, 7),
        new Quest("Export.Quest", 750, 7),
        new Quest("HireDocker.Quest", 800, 7),
        new Quest("Make100Bucks.Quest", 850, 7, 100),
    };

    public static void QuestFinished(string which)
    {
        //wont complete a quest if is not shown yet
        if (!_showed || _current >= _bank.Count)//bz Building.cs calls like 5 times when is done 
        {
            return;
        }

        if (_bank[_current].Key == which + ".Quest")
        {
            ShowCurrentPrize();

            Program.gameScene.GameController1.Dollars += _bank[_current].Prize;
            AudioCollector.PlayOneShot("BoughtLand", 0);

            _questWindow.RemoveAQuest(which);
            _current++;
        }
    }

    /// <summary>
    /// Some quest need progess and here is where it gets reported 
    /// </summary>
    /// <param name="which"></param>
    /// <param name="amt"></param>
    public static void AddToQuest(string which, float amt)
    {
        //wont complete a quest if is not shown yet
        if (!_showed || _current >= _bank.Count)//bz Building.cs calls like 5 times when is done 
        {
            return;
        }

        if (_bank[_current].Key == which + ".Quest")
        {
            //adds
            _bank[_current].AddToProgress(amt);
            //checks if is completed
            if (_bank[_current].IsQuestCompleted())
            {
                QuestFinished(which);
            }
        }
    }

    static void ShowCurrentQuest()
    {
        if (_questWindow == null)
        {
            _questWindow = MonoBehaviour.FindObjectOfType<QuestWindow>();
        }
        if (_questWindow == null || _current < 0)
        {
            return;
        }

        _showed = true;
        _questWindow.SetAQuest(_bank.ElementAt(_current).Key);

        SpawnDialog(_bank.ElementAt(_current).Key);
    }

    public static void SpawnDialog(string which)
    {
        //spawn dialog 
        Dialog.OKDialog(H.InfoKey, which);
    }

    static void ShowCurrentPrize()
    {
        Dialog.OKDialog(H.CompleteQuest, _bank[_current].Prize + "");
    }

    public static void QuestCompletedAcknowled()
    {
        _lastCompleted = Time.time;
        _showed = false;

        if (_current >= _bank.Count)
        {
            _lastCompleted = -1;
            _current = -1;
        }
    }

    public static void Update()
    {
        //return;

        if (_current < 0 || _current >= _bank.Count || _lastCompleted < 0)
        {
            return;
        }

        if (Program.gameScene.IsPassingTheTutoNow())
        {
            return;
        }

        if (_lastCompleted == 0 && !_showed && Program.gameScene.GameWasFullyLoadedAnd10SecAgo())
        {
            ShowCurrentQuest();
        }

        if (Time.time > _lastCompleted + _bank[_current].SecWait && !_showed)
        {
            ShowCurrentQuest();
        }
    }

    public static void ResetNewGame()
    {
        _lastCompleted = 0;
        _showed = false;
        _current = 0;
    }

}

class Quest
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

}