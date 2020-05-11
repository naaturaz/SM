using UnityEngine;

public class PeopleOld : General
{
    #region vars

    //people attributes
    private string _name;

    private int _age;
    private string _gender;
    private string _profession;
    private double _happyness;
    private string _ageGroup;
    private string _action;

    //living and work...
    private GameObject _home;

    private GameObject _work;
    private GameObject _foodSupply1;
    private GameObject _foodSupply2;

    //class intern var
    private GameObject startPlace;

    private GameObject endPlace;

    private bool isStartPointSet = false;
    private Vector3 direction = new Vector3();
    private Vector3 prevPos = new Vector3();
    private bool isTranslateNow = false;

    #endregion vars

    //constructors
    public PeopleOld() { }

    public PeopleOld(string type, Vector3 initialPos)
    {
        Id = Id + 1;//id has the function of autonumber
        Type = type;
        SetGender(type);
        Name = NameMe(Gender);
        RenameGamObjAsName();
        SetAgeGroup(Age);//will set age group based on age
    }

    #region Properties

    //properties
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public int Age
    {
        get { return _age; }
        set { _age = value; }
    }

    public string Gender
    {
        get { return _gender; }
        set { _gender = value; }
    }

    public string Profession
    {
        get { return _profession; }
        set { _profession = value; }
    }

    public double Happiness
    {
        get { return _happyness; }
        set { _happyness = value; }
    }

    public string AgeGroup
    {
        get { return _ageGroup; }
        set { _ageGroup = value; }
    }

    public string Action
    {
        get { return _action; }
        set { _action = value; }
    }

    //properties living and work
    public GameObject Home
    {
        get { return _home; }
        set { _home = value; }
    }

    public GameObject Work
    {
        get { return _work; }
        set { _work = value; }
    }

    public GameObject FoodSupply1
    {
        get { return _foodSupply1; }
        set { _foodSupply1 = value; }
    }

    public GameObject FoodSupply2
    {
        get { return _foodSupply2; }
        set { _foodSupply2 = value; }
    }

    #endregion Properties

    #region Methods

    public void SetAgeGroup(int agePass)
    {
        if (AgeGroup == "" || AgeGroup == null)
        {
            AgeGroup = "NewBorn";
        }
        else if (AgeGroup == "NewBorn" && agePass > 2)
        {
            AgeGroup = "Kid";
        }
        else if (AgeGroup == "Kid" && agePass > 15)
        {
            AgeGroup = "Adult";
        }
    }

    public void ToggleAction()
    {
        //action that will be the first... needed here otherwise gives exception
        if (Action == "" || Action == null)
        {
            Action = "Work";
        }
        //if not walking
        if (!isTranslateNow)
        {
            isTranslateNow = true;
            isStartPointSet = true;
        }
        //if walking and setStartPoint was set to false already
        else if (isTranslateNow && !isStartPointSet)
        {
            isTranslateNow = false;

            if (Action == "Work")
            {
                Action = "PickFood";
            }
            else if (Action == "PickFood")
            {
                Action = "Rest";
            }
            else if (Action == "Rest")
            {
                Action = "Work";
            }
        }
        //print ("Action: " +Action);
    }

    public void SetGender(string type)
    {
        if (type == "Man1")
        {
            Gender = "Male";
        }
        else if (type == "Woman1")
        {
            Gender = "Female";
        }
    }

    public string NameMe(string myGender)
    {
        string tempName = "";

        string[] maleNames =
        {
             "Alejandro", "Herminio", "Fernando", "Colon", "Oslandy", "Oslay", "Oscar", "Enrrique",
             "Aaron", "Andrew", "Bill", "Carl", "Carlos", "John", "Jose", "Riley", "Rob",
             "Craig",
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
             "Jesse", "Craig", "Alan", "Shawn", "Philip", "Chris", "Johnny", "Earl", "Antonio", "Bryan",
             "Tony", "Mike", "Stanley", "Leonard", "Nathan", "Dale", "Manuel", "Rodney", "Curtis",
             "Norman", "Allen", "Marvin", "Vincent", "Glenn", "Jeffery", "Travis", "Lee", "Melvin",
             "Kyle", "Francis", "Bradley", "Jesus", "Herbert"
        }; //http://names.mongabay.com/male_names.htm  position: 130th

        string[] femaleNames =
        {
             "Fatima", "Maria", "Madeleyne", "Luisa", "Berta", "Adelina", "Sonia", "Vera",
             "Sophia", "Emma", "Isabella", "Olivia", "Ava", "Emily", "Abigail", "Mia", "Madison",
             "Elizabeth", "Chloe", "Ella", "Avery", "Addison", "Aubrey", "Lily", "Natalie", "Sofia",
             "Charlotte", "Zoey", "Grace", "Hannah", "Amelia", "Lillian", "Samantha", "Evelyn", "Victoria",
             "Brooklyn", "Vicky", "Zoe", "Layla", "Hailey", "Leah", "Kaylee", "Anna", "Aaliyah", "Gabriella",
             "Allison", "Nevaeh", "Alexis", "Audrey", "Savannah", "Sarah", "Alyssa", "Claire", "Taylor",
             "Camila", "Arianna", "Ashley", "Brianna" , "Sophie", "Peyton", "Bella", "Khloe", "Genesis",
             "Alexa", "Serenity", "Kylie", "Aubree", "Scarlett", "Stella", "Maya", "Katherine", "Julia",
             "Lucy", "Madelyn", "Autumn", "Makayla", "Kayla", "Mackenzie", "Lauren", "Gianna", "Ariana",
             "Faith", "Alexandra", "Melanie", "Sydney", "Bailey", "Caroline", "Carla", "Naomi", "Morgan",
             "Kennedy", "Ellie", "Jasmine", "Eva", "Skylar", "Kimberly", "Violet", "Molly", "Aria",
             "Jocelyn", "Trinity", "London", "Lydia", "Madeline", "Reagan", "Piper", "Andrea", "Lina",
             "Annabelle", "Havana"
        }; //http://baby-names.familyeducation.com/popular-names/girls/#ixzz31bj38TT8 pos: 100th

        if (myGender == "Male")
        {
            int rand = Random.Range(0, maleNames.Length);
            tempName = maleNames[rand];
        }
        else if (myGender == "Female")
        {
            int rand = Random.Range(0, femaleNames.Length);
            tempName = maleNames[rand];
        }
        return tempName;
    }

    public void RenameGamObjAsName()
    {
        gameObject.name = Id + "." + Name + "." + Gender + ".Age:" + Age;
    }

    public void Walk(float speed)
    {
        gameObject.transform.LookAt(endPlace.transform.position);//must be here otherwise act funny
        Vector3 foward = new Vector3(0, 0, 1f * speed);
        transform.Translate(foward * Time.deltaTime);
    }

    private void MoveToStartPoint()
    {
        if (isStartPointSet)
        {
            gameObject.transform.position = startPlace.transform.position;
            //print("setStartPoint in MoveTo(): " +setStartPoint);
            isStartPointSet = false;
        }
    }

    private void RayCasting()
    {
        //direction
        direction = (transform.position - prevPos) / Time.deltaTime;
        prevPos = transform.position;

        //raycast
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, direction, out hit))
        {
            Debug.DrawRay(gameObject.transform.position, direction, Color.red);
            //print ( "hit.transform.name: " + hit.transform.name );

            //distance... must be inside this 'if' otherwise hit is null
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            //print ( "distance: " + distance ) ;

            //print(endPlace.name);
            //print(hit.transform.name);
            if (distance < 0.5f && endPlace.transform.name == hit.transform.name)
            {
                ToggleAction();
                //print ( "Action Toggled. Current Action: "+Action);
            }
        }
    }

    private void SetWalkPoints()
    {
        if (Action == "" || Action == null || Action == "Work")
        {
            //will assign value to global var
            startPlace = Home.transform.GetChild(0).gameObject;
            endPlace = Work.transform.GetChild(0).gameObject;
        }
        else if (Action == "PickFood")
        {
            startPlace = Work.transform.GetChild(0).gameObject;
            endPlace = FoodSupply1.transform.GetChild(0).gameObject;
        }
        else if (Action == "Rest")
        {
            startPlace = FoodSupply1.transform.GetChild(0).gameObject;
            endPlace = Home.transform.GetChild(0).gameObject;
        }
    }

    private void ControlInput()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            isTranslateNow = true;
        }
    }

    public void SetCurrentAni(string animationPass, string oldAnimation)
    {
        Animator myAnimator = gameObject.GetComponent<Animator>();
        myAnimator.SetBool(animationPass, true);
        myAnimator.SetBool(oldAnimation, false);
    }

    #endregion Methods

    #region UnityEvents

    // Use this for initialization
    private void Start()
    { }

    // Update is called once per frame
    private void LateUpdate()
    {
        ControlInput();
        RayCasting();
        //base.Update();//if u want to execute update() in base class

        //if walking
        if (isTranslateNow)
        {
            SetWalkPoints();
            MoveToStartPoint();
            Walk(3.8f);
        }
    }

    #endregion UnityEvents
}