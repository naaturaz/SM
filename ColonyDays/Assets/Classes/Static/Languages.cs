using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Languages
{
    private static string _currentLang = "English";


    private static string _houseTail = ". SugarMiller's live here and enjoy having a nice meal at least once in a while";
    private static string _animalFarmTail = ", you can raise different animals in this building";
    private static string _fieldFarmTail = ", you can seed different crops and fruits in this building";
    private static string _asLongHasInput = ", as long as it has the necessary inputs";
    private static string _produce = "In this building workers will produce the selected product, as long as it has the necessary inputs";
    private static string _storage =
        "You can use this building as storage, however if it gets full people won't work if they dont have room where to store their products";
    private static string _militar = "This building helps to decrease the Pirate Threat on your port, to be effective it must have workers. The more workers the better";


    static Dictionary<string, string> _english = new Dictionary<string, string>()
	{
       //Descriptions
       //Infr
	   { "Road.Desc","Only for decoration purposes. However people are happier if there are roads around them"},
	   { "BridgeTrail.Desc","Allows people to pass from one side of the terrain to the other"},
	   { "BridgeRoad.Desc","Allows people to pass from one side of the terrain to the other. People love these bridges. It gives a sense of prosperity and happinnes" +_houseTail},
	   { "LightHouse.Desc","Helps to increase Port visibility. Adds to Port Reputation as long as the flame is glowing"},
	   { H.Masonry + ".Desc","Important building, workers construct new buildings and work as wheelbarrowers once they have nothing to do"},
	   { H.StandLamp + ".Desc","Iluminates at night if Whale Oil is available on the town's storage."},
	   
       { H.HeavyLoad + ".Desc","These workers use horse wagons to move goods around"},


       //House
	   { "Bohio.Desc","Bohio house, primitive conditionns with unhappy people whom can only have a maximum of 2 to 3 kids" +_houseTail},
	   { "BohioB.Desc","Bohio house, primitive conditionns with unhappy people whom can only have a maximum of 2 to 3 kids" +_houseTail},
	   { "WoodHouseA.Desc","Small wooden house, a family can have 2-4 kids max" +_houseTail},
	   { "WoodHouseB.Desc","Medium wooden house, a family can have 4 kids max" +_houseTail },
	   { "WoodHouseC.Desc","Small and confortable wooden house, a family can have 2-4 kids max"+_houseTail},
	   { "BrickHouseA.Desc","Large house, a family can have 4 kids max"+_houseTail},
	   { "BrickHouseB.Desc","Large house, a family can have 4 kids max"+_houseTail},
	   { "BrickHouseC.Desc","Large house, a family can have 4 kids max"+_houseTail},
       
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
	   { H.FishingHut + ".Desc","Here a worker can catch fish in a river"},

       //Raw
	   { "Mortar.Desc","Here a worker will produce Mortar"},

	   { "Clay.Desc","Here a worker will produce Clay, raw material is needed for bricks and more"},
	   { "Pottery.Desc","Here a worker will produce crockery products, such as dishes, jars, etc"},
	   { "Mine.Desc","Here a worker can fish in a river"},
	   { "MountainMine.Desc","Here a worker will work the mine by extracting ore"},
	   { "Resin.Desc","Here a worker will work the mine by extracting minerals and metals randomly"},
	   {  H.LumberMill +".Desc","Here workers will find resources such as wood, stone, and ore"},
	   { "BlackSmith.Desc","Here workers will produce the product selected"+_asLongHasInput},
	   { "ShoreMine.Desc","Here workers will produce salt, sand or quicklime"},

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
	   { "Paper_Mill.Desc",_produce},
	   { "Printer.Desc",_produce},
	   { "CoinStamp.Desc",_produce},
	   { "Silk.Desc",_produce},
	   { "SugarMill.Desc",_produce},
	   { "Foundry.Desc",_produce},
	   { "SteelFoundry.Desc",_produce},

       //trade
	   { "Dock.Desc","Here you can add import or export orders"},
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
	   { "Church.Desc","The church gives happinnes and hope to your people"},
	   { "Tavern.Desc","The tavern gives some relaxation and entertainment to your people"},

       //Militar
	   { "PostGuard.Desc",_militar},
	   { "Fort.Desc",_militar},
	   { "Morro.Desc",_militar+". Once you build this, Pirates should know better"},




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
        { "Types_Explain", "Traditional: \nIt's a game where in the begining some buildings are locked and you have to unlock them. " +
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
	   { "MoreSpeed.HoverSmall", "More speed"},
	   { "LessSpeed.HoverSmall", "Less speed"},
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

       //Todo add to ESP
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

       //addOrder windiw
	   { "Amt_Tip.HoverSmall", "Amount of prod"},

       //Med Tooltips 
	   { "Build.HoverMed", "Place building: 'Left click' \n" +
	                       "Rotate building: 'R' key \n " +
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

       { "Emigrate.Noti.Name", "A person emigrated"},
	   { "Emigrate.Noti.Desc", "People emigrate when they are not happy with your government"},
       { "PortUp.Noti.Name", "Port is known"},
	   { "PortUp.Noti.Desc", "Your port reputation is ramping up with neighbouring ports and routes"},
       { "PortDown.Noti.Name", "Port is less known"},
	   { "PortDown.Noti.Desc", "Your port reputation went down"},

       { "BoughtLand.Noti.Name", "New Land Purchased"},
	   { "BoughtLand.Noti.Desc", "A new land region was purchased"},

       { "ShipPayed.Noti.Name", "Ship payed"},
	   { "ShipPayed.Noti.Desc", "A ship has payed {0} for goods or service"}, 
       { "ShipArrived.Noti.Name", "A ship has arrived"},
	   { "ShipArrived.Noti.Desc", "A new ship has arrived to one of our maritimes buildings"},

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
       { "NotScaledOnFloor", "The building is either to close to shore or moutain"},
       { "NotEven", "The ground underneath the building is not even"},
       { "Colliding", "Building is colliding with another one"},
       { "BadWaterHeight", "The building is too low or high on the water"},
       { "LockedRegion", "You need to own this region to build here"},
       { "HomeLess", "People in this house have no where to go. Please build a new house that" +
                     " can hold this family and try again"},   
       { "LastFood", "Cannot destroy, this is the only Storage in your village"},
       { "LastMasonry", "Cannot destroy, this is the only Masonry in your village"},


       //help

       { "Construction.HoverMed", "For the construction of any building you need to have workers in the Masonry. "+
            " Click the Masonry, then the '+' sign in the general tab. Make sure you have enough resources"},
       { "Camera", "Camera: Use [WASD] or cursor to move. " +
                   "Press the scroll wheel on your mouse, keep it pressed to rotate, or [Q] and [E]"},
       { "SeaPath", "Click on the left bottom corner 'Show/hide sea path' " +
                    "button to show the closest path to the sea"},

       { "PeopleRange", "The huge blue circle around each building marks the range of it"},

       { "PirateThreat.Help", "Pirate Threat: This is how aware are the pirates of your port. This increases as" +
	                              " you have more money. If this reaches over 90 you will lose the game"},

	   { "PortReputation.Help", "Port Reputation: The more people know your port, the more they will visit." +
	                                " If you want to increase this make sure you always have some orders" +
	                                " in the port, supplier and shipyard"},
	   { "Emigrate.Help", "Emigrates: When people are unhappy for a few years they leave. The bad" +
	                          " part of this is they won't come back, they won't produce or have children." +
	                          " The only good thing is that they increase the 'Port Reputation'"},
	   { "Food.Help", "Food: The higher the variety of food available in a household, the happier they" +
	                      " will be."},	  
                          
       { "Weight.Help", "Weight: All the weights in the game are in Kg or Lbs depending on which Unit system is selected." +
                        " You can change it in 'Options' in the 'Main Menu'"},


       { "More.Help", "If you need more help might be a good idea completing the tutorial"},








       //to  add on spanish         //to correct  
       { "TutoOver", "Your reward is $10,000. The tutorial is over now you can keep playing this game or start a new one."},

       //Tuto
       { "CamMov.Tuto", "Tutorial completion reward is $10,000. Step1: Use [WASD] or arrow keys to move the Camera."},
       { "CamMov5x.Tuto", "Use [WASD] or arrow keys and keep press the 'Left Shift' key to move the Camera 5 times quicker."},
       { "CamRot.Tuto", "Now press the scroll wheel down on your mouse and move your mouse to rotate the Camera."},

       
       { "BackToTown.Tuto", "Press the key [P] on the keyboard to go to the initial position of the camera"},
      
       { "BuyRegion.Tuto", "Regions, you need to own a region to be able to build in it. Click on '+' sign on the botton bar, then on the 'For Sale' sign in the" +
            " middle of a region to buy it. Some buildings are exempt, they can be built without ownig the region" +
            " (FishingHut, Dock, MountainMine, ShoreMine, LightHouse, PostGuard)"
            },
       
       { "Trade.Tuto", "That was easy, the hard part is coming. Click on the 'Trade' buildings button, located in the right bottom bar. "+
           "When you hover over it, it will popup 'Trade'"},
       { "CamHeaven.Tuto", "Scroll back with your mouse middle button until the camera reaches"
            + " the sky. This view is useful to place bigger buildings such as the Port"},

       { "Dock.Tuto", "Now click on the 'Dock' building, it is the 1st button. When you hover over it, it will"+
           " show it's cost and description"},
       { "Dock.Placed.Tuto", "Now the hard, read carefully. Notice that you can use the "+
           "'R' key to Rotate, and right click to cancel the building. You can cancel the building and respawned at you wish. This building has a part in the ocean and other in land." +
           " The arrow goes to the sea, the storage section goes to land. Once the arrow is white left click."},
       { "MaxSpeed.Tuto", "To go to max speed, go to the middle top screen simulation speed controller, click the "
            +" 'More Speed' button 3 times until 10x is displayed"},

       { "ShowWorkersControl.Tuto", "Click on the 'Control/Bulletin' button, located in the bottom bar. "+
           "When you hover over it, it will popup 'Control/Bulletin'. "},
       { "AddWorkers.Tuto", "Click the '+' sign to the right of the Masonry building, this is how you add more workers."
        + " You can also assign workers in the buildings."},
       { "HideBulletin.Tuto", "Keep in mind that in this window you are able to control and see differents aspects of the game. Click outside the window to close it or 'OK' button."},
       { "FinishDock.Tuto", "Now finish the Dock building. The more workers are in the Masonry the quicker is going to get done too."
       + " Also make sure you have all materials needed to build it"},
       { "ShowHelp.Tuto", "Click on the 'Help' button, located in the right bottom bar. "+
           "When you hover over it, it will popup 'Help'. There you can find some tips. " +
           " When you are done click outside the 'Help' window and will close."},



       { "SelectDock.Tuto", "Ships drop and pick goods at random from the dock's inventory. Workers are needed to move dock goods. This workers use wheelbarrows and crates. if are not this items on store they won't work. Now click on the Dock."},
       { "OrderTab.Tuto", "Go to the Orders tab."},
       { "ImportOrder.Tuto", "Click on the '+' sign besides Add Import Order."},
       
       { "AddOrder.Tuto", "Now scroll down in the products and select wood and enter 100 as the amount. Then click the 'Add' button."},
       { "CloseDockWindow.Tuto", "Now the order is added. A random ship will drop this item in the dock inventory. And then your dock workers will take it to the closest Storage building. Now click outside the window so the window closes."},
       { "Rename.Tuto", "Click on a person and then click on the title bar of the person. Like this you can change the name of any person or building in the game. Click outside so the change is saved"},

       { "Spec.Tuto", "Click on the 'Control/Bulletin' button, then on the 'Prod' menu and then on the 'Spec'. Here you can see exactly how to make each product on the game. The inputs necesaries and where is produced. Also the price"},




       //Quest
       { "Tutorial.Quest", "Quest: Finish the tutorial. Reward $10,000. It takes roughly 3 minutes to complete"},
       
       { "Lamp.Quest", "Quest: Build a StandLamp. Find it on Infrastructure, it shines at night if there is Whale Oil on Storage"},

       { "Bohio.Quest", "Quest: Build a bohio. This are cheap houses. When people turn 16 they will move to a free house if found. In this way population growth will be guaranteed. [F1] Help"},
       { "SmallFarm.Quest", "Quest: Build a FieldFarmSmall. You need farms to feed your people"},
       { "FarmHire.Quest", "Quest: Hire two farmers in a FieldFarmSmall. Click on the farm and in the plus sign assign workers. You need to have unemployed"
            +" people to be able to assign them into a new building"},
       { "FarmProduce.Quest", "Quest: Now produce 100KG of beans on the FieldFarmSmall. Click on the 'Stat' tab and will show you the production of the last 5 years"},
       { "Transport.Quest", "Quest: Transport the beans from the farm to the Storage. To do that make sure you have" + 
            " workers on the masonry. They act as wheelbarrowers when not building"},
       { "Export.Quest", "Quest: Now export 300KG of beans. On the Dock add a order of export. If you have not dock then build one."+
           "Find it in Trade. Once built click on the orders tab and add a export order with the '+' sign."+
           " Select product and enter amount"},
       { "HireDocker.Quest", "Quest: Hire four dockers. The dockers will move the goods into the Dock from the Storage if you are exporting."+
           " Or viceversa if importing"},
       { "MakeBucks.Quest", "Quest: Make $100 exporting goods in the Dock. "+
           "Once a ship arrives will randomly pay product(s) in your Dock's inventory"},
       { "HeavyLoad.Quest", "Quest: Build a HeavyLoad building. This are haulers that carry more weight. They will come handy when transporting goods around is needed"},
       { "ImportOil.Quest", "Quest: Import 500KG of Whale Oil at the Dock. This is needed to keep your lights on at night."},




       {"CompleteQuest", "Your reward is ${0}"},


       //
       {"Tutorial.Arrow", "This is the tutorial. Once finished you will win $10,000"},
       {"Quest.Arrow", "This is the quest panel. You can access the quest by clicking on it"},
       {"New.Quest.Avail", "At least one quest is available"},
       {"Quest_Button.HoverSmall", "Quest"},






       //Bulleting helps
       {"Help.Bulletin/Prod/Spec", "In this window you can see the inputs needed for each product, where is built and the price. "
       + " Scroll to the top to see the headers. Notice that one simple product may have more than a formula to produce."},

              {"Help.Bulletin/General/Workers", "In this window you can assign workers to work in various buildings. "
       + " For a building allow more people in to work, must be less than capacity and must find at least an unemployed person."},

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
	   { "Road.Desc","Solo para propositos de decoracion. De todas maneras las personas se sienten mas felices si la via esta pavimentada alrededor de ellos"},
	   { "BridgeTrail.Desc","Por aqui las personas pasan de un lado del mapa a otro"},
	   { "BridgeRoad.Desc","Por aqui las personas pasan de un lado del mapa a otro. Los ciudadanos adoran estos puentes. " +
	                       "Les da un sentido de prosperidad y felicidad" +_houseTailES},
	   { "LightHouse.Desc","Ayuda a que el puedo sea descubierto mas facil. Añade a la Reputacion del Puerto siempre y cuando la llama este encendida"},
	   { H.Masonry + ".Desc","Una construccion muy imporatante. Estos trabajadores son los que construyen y cuando no tienen nada que hacer se dedican a transportar bienes"},
	   { H.HeavyLoad + ".Desc","Una construccion muy imporatante. Estos trabajadores son los que construyen y cuando no tienen nada que hacer se dedican a transportar bienes"},

       //House
	   { "Bohio.Desc","El Bohio, una casa con condiciones muy rudimentarias, los ciudadanos se abochornan de vivir aqui, una familia puede tener el maximo de 1 niño aqui" +_houseTail},
	   { "BohioB.Desc","El Bohio, una casa con condiciones muy rudimentarias, los ciudadanos se abochornan de vivir aqui, una familia puede tener el maximo de 1 niño aqui" +_houseTail},
	   { "HouseA.Desc","Casa pequeña de madera, una familia puede tener el maximo de 2 niños aqui" +_houseTailES},
	   { "HouseB.Desc","Small house, una familia puede tener el maximo de 2 niños aqui" +_houseTailES },
	   { "HouseTwoFloor.Desc","Wooden Medium house, una familia puede tener el maximo de 3 niños aqui"+_houseTailES},
	   { "HouseMed.Desc","Medium house, una familia puede tener el maximo de 2 a 3 niños aqui"+_houseTailES},
	   { "BrickHouseA.Desc","Large house, una familia puede tener el maximo de 3 a 4 niños aqui"+_houseTailES},
	   { "BrickHouseB.Desc","Large house, una familia puede tener el maximo de 3 a 4 niños aqui"+_houseTailES},
	   { "BrickHouseC.Desc","Large house, una familia puede tener el maximo de 4 niños aqui"+_houseTailES},
       
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
       { "Mortar.Desc","Here a worker will produce Mortar"},
	   { "Clay.Desc","Aqui se produce barro, necesaria para construir ladrillos y otros productos mas"},
	   { "Pottery.Desc","Aqui se producen productos de ceramica como platos, jarras, etc"},
	   { "Mine.Desc","Esta es una mina"},
	   { "MountainMine.Desc","Esta es una mina"},
	   { "Resin.Desc","La Resina de saca de los arboles aqui"},
	   {  H.LumberMill +".Desc","Aqui los trabajadores buscan y extraen recursos naturales como madera, piedra y minerales"},
	   { "BlackSmith.Desc","Aqui el trabajador produce elementos de la forja "+_asLongHasInputES},
	   { "ShoreMine.Desc","Aqui se produce la sal, arena o cal"},

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
	   { "Paper_Mill.Desc",_produceES},
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
	   { "Shack.Desc","Shack ESP"},

       //Militar
	   { "PostGuard.Desc",_militarES},
	   { "Fort.Desc",_militarES},
	   { "Morro.Desc",_militarES+". Una vez construida esta construccion los piratas se aconsejaran mejor"},




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



       //MainMenu

        { "Types_Explain", "Tradicional: \nEn este juego algunas construcciones estan  " +
                           "bloqueadas al principio y tienes que desbloquearlas. " +
            "Lo bueno es que asi tienes alguna manera de guiarte." +
            "\n\nFreewill: \nTodas las construcciones estan disponibles. " +
            "Lo malo es que puedes perder el juego mas facilmente."},


       //Tooltips 
       //Small Tooltips 
	   { "Person.HoverSmall", "Personas / Adultos / Niños"},
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

       { "More.HoverSmall", "More"},
	   { "Less.HoverSmall", "Less"},

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
	   
       //building window
       { "Gen_Btn.HoverSmall", "General"},
       { "Inv_Btn.HoverSmall", "Inventorio"},
       { "Upg_Btn.HoverSmall", "Mejoras"},
       { "Prd_Btn.HoverSmall", "Produccion"},
       { "Sta_Btn.HoverSmall", "Stadisticas"},
       { "Ord_Btn.HoverSmall", "Ordenes"},
	   { "Stop_Production.HoverSmall", "Pare produccion"},
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
       { "Utensil", "Utensillo"},
       { "Stop", "Pare"},




       
       //Main Menu
       { "Resume Game", "Sigue el Juego"},
       { "Continue Game", "Continua Juego"},
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
        return new List<string>(){"English", "Spanish"};
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
