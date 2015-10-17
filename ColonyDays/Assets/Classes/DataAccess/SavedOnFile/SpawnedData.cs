using UnityEngine;
using System.Collections.Generic;

public class SpawnedData {

    public Vector3 Pos;
    public Quaternion Rot;
    public H Type;
    public bool IsMarkToMine;
    public int RootStringIndex;//index of the list root string, defined on TerrainSpawnController
    public int AllVertexIndex;//correspondant current obj index in the Terreno.MeshController.AllVertex List

    public List<SpawnedData> AllSpawnedObj = new List<SpawnedData>();//containts all spawned date serie saved

    public int TerraMshCntrlAllVertexIndexCount;//Terreno.MeshController.AllVertex saved

    public SpawnedData(Vector3 posP, Quaternion rotP, H typeP, bool IsMarkToMineP, int rootStringIndexP,
        int allVertexIndexP)
    {
        Pos = posP;
        Rot = rotP;
        Type = typeP;
        IsMarkToMine = IsMarkToMineP;
        RootStringIndex = rootStringIndexP;
        AllVertexIndex = allVertexIndexP;
    }

    public SpawnedData(){}
}
