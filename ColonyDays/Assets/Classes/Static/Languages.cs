using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Languages
{
    private static string _currentLang = "EN";


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
	   { "new","va"},
	   { "new1","va"},

       //Descriptions
       //Infr
	   { "BridgeTrail.Desc","Allows people pass from one side of the map to the other"},
	   { "BridgeRoad.Desc","Allows people pass from one side of the map to the other. People loves this bridges. Give them a prosperity and happinnes sense" +_houseTail},
	   { "LightHouse.Desc","Helps to make the port more discoverable. Adds to Port Reputation as long the flame is glowing"},
	   { "BuildersOffice.Desc","Really important building. This workers construct new buildings and work as wheelbarrowers once have nothing to do"},


       //House
	   { "HouseA.Desc","Small house, a family can have 2 kids maximum" +_houseTail},
	   { "HouseB.Desc","Small house, a family can have 2 kids maximum" +_houseTail },
	   { "HouseAWithTwoFloor.Desc","Medium house, a family can have 3 kids maximum"+_houseTail},
	   { "HouseMedA.Desc","Medium house, a family can have 3 kids maximum"+_houseTail},
	   { "HouseMedB.Desc","Medium house, a family can have 3 kids maximum"+_houseTail},
	   { "HouseC.Desc","Medium house, a family can have 3 kids maximum"+_houseTail},
	   { "HouseD.Desc","Medium house, a family can have 3 kids maximum"+_houseTail},
       
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
	   { "Ceramic.Desc","In this building a worker will produce Ceramic products, such as dishes, jars, etc"},
	   { "FishSmall.Desc","In this building a worker will catch fishes on a waterbody"},
	   { "FishRegular.Desc","In this building a worker will catch fishes on a waterbody"},
	   { "Mine.Desc","In this building a worker will catch fishes on a waterbody"},
	   { "MountainMine.Desc","In this building a worker will mine the mine and will produce always a random output"},
	   { "Resin.Desc","In this building a worker will mine the mine and will produce always a random output"},
	   { "Wood.Desc","In this building workers will find and mine land resources such as wood, stone, and ore"},
	   { "BlackSmith.Desc","In this building workers will produce the selected product"+_asLongHasInput},
	   { "SaltMine.Desc","In this building workers will produce salt"},

       //Prod
	   { "Brick.Desc","In this building a worker will produce Ceramic products, such as dishes, jars, etc"},
	   { "Carpintery.Desc","In this building a worker will produce Ceramic products, such as dishes, jars, etc"},
	   { "Cigars.Desc","In this building workers will produce cigars"+_asLongHasInput},
	   { "Mill.Desc","In this building workers will produce cigars"+_asLongHasInput},
	   { "Slat.Desc","In this building workers will produce cigars"+_asLongHasInput},
	   { "Tilery.Desc","In this building workers will produce roof tiles"+_asLongHasInput},
	   { "CannonParts.Desc","In this building workers will produce cannon parts"+_asLongHasInput},
	   { "Rum.Desc",_produce},
	   { "Chocolate.Desc",_produce},
	   { "Ink.Desc",_produce},

       //Ind
	   { "Cloth.Desc",_produce},
	   { "GunPowder.Desc",_produce},
	   { "Paper.Desc",_produce},
	   { "Printer.Desc",_produce},
	   { "CoinStamp.Desc",_produce},
	   { "Silk.Desc",_produce},
	   { "SugarMill.Desc",_produce},
	   { "Foundry.Desc",_produce},
	   { "SteelFoundry.Desc",_produce},

       //trade
	   { "Dock.Desc","In this building you can add import or export orders"},
	   { "DryDock.Desc","In this building you can repair ships, must have ship repair materials on the inventory to be effective"},
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


       //Militar
	   { "PostGuard.Desc",_militar},
	   { "Fort.Desc",_militar},
	   { "Morro.Desc",_militar+". Once you build this Pirates should know better"},


	};



    public static string ReturnString(string key)
    {
        if (_currentLang=="EN")
        {
            return _english[key];
        }

        return "not languages selected ";
    }



}
