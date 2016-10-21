using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class FinalReport
{
    public static  string Separator = "  |  ";
    public  int People;
    public  int Emigrate;
    public  float Food;
    public  string Happy;
    public  float PortRep;
    public  float PirateThr;
    public  float Dollar;
    public  List<string> Buildings;
    public  float TimeSec;

    public  void FinishReport(string addName = "")
    {
        GatherReport();
      //  var file = XMLSerie.WriteXMLFinalReport(this);
        Dialog.UploadXMLFile(addName + "Final", this);
    }

    private  void GatherReport()
    {
        People = PersonPot.Control.All.Count;
        Emigrate = PersonPot.Control.EmigrateController1.Emigrates.Count;
        Food =GameController.ResumenInventory1.ReturnAmountOnCategory(PCat.Food);
        Happy = PersonPot.Control.OverAllHappiness();
        PortRep = BuildingPot.Control.DockManager1.PortReputation;
        PirateThr = BuildingPot.Control.DockManager1.PirateThreat;
        Dollar = Program.gameScene.GameController1.Dollars;
        Buildings = BuildingPot.Control.Registro.StringOfAllBuildings();
        TimeSec = Time.time;
    }


}

