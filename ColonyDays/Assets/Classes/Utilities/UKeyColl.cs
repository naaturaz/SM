using UnityEngine;

public class UKeyColl : MonoBehaviour
{
    /// <summary>
    /// Say if a key is contained in its KeyedCollection
    /// </summary>
    public static bool CheckIfKeyInColl<A, T>(A coll, T item)
    {
        if (coll is StructureKey)
        {
            StructureKey colla = coll as StructureKey;
            if (colla.Contains(item.ToString())) { return true; }
        }
        else if (coll is WayKey)
        {
            WayKey colla = coll as WayKey;
            if (colla.Contains(item.ToString())) { return true; }
        }
        else if (coll is TerrainRamdonSpawnerKey)
        {
            TerrainRamdonSpawnerKey colla = coll as TerrainRamdonSpawnerKey;
            if (colla.Contains(item.ToString())) { return true; }
        }
        return false;
    }
}