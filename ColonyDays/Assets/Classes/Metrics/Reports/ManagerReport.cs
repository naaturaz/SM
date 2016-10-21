using System;
using System.Collections.Generic;
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
            FPSReport.Add(HUDFPS.FPS().ToString("n0"));
            _lastFPS = Time.time;
        }
    }

    public static void FinishAllReports()
    {
        //avoiding if was closed game even before loaded a map 
        if (!Program.gameScene.GameFullyLoaded())
        {
            return;
        }

        InputReport.FinishReport();
        FPSReport.FinishReport();
        _finalReport.FinishReport();
    }


    internal static void FinishAllReports(string p)
    {
        //avoiding be sent in the first time 
        if (!Program.gameScene.GameFullyLoaded())
        {
            return;
        }

        InputReport.FinishReport(p);
        FPSReport.FinishReport(p);
        _finalReport.FinishReport(p);
    }
}

