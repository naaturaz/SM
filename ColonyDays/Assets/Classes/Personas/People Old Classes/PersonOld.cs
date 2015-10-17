using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersonOld : General {

	#region vars
	//person attributes
	private string _name;
	private int _age;
	private HPers _gender;
    //private Profession _profession;
	private double _happyness;
    private HPers _ageGroup;
    private TaskE _task;
    private Doing _doing;
    private List<Model> foodSupply;

	//living and work...
	private GameObject _home;
	private GameObject _work;

	//class intern var
	private GameObject startPlace;
	private GameObject endPlace;
	
	private bool setStartPoint = false;
	private Vector3 direction = new Vector3();
	private Vector3 prevPos = new Vector3();
	private bool translateNow = false;
	#endregion vars

	//constructors
    public PersonOld() {}

    public PersonOld(string type, Vector3 initialPos)
	{
        //Id = Id + 1;//id has the function of autonumber
        //SetRoot(type);
        //Type = type;
        //SetGender (type);
        //Name = NameMe(Gender);
        //RenameGamObjAsName();
        //SetAgeGroup(Age);//will set age group based on age
	}

	#region Properties
	public string Name
	{
		get{return _name;}
		set{ _name = value;}
	}

	public int Age
	{
		get{ return _age;}
		set{ _age = value;}
	}

    public HPers Gender
	{
		get{return _gender;}
		set{ _gender = value;}
	}

    //public Profession Profession
    //{
    //    get{return _profession;}
    //    set{ _profession = value;}
    //}

	public double Happiness
	{
		get{ return _happyness;}
		set{ _happyness = value;}
	}

    public HPers AgeGroup
	{
		get{return _ageGroup;}
		set{ _ageGroup = value;}
	}

	public Doing Doing
	{
		get{ return _doing;}
		set{ _doing = value;}
	}

    public TaskE Task
    {
        get { return _task; }
        set { _task = value; }
    }

	//properties living and work
	public GameObject Home
	{
		get{return _home;}
		set{ _home = value;}
	}

	public GameObject Work
	{
		get{return _work;}
		set{ _work = value;}
	}
	#endregion Properties





	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}
}
