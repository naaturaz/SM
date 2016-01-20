using UnityEngine;

public class TerrainRamdonSpawner : General {

    public bool ReplantedTree { get; set; }

    static public TerrainRamdonSpawner CreateTerraSpawn(string root, Vector3 origen, int indexAllVertex, H hType,
        string name = "", Transform container = null, bool replantedTree = false)
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

        obj.ReplantedTree = replantedTree;

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
        var ele = Program.gameScene.controllerMain.TerraSpawnController.FindThis(MyId);
        return ele.ReadyToMine();
    }
}
