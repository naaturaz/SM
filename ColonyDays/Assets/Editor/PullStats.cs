using UnityEngine;
using UnityEditor;
using System.Collections;

public class PullStats 
{

    public static string AddUnityStats()
    {
        return "\n SetPass:" + UnityStats.setPassCalls + " | Shadows:" + UnityStats.shadowCasters +
               " | Skinned: " + UnityStats.visibleSkinnedMeshes;

    }
}
