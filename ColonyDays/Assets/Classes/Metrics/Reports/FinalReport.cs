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
    
    public  List<string> Inputs = new List<string>();
    public int TtlInputs;  
    
    public  List<float> FPS = new List<float>();
    public int TtlFPS;
    public float AverageFPS;
    
    public  float TimeSec;
    public  float TimeMin;
    



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
        //

        TimeSec = Time.time;
        TimeMin = Time.time/60;

        //
        //var a = DateTime.Parse(TimeSec + "");
        //TimeParsed = a+"";
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
}

