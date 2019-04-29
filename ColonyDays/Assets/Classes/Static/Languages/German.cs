using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class German
{
    private static Dictionary<string, string> _german = new Dictionary<string, string>();

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

        _german = new Dictionary<string, string>()
        {
            //Descriptions
            //Infr
            { "Road.Desc","Used for decoration purposes. People are happier if there are roads around them"},
            { "BridgeTrail.Desc","Allows people to pass from one side of the terrain to the other"},
            { "BridgeRoad.Desc","Allows people to pass from one side of the terrain to the other. People love these bridges. It gives a sense of prosperity and happiness" +_houseTail},
            { "LightHouse.Desc","Helps to increase Port visibility. Adds to Port Reputation as long it has workers in it"},
            { H.Masonry + ".Desc","Important building, workers construct new buildings and work as wheelbarrows once they have nothing to do"},
            { H.StandLamp + ".Desc","Illuminates at night if Whale Oil is available on the town's storage"},

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
            { "SugarShop.Desc", "Produces sugar subproducts!!!. " + _produce},


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

            //Decoration
            { "Fountain.Desc", "Beauty your town and adds overall happiness to your citizens"},
            { "WideFountain.Desc", "Beauty your town and adds overall happiness to your citizens"},
            { "PalmTree.Desc", "Beauty your town and adds overall happiness to your citizens"},
            { "FloorFountain.Desc", "Beauty your town and adds overall happiness to your citizens"},
            { "FlowerPot.Desc", "Beauty your town and adds overall happiness to your citizens"},
            { "PradoLion.Desc", "Beauty your town and adds overall happiness to your citizens"},



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
            {  "LumberMill" ,"Lumbermill"},
            { "BlackSmith","Blacksmith"},
            { "ShoreMine","Shore Mine"},
            { "QuickLime","Quicklime"},

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
            { "SugarMill","Sugarmill"},
            { "Foundry","Foundry"},
            { "SteelFoundry","Steel Foundry"},
            { "SugarShop","Sugarshop"},


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

            //Decorations
            { "Fountain", "Fountain"},
            { "WideFountain", "Wide Fountain"},
            { "PalmTree", "Palm Tree"},
            { "FloorFountain", "Floor Fountain"},
            { "FlowerPot", "Flower Pot"},
            { "PradoLion", "Prado Lion"},

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
            { "PathToSeaExplain", "Displays the shortest way to the sea."},


            { "BugReport", "Caught a bug? uhmm yummy.... Send it this way!! Thanks"},
            { "Invitation", "Your friend's email for a chance to join the Private Beta"},
            { "Info", ""},//use for informational Dialogs
            { "Negative", "The Crown extended a line of credit for you. If you own more than $100,000.00 is game over"},  


            //MainMenu
                { "Types_Explain", "Traditional: \nIt's a game where in the beginning some buildings are locked and you have to unlock them. " +
                    "The good thing is that this provides you with guidance." +
                    "\n\nFreewill: \nAll available buildings are unlocked right away. " +
                    "The bad thing is this way you can fail very easily." +
                    "\n\nThe 'Hard' difficulty is the closest to reality"},


            //Tooltips
            //Small Tooltips
            { "Person.HoverSmall", "Total/Adults/Kids"},
            { "Emigrate.HoverSmall", "Emigrated"},
            { "CurrSpeed.HoverSmall", "Game Speed"},
            { "Town.HoverSmall", "Town Name"},
            { "Lazy.HoverSmall", "Unemployed People"},
            { "Food.HoverSmall", "Food"},
            { "Happy.HoverSmall", "Happiness"},
            { "PortReputation.HoverSmall", "Reputation of Port"},
            { "Dollars.HoverSmall", "Dollars"},
            { "PirateThreat.HoverSmall", "Pirate Threat"},
            { "Date.HoverSmall", "Date (m/y)"},
            { "MoreSpeed.HoverSmall", "More Speed [PgUp]"},
            { "LessSpeed.HoverSmall", "Less Speed [PgDwn]"},
            { "PauseSpeed.HoverSmall", "Pause Game"},
            { "CurrSpeedBack.HoverSmall", "Current Speed"},
            { "ShowNoti.HoverSmall", "Notifications"},
            { "Menu.HoverSmall", "Main Menu"},
            { "QuickSave.HoverSmall", "Quick Save[Ctrl+S][F]"},
            { "Bug Report.HoverSmall", "Report a Bug"},
            { "Feedback.HoverSmall", "Feedback"},
            { "Hide.HoverSmall", "Hide"},
            { "CleanAll.HoverSmall", "Clean"},
            { "Bulletin.HoverSmall", "Control/Bulletin"},
            { "ShowAgainTuto.HoverSmall","Tutorial"},
            { "BuyRegion.HoverSmall", "Buy Regions"},
            { "Help.HoverSmall", "Help"},

            { "More.HoverSmall", "More"},
            { "Less.HoverSmall", "Less"},
            { "Prev.HoverSmall", "Previous"},

            { "More Positions.HoverSmall", "More"},
            { "Less Positions.HoverSmall", "Less"},


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
            { "Decoration.HoverSmall", "Decoration"},

            { "WhereIsTown.HoverSmall", "Back to Town [P]"},
            { "WhereIsSea.HoverSmall", "Show Ocean Path"},
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
            { "More Salary.HoverSmall", "Pay More"},
            { "Less Salary.HoverSmall", "Pay Less"},
            { "Next_Stage_Btn.HoverSmall", "Buy Next Stage"},
            { "Current_Salary.HoverSmall", "Current Salary"},
            { "Current_Positions.HoverSmall", "Current Positions"},
            { "Max_Positions.HoverSmall", "Max Positions"},


            { "Add_Import_Btn.HoverSmall", "Add an Import"},
            { "Add_Export_Btn.HoverSmall", "Add an Export"},
            { "Upg_Cap_Btn.HoverSmall", "Upgrades Capacity"},
            { "Close_Btn.HoverSmall", "Close"},
            { "ShowPath.HoverSmall", "Show Path"},
            { "ShowLocation.HoverSmall", "Show Location"},//TownTitle
            { "TownTitle.HoverSmall", "Town"},
            {"WarMode.HoverSmall", "Combat Mode"},
            {"BullDozer.HoverSmall", "Bulldozer"},
            {"Rate.HoverSmall", "Rate Me"},

            //addOrder windiw
            { "Amt_Tip.HoverSmall", "Product Amount"},

            //Med Tooltips 
            { "Build.HoverMed", "Place building: 'Left click' \n" +
                                "Rotate building: 'R' key \n" +
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
            { "CantProduceBzFullStore.Noti.Desc", "{0} because his/her destination storage is full"},

            { "NoInput.Noti.Name", "At least an input is missing in building"},
            { "NoInput.Noti.Desc", "A building cannot produce {0} because is missing at least one input"},

            { "Built.Noti.Name", "A building has been built"},
            { "Built.Noti.Desc", "{0} has been fully built"},

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
            { "Demolition.HoverMed", "Once the inventory is clear will be demolished. Wheelbarrows will move the inventory"},

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

            { "Population.Help", "Once they turn 16 will move to a free house if found. If there is always a free house to move to the population growth will be guaranteed. If they get into the new houses at 16 years old, you are maximizing population growth"},


            { "F1.Help", "Press [F1] for help"},

            { "Inputs.Help", "If a building can't produce because is missing inputs. Check you have the needed input(s) in the main storage and at least one worker in the masonry"},
            { "WheelBarrows.Help", "Wheelbarrows are the masonry workers. If they got nothing to build will act as wheelbarrows. If you need inputs to get into a specific building make sure you have enough of them working and also the inputs mentioned in the storage building"},

            { "Production Tab.Help", "If the building is a farm field make sure you have workers on the farm. The crop will be lost if sits there a month after harvest day"},
            { "Our Inventories.Help", "The section 'Our inventories' in the 'Add Order Window' is a resume of what we got in our Storages buildings inventories"},
            { "Inventories Explanation.Help", "This a resume of what we got in our Storages inventories. Items in other buildings inventories do not belong to the city"},

            ///word and grammarly below




            //to  add on spanish         //to correct  
            { "TutoOver", "Your reward is $10,000.00 if is the first time you complete it. The tutorial is over now you can keep playing this game or start a new one."},

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
            { "Spec.Tuto", "Click the 'Prod' menu and then on 'Spec'. Here you can see exactly how to make each product on the game. The inputs necessaries and where is produced. Also, the import and export prices"},
            { "Exports.Tuto", "Click the 'Finance' menu and then on 'Export'. Here you can see a breakdown of your village's exports"},


                //Quest
            { "Tutorial.Quest", "Quest: Finish the tutorial. Reward $10,000. It takes roughly 3 minutes to complete"},

            { "Lamp.Quest", "Quest: Build a StandLamp. Find it on Infrastructure, it shines at night if there is Whale Oil on Storage"},

            { "Shack.Quest", "Quest: Build a Shack. These are cheap houses. When people turn 16 they will move to a free house if found. In this way, population growth will be guaranteed. [F1] Help. If you see smoke in a house's chimney means there are people living in it"},

            { "SmallFarm.Quest", "Quest: Build a Small Field Farm. You need farms to feed your people"},
            { "FarmHire.Quest", "Quest: Hire two farmers in the Small Field Farm. Click on the farm and in the plus sign assign workers. You need to have unemployed"
                    +" people to be able to assign them into a new building"},



            { "FarmProduce.Quest", "Quest: Now produce " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " of beans on the Small Field Farm. Click on the 'Stat' tab and will show you the production of the last 5 years. You can see the quest progress in the quest window. If you build more small farms will be accounted for the quest"},
            { "Transport.Quest", "Quest: Transport the beans from the farm to the Storage. To do that make sure you have" +
                " workers on the masonry. They act as wheelbarrows when not building"},


            { "HireDocker.Quest", "Quest: Hire a docker. Dockers only task is to move the goods into the Dock from the Storage if you are exporting."+
            " Or vice-versa if importing. They work when there is an order in place and the goods are ready to transport. Otherwise, they stay at home resting." +
                " If you haven't built a Dock then build one."+
            " Find it in Trade." },


            { "Export.Quest", "Quest: At the Dock create an order and export exactly 300 " + Unit.CurrentWeightUnitsString() + " of beans."+
                " In the Dock click on the 'Orders' tab and add an export order with the '+' sign."+
            " Select product and enter amount"},



            { "MakeBucks.Quest", "Quest: Make $100 exporting goods in the Dock. "+
            "Once a ship arrives will randomly pay product(s) in your Dock's inventory"},
            { "HeavyLoad.Quest", "Quest: Build a Heavyload building. This are haulers that carry more weight. They will come handy when transporting goods around is needed." }, //Carts must be available on towns storages for them to work"},
            { "HireHeavy.Quest", "Quest: In the Heavyload building hire a Heavy Hauler."},


            { "ImportOil.Quest", "Quest: Import 500 " + Unit.CurrentWeightUnitsString() + " of Whale Oil at the Dock. This is needed to keep your lights on at night. Ships will randomly drop imports in your Dock's inventory"},

            { "Population50.Quest", "Reach a total population of 50 citizens"},

            //added Aug 11 2017, result: sep 9(30% off biggest sale ever)
            { "Production.Quest", "Let's produce Weapons now and sell them later. First of all, build a Blacksmith. Find it in the 'Raw' buildings menu"},
            { "ChangeProductToWeapon.Quest", "In the Blacksmith's 'Products Tab' change the production to Weapon. Workers will bring the raw materials needed to forge weapons if found"},
            { "BlackSmithHire.Quest", "Hire two blacksmiths"},
            { "WeaponsProduce.Quest", "Now produce " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " of Weapons in the Blacksmith. Click on the 'Stat' tab and will show you the production of the last 5 years. You can see the quest progress in the quest window."},
            { "ExportWeapons.Quest", "Now export 100 " + Unit.CurrentWeightUnitsString() + " of Weapons. On the Dock add an order of export. Notice that Weapons are a profitable business"},


            {"CompleteQuest", "Your reward is {0}"},


            //added Sep 14 2017
            { "BuildFishingHut.Quest", "Build a Fishing hut. In this way citizens have different foods to eat, which translates into happiness"},
            { "HireFisher.Quest", "Hire a fisher"},

            { "BuildLumber.Quest", "Build a Lumbermill. Find it in the 'Raw' buildings menu"},
            { "HireLumberJack.Quest", "Hire a Lumberjack"},

            { "BuildGunPowder.Quest", "Build a Gunpowder. Find it in the 'Industry' buildings menu"},
            { "ImportSulfur.Quest", "In the dock import 1000 " + Unit.CurrentWeightUnitsString() + " of Sulfur"},
            { "GunPowderHire.Quest", "Hire one worker in the Gunpowder building"},

            { "ImportPotassium.Quest", "In the dock import 1000 " + Unit.CurrentWeightUnitsString() + " of Potassium"},
            { "ImportCoal.Quest", "In the dock import 1000 " + Unit.CurrentWeightUnitsString() + " of Coal"},

            { "ProduceGunPowder.Quest", "Lets produce now " + Unit.WeightConverted(100).ToString("n0") + " " + Unit.CurrentWeightUnitsString() + " of Gunpowder. Notice that you will need Sulfur, Potassium and Coal to produce Gunpowder"},
            { "ExportGunPowder.Quest", "In the dock export 100 " + Unit.CurrentWeightUnitsString() + " of Gunpowder"},

            { "BuildLargeShack.Quest", "Build a Largeshack in this bigger houses population will grow faster"},

            { "BuildA2ndDock.Quest", "Build a second Dock. This dock could be used only for imports in that way you can import raw materials here and export them at another dock"},
            { "Rename2ndDock.Quest", "Rename the Docks now, so you can remember which be used only for imports and exports"},

            { "Import2000Wood.Quest", "In the Imports dock import 2000 " + Unit.CurrentWeightUnitsString() + " of Wood. This raw material is needed for everything because is used as fuel"},

            //IT HAS FINAL MESSAGE 
            //last quest it has a final message to the player. if new quest added please put the final message in the last quest
            { "Import2000Coal.Quest", "In the Imports dock import 2000 " + Unit.CurrentWeightUnitsString() + " of Coal. Coal also, is needed for everything because is used as fuel. Hope you enjoy the experience so far. Keep expanding your colony, and wealth. Also, please help to improve the game. Participate in the online forums your voice and opinions are important! Have fun Sugarmiller!"},

            //



            //Quest Titles
            { "Tutorial.Quest.Title", "Tutorial"},
            { "Lamp.Quest.Title", "Stand Lamp"},

            { "Shack.Quest.Title", "Build a Shack"},
            { "SmallFarm.Quest.Title", "Build a Farm Field"},
            { "FarmHire.Quest.Title", "Hire Two Farmers"},


            { "FarmProduce.Quest.Title", "Farm Producer"},

            { "Export.Quest.Title", "Exports"},
            { "HireDocker.Quest.Title", "Hire Docker"},
            { "MakeBucks.Quest.Title", "Make Money"},
            { "HeavyLoad.Quest.Title", "Heavy Load"},
            { "HireHeavy.Quest.Title", "Hire a Heavy Hauler"},

            { "ImportOil.Quest.Title", "Whale Oil"},

            { "Population50.Quest.Title", "50 Citizens"},
            
            //
            { "Production.Quest.Title", "Produce Weapons"},
            { "ChangeProductToWeapon.Quest.Title", "Change Product"},
            { "BlackSmithHire.Quest.Title", "Hire Two Blacksmiths"},
            { "WeaponsProduce.Quest.Title", "Forge Weapons"},
            { "ExportWeapons.Quest.Title", "Make Profit" },
            
            //
            { "BuildFishingHut.Quest.Title", "Build a Fishing Hut"},
            { "HireFisher.Quest.Title", "Hire a Fisher"},
            { "BuildLumber.Quest.Title", "Build a Lumber"},
            { "HireLumberJack.Quest.Title", "Hire a Lumberjack"},
            { "BuildGunPowder.Quest.Title", "Build a Gunpowder"},
            { "ImportSulfur.Quest.Title", "Import Sulfur"},
            { "GunPowderHire.Quest.Title", "Hire a Gunpowder worker"},
            { "ImportPotassium.Quest.Title", "Import Potassium"},
            { "ImportCoal.Quest.Title", "Import Coal"},
            { "ProduceGunPowder.Quest.Title", "Produce Gunpowder"},
            { "ExportGunPowder.Quest.Title", "Export Gunpowder"},
            { "BuildLargeShack.Quest.Title", "Build a Largeshack"},
            { "BuildA2ndDock.Quest.Title", "Build a Second Dock"},
            { "Rename2ndDock.Quest.Title", "Rename the Second Dock"},
            { "Import2000Wood.Quest.Title", "Import some Wood"},
            { "Import2000Coal.Quest.Title", "Import some Coal"},




            {"Tutorial.Arrow", "This is the tutorial. Once finished you will win $10,000"},
            {"Quest.Arrow", "This is the quest button. You can access the quest window by clicking on it"},
            {"New.Quest.Avail", "At least one quest is available"},
            {"Quest_Button.HoverSmall", "Quest"},



            //Products
            //Notification.Init()
            {"RandomFoundryOutput", "Melted Ore"},

            //OrderShow.ShowToSetCurrentProduct()
            { "RandomFoundryOutput (Ore, Wood)", "Melted Ore (Ore, Wood)"},



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
            {"Help.Bulletin/Finance/Imports", "A breakdown of the imports"},


            { "Help.Bulletin/Finance/Prices", "...."},


            {"LoadWontFit", "This load won't fit in the storage area"},

            {"Missing.Input", "Building can't produce (Inputs must be in this building inventory). Missing inputs: \n" },





            //in game
            
            { "Buildings.Ready", "\n Buildings ready to be built:"},
            { "People.Living", "People living in this house:"},
            { "Occupied:", "Filled:"},
            { "|| Capacity:", "|| Capacity:"},
            { "Users:", "\nUsers:"},
            { "Amt.Cant.Be.0", "Amount can't be 0"},
            { "Prod.Not.Select", "Please select a product"},


            //articles
            { "The.Male", "The"},
            { "The.Female", "The"},

            //
            { "Build.Destroy.Soon", "This building will be destroyed soon. If inventory is not empty, it needs to be cleared by wheelbarrows"},




            //words
            //Field Farms
            { "Bean", "Bean"},
            { "Potato", "Potato"},
            { "SugarCane", "Sugarcane"},
            { "Corn", "Corn"},
            { "Cotton", "Cotton"},
            { "Banana", "Banana"},
            { "Coconut", "Coconut"},
            //Animal Farm
            { "Chicken", "Chicken"},
            { "Egg", "Egg"},
            { "Pork", "Pork"},
            { "Beef", "Beef"},
            { "Leather", "Leather"},
            { "Fish", "Fish"},
            //mines
            { "Gold", "Gold"},
            { "Stone", "Stone"},
            { "Iron", "Iron"},

            // { "Clay", "Clay"},
            { "Ceramic", "Ceramic"},
            { "Wood", "Wood"},

            //Prod
            { "Tool", "Tool"},
            { "Tonel", "Tonel"},
            { "Cigar", "Cigar"},
            { "Tile", "Tile"},
            { "Fabric", "Fabric"},
            { "Paper", "Paper"},
            { "Map", "Map"},
            { "Book", "Book"},
            { "Sugar", "Sugar"},
            { "None", "None"},
            //
            { "Person", "Person"},
            { "Food", "Food"},
            { "Dollar", "Dollar"},
            { "Salt", "Salt"},
            { "Coal", "Coal"},
            { "Sulfur", "Sulfur"},
            { "Potassium", "Potassium"},
            { "Silver", "Silver"},
            { "Henequen", "Henequen"},
            //
            { "Sail", "Sail"},
            { "String", "String"},
            { "Nail", "Nail"},
            { "CannonBall", "Cannonball"},
            { "TobaccoLeaf", "Tobaccoleaf"},
            { "CoffeeBean", "Coffeebean"},
            { "Cacao", "Cacao"},
            { "Weapon", "Weapon"},
            { "WheelBarrow", "Wheelbarrow"},
            { "WhaleOil", "Whaleoil"},
            //
            { "Diamond", "Diamond"},
            { "Jewel", "Jewel"},
            { "Rum", "Rum"},
            { "Wine", "Wine"},
            { "Ore", "Ore"},
            { "Crate", "Crate"},
            { "Coin", "Coin"},
            { "CannonPart", "Cannon Part"},
            { "Steel", "Steel"},
            //
            { "CornFlower", "Cornflower"},
            { "Bread", "Bread"},
            { "Carrot", "Carrot"},
            { "Tomato", "Tomato"},
            { "Cucumber", "Cucumber"},
            { "Cabbage", "Cabbage"},
            { "Lettuce", "Lettuce"},
            { "SweetPotato", "Sweetpotato"},
            { "Yucca", "Yucca"},
            { "Pineapple", "Pineapple"},
            //
            { "Papaya", "Papaya"},
            { "Wool", "Wool"},
            { "Shoe", "Shoe"},
            { "CigarBox", "Cigarbox"},
            { "Water", "Water"},
            { "Beer", "Beer"},
            { "Honey", "Honey"},
            { "Bucket", "Bucket"},
            { "Cart", "Cart"},
            { "RoofTile", "Rooftile"},
            { "FloorTile", "Floortile"},
            { "Furniture", "Furniture"},
            { "Crockery", "Crockery"},

            { "Utensil", "Utensil"},
            { "Stop", "Stop"},


            //more Main GUI
            { "Workers distribution", "Workers distribution"},
            { "Buildings", "Buildings"},

            { "Age", "Age"},
            { "Gender", "Gender"},
            { "Height", "Height"},
            { "Weight", "Weight"},
            { "Calories", "Calories"},
            { "Nutrition", "Nutrition"},
            { "Profession", "Profession"},
            { "Spouse", "Spouse"},
            { "Happinness", "Happinnes"},
            { "Years Of School", "Years of School"},
            { "Age majority reach", "Age majority reach"},
            { "Home", "Home"},
            { "Work", "Work"},
            { "Food Source", "Food Source"},
            { "Religion", "Religion"},
            { "Chill", "Chilll"},
            { "Thirst", "Thirst"},
            { "Account", "Account"},

            { "Early Access Build", "Early Access Build"},

            //Main Menu
            { "Resume Game", "Resume Game"},
            { "Continue Game", "Continue Game"},
            { "Tutorial(Beta)", "Tutorial(Beta)"},
            { "New Game", "New Game"},
            { "Load Game", "Load Game"},
            { "Save Game", "Save Game"},
            { "Achievements", "Achievements"},
            { "Options", "Options"},
            { "Exit", "Exit"},
            //Screens
            //New Game
            { "Town Name:", "Town Name:"},
            { "Difficulty:", "Difficulty:"},
            { "Easy", "Easy"},
            { "Moderate", "Moderate"},
            { "Hard", "Hard"},
            { "Type of game:", "Type of game:"},
            { "Freewill", "Freewill"},
            { "Traditional", "Traditional"},
            { "New.Game.Pirates", "Pirates (if checked the town could suffer a pirate attack"},
            { "New.Game.Expires", "Food Expiration (if checked food expires with time)"},
            { "OK", "OK"},
            { "Cancel", "Cancel"},
            { "Delete", "Delete"},
            { "Enter name...", "Enter name..."},
            //Options
            { "General", "General"},
            { "Unit System:", "Unit System:"},
            { "Metric", "Metric"},
            { "Imperial", "Imperial"},
            { "AutoSave Frec:", "Autosave:"},
            { "20 min", "20 min"},
            { "15 min", "15 min"},
            { "10 min", "10 min"},
            { "5 min", "5 min"},
            { "Language:", "Language:"},
            { "English", "English"},
            { "Camera Sensitivity:", "Camera Sensitivity:"},
            { "Themes", "Themes"},
            { "Halloween:", "Halloween:"},
            { "Christmas:", "Christmas:"},
            { "Options.Change.Theme", "When changed please reload the game"},

            { "Screen", "Screen"},
            { "Quality:", "Quality:"},
            { "Beautiful", "Beautiful"},
            { "Fantastic", "Fantastic"},
            { "Simple", "Simple"},
            { "Good", "Good"},
            { "Resolution:", "Resolution:"},
            { "FullScreen:", "Fullscreen:"},

            { "Audio", "Audio"},
            { "Music:", "Music:"},
            { "Sound:", "Sound:"},
            { "Newborn", "Newborn"},
            { "Build Completed", "Build Completed"},
            { "People's Voice", "People's Voice"},
            
            //in game gui
            { "Prod", "Prod"},
            { "Finance", "Finance"},



        };

    }

    internal static void Clear()
    {
        _german.Clear();
    }

    internal static Dictionary<string, string> Dictionary()
    {
        return _german;
    }
    
    public static bool ContainsKey(string key)
    {
        return _german.ContainsKey(key);
    }

}