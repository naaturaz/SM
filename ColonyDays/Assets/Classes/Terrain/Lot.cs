/* This class that hold the lots object. Each mesh 
 * is subdived in many lots... Nov 27 2014
 * The lots are 5 real col times 5 real rows. When 
 * I say real Im refering to real vetices of the mesh. Each real
 * poly is subdived in 25 fake subpolygons and 
 * that is a lot: 5 rows times 5 col times 25 fake poly (those numbers can change).
 * 
 * Each Lot has an specific index that we use to determine the 9 lots closer to 
 * where the mouse is hitting the terrain
 */
using UnityEngine;
using System.Collections.Generic;

public class Lot
{
    //this one are the real vertices on this lot. By the way they are really close
    //to the real vertices but they are not truly real. The real vertices are in Vector3[] Vertices on Mesh Controller
    private List<Vector3> _realVertices;

    public List<Vector3> LotVertices;//this is the submeshed fake vertices
    public int Index;
    public Vector3 squareStart, squareEnd;

    public List<Vector3> RealVertices
    {
        get { return _realVertices; }
        set { _realVertices = value; }
    }

    public Lot(){}
    public Lot(Vector3 squareStart, Vector3 squareEnd)
    {
        this.squareStart = squareStart;
        this.squareEnd = squareEnd;
    }

    public Lot(List<Vector3> squareVertices, int index, Vector3 squareStart, Vector3 squareEnd)
    {
        LotVertices = squareVertices;
        Index = index;

        //UVisHelp.CreateHelpers(squareStart, Root.redSphereHelp);
        this.squareStart = squareStart;
        this.squareEnd = squareEnd;

        //GameScene.ScreenPrint(Program.gameScene.controllerMain.MeshController.iniTerr.StepX.ToString()+":"+
        //    Program.gameScene.controllerMain.MeshController.iniTerr.StepZ.ToString());

        //UVisHelp.CreateHelpers(squareEnd, Root.blueCube);
    }

    public void SetRealVertices()
    {
        RealVertices = RetuFillPolyRealY(squareStart, squareEnd,
            Mathf.Abs( Program.gameScene.controllerMain.MeshController.iniTerr.StepX),
            Mathf.Abs( Program.gameScene.controllerMain.MeshController.iniTerr.StepZ));
    }

    /// <summary>
    /// Return a filed poly with RealYs if  bool findRealY is true. Otherwise the same but the Y is NW.y
    /// </summary>
    List<Vector3> RetuFillPolyRealY(Vector3 NW, Vector3 SE, float xStep, float zStep)
    {
        List<Vector3> res = new List<Vector3>();
        SE = MoveVector3Towards(SE, Dir.NW, xStep, zStep);
        //UVisHelp.CreateHelpers(SE, Root.yellowSphereHelp);

        for (float x = NW.x; x < SE.x; x += xStep)
        {
            for (float z = NW.z; z > SE.z; z -= zStep)
            {
                //for fill a field we shiyld use the REal Y so tiles look close to ground
                res.Add(Program.gameScene.controllerMain.MeshController.Vertex.BuildVertexWithXandZ(x, z)); 
            }
        }
        //UVisHelp.CreateHelpers(res, Root.blueCube);
        return res;
    }

    Vector3 MoveVector3Towards(Vector3 current, Dir towards, float moveInX, float moveInZ)
    {
        if (towards == Dir.NW)
        {
            current.x = current.x - Mathf.Abs( moveInX);
            current.z = current.z + Mathf.Abs(moveInZ);
        }
        return current;
    }

    //returns current index of this lot.
    public int IndexOfCurrentLotBeingHover(Vector3 objPos)
    {
        if (UMesh.Contains(objPos, squareStart, squareEnd))
        {
            return Index;
        }
        return -1;
    }
}