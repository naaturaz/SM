using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ManagerReport
{
    static float _lastFPS;

    public static void Start()
    {
        
    }

    public static void Update()
    {
        //every 60sec
        if (Time.time > _lastFPS + 60f)
        {
            FPSReport.Add(HUDFPS.FPS()+"");
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
        FinalReport.FinishReport();
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
        FinalReport.FinishReport(p);
    }
}

