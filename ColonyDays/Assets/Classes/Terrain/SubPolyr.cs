using System.Collections.Generic;
using UnityEngine;

public class SubPolyr : General
{
    //Return the subpolygons selected ... this are fake subpolygons... a bunch of them
    public List<Vector3> SubPolygonsSelected(int cols, int rows, ref List<Vector3> objsHitTerrain,
        Vector3 iniHit, SubDivider subDivide, int rotationFacer, bool isMouseOnTerrain,
        Vertexer vertex, UPoly poly, List<Vector3> currentHoverVertices)
    {
        List<Vector3> subPolys = new List<Vector3>();
        subPolys = CreateListSelected(ref objsHitTerrain, cols, rows, iniHit, subDivide, isMouseOnTerrain,
            vertex, poly, currentHoverVertices);
        //print(cols + ".c||r."+rows+ " ini:."+iniHit);
        //print(objsHitTerrain.Count + " objsHitTerrain.cou");
        //print(subPolys.Count + " subPolys.coun");
        //List<Vector3> firstRow = ReturnFirstRow(subPolys, rotationFacer);
        return subPolys;
    }

    /// Creates a list for the selected sub polygons given the columns and rows
    private List<Vector3> CreateListSelected(ref List<Vector3> objsHitTerrain, int columns, int rows, Vector3 iniHit,
        SubDivider subDivide, bool isMouseOnTerrain, Vertexer vertex, UPoly poly, List<Vector3> currentHoverVertices)
    {
        objsHitTerrain = UMesh.ReturnThePos(iniHit, subDivide.XSubStep, subDivide.ZSubStep, columns, rows);

        List<Vector3> res = new List<Vector3>();
        for (int i = 0; i < objsHitTerrain.Count; i++)
        {
            res = UList.AddOneListToList(res, CreateOneSubPoly(objsHitTerrain[i], isMouseOnTerrain, vertex,
                subDivide, poly, currentHoverVertices));
        }
        //still needs to eliminate duplicates

        return res;
    }

    //Returns first row list based on the rotation facer
    private List<Vector3> ReturnFirstRow(List<Vector3> listSelected, int rotationFacer)
    {
        List<Vector3> res = new List<Vector3>();
        if (rotationFacer == 0)
        {
            List<float> allZ = UList.ReturnAxisList(listSelected, H.Z);
            float zMax = UMath.ReturnMax(allZ);
            res = FirstRowLoop(listSelected, zMax, H.Z);
        }
        else if (rotationFacer == 1)
        {
            List<float> allX = UList.ReturnAxisList(listSelected, H.X);
            float xMax = UMath.ReturnMax(allX);
            res = FirstRowLoop(listSelected, xMax, H.X);
        }
        else if (rotationFacer == 2)
        {
            List<float> allZ = UList.ReturnAxisList(listSelected, H.Z);
            float zMin = UMath.ReturnMinimum(allZ);
            res = FirstRowLoop(listSelected, zMin, H.Z);
        }
        else if (rotationFacer == 3)
        {
            List<float> allX = UList.ReturnAxisList(listSelected, H.X);
            float xMin = UMath.ReturnMinimum(allX);
            res = FirstRowLoop(listSelected, xMin, H.X);
        }
        return res;
    }

    //The loop for the first row
    private List<Vector3> FirstRowLoop(List<Vector3> listSelected, float valToAlign, H axis)
    {
        bool nearEq = false;
        List<Vector3> res = new List<Vector3>();
        for (int i = 0; i < listSelected.Count; i++)
        {
            if (axis == H.X)
            {
                nearEq = UMath.nearlyEqual(valToAlign, listSelected[i].x, 0.001f);
            }
            else if (axis == H.Z)
            {
                nearEq = UMath.nearlyEqual(valToAlign, listSelected[i].z, 0.001f);
            }

            if (nearEq)
            {
                res.Add(listSelected[i]);
            }
        }
        return res;
    }

    //create one subpoly based on where we hit the terrain
    private List<Vector3> CreateOneSubPoly(Vector3 hitPointOnSubPoly, bool isMouseOnTerrain, Vertexer vertex,
        SubDivider subDivide, UPoly poly, List<Vector3> currentHoverVertices)
    {
        //print(isMouseOnTerrain + "isMouseOnTerrain");
        List<Vector3> subPoly = new List<Vector3>();
        if (isMouseOnTerrain)
        {
            Vector3 firstVertex = vertex.FindClosestVertex(hitPointOnSubPoly, currentHoverVertices.ToArray());

            Dir quadrant = vertex.FindVertexQuadrant(firstVertex, hitPointOnSubPoly);
            Vector3 lefTopVertex = vertex.FindTopLeftVertex(firstVertex, quadrant,
                subDivide.XSubStep, subDivide.ZSubStep, currentHoverVertices.ToArray());

            subPoly = UPoly.FindPoly(lefTopVertex, subDivide.XSubStep, subDivide.ZSubStep,
                vertex, currentHoverVertices.ToArray());
            //UVisHelp.CreateHelpers(subPoly, Root.blueCube);
        }
        return subPoly;
    }
}