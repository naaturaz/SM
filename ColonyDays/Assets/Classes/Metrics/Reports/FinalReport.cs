using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class FinalReport
{
    public static string Separator = "  |  ";

    public static void FinishReport(string addName = "")
    {
        Dialog.CreateFile(addName + "Final", CreateFinalReportNow());
    }

    private static string CreateFinalReportNow()
    {
        return "People: " + PersonPot.Control.All.Count + Separator +
               "Emigrate: " + PersonPot.Control.EmigrateController1.Emigrates.Count + Separator +
               "Food: " + GameController.ResumenInventory1.ReturnAmountOnCategory(PCat.Food) + " kg \n" +
               "Happy: " + PersonPot.Control.OverAllHappiness() + Separator +
               "PortRep: " + BuildingPot.Control.DockManager1.PortReputation.ToString("F1") + Separator +
               "PirateThr: " + BuildingPot.Control.DockManager1.PirateThreat.ToString("F1") + Separator +
               "Dollar: " + Program.gameScene.GameController1.Dollars.ToString("C0") + Separator +
               "Buildings: " + BuildingPot.Control.Registro.AllBuilding.Count + Separator +
               "List of Buildings: " + BuildingPot.Control.Registro.StringOfAllBuildings() + Separator +

               "Time Sec: " + Time.time + Separator +
               "Time Min: " + Time.time/60 + Separator;


            
    }
}

