using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Languages
{
    private static string _currentLang = "English";


    private static  string _houseTail = ". SugarMiller's live here and enjoy having a nice meal at least once in a while";
    private static string _animalFarmTail = ", can raise different animals in this building";
    private static string _fieldFarmTail = ", can seed different crops and fruits in this building";
    private static string _asLongHasInput = ", as long it has the necessary inputs";
    private static string _produce = "In this building workers will produce the selected product, as long it has the necessary inputs";
    private static string _storage =
        "In this building you store all kind of items, if gets full people won't work if they dont have where to take their product";
    private static string _militar = "This building helps decreasing the Pirate Threath in your port, to be effective must have workers on it. The more the better";


    static Dictionary<string, string> _english = new Dictionary<string, string>()
	{
       //Descriptions
       //Infr
	   { "Road.Desc","Only for decoration purpose. However people is happier if there is roads around them"},
	   { "BridgeTrail.Desc","Allows people pass from one side of the terrain to the other"},
	   { "BridgeRoad.Desc","Allows people pass from one side of the terrain to the other. People loves this bridges. Give them a prosperity and happinnes sense" +_houseTail},
	   { "LightHouse.Desc","Helps to make the port more discoverable. Adds to Port Reputation as long the flame is glowing"},
	   { H.Masonry + ".Desc","Really important building. This workers construct new buildings and work as wheelbarrowers once have nothing to do"},
	   { H.HeavyLoad + ".Desc","Really important building. This workers construct new buildings and work as wheelbarrowers once have nothing to do"},

       //House
	   { "Bohio.Desc","Bohio house, really rudimentary conditions, people is unhappy living here, a family can have 1 kid maximum" +_houseTail},
	   { "HouseA.Desc","Wooden Small house, a family can have 2 kids maximum" +_houseTail},
	   { "HouseB.Desc","Small house, a family can have 2 kids maximum" +_houseTail },
	   { "HouseTwoFloor.Desc","Wooden Medium house, a family can have 3 kids maximum"+_houseTail},
	   { "HouseMed.Desc","Medium house, a family can have 2 or 3 kids maximum"+_houseTail},
	   { "HouseLargeA.Desc","Large house, a family can have 6 kids maximum"+_houseTail},
	   { "HouseLargeB.Desc","Large house, a family can have 6 kids maximum"+_houseTail},
	   { "HouseLargeC.Desc","Large house, a family can have 6 kids maximum"+_houseTail},
       
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

       //Raw
	   { "Clay.Desc","In this building a worker will produce Clay, raw material needed for bricks and more"},
	   { "Ceramic.Desc","In this building a worker will produce Ceramic products, such as dishes, jars, etc"},
	   { H.Fishing_Hut + ".Desc","In this building a worker will catch fishes on a waterbody"},
	   { "FishRegular.Desc","In this building a worker will catch fishes on a waterbody"},
	   { "Mine.Desc","In this building a worker will catch fishes on a waterbody"},
	   { "MountainMine.Desc","In this building a worker will mine the mine and will produce always a random output"},
	   { "Resin.Desc","In this building a worker will mine the mine and will produce always a random output"},
	   {  H.LumberMill +".Desc","In this building workers will find and mine land resources such as wood, stone, and ore"},
	   { "BlackSmith.Desc","In this building workers will produce the selected product"+_asLongHasInput},
	   { "SaltMine.Desc","In this building workers will produce salt"},

       //Prod
	   { "Brick.Desc","In this building a worker will produce Ceramic products, such as dishes, jars, etc"},
	   { "Carpintery.Desc","In this building a worker will produce Ceramic products, such as dishes, jars, etc"},
	   { "Cigars.Desc","In this building workers will produce cigars"+_asLongHasInput},
	   { "Mill.Desc","In this building workers will produce cigars"+_asLongHasInput},
	   { H.Tailor+".Desc","In this building workers will produce cigars"+_asLongHasInput},
	   { "Tilery.Desc","In this building workers will produce roof tiles"+_asLongHasInput},
	   { "CannonParts.Desc","In this building workers will produce cannon parts"+_asLongHasInput},
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
	   { "Dock.Desc","In this building you can add import or export orders"},
	   { H.Shipyard + ".Desc","In this building you can repair ships, must have ship repair materials on the inventory to be effective"},
	   { "Supplier.Desc","In this building you can supply ships, must have items on inventory that a ship can use for their long trip"},
	   { "StorageSmall.Desc",_storage},
	   { "StorageMed.Desc",_storage},
	   { "StorageBig.Desc",_storage},
	   { "StorageBigTwoDoors.Desc",_storage},
	   { "StorageExtraBig.Desc",_storage},

       //gov
	   { "Library.Desc","People come to this building to read and borrow books for their knowledge. The more books your libraries has the better"},
	   { "School.Desc","Here the SugarMillers get education. The more the better"},
	   { "TradesSchool.Desc","Here the SugarMillers get a specialized education on trades. The more the better"},
	   { "TownHouse.Desc","The townhouse gives happinnes and sense of prosperity to your people"},

       //other
	   { "Church.Desc","The church gives happinnes and hope to your people"},
	   { "Tavern.Desc","The tavern gives some relax and enjoy to your people"},
	   { "Shack.Desc","Shack"},

       //Militar
	   { "PostGuard.Desc",_militar},
	   { "Fort.Desc",_militar},
	   { "Morro.Desc",_militar+". Once you build this Pirates should know better"},




       //Main GUI
	   { "SaveGame.Dialog", "Save your game progress"},
	   { "LoadGame.Dialog", "Load a game"},
	   { "NameToSave", "Save your game as:"},
	   { "NameToLoad", "Game to load selected:"},
	   { "OverWrite", "There is a saved game with same name. Do you want to overwrite the file?"},
	   { "DeleteDialog", "Are you sure want to delete the saved game?"},
	   { "NotHDDSpace", "Not enough space on {0} drive to save the game"},
	   { "GameOverPirate", "Sorry, you lost the game! Pirates attack your town and killed everyone."},
	   { "GameOverMoney", "Sorry, you lost the game! The crown wont support your Caribbean island anymore."},
	   { "BuyRegion.WithMoney", "Are you sure want to buy this region."},
	   { "BuyRegion.WithOutMoney", "Sorry, you can't afford this now."},
	   { "Feedback", "Feedback!? Awesome...:) Thanks. 8) "},
	   { "BugReport", "Catched a bug? uhmm yummy.... Send it this way!! Thanks"},	  
       { "Invitation", "Your friend's email for a chance to join the Private Beta"},  
       { "Info", ""},//use for informational Dialogs

       //todo aadd to ESP
       //Tooltips 
       //Small Tooltips 
	   { "Person.HoverSmall", "People"},
	   { "Emigrate.HoverSmall", "Emigrated"},
	   { "Food.HoverSmall", "Food"},
	   { "Happy.HoverSmall", "Happiness"},
	   { "PortReputation.HoverSmall", "Port Reputation"},
	   { "Dollars.HoverSmall", "Dollars"},
	   { "PirateThreat.HoverSmall", "Pirate Threat"},
	   { "Date.HoverSmall", "Date"},
	   { "MoreSpeed.HoverSmall", "More Speed"},
	   { "LessSpeed.HoverSmall", "Less Speed"},
	   { "ShowNoti.HoverSmall", "Notifications"},
	   { "Menu.HoverSmall", "Main Menu"},
	   { "QuickSave.HoverSmall", "Quick Save"},
	   { "Bug Report.HoverSmall", "Report a bug"},
	   { "Feedback.HoverSmall", "Feedback"},

       //down bar
	   { "Infrastructure.HoverSmall", "Infrastructure"},
	   { "House.HoverSmall", "Housing"},
	   { "Farming.HoverSmall", "Farming"},
	   { "Raw.HoverSmall", "Raw"},
	   { "Prod.HoverSmall", "Production"},
	   { "Ind.HoverSmall", "Industry"},
	   { "Trade.HoverSmall", "Trade"},
	   { "Gov.HoverSmall", "Goverment"},
	   { "Other.HoverSmall", "Other"},
	   { "Militar.HoverSmall", "Militar"},
	   { "WhereIsTown.HoverSmall", "Back to town [P]"},
	   { "WhereIsSea.HoverSmall", "Show/hide path to sea"},
	   { "Helper.HoverSmall", "Mini help"},
	   
       //building window
       { "Gen_Btn.HoverSmall", "General Tab"},
       { "Inv_Btn.HoverSmall", "Inventory Tab"},
       { "Upg_Btn.HoverSmall", "Upgrades Tab"},
       { "Prd_Btn.HoverSmall", "Production Tab"},
       { "Ord_Btn.HoverSmall", "Orders Tab"},
	   { "Stop_Production.HoverSmall", "Stop Production"},
	   { "Demolish_Btn.HoverSmall", "Demolish"},
	   { "More Salary.HoverSmall", "Pay more"},
	   { "Less Salary.HoverSmall", "Pay less"},
	   { "Next_Stage_Btn.HoverSmall", "Buy Next Stage"},
	   { "Current_Salary.HoverSmall", "Current salary"},
	   { "Add_Import_Btn.HoverSmall", "Add an import"},
	   { "Add_Export_Btn.HoverSmall", "Add an export"},
	   { "Upg_Cap_Btn.HoverSmall", "Upgrades capacity"},
	   { "Close_Btn.HoverSmall", "Close"},

       //addOrder windiw
	   { "Amt_Tip.HoverSmall", "Amount of prod"},

       //Med Tooltips 
	   { "Build.HoverMed", "Place building: 'Left click' \n" +
	                       "Rotate building: 'R' key \n " +
	                       "Cancel: 'Right click'"},
	   { "Current_Salary.HoverMed", "Workers will work in the place that pays the highest salary." +
	                                " If 2 places pay the same salary, then the closest to home will be the one" +
	                                " they will choose."},


       //Notifications
	   { "BabyBorn.Noti.Name", "New Born"},
	   { "BabyBorn.Noti.Desc", "A new baby was born"},
	   { "PirateUp.Noti.Name", "Pirates Closer"},
	   { "PirateUp.Noti.Desc", "Pirates close to the shore"},
       { "PirateDown.Noti.Name", "Pirates Respect You"},
	   { "PirateDown.Noti.Desc", "Pirates respect you a bit more today"},

       { "Emigrate.Noti.Name", "A person emigrated"},
	   { "Emigrate.Noti.Desc", "People emigrate when not happy in your goverment"},
       { "PortUp.Noti.Name", "Port is known"},
	   { "PortUp.Noti.Desc", "Your port is getting more reputation in the area routes"},
       { "PortDown.Noti.Name", "Port is less known"},
	   { "PortDown.Noti.Desc", "Your port reputation went down"},

       { "BoughtLand.Noti.Name", "New Land Purchased"},
	   { "BoughtLand.Noti.Desc", "A new land region was purchased"},

       { "ShipPayed.Noti.Name", "Ship payed"},
	   { "ShipPayed.Noti.Desc", "A ship has payed for goods or service"}, 
       { "ShipArrived.Noti.Name", "Ship arrived"},
	   { "ShipArrived.Noti.Desc", "A new ship has arrived to one of our maritimes buildings"},

       //todo keep aadding to ESP
       //Main notificaion
       { "NotScaledOnFloor", "The building is too close to shore or to a mountain"},
       { "NotEven", "The ground underneath the building is not even"},
       { "Colliding", "Building is colliding with another one"},
       { "BadWaterHeight", "The building is too low or high on the water"},
       { "LockedRegion", "You need to own this region to build in here"},

       //Mini help
       { "Camera", "Camera: Use [AWSD] or cursor to move. " +
                   "Press mouse middle button and keep pressed to rotate"},
       { "SeaPath", "Click on the left botton corner 'Show/hide sea path' " +
                    "button to show the closest path to the sea"},
       { "Region", "Region: You need to own a region to be able to build on it. Click on the For Sale sign in the" +
                   " middle of each region to buy it"},
       { "PeopleRange", "The circle around each building marks the range of it"},

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
	   { "HouseA.Desc","Casa pequeña de madera, una familia puede tener el maximo de 2 niños aqui" +_houseTailES},
	   { "HouseB.Desc","Small house, una familia puede tener el maximo de 2 niños aqui" +_houseTailES },
	   { "HouseTwoFloor.Desc","Wooden Medium house, una familia puede tener el maximo de 3 niños aqui"+_houseTailES},
	   { "HouseMed.Desc","Medium house, una familia puede tener el maximo de 2 a 3 niños aqui"+_houseTailES},
	   { "HouseLargeA.Desc","Large house, una familia puede tener el maximo de 3 a 4 niños aqui"+_houseTailES},
	   { "HouseLargeB.Desc","Large house, una familia puede tener el maximo de 3 a 4 niños aqui"+_houseTailES},
	   { "HouseLargeC.Desc","Large house, una familia puede tener el maximo de 4 niños aqui"+_houseTailES},
       
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

       //Raw
	   { "Clay.Desc","Aqui se produce barro, necesaria para construir ladrillos y otros productos mas"},

	   { "Ceramic.Desc","Aqui se producen productos de ceramica como platos, jarras, etc"},
	   { H.Fishing_Hut + ".Desc","Aqui se pescan peces"},
	   { "FishRegular.Desc","Aqui se pescan peces"},
	   { "Mine.Desc","Esta es una mina"},
	   { "MountainMine.Desc","Esta es una mina"},
	   { "Resin.Desc","La Resina de saca de los arboles aqui"},
	   {  H.LumberMill +".Desc","Aqui los trabajadores buscan y extraen recursos naturales como madera, piedra y minerales"},
	   { "BlackSmith.Desc","Aqui el trabajador produce elementos de la forja "+_asLongHasInputES},
	   { "SaltMine.Desc","Aqui se produce la sal"},

       //Prod
	   { "Brick.Desc",_produceES},
	   { "Carpintery.Desc",_produceES},
	   { "Cigars.Desc",_produceES},
	   { "Mill.Desc",_produceES},
	   { H.Tailor+".Desc",_produceES},
	   { "Tilery.Desc",_produceES},
	   { "CannonParts.Desc",_produceES},
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


       //words
       //Fiel Farms
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
        else if (_currentLang == "Español")
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
    }

    internal static string CurrentLang()
    {
        return _currentLang;
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
