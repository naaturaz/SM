/*Handles funtions related to vertex operations 
 */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Vertexer : General//only to use print()
{
    private List<int> _indexesHover = new List<int>();//the lots indexes hovered

    /// <summary>
    /// Will hold the list of int and corresponendt of Vector 3 for CurrentVertexs asked for MeshController
    /// This is implemented fot GC
    /// </summary>
    Dictionary<int, List<Vector3>> _vertexBank = new Dictionary<int, List<Vector3>>(); 

    public List<int> IndexesHover
    {
        get { return _indexesHover; }
    }

    //it finds the closer vertex to it
    public Vector3 FindClosestVertex(Vector3 to, Vector3[] verticesP, float epsilon = 0.01f)
    {
        List<float> distances = UMath.ReturnDistances(verticesP, to);
        List<int> minimosIndex = UMath.ReturnTheIndexes(distances, 1, epsilon);
        return verticesP[minimosIndex[0]];
    }

    public List<Vector3> UpdateCurrentVertices(Malla mallaPass, int columns, int rows, float stepX, float stepZ,
        RaycastHit hitMouseOnTerrain)
    {
        if (mallaPass.Lots.Count == 0)
        {
            print("Malla pass had not lots assgined");
            return new List<Vector3>();
        }

        //for GC
        var indexMiddle = UMesh.ReturnIndexContain(hitMouseOnTerrain.point, mallaPass.Lots);
        if (IsOnBankAlready(indexMiddle))
        {
            return _vertexBank[indexMiddle];
        }

        List<Vector3> objects = new List<Vector3>();
        objects = UMesh.ReturnThePos(hitMouseOnTerrain.point, stepX, stepZ, columns, rows);
        
        _indexesHover = UMesh.ReturnIndexesContain(objects, mallaPass.Lots);
        var res = UMesh.ReturnCurrentLotsVertex(IndexesHover, mallaPass.Lots);

        AddToBank(indexMiddle, res);
        return res;
    }


    void AddToBank(int indexMiddle, List<Vector3> vertexes)
    {
        _vertexBank.Add(indexMiddle, vertexes);
    }

    bool IsOnBankAlready(int indexMiddle)
    {
        return _vertexBank.ContainsKey(indexMiddle);
    }




    //Given x and z Build a fake vertex where the terrain hits on Y... 
    public Vector3 BuildVertexWithXandZ(float x, float z)
    {
//       Debug.Log("BlueRay");
        return new Vector3(x, m.SubDivide.FindYValueOnTerrain(x, z), z);
    }

    //given the closest vertex of any polygon to the Vector3 mousePos
    //will find in wich Quadrant the Vector3 mousePos is 
    public Dir FindVertexQuadrant(Vector3 vertex, Vector3 mousePos)
    {
        Dir quadrant = Dir.None;
        if (vertex.x <= mousePos.x && vertex.z >= mousePos.z)
        {
            quadrant = Dir.UpLeft;
        }
        else if (vertex.x >= mousePos.x && vertex.z >= mousePos.z)
        {
            quadrant = Dir.UpRight;
        }
        else if (vertex.x >= mousePos.x && vertex.z <= mousePos.z)
        {
            quadrant = Dir.DownRight;
        }
        else if (vertex.x <= mousePos.x && vertex.z <= mousePos.z)
        {
            quadrant = Dir.DownLeft;
        }
        return quadrant;
    }


    /// Find the top left vertex given the firt vertex of any poly and in which quadrant is
    /// ... with the float stepX, float stepZ will find it in Vector3[] vertices
    public Vector3 FindTopLeftVertex(Vector3 firstVertex, Dir quadrant, float stepX, float stepZ, 
        Vector3[] vertices)
    {
        Vector3 upLeftVertex = new Vector3();
        Vector3 temp = firstVertex;
        if (quadrant == Dir.UpLeft)
        {
            upLeftVertex = firstVertex;
        }
        else if (quadrant == Dir.UpRight)
        {
            temp.x = firstVertex.x - Mathf.Abs(stepX);
            upLeftVertex = FindClosestVertex(temp, vertices);
        }
        else if (quadrant == Dir.DownRight)
        {
            temp.x = firstVertex.x - Mathf.Abs(stepX);
            temp.z = firstVertex.z + Mathf.Abs(stepZ);
            upLeftVertex = FindClosestVertex(temp, vertices);
        }
        else if (quadrant == Dir.DownLeft)
        {
            temp.z = firstVertex.z + Mathf.Abs(stepZ);
            upLeftVertex = FindClosestVertex(temp, vertices);
        }
        return upLeftVertex;
    }

    public Vector3 FindTopLeftVertex(Vector3 firstVertex, float stepX, float stepZ, Vector3[] vertices)
    {
        Dir quadrant = FindVertexQuadrant(firstVertex, Input.mousePosition);
        return FindTopLeftVertex(firstVertex, quadrant, stepX, stepZ, vertices);
    }

    public Vector3 FindTopLeftVertexInSubMesh(Vector3 firstVertex, Vector3[] vertices)
    {
        Dir quadrant = FindVertexQuadrant(firstVertex, Input.mousePosition);
        return FindTopLeftVertex(firstVertex, quadrant, m.SubDivide.XSubStep, m.SubDivide.ZSubStep, vertices);
    }
}