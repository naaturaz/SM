using UnityEngine;

public class TerrainRamdonSpawner : General {

    //the tree height 
    //used when a tree is replanted 
    /// <summary>
    /// the height of the elements they start always with 1. if a tree is replanted is 
    /// set to zero and has to reegrow
    /// </summary>
    public float Height = .25f;

    public MDate SeedDate;

    private float _maxHeight;


    private int _rootToSpawnIndex;

    //set in UnityEditor Mannually
    public float MaxHeight
    {
        get { return _maxHeight; }
        set { _maxHeight = value; }
    }


    public bool ReplantedTree { get; set; }

    private bool _shouldReplant;
    public bool ShouldReplant
    {
        get { return _shouldReplant; }
        set { _shouldReplant = value; }
    }



    /// <summary>
    /// Only used by PoolTrees bz they thenw will pass this info into the DataList
    /// </summary>
    public int RootToSpawnIndex
    {
        get { return _rootToSpawnIndex; }
        set { _rootToSpawnIndex = value; }
    }


    static public TerrainRamdonSpawner CreateTerraSpawn(string root, Vector3 origen, Vector3 rotation,
        int indexAllVertex, H hType,
        string name = "", Transform container = null, bool replantedTree = false,
        float height = 0, MDate seedDate = null, float maxHeight = 0,
        Quaternion rot = new Quaternion())
    {
        WAKEUP = true;
        TerrainRamdonSpawner obj = null;
        obj = (TerrainRamdonSpawner)Resources.Load(root, typeof(TerrainRamdonSpawner));

        if (obj==null)
        {
            Debug.Log("null:"+root);
        }

        obj = (TerrainRamdonSpawner)Instantiate(obj, origen, Quaternion.identity);
        if (name != "") { obj.name = name; }
        if (container != null){obj.transform.SetParent( container);}
        obj.IndexAllVertex = indexAllVertex;
        obj.HType = hType;
        obj.Category = obj.DefineCategory(hType);
        obj.MyId = obj.Rename(name, obj.Id, obj.HType);
        obj.transform.name = obj.MyId;

        //here to avoid rotating object after spwaned
        //for loading
        obj.transform.rotation = rot;
        //for new obj
        obj.transform.Rotate(rotation);

        obj.ReplantedTree = replantedTree;
        obj.Height = height;
        obj.SeedDate = seedDate;
        return obj;
    }

	// Use this for initialization
	protected void Start () 
    {
        //needs to be define so in Router.cs he can see it as a blocking 
        Category = DefineCategory(HType);
	}
	
	// Update is called once per frame
	void Update () {
	
	}



    internal bool Grown()
    {
        var ele = Program.gameScene.controllerMain.TerraSpawnController.Find(MyId);

        if (ele==null)
        {
            return false;
        }

        return ele.ReadyToMine();
    }

    //bool _replatedTreeWasStarted;

    ///// <summary>
    ///// So goes back to pool and then can be used again
    ///// </summary>
    //internal void Reset()
    //{
    //    ReplatedTreeWasStarted = false;
    //    _shouldReplant = false;

    //    //
    //    MyId = "Reset tree" + Id;
    //    name = MyId;

    //    transform.position=new Vector3();

    //}

    
    protected int howDeepInY = 50;
    /// <summary>
    /// The action of getting a new Swaped in tree ready 
    /// </summary>
    /// <param name="oldTree"></param>
    internal void SwapIn(TerrainRamdonSpawner oldTree)
    {
        var oldP = oldTree.transform.position;
        MyId = oldTree.MyId;
        name = oldTree.name;

        //hiding in deep into terrain
        transform.position = new Vector3(oldP.x, oldP.y - howDeepInY, oldP.z);
        transform.rotation = oldTree.transform.rotation;
        IndexAllVertex = oldTree.IndexAllVertex;
        SeedDate = Program.gameScene.GameTime1.CurrentDate();
        ReplantedTree = true;
    }

    /// <summary>
    /// will add this to the Crsytals so it can be routed to
    /// </summary>
    internal void AddCrystals()
    {
        if (ReplantedTree || ShouldReplant || name.Contains("Orna") || name.Contains("Grass"))
        {
            return;
        }
        var still = (StillElement)this;
        still.RedoCrystals();

    }
}
