using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class FinalReport
{
    public static void FinishReport(string addName = "")
    {
        Dialog.CreateFile(addName + "Input", CreateFinalReportNow());
    }

    private static string CreateFinalReportNow()
    {
        return "People: " + PersonPot.Control.All.Count + "\n" +
               "Emigrate: " + PersonPot.Control.EmigrateController1.Emigrates.Count + "\n" +
               "Food: " + GameController.ResumenInventory1.ReturnAmountOnCategory(PCat.Food) + " kg \n" +
               "Happy: " + PersonPot.Control.OverAllHappiness() + "\n" +
               "PortRep: " + BuildingPot.Control.DockManager1.PortReputation.ToString("F1") + "\n" +
               "PirateThr: " + BuildingPot.Control.DockManager1.PirateThreat.ToString("F1") + "\n" +
               "Dollar: " + Program.gameScene.GameController1.Dollars.ToString("C0") + "\n" +
               "Buildings: " + BuildingPot.Control.Registro.AllBuilding.Count + "\n" +

               "Time: " + Time.time + "\n";
            
    }
}

