using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UTerra
{
    /// <summary>
    /// Syas if param is on terrain
    ///
    /// Needs to be Testet
    /// </summary>
    public static bool IsOnTerrain(Vector3 a)
    {
        Rect terra = U2D.FromPolyToRect(Program.gameScene.controllerMain.MeshController.wholeMalla);
        terra = U2D.ReturnRectYInverted(terra);//must be inverted to be on same Y values

        if (terra.Contains(new Vector2(a.x, a.z)))
        { return true; }

        return false;
    }

    private static List<Vector3> terrainPolyScaled;

    public static bool IsOnTerrainManipulateTerrainSize(Vector3 a, float manipulateBy)
    {
        //bz kkeeps getting smaller
        //if (terrainPolyScaled == null)
        //{
        //bz was referencing that List
        var array = Program.gameScene.controllerMain.MeshController.wholeMalla.ToArray();
        terrainPolyScaled = UPoly.ScalePoly(array.ToList(), manipulateBy);
        //}

        Rect terra = U2D.FromPolyToRect(terrainPolyScaled);
        terra = U2D.ReturnRectYInverted(terra);//must be inverted to be on same Y values
        UVisHelp.CreateDebugLines(terra, Color.yellow);

        if (terra.Contains(new Vector2(a.x, a.z)))
        { return true; }

        return false;
    }
}