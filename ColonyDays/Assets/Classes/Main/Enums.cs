﻿using UnityEngine;
using System.Collections;
using System;

public enum Job
{
    Forester,
    Insider,
    Docker,
    Builder,
    FisherMan,
    ShackBuilder,
    None,
    WheelBarrow,
    Homer,
    Farmer,
    SaltMiner,
}

public enum TaskE //to be use for the person class
{ Homing, Working, GettingFood, Idlying }

public enum Doing //to be use for the person class
{
    resting, walking, working, pickingUp, carring,
    droppingOff, running, guideAnimal, ridingVehicle
}

public enum HPers //to be use for the person class
{
    None,
    Male, Female,
    NewBorn, Kid, Adult,
    Emigrant,
    Home, Work, FoodSource, Religion, Chill, OldHome,
    Homing, Working, GettingFood, Idlying, IdleSpot, Walking, Praying, Chilling,
    NewHome,
    MovingToNewHome,
    NowToNewHome,


    Restarting,
    IdleInHome,
    CheckingStuff,
    Way,
    InWork,
    WalkingToJobSite, WalkingBackToOffice,
    Done,
    InWorkBack,
    // WorkDrop
    WorkingInPlaceNow,
    AniFullyTrans,
    DoneAtFoodScr,
    DoneAtWork,
    GoingToShack,
    WheelBarrow,
    DoneAtWheelBarrow,
    DoneAtHome,
    Enemy,
    DockerSupply,
    DockerBackToDock,
}

public enum EducationLevel
{
    None, Primary, Secundary, Trades
}

public enum Ani//animtatios
{
    isCarry, isIdle, isWalk, isSummon
}

public enum Role
{ Father, Mother, Kid, None }

/// <summary>
/// For production, the products can be produce in buildings  
/// 
/// //inventory items... invetory for people and houses, and business
/// </summary>
public enum P
{
    //Farm
    Bean, Potato, SugarCane, Corn, Cotton,
    Banana, Coconut,
    //Animal Farm
    Chicken, Egg, Pork, Beef, Leather,
    Clay, PalmLeaf,
    Crockery,
    //Fishes
    Fish,
    //Mines
    Gold, Stone, Iron,
    //Resin,
    //Wood,
    Wood,
    //BlackSmith,
    //Sword, Axe, 
    Tool,

    //Brick, 
    Brick,
    //Carpintery,
    Barrel,
    //Cigars,
    Cigar,
    //Slat,
    //Slat,
    //Tilery,
    FloorTile,
    //Cloth,
    Fabric,
    //GunPowder,
    GunPowder,
    //Paper,
    Paper,
    //PrinterSmall, PrinterBig,
    Map, Book,
    //Silk, 
    //Silk, //decide if include or not in the game 
    SugarMill,
    Sugar,
    None,

    //other not defined yet
    Person, Food, Dollar,
    Salt,
    Coal,
    Sulfur,
    Potassium,
    Silver,
    Henequen,
    //HolyWood,
    Sail,
    String,
    Nail,
    CannonBall,
    TobaccoLeaf,
    CoffeeBean,
    Cacao,
    Chocolate,
    Weapon,
    WheelBarrow,

    Diamond,
    Jewel,
    Cloth,
    Rum,
    Wine,
    Ore,
    Crate,
    Coin
    ,
    //CrystalCoin, CaribbeanCoin, SugarCoin,
    CannonPart,
    Ink,
    Steel,
    RandomMineOutput, RandomFoundryOutput,

    CornFlower,
    Bread,
    Carrot,
    Tomato,
    Cucumber,
    Cabbage,
    Lettuce,
    SweetPotato,
    Cassava,
    Pineapple,
    //Mango,
    //Avocado,
    //Guava,
    //Orange,
    Papaya,
    Wool,
    Shoe,
    CigarBox,
    Water,
    Beer,
    Honey,
    Bucket,
    Cart,
    RoofTile,
    Utensil,
    Stop,
    Year,
    CornMeal,
    Horse,
    Furniture,
    Mortar,
    QuickLime,
    Sand,
    Machinery,
    Product,
    Input1,
    Input2,
    Input3,
    WhaleOil,
    Candy,
    Rubber,
}

/// <summary>
/// Products categories
/// </summary>
public enum PCat
{
    None, Food
}


//This enum is H for helper so we dont have to type anything eventually all strings around should be here
//shhort strings
public enum H
{
    None, Done, Cancel, Selection, Male, Female,
    Sound, Music, Easy, Normal, Hard, Load, Save, Enable, Disable, Spawn, Kill, Tips, SpawnPoint,
    NoSelection, Player, CamFollow, CamRTS, X, Y, Z, Poly, Mesh, Full, Tile, Create, Update,
    Stone, Iron, Tree, Ornament, Grass, RemoveSelection, Horiz, Vertic, PlanesVertic, PlanesHor, TerraSpawn,
    Geometry, Stage1, Stage2, Stage3, Main, s2, s3, Stages, Smoke, MaritimeBound, TerraBound, TerraUnderBound,
    Next_Stage, Same_Length_Both_Sides,
    Canvas, Panel, Add, Remove,

    Bridge, BridgeRoadUnit, BridgeTrailUnit, Wheel, Storage,


    //ADDING A NEW STRUCTURE INSTRUCTIONS:
    /////////////////anything u change BELOW HERE SHOULD BE ADDED IN THEIR SPECIFIC ENUM USED TO MAP KEYBOARD
    /// and added on Root.cs and 
    /// stats should be added on Book.cs.
    /// and add Desc on Languages.cs
    /// add on UnlockBuilds.cs
    /// and underneath the proper region in this file
    /// and if produces anything u want to added in Production.cs
    /// Adjustment may be needed in the NavMesh, Building.cs 1410
    /// 
    /// more below for: Decorations, Production, Category, DoubleBound
    /// 
    /// if is a decoration needs to be added as small item in Decorations.cs
    /// 
    /// add the type of Product produces on Production.cs
    /// if Category will be diff than structure set on General.DefineCategory()
    /// 
    /// if is a Double bound structure such as Dock or MountainMine needs to be added on 
    /// List: doubleBounds on Building.cs
    //Structures Categores
    Infrastructure, Housing, Farming, Raw, Production, Industry, Trade, GovServices, Other, Militar,

    //infr F1
    StandLamp,
    Trail, Road, BridgeTrail, BridgeRoad, CoachMan,
    LightHouse, WheelBarrow, StockPile, Masonry,
    HeavyLoad,

    //house F2 ... bugg bz they had numbers on it .. They cant have numbers
    //Bohio, BohioB,
    Shack,
    MediumShack,
    LargeShack,

    WoodHouseA, WoodHouseB, WoodHouseC,
    BrickHouseA,
    BrickHouseC,
    BrickHouseB,


    //farming F3
    //Farm, this farm removed is the draggable farm 
    AnimalFarmSmall, AnimalFarmMed, AnimalFarmLarge, AnimalFarmXLarge,
    FieldFarmSmall, FieldFarmMed,
    FieldFarmLarge, FieldFarmXLarge, //blockin the game, when they are selected 
    FishingHut,
    //raw F4
    LumberMill, Clay, ShoreMine,
    MountainMine,
    BlackSmith, QuickLime,
    Mortar, Pottery,
    //prod F5
    Brick, Carpentry, Cigars, Mill, Tailor,
    Armory, Distillery, Chocolate, Ink,
    //industry F6
    Cloth, GunPowder, PaperMill, Printer, CoinStamp, SugarMill, Foundry, SugarShop,
    //Trade F7
    Dock, Shipyard, Supplier, StorageSmall, StorageMed, StorageBig, StorageBigTwoDoors, StorageExtraBig,
    //gov F8
    //Clinic, CommerceChamber, Customs, 
    Library, School, TradesSchool,
    TownHouse,
    //other F9
    //Religous
    Church,
    //Old
    Tavern,

    //Militar
    WoodPost, PostGuard, Fort, Morro, 

    //Decorations 
    Fountain, WideFountain, PalmTree,
    FloorFountain, FlowerPot, PradoLion,


    WillBeDestroy,
    Initial,
    FrontCollider,
    RearCollider,
    Descending,
    Ascending,

    Bottom01, Bottom02, Bottom03, Top01, Top02, Top03,
    AtoA1,
    BtoA1,
    AtoB1,
    BtoB1,
    Unit,
    Piece,
    Bridge_Trail_Piece_12,
    InBuildIniPoint,
    InBuildMidPointA,
    InBuildWorkPoint01,
    InBuildWorkPoint02,
    InBuildWorkPoint03,
    Kids,
    Dummy,
    MovingToNewHome,
    Evacuation,
    AnimalFarm,
    Small,
    Med,
    Large,
    XLarge,
    FarmZone,
    Slot1,
    Slot2,
    Slot3,
    Slot4,
    Slot5,
    Slot6,
    Slot7,
    Slot8,
    Slot9,
    Slot10,
    Farm,
    Title,
    Cost,
    Description,
    Person,
    Info,
    Inv,
    Gen_Btn,
    Inv_Btn,
    Bolsa,
    Next_Stage_Btn,
    Ord_Btn,
    General,
    Gaveta,
    Orders,
    Add_Btn,
    Cancel_Btn,

    Once, OnceAYear, Every5Year,
    New_Order_Lbl_Prod,
    New_Order_Lbl_Amt,
    New_Order_Lbl_Frec,
    Output_Lbl_Prod,
    Output_Lbl_Amt,
    Output_Lbl_Frec,
    Input_Amt,
    Input_Price,
    Output_Lbl_Price,
    PriceGroup,
    IniPos_Import,
    IniPos_Export,
    Remove_Order_Btn,
    Upgrades,
    Upg_Btn,
    Upg_Mat_Btn,
    Upg_Cap_Btn,


    Obstacle, MountainObstacle, WaterObstacle,

    Way3,
    Way2,
    Way1,
    Gold,
    Enviroment,
    LinkRect,
    LandZone,
    Poll,
    Bottom,
    Top,
    Height,
    Width,
    RectCorner,
    Door,
    IniPos_Import_OnProcess,
    IniPos_Export_OnProcess,
    Output_Error_Msg,
    Products,
    IniPos,
    Display_Lbl,
    Prd_Btn,
    Prd_Btns_Pos,
    Btn,
    LockDown,
    Demolish_Btn,
    Cancel_Demolish_Btn,
    ShipSmall,
    Text,
    Icon,
    Inv_Ini_Pos,
    Resources,
    Start,
    Bridge_Trail_Piece_10,
    LOD0,
    LOD1,
    LOD2,
    Animal,
    Decoration,
    OverWrite,
    DeleteDialog,
    NotHDDSpace,
    GameOverPirate,
    GameOverMoney,
    Unlock,
    Lock,
    Coming_Soon,
    Max_Cap_Reach,
    BuyRegion,
    Feedback,
    BugReport,
    OnlyForDev,
    Invitation,
    Marine,
    Mountain,
    OnATrip,
    AtFarm,
    Home,
    YearReport,
    Plant,
    TutoOver,
    Negative,
    OptionalFeedback,
    InfoKey,
    CompleteQuest,
    MandatoryFeedback,
    Building,
    Day,
    Night,
    Enemy,
    War,
    BullDozer,
    Defender,
    PathToSeaExplain,
    Show,
}

//if new categorY IS ADDED PLS ADD ON BOOK.CS
public enum StCat //strucutre categories
{
    Infrastructure, Housing, Farming, Raw, Production, Industry, Trade, GovServices, Other, Militar, Decoration
}


public enum StInfr //for structures game structure.cs 
{
    //Infrastructure
    StandLamp,
    //Trail,
    //Road, 
    //BridgeTrail, BridgeRoad,
    //CoachMan, 
    LightHouse,
    //WheelBarrow, 
    //b4StockPile, 
    Masonry,
    HeavyLoad,
    
}

public enum StHous //for structures game structure.cs 
{
    //Housing
    //Bohio,
    Shack, MediumShack, LargeShack,
    WoodHouseA, WoodHouseB, WoodHouseC,
    BrickHouseA,
    BrickHouseC,
    BrickHouseB,
}

public enum StFarm
{
    //Farm,
    //AnimalFarmSmall, AnimalFarmMed, AnimalFarmLarge, AnimalFarmXLarge,
    FieldFarmSmall, FieldFarmMed, 
    //FieldFarmLarge, FieldFarmXLarge,
    FishingHut, 
}


public enum StRaw //for structures game structure.cs 
{
    LumberMill, Clay, ShoreMine,
    MountainMine,
    BlackSmith, QuickLime, 
    Mortar, Pottery,
}

public enum StProd //for structures game structure.cs 
{
    //Production
    Brick, Carpentry, Cigars, 
    //Mill,
    Tailor, 
    Armory, Distillery, Chocolate, Ink,
}

//Ind
public enum StInd
{   Cloth, GunPowder, PaperMill, Printer,
    //CoinStamp,
    SugarMill,
    Foundry, SugarShop}

public enum StTrade //for structures game structure.cs 
{
    //Trade
    Dock, //Shipyard, Supplier, 
    StorageSmall, StorageMed, StorageBig, 
    //StorageBigTwoDoors, StorageExtraBig,
}



public enum StGov //for structures game structure.cs 
{
    //Gov Services
    //Clinic,CommerceChamber,Customs,
    //Library,
    School,TradesSchool,
    //TownHouse,
}

public enum StOther //for structures game structure.cs 
{
    //Religous
    Church,
    //Old
    Tavern,
}

public enum StMil //for structures game structure.cs 
{
    //WoodPost,
    PostGuard,
    //Fort, Morro,
}

public enum StDec //for structures game structure.cs 
{
    Fountain, WideFountain, PalmTree,
    FloorFountain, FlowerPot, PradoLion,

}




public enum Ma //for materials
{
    matBuildBase, matBuildUpg1, matBuildUpg2

}
public enum Ca //category
{
    None, Way, Structure, DraggableSquare, Spawn, Shore
}

public enum Mode //for building
{
    None, Building, Placing, Cutting,
}

//for directions
public enum Dir
{
    None, Positive, Negative, Up, Down, Right, Left, Vertical, Horizontal, VerticHorizo,
    UpLeft, UpRight, DownLeft, DownRight,
    SWtoNE, SEtoNW, NWtoSE, NEtoSW, NW, NE, SE, SW, N, S, W, E
}

//This enum will hold all the buttons action so every action will be listed here, from simple sound on off action
//to spawn new menus
public enum Btn//for button action //this ones should not be spawners for now sep 25 2014
{
    None, SelectMale1, SelectMale2, PlayNewGame, BackToMain, OkPause, CancelPause,
    SettingsPause, ExitGame, SwitchSound, SwitchMusic, BackToPauseMenu
}

public enum F//for fades
{ FadeIn, FadeOut }

//for STATIC STRINGS this is  is created so we start removing all strings from the code
//this is for longer strings 2 words or more  
public enum S
{
    Confirm_Menu_Spawner,
    Settings_Pause_Menu_Spawner,
    RightClickedMenu,
    Visual_Helper,
    Up_Side_Elevator
}




public enum BtnsE
{
    Select_Cube_Btn_Raw_3dMenu,
    Select_Cylinder_Btn_Raw_3dMenu,
    Select_Sphere_Btn_Raw_3dMenu,
    Select_Pyramid_Btn_Raw_3dMenu,
    Select_Cone_Btn_Raw_3dMenu,

    Select_Bomb_Btn_Element_3dMenu,
    Select_Elevator_Btn_Element_3dMenu,
    Select_Mine_Btn_Element_3dMenu,
    Select_Spike_Btn_Element_3dMenu,
    Select_Spring_Btn_Element_3dMenu
}

public enum Month
{
    Jan, Feb, Mar, Apr, May, Jun,
    Jul, Aug, Sep, Oct, Nov, Dec,
    None
}


public enum Tile//for roads
{
    NW, N, NE, E, SE, S, SW, W, In,
    None,
    NSW, NSE, NS,
    NWE, SWE, WE,
    NSWE
}

public enum G//for General Random stuff
{
    Home, Sea,
}

/// <summary>
/// By Ronny and they share same material too
/// </summary>
public enum Ron1
{
    Brick, Wood, Carpintery, Clay, SaltMine, BlackSmith, FishHut, Masonry, LightHouse,
    Church, Tavern, 
    MountainMine, Mill, School, PostGuard
}

public enum Ron2
{
    Library, TradesSchool, 
    Dock, StorageSmall,
    BridgeRoad, 
    GunPowder, Armory, Tailor, Pottery,
    CoinStamp, Chocolate, Distillery, Foundry, Printer, Cigars, Ink,
    Cloth,


    
}


public enum Ron3
{
    PaperMill,
    Mortar,
    TownHouse,
    QuickLime,
    WoodPost,
    StorageSmall2,

}

public enum RonWoodHouse
{
    WoodHouseA, WoodHouseB, WoodHouseC,
}

public enum RonBrickHouse
{
    BrickHouseA, BrickHouseB, BrickHouseC,
}

public enum RonBohioHouse
{
    Bohio, BohioB
}

public static class Enums 
{


    /// <summary>
    /// All Enums pass here need to have a None defined as an element 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T ParseEnum<T>(string value)
    {
        try
        {
            return (T)Enum.Parse(typeof(T), value, true);

        }
        catch (Exception)
        {
            return (T)Enum.Parse(typeof(T), "None", true);
            //throw;
        }
    }

}