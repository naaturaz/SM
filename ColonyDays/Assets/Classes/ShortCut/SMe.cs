using UnityEngine;
using System.Collections.Generic;
//Will containt properties that will reference directyl to Terreno.MeshController.variable

//ShortCut MeshController Variables
public class SMe {

    /// <summary>
    /// Shared from MeshCrontoller
    /// </summary>


    public MeshController MeshController
    {
        get { return Program.gameScene.controllerMain.MeshController; }
        set { Program.gameScene.controllerMain.MeshController = value; }
    }

    public RaycastHit HitMouseOnTerrain
    {
        get { return Program.gameScene.controllerMain.MeshController.HitMouseOnTerrain; }
        set { Program.gameScene.controllerMain.MeshController.HitMouseOnTerrain = value; }
    }

    //Mesh classes helpers, these all inherit from General: Monobehaviuor


    public Malla Malla
    {
        get { return Program.gameScene.controllerMain.MeshController.Malla; }
        set { Program.gameScene.controllerMain.MeshController.Malla = value; }
    }

    public List<Vector3> AllVertexs
    {
        get { return Program.gameScene.controllerMain.MeshController.AllVertexs; }
        set { Program.gameScene.controllerMain.MeshController.AllVertexs = value; }
    }


    public UPoly Poly
    {
        get { return Program.gameScene.controllerMain.MeshController.Poly; }
        set { Program.gameScene.controllerMain.MeshController.Poly = value; }
    }

    public SubDivider SubDivide
    {
        get {
            return Program.gameScene.controllerMain.MeshController.subDivide; 
        }
        set { Program.gameScene.controllerMain.MeshController.subDivide = value; }
    }

    public Vertexer Vertex
    {
        get { return Program.gameScene.controllerMain.MeshController.Vertex; }
        set { Program.gameScene.controllerMain.MeshController.Vertex = value; }
    }

    public InitializerTerrain IniTerr
    {
        get { return Program.gameScene.controllerMain.MeshController.iniTerr; }
        set { Program.gameScene.controllerMain.MeshController.iniTerr = value; }
    }


    public SubPolyr SubPolyr
    {
        get { return Program.gameScene.controllerMain.MeshController.SubPolyr; }
        set { Program.gameScene.controllerMain.MeshController.SubPolyr = value; }
    }

    //dont inherit


    public SubMeshData SubMesh
    {
        get { return Program.gameScene.controllerMain.MeshController.subMesh; }
        set { Program.gameScene.controllerMain.MeshController.subMesh = value; }
    }



    public bool IsMouseOnTerrain
    {
        get { return Program.gameScene.controllerMain.MeshController.IsMouseOnTerrain; }
        set { Program.gameScene.controllerMain.MeshController.IsMouseOnTerrain = value; }
    }

    //Malla the big lot vertices held in current hovering


    public List<Vector3> CurrentHoverVertices
    {
        get { return Program.gameScene.controllerMain.MeshController.CurrentHoverVertices; }
        set { Program.gameScene.controllerMain.MeshController.CurrentHoverVertices = value; }
    }

    //SubPolygons


    public List<Vector3> SubPolysList
    {
        get { return Program.gameScene.controllerMain.MeshController.SubPolysList; }
        set { Program.gameScene.controllerMain.MeshController.SubPolysList = value; }
    }

    public Vector3[] Vertices
    {
        get { return Program.gameScene.controllerMain.MeshController.Vertices; }
        set { Program.gameScene.controllerMain.MeshController.Vertices = value; }
    }
}
