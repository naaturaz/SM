using UnityEngine;
using System.Collections.Generic;

public class SpawnedData {

    public Vector3 Pos;
    public Quaternion Rot;
    public H Type;
    public int RootStringIndex;//index of the list root string, defined on TerrainSpawnController
    public int AllVertexIndex;//correspondant current obj index in the Terreno.MeshController.AllVertex List

    public List<SpawnedData> AllSpawnedObj = new List<SpawnedData>();//containts all spawned date serie saved
    public int TerraMshCntrlAllVertexIndexCount;//Terreno.MeshController.AllVertex saved

    //the tree height 
    //used when a tree is replanted 
    public float TreeHeight = 1;

    public MDate SeedDate;
    public float MaxHeight;
    public bool TreeFall;
    public float Weight;

    public int Region;


    public SpawnedData(Vector3 posP, Quaternion rotP, H typeP,  int rootStringIndexP,
        int allVertexIndexP, float treeHeight=0, MDate seedDate=null, float maxHeight=0,
        bool treeFall=false, float weight=0, int region = -1)
    {
        Pos = posP;
        Rot = rotP;
        Type = typeP;
        RootStringIndex = rootStringIndexP;
        AllVertexIndex = allVertexIndexP;

        TreeHeight = treeHeight;
        SeedDate = seedDate;
        MaxHeight = maxHeight;

        TreeFall = treeFall;
        Weight = weight;

        Region = region;
    }

    public SpawnedData(){}




    /// <summary>
    /// The distance of the midle of the region to the init region 
    /// </summary>
    /// <returns></returns>
    internal float RegionDistanceToInit()
    {
        var cryRegion = MeshController.CrystalManager1.CrystalRegions[TownLoader.InitRegion];

        //if fell outside
        if (Region < 0 || Region > 155)
        {
            var a = "";
            return 1000;
        }

        return Vector3.Distance(MeshController.CrystalManager1.CrystalRegions[Region].Position(),
            cryRegion.Position());
    }
}
