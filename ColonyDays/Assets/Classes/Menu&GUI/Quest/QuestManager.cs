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

    static Dictionary<string, float> _bank = new Dictionary<string, float>()
    { 
        {"Tutorial.Quest", 10000},
        {"SmallFarm.Quest", 5000},
        {"Bohio.Quest", 2000},
    };

    public static void QuestFinished(string which)
    {
        //wont complete a quest if is not shown yet
        if (!_showed || _current >= _bank.Count)//bz Building.cs calls like 5 times when is done 
        {
            return;
        }

        if (_bank.ElementAt(_current).Key == which + ".Quest")
        {
            ShowCurrentPrize();

            Program.gameScene.GameController1.Dollars += _bank.ElementAt(_current).Value;
            AudioCollector.PlayOneShot("BoughtLand", 0);

            _questWindow.RemoveAQuest(which);
            _current++;
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
        Dialog.OKDialog(H.CompleteQuest, _bank.ElementAt(_current).Value+"");
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
        if (_lastCompleted < 0)
        {
            return;
        }

        if (_lastCompleted == 0 && !_showed && Program.gameScene.GameWasFullyLoadedAnd10SecAgo())
        {
            ShowCurrentQuest();
        }

        if (Time.time > _lastCompleted + 5 && !_showed)
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

