using UnityEngine;
using UnityEngine.UI;

public class MyText : MonoBehaviour
{
    private bool mappedOnce;
    private Text thisText;

	// Use this for initialization
	void Start ()
	{
	    thisText = GetComponent<Text>();

	    Map();
	}

    private void Map()
    {
        if (Program.InputMain == null || !Program.InputMain.IsGameFullyLoaded())
        {
            return;
        }

        mappedOnce = true;

        if (name == "Person")
        {
            thisText.text = PersonPot.Control.All.Count + "";
        }
        if (name == "Food")
        {
            thisText.text = GameController.Inventory1.ReturnAmountOnCategory(PCat.Food) + "";
        }
        if (name == "Wood")
        {
            thisText.text = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Wood) + "";
        }
        if (name == "Stone")
        {
            thisText.text = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Stone) + "";
        }
        if (name == "Brick")
        {
            thisText.text = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Brick) + "";
        }
        if (name == "Iron")
        {
            thisText.text = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Iron) + "";
        }
        if (name == "Gold")
        {
            thisText.text = GameController.Inventory1.ReturnAmtOfItemOnInv(P.Gold) + "";
        }
        if (name == "Happy")
        {
            thisText.text = PersonPot.Control.OverAllHappiness();
        }

        if (name == "Dollars")
        {
            thisText.text = GameController.Dollars.ToString("C0");
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (reMap)
	    {
            //9 the amount of MyText.cs scripts
	        if (reMapCount > 9)
	        {
                reMap = false;
	            reMapCount = 0;
                return;
	        }
            
            Map();
	        reMapCount++;
	    }

	    if (!mappedOnce)
	    {
	        Map();
	    }

        if (name == "Date")
        {
            thisText.text = Program.gameScene.GameTime1.Day + " "
                + Program.gameScene.GameTime1.MonthFormat() + " "
                + Program.gameScene.GameTime1.Year;
        }
	}

    private static bool reMap;
    private static int reMapCount;//since is static need to remap all the times exist MyText.cs scripts
    /// <summary>
    /// Everytime something change on inventroy on GameController should call this
    /// </summary>
    public static void ManualUpdate()
    {
        reMap = true;
    }
}
