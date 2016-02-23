using System;
using System.Collections.Generic;

public class Naming
{
    Random _random = new Random();

    private H _gender;//f female, m male
    private static int _currentMales;//how many males on the game.. this has to be pulled from some class
    private static int _currentFemales;//how many males on the game.. this has to be pulled from some class

    private static List<string> _peoplesName = new List<string>();//if is not here then can be assign 

    private int longestName = 10;//longest name lenght

    public Naming(H gender)
    {
        if (gender == H.None) { throw new Exception("gender must be set");}
        _gender = gender;
    }

    public static List<string> PeoplesName
    {
        get { return _peoplesName; }
        set { _peoplesName = value; }
    }

    public string NewName()
    {
        string name = NamingTechnics(_random.Next(0, 2));
        name = CaseItRight(name);
        name = Capped(name);

        if (!IsNewName(name))
        { name = NewName(); }

        _peoplesName.Add(name);
        return name;
    }

    string Capped(string newName)
    {
        if (newName.Length > longestName)
        {
            newName = newName.Substring(0, longestName);
        }
        return newName;
    }

    string CaseItRight(string newName)
    {
        newName = newName.ToLower();
        string ini = newName.Substring(0, 1);
        ini = ini.ToUpper();
        newName = newName.Substring(1, newName.Length - 1);
        return ini + newName;
    }

    bool IsNewName(string newName)
    {
        if (PeoplesName.Count == 0) { return true; }

        for (int i = 0; i < PeoplesName.Count; i++)
        {
            if (PeoplesName[i] == newName)
            { return false; }
        }
        return true;
    }

    string NamingTechnics(int tech)
    {
        if (tech == 1)
        { return NameMeRealName(); }

        return Add2Names();//0
    }

    string Add2Names()
    {
        string name1 = NameMeRealName();
        string name2 = NameMeRealName();

        name1 = name1.Substring(_random.Next(name1.Length / 2, name1.Length));
        name2 = name2.Substring(_random.Next(0, name2.Length / 2));
        return name1 + name2;
    }

    string NameMeRealName()
    {
        string tempName = "";
        string[] maleNames =
        {
            "Manuel", "Alejandro", "Anthony", "Jose", "Herminio", "Aldo", "Isidro", "Lazaro",
            "Fernando", "Colon", "Oslandy", "Oslay",
            "Almendra", "Enrrique",
            "Yoniel", "Santana", "Sandor", "Ferreiro", "Michel", "Otero", 
            "Yohandry", "Yoandy",

            "Aaron", "Andrew", "Bill", "Carl", "Carlos", "John", "Jose", "Riley", "Rob",
            "Craig", "Elon", "Musk", "Page",
            "James", "Robert", "Michael", "William", "David", "Dave", "Richard", "Charles",
            "Joseph", "Thomas", "Christopher", "Paul", "Mark", "Donald", "George", "Kenneth",
            "Steven", "Edward", "Brian", "Ronald", "Anthony", "Kevin", "Jason", "Matthew", "Gary",
            "Timothy", "Larry", "Jeffrey", "Jeff", "Frank", "Scott", "Stephen", "Raymond",
            "Gregory", "Joshua", "Jerry", "Tom", "Walter", "Patrick", "Peter", "Harold",
            "Douglas", "Henry", "Arthur", "Ryan", "Roger", "Joe", "Juan", "Jack", "Jake", "Albert",
            "Jonathan", "Justin", "Terry", "Sean", "Terry", "Gerald", "Keith", "Samuel", "Willie",
            "Ralph", "Lawrence", "Nicholas", "Nick", "Roy", "Ben", "Benjamin", "Bruce", "Brandon",
            "Adam", "Harry", "Fred", "Alfred", "Wayne", "Billy", "Steve", "Louis", "Luis", "Jeremy",
            "Randy", "Howard", "Eugene", "Bobby", "Victor", "Martin", "Ernest", "Phillip", "Todd",
            "Jesse", "Jeffo", "Alan", "Shawn", "Philip", "Chris", "Johnny", "Earl", "Antonio", "Bryan",
            "Tony", "Mike", "Stanley", "Leonard", "Nathan", "Dale", "Rodney", "Curtis",
            "Norman", "Allen", "Marvin", "Vincent", "Glenn", "Jeffery", "Travis", "Lee", "Melvin",
            "Kyle", "Francis", "Bradley", "Jesus", "Herbert"
        }; //http://names.mongabay.com/male_names.htm  position: 130th

        string[] femaleNames =
        {
            "Fatima", "Amalia", "Cristina", "Maria", "Madeleyne", "Luisa", "Berta", "Candelaria", "Canda", "Candita",
            "Lazara", "Caridad",
            "Adelina", "Sonia", "Vera",
            "Amelie", "Amelia", "Marita",

            "Nikita", "Emma", "Isabella", "Olivia", "Ava", "Emily", "Abigail", "Mia", "Madison",
            "Elizabeth", "Chloe", "Ella", "Avery", "Addison", "Aubrey", "Lily", "Natalie", "Anita",
            "Charlotte", "Zoey", "Grace", "Hannah", "Amelia", "Lillian", "Samantha", "Evelyn", "Victoria",
            "Brooklyn", "Vicky", "Zoe", "Layla", "Hailey", "Leah", "Kaylee", "Anna", "Aaliyah", "Gabriella",
            "Allison", "Nevaeh", "Alexis", "Audrey", "Savannah", "Sarah", "Alyssa", "Claire", "Taylor",
            "Camila", "Arianna", "Ashley", "Brianna", "Sophie", "Peyton", "Bella", "Khloe", "Genesis",
            "Alexa", "Serenity", "Kylie", "Aubree", "Scarlett", "Stella", "Maya", "Katherine", "Julia",
            "Lucy", "Madelyn", "Autumn", "Makayla", "Kayla", "Mackenzie", "Lauren", "Gianna", "Ariana",
            "Faith", "Alexandra", "Melanie", "Sydney", "Bailey", "Caroline", "Carla", "Naomi", "Morgan",
            "Kennedy", "Ellie", "Jasmine", "Eva", "Skylar", "Kimberly", "Violet", "Molly", "Aria",
            "Jocelyn", "Trinity", "London", "Lydia", "Madeline", "Reagan", "Piper", "Andrea", "Lina",
            "Annabelle", "Havana"
        }; //http://baby-names.familyeducation.com/popular-names/girls/#ixzz31bj38TT8 pos: 100th

        if (_gender == H.Male)
        {
            int rand = _random.Next(0, maleNames.Length);
            tempName = maleNames[rand];
        }
        else if (_gender == H.Female)
        {
            int rand = _random.Next(0, femaleNames.Length);
            tempName = femaleNames[rand];
        }
        return tempName;
    }
}
