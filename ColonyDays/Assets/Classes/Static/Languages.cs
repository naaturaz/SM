using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Languages
{
    private static string _currentLang = "English";


    private static string _houseTail = ". SugarMiller's live here and enjoy having a nice meal at least once in a while";
    private static string _animalFarmTail = ", you can raise different animals in this building";
    private static string _fieldFarmTail = ", you can plant different crops and fruits in this building";
    private static string _asLongHasInput = ", as long as it has the necessary inputs";
    private static string _produce = "In this building workers will produce the selected product, as long as it has the necessary inputs";
    private static string _storage =
    "You can use this building as storage, however, if it gets full people won't work if they don't have room where to store their products";
    private static string _militar = "This building helps to decrease the Pirate Threat on your port, to be effective it must have workers. The more workers the better";

    private static string _notRegionNeeded = " Can be built without owning the region.";

    static Dictionary<string, string> _english = new Dictionary<string, string>()
    {
        //Descriptions
        //Infr
        { "Road.Desc","Used for decoration purposes. People are happier if there are roads around them"},
        { "BridgeTrail.Desc","Allows people to pass from one side of the terrain to the other"},
        { "BridgeRoad.Desc","Allows people to pass from one side of the terrain to the other. People love these bridges. It gives a sense of prosperity and happiness" +_houseTail},
        { "LightHouse.Desc","Helps to increase Port visibility. Adds to Port Reputation as long as the flame is glowing"},
        { H.Masonry + ".Desc","Important building, workers construct new buildings and work as wheelbarrows once they have nothing to do"},
        { H.StandLamp + ".Desc","Illuminates at night if Whale Oil is available on the town's storage."},

        { H.HeavyLoad + ".Desc","These workers use horse wagons to move goods around"},


       //House
	   { "Bohio.Desc", "Bohio house, primitive conditions with unhappy people whom can only have a maximum of 2 to 3 kids"},

       { "Shack.Desc", "Shack, primitive conditions with unhappy people whom can only have a maximum of 2 kids"},
       { "MediumShack.Desc", "The Medium Shack, has above primitive conditions with small happiness, can have a maximum of 2 to 3 kids"},
       { "LargeShack.Desc", "A Large Shack, has somewhat good conditions with happiness, can have a maximum of 2 to 4 kids"},


       { "WoodHouseA.Desc", "Medium wooden house, a family can have 2-3 kids max" },
       { "WoodHouseB.Desc", "Medium wooden house, a family can have 3-4 kids max"  },
       { "WoodHouseC.Desc", "Medium wooden house, a family can have 2-3 kids max"},
       { "BrickHouseA.Desc", "Medium house, a family can have 3 kids max"},
       { "BrickHouseB.Desc","Large house, a family can have 3-4 kids max"},
       { "BrickHouseC.Desc","Large house, a family can have 4 kids max"},

       
       //Farms
       //Animal
	   { "AnimalFarmSmall.Desc","Small animal farm"+_animalFarmTail},
       { "AnimalFarmMed.Desc","Medium animal farm"+_animalFarmTail},
       { "AnimalFarmLarge.Desc","Large animal farm"+_animalFarmTail},
       { "AnimalFarmXLarge.Desc","Extra large animal farm"+_animalFarmTail},
       //Fields
	   { "FieldFarmSmall.Desc","Small field farm"+_fieldFarmTail},
       { "FieldFarmMed.Desc","Medium field farm"+_fieldFarmTail},
       { "FieldFarmLarge.Desc","Large field farm"+_fieldFarmTail},
       { "FieldFarmXLarge.Desc","Extra large field farm"+_fieldFarmTail},
       { H.FishingHut + ".Desc","Here a worker can catch fish in a river (must be place by the river)." + _notRegionNeeded},

       //Raw
	   { "Mortar.Desc","Here a worker will produce Mortar"},
       { "Clay.Desc","Here a worker will produce Clay, raw material is needed for bricks and more"},
       { "Pottery.Desc","Here a worker will produce crockery products, such as dishes, jars, etc"},
       { "Mine.Desc","Here a worker can fish in a river"},
       { "MountainMine.Desc","Here a worker will work the mine by extracting ore"},
       { "Resin.Desc","Here a worker will work the mine by extracting minerals and metals randomly"},
       {  H.LumberMill +".Desc","Here workers will find resources such as wood, stone, and ore"},
       { "BlackSmith.Desc","Here workers will produce the product selected"+_asLongHasInput},
       { "ShoreMine.Desc","Here workers will produce salt and sand"},
       { "QuickLime.Desc","Here workers will produce quicklime"},

       //Prod
	   { "Brick.Desc","Here a worker will produce clay made products, such as bricks, etc"},
       { "Carpentry.Desc","Here a worker will produce wood made products, such as crates, barrels, etc"},
       { "Cigars.Desc","Here workers will produce cigars"+_asLongHasInput},
       { "Mill.Desc","Here workers will ground some grains"+_asLongHasInput},
       { H.Tailor+".Desc","Here workers will produce clothes"+_asLongHasInput},
       { "Tilery.Desc","Here workers will produce roof tiles"+_asLongHasInput},
       { "Armory.Desc","Here workers will produce weapons"+_asLongHasInput},
       { H.Distillery+".Desc",_produce},
       { "Chocolate.Desc",_produce},
       { "Ink.Desc",_produce},

       //Ind
	   { "Cloth.Desc",_produce},
       { "GunPowder.Desc",_produce},
       { "PaperMill.Desc",_produce},
       { "Printer.Desc",_produce},
       { "CoinStamp.Desc",_produce},
       { "Silk.Desc",_produce},
       { "SugarMill.Desc",_produce},
       { "Foundry.Desc",_produce},
       { "SteelFoundry.Desc",_produce},

       //trade
	   { "Dock.Desc","Here you can add import or export orders (must be placed by the ocean)." + _notRegionNeeded},
       { H.Shipyard + ".Desc","You can repairs your ships here, but you must have ship repair materials in inventory"},
       { "Supplier.Desc","You can supply ships with goods here, but you must have items in inventory that a ship can use for their long trip"},
       { "StorageSmall.Desc",_storage},
       { "StorageMed.Desc",_storage},
       { "StorageBig.Desc",_storage},
       { "StorageBigTwoDoors.Desc",_storage},
       { "StorageExtraBig.Desc",_storage},

       //gov
	   { "Library.Desc","People come to this building to read and borrow books for their knowledge. The more inventory in the libraries the better"},
       { "School.Desc","Here people will get an education. Here more is better"},
       { "TradesSchool.Desc","Here people will get specialized education on trades. The more the better"},
       { "TownHouse.Desc","The townhouse increases happiness and prosperity to your people"},

       //other
	   { "Church.Desc","The church gives happiness and hope to your people"},
       { "Tavern.Desc","The tavern gives some relaxation and entertainment to your people"},

       //Militar
	   { "WoodPost.Desc", "They spot bandits and pirates quicker so you can prepare in advance"},
       { "PostGuard.Desc",_militar},
       { "Fort.Desc",_militar},
       { "Morro.Desc",_militar+". Once you build this, Pirates should know better"},


       //Buildings name
       //Infr
        { "Road","Road"},
        { "BridgeTrail","Trail Bridge"},
        { "BridgeRoad","Road Bridge"},
        { "LightHouse","Light House"},
        { "Masonry","Masonry"},
        {   "StandLamp","Stand Lamp"},
        { "HeavyLoad","Heavy Load"},


       //House
       { "Shack", "Shack"},
       { "MediumShack", "Medium Shack"},
       { "LargeShack", "Large Shack"},

       { "WoodHouseA", "Medium Wood House" },
       { "WoodHouseB", "Large Wood House"  },
       { "WoodHouseC", "Luxury Wood House"},
       { "BrickHouseA", "Medium Brick House"},
       { "BrickHouseB","Luxury Brick House"},
       { "BrickHouseC","Large Brick House"},

       
       //Farms
       //Animal
	   { "AnimalFarmSmall","Small Animal Farm"},
       { "AnimalFarmMed","Medium Animal Farm"},
       { "AnimalFarmLarge","Large Animal Farm"},
       { "AnimalFarmXLarge","Extra Large Animal Farm"},
       //Fields
	   { "FieldFarmSmall","Small Field Farm"},
       { "FieldFarmMed","Medium Field Farm"},
       { "FieldFarmLarge","Large Field Farm"},
       { "FieldFarmXLarge","Extra Large Field Farm"},
       { "FishingHut","Fishing Hut"},

       //Raw
	   { "Mortar","Mortar"},
       { "Clay","Clay"},
       { "Pottery","Pottery"},
       { "MountainMine","Mountain Mine"},
       {  "LumberMill" ,"Lumber Mill"},
       { "BlackSmith","Black Smith"},
       { "ShoreMine","Shore Mine"},
       { "QuickLime","QuickLime"},

       //Prod
	   { "Brick","Brick"},
       { "Carpentry","Carpentry"},
       { "Cigars","Cigars"},
       { "Mill","Mill"},
       { "Tailor","Tailor"},
       { "Tilery","Tilery"},
       { "Armory","Armory"},
       { "Distillery","Distillery"},
       { "Chocolate","Chocolate"},
       { "Ink","Ink"},

       //Ind
	   { "Cloth","Cloth"},
       { "GunPowder","Gunpowder"},
       { "PaperMill","Papermill"},
       { "Printer","Printer"},
       { "CoinStamp","Coin Stamp"},
       { "SugarMill","SugarMill"},
       { "Foundry","Foundry"},
       { "SteelFoundry","Steel Foundry"},

       //trade
	   { "Dock","Dock"},
       { "Shipyard","Shipyard"},
       { "Supplier","Supplier"},
       { "StorageSmall","Small Storage"},
       { "StorageMed","Medium Storage"},
       { "StorageBig","Big Storage"},

       //gov
	   { "Library","Library"},
       { "School","School"},
       { "TradesSchool","Trades School"},
       { "TownHouse","Townhouse"},

       //other
	   { "Church","Church"},
       { "Tavern","Tavern"},

       //Militar
	   { "WoodPost", "Wood Guard Duty"},
       { "PostGuard","Stone Guard Duty"},
       { "Fort","Fort"},
       { "Morro","Morro"},



       //Main GUI
	   { "SaveGame.Dialog", "Save your game progress"},
       { "LoadGame.Dialog", "Load a game"},
       { "NameToSave", "Save your game as:"},
       { "NameToLoad", "Game to load selected:"},
       { "OverWrite", "There is already a saved game with the same name. Do you want to overwrite the file?"},
       { "DeleteDialog", "Are you sure you want to delete the saved game?"},
       { "NotHDDSpace", "Not enough space on {0} drive to save the game"},
       { "GameOverPirate", "Sorry, you lost the game! Pirates attacked your town and killed everyone."},
       { "GameOverMoney", "Sorry, you lost the game! The crown won't support your Caribbean island anymore."},
       { "BuyRegion.WithMoney", "Are you sure want to buy this region."},
       { "BuyRegion.WithOutMoney", "Sorry, you can't afford this now."},
       { "Feedback", "Feedback!? Awesome...:) Thanks. 8) "},
       { "OptionalFeedback", "Feedback is crucial. Pls drop a word"},
       { "MandatoryFeedback", "Only the dev team will see this. Your rate is?"},

       { "BugReport", "Caught a bug? uhmm yummy.... Send it this way!! Thanks"},
       { "Invitation", "Your friend's email for a chance to join the Private Beta"},
       { "Info", ""},//use for informational Dialogs
       { "Negative", "The Crown extended a line of credit for you. If you own more than 100 000 is game over"},  


       //MainMenu
        { "Types_Explain", "Traditional: \nIt's a game where in the beginning some buildings are locked and you have to unlock them. " +
            "The good thing is that this provides you with guidance." +
            "\n\nFreewill: \nAll available buildings are unlocked right away. " +
            "The bad thing is this way you can fail very easily." +
            "\n\nThe 'Hard' difficulty is the closest to reality"},


       //Tooltips 
       //Small Tooltips 
	   { "Person.HoverSmall", "People / Adults / Kids"},
       { "Emigrate.HoverSmall", "Emigrated"},
       { "Lazy.HoverSmall", "Unemployed people"},
       { "Food.HoverSmall", "Food"},
       { "Happy.HoverSmall", "Happiness"},
       { "PortReputation.HoverSmall", "Reputation of Port"},
       { "Dollars.HoverSmall", "Dollars"},
       { "PirateThreat.HoverSmall", "Pirate Threat"},
       { "Date.HoverSmall", "Date (m/y)"},
       { "MoreSpeed.HoverSmall", "More speed [PgUp]"},
       { "LessSpeed.HoverSmall", "Less speed [PgDwn]"},
       { "PauseSpeed.HoverSmall", "Pause game"},
       { "CurrSpeedBack.HoverSmall", "Current speed"},
       { "ShowNoti.HoverSmall", "Notifications"},
       { "Menu.HoverSmall", "Main Menu"},
       { "QuickSave.HoverSmall", "Quick Save [F]"},
       { "Bug Report.HoverSmall", "Report a bug"},
       { "Feedback.HoverSmall", "Feedback"},
       { "Hide.HoverSmall", "Hide"},
       { "CleanAll.HoverSmall", "Clean"},
       { "Bulletin.HoverSmall", "Control/Bulletin"},
       {"ShowAgainTuto.HoverSmall","Tutorial"},
       { "BuyRegion.HoverSmall", "Buy Regions"},

       { "More.HoverSmall", "More"},
       { "Less.HoverSmall", "Less"},
       { "Prev.HoverSmall", "Previous"},

       //down bar
	   { "Infrastructure.HoverSmall", "Infrastructure"},
       { "House.HoverSmall", "Housing"},
       { "Farming.HoverSmall", "Farming"},
       { "Raw.HoverSmall", "Raw"},
       { "Prod.HoverSmall", "Production"},
       { "Ind.HoverSmall", "Industry"},
       { "Trade.HoverSmall", "Trade"},
       { "Gov.HoverSmall", "Government"},
       { "Other.HoverSmall", "Other"},
       { "Militar.HoverSmall", "Military"},
       { "WhereIsTown.HoverSmall", "Back to town [P]"},
       { "WhereIsSea.HoverSmall", "Show/hide path to sea"},
       { "Helper.HoverSmall", "Help"},
       { "Tempeture.HoverSmall", "Temperature"},
	   
       //building window
       { "Gen_Btn.HoverSmall", "General Tab"},
       { "Inv_Btn.HoverSmall", "Inventory Tab"},
       { "Upg_Btn.HoverSmall", "Upgrades Tab"},
       { "Prd_Btn.HoverSmall", "Production Tab"},
       { "Sta_Btn.HoverSmall", "Stats Tab"},
       { "Ord_Btn.HoverSmall", "Orders Tab"},
       { "Stop_Production.HoverSmall", "Stop Production"},
       { "Demolish_Btn.HoverSmall", "Demolish"},
       { "More Salary.HoverSmall", "Pay more"},
       { "Less Salary.HoverSmall", "Pay less"},
       { "Next_Stage_Btn.HoverSmall", "Buy Next Stage"},
       { "Current_Salary.HoverSmall", "Current salary"},
       { "Current_Positions.HoverSmall", "Current positions"},
       { "Max_Positions.HoverSmall", "Max positions"},


       { "Add_Import_Btn.HoverSmall", "Add an import"},
       { "Add_Export_Btn.HoverSmall", "Add an export"},
       { "Upg_Cap_Btn.HoverSmall", "Upgrades capacity"},
       { "Close_Btn.HoverSmall", "Close"},
       { "ShowPath.HoverSmall", "Show path"},
       { "ShowLocation.HoverSmall", "Show location"},//TownTitle
       { "TownTitle.HoverSmall", "Town"},
       {"WarMode.HoverSmall", "Combat Mode"},
       {"BullDozer.HoverSmall", "Bulldozer"},
       {"Rate.HoverSmall", "Pls Rate Me"},

       //addOrder windiw
	   { "Amt_Tip.HoverSmall", "Amount of prod"},

       //Med Tooltips 
	   { "Build.HoverMed", "Place building: 'Left click' \n" +
                           "Rotate building: 'R' key \n " +
                           "Cancel: 'Right click'"},
        { "BullDozer.HoverMed", "Clean area: 'Left click' \n" +
          "Cancel: 'Right click' \nCost: $10 per use "},

        { "Road.HoverMed", "Start: 'Left click' \n" +
            "Expand: 'Move mouse' \n" +
            "Set: 'Left click again' \n" +
          "Cancel: 'Right click'"},

       { "Current_Salary.HoverMed", "Workers will go to work, where the highest salary is paid." +
                                    " If 2 places pay the same salary, then the closest to home will be chosen."},



       //Notifications
	   { "BabyBorn.Noti.Name", "New Born"},
       { "BabyBorn.Noti.Desc", "{0} was born"},
       { "PirateUp.Noti.Name", "Pirates Closer"},
       { "PirateUp.Noti.Desc", "Pirates close to shore"},
       { "PirateDown.Noti.Name", "Pirates Respect You"},
       { "PirateDown.Noti.Desc", "Pirates respect you a bit more today"},

       { "Emigrate.Noti.Name", "A citizen has emigrated"},
       { "Emigrate.Noti.Desc", "People emigrate when they are not happy with your government"},
       { "PortUp.Noti.Name", "Port is known"},
       { "PortUp.Noti.Desc", "Your port reputation is ramping up with neighboring ports and routes"},
       { "PortDown.Noti.Name", "Port is less known"},
       { "PortDown.Noti.Desc", "Your port reputation went down"},

       { "BoughtLand.Noti.Name", "New Land Purchased"},
       { "BoughtLand.Noti.Desc", "A new land region was purchased"},

       { "ShipPayed.Noti.Name", "Ship paid out"},
       { "ShipPayed.Noti.Desc", "A ship has paid out {0} for goods or service"},
       { "ShipArrived.Noti.Name", "A ship has arrived"},
       { "ShipArrived.Noti.Desc", "A new ship has arrived to one of our maritime buildings"},

       { "AgeMajor.Noti.Name", "New worker"},
       { "AgeMajor.Noti.Desc", "{0} is ready to work"},


       { "PersonDie.Noti.Name", "A person has died"},
       { "PersonDie.Noti.Desc", "{0} has died"},

       { "DieReplacementFound.Noti.Name", "A person has died"},
       { "DieReplacementFound.Noti.Desc", "{0} has died. A job replacement was found."},

       { "DieReplacementNotFound.Noti.Name", "A person has died"},
       { "DieReplacementNotFound.Noti.Desc", "{0} has died. No job replacement was found"},


       { "FullStore.Noti.Name", "A storage is getting full"},
       { "FullStore.Noti.Desc", "A storage is at {0}% capacity"},

       { "CantProduceBzFullStore.Noti.Name", "A worker cannot produce"},
       { "CantProduceBzFullStore.Noti.Desc", "{0} because his destination storage is full"},

       { "NoInput.Noti.Name", "At least an input is missing in building"},
       { "NoInput.Noti.Desc", "A building cannot produce {0} because is missing at least one input"},

       { "Built.Noti.Name", "A building has being built"},
       { "Built.Noti.Desc", "{0} has being fully built"},

       { "cannot produce", "cannot produce"},

       



       //Main notificaion
       //Shows on the middle of the screen
       { "NotScaledOnFloor", "The building is either too close to shore or mountain"},
       { "NotEven", "The ground underneath the building is not even"},
       { "Colliding", "Building is colliding with another one"},
       { "Colliding.BullDozer", "Bulldozer is colliding with a building. Can only be used on terrain (trees, rocks)"},

       { "BadWaterHeight", "The building is too low or high on the water"},
       { "LockedRegion", "You need to own this region to build here"},
       { "HomeLess", "People in this house have nowhere to go. Please build a new house that" +
                     " can hold this family and try again"},
       { "LastFood", "Cannot destroy, this is the only Storage in your village"},
       { "LastMasonry", "Cannot destroy, this is the only Masonry in your village"},
       { "OnlyOneDemolish", "You are demolishing a building already. Try again after demolition is completed"},


       //help

       { "Construction.HoverMed", "For the construction of any building you need to have workers in the Masonry. "+
            " Click the Masonry, then the '+' sign in the general tab. Make sure you have enough resources"},


       { "Construction.Help", "For the construction of any building you need to have workers in the Masonry. "+
            " Click the Masonry, then the '+' sign in the general tab. Make sure you have enough resources"},
       { "Camera.Help", "Camera: Use [WASD] or cursor to move. " +
                   "Press the scroll wheel on your mouse, keep it pressed to rotate, or [Q] and [E]"},
       { "Sea Path.Help", "Click on the left bottom corner 'Show/hide sea path' " +
                    "button to show the closest path to the sea"},

       { "People Range.Help", "The huge blue circle around each building marks the range of it"},

       { "Pirate Threat.Help", "Pirate Threat: This is how aware are the pirates of your port. This increases as" +
                                  " you have more money. If this reaches over 90 you will lose the game. You can counter the threat by constructing military buildings"},

       { "Port Reputation.Help", "Port Reputation: The more people know your port, the more they will visit." +
                                    " If you want to increase this make sure you always have some orders" +
                                    " in the Dock"},
       { "Emigrate.Help", "Emigrates: When people are unhappy for a few years they leave. The bad" +
                              " part of this is they won't come back, they won't produce or have children." +
                              " The only good thing is that they increase the 'Port Reputation'"},
       { "Food.Help", "Food: The higher the variety of food available in a household, the happier they" +
                          " will be."},

       { "Weight.Help", "Weight: All the weights in the game are in Kg or Lbs depending on which Unit system is selected." +
                        " You can change it in 'Options' in the 'Main Menu'"},
       { "What are Ft3 and M3?.Help", "The storage capacity is determined by the volume of the building. Ft3 is a cubic foot. M3 is a cubic meter" },//. Keep in mind that less dense products will fill up your storage quickly. To see products density Bulletin/Prod/Spec"},

       { "More.Help", "If you need more help might be a good idea completing the tutorial, or simply posting a question on SugarMill's Forums"},

        //more 
       { "Products Expiration.Help", "Products expiration: Just like in real life, in this game every product expires. Some food items expire sooner than others. You can see how many products had expired on Bulletin/Prod/Expire"},
       { "Horse Carriages.Help", "As the game has real measurements people can carry only so much. That's when horse-drawn carriages come into place. They carry a lot more, as a result, your economy gets boosted. A person in their best years might carry around 15KG, a wheelbarrow closer to 60KG, but the smaller cart can carry 240KG. To use them build a HeavyLoad"},
       { "Usage of goods.Help", "Usage of goods: Crates, barrels, wheelbarrows, carts, tools, cloth, crockery, furniture and utensils are all needed to do the traditional activities of a town. As these goods get used, they diminish, as a result, a person won't carry anything if there are no crates. Keep an eye on that ;)"},
       { "Happiness.Help", "Happiness: People's happiness is influenced by various factors. How much money they have, food variety, religion satisfaction, access to leisure, house comfort and education level. Also if a person has access to utensils, crockery and cloth will influence their happiness."},
       { "Line production.Help", "Line production: To make a simple nail you need to mine ore, in the foundry melt the iron, and finally in the blacksmith make the nail. If you got enough money, you can always buy the nail directly on a ship, or any other product."},
       { "Bulletin.Help", "The pages icon on the bottom bar is the Bulletin/Control Window. Please get a minute to explore it."},
       { "Trading.Help", "You will need to have at least one Dock to be able to trade. On it, you can add import/export orders and make some cash. If you need help adding an order you might want to complete the Tutorial"},

       { "Combat Mode.Help", "It activates when a Pirate/Bandit is detected by one of your citizens. Once the mode is active you can command units directly to attack. Select them and right click to objective to attack"},

       { "Population.Help", "Once they turn 16 will move to a free house if found. If there is always a free house to move to the population growth will be guaranteed."},



       { "F1.Help", "Press [F1] for help"},

       { "Inputs.Help", "If a building can't produce because is missing inputs. Check you have the needed input(s) in the main storage and at least one worker in the masonry"},
       { "WheelBarrows.Help", "Wheelbarrows are the masonry workers. If they got nothing to build will act as wheelbarrows. If you need inputs to get into a specific building make sure you have enough of them working and also the inputs mentioned in the storage building"},


       ///word and grammarly below




       //to  add on spanish         //to correct  
       { "TutoOver", "Your reward is $10,000 if is the first time you complete it. The tutorial is over now you can keep playing this game or start a new one."},

       //Tuto
       { "CamMov.Tuto", "Tutorial completion reward is $10,000 (one time reward per game). Step1: Use [WASD] or arrow keys to move the Camera. Do this for at least 5 seconds"},
       { "CamMov5x.Tuto", "Use [WASD] or arrow keys and keep press the 'Left Shift' key to move the Camera 5 times quicker. Do this for at least 5 seconds"},
       { "CamRot.Tuto", "Now press the scroll wheel down on your mouse and move your mouse to rotate the Camera. Do this for at least 5 seconds"},


       { "BackToTown.Tuto", "Press the key [P] on the keyboard to go to the initial position of the camera"},

       { "BuyRegion.Tuto", "Regions, you need to own a region to be able to build in it. Click on '+' sign on the bottom bar, then on the 'For Sale' sign in the" +
            " middle of a region to buy it. Some buildings are exempt, they can be built without owning the region" +
            " (FishingHut, Dock, MountainMine, ShoreMine, LightHouse, PostGuard)"
            },

       { "Trade.Tuto", "That was easy, the hard part is coming. Click on the 'Trade' buildings button, located in the right bottom bar. "+
           "When you hover over it, it will popup 'Trade'"},
       { "CamHeaven.Tuto", "Scroll back with your mouse middle button until the camera reaches"
            + " the sky. This view is useful to place bigger buildings such as the Port"},

       { "Dock.Tuto", "Now click on the 'Dock' building, it is the 1st button. When you hover over it, it will"+
           " show its cost and description"},
       { "Dock.Placed.Tuto", "Now the hard, read carefully. Notice that you can use the "+
           "'R' key to Rotate, and right click to cancel the building. This building has a part in the ocean and other in the land." +
           " The arrow goes to the sea, the storage section goes to land. Once the arrow is colored white, left click."},
       { "2XSpeed.Tuto", "Increase the game's speed, go to the middle top screen simulation speed controller, click the "
            +" 'More Speed' button 1 time until 2x is displayed"},

       { "ShowWorkersControl.Tuto", "Click on the 'Control/Bulletin' button, located in the bottom bar. "+
           "When you hover over it, it will popup 'Control/Bulletin'. "},
       { "AddWorkers.Tuto", "Click the '+' sign to the right of the Masonry building, this is how you add more workers."},
       { "HideBulletin.Tuto", "Keep in mind that in this window you are able to control and see different aspects of the game. Click outside the window to close it or 'OK' button."},
       { "FinishDock.Tuto", "Now finish the Dock building. The more workers are in the Masonry the quicker is going to get done too."
       + " Also make sure you have all materials needed to build it"},
       { "ShowHelp.Tuto", "Click on the 'Help' button, located in the bottom bar. "+
           "When you hover over it, it will popup 'Help'. There you can find some tips."},


       { "SelectDock.Tuto", "Ships drop and pick goods at random from the dock's inventory. Workers are needed to move dock goods in and out. They need wheelbarrows and crates. If are no items in the dock storage they won't work. Now click on the Dock."},


       { "OrderTab.Tuto", "Go to the Orders tab on the Dock's Window."},
       { "ImportOrder.Tuto", "Click on the '+' sign beside Add Import Order."},

       { "AddOrder.Tuto", "Now scroll down in the products and select wood and enter 100 as the amount. Then click the 'Add' button."},
       { "CloseDockWindow.Tuto", "Now the order is added. A random ship will drop this item in the dock inventory. And then your dock workers will take it to the closest Storage building. Now click out the window, so it closes."},
       { "Rename.Tuto", "Click on a person and then click on the title bar of the person. Like this, you can change the name of any person or building in the game. Click outside so the change is saved"},
       { "RenameBuild.Tuto", "Now click on a building and change its name in the same way. Remember to click outside so the change is saved"},

       { "BullDozer.Tuto", "Now click on the Bulldozer icon on the bottom bar. Then remove a tree or a rock from the terrain."},


       { "Budget.Tuto", "Click on the 'Control/Bulletin' button, then on 'Finance' menu and then on 'Ledger'. This is the game ledger"},
       { "Prod.Tuto", "Click on 'Prod' menu and then on 'Produce'. Will show the village's production for the last 5 years"},
       { "Spec.Tuto", "Click the 'Prod' menu and then on 'Spec'. Here you can see exactly how to make each product on the game. The inputs necessaries and where is produced. Also the price"},
       { "Exports.Tuto", "Click the 'Finance' menu and then on 'Export'. Here you can see a breakdown of your village's exports"},


        //Quest
       { "Tutorial.Quest", "Quest: Finish the tutorial. Reward $10,000. It takes roughly 3 minutes to complete"},

       { "Lamp.Quest", "Quest: Build a StandLamp. Find it on Infrastructure, it shines at night if there is Whale Oil on Storage"},

       { "Shack.Quest", "Quest: Build a Shack. These are cheap houses. When people turn 16 they will move to a free house if found. In this way, population growth will be guaranteed. [F1] Help"},

        { "SmallFarm.Quest", "Quest: Build a Small Field Farm. You need farms to feed your people"},
       { "FarmHire.Quest", "Quest: Hire two farmers in the Small Field Farm. Click on the farm and in the plus sign assign workers. You need to have unemployed"
            +" people to be able to assign them into a new building"},



        { "FarmProduce.Quest", "Quest: Now produce " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " of beans on the FieldFarmSmall. Click on the 'Stat' tab and will show you the production of the last 5 years. You can see the quest progress in the quest window. If you build more small farms will be accounted for the quest"},
        { "Transport.Quest", "Quest: Transport the beans from the farm to the Storage. To do that make sure you have" +
            " workers on the masonry. They act as wheelbarrows when not building"},

            { "Export.Quest", "Quest: Now export 300 " + Unit.CurrentWeightUnitsString() + " of beans. On the Dock add an order of export. If you have not dock then build one."+
           " Find it in Trade. Once built click on the orders tab and add an export order with the '+' sign."+
           " Select product and enter amount"},
       { "HireDocker.Quest", "Quest: Hire a docker. Dockers only task is to move the goods into the Dock from the Storage if you are exporting."+
           " Or vice-versa if importing. They work when there is an order in place and the goods are ready to transport. Otherwise, they stay at home resting." +
            " If you have more than one docker already fire all of them and then hire one"},
       { "MakeBucks.Quest", "Quest: Make $100 exporting goods in the Dock. "+
           "Once a ship arrives will randomly pay product(s) in your Dock's inventory"},
       { "HeavyLoad.Quest", "Quest: Build a HeavyLoad building. This are haulers that carry more weight. They will come handy when transporting goods around is needed." }, //Carts must be available on towns storages for them to work"},
       { "ImportOil.Quest", "Quest: Import 500 " + Unit.CurrentWeightUnitsString() + " of Whale Oil at the Dock. This is needed to keep your lights on at night."},

       { "Population50.Quest", "Reach a total population of 50 citizens"},
       {"CompleteQuest", "Your reward is ${0}"},


       //Quest Titles
       { "Tutorial.Quest.Title", "Tutorial"},
       { "Lamp.Quest.Title", "Stand Lamp"},

       { "Shack.Quest.Title", "Build a Shack"},
       { "SmallFarm.Quest.Title", "Build a Farm Field"},
       { "FarmHire.Quest.Title", "Hire a Farmer"},


       { "FarmProduce.Quest.Title", "Farm Producer"},

       { "Export.Quest.Title", "Exports"},
       { "HireDocker.Quest.Title", "Hire Docker"},
       { "MakeBucks.Quest.Title", "Make Money"},
       { "HeavyLoad.Quest.Title", "Heavy Load"},
       { "ImportOil.Quest.Title", "Whale Oil"},

       { "Population50.Quest.Title", "50 Citizens"},


        //
       {"Tutorial.Arrow", "This is the tutorial. Once finished you will win $10,000"},
       {"Quest.Arrow", "This is the quest button. You can access the quest window by clicking on it"},
       {"New.Quest.Avail", "At least one quest is available"},
       {"Quest_Button.HoverSmall", "Quest"},



       //Bulleting helps
       {"Help.Bulletin/Prod/Produce", "Here is shown what is being produced in the village."},
       {"Help.Bulletin/Prod/Expire", "Here is shown what has expired on the village."},
       {"Help.Bulletin/Prod/Consume", "Here is shown what is being consumed by your people."},

       {"Help.Bulletin/Prod/Spec", "In this window, you can see the inputs needed for each product, where is built and the price. "
       + "Scroll to the top to see the headers. Notice that one simple product may have more than a formula to produce."},

      {"Help.Bulletin/General/Buildings", "This is a resume of how many buildings are of each type."},

              {"Help.Bulletin/General/Workers", "In this window, you can assign workers to work in various buildings. "
       + "For a building allow more people into work, must be less than capacity and must find at least an unemployed person."},

       {"Help.Bulletin/Finance/Ledger", "Here is shown your ledger. Salary is the amount of money paid to a worker. The more people working the more salary will be paid out."},
       {"Help.Bulletin/Finance/Exports", "A breakdown of the exports"},

       { "Help.Bulletin/Finance/Prices", "...."},


       {"LoadWontFit", "This load won't fit in the storage area"},

        //and so on
       {"Missing.Input", "Building can't produce (Inputs must be in this building inventory). Missing inputs: \n" },





        //in game
        
        { "Buildings.Ready", "\n Buildings ready to be built:"},
        { "People.Living", "People living in this house:"},
        { "Occupied:", "Filled:"},
        { "|| Capacity:", "|| Capacity:"},
        { "Users:", "\nUsers:"},
        { "Amt.Cant.Be.0", "Amount can't be 0"},
        { "Prod.Not.Select", "Please select a product"},



    };



































    //ESPANNOL
    private static string _houseTailES = ". A los Azucareros les encanta comerse una buena comida de vez en cuando";
    private static string _animalFarmTailES = ", aqui se pueden criar diferentes animales";
    private static string _fieldFarmTailES = ", aqui se puede cultivar diferentes cultivos";
    private static string _asLongHasInputES = ", siempre y cuando tenga la materia prima necesaria";
    private static string _produceES = "Aqui los trabajadores produciran el producto selectionado, siempre y cuando exista la materia prima";
    private static string _storageES =
        "Aqui se almacenan todos los productos, si se llena los ciudadanos no tendran donde almacenar sus cosas";
    private static string _militarES = "Con esta construccion la Amenaza Pirata decrece, " +
                                       "para ser efectiva necesita trabajadores. Mientras mas, mejor";


    static Dictionary<string, string> _spanish = new Dictionary<string, string>()
    {
       //Descriptions
       //Infr
	   { "Road.Desc","Solo para propositos de decoracion. Las personas se sienten mas felices si la via esta pavimentada alrededor de ellos"},
       { "BridgeTrail.Desc","Por aqui las personas pasan de un lado del mapa a otro"},
       { "BridgeRoad.Desc","Por aqui las personas pasan de un lado del mapa a otro. Los ciudadanos adoran estos puentes. " +
                           "Les da un sentido de prosperidad y felicidad" +_houseTailES},
       { "LightHouse.Desc","Ayuda a que el puedo sea descubierto mas facil. Añade a la Reputacion del Puerto siempre y cuando la llama este encendida"},
       { H.Masonry + ".Desc","Una construccion muy imporatante. Estos trabajadores son los que construyen y cuando no tienen nada que hacer se dedican a transportar bienes"},
       { H.HeavyLoad + ".Desc","Una construccion muy imporatante. Estos trabajadores son los que construyen y cuando no tienen nada que hacer se dedican a transportar bienes"},
        { H.StandLamp + ".Desc","Alumbra por las noches si hay Aceite de Ballena en la almancen."},


       //House
	   { "Bohio.Desc","El Bohio, una casa con condiciones muy rudimentarias, los ciudadanos se abochornan de vivir aqui, una familia puede tener el maximo de 1 niño aqui" +_houseTail},

       { "Shack.Desc", "Casucha: Con condiciones de vida primitiva, las personas no son felicies viviendo aqui y pueden tener un maximo de 2 niños"},
       { "MediumShack.Desc", "Casucha mediana: Las condiciones son basicas, y las personas sienten muy poca felicidad viviendo aqui y pueden tener un maximo de 2-3 niños"},
       { "LargeShack.Desc", "Casucha grande: Las condiciones son un poco mejor que basicas, y las personas sienten algo de felicidad viviendo aqui y pueden tener un maximo de 2-4 niños"},

        { "HouseA.Desc","Casa pequeña de madera, una familia puede tener el maximo de 2 niños aqui" +_houseTailES},
       { "HouseB.Desc","Small house, una familia puede tener el maximo de 2 niños aqui" +_houseTailES },
       { "HouseTwoFloor.Desc","Wooden Medium house, una familia puede tener el maximo de 3 niños aqui"+_houseTailES},
       { "HouseMed.Desc","Medium house, una familia puede tener el maximo de 2 a 3 niños aqui"+_houseTailES},
       { "BrickHouseA.Desc","Casa de ladrillos:, una familia puede tener el maximo de 3 a 4 niños aqui"+_houseTailES},
       { "BrickHouseB.Desc","Casa de ladrillos:, una familia puede tener el maximo de 3 a 4 niños aqui"+_houseTailES},
       { "BrickHouseC.Desc","Casa de ladrillos:, una familia puede tener el maximo de 4 niños aqui"+_houseTailES},
       
       //Farms
       //Animal
	   { "AnimalFarmSmall.Desc","Finca de animales chica"+_animalFarmTailES},
       { "AnimalFarmMed.Desc","Finca de animales mediana"+_animalFarmTailES},
       { "AnimalFarmLarge.Desc","Finca de animales grande"+_animalFarmTailES},
       { "AnimalFarmXLarge.Desc","Finca de animales super grande"+_animalFarmTailES},
       //Fields
	   { "FieldFarmSmall.Desc","Finca de cultivos chica"+_fieldFarmTailES},
       { "FieldFarmMed.Desc","Finca de cultivos mediana"+_fieldFarmTailES},
       { "FieldFarmLarge.Desc","Finca de cultivos grande"+_fieldFarmTailES},
       { "FieldFarmXLarge.Desc","Finca de cultivos super grande"+_fieldFarmTailES},
       { H.FishingHut + ".Desc","Aqui se pescan peces"},

       //Raw
       { "Clay.Desc","Aqui se produce barro, necesaria para construir ladrillos y otros productos mas"},
       { "Pottery.Desc","Aqui se producen productos de ceramica como platos, jarras, etc"},
       { "Mine.Desc","Esta es una mina"},
       { "MountainMine.Desc","Esta es una mina"},
       { "Resin.Desc","La Resina de saca de los arboles aqui"},
       {  H.LumberMill +".Desc","Aqui los trabajadores buscan y extraen recursos naturales como madera, piedra y minerales"},
       { "BlackSmith.Desc","Aqui el trabajador produce elementos de la forja "+_asLongHasInputES},
       { "ShoreMine.Desc","Aqui se produce la sal, o arena"},
       { "QuickLime.Desc","Aqui los trabajadores producen cal"},
       { "Mortar.Desc","Aqui los trabajadores producen mezcla"},


       //Prod
	   { "Brick.Desc",_produceES},
       { "Carpentry.Desc",_produceES},
       { "Cigars.Desc",_produceES},
       { "Mill.Desc",_produceES},
       { H.Tailor+".Desc",_produceES},
       { "Tilery.Desc",_produceES},
       { "Armory.Desc",_produceES},
       { H.Distillery+".Desc",_produceES},
       { "Chocolate.Desc",_produceES},
       { "Ink.Desc",_produceES},

       //Ind
	   { "Cloth.Desc",_produceES},
       { "GunPowder.Desc",_produceES},
       { "PaperMill.Desc",_produceES},
       { "Printer.Desc",_produceES},
       { "CoinStamp.Desc",_produceES},
       { "Silk.Desc",_produceES},
       { "SugarMill.Desc",_produceES},
       { "Foundry.Desc",_produceES},
       { "SteelFoundry.Desc",_produceES},

       //trade
	   { "Dock.Desc","Aqui se pueden importar y exportar bienes"},
       { H.Shipyard + ".Desc","Aqui se reparan los barcos, para ser efectivo debe tener los materiales necesarios"},
       { "Supplier.Desc","Aqui se abastecen los barcos para sus largos viajes, siempre y cuando haiga bienes aqui"},
       { "StorageSmall.Desc",_storageES},
       { "StorageMed.Desc",_storageES},
       { "StorageBig.Desc",_storageES},
       { "StorageBigTwoDoors.Desc",_storageES},
       { "StorageExtraBig.Desc",_storageES},

       //gov
	   { "Library.Desc","Aqui la gente viene a nutrirse de conocimiento. Mientras mas libros haiga es mejor"},
       { "School.Desc","Aqui empieza la educacion de los Azucareros, mientras mas mejor"},
       { "TradesSchool.Desc","Aqui los Azucareros aprenden habilidades especiales, mientras mas mejor"},
       { "TownHouse.Desc","La casa de gobierno le da alegria y sentido de prosperidad a los ciudadanos"},

       //other
	   { "Church.Desc","La iglesia le da felicidad y esperanza a la gente"},
       { "Tavern.Desc","Aqui la gente viene a tomar y a divertirse"},

       //Militar
	   { "PostGuard.Desc",_militarES},
       { "Fort.Desc",_militarES},
       { "Morro.Desc",_militarES+". Una vez construida esta construccion los piratas te respetaran infinitamente"},
       { "WoodPost.Desc", "Ellos ven los pirates y bandidos primero de esta manera te puedes preparar mejor y con mas tiempo"},

       //Construcciones

       //infr
	   { "Masonry","Casa de Albañiles"},

       //Casas
	   { "Shack","Casucha"},

       //Trade
	   { "StorageBig","AlmacenGrande"},




       //Main GUI
	   { "SaveGame.Dialog", "Salva tu partida"},
       { "LoadGame.Dialog", "Carga una partida"},
       { "NameToSave", "Salva tu partida como:"},
       { "NameToLoad", "La partida selecciona es:"},
       { "OverWrite", "Ya existe un archivo con este nombre. Quieres sobre escribirlo?"},
       { "DeleteDialog", "Estas seguro que quieres borrar esta partida?"},
       { "NotHDDSpace", "Not hay espacio suficiente en torre {0} para salvar la partida"},
       { "GameOverPirate", "Lo siento, perdiste el juego! Los piratas te atacaron y mataron a todos."},
       { "GameOverMoney", "Lo siento, perdiste el juego! La corona no te ayudara mas con tu sueño Caribeño."},
       { "BuyRegion.WithMoney", "Estas seguro que quieres comprar esta region."},
       { "BuyRegion.WithOutMoney", "No tienes dinero para comprar esto ahora."},
       { "Feedback", "Sugerencias si...:) Gracias. 8) "},
       { "BugReport", "Bug, mandalo, gracias"},
       { "Invitation", "Pon el email de un amigo, quizas sea invitado a la Beta"},
       { "Info", ""},//use for informational Dialogs


       //more Main GUI
       { "Workers distribution", "Distribucion de los trabajadores"},
       { "Buildings", "Construcciones"},

       { "Age", "Edad"},
       { "Gender", "Genero"},
       { "Height", "Altura"},
       { "Weight", "Peso"},
       { "Calories", "Calorias"},
       { "Nutrition", "Nutricion"},
       { "Profession", "Profesion"},
       { "Spouse", "Conyugue"},
       { "Happinness", "Felicidad"},
       { "Years Of School", "Años de escuela"},
       { "Age majority reach", "Mayor de edad"},
       { "Home", "Hogar"},
       { "Work", "Trabajo"},
       { "Food Source", "Almacen"},
       { "Religion", "Religion"},
       { "Chill", "Relajamiento"},
       { "Thirst", "Sed"},
       { "Account", "Cuenta"},

       { "Early Access Build", "Acceso Anticipado"},






       //MainMenu

        { "Types_Explain", "Tradicional: \nEn este juego algunas construcciones estan  " +
                           "bloqueadas al principio y tienes que desbloquearlas. " +
            "Lo bueno es que asi tienes alguna manera de guiarte." +
            "\n\nFreewill: \nTodas las construcciones estan disponibles. " +
            "Lo malo es que puedes perder el juego mas facilmente."},


       //Tooltips 
       //Small Tooltips 
	   { "Person.HoverSmall", "Pers./Adul./Niñ."},
       { "Emigrate.HoverSmall", "Emigrados"},
       { "Lazy.HoverSmall", "Desempleados"},
       { "Food.HoverSmall", "Comida"},
       { "Happy.HoverSmall", "Felicidad"},
       { "PortReputation.HoverSmall", "Reputacion Portuaria"},
       { "Dollars.HoverSmall", "Dinero"},
       { "PirateThreat.HoverSmall", "Amenaza Pirata"},
       { "Date.HoverSmall", "Fecha (m/a)"},
       { "MoreSpeed.HoverSmall", "Mas velocidad"},
       { "LessSpeed.HoverSmall", "Menos velocidad"},
       { "PauseSpeed.HoverSmall", "Pausa"},
       { "CurrSpeedBack.HoverSmall", "Velocidad actual"},
       { "ShowNoti.HoverSmall", "Notificaciones"},
       { "Menu.HoverSmall", "Menu Principal"},
       { "QuickSave.HoverSmall", "Salva rapida [F]"},
       { "Bug Report.HoverSmall", "Reporte un bug"},
       { "Feedback.HoverSmall", "Sugerencias"},
       { "Hide.HoverSmall", "Esconder"},
       { "CleanAll.HoverSmall", "Limpiar"},
       { "Bulletin.HoverSmall", "Control/Boletin"},
       {"ShowAgainTuto.HoverSmall","Tutorial"},
       { "Quest_Button.HoverSmall", "Desafios"},
       { "TownTile.HoverSmall", "Nombre del pueblo"},

       { "More.HoverSmall", "Mas"},
       { "Less.HoverSmall", "Menos"},

       { "BuyRegion.HoverSmall", "Compra region"},
       { "BullDozer.HoverSmall", "Bulldozer"},


       //down bar
	   { "Infrastructure.HoverSmall", "Infraestructuras"},
       { "House.HoverSmall", "Casas"},
       { "Farming.HoverSmall", "Fincas"},
       { "Raw.HoverSmall", "Basico"},
       { "Prod.HoverSmall", "Produccion"},
       { "Ind.HoverSmall", "Industrias"},
       { "Trade.HoverSmall", "Comercio"},
       { "Gov.HoverSmall", "Govierno"},
       { "Other.HoverSmall", "Otros"},
       { "Militar.HoverSmall", "Militar"},
       { "WhereIsTown.HoverSmall", "Centrar el pueblo [P]"},
       { "WhereIsSea.HoverSmall", "Muestre/Esconda al mar"},
       { "Helper.HoverSmall", "Mini ayuda"},
       { "Tempeture.HoverSmall", "Temperatura"},
	   
       //building window
       { "Gen_Btn.HoverSmall", "General"},
       { "Inv_Btn.HoverSmall", "Inventario"},
       { "Upg_Btn.HoverSmall", "Mejoras"},
       { "Prd_Btn.HoverSmall", "Produccion"},
       { "Sta_Btn.HoverSmall", "Estadisticas"},
       { "Ord_Btn.HoverSmall", "Ordenes"},
       { "Stop_Production.HoverSmall", "Parar produccion"},
       { "Demolish_Btn.HoverSmall", "Demoler"},
       { "More Salary.HoverSmall", "Pagar mas"},
       { "Less Salary.HoverSmall", "Pagar menos"},
       { "Next_Stage_Btn.HoverSmall", "Compre"},
       { "Current_Salary.HoverSmall", "Salario actual"},
       { "Current_Positions.HoverSmall", "Posiciones actuales"},
       { "Add_Import_Btn.HoverSmall", "Añade una importacion"},
       { "Add_Export_Btn.HoverSmall", "Añade una exportacion"},
       { "Upg_Cap_Btn.HoverSmall", "Mejora la capacidad"},
       { "Close_Btn.HoverSmall", "Cerrar"},
       { "ShowPath.HoverSmall", "Enseñar camino"},
       { "ShowLocation.HoverSmall", "Enseñar lugar"},
       { "Max_Positions.HoverSmall", "Max de trabajadores"},
       {"Rate.HoverSmall", "Porfa Evaluame"},

       //addOrder windiw
	   { "Amt_Tip.HoverSmall", "Cantidad de productos"},

       //Med Tooltips 
	   { "Build.HoverMed", "Fijar construccion: 'Click izquierdo' \n" +
                           "Rotar construccion: tecla 'R' \n " +
                           "Cancelar: 'Click derecho'"},
       { "Current_Salary.HoverMed", "Los trabajadores prefieren trabajar donde se pague mas dinero." +
                                    " Si dos lugares pagan igual entonces escogeran el que este mas cerca a" +
                                    " casa."},



       //Notifications
	   { "BabyBorn.Noti.Name", "Recien nacido"},
       { "BabyBorn.Noti.Desc", "Un niño a nacido que alegria"},
       { "PirateUp.Noti.Name", "Los pirates se acercan"},
       { "PirateUp.Noti.Desc", "Un barco de bandera pirata se ha visto cerca de la costa"},
       { "PirateDown.Noti.Name", "Los piratas te temen"},
       { "PirateDown.Noti.Desc", "Hoy los pirates te respetan un poco mas"},

       { "Emigrate.Noti.Name", "Una persona a emigrado"},
       { "Emigrate.Noti.Desc", "Las personas emigran cuando no estan felices"},
       { "PortUp.Noti.Name", "El puerto de conoce"},
       { "PortUp.Noti.Desc", "Tu puerto esta recibiendo mas atencion por los comerciantes"},
       { "PortDown.Noti.Name", "Tu puerto es desconocido"},
       { "PortDown.Noti.Desc", "Tu puerto se conoce cada vez menos entre los comerciantes"},

       { "BoughtLand.Noti.Name", "Nueva tierra"},
       { "BoughtLand.Noti.Desc", "Nueva tierra ha sido comprada"},

       { "ShipPayed.Noti.Name", "Pago de comercio"},
       { "ShipPayed.Noti.Desc", "Un barco a pagado por los bienes adquiridos en tu puerto"},
       { "ShipArrived.Noti.Name", "Barco ha llegado"},
       { "ShipArrived.Noti.Desc", "Un barco ha llegado a una de nuestras construcciones maritimas"},

       { "AgeMajor.Noti.Name", "Un Trabajador Nuevo"},
       { "AgeMajor.Noti.Desc", "{0} esta listo(a) para trabajar"},


       { "PersonDie.Noti.Name", "Una persona ha muerto"},
       { "PersonDie.Noti.Desc", "{0} ha muerto"},

       { "DieReplacementFound.Noti.Name", "Una persona ha muerto"},
       { "DieReplacementFound.Noti.Desc", "{0} ha muerto. Ya fue reemplazado en su trabajo"},

       { "DieReplacementNotFound.Noti.Name", "Una persona ha muerto"},
       { "DieReplacementNotFound.Noti.Desc", "{0} ha muerto. No se encontro reemplazo en su trabajo"},


       { "FullStore.Noti.Name", "Una almacen se esta llenando"},
       { "FullStore.Noti.Desc", "Una almacen esta al {0}% de su capacidad"},

       { "CantProduceBzFullStore.Noti.Name", "El trabajador no puede producir"},
       { "CantProduceBzFullStore.Noti.Desc", "{0} El trabajador no puede producir porque su almacen esta llena"},

       { "NoInput.Noti.Name", "Al menos un insumo falta en el edificio"},
       { "NoInput.Noti.Desc", "Un edificio no produce {0} porque le falta aunque sea un insumo"},

       { "Built.Noti.Name", "Una construccion ha sido terminada"},
       { "Built.Noti.Desc", "{0} a sido construido(a)"},

       { "cannot produce", "No puede producir"},






       //Main notificaion
       //Shows on the middle of the screen
       { "NotScaledOnFloor", "La construccion esta muy cerca al mar o una montaña"},
       { "NotEven", "El piso no esta parejo en la base de la construccion"},
       { "Colliding", "La construccion choca con otra"},
       { "BadWaterHeight", "La construccion esta muy alta o muy baja en el agua"},
       { "LockedRegion", "Necesitas ser dueño de esta tierra para construir aqui"},
       { "HomeLess", "La gente en esta casa no tiene a donde ir. Por favor construye una" +
                     " nueva casa que pueda albegar a esta familia"},
       { "LastFood", "No puedes destruir la unica almacen en la villa"},
       { "LastMasonry", "No puedes destruir la unica casa de albañiles en la villa"},


       //Mini help
       { "Camera", "Camara: Use [AWSD] or el cursor para mover. " +
                   "Presione el boton del medio del raton para rotar camara, o [Q] y [E]"},
       { "SeaPath", "Presione en el boton 'Mostrar al mar' " +
                    "y el camino mas cercano al mar sera mostrado"},
       { "Region", "Region: Necesitas ser dueño de una region para construir en ella. " +
                   "Presione en la señal 'Se vende' para comprar una"},
       { "PeopleRange", "Rango: El circulo azul gigante es el rango de cada construccion"},

       { "PirateThreat.Help", "Amenaza Pirata: Esto es cuan al dia estan los piratas con tu puerto. " +
                              "Se incrementa a medida que acumules mas dinero y riquezas. " +
                              "Si pasa 90 entonces pierdes el juego."},

       { "PortReputation.Help", "Reputacion Portuaria: Mientras mas comerciantes y marineros conozcan tu puerto mas lo visitaran." +
                                    "Si quieres aumentar esto asegurate de que siempre haiga ordenes en tus construcciones maritimas" +
                                " (Puerto, Astillero, Abastecedor)"},
       { "Emigrate.Help", "Emigrados: Cuando la gente esta infeliz por algunos años se van de tus tierras. " +
                          "Lo malo es que no viraran, produciran bienes o tedran niños jamas." +
                              "Lo bueno es que aumentan 'La Reputacion Portuaria'"},
       { "Food.Help", "Comida: Mientras mas variedad de comidas las personas tengan mas felices seran."},

       { "Weight.Help", "Peso: Todos los pesos en el juego estan en Kg o Lbs" +
                        " dependiendo en el sistema de unidad seleccionado." +
                        " Se puede cambiar en 'Opciones' en el 'Menu Principal'"},



       { "More.Help", "Si necesita mas ayuda siempre es una buena idea pasar el tutorial, or o postear una pregunta en el Forum"},

        //more 
       { "Products Expiration.Help", "Caducidad de productos: Como en la vida real los productos expiran. En la tabla the productos expirados se puede ver si alguno ha expirado Bulletin/Prod/Expire"},
       { "Horse Carriages.Help", "Las personas con carretillas tiene limites de carga. Por eso estas carretas con caballos son usadas en el juego, ya que pueden cargar mucho mas. Como resultado la economia se mueve mas de prisa. Una persona carga alrededor de 15KG, un carretillero 60KG, y las carretas chicas hasta 240KG. Construye un HeavyLoad para usarlas"},
       { "Usage of goods.Help", "Consumo de bienes: Cajas, barriles, carretillas, carretas, herramientas, ropa, ceramicas, muebles y utensillos son todos necesarios para mantener las actividades de la villa. A medida que estos bienes son usados disminuye la cantidad en el almacen, por ej. una persona no cargara nada si no hay cajas"},
       { "Happiness.Help", "Felicidad: La felicidad de las personas esta influenciada por varios factores. Variedad de comidas, satisfaccion religiosa, esparcimiento, confort de la casa, nivel de educacion, utensillos, ceramica y ropa."},
       { "Line production.Help", "Linea de produccion: Para hacer un KG de puntillas tienes que encontrar y minar los minerales, en la fundicion derretir el hierro, y finalmente en el herrero hacer las puntillas. O simplemente comprarla en el puerto"},
       { "Bulletin.Help", "El icono con las paginas en la barra infierior es la ventana de Bulletin/Control. Por favor toma un minuto para explorarla."},
         { "Trading.Help", "Necesitas al menos un puerto para comerciar. En el puerto puedes agregar ordener de importacion y exportacion. Si necesitas mas ayuda puedes pasar el tutorial."},

       { "Combat Mode.Help", "Se activa cuando un pirata o bandido es visto por uno de tus ciudadanos."},

       { "Population.Help", "Cuando los jovenes cumplen 16 años se mudan a una casa vacia si existe. Si siempre hay casas vacias el crecimiento de la poblacion esta garantizado."},

       { "F1.Help", "Presiona [F1] para ayuda"},

       { "Inputs.Help", "Si un edificio no produce porque le faltan insumos, chequa que los insumos necesarios esten en la almacen y que tengas trabajadores en la Casa De Albañiles"},




        { "WheelBarrows.Help", "Los carretilleros son los trabajadores de la Casa de Albañiles. Si ellos no tienen nada que hacer entonces haran el trabajo de carretilleros. Si necesitas algun insumo en un edificio, asegurate de tener bastantes de estos trabajando y por su puesto los insumos disponibles en la almacen"},









       { "TutoOver", "Tu premio sera de $10,000 si es la 1era vez que completas el tutorial. Este es el fin del tutorial ahora puedes seguir jugando."},

       //Tuto
       { "CamMov.Tuto", "El premio por completar el tutorial son $10,000 (solo un premio por juego). Paso 1: Usa [WASD] o las teclas del cursos para mover la camara. Haz esto por al menos 5 segundos"},
       { "CamMov5x.Tuto", "Usa [WASD] o las teclas del cursos + 'Shift Izq' para mover la camara 5 veces mas rapido. Haz esto por al menos 5 segundos"},
       { "CamRot.Tuto", "Presiona la rueda del raton y despues mueve el raton para girar la camara. Haz esto por al menos 5 segundos"},

       { "BackToTown.Tuto", "Presiona la tecla [P] para ir al centro del pueblo"},

       { "BuyRegion.Tuto", "Necesitas ser dueño de una region para poder construir en ella. Haz click en el signo de '+' en la barra inferior, despues en la señal de 'For Sale' " +
            " en una region para comprarla. Estas construcciones pueden ser construidas sin ser dueño de la region:" +
            " (FishingHut, Dock, MountainMine, ShoreMine, LightHouse, PostGuard)"
            },

       { "Trade.Tuto", "Eso fue facil ahora viene lo dificil. Haz click en 'Comercio', en la barra inferior. "+
           "Cuando pases el cursor del raton se vera que dice 'Comercio'"},
       { "CamHeaven.Tuto", "Gira la rueda del raton hacia detras hasta que alcanzes el limite en el cielo. Esta vista es usada para emplazar grandes construcciones como el 'Puerto'"},

       { "Dock.Tuto", "Haz click en la construccion 'Puerto'. Cuando pases el cursor del raton por encima del icono saldra su costo y descripcion"},
       { "Dock.Placed.Tuto", "Ahora viene lo mas dificil. Puedes usar la tecla 'R' para rotar la construccion y click derecho para cancelar. "+
           " Esta construccion tiene una parte que va en tierra y otra en agua." +
           " La flecha debe ir en el agua, la seccion del almacenaje en tierra. Cuando la flecha se ponga blanca haz click izq para emplazar el edificio."},

        { "2XSpeed.Tuto", "Aumenta la velocidad del juego, en la parte superior de la pantalla en el centro, haz click en "
            +" 'Mas' hasta que aparezca el '2x'"},

       { "ShowWorkersControl.Tuto", "Haz click en boton de 'Control/Boletin', en la parte inferior de la pantalla. "+
           "Si le pasas el cursor del raton por encima se vera 'Control/Boletin'"},
       { "AddWorkers.Tuto", "Haz click en el signo de '+', Asi es como se añaden mas trabajadores."},


        { "HideBulletin.Tuto", "En este formulario puedes controlar y ver varios aspectos de la partida. Haz click fuera del formulario o 'OK' para cerrarlo"},
       { "FinishDock.Tuto", "Ahora termina el Puerto. Mientras mas trabajadores haiga en la Casa de Albañiles mas rapido se terminara."
       + " Tambien asegurate que tienes todos los materiales necesarios para construirlo."},
       { "ShowHelp.Tuto", "Haz click en el boton de ayuda, en la barra inferior. "+
           " Aqui puedes encontrar la ayuda del juego."},

       { "SelectDock.Tuto", "Los barcos escogen bienes al azar del inventario del puerto. Necesitas trabajadores para que muevan los bienes hacia y desde el puerto para las almacenes. Estos trabajadores utilizan cajas y carretllis para mover los bienes. Ahora haz click en el Puerto."},
       { "OrderTab.Tuto", "Haz click en las Ordenes en el formulario del Puerto"},
       { "ImportOrder.Tuto", "Haz click en el signo de '+' al lado de 'Orden de Importacion'"},



       { "AddOrder.Tuto", "Ahora navega hasta el final de la lista y escoge 'Madera', pon la cantidad de 100. Despues haz click en el botton de 'Añadir'"},
       { "CloseDockWindow.Tuto", "Ya la orden fue añadida. Un barco depositara estos bienes en el puerto. Despues tus trabajadores portuarios lo llevaran para la almacen mas cercana. Ahora cierra este formulario."},
       { "Rename.Tuto", "Haz click en una persona y despues click en la barra de titulo del formulario. De esta manera le puedes cambiar el nombre a cualquier persona o edificio. Haz click afuera del titulo y los cambios seran guardados"},
       { "RenameBuild.Tuto", "Now click on a building and change its name in the same way. Remember to click outside so the change is saved"},

       { "BullDozer.Tuto", "Now click on the Bulldozer icon on the bottom bar. Then remove a tree or a rock from the terrain."},


       { "Budget.Tuto", "Haz click en el boton 'Control/Boletin', despues en 'Finanzas' y despues en 'Presupuesto'"},
       { "Prod.Tuto", "Haz click en el menu 'Prod' y despues en 'Producido'. Muestra lo producido en los ultimos 5 años"},
       { "Spec.Tuto", "Haz click en el menu 'Prod' despues en 'Spec'. Aqui se ve exactamente como hacer todos los bienes en el juego. Los isumos necesarios, donde es producido y ademas el precio"},
       { "Exports.Tuto", "Haz click en el menu 'Finanzas' y despues en 'Exportaciones'. Aqui se ve un sumario de las exportaciones"},

        
        //Quest
       { "Tutorial.Quest", "Desafio: Termina el tutorial. $10,000 en premio. Toma alrededor de 3 minutos para ser completado"},

       { "Lamp.Quest", "Desafio: Construye una farola. Esta en Infraestructuras, son encedidas de noche si hay Aceite de Ballena en la Almacen"},

       { "Shack.Quest", "Desafio: Construye una casucha. Estas son casas baratas. Cuando las personas cumplen 16 años se mudan a un casa nueva si existe. De esta manera se garantiza el crecimiento de la poblacion. [F1] Ayuda"},
       { "SmallFarm.Quest", "Desafio: Construye una Finca de Cultivos Chica. Necesitas estas para alimentar a tu pueblo"},
       { "FarmHire.Quest", "Desafio: Contrata a dos granjeros en la Finca de Cultivos Chica. Haz click en la finca y despues en el signo de mas para asignar trabajadores. Para esto necesitas tener trabajadores desempleados"},


        { "FarmProduce.Quest", "Desafio: Produce " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " de Frijol en la Finca de Cultivos Chica. Haz click en la pestaña 'Stat' y te mostrara la produccion de los ultimos 5 años. Puedes ver el avance en el desafio en el formulario de desafios. Si construyes mas Fincas de Cultivos Chica ayudaran a pasar este desafio"},
        { "Transport.Quest", "Desafio: Transporta el Frijol de la Finca hacia la Almacen. Para hacer esto asegurate de que hay trabajadores en la Casa de Albañiles. Ellos se convierten en carretilleros cuando no trabajan"},

            { "Export.Quest", "Desafio: Exporta 300 " + Unit.CurrentWeightUnitsString() + " de Frijol. Añade una orden de Exportacion en el Puerto. Si no tienes un Puerto entonces construye uno."+
           "El icono del Puerto esta en Comercio. Cuando este hecho haz click en la pestaña de ordenes, añade una orden de exportacion, y selecciona el producto y la cantidad a exportar."},

       { "HireDocker.Quest", "Desafio: Contrata un portuario. La unica tarea de ellos es mover bienes desde el Almacen hacia el Puerto si estas exportando."+
           " O vice-versa si estas importando. Ellos trabajan cuando hay ordenes en el puerto y los bienes estan listos para su transporte. Sino se quedan en casa descanzando." +
            " Si ya tienes trabajadores aqui despidelos a todos y despues contrata a uno de nuevo."},
       { "MakeBucks.Quest", "Desafio: Haz $100 exportando bienes en el Puerto. "+
           "Cuando un barco llegue pagara bienes al azar que haiga en las bodegas de tu Puerto"},
       { "HeavyLoad.Quest", "Desafio: Construye el edificio de Carga Pesada. Estos son transportistas que cargan mas peso. Seran muy utiles cuando mucha carga necesita ser transportada en tu villa"},
       { "ImportOil.Quest", "Desafio: Importa 500 " + Unit.CurrentWeightUnitsString() + " de Aceite de Ballena en el Puerto. Es necesario para encender las Farolas por las noches."},

       { "Population50.Quest", "Obten 50 personas en total"},
       {"CompleteQuest", "Tu premio es de ${0}"},


              //Quest Titles
       { "Tutorial.Quest.Title", "Tutorial"},
       { "Lamp.Quest.Title", "Stand Lamp"},

       { "Shack.Quest.Title", "Build a Shack"},
       { "SmallFarm.Quest.Title", "Build a Farm Field"},
       { "FarmHire.Quest.Title", "Hire a Farmer"},


       { "FarmProduce.Quest.Title", "Farm Producer"},

       { "Export.Quest.Title", "Exports"},
       { "HireDocker.Quest.Title", "Hire Docker"},
       { "MakeBucks.Quest.Title", "Make Money"},
       { "HeavyLoad.Quest.Title", "Heavy Load"},
       { "ImportOil.Quest.Title", "Whale Oil"},

       { "Population50.Quest.Title", "50 Citizens"},





       //words
       //Field Farms
	   { "Bean", "Frijol"},
       { "Potato", "Papa"},
       { "SugarCane", "Caña"},
       { "Corn", "Maiz"},
       { "Cotton", "Algodon"},
       { "Banana", "Platano"},
       { "Coconut", "Coco"},
       //Animal Farm
	   { "Chicken", "Pollo"},
       { "Egg", "Huevo"},
       { "Pork", "Cerdo"},
       { "Beef", "Res"},
       { "Leather", "Cuero"},
       { "Fish", "Pescado"},
       //mines
	   { "Gold", "Oro"},
       { "Stone", "Piedra"},
       { "Iron", "Hierro"},

       { "Clay", "Arcilla"},
       { "Ceramic", "Ceramica"},
       { "Wood", "Madera"},

       //Prod
	   { "Tool", "Herramienta"},
       { "Brick", "Ladrillo"},
       { "Tonel", "Tonel"},
       { "Cigar", "Tabaco"},
       { "Tile", "Loza"},
       { "Fabric", "Tejido"},
       { "GunPowder", "Polvora"},
       { "Paper", "Papel"},
       { "Map", "Mapa"},
       { "Book", "Libro"},
       { "Sugar", "Azucar"},
       { "None", "Ninguno"},
       //
	   { "Person", "Persona"},
       { "Food", "Comida"},
       { "Dollar", "Dollar"},
       { "Salt", "Sal"},
       { "Coal", "Carbon"},
       { "Sulfur", "Sulfuro"},
       { "Potassium", "Potasio"},
       { "Silver", "Plata"},
       { "Henequen", "Henequen"},
	   //
       { "Sail", "Vela"},
       { "String", "Cuerda"},
       { "Nail", "Puntilla"},
       { "CannonBall", "Bola de cañon"},
       { "TobaccoLeaf", "Hoja de tabaco"},
       { "CoffeeBean", "Grano de cafe"},
       { "Cacao", "Cocoa"},
       { "Chocolate", "Chocolate"},
       { "Weapon", "Arma"},
       { "WheelBarrow", "Carretilla"},
       //
       { "Diamond", "Diamante"},
       { "Jewel", "Joya"},
       { "Cloth", "Ropa"},
       { "Rum", "Ron"},
       { "Wine", "Vino"},
       { "Ore", "Mineral"},
       { "Crate", "Caja"},
       { "Coin", "Moneda"},
       { "CannonPart", "Pieza de cañon"},
       { "Ink", "Tinta"},
       { "Steel", "Acero"},
       //
       { "CornFlower", "Harina de castilla"},
       { "Bread", "Pan"},
       { "Carrot", "Zanahoria"},
       { "Tomato", "Tomate"},
       { "Cucumber", "Pepino"},
       { "Cabbage", "Col"},
       { "Lettuce", "Lechuga"},
       { "SweetPotato", "Boniato"},
       { "Yucca", "Yuca"},
       { "Pineapple", "Piña"},
       //
       { "Papaya", "Fruta bomba"},
       { "Wool", "Lana"},
       { "Shoe", "Zapato"},
       { "CigarBox", "Caja de tabaco"},
       { "Water", "Agua"},
       { "Beer", "Cerveza"},
       { "Honey", "Miel"},
       { "Bucket", "Cubo"},
       { "Cart", "Carreta"},
       { "RoofTile", "Teja"},
       { "FloorTile", "Azulejo"},
       { "Mortar", "Mezcla"},
       { "Furniture", "Muebless"},

       { "Utensil", "Utensillo"},
       { "Stop", "Pare"},





       
       //Main Menu
       { "Resume Game", "Sigue el Juego"},
       { "Continue Game", "Continuar el Juego"},
       { "New Game", "Juego Nuevo"},
       { "Load Game", "Cargar Juego"},
       { "Save Game", "Salvar Juego"},
       { "Options", "Opciones"},
       { "Credits", "Creditos"},
       { "Exit", "Salir del Juego"},
       { "Achievements", "Logros"},
       { "Town Name:", "Nombre del Pueblo:"},
       { "OK", "OK"},
       { "Cancel", "Cancelar"},
       { "Enter name...", "Escribe el nombre..."},
       { "Terrain Name:", "Nombre del terreno:"},
       { "Difficulty:", "Dificultad:"},
       { "Type of game:", "Tipo de juego:"},

       { "Pirates (if check the town could suffer a Pirate attack)", "Piratas (El pueblo pudiese sufrir el ataque de piratas)"},
       { "Food Expiration (if check food expires with time)", "Caducidad de la comida (La comida tiene fecha de caducidad)"},

        { "Freewill", "Libertad"},
        { "Traditional", "Tradicional"},


        { "Click Here", "Haz click aquí"},

       { "Newbie", "Novato"},
       { "Easy", "Fácil"},
       { "Moderate", "Mas o menos"},
       { "Hard", "Duro"},
       { "Insane", "Locura"},

       { "Save Name:", "Nombre de la Salva:"},
       { "Delete", "Borra"},
       { "FullScreen:", "Pantalla completa:"},
       { "Quality:", "Calidad:"},
       { "Resolution:", "Resolucion:"},
       { "Screen", "Pantalla"},

       { "Music:", "Musica:"},
       { "Audio", "Audio"},
       { "Sound:", "Sonido:"},
       { "General", "General"},
       { "Unit System:", "Sistema de unidades:"},

       { "AutoSave Frec:", "Frecuencia de auto salva:"},
       { "Language:", "Lenguage:"},
       //{ "Loading...", "Cargando..."},
       { "Menu", "Menu"},

       { "Camera Sensitivity:", "Velocidad de la Camara:"},

       //
       { "Tutorial(Beta)", "Tutorial(Beta)"},




       //in game
        { "Buildings.Ready", "\n Edificios listos para ser construidos:"},
        { "People.Living", "Personas en esta casa:"},
        { "Occupied:", "En uso:"},
        { "|| Capacity:", "|| Capacidad:"},
        { "Users:", "\nUsuarios:"},

        { "Orders in progress:", "Ordenes en progreso::"},

        { "Amt.Cant.Be.0", "Amount can't be 0"},
        { "Prod.Not.Select", "Please select a product"},
        //ProductStat.cs has a lot of text to put here 

        //SpecTile.cs has

        //ShowAPersonBuildingDetails has
        //BuildingWindow.cs
    };

    public static string ReturnString(string key)
    {
        if (_currentLang == "English")
        {
            if (_english.ContainsKey(key))
            {
                return _english[key];
            }
            //in English if key is not found will return key alone 
            //'Potato' is an ex, will passed as a key and is not even in the Dict
            return key;
        }
        else if (_currentLang == "Español(Beta)")
        {
            if (_spanish.ContainsKey(key))
            {
                return _spanish[key];
            }
            return key + " not in ES Languages";
        }
        return "not languages selected ";
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
            return _spanish;
        }

        return _english;

    }
}
