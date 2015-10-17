using System.Collections.ObjectModel;
using UnityEngine;

public class RegFileKey : KeyedCollection<string, RegFile>
{
    protected override string GetKeyForItem(RegFile item)
    {
        return item.MyId;

    }
}

public class TerrainRamdonSpawnerKey : KeyedCollection<string, TerrainRamdonSpawner>
{
    protected override string GetKeyForItem(TerrainRamdonSpawner item)
    {
        return item.MyId;
    }
}

public class StructureKey : KeyedCollection<string, Structure>
{
    protected override string GetKeyForItem(Structure item)
    {
        return item.MyId;
    }
}

public class FarmKey : KeyedCollection<string, DragSquare>
{
    protected override string GetKeyForItem(DragSquare item)
    {
        return item.MyId;
    }
}

public class WayKey : KeyedCollection<string, Way>
{
    protected override string GetKeyForItem(Way item)
    {
        return item.MyId;
    }
}

public class KeyedColl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
