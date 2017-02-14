/* Here is all the variables needed to recreate a person object
*
*/

using UnityEngine;

public class PersonFile  {

    public int _age;

    public bool IsMajor;

    //Birthday
    public int LastBDYear;//the year I had the last birthday. So i dont have more thn 1 BD a Year 
    public int BirthMonth;//the birthday of the person
    public  int UnHappyYears;//if is over X amount will emmigrate

    public int _lifeLimit;
    public string _name;
    public EducationLevel _educationLevel;
    public double _happinnes, _prosperity;
    public H _gender;

    //only for females
    public bool _isPregnant;

    public int LastNewBornYear = -10;//the last pregnancy she had, when was the birth year 
    //once pregnant will tell u the due date 
    public int DueMonth;
    public int DueYear;


    public string _nutritionLevel;

    //Places
    public string _home;
    public string _work;
    public string _foodSource;
    public string _religion;
    public string _chill;

    //Person Own Objects and fields
    public Brain _brain;
    public Body _body;
    public string _spouse = "";
    public bool _isWidow;//if is wont get married again

    public string MyId; 

    //tehc stuff
    public Vector3 Position;
    public Quaternion Rotation;

    public Inventory Inventory;

    public Profession ProfessionProp;

    public bool IsStudent;
    public string Father;
    public string Mother;

    public string IsBooked;
    public string HomerFoodSrc;

    //WheelBarrower
    public Order Order;//the order of a wheelBarrower

    public Order PrevOrder;


    public string SourceBuildKey;//from where taking the load 
    public string DestinyBuildKey;//where taking load 

    public string FamilyId;

    public int YearsOfSchool { get; set; }
    public string NutritionLevel { get; set; }

    public string StartingBuild;

    public PersonFile(Person pers)
    {
        _age = pers.Age;

        IsMajor = pers.IsMajor;

        //Birthday
        LastBDYear = pers.LastBdYear;//the year I had the last birthday. So i dont have more thn 1 BD a Year 
        BirthMonth = pers.BirthMonth;//the birthday of the person
        UnHappyYears = pers.UnHappyYears;



        _lifeLimit = pers.LifeLimit;
        _name = pers.Name;
        _educationLevel = pers.EducationLevel;
        _happinnes = pers.Happinnes;
        _prosperity = pers.Prosperity;
        _gender = pers.Gender;



        LastNewBornYear = pers.LastNewBornYear;//the last pregnancy she had, when was the birth year 
        //once pregnant will tell u the due date 
        DueMonth = pers.DueMonth;
        DueYear = pers.DueYear;



        _isPregnant = pers.IsPregnant;
        _nutritionLevel = pers.NutritionLevel;
       
        
        SavePersonStructs(pers);
        
        _brain = pers.Brain;
        _body = pers.Body;
        _spouse= pers.Spouse;
        _isWidow = pers.IsWidow;
        MyId = pers.MyId;
        Position = pers.transform.position;
        Rotation = pers.transform.rotation;
        Inventory = pers.Inventory;

        SaveProfesion(pers);
        IsStudent = pers.IsStudent;
        Father = pers.Father;
        Mother = pers.Mother;

        IsBooked = pers.IsBooked;
        //HomerFoodSrc = pers.HomerFoodSrc;

        Order = pers.ProfessionProp.Order1;
        PrevOrder = pers.PrevOrder;

        DestinyBuildKey = pers.ProfessionProp.DestinyBuildKey;
        SourceBuildKey = pers.ProfessionProp.SourceBuildKey;

        FamilyId = pers.FamilyId;
        YearsOfSchool = pers.YearsOfSchool;
        SavedJob = pers.SavedJob;
        PrevJob = pers.PrevJob;

        NutritionLevel = pers.NutritionLevel;
        StartingBuild = pers.StartingBuild;

        Weight = pers.Weight;
        Height = pers.Height;
        Nutrition1 = pers.Nutrition1;
        WasFired = pers.WasFired;
        PersonBank = pers.PersonBank1;
    }

    void SavePersonStructs(Person pers)
    {
        if (pers.Home != null)
        {
            _home = pers.Home.MyId;
        }

        if (pers.Work != null)
        {
            _work = pers.Work.MyId;
        }
        if (pers.FoodSource != null)
        {
            _foodSource = pers.FoodSource.MyId;
        }
        if (pers.Religion != null)
        {
            _religion = pers.Religion.MyId;
        }
        if (pers.Chill != null)
        {
            _chill = pers.Chill.MyId;
        }
    }

    /// <summary>
    /// Created to save Profesion. Since cant save a derived class will create a prof instance and will 
    /// copy all attribute 
    /// </summary>
    void SaveProfesion(Person personToSave)
    {
        ProfessionProp = new Profession(personToSave.ProfessionProp);
    }

    public PersonFile() { }




    public Job SavedJob { get; set; }

    public Job PrevJob { get; set; }

    public float Weight { get; set; }

    public float Height { get; set; }

    public Nutrition Nutrition1 { get; set; }

    public bool WasFired { get; set; }

    public PersonBank PersonBank { get; set; }
}
