/*Initializes values for the terrain */
using System.Collections.Generic;
using UnityEngine;

public class InitializerTerrain : General
{
    public float StepX ;//how far in X a real vertices is from another
    public float StepZ;//how far in Z a real vertices is from another
    public int Columns = 0;//how many columns the mesh has
    public int Rows = 0;//how many rows the mesh has
    public Vector3 MathCenter;//center of terrain on real Y

    //of the malla the whole terrain 
    private float _lenght;
    private float _height;

    public float Lenght
    {
        get { return _lenght; }
    }

    public float Height
    {
        get { return _height; }
    }

    //initializes the Vector3[] vertices and the Mesh 
    public void Initializer(ref Vector3[] vertices, ref Mesh mesh)
    {
        MeshCollider meshCollider = Program.gameScene.Terreno.GetComponent<Collider>() as MeshCollider;
        mesh = meshCollider.sharedMesh;
        vertices = mesh.vertices;
        //print("vertices.Length:" + vertices.Length);
    }

    //initializes the List Vector3 wholeMalla, nextStart, and zLot by ref
    //in this class initializes  the fields,StepX , StepZ, Columns, Rows
    public void InitializeMallaStats(Vector3[] vertices, ref List<Vector3> wholeMalla, 
        ref Vector3 nextStart, ref float zLot)
    {
        if (StepX != 0) return;
        if (vertices == null) return;

        wholeMalla = UPoly.ReturnWholeMallaAs1Poly(Program.gameScene.controllerMain.MeshController.Vertices);
        nextStart = wholeMalla[0];
        zLot = wholeMalla[0].z;

        //UVisHelp.CreateHelpers(wholeMalla[0], Root.redSphereHelp);
        //UVisHelp.CreateHelpers(wholeMalla[2], Root.redSphereHelp);

        _lenght = wholeMalla[0].x - wholeMalla[1].x;
        _height = wholeMalla[3].z - wholeMalla[0].z;

        MathCenter = m.Vertex.BuildVertexWithXandZ((wholeMalla[0].x + wholeMalla[1].x) / 2,
            (wholeMalla[3].z + wholeMalla[0].z) / 2);

        float epsi = 0.001f;

        for (int i = 0; i < vertices.Length; i++)
        {
            bool eq = UMath.nearlyEqual(vertices[i].x, wholeMalla[0].x, epsi);
            if (eq)
            {
                Columns++;
            }

            eq = UMath.nearlyEqual(vertices[i].z, wholeMalla[0].z, epsi);
            if (eq)
            {
                Rows++;
            }
        }
        StepX = _lenght / Columns;
        StepZ = _height / Rows;

        //print("Columns:" + Columns);
        //print("Rows:" + Rows);
    }

    //Initialize the Lot Steps values... for scanTerra() will not change the nextStart here.
    //For loading mesh vertices from file we change the Vector3 nextStart for 1st only time here 
    public void InitializeLotStepVal(
        ref Vector3 nextStart, SubDivider subDivide, List<Vector3> wholeMalla, Vector3[] Vertices,
        ref float lotStepX, ref float lotStepZ,
        int inPolyDiv = 0, int polyX = 0, int polyZ = 0)
    {
        if (nextStart == wholeMalla[0] && polyX != 0)
        {
            subDivide.SubDivideLot(ref nextStart, polyX, polyZ,
            StepX, StepZ, inPolyDiv, Vertices);
        }
        if (lotStepZ == 0)
        {
            lotStepZ = wholeMalla[0].z - nextStart.z;
            lotStepX = wholeMalla[0].x - nextStart.x;
            //print(lotStepZ + "lotStepZ." + lotStepX + "lotStepX");
            //print(wholeMalla[0] + "wholeMalla." + nextStart + "nextStart");
        }
    }
}
