using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;

/// <summary>
/// This manages all notifications of the game 
/// </summary>
public class NotificationsManager
{
    /// <summary>
    /// Some notifications repeat like full storage or cant produce bz full storage they will repeat every:
    /// </summary>
    static float _notiFrec = 120;

    public static float NotiFrec
    {
        get { return NotificationsManager._notiFrec; }
        set { NotificationsManager._notiFrec = value; }
    }

    //set of notifications to uncheck on Options
    private bool _pirates;
    private float _lowPirate10;//the low 10%, means if im in 25% this is going to be 20%
    private float _highPirate10 = 10;//the high 10%, means if im in 25% this is going to be 30%
    private float _lastPirate;

    //the time willwait until notify again 
    private float _coolDown = 20;//seconds

    private bool _reputation;
    private bool _stillNeedInit;

    private MainNotificationGO _mainNotificationGo;

    public NotificationsManager()
    {
        Init();
    }

    private void Init()
    {
        if (BuildingPot.Control.DockManager1 == null || !Program.gameScene.GameFullyLoaded())
        {
            _stillNeedInit = true;
            return;
        }
        _stillNeedInit = false;
        FindEnds(BuildingPot.Control.DockManager1.PirateThreat, out _lowPirate10, out _highPirate10);

    }

    private void FindEnds(float val, out float low, out float high)
    {
        if (val < 10)
        {
            low = 0;
            high = 10;
        }
        else
        {
            var firstDigit = val.ToString().Substring(0, 1);
            low = float.Parse(firstDigit + 0);
            high = float.Parse(firstDigit + 0) + 10;
        }
    }


    public void UpdateOneSecond()
    {
        if (_stillNeedInit)
        {
            Init();
        }

        CheckPirate();
    }

    private void CheckPirate()
    {
        var newLow = 0f;
        var newHi = 0f;

        FindEnds(BuildingPot.Control.DockManager1.PirateThreat, out newLow, out newHi);

        if (newLow > _lowPirate10)
        {
            Notify("PirateUp");
        }
        else if (newLow < _lowPirate10)
        {
            //went down 10
            Notify("PirateDown");

        }
        _lowPirate10 = newLow;
        _highPirate10 = newHi;
    }

    public void Notify(string which)
    {
        if (Program.MouseListener.NotificationWindow == null || Program.MyScreen1.IsMainMenuOn())
        {
            return;
        }

        Program.MouseListener.NotificationWindow.Notify(which);
    }

    /// <summary>
    /// When something needs to be specify
    /// </summary>
    /// <param name="which"></param>
    /// <param name="addP"></param>
    internal void Notify(string which, string addP)
    {
        if (Program.MouseListener.NotificationWindow == null || Program.MyScreen1.IsMainMenuOn())
        {
            return;
        }

        Program.MouseListener.NotificationWindow.Notify(which, addP);
    }



    public void MainNotify(string which)
    {
        if (_mainNotificationGo == null)
        {
            _mainNotificationGo = GameObject.FindObjectOfType<MainNotificationGO>();
        }

        _mainNotificationGo.Show(which);
    }


    internal void HideMainNotify()
    {
        if (_mainNotificationGo == null)
        {
            _mainNotificationGo = GameObject.FindObjectOfType<MainNotificationGO>();
        }
        _mainNotificationGo.Hide();

    }


}
