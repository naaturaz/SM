using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;


public class ManagerReport
{
    static float _lastFPS;
    private static FinalReport _finalReport;

    public static void Start()
    {
        _finalReport = new FinalReport();
    }

    public static void Update()
    {
        //every 60sec
        if (Time.time > _lastFPS + 60f)
        {
            _finalReport.AddFPS(HUDFPS.FPS());
            _lastFPS = Time.time;
        }
    }

    /// <summary>
    /// To be called when game is loaded 
    /// </summary>
    public static void AddFPS()
    {
        _finalReport.AddFPS(HUDFPS.FPS());
    }

    public static void AddInput(string a)
    {
        _finalReport.AddInput(a);
    }

    public static void FinishAllReports()
    {
        //avoiding if was closed game even before loaded a map 
        if (!Program.gameScene.GameFullyLoaded())
        {
            return;
        }

        _finalReport.FinishReport();
    }


    internal static void FinishAllReports(string p)
    {
        //avoiding be sent in the first time 
        if (!Program.gameScene.GameFullyLoaded())
        {
            return;
        }

        _finalReport.FinishReport(p);
    }

    internal static void AddNewSpeed(int p)
    {
        _finalReport.AddSpeed(p);

    }

    /// <summary>
    /// Will find if FeedBack, BugReport or Invitation
    /// </summary>
    /// <param name="type"></param>
    /// <param name="text"></param>
    internal static void AddNewText(string type, string text)
    {
        if (type == "Feedback" || type == "OptionalFeedback")
        {
            _finalReport.AddToFeedBack(text);
        }
        else if (type == "BugReport")
        {
            _finalReport.AddToBugReport(text);
            
        }
        else if (type == "Invitation")
        {
            _finalReport.AddToInvitation(text);
            
        }
    }
}

