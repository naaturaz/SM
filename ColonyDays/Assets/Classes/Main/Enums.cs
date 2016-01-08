using UnityEngine;
using System.Collections;

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
    SaltMiner
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
    Bean, Potato, SugarCane, Corn, Tobacco, Cotton,
    Banana, Coconut,
    //Animal Farm
    Chicken, Egg, Pork, Beef, Leather,
    //Clay
    Clay,
    //Fishes
    Fish,
    //Mines
    Gold, Stone, Iron,
    //Resin,
    Resin,
    //Wood,
    Wood,
    //BlackSmith,
    Axe, Tool, Sword,

    //Brick, 
    Brick,
    //Carpintery,
    Tonel,
    //Cigars,
    Cigar,
    //Slat,
    Slat,
    //Tilery,
    Tile,
    //Cloth,
    Fabric,
    //GunPowder,
    GunPowder,
    //Paper,
    Paper,
    //PrinterSmall, PrinterBig,
    PaperNews, Books,
    //Silk, 
    Silk, //decide if include or not in the game 
    //SugarMill,
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
    /// stats should be added on Book.cs. thts all 
    /// add the type of Product produces on Production.cs
    /// if Category will be diff than structure set on General.DefineCategory()
    /// 
    /// if is a Double bound structure such as Dock or MountainMine needs to be added on 
    /// List: doubleBounds on Building.cs
    //Structures Categores
    Infrastructure, Housing, Farming, Raw, Production, Industry, Trade, GovServices, Other,

    //infr F1
    Trail, Road, BridgeTrail, BridgeRoad, CoachMan, LightHouse, WheelBarrow, StockPile, BuildersOffice,
    //house F2 ... bugg bz they had numbers on it .. They cant have numbers
    HouseA, HouseB, HouseAWithTwoFloor, HouseMedA, HouseMedB, HouseC, HouseD,
    //farming F3
    //Farm, this farm removed is the draggable farm 
    AnimalFarmSmall, AnimalFarmMed, AnimalFarmLarge, AnimalFarmXLarge,
    FieldFarmSmall, FieldFarmMed, FieldFarmLarge, FieldFarmXLarge,
    //raw F4
    Clay, FishSmall, FishRegular, Mine, MountainMine, Resin, Wood, BlackSmith, SaltMine,
    //prod F5
    Brick, Carpintery, Cigars, Mill, Slat, Tilery,
    //industry F6
    Cloth, GunPowder, Paper, PrinterSmall, PrinterBig, Silk, SugarMill,
    //Trade F7
    Dock, DryDock, StorageSmall, StorageMed, StorageBig, StorageBigTwoDoors, StorageExtraBig,
    //gov F8
    Clinic, CommerceChamber, Customs, Library, School, TradesSchool, TownHouse,
    //other F9
    //Religous
    Church,
    //Old
    Tavern, Shack,





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






}

//if new categorY IS ADDED PLS ADD ON BOOK.CS
public enum StCat //strucutre categories
{
    Infrastructure, Housing, Farming, Raw, Production, Industry, Trade, GovServices, Other,
}


public enum StInfr //for structures game structure.cs 
{
    //Infrastructure
    Trail, Road, BridgeTrail, BridgeRoad,
    CoachMan, LightHouse, WheelBarrow, StockPile, BuildersOffice
}

public enum StHous //for structures game structure.cs 
{
    //Housing
    HouseA, HouseB, HouseAWithTwoFloor, HouseMedA, HouseMedB, HouseC,
    HouseD
}

public enum StFarm
{
    //Farm,
    AnimalFarmSmall, AnimalFarmMed, AnimalFarmLarge, AnimalFarmXLarge,
    FieldFarmSmall, FieldFarmMed, FieldFarmLarge, FieldFarmXLarge,
}


public enum StRaw //for structures game structure.cs 
{
    //Raw
    Clay, FishSmall, FishRegular, Mine, MountainMine, Resin, Wood, BlackSmith, SaltMine
}

public enum StProd //for structures game structure.cs 
{
    //Production
    Brick,Carpintery,Cigars,Mill,Slat,Tilery,
}

public enum StInd
{ Cloth, GunPowder, Paper, PrinterSmall, PrinterBig, Silk, SugarMill, }

public enum StTrade //for structures game structure.cs 
{
    //Trade
    Dock, DryDock, StorageSmall, StorageMed, StorageBig, StorageBigTwoDoors, StorageExtraBig,
}



public enum StGov //for structures game structure.cs 
{
    //Gov Services
    Clinic,CommerceChamber,Customs,Library,School,TradesSchool,TownHouse,
}

public enum StOther //for structures game structure.cs 
{
    //Religous
    Church,
    //Old
    Tavern, Shack
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


public static class Enums { }