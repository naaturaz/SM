/*
 * This is a really omportatn class. This is the one the control the whole game in sense of inventory.
 * Holds the inventory of the game
 * 
 * 
 * Controls the game in the sense too which Scene with BUilding with Person Load.
 * Or create new game 
 */
public class GameController  {

    //Main inventory of the game .. wht u see on the GUI 
    //will have all tht is in all Storages combined 
    //is a total inventory. Representing all tht is in those inventories 
    static ResumenInventory _inventory = new ResumenInventory();

    private static int _dollars = 100000;//the dollars the player has 
    private static StartingCondition _startingCondition;

    static public ResumenInventory Inventory1
    {
        get { return _inventory; }
        set
        {
            
            _inventory = value;
        }
    }

    public static int Dollars
    {
        get { return _dollars; }
        set
        {
            MyText.ManualUpdate();
            _dollars = value;
        }
    }



    static public void LoadStartingConditions(StartingCondition startingCondition)
    {
        _startingCondition = startingCondition;

        //if (!Inventory1.IsEmpty())// the inventory here is not empty means was loaded already)
        //{
        //    return;
        //}
        

        //var inv = CreateInitialInv(startingCondition);

        //LoadIntoInv(inv);
    }

    static Inventory CreateInitialInv(StartingCondition startingCondition)
    {
        Dollars += startingCondition.iniDollar;
        Inventory inv = new Inventory();

        inv.Add(P.Wood, startingCondition.iniWood);
        inv.Add(P.Food, startingCondition.iniFood);

        inv.Add(P.Stone, startingCondition.iniStone);
        inv.Add(P.Brick, startingCondition.iniBrick);
        inv.Add(P.Iron, startingCondition.iniIron);

        inv.Add(P.Gold, startingCondition.iniGold);

        return inv;
    }

    /// <summary>
    /// Will load the Starting conditions into the 1st Storage Type of building.
    /// This is done only at first when game is created
    /// </summary>
    static void LoadIntoInv(Inventory invt)
    {
        if (BuildingPot.Control.FoodSources.Count == 0 )
        {
            return;
        }

        var key = BuildingPot.Control.FoodSources[0];

        var storage = Brain.GetStructureFromKey(key);

        storage.Inventory.AddItems(invt.InventItems);
    }

    /// <summary>
    /// The first Storage will call this so the first Lote is get into there 
    /// </summary>
    static public void SetInitialLote()
    {
        var inv = CreateInitialInv(_startingCondition);
        LoadIntoInv(inv);
    }
}
