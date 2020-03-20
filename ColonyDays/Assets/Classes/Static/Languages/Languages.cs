using System.Collections.Generic;

public class Languages
{
    private static string _currentLang = "English";
    private static LangDict _sp = new LangDict();
    private static LangDict _fr = new LangDict();
    private static LangDict _de = new LangDict();
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
        _en.Add("Date.HoverSmall", "Date (Mmm/Y)");
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

        _en.Add("Construction.HoverMed", "For the construction of any building you need to have workers in the Masonry." +
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
        _en.Add("What is Ft3 and M3?.Help", "The storage capacity is determined by the volume of the building. Ft3 is a cubic foot. M3 is a cubic meter. Keep in mind that less dense products will fill up your storage quickly."); 
        // To see products density Bulletin/Prod/Spec" );

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
        _en.Add("Rename.Tuto", "Click on a person and then click on the title bar of the person. Like this, you can change the name of any person or building in the game. Move outside of the input box so the change is saved");
        _en.Add("RenameBuild.Tuto", "Now click on a building and change its name in the same way. Remember to move outside of the input box so the change is saved");

        _en.Add("BullDozer.Tuto", "Now click on the Bulldozer icon on the bottom bar. Then remove a tree or a rock from the terrain.");


        _en.Add("Budget.Tuto", "Click on the 'Control/Bulletin' button, then on 'Finance' menu and then on 'Ledger'. This is the game ledger");
        _en.Add("Prod.Tuto", "Click on 'Prod' menu and then on 'Produce'. Will show the village's production for the last 5 years");
        _en.Add("Spec.Tuto", "Click the 'Prod' menu and then on 'Spec'. Here you can see exactly how to make each product on the game. The inputs necessaries and where is produced. Also, the import and export prices");
        _en.Add("Exports.Tuto", "Click the 'Finance' menu and then on 'Export'. Here you can see a breakdown of your village's exports");


        //Quest
        _en.Add("Tutorial.Quest", "Quest: Finish the tutorial. Reward $10,000. It takes roughly 3 minutes to complete");

        _en.Add("Lamp.Quest", "Quest: Build a Stand Lamp. Find it on Infrastructure, it shines at night if there is Whale Oil on Storage");

        _en.Add("Shack.Quest", "Quest: Build a Shack. These are cheap houses. When people turn 16 they will move to a free house if found. In this way, population growth will be guaranteed. [F1] Help. If you see smoke in a house's chimney means there are people living in it");

        _en.Add("SmallFarm.Quest", "Quest: Build a Small Field Farm. You need farms to feed your people");
        _en.Add("FarmHire.Quest", "Quest: Hire two farmers in the Small Field Farm. Click on the farm and assign workers. You need to have unemployed"
                    + " people to be able to assign them into a new building. Tip: if you did it already, fire them and rehire them.");

        _en.Add("FarmProduce.Quest", "Quest: Now produce " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " of beans on the Small Field Farm. Click on the 'Stat' tab and will show you the production of the last 5 years. You can see the quest progress in the quest window. If you build more small farms will be accounted for the quest.");
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


        _en.Add("CompleteQuest", "Nice :) Well done! Your reward is {0}");


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
        _en.Add("Tutorial.Arrow", "This is the tutorial. Once finished you will be rewarded $10,000");
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
        _en.Add("Tutorial", "Tutorial");
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

        //After Dec 28, 2018
        _en.Add("Down.HoverSmall", "Decrease Priority");
        _en.Add("Up.HoverSmall", "Increase Priority");
        _en.Add("Trash.HoverSmall", "Delete Order");
        _en.Add("Counting...", "Counting...");
        _en.Add("Ten Orders Limit", "Ten orders is the limit");

        //After May 1, 2019
        _en.Add("Our inventories:", "Our inventories:");
        _en.Add("Select Product:", "Select Product:");
        _en.Add("Current_Rank.HoverSmall", "Number in Queue");

        _en.Add("Construction.Progress", "Construction progress at: ");
        _en.Add("Warning.This.Building", "Warning: This building cannot be built now. Missing resource(s):\n");
        _en.Add("Product.Selected", "Product selected: ");
        _en.Add("Harvest.Date", "\nHarvest date: ");
        _en.Add("Progress", "\nProgress: ");

        //AddOrderWindow.cs
        _en.Add("Add.New", "Add New ");
        _en.Add("Order", " Order");
        _en.Add("Import", "Import");
        _en.Add("Export", "Export");
        //AddOrderWindow GUI
        _en.Add("Enter Amount:", "Enter Amount:");
        _en.Add("Enter amount...", "Enter amount...");
        _en.Add("New Order:", "New Order:");
        _en.Add("Product:", "Product:");
        _en.Add("Amount:", "Amount:");
        _en.Add("Order total price:", "Order total price:");
        _en.Add("Add", "Add");
        
        //BuildingWindow GUI
        _en.Add("Product Description:", "Product Description:");
        _en.Add("Production report by years:", "Production report by years:");
        _en.Add("Import Orders", "Import Orders");
        _en.Add("Export Orders", "Export Orders");
        _en.Add("Orders in progress:", "Orders in progress:");

        //Jun 12, 2019
        _en.Add("ShowQuest.HoverSmall", "Current Quest");
        _en.Add("Have Fun", "Have Fun");
        _en.Add("Current Quest:", "Current Quest:");
        _en.Add("Reward: ", "Reward: ");



        //Dec 16
        _en.Add("New bought lands", "New Bought Lands");


        //Dec 18
        //Main Menu
        _en.Add("Command Keys", "Command Keys");
        _en.Add("Command Keys.Text", "[F1] Help\n[F9] Hide/Show GUI\n[P] Centers Camera to Town");

        _en.Add("Credits", "Credits");
        _en.Add("Credits.Text", "Translation:\nCédric Gauché (fr)\nKarsten Eidner (de)");


        //All Lang Needed for sure
		_en.Add("Attention.Production", "Attention: Production was stopped. To resume production in this Building select a product");


        //Dec 20 2019 for all Langs
        _en.Add("I.Can.Service", "\n\nI can service ");

        //Mar 20 2020
        _en.Add("Rotten", "Rotten");
        _en.Add("Ready", "Ready");



        //has a lot of text to put here 
        //ProductStat.cs (rev)
        //Dispatch.cs (rev)
        //ButtonTile.cs (rev)
        //Plant.cs (rev)
        //GameTime.cs (rev)
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

    internal static bool Contains(string key)
    {
        ReloadIfNeeded();

        if (_currentLang == "English")
        {
            return _en.ContainsKey(key);
        }
        else if (_currentLang == "Español")
        {
            return Spanish.ContainsKey(key);
        }
        else if (_currentLang == "Français")
        {
            return French.ContainsKey(key);
        }
        else if (_currentLang == "Deutsch")
        {
            return German.ContainsKey(key);
        }
        return false;
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
            if (Spanish.ContainsKey(key))
            {
                return Spanish.ReturnValueWithKey(key);
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
                return French.ReturnValueWithKey(key);
            }
            return "fr:" + key;
        }
        else if (_currentLang == "Deutsch")
        {
            if (German.ContainsKey(key))
            {
                return German.ReturnValueWithKey(key);
            }
            return "de:" + key;
        }
        else
        {
            //English as Default
            if (_en.ContainsKey(key))
            {
                return _en.ReturnValueWithKey(key);
            }
            return key;
        }
    }

    private static void ReloadIfNeeded()
    {
        if (_en.Count == 0)
        {
            ReloadDict();
            French.ReloadDict();
            German.ReloadDict();
            Spanish.ReloadDict();
        }
    }

    public static void ResetLanguages()
    {
        _en.Clear();
        _sp.Clear();
        _fr.Clear();
        _de.Clear();
        _portuguese.Clear();
        French.Clear();
        German.Clear();
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
        if (_currentLang == "Español(Beta)")
        {
            return _sp.Dictionary;
        }
        if (_currentLang == "Português(Beta)") return _portuguese;
        if (_currentLang == "Français(Beta)") return _fr.Dictionary;
        if (_currentLang == "Deutsch") return _de.Dictionary;

        return _en.Dictionary;
    }
}
