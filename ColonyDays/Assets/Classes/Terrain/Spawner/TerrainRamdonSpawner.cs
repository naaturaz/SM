using UnityEngine;

public class TerrainRamdonSpawner : General {

   // public H Instruction { get; set; }

    bool _isMarkToMine;
    public bool IsMarkToMine
    {
        get { return _isMarkToMine; }
        set { _isMarkToMine = value; }
    }

    static public TerrainRamdonSpawner CreateTerraSpawn(string root, Vector3 origen, int indexAllVertex, H hType,
        string name = "", Transform container = null)
    {
        WAKEUP = true;
        TerrainRamdonSpawner obj = null;
        obj = (TerrainRamdonSpawner)Resources.Load(root, typeof(TerrainRamdonSpawner));
        obj = (TerrainRamdonSpawner)Instantiate(obj, origen, Quaternion.identity);
        if (name != "") { obj.name = name; }
        if (container != null){obj.transform.parent = container;}
        obj.IndexAllVertex = indexAllVertex;
        obj.HType = hType;
        obj.MyId = obj.Rename(name, obj.Id, obj.HType);
        obj.transform.name = obj.MyId;

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


}
