using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public static bool IsOnTerrainManipulateTerrainSize(Vector3 a, float manipulateBy)
    {
        var terrainPolyScaled = UPoly.ScalePoly(Program.gameScene.controllerMain.MeshController.wholeMalla, manipulateBy);

        Rect terra = U2D.FromPolyToRect(terrainPolyScaled);
        terra = U2D.ReturnRectYInverted(terra);//must be inverted to be on same Y values 

        if (terra.Contains(new Vector2(a.x, a.z)))
        { return true; }

        return false;
    }

}
