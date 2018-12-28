using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Languages
{
    private static string _currentLang = "English";
    private static LangDict _sp = new LangDict();
    private static LangDict _en = new LangDict();
    private static Dictionary<string, string> _portuguese = new Dictionary<string, string>();

    /// <summary>
    /// Will be called when a System Units is changed 
    /// </summary>
    public static void ReloadDict()
    {
        string _houseTail = ". SugarMiller's live here and enjoy having a nice meal at least once in a while";
        string _animalFarmTail = ", you can raise different animals in this building";
        string _fieldFarmTail = ", you can plant different crops and fruits in this building";
        string _asLongHasInput = ", as long as it has the necessary inputs";
        string _produce = "In this building workers will produce the selected product, as long as it has the necessary inputs";
        string _storage =
        "This is a Storage building. If it gets full people won't work as they won't have where to store their products";
        string _militar = "This building helps to decrease the Pirate Threat on your port, to be effective it must have workers. The more workers the better";
        string _notRegionNeeded = " Can be built without owning the region.";

        _en = new LangDict();


        //Descriptions
        //Infr
        _en.Add("Road.Desc", "Used for decoration purposes. People are happier if there are roads around them");
        _en.Add("BridgeTrail.Desc", "Allows people to pass from one side of the terrain to the other");
        _en.Add("BridgeRoad.Desc", "Allows people to pass from one side of the terrain to the other. People love these bridges. It gives a sense of prosperity and happiness" + _houseTail);
        _en.Add("LightHouse.Desc", "Helps to increase Port visibility. Adds to Port Reputation as long it has workers in it");
        _en.Add(H.Masonry + ".Desc", "Important building, workers construct new buildings and work as wheelbarrows once they have nothing to do");
        _en.Add(H.StandLamp + ".Desc", "Illuminates at night if Whale Oil is available on the town's storage");

        _en.Add(H.HeavyLoad + ".Desc", "These workers use horse wagons to move goods around");


        //House
        _en.Add("Bohio.Desc", "Bohio house, primitive conditions with unhappy people whom can only have a maximum of 2 to 3 kids");

        _en.Add("Shack.Desc", "Shack, primitive conditions with unhappy people whom can only have a maximum of 2 kids");
        _en.Add("MediumShack.Desc", "The Medium Shack, has above primitive conditions with small happiness, can have a maximum of 2 to 3 kids");
        _en.Add("LargeShack.Desc", "A Large Shack, has somewhat good conditions with happiness, can have a maximum of 2 to 4 kids");


        _en.Add("WoodHouseA.Desc", "Medium wooden house, a family can have 2-3 kids max");
        _en.Add("WoodHouseB.Desc", "Medium wooden house, a family can have 3-4 kids max");
        _en.Add("WoodHouseC.Desc", "Medium wooden house, a family can have 2-3 kids max");
        _en.Add("BrickHouseA.Desc", "Medium house, a family can have 3 kids max");
        _en.Add("BrickHouseB.Desc", "Large house, a family can have 3-4 kids max");
        _en.Add("BrickHouseC.Desc", "Large house, a family can have 4 kids max");


        //Farms
        //Animal
        _en.Add("AnimalFarmSmall.Desc", "Small animal farm" + _animalFarmTail);
        _en.Add("AnimalFarmMed.Desc", "Medium animal farm" + _animalFarmTail);
        _en.Add("AnimalFarmLarge.Desc", "Large animal farm" + _animalFarmTail);
        _en.Add("AnimalFarmXLarge.Desc", "Extra large animal farm" + _animalFarmTail);
        //Fields
        _en.Add("FieldFarmSmall.Desc", "Small field farm" + _fieldFarmTail);
        _en.Add("FieldFarmMed.Desc", "Medium field farm" + _fieldFarmTail);
        _en.Add("FieldFarmLarge.Desc", "Large field farm" + _fieldFarmTail);
        _en.Add("FieldFarmXLarge.Desc", "Extra large field farm" + _fieldFarmTail);
        _en.Add(H.FishingHut + ".Desc", "Here a worker can catch fish in a river (must be place by the river)." + _notRegionNeeded);

        //Raw
        _en.Add("Mortar.Desc", "Here a worker will produce Mortar");
        _en.Add("Clay.Desc", "Here a worker will produce Clay, raw material is needed for bricks and more");
        _en.Add("Pottery.Desc", "Here a worker will produce crockery products, such as dishes, jars, etc");
        _en.Add("Mine.Desc", "Here a worker can fish in a river");
        _en.Add("MountainMine.Desc", "Here a worker will work the mine by extracting ore");
        _en.Add("Resin.Desc", "Here a worker will work the mine by extracting minerals and metals randomly");
        _en.Add(H.LumberMill + ".Desc", "Here workers will find resources such as wood, stone, and ore");
        _en.Add("BlackSmith.Desc", "Here workers will produce the product selected" + _asLongHasInput);
        _en.Add("ShoreMine.Desc", "Here workers will produce salt and sand");
        _en.Add("QuickLime.Desc", "Here workers will produce quicklime");

        //Prod
        _en.Add("Brick.Desc", "Here a worker will produce clay made products, such as bricks, etc");
        _en.Add("Carpentry.Desc", "Here a worker will produce wood made products, such as crates, barrels, etc");
        _en.Add("Cigars.Desc", "Here workers will produce cigars" + _asLongHasInput);
        _en.Add("Mill.Desc", "Here workers will ground some grains" + _asLongHasInput);
        _en.Add(H.Tailor + ".Desc", "Here workers will produce clothes" + _asLongHasInput);
        _en.Add("Tilery.Desc", "Here workers will produce roof tiles" + _asLongHasInput);
        _en.Add("Armory.Desc", "Here workers will produce weapons" + _asLongHasInput);
        _en.Add(H.Distillery + ".Desc", _produce);
        _en.Add("Chocolate.Desc", _produce);
        _en.Add("Ink.Desc", _produce);

        //Ind
        _en.Add("Cloth.Desc", _produce);
        _en.Add("GunPowder.Desc", _produce);
        _en.Add("PaperMill.Desc", _produce);
        _en.Add("Printer.Desc", _produce);
        _en.Add("CoinStamp.Desc", _produce);
        _en.Add("Silk.Desc", _produce);
        _en.Add("SugarMill.Desc", _produce);
        _en.Add("Foundry.Desc", _produce);
        _en.Add("SugarShop.Desc", "Produces sugar subproducts!!!. " + _produce);


        _en.Add("SteelFoundry.Desc", _produce);

        //trade
        _en.Add("Dock.Desc", "Here you can add import or export orders (must be placed by the ocean)." + _notRegionNeeded);
        _en.Add(H.Shipyard + ".Desc", "You can repairs your ships here, but you must have ship repair materials in inventory");
        _en.Add("Supplier.Desc", "You can supply ships with goods here, but you must have items in inventory that a ship can use for their long trip");
        _en.Add("StorageSmall.Desc", _storage);
        _en.Add("StorageMed.Desc", _storage);
        _en.Add("StorageBig.Desc", _storage);
        _en.Add("StorageBigTwoDoors.Desc", _storage);
        _en.Add("StorageExtraBig.Desc", _storage);

        //gov
        _en.Add("Library.Desc", "People come to this building to read and borrow books for their knowledge. The more inventory in the libraries the better");
        _en.Add("School.Desc", "Here people will get an education. Here more is better");
        _en.Add("TradesSchool.Desc", "Here people will get specialized education on trades. The more the better");
        _en.Add("TownHouse.Desc", "The townhouse increases happiness and prosperity to your people");

        //other
        _en.Add("Church.Desc", "The church gives happiness and hope to your people");
        _en.Add("Tavern.Desc", "The tavern gives some relaxation and entertainment to your people");

        //Militar
        _en.Add("WoodPost.Desc", "They spot bandits and pirates quicker so you can prepare in advance");
        _en.Add("PostGuard.Desc", _militar);
        _en.Add("Fort.Desc", _militar);
        _en.Add("Morro.Desc", _militar + ". Once you build this, Pirates should know better");

        //Decoration
        _en.Add("Fountain.Desc", "Beauty your town and adds overall happiness to your citizens");
        _en.Add("WideFountain.Desc", "Beauty your town and adds overall happiness to your citizens");
        _en.Add("PalmTree.Desc", "Beauty your town and adds overall happiness to your citizens");
        _en.Add("FloorFountain.Desc", "Beauty your town and adds overall happiness to your citizens");
        _en.Add("FlowerPot.Desc", "Beauty your town and adds overall happiness to your citizens");
        _en.Add("PradoLion.Desc", "Beauty your town and adds overall happiness to your citizens");



        //Buildings name
        //Infr
        _en.Add("Road", "Road");
        _en.Add("BridgeTrail", "Trail Bridge");
        _en.Add("BridgeRoad", "Road Bridge");
        _en.Add("LightHouse", "Light House");
        _en.Add("Masonry", "Masonry");
        _en.Add("StandLamp", "Stand Lamp");
        _en.Add("HeavyLoad", "Heavy Load");


        //House
        _en.Add("Shack", "Shack");
        _en.Add("MediumShack", "Medium Shack");
        _en.Add("LargeShack", "Large Shack");

        _en.Add("WoodHouseA", "Medium Wood House");
        _en.Add("WoodHouseB", "Large Wood House");
        _en.Add("WoodHouseC", "Luxury Wood House");
        _en.Add("BrickHouseA", "Medium Brick House");
        _en.Add("BrickHouseB", "Luxury Brick House");
        _en.Add("BrickHouseC", "Large Brick House");


        //Farms
        //Animal
        _en.Add("AnimalFarmSmall", "Small Animal Farm");
        _en.Add("AnimalFarmMed", "Medium Animal Farm");
        _en.Add("AnimalFarmLarge", "Large Animal Farm");
        _en.Add("AnimalFarmXLarge", "Extra Large Animal Farm");
        //Fields
        _en.Add("FieldFarmSmall", "Small Field Farm");
        _en.Add("FieldFarmMed", "Medium Field Farm");
        _en.Add("FieldFarmLarge", "Large Field Farm");
        _en.Add("FieldFarmXLarge", "Extra Large Field Farm");
        _en.Add("FishingHut", "Fishing Hut");

        //Raw
        _en.Add("Mortar", "Mortar");
        _en.Add("Clay", "Clay");
        _en.Add("Pottery", "Pottery");
        _en.Add("MountainMine", "Mountain Mine");
        _en.Add("LumberMill", "Lumbermill");
        _en.Add("BlackSmith", "Blacksmith");
        _en.Add("ShoreMine", "Shore Mine");
        _en.Add("QuickLime", "Quicklime");

        //Prod
        _en.Add("Brick", "Brick");
        _en.Add("Carpentry", "Carpentry");
        _en.Add("Cigars", "Cigars");
        _en.Add("Mill", "Mill");
        _en.Add("Tailor", "Tailor");
        _en.Add("Tilery", "Tilery");
        _en.Add("Armory", "Armory");
        _en.Add("Distillery", "Distillery");
        _en.Add("Chocolate", "Chocolate");
        _en.Add("Ink", "Ink");

        //Ind
        _en.Add("Cloth", "Cloth");
        _en.Add("GunPowder", "Gunpowder");
        _en.Add("PaperMill", "Papermill");
        _en.Add("Printer", "Printer");
        _en.Add("CoinStamp", "Coin Stamp");
        _en.Add("SugarMill", "Sugarmill");
        _en.Add("Foundry", "Foundry");
        _en.Add("SteelFoundry", "Steel Foundry");
        _en.Add("SugarShop", "Sugarshop");


        //trade
        _en.Add("Dock", "Dock");
        _en.Add("Shipyard", "Shipyard");
        _en.Add("Supplier", "Supplier");
        _en.Add("StorageSmall", "Small Storage");
        _en.Add("StorageMed", "Medium Storage");
        _en.Add("StorageBig", "Big Storage");

        //gov
        _en.Add("Library", "Library");
        _en.Add("School", "School");
        _en.Add("TradesSchool", "Trades School");
        _en.Add("TownHouse", "Townhouse");

        //other
        _en.Add("Church", "Church");
        _en.Add("Tavern", "Tavern");

        //Militar
        _en.Add("WoodPost", "Wood Guard Duty");
        _en.Add("PostGuard", "Stone Guard Duty");
        _en.Add("Fort", "Fort");
        _en.Add("Morro", "Morro");

        //Decorations
        _en.Add("Fountain", "Fountain");
        _en.Add("WideFountain", "Wide Fountain");
        _en.Add("PalmTree", "Palm Tree");
        _en.Add("FloorFountain", "Floor Fountain");
        _en.Add("FlowerPot", "Flower Pot");
        _en.Add("PradoLion", "Prado Lion");

        //Main GUI
        _en.Add("SaveGame.Dialog", "Save your game progress");
        _en.Add("LoadGame.Dialog", "Load a game");
        _en.Add("NameToSave", "Save your game as:");
        _en.Add("NameToLoad", "Game to load selected:");
        _en.Add("OverWrite", "There is already a saved game with the same name. Do you want to overwrite the file?");
        _en.Add("DeleteDialog", "Are you sure you want to delete the saved game?");
        _en.Add("NotHDDSpace", "Not enough space on {0} drive to save the game");
        _en.Add("GameOverPirate", "Sorry, you lost the game! Pirates attacked your town and killed everyone.");
        _en.Add("GameOverMoney", "Sorry, you lost the game! The crown won't support your Caribbean island anymore.");
        _en.Add("BuyRegion.WithMoney", "Are you sure want to buy this region.");
        _en.Add("BuyRegion.WithOutMoney", "Sorry, you can't afford this now.");
        _en.Add("Feedback", "Feedback!? Awesome...:) Thanks. 8) ");
        _en.Add("OptionalFeedback", "Feedback is crucial. Pls drop a word");
        _en.Add("MandatoryFeedback", "Only the dev team will see this. Your rate is?");
        _en.Add("PathToSeaExplain", "Displays the shortest way to the sea.");


        _en.Add("BugReport", "Caught a bug? uhmm yummy.... Send it this way!! Thanks");
        _en.Add("Invitation", "Your friend's email for a chance to join the Private Beta");
        _en.Add("Info", "");//use for informational Dialogs
        _en.Add("Negative", "The Crown extended a line of credit for you. If you own more than $100,000.00 is game over");


        //MainMenu
        _en.Add("Types_Explain", "Traditional: \nIt's a game where in the beginning some buildings are locked and you have to unlock them. " +
                "The good thing is that this provides you with guidance." +
                "\n\nFreewill: \nAll available buildings are unlocked right away. " +
                "The bad thing is this way you can fail very easily." +
                "\n\nThe 'Hard' difficulty is the closest to reality");


        //Tooltips
        //Small Tooltips
        _en.Add("Person.HoverSmall", "Total/Adults/Kids");
        _en.Add("Emigrate.HoverSmall", "Emigrated");
        _en.Add("CurrSpeed.HoverSmall", "Game Speed");
        _en.Add("Town.HoverSmall", "Town Name");
        _en.Add("Lazy.HoverSmall", "Unemployed People");
        _en.Add("Food.HoverSmall", "Food");
        _en.Add("Happy.HoverSmall", "Happiness");
        _en.Add("PortReputation.HoverSmall", "Reputation of Port");
        _en.Add("Dollars.HoverSmall", "Dollars");
        _en.Add("PirateThreat.HoverSmall", "Pirate Threat");
        _en.Add("Date.HoverSmall", "Date (m/y)");
        _en.Add("MoreSpeed.HoverSmall", "More Speed [PgUp]");
        _en.Add("LessSpeed.HoverSmall", "Less Speed [PgDwn]");
        _en.Add("PauseSpeed.HoverSmall", "Pause Game");
        _en.Add("CurrSpeedBack.HoverSmall", "Current Speed");
        _en.Add("ShowNoti.HoverSmall", "Notifications");
        _en.Add("Menu.HoverSmall", "Main Menu");
        _en.Add("QuickSave.HoverSmall", "Quick Save[Ctrl+S][F]");
        _en.Add("Bug Report.HoverSmall", "Report a Bug");
        _en.Add("Feedback.HoverSmall", "Feedback");
        _en.Add("Hide.HoverSmall", "Hide");
        _en.Add("CleanAll.HoverSmall", "Clean");
        _en.Add("Bulletin.HoverSmall", "Control/Bulletin");
        _en.Add("ShowAgainTuto.HoverSmall", "Tutorial");
        _en.Add("BuyRegion.HoverSmall", "Buy Regions");
        _en.Add("Help.HoverSmall", "Help");

        _en.Add("More.HoverSmall", "More");
        _en.Add("Less.HoverSmall", "Less");
        _en.Add("Prev.HoverSmall", "Previous");

        _en.Add("More Positions.HoverSmall", "More");
        _en.Add("Less Positions.HoverSmall", "Less");


        //down bar
        _en.Add("Infrastructure.HoverSmall", "Infrastructure");
        _en.Add("House.HoverSmall", "Housing");
        _en.Add("Farming.HoverSmall", "Farming");
        _en.Add("Raw.HoverSmall", "Raw");
        _en.Add("Prod.HoverSmall", "Production");
        _en.Add("Ind.HoverSmall", "Industry");
        _en.Add("Trade.HoverSmall", "Trade");
        _en.Add("Gov.HoverSmall", "Government");
        _en.Add("Other.HoverSmall", "Other");
        _en.Add("Militar.HoverSmall", "Military");
        _en.Add("Decoration.HoverSmall", "Decoration");

        _en.Add("WhereIsTown.HoverSmall", "Back to Town [P]");
        _en.Add("WhereIsSea.HoverSmall", "Show Ocean Path");
        _en.Add("Helper.HoverSmall", "Help");
        _en.Add("Tempeture.HoverSmall", "Temperature");

        //building window
        _en.Add("Gen_Btn.HoverSmall", "General Tab");
        _en.Add("Inv_Btn.HoverSmall", "Inventory Tab");
        _en.Add("Upg_Btn.HoverSmall", "Upgrades Tab");
        _en.Add("Prd_Btn.HoverSmall", "Production Tab");
        _en.Add("Sta_Btn.HoverSmall", "Stats Tab");
        _en.Add("Ord_Btn.HoverSmall", "Orders Tab");
        _en.Add("Stop_Production.HoverSmall", "Stop Production");
        _en.Add("Demolish_Btn.HoverSmall", "Demolish");
        _en.Add("More Salary.HoverSmall", "Pay More");
        _en.Add("Less Salary.HoverSmall", "Pay Less");
        _en.Add("Next_Stage_Btn.HoverSmall", "Buy Next Stage");
        _en.Add("Current_Salary.HoverSmall", "Current Salary");
        _en.Add("Current_Positions.HoverSmall", "Current Positions");
        _en.Add("Max_Positions.HoverSmall", "Max Positions");


        _en.Add("Add_Import_Btn.HoverSmall", "Add an Import");
        _en.Add("Add_Export_Btn.HoverSmall", "Add an Export");
        _en.Add("Upg_Cap_Btn.HoverSmall", "Upgrades Capacity");
        _en.Add("Close_Btn.HoverSmall", "Close");
        _en.Add("ShowPath.HoverSmall", "Show Path");
        _en.Add("ShowLocation.HoverSmall", "Show Location");//TownTitle
        _en.Add("TownTitle.HoverSmall", "Town");
        _en.Add("WarMode.HoverSmall", "Combat Mode");
        _en.Add("BullDozer.HoverSmall", "Bulldozer");
        _en.Add("Rate.HoverSmall", "Rate Me");

        //addOrder windiw
        _en.Add("Amt_Tip.HoverSmall", "Product Amount");

        //Med Tooltips 
        _en.Add("Build.HoverMed", "Place building: 'Left click' \n" +
                                "Rotate building: 'R' key \n" +
                                "Cancel: 'Right click'");
        _en.Add("BullDozer.HoverMed", "Clean area: 'Left click' \n" +
            "Cancel: 'Right click' \nCost: $10 per use ");

        _en.Add("Road.HoverMed", "Start: 'Left click' \n" +
                "Expand: 'Move mouse' \n" +
                "Set: 'Left click again' \n" +
            "Cancel: 'Right click'");

        _en.Add("Current_Salary.HoverMed", "Workers will go to work, where the highest salary is paid." +
                                            " If 2 places pay the same salary, then the closest to home will be chosen.");



        //Notifications
        _en.Add("BabyBorn.Noti.Name", "New Born");
        _en.Add("BabyBorn.Noti.Desc", "{0} was born");
        _en.Add("PirateUp.Noti.Name", "Pirates Closer");
        _en.Add("PirateUp.Noti.Desc", "Pirates close to shore");
        _en.Add("PirateDown.Noti.Name", "Pirates Respect You");
        _en.Add("PirateDown.Noti.Desc", "Pirates respect you a bit more today");

        _en.Add("Emigrate.Noti.Name", "A citizen has emigrated");
        _en.Add("Emigrate.Noti.Desc", "People emigrate when they are not happy with your government");
        _en.Add("PortUp.Noti.Name", "Port is known");
        _en.Add("PortUp.Noti.Desc", "Your port reputation is ramping up with neighboring ports and routes");
        _en.Add("PortDown.Noti.Name", "Port is less known");
        _en.Add("PortDown.Noti.Desc", "Your port reputation went down");

        _en.Add("BoughtLand.Noti.Name", "New Land Purchased");
        _en.Add("BoughtLand.Noti.Desc", "A new land region was purchased");

        _en.Add("ShipPayed.Noti.Name", "Ship paid out");
        _en.Add("ShipPayed.Noti.Desc", "A ship has paid out {0} for goods or service");
        _en.Add("ShipArrived.Noti.Name", "A ship has arrived");
        _en.Add("ShipArrived.Noti.Desc", "A new ship has arrived to one of our maritime buildings");

        _en.Add("AgeMajor.Noti.Name", "New worker");
        _en.Add("AgeMajor.Noti.Desc", "{0} is ready to work");


        _en.Add("PersonDie.Noti.Name", "A person has died");
        _en.Add("PersonDie.Noti.Desc", "{0} has died");

        _en.Add("DieReplacementFound.Noti.Name", "A person has died");
        _en.Add("DieReplacementFound.Noti.Desc", "{0} has died. A job replacement was found.");

        _en.Add("DieReplacementNotFound.Noti.Name", "A person has died");
        _en.Add("DieReplacementNotFound.Noti.Desc", "{0} has died. No job replacement was found");


        _en.Add("FullStore.Noti.Name", "A storage is getting full");
        _en.Add("FullStore.Noti.Desc", "A storage is at {0}% capacity");

        _en.Add("CantProduceBzFullStore.Noti.Name", "A worker cannot produce");
        _en.Add("CantProduceBzFullStore.Noti.Desc", "{0} because his/her destination storage is full");

        _en.Add("NoInput.Noti.Name", "At least an input is missing in building");
        _en.Add("NoInput.Noti.Desc", "A building cannot produce {0} because is missing at least one input");

        _en.Add("Built.Noti.Name", "A building has been built");
        _en.Add("Built.Noti.Desc", "{0} has been fully built");

        _en.Add("cannot produce", "cannot produce");





        //Main notificaion
        //Shows on the middle of the screen
        _en.Add("NotScaledOnFloor", "The building is either too close to shore or mountain");
        _en.Add("NotEven", "The ground underneath the building is not even");
        _en.Add("Colliding", "Building is colliding with another one");
        _en.Add("Colliding.BullDozer", "Bulldozer is colliding with a building. Can only be used on terrain (trees, rocks)");

        _en.Add("BadWaterHeight", "The building is too low or high on the water");
        _en.Add("LockedRegion", "You need to own this region to build here");
        _en.Add("HomeLess", "People in this house have nowhere to go. Please build a new house that" +
                            " can hold this family and try again");
        _en.Add("LastFood", "Cannot destroy, this is the only Storage in your village");
        _en.Add("LastMasonry", "Cannot destroy, this is the only Masonry in your village");
        _en.Add("OnlyOneDemolish", "You are demolishing a building already. Try again after demolition is completed");


        //help

        _en.Add("Construction.HoverMed", "For the construction of any building you need to have workers in the Masonry. " +
                    " Click the Masonry, then the '+' sign in the general tab. Make sure you have enough resources");
        _en.Add("Demolition.HoverMed", "Once the inventory is clear will be demolished. Wheelbarrows will move the inventory");

        _en.Add("Construction.Help", "For the construction of any building you need to have workers in the Masonry. " +
                    " Click the Masonry, then the '+' sign in the general tab. Make sure you have enough resources");
        _en.Add("Camera.Help", "Camera: Use [WASD] or cursor to move. " +
                        "Press the scroll wheel on your mouse, keep it pressed to rotate, or [Q] and [E]");
        _en.Add("Sea Path.Help", "Click on the left bottom corner 'Show/hide sea path' " +
                            "button to show the closest path to the sea");

        _en.Add("People Range.Help", "The huge blue circle around each building marks the range of it");

        _en.Add("Pirate Threat.Help", "Pirate Threat: This is how aware are the pirates of your port. This increases as" +
                                        " you have more money. If this reaches over 90 you will lose the game. You can counter the threat by constructing military buildings");

        _en.Add("Port Reputation.Help", "Port Reputation: The more people know your port, the more they will visit." +
                                            " If you want to increase this make sure you always have some orders" +
                                            " in the Dock");
        _en.Add("Emigrate.Help", "Emigrates: When people are unhappy for a few years they leave. The bad" +
                                    " part of this is they won't come back, they won't produce or have children." +
                                    " The only good thing is that they increase the 'Port Reputation'");
        _en.Add("Food.Help", "Food: The higher the variety of food available in a household, the happier they" +
                                " will be.");

        _en.Add("Weight.Help", "Weight: All the weights in the game are in Kg or Lbs depending on which Unit system is selected." +
                                " You can change it in 'Options' in the 'Main Menu'");
        _en.Add("What are Ft3 and M3?.Help", "The storage capacity is determined by the volume of the building. Ft3 is a cubic foot. M3 is a cubic meter");//. Keep in mind that less dense products will fill up your storage quickly. To see products density Bulletin/Prod/Spec" );

        _en.Add("More.Help", "If you need more help might be a good idea completing the tutorial, or simply posting a question on SugarMill's Forums");

        //more 
        _en.Add("Products Expiration.Help", "Products expiration: Just like in real life, in this game every product expires. Some food items expire sooner than others. You can see how many products had expired on Bulletin/Prod/Expire");
        _en.Add("Horse Carriages.Help", "As the game has real measurements people can carry only so much. That's when horse-drawn carriages come into place. They carry a lot more, as a result, your economy gets boosted. A person in their best years might carry around 15KG, a wheelbarrow closer to 60KG, but the smaller cart can carry 240KG. To use them build a HeavyLoad");
        _en.Add("Usage of goods.Help", "Usage of goods: Crates, barrels, wheelbarrows, carts, tools, cloth, crockery, furniture and utensils are all needed to do the traditional activities of a town. As these goods get used, they diminish, as a result, a person won't carry anything if there are no crates. Keep an eye on that ;)");
        _en.Add("Happiness.Help", "Happiness: People's happiness is influenced by various factors. How much money they have, food variety, religion satisfaction, access to leisure, house comfort and education level. Also if a person has access to utensils, crockery and cloth will influence their happiness.");
        _en.Add("Line production.Help", "Line production: To make a simple nail you need to mine ore, in the foundry melt the iron, and finally in the blacksmith make the nail. If you got enough money, you can always buy the nail directly on a ship, or any other product.");
        _en.Add("Bulletin.Help", "The pages icon on the bottom bar is the Bulletin/Control Window. Please get a minute to explore it.");
        _en.Add("Trading.Help", "You will need to have at least one Dock to be able to trade. On it, you can add import/export orders and make some cash. If you need help adding an order you might want to complete the Tutorial");

        _en.Add("Combat Mode.Help", "It activates when a Pirate/Bandit is detected by one of your citizens. Once the mode is active you can command units directly to attack. Select them and right click to objective to attack");

        _en.Add("Population.Help", "Once they turn 16 will move to a free house if found. If there is always a free house to move to the population growth will be guaranteed. If they get into the new houses at 16 years old, you are maximizing population growth");


        _en.Add("F1.Help", "Press [F1] for help");

        _en.Add("Inputs.Help", "If a building can't produce because is missing inputs. Check you have the needed input(s) in the main storage and at least one worker in the masonry");
        _en.Add("WheelBarrows.Help", "Wheelbarrows are the masonry workers. If they got nothing to build will act as wheelbarrows. If you need inputs to get into a specific building make sure you have enough of them working and also the inputs mentioned in the storage building");

        _en.Add("Production Tab.Help", "If the building is a farm field make sure you have workers on the farm. The crop will be lost if sits there a month after harvest day");
        _en.Add("Our Inventories.Help", "The section 'Our inventories' in the 'Add Order Window' is a resume of what we got in our Storages buildings inventories");
        _en.Add("Inventories Explanation.Help", "This a resume of what we got in our Storages inventories. Items in other buildings inventories do not belong to the city");

        ///word and grammarly below




        //to  add on spanish         //to correct  
        _en.Add("TutoOver", "Your reward is $10,000.00 if is the first time you complete it. The tutorial is over now you can keep playing this game or start a new one.");

        //Tuto
        _en.Add("CamMov.Tuto", "Tutorial completion reward is $10,000 (one time reward per game). Step1: Use [WASD] or arrow keys to move the Camera. Do this for at least 5 seconds");
        _en.Add("CamMov5x.Tuto", "Use [WASD] or arrow keys and keep press the 'Left Shift' key to move the Camera 5 times quicker. Do this for at least 5 seconds");
        _en.Add("CamRot.Tuto", "Now press the scroll wheel down on your mouse and move your mouse to rotate the Camera. Do this for at least 5 seconds");


        _en.Add("BackToTown.Tuto", "Press the key [P] on the keyboard to go to the initial position of the camera");

        _en.Add("BuyRegion.Tuto", "Regions, you need to own a region to be able to build in it. Click on '+' sign on the bottom bar, then on the 'For Sale' sign in the" +
                    " middle of a region to buy it. Some buildings are exempt, they can be built without owning the region" +
                    " (FishingHut, Dock, MountainMine, ShoreMine, LightHouse, PostGuard)"
                     );

        _en.Add("Trade.Tuto", "That was easy, the hard part is coming. Click on the 'Trade' buildings button, located in the right bottom bar. " +
                "When you hover over it, it will popup 'Trade'");
        _en.Add("CamHeaven.Tuto", "Scroll back with your mouse middle button until the camera reaches"
                    + " the sky. This view is useful to place bigger buildings such as the Port");

        _en.Add("Dock.Tuto", "Now click on the 'Dock' building, it is the 1st button. When you hover over it, it will" +
                " show its cost and description");
        _en.Add("Dock.Placed.Tuto", "Now the hard, read carefully. Notice that you can use the " +
                "'R' key to Rotate, and right click to cancel the building. This building has a part in the ocean and other in the land." +
                " The arrow goes to the sea, the storage section goes to land. Once the arrow is colored white, left click.");
        _en.Add("2XSpeed.Tuto", "Increase the game's speed, go to the middle top screen simulation speed controller, click the "
                    + " 'More Speed' button 1 time until 2x is displayed");

        _en.Add("ShowWorkersControl.Tuto", "Click on the 'Control/Bulletin' button, located in the bottom bar. " +
                "When you hover over it, it will popup 'Control/Bulletin'. ");
        _en.Add("AddWorkers.Tuto", "Click the '+' sign to the right of the Masonry building, this is how you add more workers.");
        _en.Add("HideBulletin.Tuto", "Keep in mind that in this window you are able to control and see different aspects of the game. Click outside the window to close it or 'OK' button.");
        _en.Add("FinishDock.Tuto", "Now finish the Dock building. The more workers are in the Masonry the quicker is going to get done too."
            + " Also make sure you have all materials needed to build it");
        _en.Add("ShowHelp.Tuto", "Click on the 'Help' button, located in the bottom bar. " +
                "When you hover over it, it will popup 'Help'. There you can find some tips.");


        _en.Add("SelectDock.Tuto", "Ships drop and pick goods at random from the dock's inventory. Workers are needed to move dock goods in and out. They need wheelbarrows and crates. If are no items in the dock storage they won't work. Now click on the Dock.");


        _en.Add("OrderTab.Tuto", "Go to the Orders tab on the Dock's Window.");
        _en.Add("ImportOrder.Tuto", "Click on the '+' sign beside Add Import Order.");

        _en.Add("AddOrder.Tuto", "Now scroll down in the products and select wood and enter 100 as the amount. Then click the 'Add' button.");
        _en.Add("CloseDockWindow.Tuto", "Now the order is added. A random ship will drop this item in the dock inventory. And then your dock workers will take it to the closest Storage building. Now click out the window, so it closes.");
        _en.Add("Rename.Tuto", "Click on a person and then click on the title bar of the person. Like this, you can change the name of any person or building in the game. Click outside so the change is saved");
        _en.Add("RenameBuild.Tuto", "Now click on a building and change its name in the same way. Remember to click outside so the change is saved");

        _en.Add("BullDozer.Tuto", "Now click on the Bulldozer icon on the bottom bar. Then remove a tree or a rock from the terrain.");


        _en.Add("Budget.Tuto", "Click on the 'Control/Bulletin' button, then on 'Finance' menu and then on 'Ledger'. This is the game ledger");
        _en.Add("Prod.Tuto", "Click on 'Prod' menu and then on 'Produce'. Will show the village's production for the last 5 years");
        _en.Add("Spec.Tuto", "Click the 'Prod' menu and then on 'Spec'. Here you can see exactly how to make each product on the game. The inputs necessaries and where is produced. Also, the import and export prices");
        _en.Add("Exports.Tuto", "Click the 'Finance' menu and then on 'Export'. Here you can see a breakdown of your village's exports");


        //Quest
        _en.Add("Tutorial.Quest", "Quest: Finish the tutorial. Reward $10,000. It takes roughly 3 minutes to complete");

        _en.Add("Lamp.Quest", "Quest: Build a StandLamp. Find it on Infrastructure, it shines at night if there is Whale Oil on Storage");

        _en.Add("Shack.Quest", "Quest: Build a Shack. These are cheap houses. When people turn 16 they will move to a free house if found. In this way, population growth will be guaranteed. [F1] Help. If you see smoke in a house's chimney means there are people living in it");

        _en.Add("SmallFarm.Quest", "Quest: Build a Small Field Farm. You need farms to feed your people");
        _en.Add("FarmHire.Quest", "Quest: Hire two farmers in the Small Field Farm. Click on the farm and in the plus sign assign workers. You need to have unemployed"
                    + " people to be able to assign them into a new building");



        _en.Add("FarmProduce.Quest", "Quest: Now produce " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " of beans on the Small Field Farm. Click on the 'Stat' tab and will show you the production of the last 5 years. You can see the quest progress in the quest window. If you build more small farms will be accounted for the quest");
        _en.Add("Transport.Quest", "Quest: Transport the beans from the farm to the Storage. To do that make sure you have" +
                " workers on the masonry. They act as wheelbarrows when not building");


        _en.Add("HireDocker.Quest", "Quest: Hire a docker. Dockers only task is to move the goods into the Dock from the Storage if you are exporting." +
            " Or vice-versa if importing. They work when there is an order in place and the goods are ready to transport. Otherwise, they stay at home resting." +
                " If you haven't built a Dock then build one." +
            " Find it in Trade.");


        _en.Add("Export.Quest", "Quest: At the Dock create an order and export exactly 300 " + Unit.CurrentWeightUnitsString() + " of beans." +
                " In the Dock click on the 'Orders' tab and add an export order with the '+' sign." +
            " Select product and enter amount");



        _en.Add("MakeBucks.Quest", "Quest: Make $100 exporting goods in the Dock. " +
            "Once a ship arrives will randomly pay product(s) in your Dock's inventory");
        _en.Add("HeavyLoad.Quest", "Quest: Build a Heavyload building. This are haulers that carry more weight. They will come handy when transporting goods around is needed."); //Carts must be available on towns storages for them to work" );
        _en.Add("HireHeavy.Quest", "Quest: In the Heavyload building hire a Heavy Hauler.");


        _en.Add("ImportOil.Quest", "Quest: Import 500 " + Unit.CurrentWeightUnitsString() + " of Whale Oil at the Dock. This is needed to keep your lights on at night. Ships will randomly drop imports in your Dock's inventory");

        _en.Add("Population50.Quest", "Reach a total population of 50 citizens");

        //added Aug 11 2017, result: sep 9(30% off biggest sale ever)
        _en.Add("Production.Quest", "Let's produce Weapons now and sell them later. First of all, build a Blacksmith. Find it in the 'Raw' buildings menu");
        _en.Add("ChangeProductToWeapon.Quest", "In the Blacksmith's 'Products Tab' change the production to Weapon. Workers will bring the raw materials needed to forge weapons if found");
        _en.Add("BlackSmithHire.Quest", "Hire two blacksmiths");
        _en.Add("WeaponsProduce.Quest", "Now produce " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " of Weapons in the Blacksmith. Click on the 'Stat' tab and will show you the production of the last 5 years. You can see the quest progress in the quest window.");
        _en.Add("ExportWeapons.Quest", "Now export 100 " + Unit.CurrentWeightUnitsString() + " of Weapons. On the Dock add an order of export. Notice that Weapons are a profitable business");


        _en.Add("CompleteQuest", "Your reward is {0}");


        //added Sep 14 2017
        _en.Add("BuildFishingHut.Quest", "Build a Fishing hut. In this way citizens have different foods to eat, which translates into happiness");
        _en.Add("HireFisher.Quest", "Hire a fisher");

        _en.Add("BuildLumber.Quest", "Build a Lumbermill. Find it in the 'Raw' buildings menu");
        _en.Add("HireLumberJack.Quest", "Hire a Lumberjack");

        _en.Add("BuildGunPowder.Quest", "Build a Gunpowder. Find it in the 'Industry' buildings menu");
        _en.Add("ImportSulfur.Quest", "In the dock import 1000 " + Unit.CurrentWeightUnitsString() + " of Sulfur");
        _en.Add("GunPowderHire.Quest", "Hire one worker in the Gunpowder building");

        _en.Add("ImportPotassium.Quest", "In the dock import 1000 " + Unit.CurrentWeightUnitsString() + " of Potassium");
        _en.Add("ImportCoal.Quest", "In the dock import 1000 " + Unit.CurrentWeightUnitsString() + " of Coal");

        _en.Add("ProduceGunPowder.Quest", "Lets produce now " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " of Gunpowder. Notice that you will need Sulfur, Potassium and Coal to produce Gunpowder");
        _en.Add("ExportGunPowder.Quest", "In the dock export 100 " + Unit.CurrentWeightUnitsString() + " of Gunpowder");

        _en.Add("BuildLargeShack.Quest", "Build a Largeshack in this bigger houses population will grow faster");

        _en.Add("BuildA2ndDock.Quest", "Build a second Dock. This dock could be used only for imports in that way you can import raw materials here and export them at another dock");
        _en.Add("Rename2ndDock.Quest", "Rename the Docks now, so you can remember which be used only for imports and exports");

        _en.Add("Import2000Wood.Quest", "In the Imports dock import 2000 " + Unit.CurrentWeightUnitsString() + " of Wood. This raw material is needed for everything because is used as fuel");

        //IT HAS FINAL MESSAGE 
        //last quest it has a final message to the player. if new quest added please put the final message in the last quest
        _en.Add("Import2000Coal.Quest", "In the Imports dock import 2000 " + Unit.CurrentWeightUnitsString() + " of Coal. Coal also, is needed for everything because is used as fuel. Hope you enjoy the experience so far. Keep expanding your colony, and wealth. Also, please help to improve the game. Participate in the online forums your voice and opinions are important! Have fun Sugarmiller!");

        //



        //Quest Titles
        _en.Add("Tutorial.Quest.Title", "Tutorial");
        _en.Add("Lamp.Quest.Title", "Stand Lamp");

        _en.Add("Shack.Quest.Title", "Build a Shack");
        _en.Add("SmallFarm.Quest.Title", "Build a Farm Field");
        _en.Add("FarmHire.Quest.Title", "Hire Two Farmers");


        _en.Add("FarmProduce.Quest.Title", "Farm Producer");

        _en.Add("Export.Quest.Title", "Exports");
        _en.Add("HireDocker.Quest.Title", "Hire Docker");
        _en.Add("MakeBucks.Quest.Title", "Make Money");
        _en.Add("HeavyLoad.Quest.Title", "Heavy Load");
        _en.Add("HireHeavy.Quest.Title", "Hire a Heavy Hauler");

        _en.Add("ImportOil.Quest.Title", "Whale Oil");

        _en.Add("Population50.Quest.Title", "50 Citizens");

        //
        _en.Add("Production.Quest.Title", "Produce Weapons");
        _en.Add("ChangeProductToWeapon.Quest.Title", "Change Product");
        _en.Add("BlackSmithHire.Quest.Title", "Hire Two Blacksmiths");
        _en.Add("WeaponsProduce.Quest.Title", "Forge Weapons");
        _en.Add("ExportWeapons.Quest.Title", "Make Profit");

        //
        _en.Add("BuildFishingHut.Quest.Title", "Build a Fishing Hut");
        _en.Add("HireFisher.Quest.Title", "Hire a Fisher");
        _en.Add("BuildLumber.Quest.Title", "Build a Lumber");
        _en.Add("HireLumberJack.Quest.Title", "Hire a Lumberjack");
        _en.Add("BuildGunPowder.Quest.Title", "Build a Gunpowder");
        _en.Add("ImportSulfur.Quest.Title", "Import Sulfur");
        _en.Add("GunPowderHire.Quest.Title", "Hire a Gunpowder worker");
        _en.Add("ImportPotassium.Quest.Title", "Import Potassium");
        _en.Add("ImportCoal.Quest.Title", "Import Coal");
        _en.Add("ProduceGunPowder.Quest.Title", "Produce Gunpowder");
        _en.Add("ExportGunPowder.Quest.Title", "Export Gunpowder");
        _en.Add("BuildLargeShack.Quest.Title", "Build a Largeshack");
        _en.Add("BuildA2ndDock.Quest.Title", "Build a Second Dock");
        _en.Add("Rename2ndDock.Quest.Title", "Rename the Second Dock");
        _en.Add("Import2000Wood.Quest.Title", "Import some Wood");
        _en.Add("Import2000Coal.Quest.Title", "Import some Coal");



        //
        _en.Add("Tutorial.Arrow", "This is the tutorial. Once finished you will win $10,000");
        _en.Add("Quest.Arrow", "This is the quest button. You can access the quest window by clicking on it");
        _en.Add("New.Quest.Avail", "At least one quest is available");
        _en.Add("Quest_Button.HoverSmall", "Quest");



        //Products
        //Notification.Init()
        _en.Add("RandomFoundryOutput", "Melted Ore");

        //OrderShow.ShowToSetCurrentProduct()
        _en.Add("RandomFoundryOutput (Ore, Wood)", "Melted Ore (Ore, Wood)");



        //Bulleting helps
        _en.Add("Help.Bulletin/Prod/Produce", "Here is shown what is being produced in the village.");
        _en.Add("Help.Bulletin/Prod/Expire", "Here is shown what has expired on the village.");
        _en.Add("Help.Bulletin/Prod/Consume", "Here is shown what is being consumed by your people.");

        _en.Add("Help.Bulletin/Prod/Spec", "In this window, you can see the inputs needed for each product, where is built and the price. "
            + "Scroll to the top to see the headers. Notice that one simple product may have more than a formula to produce.");

        _en.Add("Help.Bulletin/General/Buildings", "This is a resume of how many buildings are of each type.");

        _en.Add("Help.Bulletin/General/Workers", "In this window, you can assign workers to work in various buildings. "
    + "For a building allow more people into work, must be less than capacity and must find at least an unemployed person.");

        _en.Add("Help.Bulletin/Finance/Ledger", "Here is shown your ledger. Salary is the amount of money paid to a worker. The more people working the more salary will be paid out.");
        _en.Add("Help.Bulletin/Finance/Exports", "A breakdown of the exports");
        _en.Add("Help.Bulletin/Finance/Imports", "A breakdown of the imports");


        _en.Add("Help.Bulletin/Finance/Prices", "....");


        _en.Add("LoadWontFit", "This load won't fit in the storage area");

        //and so on
        _en.Add("Missing.Input", "Building can't produce (Inputs must be in this building inventory). Missing inputs: \n");





        //in game

        _en.Add("Buildings.Ready", "\n Buildings ready to be built:");
        _en.Add("People.Living", "People living in this house:");
        _en.Add("Occupied:", "Filled:");
        _en.Add("|| Capacity:", "|| Capacity:");
        _en.Add("Users:", "\nUsers:");
        _en.Add("Amt.Cant.Be.0", "Amount can't be 0");
        _en.Add("Prod.Not.Select", "Please select a product");


        //articles
        _en.Add("The.Male", "The");
        _en.Add("The.Female", "The");

        //
        _en.Add("Build.Destroy.Soon", "This building will be destroyed soon. If inventory is not empty, it needs to be cleared by wheelbarrows");




        //words
        //Field Farms
        _en.Add("Bean", "Bean");
        _en.Add("Potato", "Potato");
        _en.Add("SugarCane", "Sugarcane");
        _en.Add("Corn", "Corn");
        _en.Add("Cotton", "Cotton");
        _en.Add("Banana", "Banana");
        _en.Add("Coconut", "Coconut");
        //Animal Farm
        _en.Add("Chicken", "Chicken");
        _en.Add("Egg", "Egg");
        _en.Add("Pork", "Pork");
        _en.Add("Beef", "Beef");
        _en.Add("Leather", "Leather");
        _en.Add("Fish", "Fish");
        //mines
        _en.Add("Gold", "Gold");
        _en.Add("Stone", "Stone");
        _en.Add("Iron", "Iron");

        // { "Clay", "Clay" );
        _en.Add("Ceramic", "Ceramic");
        _en.Add("Wood", "Wood");

        //Prod
        _en.Add("Tool", "Tool");
        _en.Add("Tonel", "Tonel");
        _en.Add("Cigar", "Cigar");
        _en.Add("Tile", "Tile");
        _en.Add("Fabric", "Fabric");
        _en.Add("Paper", "Paper");
        _en.Add("Map", "Map");
        _en.Add("Book", "Book");
        _en.Add("Sugar", "Sugar");
        _en.Add("None", "None");
        //
        _en.Add("Person", "Person");
        _en.Add("Food", "Food");
        _en.Add("Dollar", "Dollar");
        _en.Add("Salt", "Salt");
        _en.Add("Coal", "Coal");
        _en.Add("Sulfur", "Sulfur");
        _en.Add("Potassium", "Potassium");
        _en.Add("Silver", "Silver");
        _en.Add("Henequen", "Henequen");
        //
        _en.Add("Sail", "Sail");
        _en.Add("String", "String");
        _en.Add("Nail", "Nail");
        _en.Add("CannonBall", "Cannonball");
        _en.Add("TobaccoLeaf", "Tobaccoleaf");
        _en.Add("CoffeeBean", "Coffeebean");
        _en.Add("Cacao", "Cacao");
        // { "Chocolate", "Chocolate" );
        _en.Add("Weapon", "Weapon");
        _en.Add("WheelBarrow", "Wheelbarrow");
        _en.Add("WhaleOil", "Whaleoil");
        //
        _en.Add("Diamond", "Diamond");
        _en.Add("Jewel", "Jewel");
        // { "Cloth", "Cloth" );
        _en.Add("Rum", "Rum");
        _en.Add("Wine", "Wine");
        _en.Add("Ore", "Ore");
        _en.Add("Crate", "Crate");
        _en.Add("Coin", "Coin");
        _en.Add("CannonPart", "Cannon Part");
        // { "Ink", "Ink" );
        _en.Add("Steel", "Steel");
        //
        _en.Add("CornFlower", "Cornflower");
        _en.Add("Bread", "Bread");
        _en.Add("Carrot", "Carrot");
        _en.Add("Tomato", "Tomato");
        _en.Add("Cucumber", "Cucumber");
        _en.Add("Cabbage", "Cabbage");
        _en.Add("Lettuce", "Lettuce");
        _en.Add("SweetPotato", "Sweetpotato");
        _en.Add("Yucca", "Yucca");
        _en.Add("Pineapple", "Pineapple");
        //
        _en.Add("Papaya", "Papaya");
        _en.Add("Wool", "Wool");
        _en.Add("Shoe", "Shoe");
        _en.Add("CigarBox", "Cigarbox");
        _en.Add("Water", "Water");
        _en.Add("Beer", "Beer");
        _en.Add("Honey", "Honey");
        _en.Add("Bucket", "Bucket");
        _en.Add("Cart", "Cart");
        _en.Add("RoofTile", "Rooftile");
        _en.Add("FloorTile", "Floortile");
        //{ "Mortar", "Mortar" );
        _en.Add("Furniture", "Furniture");
        _en.Add("Crockery", "Crockery");

        _en.Add("Utensil", "Utensil");
        _en.Add("Stop", "Stop");


        //more Main GUI
        _en.Add("Workers distribution", "Workers distribution");
        _en.Add("Buildings", "Buildings");

        _en.Add("Age", "Age");
        _en.Add("Gender", "Gender");
        _en.Add("Height", "Height");
        _en.Add("Weight", "Weight");
        _en.Add("Calories", "Calories");
        _en.Add("Nutrition", "Nutrition");
        _en.Add("Profession", "Profession");
        _en.Add("Spouse", "Spouse");
        _en.Add("Happinness", "Happinnes");
        _en.Add("Years Of School", "Years of School");
        _en.Add("Age majority reach", "Age majority reach");
        _en.Add("Home", "Home");
        _en.Add("Work", "Work");
        _en.Add("Food Source", "Food Source");
        _en.Add("Religion", "Religion");
        _en.Add("Chill", "Chilll");
        _en.Add("Thirst", "Thirst");
        _en.Add("Account", "Account");

        _en.Add("Early Access Build", "Early Access Build");

        //Main Menu
        _en.Add("Resume Game", "Resume Game");
        _en.Add("Continue Game", "Continue Game");
        _en.Add("Tutorial(Beta)", "Tutorial(Beta)");
        _en.Add("New Game", "New Game");
        _en.Add("Load Game", "Load Game");
        _en.Add("Save Game", "Save Game");
        _en.Add("Achievements", "Achievements");
        _en.Add("Options", "Options");
        _en.Add("Exit", "Exit");
        //Screens
        //New Game
        _en.Add("Town Name:", "Town Name:");
        _en.Add("Difficulty:", "Difficulty:");
        _en.Add("Easy", "Easy");
        _en.Add("Moderate", "Moderate");
        _en.Add("Hard", "Hard");
        _en.Add("Type of game:", "Type of game:");
        _en.Add("Freewill", "Freewill");
        _en.Add("Traditional", "Traditional");
        _en.Add("New.Game.Pirates", "Pirates (if checked the town could suffer a pirate attack");
        _en.Add("New.Game.Expires", "Food Expiration (if checked food expires with time)");
        _en.Add("OK", "OK");
        _en.Add("Cancel", "Cancel");
        _en.Add("Delete", "Delete");
        _en.Add("Enter name...", "Enter name...");
        //Options
        _en.Add("General", "General");
        _en.Add("Unit System:", "Unit System:");
        _en.Add("Metric", "Metric");
        _en.Add("Imperial", "Imperial");
        _en.Add("AutoSave Frec:", "Autosave:");
        _en.Add("20 min", "20 min");
        _en.Add("15 min", "15 min");
        _en.Add("10 min", "10 min");
        _en.Add("5 min", "5 min");
        _en.Add("Language:", "Language:");
        _en.Add("English", "English");
        _en.Add("Camera Sensitivity:", "Camera Sensitivity:");
        _en.Add("Themes", "Themes");
        _en.Add("Halloween:", "Halloween:");
        _en.Add("Christmas:", "Christmas:");
        _en.Add("Options.Change.Theme", "When changed please reload the game");

        _en.Add("Screen", "Screen");
        _en.Add("Quality:", "Quality:");
        _en.Add("Beautiful", "Beautiful");
        _en.Add("Fantastic", "Fantastic");
        _en.Add("Simple", "Simple");
        _en.Add("Good", "Good");
        _en.Add("Resolution:", "Resolution:");
        _en.Add("FullScreen:", "Fullscreen:");

        _en.Add("Audio", "Audio");
        _en.Add("Music:", "Music:");
        _en.Add("Sound:", "Sound:");
        _en.Add("Newborn", "Newborn");
        _en.Add("Build Completed", "Build Completed");
        _en.Add("People's Voice", "People's Voice");

        //in game gui
        _en.Add("Prod", "Prod");
        _en.Add("Finance", "Finance");

        //After Oct 20th 2018
        _en.Add("Resources", "Resources");
        _en.Add("Dollars", "Dollars");
        _en.Add("Coming.Soon", "This building is coming soon to the game");
        _en.Add("Max.Population", "Can't build more houses. Max population reached");

        _en.Add("To.Unlock", "To unlock: ");
        _en.Add("People", "People");
        _en.Add("Of.Food", " of food. ");
        _en.Add("Port.Reputation.Least", "Port reputation at least at ");
        _en.Add("Pirate.Threat.Less", "Pirate threat less than ");
        _en.Add("Skip", "Skip");

        //After Dec 8, 2018
        _en.Add("ReloadMod.HoverSmall", "Reload Mod Files");
        _en.Add("isAboveHeight.MaritimeBound", "Building's land portion is below allowed height");
        _en.Add("arePointsEven.MaritimeBound", "Building's land portion is not in an even terrain");
        _en.Add("isOnTheFloor.MaritimeBound", "Building's land portion is not at the common height");
        _en.Add("isBelowHeight.MaritimeBound", "Building's maritime portion must be in the water");

        _en.Add("InLand.Helper", "On Land");
        _en.Add("InWater.Helper", "On Water");



















        // //ESPANNOL
        // string _houseTailES = ". A los Azucareros les encanta comerse una buena comida de vez en cuando";
        // string _animalFarmTailES = ", aqui se pueden criar diferentes animales";
        // string _fieldFarmTailES = ", aqui se puede cultivar diferentes cultivos";
        // string _asLongHasInputES = ", siempre y cuando tenga la materia prima necesaria";
        // string _produceES = "Aqui los trabajadores produciran el producto selectionado, siempre y cuando exista la materia prima";
        // string _storageES =
        //     "Aqui se almacenan todos los productos, si se llena los ciudadanos no tendran donde almacenar sus cosas";
        // string _militarES = "Con esta construcción la Amenaza Pirata decrece, " +
        //                                     "para ser efectiva necesita trabajadores. Mientras mas, mejor";

        // _sp = new LangDict();

        // //Descriptions
        // //Infr
        // _sp.Add("Road.Desc", "Solo para propositos de decoracion. Las personas se sienten mas felices si la via esta pavimentada alrededor de ellos");
        // _sp.Add("BridgeTrail.Desc", "Por aqui las personas pasan de un lado del mapa a otro");
        // _sp.Add("BridgeRoad.Desc", "Por aqui las personas pasan de un lado del mapa a otro. Los ciudadanos adoran estos puentes. " +
        //                        "Les da un sentido de prosperidad y felicidad" + _houseTailES);
        // _sp.Add(  "LightHouse.Desc","Ayuda a que el puedo sea descubierto mas facil. Añade a la Reputacion del Puerto siempre y cuando la llama este encendida"  );
        // _sp.Add(  H.Masonry + ".Desc","Una construcción muy imporatante. Estos trabajadores son los que construyen y cuando no tienen nada que hacer se dedican a transportar bienes"  );
        // _sp.Add(  H.HeavyLoad + ".Desc","Una construcción muy imporatante. Estos trabajadores son los que construyen y cuando no tienen nada que hacer se dedican a transportar bienes"  );
        // _sp.Add(  H.StandLamp + ".Desc","Alumbra por las noches si hay Aceite de Ballena en la almancen."  );

        // //House
        // _sp.Add("Bohio.Desc", "El Bohio, una casa con condiciones muy rudimentarias, los ciudadanos se abochornan de vivir aqui, una familia puede tener el maximo de 1 niño aqui" + _houseTail);

        // _sp.Add(  "Shack.Desc", "Casucha: Con condiciones de vida primitiva, las personas no son felicies viviendo aqui y pueden tener un maximo de 2 niños"  );
        // _sp.Add(  "MediumShack.Desc", "Casucha mediana: Las condiciones son basicas, y las personas sienten muy poca felicidad viviendo aqui y pueden tener un maximo de 2-3 niños"  );
        // _sp.Add(  "LargeShack.Desc", "Casucha grande: Las condiciones son un poco mejor que basicas, y las personas sienten algo de felicidad viviendo aqui y pueden tener un maximo de 2-4 niños"  );

        // _sp.Add(  "HouseA.Desc","Casa pequeña de madera, una familia puede tener el maximo de 2 niños aqui" +_houseTailES);
        // _sp.Add("HouseB.Desc", "Small house, una familia puede tener el maximo de 2 niños aqui" + _houseTailES);
        // _sp.Add(  "HouseTwoFloor.Desc","Wooden Medium house, una familia puede tener el maximo de 3 niños aqui"+_houseTailES);
        // _sp.Add(  "HouseMed.Desc","Medium house, una familia puede tener el maximo de 2 a 3 niños aqui"+_houseTailES);
        // _sp.Add(  "BrickHouseA.Desc","Casa de ladrillos:, una familia puede tener el maximo de 3 a 4 niños aqui"+_houseTailES);
        // _sp.Add(  "BrickHouseB.Desc","Casa de ladrillos:, una familia puede tener el maximo de 3 a 4 niños aqui"+_houseTailES);
        // _sp.Add(  "BrickHouseC.Desc","Casa de ladrillos:, una familia puede tener el maximo de 4 niños aqui"+_houseTailES);

        //    //Farms
        //    //Animal
        // _sp.Add(  "AnimalFarmSmall.Desc","Finca de animales chica"+_animalFarmTailES);
        // _sp.Add(  "AnimalFarmMed.Desc","Finca de animales mediana"+_animalFarmTailES);
        // _sp.Add(  "AnimalFarmLarge.Desc","Finca de animales grande"+_animalFarmTailES);
        // _sp.Add(  "AnimalFarmXLarge.Desc","Finca de animales super grande"+_animalFarmTailES);
        //    //Fields
        // _sp.Add(  "FieldFarmSmall.Desc","Finca de cultivos chica"+_fieldFarmTailES);
        // _sp.Add(  "FieldFarmMed.Desc","Finca de cultivos mediana"+_fieldFarmTailES);
        // _sp.Add(  "FieldFarmLarge.Desc","Finca de cultivos grande"+_fieldFarmTailES);
        // _sp.Add(  "FieldFarmXLarge.Desc","Finca de cultivos super grande"+_fieldFarmTailES);
        // _sp.Add(  H.FishingHut + ".Desc","Aqui se pescan peces"  );

        //    //Raw
        // _sp.Add(  "Clay.Desc","Aqui se produce barro, necesaria para construir ladrillos y otros productos mas"  );
        // _sp.Add(  "Pottery.Desc","Aqui se producen productos de ceramica como platos, jarras, etc"  );
        // _sp.Add(  "Mine.Desc","Esta es una mina"  );
        // _sp.Add(  "MountainMine.Desc","Esta es una mina"  );
        // _sp.Add(  "Resin.Desc","La Resina de saca de los arboles aqui"  );
        // _sp.Add(   H.LumberMill +".Desc","Aqui los trabajadores buscan y extraen recursos naturales como madera, piedra y minerales"  );
        // _sp.Add(  "BlackSmith.Desc","Aqui el trabajador produce elementos de la forja "+_asLongHasInputES);
        // _sp.Add(  "ShoreMine.Desc","Aqui se produce la sal, o arena"  );
        // _sp.Add(  "QuickLime.Desc","Aqui los trabajadores producen cal"  );
        // _sp.Add(  "Mortar.Desc","Aqui los trabajadores producen mezcla"  );

        //    //Prod
        // _sp.Add(  "Brick.Desc",_produceES);
        // _sp.Add(  "Carpentry.Desc",_produceES);
        // _sp.Add(  "Cigars.Desc",_produceES);
        // _sp.Add(  "Mill.Desc",_produceES);
        // _sp.Add(  H.Tailor+".Desc",_produceES);
        // _sp.Add(  "Tilery.Desc",_produceES);
        // _sp.Add(  "Armory.Desc",_produceES);
        // _sp.Add(  H.Distillery+".Desc",_produceES);
        // _sp.Add(  "Chocolate.Desc",_produceES);
        // _sp.Add(  "Ink.Desc",_produceES);

        //    //Ind
        // _sp.Add(  "Cloth.Desc",_produceES);
        // _sp.Add(  "GunPowder.Desc",_produceES);
        // _sp.Add(  "PaperMill.Desc",_produceES);
        // _sp.Add(  "Printer.Desc",_produceES);
        // _sp.Add(  "CoinStamp.Desc",_produceES);
        // _sp.Add(  "Silk.Desc",_produceES);
        // _sp.Add(  "SugarMill.Desc",_produceES);
        // _sp.Add(  "Foundry.Desc",_produceES);
        // _sp.Add(  "SteelFoundry.Desc",_produceES);
        // _sp.Add(  "SugarShop.Desc", "Produce derivados de la azucar. " + _produceES);

        //    //trade
        // _sp.Add(  "Dock.Desc","Aqui se pueden importar y exportar bienes"  );
        // _sp.Add(  H.Shipyard + ".Desc","Aqui se reparan los barcos, para ser efectivo debe tener los materiales necesarios"  );
        // _sp.Add(  "Supplier.Desc","Aqui se abastecen los barcos para sus largos viajes, siempre y cuando haiga bienes aqui"  );
        // _sp.Add(  "StorageSmall.Desc",_storageES);
        // _sp.Add(  "StorageMed.Desc",_storageES);
        // _sp.Add(  "StorageBig.Desc",_storageES);
        // _sp.Add(  "StorageBigTwoDoors.Desc",_storageES);
        // _sp.Add(  "StorageExtraBig.Desc",_storageES);

        //    //gov
        // _sp.Add(  "Library.Desc","Aqui la gente viene a nutrirse de conocimiento. Mientras mas libros haiga es mejor"  );
        // _sp.Add(  "School.Desc","Aqui empieza la educacion de los Azucareros, mientras mas mejor"  );
        // _sp.Add(  "TradesSchool.Desc","Aqui los Azucareros aprenden habilidades especiales, mientras mas mejor"  );
        // _sp.Add(  "TownHouse.Desc","La casa de gobierno le da alegria y sentido de prosperidad a los ciudadanos"  );

        //    //other
        // _sp.Add(  "Church.Desc","La iglesia le da felicidad y esperanza a la gente"  );
        // _sp.Add(  "Tavern.Desc","Aqui la gente viene a tomar y a divertirse"  );

        //    //Militar
        // _sp.Add(  "PostGuard.Desc",_militarES);
        // _sp.Add(  "Fort.Desc",_militarES);
        // _sp.Add(  "Morro.Desc",_militarES+". Una vez construida esta construcción los piratas te respetaran infinitamente"  );
        // _sp.Add(  "WoodPost.Desc", "Ellos ven los pirates y bandidos primero de esta manera te puedes preparar mejor y con mas tiempo"  );


        //    //Structures Categores
        // _sp.Add(  "Infrastructure", "Infrastructura"  );
        // _sp.Add(  "Housing", "Casas"  );
        // _sp.Add(  "Farming", "Comida"  );
        // _sp.Add(  "Raw", "Materias Primas"  );
        // _sp.Add(  "Production", "Produccion"  );
        // _sp.Add(  "Industry", "Industrias"  );
        // _sp.Add(  "Trade", "Comercio"  );
        // _sp.Add(  "GovServices", "Servicios de Gobierno"  );
        // _sp.Add(  "Other", "Otros"  );
        // _sp.Add(  "Militar", "Militar"  );

        //    //Buildings name
        //    //Infr
        // _sp.Add(  "StandLamp", "Lampara de Calle"  );
        // _sp.Add(  "Trail", "Sendero"  );
        // _sp.Add(  "Road", "Calle"  );
        // _sp.Add(  "BridgeTrail", "Puente Pequeño"  );
        // _sp.Add(  "BridgeRoad", "Puente Mediano"  );
        // _sp.Add(  "CoachMan", "CoachMan"  );
        // _sp.Add(  "LightHouse", "Faro"  );
        // _sp.Add(  "WheelBarrow", "Carretilleros"  );
        // _sp.Add(  "StockPile", "Explanada"  );
        // _sp.Add(  "Masonry", "Masonry"  );
        // _sp.Add(  "HeavyLoad", "Cocheros"  );

        //    //House
        // _sp.Add(  "Shack", "Casucha"  );
        // _sp.Add(  "MediumShack", "Casucha Mediana"  );
        // _sp.Add(  "LargeShack", "Casucha Grande"  );

        // _sp.Add("WoodHouseA", "Casa de Madera Mediana"  );
        // _sp.Add("WoodHouseB", "Casa de Madera Grande" );
        // _sp.Add(  "WoodHouseC", "Casa de Madera de Lujo"  );
        // _sp.Add(  "BrickHouseA", "Casa de Ladrillos Mediana"  );
        // _sp.Add(  "BrickHouseB", "Casa de Ladrillos de Lujo"  );
        // _sp.Add(  "BrickHouseC", "Casa de Ladrillos Grande"  );

        //    //Farms
        //    //Animal
        // _sp.Add(  "AnimalFarmSmall","Granja Pequeña de Animales"  );
        // _sp.Add(  "AnimalFarmMed","Granja Mediana de Animales"  );
        // _sp.Add(  "AnimalFarmLarge","Granja Grande de Animales"  );
        // _sp.Add(  "AnimalFarmXLarge","Granja Enorme de Animales"  );
        //    //Fields
        // _sp.Add(  "FieldFarmSmall","Granja Pequeña de Cultivos"  );
        // _sp.Add(  "FieldFarmMed","Granja Mediana de Cultivos"  );
        // _sp.Add(  "FieldFarmLarge","Granja Grande de Cultivos"  );
        // _sp.Add(  "FieldFarmXLarge","Granja Enorme de Cultivos"  );
        // _sp.Add(  "FishingHut","Fishing Hut"  );

        //    //Raw
        // _sp.Add(  "Mortar","Mortero"  );
        // _sp.Add(  "Pottery","Taller de Porcelana"  );
        // _sp.Add(  "MountainMine","Mina"  );
        // _sp.Add(   "LumberMill" ,"Casa de Leñadores"  );
        // _sp.Add(  "BlackSmith","Herrero"  );
        // _sp.Add(  "ShoreMine","Shore Mine"  );
        // _sp.Add(  "QuickLime","Quicklime"  );

        // //Prod
        // _sp.Add(  "Brick","Ladrillo"  );
        // _sp.Add(  "Carpentry","Carpinteria"  );
        // _sp.Add(  "Cigars","Tabaqueria"  );
        // _sp.Add(  "Mill","Molino"  );
        // _sp.Add(  "Tailor","Sastre"  );
        // _sp.Add(  "Tilery","Tilery"  );
        // _sp.Add(  "Armory","Fabrica de Armas"  );
        // _sp.Add(  "Distillery","Destileria"  );
        // _sp.Add(  "Chocolate","Casa del Chocolate"  );
        // _sp.Add(  "Ink","Ink"  );

        //    //Ind
        // _sp.Add(  "Cloth","Telar"  );
        // //_sp.Add(  "GunPowder","Fabrica de Polvora"  );
        // _sp.Add(  "PaperMill","Fabirca de Papel"  );
        // _sp.Add(  "Printer","Imprenta"  );
        // _sp.Add(  "CoinStamp","Casa de la Moneda"  );
        // _sp.Add(  "SugarMill","Central Azucarero"  );
        // _sp.Add(  "Foundry","Fundicion"  );
        // _sp.Add(  "SteelFoundry","Fundicion de Aceros"  );
        // _sp.Add(  "SugarShop","Casa del Azucar"  );

        //    //trade
        // _sp.Add(  "Dock","Puerto"  );
        // _sp.Add(  "Shipyard","Astillero"  );
        // _sp.Add(  "Supplier","Suministrador"  );
        // _sp.Add(  "StorageSmall","Almacen Pequeña"  );
        // _sp.Add(  "StorageMed","Almacen Mediana"  );
        // _sp.Add(  "StorageBig","Almacen Grande"  );

        //    //gov
        // _sp.Add(  "Library","Biblioteca"  );
        // _sp.Add(  "School","Escuela"  );
        // _sp.Add(  "TradesSchool","Escuela de Oficios"  );
        // _sp.Add(  "TownHouse","Ayuntamiento"  );

        //    //other
        // _sp.Add(  "Church","Iglesia"  );
        // _sp.Add(  "Tavern","Taverna"  );

        //    //Militar
        // _sp.Add(  "WoodPost", "Torre de Madera"  );
        // _sp.Add(  "PostGuard","Torreon"  );
        // _sp.Add(  "Fort","Fuerte"  );
        // _sp.Add(  "Morro","Morro"  );

        //    //Decorations 
        // _sp.Add(  "Fountain", "Fuente"  );
        // _sp.Add(  "WideFountain","Fuente Grande"  );
        // _sp.Add(  "PalmTree","Palma"  );
        // _sp.Add(  "FloorFountain","Fuente Rasa"  );
        // _sp.Add(  "FlowerPot","Flores"  );
        // _sp.Add(  "PradoLion","Leon del Prado"  );






        //    //Main GUI
        // _sp.Add(  "SaveGame.Dialog", "Salva tu partida"  );
        // _sp.Add(  "LoadGame.Dialog", "Carga una partida"  );
        // _sp.Add(  "NameToSave", "Salva tu partida como:"  );
        // _sp.Add(  "NameToLoad", "La partida selecciona es:"  );
        // _sp.Add(  "OverWrite", "Ya existe un archivo con este nombre. Quieres sobre escribirlo?"  );
        // _sp.Add(  "DeleteDialog", "Estas seguro que quieres borrar esta partida?"  );
        // _sp.Add(  "NotHDDSpace", "Not hay espacio suficiente en torre {0} para salvar la partida"  );
        // _sp.Add(  "GameOverPirate", "Lo siento, perdiste el juego! Los piratas te atacaron y mataron a todos."  );
        // _sp.Add(  "GameOverMoney", "Lo siento, perdiste el juego! La corona no te ayudara mas con tu sueño Caribeño."  );
        // _sp.Add(  "BuyRegion.WithMoney", "Estas seguro que quieres comprar esta region."  );
        // _sp.Add(  "BuyRegion.WithOutMoney", "No tienes dinero para comprar esto ahora."  );
        // _sp.Add(  "Feedback", "Sugerencias si...:) Gracias. 8) "  );
        // _sp.Add(  "BugReport", "Bug, mandalo, gracias"  );
        // _sp.Add(  "Invitation", "Pon el email de un amigo, quizas sea invitado a la Beta"  );
        // _sp.Add(  "Info", ""  );//use for informational Dialogs








        //    //MainMenu

        // _sp.Add(  "Types_Explain", "Tradicional: \nEn este juego algunas construcciones estan  " +
        //                    "bloqueadas al principio y tienes que desbloquearlas. " +
        //        "Lo bueno es que asi tienes alguna manera de guiarte." +
        //        "\n\nFreewill: \nTodas las construcciones estan disponibles. " +
        //        "Lo malo es que puedes perder el juego mas facilmente."  );


        //    //Tooltips 
        //    //Small Tooltips 
        // _sp.Add(  "Person.HoverSmall", "Pers./Adul./Niñ."  );
        // _sp.Add(  "Emigrate.HoverSmall", "Emigrados"  );
        // _sp.Add(  "Lazy.HoverSmall", "Desempleados"  );
        // _sp.Add(  "Food.HoverSmall", "Comida"  );
        // _sp.Add(  "Happy.HoverSmall", "Felicidad"  );
        // _sp.Add(  "PortReputation.HoverSmall", "Reputacion Portuaria"  );
        // _sp.Add(  "Dollars.HoverSmall", "Dinero"  );
        // _sp.Add(  "PirateThreat.HoverSmall", "Amenaza Pirata"  );
        // _sp.Add(  "Date.HoverSmall", "Fecha (m/a)"  );
        // _sp.Add(  "MoreSpeed.HoverSmall", "Mas velocidad"  );
        // _sp.Add(  "LessSpeed.HoverSmall", "Menos velocidad"  );
        // _sp.Add(  "PauseSpeed.HoverSmall", "Pausa"  );
        // _sp.Add(  "CurrSpeedBack.HoverSmall", "Velocidad actual"  );
        // _sp.Add(  "ShowNoti.HoverSmall", "Notificaciones"  );
        // _sp.Add(  "Menu.HoverSmall", "Menu Principal"  );
        // _sp.Add(  "QuickSave.HoverSmall", "Salva rapida [F]"  );
        // _sp.Add(  "Bug Report.HoverSmall", "Reporte un bug"  );
        // _sp.Add(  "Feedback.HoverSmall", "Sugerencias"  );
        // _sp.Add(  "Hide.HoverSmall", "Esconder"  );
        // _sp.Add(  "CleanAll.HoverSmall", "Limpiar"  );
        // _sp.Add(  "Bulletin.HoverSmall", "Control/Boletin"  );
        // _sp.Add( "ShowAgainTuto.HoverSmall","Tutorial"  );
        // _sp.Add(  "Quest_Button.HoverSmall", "Desafios"  );
        // _sp.Add(  "TownTile.HoverSmall", "Nombre del pueblo"  );

        // _sp.Add(  "More.HoverSmall", "Mas"  );
        // _sp.Add(  "Less.HoverSmall", "Menos"  );

        // _sp.Add(  "BuyRegion.HoverSmall", "Compra region"  );
        // _sp.Add(  "BullDozer.HoverSmall", "Bulldozer"  );
        // _sp.Add(  "Help.HoverSmall", "Ayuda"  );


        //    //down bar
        // _sp.Add(  "Infrastructure.HoverSmall", "Infraestructuras"  );
        // _sp.Add(  "House.HoverSmall", "Casas"  );
        // _sp.Add(  "Farming.HoverSmall", "Fincas"  );
        // _sp.Add(  "Raw.HoverSmall", "Basico"  );
        // _sp.Add(  "Prod.HoverSmall", "Produccion"  );
        // _sp.Add(  "Ind.HoverSmall", "Industrias"  );
        // _sp.Add(  "Trade.HoverSmall", "Comercio"  );
        // _sp.Add(  "Gov.HoverSmall", "Govierno"  );
        // _sp.Add(  "Other.HoverSmall", "Otros"  );
        // _sp.Add(  "Militar.HoverSmall", "Militar"  );
        // _sp.Add(  "WhereIsTown.HoverSmall", "Centrar el pueblo [P]"  );
        // _sp.Add(  "WhereIsSea.HoverSmall", "Muestre/Esconda al mar"  );
        // _sp.Add(  "Helper.HoverSmall", "Mini ayuda"  );
        // _sp.Add(  "Tempeture.HoverSmall", "Temperatura"  );

        //    //building window
        // _sp.Add(  "Gen_Btn.HoverSmall", "General"  );
        // _sp.Add(  "Inv_Btn.HoverSmall", "Inventario"  );
        // _sp.Add(  "Upg_Btn.HoverSmall", "Mejoras"  );
        // _sp.Add(  "Prd_Btn.HoverSmall", "Produccion"  );
        // _sp.Add(  "Sta_Btn.HoverSmall", "Estadisticas"  );
        // _sp.Add(  "Ord_Btn.HoverSmall", "Ordenes"  );
        // _sp.Add(  "Stop_Production.HoverSmall", "Parar produccion"  );
        // _sp.Add(  "Demolish_Btn.HoverSmall", "Demoler"  );
        // _sp.Add(  "More Salary.HoverSmall", "Pagar mas"  );
        // _sp.Add(  "Less Salary.HoverSmall", "Pagar menos"  );
        // _sp.Add(  "Next_Stage_Btn.HoverSmall", "Compre"  );
        // _sp.Add(  "Current_Salary.HoverSmall", "Salario actual"  );
        // _sp.Add(  "Current_Positions.HoverSmall", "Posiciones actuales"  );
        // _sp.Add(  "Add_Import_Btn.HoverSmall", "Añade una importacion"  );
        // _sp.Add(  "Add_Export_Btn.HoverSmall", "Añade una exportacion"  );
        // _sp.Add(  "Upg_Cap_Btn.HoverSmall", "Mejora la capacidad"  );
        // _sp.Add(  "Close_Btn.HoverSmall", "Cerrar"  );
        // _sp.Add(  "ShowPath.HoverSmall", "Enseñar camino"  );
        // _sp.Add(  "ShowLocation.HoverSmall", "Enseñar lugar"  );
        // _sp.Add(  "Max_Positions.HoverSmall", "Max de trabajadores"  );
        // _sp.Add( "Rate.HoverSmall", "Porfa Evaluame"  );

        //    //addOrder windiw
        // _sp.Add(  "Amt_Tip.HoverSmall", "Cantidad de productos"  );

        //    //Med Tooltips 
        // _sp.Add(  "Build.HoverMed", "Fijar construccion: 'Click izquierdo' \n" +
        //                        "Rotar construccion: tecla 'R' \n " +
        //                        "Cancelar: 'Click derecho'"  );
        // _sp.Add(  "Current_Salary.HoverMed", "Los trabajadores prefieren trabajar donde se pague mas dinero." +
        //                                    " Si dos lugares pagan igual entonces escogeran el que este mas cerca a" +
        //                                    " casa."  );



        //    //Notifications
        // _sp.Add(  "BabyBorn.Noti.Name", "Recien nacido"  );
        // _sp.Add(  "BabyBorn.Noti.Desc", "Un niño a nacido que alegria"  );
        // _sp.Add(  "PirateUp.Noti.Name", "Los pirates se acercan"  );
        // _sp.Add(  "PirateUp.Noti.Desc", "Un barco de bandera pirata se ha visto cerca de la costa"  );
        // _sp.Add(  "PirateDown.Noti.Name", "Los piratas te temen"  );
        // _sp.Add(  "PirateDown.Noti.Desc", "Hoy los pirates te respetan un poco mas"  );

        // _sp.Add(  "Emigrate.Noti.Name", "Una persona a emigrado"  );
        // _sp.Add(  "Emigrate.Noti.Desc", "Las personas emigran cuando no estan felices"  );
        // _sp.Add(  "PortUp.Noti.Name", "El puerto de conoce"  );
        // _sp.Add(  "PortUp.Noti.Desc", "Tu puerto esta recibiendo mas atencion por los comerciantes"  );
        // _sp.Add(  "PortDown.Noti.Name", "Tu puerto es desconocido"  );
        // _sp.Add(  "PortDown.Noti.Desc", "Tu puerto se conoce cada vez menos entre los comerciantes"  );

        // _sp.Add(  "BoughtLand.Noti.Name", "Nuevo lote de tierra"  );
        // _sp.Add(  "BoughtLand.Noti.Desc", "Nuevo lote de tierra ha sido comprado"  );

        // _sp.Add(  "ShipPayed.Noti.Name", "Pago de comercio"  );
        // _sp.Add(  "ShipPayed.Noti.Desc", "Un barco a pagado por los bienes adquiridos en tu puerto"  );
        // _sp.Add(  "ShipArrived.Noti.Name", "Barco ha llegado"  );
        // _sp.Add(  "ShipArrived.Noti.Desc", "Un barco ha llegado a una de nuestras construcciones maritimas"  );

        // _sp.Add(  "AgeMajor.Noti.Name", "Un Trabajador Nuevo"  );
        // _sp.Add(  "AgeMajor.Noti.Desc", "{0} esta listo(a) para trabajar"  );


        // _sp.Add(  "PersonDie.Noti.Name", "Una persona ha muerto"  );
        // _sp.Add(  "PersonDie.Noti.Desc", "{0} ha muerto"  );

        // _sp.Add(  "DieReplacementFound.Noti.Name", "Una persona ha muerto"  );
        // _sp.Add(  "DieReplacementFound.Noti.Desc", "{0} ha muerto y ha sido reemplazado en su trabajo"  );

        // _sp.Add(  "DieReplacementNotFound.Noti.Name", "Una persona ha muerto"  );
        // _sp.Add(  "DieReplacementNotFound.Noti.Desc", "{0} ha muerto. No se encontro reemplazo en su trabajo"  );


        // _sp.Add(  "FullStore.Noti.Name", "Una almacen se esta llenando"  );
        // _sp.Add(  "FullStore.Noti.Desc", "Una almacen esta al {0}% de su capacidad"  );

        // _sp.Add(  "CantProduceBzFullStore.Noti.Name", "El trabajador no puede producir"  );
        // _sp.Add(  "CantProduceBzFullStore.Noti.Desc", "{0} el trabajador no puede producir porque su almacen esta llena"  );

        // _sp.Add(  "NoInput.Noti.Name", "Al menos un insumo falta en el edificio"  );
        // _sp.Add(  "NoInput.Noti.Desc", "Un edificio no produce {0} porque le falta aunque sea un insumo"  );

        // _sp.Add(  "Built.Noti.Name", "Una construcción ha sido terminada"  );
        // _sp.Add(  "Built.Noti.Desc", "{0} a sido construido(a)"  );

        // _sp.Add(  "cannot produce", "La producción se ha parado"  );






        //    //Main notificaion
        //    //Shows on the middle of the screen
        // _sp.Add(  "NotScaledOnFloor", "La construcción esta muy cerca al mar o una montaña"  );
        // _sp.Add(  "NotEven", "El piso no esta parejo en la base de la construccion"  );
        // _sp.Add(  "Colliding", "La construcción choca con otra"  );
        // _sp.Add(  "BadWaterHeight", "La construcción esta muy alta o muy baja en el agua"  );
        // _sp.Add(  "LockedRegion", "Necesitas ser dueño de esta tierra para construir aqui"  );
        // _sp.Add(  "HomeLess", "La gente en esta casa no tiene a donde ir. Por favor construye una" +
        //                    " nueva casa que pueda albegar a esta familia"  );
        // _sp.Add(  "LastFood", "No puedes destruir la unica almacen en la villa"  );
        // _sp.Add(  "LastMasonry", "No puedes destruir la unica casa de albañiles en la villa"  );


        //    //Mini help
        // _sp.Add(  "Camera", "Camara: Use [AWSD] or el cursor para mover. " +
        //                "Presione el boton del medio del raton para rotar camara, o [Q] y [E]"  );
        // _sp.Add(  "SeaPath", "Presione en el boton 'Mostrar al mar' " +
        //                    "y el camino mas cercano al mar sera mostrado"  );
        // _sp.Add(  "Region", "Region: Necesitas ser dueño de una region para construir en ella. " +
        //                "Presione en la señal 'Se vende' para comprar una"  );
        // _sp.Add(  "PeopleRange", "Rango: El circulo azul gigante es el rango de cada construccion"  );

        // _sp.Add(  "PirateThreat.Help", "Amenaza Pirata: Esto es cuan al dia estan los piratas con tu puerto. " +
        //                            "Se incrementa a medida que acumules mas dinero y riquezas. " +
        //                            "Si pasa 90 entonces pierdes el juego."  );

        // _sp.Add(  "PortReputation.Help", "Reputacion Portuaria: Mientras mas comerciantes y marineros conozcan tu puerto mas lo visitaran." +
        //                                    "Si quieres aumentar esto asegurate de que siempre haiga ordenes en tus construcciones maritimas" +
        //                                " (Puerto, Astillero, Abastecedor)"  );
        // _sp.Add(  "Emigrate.Help", "Emigrados: Cuando la gente esta infeliz por algunos años se van de tus tierras. " +
        //                        "Lo malo es que no viraran, produciran bienes o tedran niños jamas." +
        //                            "Lo bueno es que aumentan 'La Reputacion Portuaria'"  );
        // _sp.Add(  "Food.Help", "Comida: Mientras mas variedad de comidas las personas tengan mas felices seran."  );

        // _sp.Add(  "Weight.Help", "Peso: Todos los pesos en el juego estan en Kg o Lbs" +
        //                        " dependiendo en el sistema de unidad seleccionado." +
        //                        " Se puede cambiar en 'Opciones' en el 'Menu Principal'"  );



        // _sp.Add(  "More.Help", "Si necesita mas ayuda siempre es una buena idea pasar el tutorial, or o postear una pregunta en el Forum"  );

        //        //more 
        // _sp.Add(  "Products Expiration.Help", "Caducidad de productos: Como en la vida real los productos expiran. En la tabla the productos expirados se puede ver si alguno ha expirado Bulletin/Prod/Expire"  );
        // _sp.Add(  "Horse Carriages.Help", "Las personas con carretillas tiene limites de carga. Por eso estas carretas con caballos son usadas en el juego, ya que pueden cargar mucho mas. Como resultado la economia se mueve mas de prisa. Una persona carga alrededor de 15KG, un carretillero 60KG, y las carretas chicas hasta 240KG. Construye un HeavyLoad para usarlas"  );
        // _sp.Add(  "Usage of goods.Help", "Consumo de bienes: Cajas, barriles, carretillas, carretas, herramientas, ropa, ceramicas, muebles y utensillos son todos necesarios para mantener las actividades de la villa. A medida que estos bienes son usados disminuye la cantidad en el almacen, por ej. una persona no cargara nada si no hay cajas"  );
        // _sp.Add(  "Happiness.Help", "Felicidad: La felicidad de las personas esta influenciada por varios factores. Variedad de comidas, satisfacción religiosa, esparcimiento, confort de la casa, nivel de educacion, utensillos, ceramica y ropa."  );
        // _sp.Add(  "Line production.Help", "Linea de produccion: Para hacer un KG de puntillas tienes que encontrar y minar los minerales, en la fundicion derretir el hierro, y finalmente en el herrero hacer las puntillas. O simplemente comprarla en el puerto"  );
        // _sp.Add(  "Bulletin.Help", "El icono con las paginas en la barra infierior es la ventana de Bulletin/Control. Por favor toma un minuto para explorarla."  );
        // _sp.Add(  "Trading.Help", "Necesitas al menos un puerto para comerciar. En el puerto puedes agregar ordener de importacion y exportacion. Si necesitas mas ayuda puedes pasar el tutorial."  );

        // _sp.Add(  "Combat Mode.Help", "Se activa cuando un pirata o bandido es visto por uno de tus ciudadanos."  );

        // _sp.Add(  "Population.Help", "Cuando los jovenes cumplen 16 años se mudan a una casa vacia si existe. Si siempre hay casas vacias el crecimiento de la poblacion esta garantizado."  );

        // _sp.Add(  "F1.Help", "Presiona [F1] para ayuda"  );

        // _sp.Add(  "Inputs.Help", "Si un edificio no produce porque le faltan insumos, chequa que los insumos necesarios esten en la almacen y que tengas trabajadores en la Casa De Albañiles"  );




        // _sp.Add(  "WheelBarrows.Help", "Los carretilleros son los trabajadores de la Casa de Albañiles. Si ellos no tienen nada que hacer entonces haran el trabajo de carretilleros. Si necesitas algun insumo en un edificio, asegurate de tener bastantes de estos trabajando y por su puesto los insumos disponibles en la almacen"  );









        // _sp.Add(  "TutoOver", "Tu premio sera de $10,000 si es la 1era vez que completas el tutorial. Este es el fin del tutorial ahora puedes seguir jugando."  );

        //    //Tuto
        // _sp.Add(  "CamMov.Tuto", "El premio por completar el tutorial son $10,000 (solo un premio por juego). Paso 1: Usa [WASD] o las teclas del cursos para mover la camara. Haz esto por al menos 5 segundos"  );
        // _sp.Add(  "CamMov5x.Tuto", "Usa [WASD] o las teclas del cursos + 'Shift Izq' para mover la camara 5 veces mas rapido. Haz esto por al menos 5 segundos"  );
        // _sp.Add(  "CamRot.Tuto", "Presiona la rueda del raton y despues mueve el raton para girar la camara. Haz esto por al menos 5 segundos"  );

        // _sp.Add(  "BackToTown.Tuto", "Presiona la tecla [P] para ir al centro del pueblo"  );

        // _sp.Add("BuyRegion.Tuto", "Necesitas ser dueño de una region para poder construir en ella. Haz click en el signo de '+' en la barra inferior, despues en la señal de 'For Sale' " +
        //            " en una region para comprarla. Estas construcciones pueden ser construidas sin ser dueño de la region:" +
        //            " (FishingHut, Dock, MountainMine, ShoreMine, LightHouse, PostGuard)"
        //    );

        // _sp.Add(  "Trade.Tuto", "Eso fue facil ahora viene lo dificil. Haz click en 'Comercio', en la barra inferior. "+
        //        "Cuando pases el cursor del raton se vera que dice 'Comercio'"  );
        // _sp.Add(  "CamHeaven.Tuto", "Gira la rueda del raton hacia detras hasta que alcanzes el limite en el cielo. Esta vista es usada para emplazar grandes construcciones como el 'Puerto'"  );

        // _sp.Add(  "Dock.Tuto", "Haz click en la construcción 'Puerto'. Cuando pases el cursor del raton por encima del icono saldra su costo y descripcion"  );
        // _sp.Add(  "Dock.Placed.Tuto", "Ahora viene lo mas dificil. Puedes usar la tecla 'R' para rotar la construcción y click derecho para cancelar. "+
        //        " Esta construcción tiene una parte que va en tierra y otra en agua." +
        //        " La flecha debe ir en el agua, la sección del almacenaje en tierra. Cuando la flecha se ponga blanca haz click izq para emplazar el edificio."  );

        // _sp.Add(  "2XSpeed.Tuto", "Aumenta la velocidad del juego, en la parte superior de la pantalla en el centro, haz click en "
        //            +" 'Mas' hasta que aparezca el '2x'"  );

        // _sp.Add(  "ShowWorkersControl.Tuto", "Haz click en boton de 'Control/Boletin', en la parte inferior de la pantalla. "+
        //        "Si le pasas el cursor del raton por encima se vera 'Control/Boletin'"  );
        // _sp.Add(  "AddWorkers.Tuto", "Haz click en el signo de '+', Asi es como se añaden mas trabajadores."  );


        // _sp.Add(  "HideBulletin.Tuto", "En este formulario puedes controlar y ver varios aspectos de la partida. Haz click fuera del formulario o 'OK' para cerrarlo"  );
        // _sp.Add(  "FinishDock.Tuto", "Ahora termina el Puerto. Mientras mas trabajadores haiga en la Casa de Albañiles mas rapido se terminara."
        //    + " Tambien asegurate que tienes todos los materiales necesarios para construirlo."  );
        // _sp.Add(  "ShowHelp.Tuto", "Haz click en el boton de ayuda, en la barra inferior. "+
        //        " Aqui puedes encontrar la ayuda del juego."  );

        // _sp.Add(  "SelectDock.Tuto", "Los barcos escogen bienes al azar del inventario del puerto. Necesitas trabajadores para que muevan los bienes hacia y desde el puerto para las almacenes. Estos trabajadores utilizan cajas y carretllis para mover los bienes. Ahora haz click en el Puerto."  );
        // _sp.Add(  "OrderTab.Tuto", "Haz click en las Ordenes en el formulario del Puerto"  );
        // _sp.Add(  "ImportOrder.Tuto", "Haz click en el signo de '+' al lado de 'Orden de Importacion'"  );



        // _sp.Add(  "AddOrder.Tuto", "Ahora navega hasta el final de la lista y escoge 'Madera', pon la cantidad de 100. Despues haz click en el botton de 'Añadir'"  );
        // _sp.Add(  "CloseDockWindow.Tuto", "Ya la orden fue añadida. Un barco depositara estos bienes en el puerto. Despues tus trabajadores portuarios lo llevaran para la almacen mas cercana. Ahora cierra este formulario."  );
        // _sp.Add(  "Rename.Tuto", "Haz click en una persona y despues click en la barra de titulo del formulario. De esta manera le puedes cambiar el nombre a cualquier persona o edificio. Haz click afuera del titulo y los cambios seran guardados"  );
        // _sp.Add(  "RenameBuild.Tuto", "Now click on a building and change its name in the same way. Remember to click outside so the change is saved"  );

        // _sp.Add(  "BullDozer.Tuto", "Now click on the Bulldozer icon on the bottom bar. Then remove a tree or a rock from the terrain."  );


        // _sp.Add(  "Budget.Tuto", "Haz click en el boton 'Control/Boletin', despues en 'Finanzas' y despues en 'Presupuesto'"  );
        // _sp.Add(  "Prod.Tuto", "Haz click en el menu 'Prod' y despues en 'Producido'. Muestra lo producido en los ultimos 5 años"  );
        // _sp.Add(  "Spec.Tuto", "Haz click en el menu 'Prod' despues en 'Spec'. Aqui se ve exactamente como hacer todos los bienes en el juego. Los isumos necesarios, donde es producido y ademas el precio"  );
        // _sp.Add(  "Exports.Tuto", "Haz click en el menu 'Finanzas' y despues en 'Exportaciones'. Aqui se ve un sumario de las exportaciones"  );


        //        //Quest
        // _sp.Add(  "Tutorial.Quest", "Desafio: Termina el tutorial. $10,000 en premio. Toma alrededor de 3 minutos para ser completado"  );
        // _sp.Add(  "Lamp.Quest", "Desafio: Construye una farola. Esta en Infraestructuras, son encedidas de noche si hay Aceite de Ballena en la Almacen"  );
        // _sp.Add(  "Shack.Quest", "Desafio: Construye una casucha. Estas son casas baratas. Cuando las personas cumplen 16 años se mudan a un casa nueva si existe. De esta manera se garantiza el crecimiento de la poblacion. [F1] Ayuda"  );
        // _sp.Add(  "SmallFarm.Quest", "Desafio: Construye una Finca de Cultivos Chica. Necesitas estas para alimentar a tu pueblo"  );
        // _sp.Add(  "FarmHire.Quest", "Desafio: Contrata a dos granjeros en la Finca de Cultivos Chica. Haz click en la finca y despues en el signo de mas para asignar trabajadores. Para esto necesitas tener trabajadores desempleados"  );
        // _sp.Add(  "FarmProduce.Quest", "Desafio: Produce " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " de Frijol en la Finca de Cultivos Chica. Haz click en la pestaña 'Stat' y te mostrara la producción de los ultimos 5 años. Puedes ver el avance en el desafio en el formulario de desafios. Si construyes mas Fincas de Cultivos Chica ayudaran a pasar este desafio"  );
        // _sp.Add(  "Transport.Quest", "Desafio: Transporta el Frijol de la Finca hacia la Almacen. Para hacer esto asegurate de que hay trabajadores en la Casa de Albañiles. Ellos se convierten en carretilleros cuando no trabajan"  );
        // _sp.Add(  "Export.Quest", "Desafio: Exporta 300 " + Unit.CurrentWeightUnitsString() + " de Frijol. Añade una orden de Exportacion en el Puerto. Si no tienes un Puerto entonces construye uno."+
        //        "El icono del Puerto esta en Comercio. Cuando este hecho haz click en la pestaña de ordenes, añade una orden de exportacion, y selecciona el producto y la cantidad a exportar."  );
        // _sp.Add(  "HireDocker.Quest", "Desafio: Contrata un portuario. La unica tarea de ellos es mover bienes desde el Almacen hacia el Puerto si estas exportando."+
        //        " O vice-versa si estas importando. Ellos trabajan cuando hay ordenes en el puerto y los bienes estan listos para su transporte. Sino se quedan en casa descanzando." +
        //            " Si ya tienes trabajadores aqui despidelos a todos y despues contrata a uno de nuevo."  );
        // _sp.Add(  "MakeBucks.Quest", "Desafio: Haz $100 exportando bienes en el Puerto. "+
        //        "Cuando un barco llegue pagara bienes al azar que haiga en las bodegas de tu Puerto"  );
        // _sp.Add(  "HeavyLoad.Quest", "Desafio: Construye el edificio de Carga Pesada. Estos son transportistas que cargan mas peso. Seran muy utiles cuando mucha carga necesita ser transportada en tu villa"  );
        // _sp.Add(  "ImportOil.Quest", "Desafio: Importa 500 " + Unit.CurrentWeightUnitsString() + " de Aceite de Ballena en el Puerto. Es necesario para encender las Farolas por las noches."  );
        // _sp.Add(  "Population50.Quest", "Obten 50 personas en total"  );


        //        //added Aug 11 2017, result: sep 9(30% off biggest sale ever)
        // _sp.Add(  "Production.Quest", "Ahora vamos a producir armas que despues venderemos. Primero construye un Herrero. Encuentralo en el menu 'Basico'"  );




        // _sp.Add(  "ChangeProductToWeapon.Quest", "En el Herrero(Blacksmith), click en la pestaña de 'Productos(Products)' y cambien la producción a 'Armas(Weapon)'. Los trabajadores traeran los materiales necesarios si estan disponibles"  );
        // _sp.Add(  "BlackSmithHire.Quest", "Contrata a dos herreros"  );
        // _sp.Add(  "WeaponsProduce.Quest", "Ahora produce " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " de Armas en el Herrero. Click en 'Stat', para que veas un reporte productivo de los ultimos 5 años. Puedes ver el avance del reto en la ventana 'Retos'."  );
        // _sp.Add(  "ExportWeapons.Quest", "Ahora exporta 100 " + Unit.CurrentWeightUnitsString() + " de Armas. En el Puerto añade una orden de exportacion. Te daras cuenta que hacer Armas es un negocio con buen lucro"  );

        //    //added Sep 14 2017
        // _sp.Add(  "BuildFishingHut.Quest", "Construye un pesquero. De esta manera los ciudadanos tienen variedad de comidas para comer y seran mas felices"  );
        // _sp.Add(  "HireFisher.Quest", "Contrata a un pescador"  );

        // _sp.Add(  "BuildLumber.Quest", "Construya una 'Casa de Leñadores(Lumbermill)'. Encuentralo en el menu 'Raw'"  );
        // _sp.Add(  "HireLumberJack.Quest", "Contrata a un leñador"  );

        // _sp.Add(  "BuildGunPowder.Quest", "Construye una Fabrica de Polvora. Esta en el menu de construcciones 'Industrias'"  );
        // _sp.Add(  "ImportSulfur.Quest", "En el Puerto importa " + Unit.CurrentWeightUnitsString() + " de Azufre"  );
        // _sp.Add(  "GunPowderHire.Quest", "Contrata a un trabajador en la Fabrica de Polvora"  );

        // _sp.Add(  "ImportPotassium.Quest", "En el Puerto importa " + Unit.CurrentWeightUnitsString() + " de Potasio"  );
        // _sp.Add(  "ImportCoal.Quest", "En el Puerto importa " + Unit.CurrentWeightUnitsString() + " de Carbon"  );

        // _sp.Add(  "ProduceGunPowder.Quest", "Produce ahora " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " de Polvora. Necesitaras Azufre, Potasio y Carbon para producir Polvora."  );
        // _sp.Add(  "ExportGunPowder.Quest", "En el Puerto exporta " + Unit.CurrentWeightUnitsString() + " de Polvora."  );

        // _sp.Add(  "BuildLargeShack.Quest", "Construye un Largeshack, con casas mas grandes la poblacion aumentara mas rapido."  );

        // _sp.Add(  "BuildA2ndDock.Quest", "Construye otro Puerto mas. Este puerto lo pudieses usar solo para importar, de esa manera importas materias primas y exportas bienes producidos en el otro Puerto"  );
        // _sp.Add(  "Rename2ndDock.Quest", "Renombra los Puertos, asi recordaras cual usar para importar y exportar."  );

        // _sp.Add(  "Import2000Wood.Quest", "En el Puerto de Importaciones importa 2000 " + Unit.CurrentWeightUnitsString() + " de Madera. Esta materia prima se usa para todo porque se usa como combustible."  );

        //    //IT HAS FINAL MESSAGE 
        //    //last quest it has a final message to the player. if new quest added please put the final message in the last quest
        // _sp.Add(  "Import2000Coal.Quest", "En el Puerto de Importaciones importa 2000 " + Unit.CurrentWeightUnitsString() + " de Carbon. El Carbon tambien se puede usar como combustible. Espero estes disfutamdo el juego hasta ahora. Sigue expandiendo tu colonia y riquezas. Por favor tambien ayuda a mejorar el juego. Participa en el forum de Steam y deja tus sugerencias y cualquier ideas que tengas para mejorar el juego. Diviertete!"  );
        //    //

        // _sp.Add(  "CompleteQuest", "Tu premio es de ${0}"  );









        //    //Quest Titles
        // _sp.Add(  "Tutorial.Quest.Title", "Tutorial"  );
        // _sp.Add(  "Lamp.Quest.Title", "Lampara de Calle"  );

        // _sp.Add(  "Shack.Quest.Title", "Construye una Casucha"  );
        // _sp.Add(  "SmallFarm.Quest.Title", "Construye una Granja Pequeña"  );
        // _sp.Add(  "FarmHire.Quest.Title", "Contrata dos Granjeros"  );


        // _sp.Add(  "FarmProduce.Quest.Title", "Productor agricola"  );

        // _sp.Add(  "Export.Quest.Title", "Exportaciones"  );
        // _sp.Add(  "HireDocker.Quest.Title", "Contratacion en el Puero"  );
        // _sp.Add(  "MakeBucks.Quest.Title", "Haz dinero"  );
        // _sp.Add(  "HeavyLoad.Quest.Title", "Carga pesada"  );
        // _sp.Add(  "HireHeavy.Quest.Title", "Contrata a un cochero"  );

        // _sp.Add(  "ImportOil.Quest.Title", "Aceite de ballena"  );

        // _sp.Add(  "Population50.Quest.Title", "50 Ciudadanos"  );

        //    //
        // _sp.Add(  "Production.Quest.Title", "Produce Armas"  );
        // _sp.Add(  "ChangeProductToWeapon.Quest.Title", "Cambia el Producto"  );
        // _sp.Add(  "BlackSmithHire.Quest.Title", "Contrata dos herreros"  );
        // _sp.Add(  "WeaponsProduce.Quest.Title", "Produce Armas"  );
        // _sp.Add(  "ExportWeapons.Quest.Title", "Ganancias"  );

        //    //
        // _sp.Add(  "BuildFishingHut.Quest.Title", "Construye Casa de Pescadores"  );
        // _sp.Add(  "HireFisher.Quest.Title", "Contrata a un Pescador"  );
        // _sp.Add(  "BuildLumber.Quest.Title", "Construye Casa de Leñadores"  );
        // _sp.Add(  "HireLumberJack.Quest.Title", "Contrata a un Leñador"  );
        // _sp.Add(  "BuildGunPowder.Quest.Title", "Construye Fabrica de Polvora"  );
        // _sp.Add(  "ImportSulfur.Quest.Title", "Importa Azufre(Sulfur)"  );
        // _sp.Add(  "GunPowderHire.Quest.Title", "Contrata"  );
        // _sp.Add(  "ImportPotassium.Quest.Title", "Importa Potasio"  );
        // _sp.Add(  "ImportCoal.Quest.Title", "Importa Carbon"  );
        // _sp.Add(  "ProduceGunPowder.Quest.Title", "Produce Polvoara"  );
        // _sp.Add(  "ExportGunPowder.Quest.Title", "Exporta Polvoara"  );
        // _sp.Add(  "BuildLargeShack.Quest.Title", "Construye un Largeshack"  );
        // _sp.Add(  "BuildA2ndDock.Quest.Title", "Construye un otro Puerto"  );
        // _sp.Add(  "Rename2ndDock.Quest.Title", "Renombra en nuevo Puerto"  );
        // _sp.Add(  "Import2000Wood.Quest.Title", "Importa Madera"  );
        // _sp.Add(  "Import2000Coal.Quest.Title", "Importa Carbon"  );












        //    //Main Menu
        // _sp.Add(  "Resume Game", "Sigue el Juego"  );
        // _sp.Add(  "Continue Game", "Continuar el Juego"  );
        // _sp.Add(  "New Game", "Juego Nuevo"  );
        // _sp.Add(  "Load Game", "Cargar Juego"  );
        // _sp.Add(  "Save Game", "Salvar Juego"  );
        // _sp.Add(  "Options", "Opciones"  );
        // _sp.Add(  "Credits", "Creditos"  );
        // _sp.Add(  "Exit", "Salir del Juego"  );
        // _sp.Add(  "Achievements", "Logros"  );
        // _sp.Add(  "Town Name:", "Nombre del Pueblo:"  );
        // _sp.Add(  "OK", "OK"  );
        // _sp.Add(  "Cancel", "Cancelar"  );
        // _sp.Add(  "Enter name...", "Escribe el nombre..."  );
        // _sp.Add(  "Terrain Name:", "Nombre del terreno:"  );
        // _sp.Add(  "Difficulty:", "Dificultad:"  );
        // _sp.Add(  "Type of game:", "Tipo de juego:"  );

        // _sp.Add(  "Pirates (if check the town could suffer a Pirate attack)", "Piratas (El pueblo pudiese sufrir el ataque de piratas)"  );
        // _sp.Add(  "Food Expiration (if check food expires with time)", "Caducidad de la comida (La comida tiene fecha de caducidad)"  );

        // _sp.Add(  "Freewill", "Libertad"  );
        // _sp.Add(  "Traditional", "Tradicional"  );


        // _sp.Add(  "Click Here", "Haz click aquí"  );

        // _sp.Add(  "Newbie", "Novato"  );
        // _sp.Add(  "Easy", "Fácil"  );
        // _sp.Add(  "Moderate", "Mas o menos"  );
        // _sp.Add(  "Hard", "Duro"  );
        // _sp.Add(  "Insane", "Locura"  );

        // _sp.Add(  "Save Name:", "Nombre de la Salva:"  );
        // _sp.Add(  "Delete", "Borra"  );
        // _sp.Add(  "FullScreen:", "Pantalla completa:"  );
        // _sp.Add(  "Quality:", "Calidad:"  );
        // _sp.Add(  "Resolution:", "Resolucion:"  );
        // _sp.Add(  "Screen", "Pantalla"  );

        // _sp.Add(  "Music:", "Musica:"  );
        // _sp.Add(  "Audio", "Audio"  );
        // _sp.Add(  "Sound:", "Sonido:"  );
        // _sp.Add(  "General", "General"  );
        // _sp.Add(  "Unit System:", "Sistema de unidades:"  );

        // _sp.Add(  "AutoSave Frec:", "Frecuencia de auto salva:"  );
        // _sp.Add(  "Language:", "Lenguage:"  );
        //    //{ "Loading...", "Cargando..."  );
        // _sp.Add(  "Menu", "Menu"  );

        // _sp.Add(  "Camera Sensitivity:", "Velocidad de la Camara:"  );

        //    //
        // _sp.Add(  "Tutorial(Beta)", "Tutorial(Beta)"  );




        //    //in game
        // _sp.Add(  "Buildings.Ready", "\n Edificios listos para ser construidos:"  );
        // _sp.Add(  "People.Living", "Personas en esta casa:"  );
        // _sp.Add(  "Occupied:", "En uso:"  );
        // _sp.Add(  "|| Capacity:", "|| Capacidad:"  );
        // _sp.Add(  "Users:", "\nUsuarios:"  );
        // _sp.Add(  "Amt.Cant.Be.0", "La cantidad no puede ser zero."  );
        // _sp.Add(  "Prod.Not.Select", "Por favor seleccione un producto"  );

        // _sp.Add(  "Orders in progress:", "Ordenes en progreso:"  );

        //    //articles
        // _sp.Add(  "The.Male", "El"  );
        // _sp.Add(  "The.Female", "La"  );

        //    //
        // _sp.Add(  "Build.Destroy.Soon", "Esta construccion sera destruida. Si el inventorio no esta vacio los carretilleros deberan hacerlo."  );













        //    //words
        //    //Field Farms
        // _sp.Add(  "Bean", "Frijol"  );
        // _sp.Add(  "Potato", "Papa"  );
        // _sp.Add(  "SugarCane", "Caña"  );
        // _sp.Add(  "Corn", "Maiz"  );
        // _sp.Add(  "Cotton", "Algodon"  );
        // _sp.Add(  "Banana", "Platano"  );
        // _sp.Add(  "Coconut", "Coco"  );
        //    //Animal Farm
        // _sp.Add(  "Chicken", "Pollo"  );
        // _sp.Add(  "Egg", "Huevo"  );
        // _sp.Add(  "Pork", "Cerdo"  );
        // _sp.Add(  "Beef", "Res"  );
        // _sp.Add(  "Leather", "Cuero"  );
        // _sp.Add(  "Fish", "Pescado"  );
        //    //mines
        // _sp.Add(  "Gold", "Oro"  );
        // _sp.Add(  "Stone", "Piedra"  );
        // _sp.Add(  "Iron", "Hierro"  );

        // _sp.Add(  "Clay", "Arcilla"  );
        // _sp.Add(  "Ceramic", "Ceramica"  );
        // _sp.Add(  "Wood", "Madera"  );

        //    //Prod
        // _sp.Add(  "Tool", "Herramienta"  );
        // _sp.Add(  "Tonel", "Tonel"  );
        // _sp.Add(  "Cigar", "Tabaco"  );
        // _sp.Add(  "Tile", "Loza"  );
        // _sp.Add(  "Fabric", "Tejido"  );
        // //_sp.Add(  "GunPowder", "Polvora"  );
        // _sp.Add(  "Paper", "Papel"  );
        // _sp.Add(  "Map", "Mapa"  );
        // _sp.Add(  "Book", "Libro"  );
        // _sp.Add(  "Sugar", "Azucar"  );
        // _sp.Add(  "None", "Ninguno"  );
        //    //
        // _sp.Add(  "Person", "Persona"  );
        // _sp.Add(  "Food", "Comida"  );
        // _sp.Add(  "Dollar", "Dollar"  );
        // _sp.Add(  "Salt", "Sal"  );
        // _sp.Add(  "Coal", "Carbon"  );
        // _sp.Add(  "Sulfur", "Sulfuro"  );
        // _sp.Add(  "Potassium", "Potasio"  );
        // _sp.Add(  "Silver", "Plata"  );
        // _sp.Add(  "Henequen", "Henequen"  );
        //    //
        // _sp.Add(  "Sail", "Vela"  );
        // _sp.Add(  "String", "Cuerda"  );
        // _sp.Add(  "Nail", "Puntilla"  );
        // _sp.Add(  "CannonBall", "Bola de cañon"  );
        // _sp.Add(  "TobaccoLeaf", "Hoja de tabaco"  );
        // _sp.Add(  "CoffeeBean", "Grano de cafe"  );
        // _sp.Add(  "Cacao", "Cocoa"  );
        // _sp.Add(  "Chocolate", "Chocolate"  );
        // _sp.Add(  "Weapon", "Arma"  );
        // _sp.Add(  "WheelBarrow", "Carretilla"  );
        //    //
        // _sp.Add(  "Diamond", "Diamante"  );
        // _sp.Add(  "Jewel", "Joya"  );
        // _sp.Add(  "Cloth", "Ropa"  );
        // _sp.Add(  "Rum", "Ron"  );
        // _sp.Add(  "Wine", "Vino"  );
        // _sp.Add(  "Ore", "Mineral"  );
        // _sp.Add(  "Crate", "Caja"  );
        // _sp.Add(  "Coin", "Moneda"  );
        // _sp.Add(  "CannonPart", "Pieza de cañon"  );
        // _sp.Add(  "Ink", "Tinta"  );
        // _sp.Add(  "Steel", "Acero"  );
        //    //
        // _sp.Add(  "CornFlower", "Harina de castilla"  );
        // _sp.Add(  "Bread", "Pan"  );
        // _sp.Add(  "Carrot", "Zanahoria"  );
        // _sp.Add(  "Tomato", "Tomate"  );
        // _sp.Add(  "Cucumber", "Pepino"  );
        // _sp.Add(  "Cabbage", "Col"  );
        // _sp.Add(  "Lettuce", "Lechuga"  );
        // _sp.Add(  "SweetPotato", "Boniato"  );
        // _sp.Add(  "Yucca", "Yuca"  );
        // _sp.Add(  "Pineapple", "Piña"  );
        //    //
        // _sp.Add(  "Papaya", "Fruta bomba"  );
        // _sp.Add(  "Wool", "Lana"  );
        // _sp.Add(  "Shoe", "Zapato"  );
        // _sp.Add(  "CigarBox", "Caja de tabaco"  );
        // _sp.Add(  "Water", "Agua"  );
        // _sp.Add(  "Beer", "Cerveza"  );
        // _sp.Add(  "Honey", "Miel"  );
        // _sp.Add(  "Bucket", "Cubo"  );
        // _sp.Add(  "Cart", "Carreta"  );
        // _sp.Add(  "RoofTile", "Teja"  );
        // _sp.Add(  "FloorTile", "Azulejo"  );
        // _sp.Add(  "Mortar", "Mezcla"  );
        // _sp.Add(  "Furniture", "Muebless"  );

        // _sp.Add(  "Utensil", "Utensillo"  );
        // _sp.Add(  "Stop", "Pare"  );


        //    //more Main GUI
        // _sp.Add(  "Workers distribution", "Distribucion de los trabajadores"  );
        // _sp.Add(  "Buildings", "Construcciones"  );

        // _sp.Add(  "Age", "Edad"  );
        // _sp.Add(  "Gender", "Genero"  );
        // _sp.Add(  "Height", "Altura"  );
        // _sp.Add(  "Weight", "Peso"  );
        // _sp.Add(  "Calories", "Calorias"  );
        // _sp.Add(  "Nutrition", "Nutricion"  );
        // _sp.Add(  "Profession", "Profesion"  );
        // _sp.Add(  "Spouse", "Conyugue"  );
        // _sp.Add(  "Happinness", "Felicidad"  );
        // _sp.Add(  "Years Of School", "Años de escuela"  );
        // _sp.Add(  "Age majority reach", "Mayor de edad"  );
        // _sp.Add(  "Home", "Hogar"  );
        // _sp.Add(  "Work", "Trabajo"  );
        // _sp.Add(  "Food Source", "Almacen"  );
        // _sp.Add(  "Religion", "Religion"  );
        // _sp.Add(  "Chill", "Relajamiento"  );
        // _sp.Add(  "Thirst", "Sed"  );
        // _sp.Add(  "Account", "Cuenta"  );

        // _sp.Add(  "Early Access Build", "Acceso Anticipado"  );








        //ProductStat.cs has a lot of text to put here 

        //SpecTile.cs has

        //ShowAPersonBuildingDetails has
        //BuildingWindow.cs
        //Dispatch.cs
        //ButtonTile.cs
        //Plant.cs
        //GameTime.cs
        //Profession.cs
        //GUIElement.cs
        //OrderShow.cs
































        ////PORTUGUESE
        //_portuguese = new Dictionary<string, string>()
        //{
        //    //words
        //    //Field Farms
        //    { "Bean", "Feijão"},
        //    { "Potato", "Batata"},
        //    { "SugarCane", "Cana"},
        //    { "Corn", "Milho"},
        //    { "Cotton", "Algodão"},
        //    { "Banana", "Banana"},
        //    { "Coconut", "Coco"},
        //    //Animal Farm
        //    { "Chicken", "Frango"},
        //    { "Egg", "Ovo"},
        //    { "Pork", "Poroc"},
        //    { "Beef", "Beef"},
        //    { "Leather", "Couro"},
        //    { "Fish", "Peixe"},
        //    //mines
        //    { "Gold", "Gold"},
        //    { "Stone", "Pedra"},
        //    { "Iron", "Ferro"},

        //    { "Clay", "Argila"},
        //    { "Ceramic", "Cerâmica"},
        //    { "Wood", "Madeira"},

        //    //Prod
        //    { "Tool", "Ferramenta"},
        //  { "Brick", "Tijolo"},
        //    { "Tonel", "Tonel"},
        //    { "Cigar", "Charuto"},
        //    { "Tile", "Azulejo"},
        //    { "Fabric", "Tecido"},
        //    { "GunPowder", "Pólvora"},
        //    { "Paper", "Papel"},
        //    { "Map", "Mapa"},
        //    { "Book", "Livro"},
        //    { "Sugar", "Sugar"},
        //    { "None", "Nenhum"},
        //    //
        //    { "Person", "Pessoa"},
        //    { "Food", "Comida"},
        //    { "Dollar", "Dollar"},
        //    { "Salt", "Salt"},
        //    { "Coal", "Carvão"},
        //    { "Sulfur", "Enxofre"},
        //    { "Potassium", "Potássio"},
        //    { "Silver", "Silver"},
        //    { "Henequen", "Henequen"},
        //    //
        //    { "Sail", "Vela"},
        //    { "String", "Corda"},
        //    { "Nail", "Prego"},
        //    { "CannonBall", "Cannonball"},
        //    { "TobaccoLeaf", "Tobaccoleaf"},
        //    { "CoffeeBean", "Coffeebean"},
        //    { "Cacao", "Cacau"},
        //    { "Chocolate", "Chocolate"},
        //    { "Weapon", "Arma"},
        //    { "WheelBarrow", "Carrinho de Mão"},
        //    //
        //    { "Diamond", "Diamond"},
        //    { "Jewel", "Jewel"},
        //    { "Cloth", "Pano"},
        //    { "Rum", "Rum"},
        //    { "Wine", "Vihno"},
        //    { "Ore", "Ore"},
        //    { "Crate", "Caixa"},
        //    { "Coin", "Coin"},
        //    { "CannonPart", "Cannon Part"},
        //    { "Ink", "Ink"},
        //    { "Steel", "Steel"},
        //    //
        //    { "CornFlower", "Cornflower"},
        //    { "Bread", "Pão"},
        //    { "Carrot", "Cenoura"},
        //    { "Tomato", "Tomate"},
        //    { "Cucumber", "Pepino"},
        //    { "Cabbage", "Repolho"},
        //    { "Lettuce", "Alface"},
        //    { "SweetPotato", "Batata Doce"},
        //    { "Yucca", "Yucca"},
        //    { "Pineapple", "Abacaxi"},
        //    //
        //    { "Papaya", "Mamão"},
        //    { "Wool", "Lã"},
        //    { "Shoe", "Shoe"},
        //    { "CigarBox", "Cigarbox"},
        //    { "Water", "Água"},
        //    { "Beer", "Cerveja"},
        //    { "Honey", "Mel"},
        //    { "Bucket", "Bucket"},
        //    { "Cart", "Carrinho"},
        //    { "RoofTile", "Rooftile"},
        //    { "FloorTile", "Floortile"},
        //  { "Mortar", "Argamassa"},
        //    { "Furniture", "Mobiliário"},

        //    { "Utensil", "Utensílio"},
        //    { "Stop", "Para"},


        //    //more Main GUI
        //    { "Workers distribution", "Distribuição de Trabalhadores"},
        //    { "Buildings", "Edifícios"},

        //    { "Age", "Idade"},
        //    { "Gender", "Sexo"},
        //    { "Height", "Altura"},
        //    { "Weight", "Peso"},
        //    { "Calories", "Calorias"},
        //    { "Nutrition", "Nutrição"},
        //    { "Profession", "Profissão"},
        //    { "Spouse", "Cônjuge"},
        //    { "Happinness", "Happinnes"},
        //    { "Years Of School", "Anos de Escola"},
        //    { "Age majority reach", "Idade maioria alcance"},
        //    { "Home", "Home"},
        //    { "Work", "Trabalho"},
        //    { "Food Source", "Fonte de Alimento"},
        //    { "Religion", "Religião"},
        //    { "Chill", "Chilll"},
        //    { "Thirst", "Thirst"},
        //    { "Account", "Conta"},

        //    { "Early Access Build", "Early Access Build"},
        //};




    }






    public static string ReturnString(string key)
    {
        ReloadIfNeeded();

        if (_currentLang == "English")
        {
            if (_en.ContainsKey(key))
            {
                return _en.ReturnValueWithKey(key);
            }
            //in English if key is not found will return key and in the start of the word 'en:'
            //ex: if the word: 'Knee' is passed as key and not found will return 'en:Knee' so you know it needs to be added
            //to the English dictionary
            // return "en:" + key;
            return key;
        }
        else if (_currentLang == "Español(Beta)")
        {
            if (_sp.ContainsKey(key))
            {
                return _sp.ReturnValueWithKey(key);
            }
            return "es:" + key;
        }
        else if (_currentLang == "Português(Beta)")
        {
            if (_portuguese.ContainsKey(key))
            {
                return _portuguese[key];
            }
            return "pt:" + key;
        }
        else if (_currentLang == "Français(Beta)")
        {
            if (French.ContainsKey(key))
            {
                return French.Dictionary()[key];
            }
            return "fr:" + key;
        }
        return "not languages selected ";
    }

    private static void ReloadIfNeeded()
    {
        if (_en.Count == 0)
        {
            ReloadDict();
            French.ReloadDict();
        }
    }

    public static void ResetLanguages()
    {
        _en.Clear();
        _sp.Clear();
        _portuguese.Clear();
        French.Clear();
    }

    public static void SetCurrentLang(string lang)
    {
        _currentLang = lang;
        AudioCollector.InitSpecPeoplesList();
    }

    internal static string CurrentLang()
    {
        return _currentLang;
    }

    internal static List<string> AllLang()
    {
        return new List<string>() { "English", "Spanish" };
    }

    internal static bool DoIHaveHoverMed(string key)
    {
        var currentLang = ReturnCurrentDict();

        return currentLang.ContainsKey(key + ".HoverMed");
    }

    static Dictionary<string, string> ReturnCurrentDict()
    {
        if (_currentLang == "Español")
        {
            return _sp.Dictionary;
        }
        if (_currentLang == "Português(Beta)") return _portuguese;
        if (_currentLang == "Français(Beta)") return French.Dictionary();

        return _en.Dictionary;

    }
}
