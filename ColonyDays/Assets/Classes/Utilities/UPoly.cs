/*
 * Make sure in the terrain no real vertex is on Vector.zero this will bring 
 * huge bugs
 * 
 * 
 */
using UnityEngine;
using System.Collections.Generic;

public class UPoly
{
    static SMe m = new SMe();

    #region Find Other Corner Side Used on Router.cs

    /// <summary>
    /// Give poly, side was hitted and the common corner wichi in the case of person in the closest corner from 
    /// the start point of a Route, will tell u the other corner in the poly.
    /// Ex: If common corner is 'SW' and sideHitLanded is 'W' the result is = 'NW'
    /// </summary>
    public static Vector3 FindOtherCornerOnSide(List<Vector3> poly, Dir sideHitLanded, Vector3 commonCorner)
    {
        Dir commonCornerDir = TellMeWhichCornerIsThis(poly, commonCorner);

        Dir dirH = RemoveDuplicateAndInverseDir(sideHitLanded, commonCornerDir);
        Vector3 res = new Vector3();

        Dir[] dirMap = new Dir[]{Dir.NW, Dir.NE, Dir.SE, Dir.SW};

        for (int i = 0; i < dirMap.Length; i++)
        {
            if (dirH == dirMap[i])
            {
                res = poly[i];
            }
        }
        return res;
    }

    /// <summary>
    /// side hitted + coommeom corner utility,
    /// ex, N + NE should return NW
    /// </summary>
    /// <returns></returns>
    public static Dir RemoveDuplicateAndInverseDir(Dir sideHitLanded,  Dir commonCornerDir)
    {
        char[] corner = commonCornerDir.ToString().ToCharArray();
        char[] side = sideHitLanded.ToString().ToCharArray();
        char[] t = new char[2];

        //ex N and NE. We keep N and flip E
        if (side[0] == corner[0])
        {
            t[0] = side[0]; 
            t[1] = FlipDir(corner[1]);
        }
        else if (side[0] == corner[1])
        {
            t[0] = side[0]; 
            t[1] = FlipDir(corner[0]);
        }
        //calling here bz for exam iin case of a W side + SW corner the smash will be = WN
        return RetDirOrdered(t);
    }

    /// <summary>
    /// from a WN will make it NW
    /// </summary>
    public static Dir RetDirOrdered(char[] unorder)
    {
        Dir res = Dir.None;
        if ((unorder[0] == 'W' && unorder[1] == 'N') || (unorder[0] == 'N' && unorder[1] == 'W'))
        {
            res = Dir.NW;
        }
        else if ((unorder[0] == 'W' && unorder[1] == 'S') || (unorder[0] == 'S' && unorder[1] == 'W'))
        {
            res = Dir.SW;
        }
        else if ((unorder[0] == 'E' && unorder[1] == 'N') || (unorder[0] == 'N' && unorder[1] == 'E'))
        {
            res = Dir.NE;
        }
        else if ((unorder[0] == 'E' && unorder[1] == 'S') || (unorder[0] == 'S' && unorder[1] == 'E'))
        {
            res = Dir.SE;
        }
        return res;
    }

    public static char FlipDir(char flip)
    {
        if (flip == 'N')
        {flip = 'S';}
        else if (flip == 'S')
        { flip = 'N'; }
        else if (flip == 'W')
        { flip = 'E'; }
        else if (flip == 'E')
        { flip = 'W'; }
        return flip;
    }

    public static Dir TellMeWhichCornerIsThis(List<Vector3> poly, Vector3 commonCorner)
    {
        Dir res = Dir.None;
        if (UMath.nearEqualByDistance(commonCorner, poly[0], 0.01f))
        {
            res = Dir.NW;
        }
        if (UMath.nearEqualByDistance(commonCorner, poly[1], 0.01f))
        {
            res = Dir.NE;
        }
        if (UMath.nearEqualByDistance(commonCorner, poly[2], 0.01f))
        {
            res = Dir.SE;
        }
        if (UMath.nearEqualByDistance(commonCorner, poly[3], 0.01f))
        {
            res = Dir.SW;
        }
        return res;
    }

    #endregion


    /// <summary>
    /// Given 2 diagonal points find the other 2 left to create a poly gon
    /// </summary>
    public static Vector3[] ReturnOtherTwoOfPoly(Vector3 one, Vector3 two)
    {
        return new Vector3[] { new Vector3(one.x, one.y, two.z), new Vector3(two.x, one.y, one.z) };
    }

    //Finds a Poly given the topLeft, the steps will find it in the Vector3[] arrayVerts
    public static List<Vector3> FindPoly(Vector3 topLeft, float stepX, float stepZ, Vertexer vertex, Vector3[] arrayVerts)
    {   //with the creating of the dummy ones is working like a charm.. bz the dummy vector3 is really close
        //to the real one... before in mountains didnt work bz in Y they were to far and the closest vertices
        //in a list were diffrent tht i was expecting
        Vector3 temp = topLeft;
        List<Vector3> poly = new List<Vector3>() { topLeft };

        //top Right
        temp.x = topLeft.x + Mathf.Abs(stepX);
        Vector3 toprightDummy = vertex.BuildVertexWithXandZ(temp.x, temp.z);
        Vector3 topright = m.Vertex.FindClosestVertex(toprightDummy, arrayVerts);
        poly.Add(topright);

        //bottom right
        temp.x = topLeft.x + Mathf.Abs(stepX);
        temp.z = topLeft.z - Mathf.Abs(stepZ);
        Vector3 bottomRightDummy = vertex.BuildVertexWithXandZ(temp.x, temp.z);
        Vector3 bottomRight = m.Vertex.FindClosestVertex(bottomRightDummy, arrayVerts);
        poly.Add(bottomRight);

        //bottom left
        temp.x = topLeft.x; //refer again to orignal val
        temp.z = topLeft.z - Mathf.Abs(stepZ);
        Vector3 bottomLeftDummy = vertex.BuildVertexWithXandZ(temp.x, temp.z);
        Vector3 bottomLeft = m.Vertex.FindClosestVertex(bottomLeftDummy, arrayVerts);
        poly.Add(bottomLeft);

        return poly;
    }

    //Creates a Poly given the topLeft, the steps will create it 
    public static List<Vector3> CreatePoly(Vector3 topLeft, float stepX, float stepZ, Vertexer vertex)
    {   //with the creating of the dummy ones is working like a charm.. bz the dummy vector3 is really close
        //to the real one... before in mountains didnt work bz in Y they were to far and the closest vertices
        //in a list were diffrent tht i was expecting
        Vector3 temp = topLeft;
        List<Vector3> poly = new List<Vector3>() { topLeft };

        //top Right
        temp.x = topLeft.x + Mathf.Abs(stepX);
        Vector3 toprightDummy = vertex.BuildVertexWithXandZ(temp.x, temp.z);
        poly.Add(toprightDummy);

        //bottom right
        temp.x = topLeft.x + Mathf.Abs(stepX);
        temp.z = topLeft.z - Mathf.Abs(stepZ);
        Vector3 bottomRightDummy = vertex.BuildVertexWithXandZ(temp.x, temp.z);
        poly.Add(bottomRightDummy);

        //bottom left
        temp.x = topLeft.x; //refer again to orignal val
        temp.z = topLeft.z - Mathf.Abs(stepZ);
        Vector3 bottomLeftDummy = vertex.BuildVertexWithXandZ(temp.x, temp.z);
        poly.Add(bottomLeftDummy);

        return poly;
    }

    public static List<Vector3> CreateSubMeshPoly(Vector3 topLeft, int wideSquare = 1)
    {
        return CreatePoly(topLeft, m.SubDivide.XSubStep * wideSquare, m.SubDivide.ZSubStep * wideSquare, m.Vertex);
    }

    public static List<Vector3> ReturnWholeMallaAs1Poly(Vector3[] vertices)
    {   
        Vector3 topRight = vertices[0];
        Vector3 botLeft = vertices[vertices.Length - 1];
        Vector3 topLeft = new Vector3(botLeft.x, 0, topRight.z);
        Vector3 botRight = new Vector3(topRight.x, 0, botLeft.z);
        return new List<Vector3>() { topLeft, topRight, botRight, botLeft };
    }

    //the Vector3 list must have this order: NW, NE, SE, SW
    public static List<Vector3> ScalePoly(List<Vector3> polyPass, float by)
    {
        polyPass[0] = new Vector3(polyPass[0].x - by, polyPass[0].y, polyPass[0].z + by);
        polyPass[1] = new Vector3(polyPass[1].x + by, polyPass[1].y, polyPass[1].z + by);
        polyPass[2] = new Vector3(polyPass[2].x + by, polyPass[2].y, polyPass[2].z - by);
        polyPass[3] = new Vector3(polyPass[3].x - by, polyPass[3].y, polyPass[3].z - by);
        return polyPass;
    }

    public static Vector3[] ScalePoly(Vector3[] polyPass, float by)
    {
        polyPass[0] = new Vector3(polyPass[0].x - by, polyPass[0].y, polyPass[0].z + by);
        polyPass[1] = new Vector3(polyPass[1].x + by, polyPass[1].y, polyPass[1].z + by);
        polyPass[2] = new Vector3(polyPass[2].x + by, polyPass[2].y, polyPass[2].z - by);
        polyPass[3] = new Vector3(polyPass[3].x - by, polyPass[3].y, polyPass[3].z - by);
        return polyPass;
    }

    //the Vector3 list must have this order: NW, NE, SE, SW
    //this one is used for instances that dont need to ref the 'list' passed 
    public static List<Vector3> ScalePolyNewList(List<Vector3> polyPass, float by)
    {
        List<Vector3> newList= new List<Vector3>();
        newList.Add( new Vector3(polyPass[0].x - by, polyPass[0].y, polyPass[0].z + by));
        newList.Add( new Vector3(polyPass[1].x + by, polyPass[1].y, polyPass[1].z + by));
        newList.Add(  new Vector3(polyPass[2].x + by, polyPass[2].y, polyPass[2].z - by));
        newList.Add( new Vector3(polyPass[3].x - by, polyPass[3].y, polyPass[3].z - by));
        return newList;
    }

    //the Vector3 list must have this order: NW, NE, SE, SW
    public static List<Vector3> ScalePoly(List<Vector3> polyPass, float onX, float onZ)
    {
        polyPass[0] = new Vector3(polyPass[0].x - onX, polyPass[0].y, polyPass[0].z + onZ);
        polyPass[1] = new Vector3(polyPass[1].x + onX, polyPass[1].y, polyPass[1].z + onZ);
        polyPass[2] = new Vector3(polyPass[2].x + onX, polyPass[2].y, polyPass[2].z - onZ);
        polyPass[3] = new Vector3(polyPass[3].x - onX, polyPass[3].y, polyPass[3].z - onZ);
        return polyPass;
    }

    /// <summary>
    /// will return a poly from the subMesh, starting on top Left...
    /// 
    /// </summary>
    /// <param name="topLeft"></param>
    /// <returns></returns>
    public static List<Vector3> RetSubMeshPoly(Vector3 topLeft, List<Vector3> currentVertices, int wideSquare = 1)
    {
        List<Vector3> res = CreatePoly(topLeft, m.SubDivide.XSubStep * wideSquare,
           m.SubDivide.ZSubStep * wideSquare, m.Vertex);
        return res;
    }

    /// <summary>
    /// Creates a poly from param pos. with its dimensions
    /// </summary>
    public static List<Vector3> CreatePolyFromVector3(Vector3 pos, float xDim, float zDim)
    {
        List<Vector3> polyPass = new List<Vector3>();
        polyPass.Add( new Vector3(pos.x - xDim, pos.y, pos.z + zDim));
        polyPass.Add( new Vector3(pos.x + xDim, pos.y, pos.z + zDim));
        polyPass.Add(  new Vector3(pos.x + xDim, pos.y, pos.z - zDim));
        polyPass.Add( new Vector3(pos.x - xDim, pos.y, pos.z - zDim));
        return polyPass;
    }

    /// <summary>
    /// Will return the Rectangle as a poly on Terrain
    /// This doest nt return a Square Poly
    /// </summary>
    /// <param name="selection"></param>
    /// <returns></returns>
    public static List<Vector3> RetOnScreenPoly(Rect selection)
    {
        //invert Y first 
       // selection = U2D.ReturnRectYInverted(selection);

        Vector2 NWview = new Vector2(selection.xMin,  selection.yMax);
        Vector2 NEview = new Vector2(selection.xMax,  selection.yMax);
        Vector2 SEview = new Vector2(selection.xMax,  selection.yMin);
        Vector2 SWview = new Vector2(selection.xMin,  selection.yMin);

        Vector3 NW = RayCastTerrain(NWview).point;
        Vector3 NE = RayCastTerrain(NEview).point;
        Vector3 SE = RayCastTerrain(SEview).point;
        Vector3 SW = RayCastTerrain(SWview).point;

        return  new List<Vector3>(){NW, NE, SE, SW} ;
    }

    /// <summary>
    /// Will return poly on Terrain given the first two points and the direction the mouse
    /// was drageed on terrain
    /// </summary>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <returns></returns>
    public static List<Vector3> RetTerrainPoly(Vector3 one, Vector3 two, Dir dir)
    {
        Vector3 NW = new Vector3();
        Vector3 NE = new Vector3();
        Vector3 SE = new Vector3();
        Vector3 SW = new Vector3();

        if (dir == Dir.SWtoNE)
        {
            NW = m.Vertex.BuildVertexWithXandZ(one.x, two.z);
            NE = two;
            SE = m.Vertex.BuildVertexWithXandZ(two.x, one.z);
            SW = one;
        }
        else if (dir == Dir.SEtoNW)
        {
            NW = two;
            NE = m.Vertex.BuildVertexWithXandZ(one.x, two.z);
            SE = one;
            SW = m.Vertex.BuildVertexWithXandZ(two.x, one.z);
        }
        else if (dir == Dir.NWtoSE)
        {
            NW = one;
            NE = m.Vertex.BuildVertexWithXandZ(two.x, one.z);
            SE = two;
            SW = m.Vertex.BuildVertexWithXandZ(one.x, two.z);
        }
        else if (dir == Dir.NEtoSW)
        {
            NW = m.Vertex.BuildVertexWithXandZ(two.x, one.z);
            NE = one;
            SE = m.Vertex.BuildVertexWithXandZ(one.x, two.z);
            SW = two;
        }

        return new List<Vector3>() { NW, NE, SE, SW };
    }

    /// <summary>
    /// Return distance btw Vectors, distance in X and then Z
    /// </summary>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <returns></returns>
    public static List<float> RetDistances(Vector3 one, Vector3 two)
    {
        //distance X then Z
        List<float> res = new List<float>();
        
        res.Add(Mathf.Abs( one.x - two.x ));
        res.Add(Mathf.Abs(  one.z - two.z));
       
        return res;
    }

    /// <summary>
    /// Will ray cast to terrain (layer mask 8) from active camera will ret the raycast
    /// </summary>
    /// <param name="screenPoint"></param>
    /// <returns></returns>
    public static RaycastHit RayCastTerrain(Vector2 screenPoint)
    {
        CamControl mainCam = USearch.FindCurrentCamera();

        RaycastHit HitMouseOnTerrain = new RaycastHit();

        // Bit shift the index of the layer (8) to get a bit mask
        // This would cast rays only against colliders in layer 8.
        int layerMask = 1 << 8;
        // Does the ray intersect any objects in the layer 8 "Terrain Layer"
        if (Physics.Raycast(mainCam.transform.GetComponent<Camera>().ScreenPointToRay(screenPoint), out HitMouseOnTerrain,
            Mathf.Infinity, layerMask))
        {
        }
        else
        {
            Debug.Log("Mouse Did not Hit Layer 8: Terrain. UPoly.cs");
        }
        return HitMouseOnTerrain;
    }

    /// <summary>
    /// Will ray cast to layer , from active camera will ret the raycast
    /// </summary>
    /// <param name="screenPoint"></param>
    /// <returns></returns>
    public static RaycastHit RayCastLayer(Vector2 screenPoint, int layerNumb)
    {
        CamControl mainCam = USearch.FindCurrentCamera();
        RaycastHit HitMouse = new RaycastHit();

        int layerMask = 1 << layerNumb;
        if (Physics.Raycast(mainCam.transform.GetComponent<Camera>().ScreenPointToRay(screenPoint), out HitMouse,
            Mathf.Infinity, layerMask))
        {
        }
        else
        {
            //Debug.Log("Mouse Did not Hit Layer:"+layerNumb);
        }
        return HitMouse;
    }


    /// <summary>
    /// Will ray cast to all obj from active camera will ret the raycast
    /// </summary>
    /// <param name="screenPoint"></param>
    /// <returns></returns>
    public static RaycastHit RayCastAll(Vector2 screenPoint)
    {
        CamControl mainCam = USearch.FindCurrentCamera();
        RaycastHit HitMouse = new RaycastHit();

        if (Physics.Raycast(mainCam.transform.GetComponent<Camera>().ScreenPointToRay(screenPoint), out HitMouse,
            Mathf.Infinity))
        {
        }
        else
        {
            Debug.Log("Mouse Did not Hit any obj UPoly.cs");
        }
        return HitMouse;
    }

    

    public static List<Vector3> ReturnARealPoly(Vector3 hitVector3, Vector3[] Vertices, InitializerTerrain iniTerr,
        Vertexer vertex)
    {
        if (hitVector3 == null || hitVector3 == new Vector3())
        {
            return new List<Vector3>();
        }
        Vector3 firstVertex = vertex.FindClosestVertex(hitVector3, Vertices);

        Dir quadrant = vertex.FindVertexQuadrant(firstVertex, hitVector3);
        Vector3 lefTopVertex = vertex.FindTopLeftVertex(firstVertex, quadrant,
            iniTerr.StepX, iniTerr.StepZ, Vertices);
        //UVisHelp.CreateHelpers(lefTopVertex, Root.redSphereHelp);

        List<Vector3> poly = FindPoly(lefTopVertex, iniTerr.StepX, iniTerr.StepZ, vertex, Vertices);
        //UVisHelp.CreateHelpers(poly, Root.boxCollHelp);
        return poly;
    }

    //void CreateAndSubDivideARealPoly(Vector3 hitVector3)
    //{
    //    if (hitVector3 != null && hitVector3 != new Vector3())
    //    {
    //        Vector3 firstVertex = Vertex.FindClosestVertex(hitVector3, Vertices);

    //        D quadrant = Vertex.FindVertexQuadrant(firstVertex, hitVector3);
    //        Vector3 lefTopVertex = Vertex.FindTopLeftVertex(firstVertex, quadrant,
    //            iniTerr.StepX, iniTerr.StepZ, Vertices);
    //        UVisHelp.CreateHelpers(lefTopVertex, Root.redSphereHelp);

    //        List<Vector3> poly = Poly.FindPoly(lefTopVertex, iniTerr.StepX, iniTerr.StepZ, Vertex, Vertices);
    //        UVisHelp.CreateHelpers(poly, Root.boxCollHelp);
    //        List<Vector3> subDiv = subDivide.SubDividePoly(poly, 5, H.Tile);

    //        UVisHelp.CreateHelpers(subDiv, Root.blueSphereHelp);
    //    }
    //}

    //void RealPolysDetect(ref List<Vector3> realPolys, List<Vector3> objsHitTerrain)
    //{
    //    for (int i = 0; i < objsHitTerrain.Count; i++)
    //    {
    //        realPolys = UList.AddOneListToList(realPolys, Poly.ReturnARealPoly(objsHitTerrain[i], Vertices,
    //            iniTerr, Vertex));
    //    }
    //    realPolys = UList.EliminateDuplicatesByDist(realPolys, 0.001f);
    //    print(realPolys.Count + " realPolys.Count");
    //}

    /// <summary>
    /// Will displace polygon on the selected axis as far as the amount is 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="amount"></param>
    /// <param name="axis"></param>
    /// <returns></returns>
    List<Vector3> displacePoly(List<Vector3> list, float amount, H axis)
    {
        Vector3 temp = new Vector3();
        for (int i = 0; i < list.Count; i++)
        {
            if (axis == H.Z)
            {
                temp = list[i];
                temp.z += amount;
                list[i] = temp;
            }
        }
        return list;
    }

    //will even many polys in Y. this Method has a bug that 
    //put all vertices toghether
    public List<Vector3> EvenInYManyPolys(List<Vector3> manyPoly, ref Vector3[] vertices, ref bool isToEven,
        float maxHeightForEven, ref float minY)
    {
        float maxY = UMath.ReturnMax(UList.ReturnAxisList(manyPoly, H.Y));
        minY = UMath.ReturnMinimum(UList.ReturnAxisList(manyPoly, H.Y));
        float heightDiff = maxY - minY;
        if (heightDiff >= maxHeightForEven)
        {
            isToEven = false;
            return manyPoly;
        }

        isToEven = true;
        //print(heightDiff + " heightDiff");

        for (int i = 0; i < manyPoly.Count; i++)
        {
            Vector3 newModifiedYPos = manyPoly[i];
            newModifiedYPos.y = minY;
            manyPoly[i] = newModifiedYPos;
        }

        float epsilon = 0.1f;
        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = 0; j < manyPoly.Count; j++)
            {
                //if this are the same in X and Z we are updating 
                bool x = UMath.nearlyEqual(vertices[i].x, manyPoly[j].x, epsilon);
                bool z = UMath.nearlyEqual(vertices[i].z, manyPoly[j].z, epsilon);
                if (x && z)
                {   //updating the whole vertices matrix
                    vertices[i] = manyPoly[j];
                }
            }
        }
        return manyPoly;
    }

    //will even 1 poly  in Y.
    private List<Vector3> EvenInYPoly(List<Vector3> poly, ref Vector3[] vertices)
    {
        float maxY = UMath.ReturnMax(poly[0].y, poly[1].y, poly[2].y, poly[3].y);
        float minY = UMath.ReturnMinimum(poly[0].y, poly[1].y, poly[2].y, poly[3].y);
        float heightDiff = maxY - minY;

        //print(heightDiff + " heightDiff");

        for (int i = 0; i < poly.Count; i++)
        {
            Vector3 newModifiedYPos = poly[i];
            newModifiedYPos.y = minY;
            poly[i] = newModifiedYPos;
        }

        float epsilon = 0.1f;
        for (int i = 0; i < vertices.Length; i++)
        {
            for (int j = 0; j < poly.Count; j++)
            {
                //if this are the same in X and Z we are updating 
                bool x = UMath.nearlyEqual(vertices[i].x, poly[j].x, epsilon);
                bool z = UMath.nearlyEqual(vertices[i].z, poly[j].z, epsilon);
                if (x && z)
                {   //updating the whole vertices matrix
                    vertices[i] = poly[j];
                }
            }
        }
        return poly;
    }
}