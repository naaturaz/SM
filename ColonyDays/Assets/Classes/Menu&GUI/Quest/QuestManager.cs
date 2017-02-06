﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class QuestManager
{
    int _current = -1;

    float _lastCompleted;
    bool _showed;

    QuestWindow _questWindow;

    List<int> _currentQuests = new List<int>();
    List<int> _doneQuest = new List<int>();

    bool _wasLoaded;//whe is a loaded game

    public int CurrentQuest
    {
        get { return _current; }
        set { _current = value; }
    }

    public bool Showed
    {
        get { return _showed; }
        set { _showed = value; }
    }

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




    List<Quest> _bank = new List<Quest>()
    { 
        //need to mention reward still 
        new Quest("Bohio.Quest", 500, 5.5f),
        new Quest("SmallFarm.Quest", 550, 5.5f),
        new Quest("FarmHire.Quest", 600, 5.1f),
        new Quest("FarmProduce.Quest", 650, 5.1f, 100),
        //new Quest("Transport.Quest", 700, 5.5f),
        new Quest("Export.Quest", 750, 5.5f),
        new Quest("HireDocker.Quest", 800, 5.5f),
        new Quest("MakeBucks.Quest", 850, 5.5f, 100),
        new Quest("HeavyLoad.Quest", 900, 5.5f),

    };

    public QuestManager() { }


    public void QuestFinished(string which)
    {
        //wont complete a quest if is not shown yet
        if (!Showed || CurrentQuest >= _bank.Count)//bz Building.cs calls like 5 times when is done 
        {
            return;
        }

        if (_bank[CurrentQuest].Key == which + ".Quest")
        {
            Show_currentPrize();

            Program.gameScene.GameController1.Dollars += _bank[CurrentQuest].Prize;
            AudioCollector.PlayOneShot("BoughtLand", 0);

            _questWindow.RemoveAQuest(_bank[CurrentQuest]);
            _currentQuests.Remove(CurrentQuest);//remove from _current list
            _doneQuest.Add(CurrentQuest);//adds to done list 

            CurrentQuest++;
            //adds to _current list
            if (CurrentQuest < _bank.Count)
            {
                _currentQuests.Add(CurrentQuest);
            }
        }
    }

    /// <summary>
    /// Some quest need progess and here is where it gets reported 
    /// </summary>
    /// <param name="which"></param>
    /// <param name="amt"></param>
    public void AddToQuest(string which, float amt)
    {
        //wont complete a quest if is not shown yet
        if (!Showed || CurrentQuest >= _bank.Count)//bz Building.cs calls like 5 times when is done 
        {
            return;
        }

        if (_bank[CurrentQuest].Key == which + ".Quest")
        {
            //adds
            _bank[CurrentQuest].AddToProgress(amt);
            //checks if is completed
            if (_bank[CurrentQuest].IsQuestCompleted())
            {
                QuestFinished(which);
            }
        }
    }

    void ShowCurrentQuest()
    {
        if (_questWindow == null)
        {
            _questWindow = MonoBehaviour.FindObjectOfType<QuestWindow>();
        }
        if (_questWindow == null || CurrentQuest < 0)
        {
            return;
        }

        Showed = true;
        _questWindow.SetAQuest(_bank.ElementAt(CurrentQuest));

        if (_wasLoaded)
        {
            _wasLoaded = false;
            return;
        }

        SpawnDialog(_bank.ElementAt(CurrentQuest).Key);
    }

    public void SpawnDialog(string which)
    {
        //spawn dialog 
        Dialog.OKDialog(H.InfoKey, which);
    }

    void Show_currentPrize()
    {
        Dialog.OKDialog(H.CompleteQuest, _bank[CurrentQuest].Prize + "");
    }

    public void QuestCompletedAcknowled()
    {
        _lastCompleted = Time.time;
        Showed = false;

        if (CurrentQuest >= _bank.Count)
        {
            _lastCompleted = -1;
            CurrentQuest = -1;
        }
    }

    public void Update()
    {
        if (Program.MouseListener.IsAWindowShownNow() || CamControl.IsMainMenuOn() ||
            CurrentQuest >= _bank.Count || _lastCompleted < 0)
        {
            return;
        }

        if (Program.gameScene.IsPassingTheTutoNow())
        {
            return;
        }

        //starts up
        if (CurrentQuest == -1 && _lastCompleted == 0 && !Showed && Program.gameScene.GameWasFullyLoadedAnd10SecAgo())
        {
            CurrentQuest = 0;
            ShowCurrentQuest();
        }

        //to show  others  and loaded 
        if (Time.time > _lastCompleted + _bank[CurrentQuest].SecWait && !Showed)
        {
            ShowCurrentQuest();
        }
    }

    public void ResetNewGame()
    {
        _lastCompleted = 0;
        Showed = false;
        CurrentQuest = 0;
    }


    /// <summary>
    /// Called right after loaded meant to show current 
    /// </summary>
    internal void JustLoadedShowCurrent()
    {
        _wasLoaded = true;
        Showed = false;
    }

    internal bool IsQuestingNow()
    {
        return CurrentQuest != -1;
    }

    public void TutoCallWhenDone()
    {
        CurrentQuest = 0;
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

    internal float PercetageDone()
    {
        return _progress / _goal;
    }
}