using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MyText : MonoBehaviour
{
    private bool mappedOnce;
    private Text thisText;

    // Use this for initialization
    void Start()
    {
        thisText = GetComponent<Text>();
        Map();

        StartCoroutine("FiveSecUpdate");
    }

    private IEnumerator FiveSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(5); // wait

            if (name == "Loaded")
            {
                thisText.text = Program.gameScene.controllerMain.TerraSpawnController.PercentageLoaded();
            }
        }
    }

    int adult;
    private void Map()
    {
        if (Program.InputMain == null)
        {
            return;
        }

        if (name == "Version")
        {
            thisText.text = GameScene.VersionLoaded();
            mappedOnce = true;
        }

        if (!Program.InputMain.IsGameFullyLoaded() || !Program.gameScene.GameFullyLoaded())
        {
            return;
        }

        mappedOnce = true;

        if (name == "CurrSpeed")
        {
            thisText.text = Program.gameScene.GameSpeed + "x";
        }
        else if (name == "Person")
        {
            adult = PersonPot.Control.All.Count(a => a.Age >= JobManager.majorityAge);
            var all = PersonPot.Control.All.Count;

            thisText.text = all + "/" + adult + "/" + (all - adult);
        }
        else if (name == "Emigrate")
        {
            thisText.text = PersonPot.Control.EmigrateController1.Emigrates.Count + "";
        }
        else if (name == "Food")
        {
            var amt = GameController.ResumenInventory1.ReturnAmountOnCategory(PCat.Food);

            thisText.text = Unit.WeightConverted(amt).ToString("N0") + " " +
                Unit.WeightUnit();
        }
        else if (name == "Happy")
        {
            thisText.text = PersonPot.Control.OverAllHappiness();
        }

        else if (name == "PortReputation")
        {
            thisText.text = BuildingPot.Control.DockManager1.PortReputation.ToString("F0");
        }
        else if (name == "PirateThreat")
        {
            if (!Program.IsPirate)
            {
                thisText.text = "-";
                return;
            }

            thisText.text = BuildingPot.Control.DockManager1.PirateThreat.ToString("F0");
        }

        else if (name == "Dollars")
        {
            thisText.text = DollarFormat(Program.gameScene.GameController1.Dollars);
        }

        else if (name == "Temp")
        {
            thisText.text = Tempeture.Current().ToString("n0");
        }
        else if (name == "Town")
        {
            thisText.text = Program.MyScreen1.TownName;
        }
    }


    public static string DollarFormat(float amt)
    {
        return Sign(amt) + "$" + String.Format("{0:n}", Math.Abs(amt));
    }

    static string Sign(float amt)
    {
        if (amt < 0)
        {
            return "-";
        }
        return "";
    }


    public static int Lazy()
    {
        return PersonPot.Control.All.Count(a => a.Age >= JobManager.majorityAge)
            - BuildingPot.Control.Registro.MaxPositions();
    }





    private static int reMapCount;//since is static need to remap all the times exist MyText.cs scripts

    // Update is called once per frame
    void Update()
    {
        reMapCount++;

        if (reMapCount > 60)//180
        {
            reMapCount = 0;
            Map();
        }

        if (_speedChanged && name == "CurrSpeed")
        {
            _speedChanged = false;
            thisText.text = Program.gameScene.GameSpeed + "x";
        }

        if (!mappedOnce)
        {
            Map();
        }

        if (reMapCount % 2 != 0)
        {
            return;
        }


        if (name == "Date")
        {
            thisText.text = Program.gameScene.GameTime1.MonthFormat() + " "
                + Program.gameScene.GameTime1.Year;
        }
        else if (name == "Lazy")
        {
            thisText.text = Lazy() + "";
        }
    }

    private static bool _speedChanged;
    public static void UpdateNow()
    {
        _speedChanged = true;
    }


}
