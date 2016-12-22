using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class FinalReport
{
    public static  string Separator = "  |  ";
    public  int People;
    public  MDate GameDate;
    public  int Emigrate;
    public  float Food;
    public  string Happy;
    public  float PortRep;
    public  float PirateThr;
    public  float Dollar;
    public  List<string> Buildings;

    public List<string> FeedBack = new List<string>();
    public List<string> BugReport = new List<string>();
    public List<string> Invitation = new List<string>();
    
    public  List<string> Inputs = new List<string>();
    public int TtlInputs;  
    
    public  List<float> FPS = new List<float>();
    public int TtlFPS;
    public float AverageFPS;

    public List<int> Speed = new List<int>();
    public List<string> SpeedInfo = new List<string>();
    public float AverageSpeed;
    
    public  float TimeSec;
    public  float TimeMin;

    public int Difficulty;
    public string ScreenSize;
    public int InitialRegionIndex;
    public H TypeOfGame;

    public  void FinishReport(string addName = "")
    {
        GatherReport();
      //  var file = XMLSerie.WriteXMLFinalReport(this);
        Dialog.UploadXMLFile(addName + "Final", this);
    }

    private  void GatherReport()
    {
        People = PersonPot.Control.All.Count;
        GameDate = Program.gameScene.GameTime1.CurrentDate();
        Emigrate = PersonPot.Control.EmigrateController1.Emigrates.Count;
        Food =GameController.ResumenInventory1.ReturnAmountOnCategory(PCat.Food);
        Happy = PersonPot.Control.OverAllHappiness();
        PortRep = BuildingPot.Control.DockManager1.PortReputation;
        PirateThr = BuildingPot.Control.DockManager1.PirateThreat;
        Dollar = Program.gameScene.GameController1.Dollars;
        Buildings = BuildingPot.Control.Registro.StringOfAllBuildings();

        //fps average
        var ttlTemp = 0f;
        for (int i = 0; i < FPS.Count; i++)
        {
            ttlTemp += FPS[i];
        }
        AverageFPS = ttlTemp/FPS.Count;

        TimeSec = Time.time;
        TimeMin = Time.time/60;

        // average
        var tempSpeedTtl = 0;
        for (int i = 0; i < Speed.Count; i++)
        {
            tempSpeedTtl += Speed[i];
        }
        AverageSpeed = (float)tempSpeedTtl/Speed.Count;

        Difficulty = PersonPot.Control.Difficulty;
        ScreenSize = "Screen: Height: " + Screen.height + ". Width" + Screen.width;
        InitialRegionIndex = MeshController.CrystalManager1.InitialRegionIndex;
        TypeOfGame = Program.TypeOfGame;
    }

    public void AddInput(string add)
    {
        Inputs.Add(add);
        TtlInputs++;
    }

    public void AddFPS(float add)
    {
       FPS.Add(add) ;
        TtlFPS++;
    }

    internal void AddSpeed(int p)
    {
        Speed.Add(p);
        SpeedInfo.Add(Time.time + " > " + p);
    }

    internal void AddToFeedBack(string text)
    {
        FeedBack.Add(text);
    }

    internal void AddToBugReport(string text)
    {
        BugReport.Add(text);
    }

    internal void AddToInvitation(string text)
    {
        Invitation.Add(text);
    }
}

