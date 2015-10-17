using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubDivider : General
{
    private float _xSubStep, _zSubStep;

    //how far in X a in subdivided Polygon is
    public float XSubStep
    {
        get { return _xSubStep; }
        set{_xSubStep = value;}
    }

    /// <summary>
    /// how far in Z a in subdivided Polygon is
    /// in this proj this value is neg so u need to do math.abs()
    /// </summary>
    public float ZSubStep
    {
        get { return Mathf.Abs(_zSubStep); }
        set{ _zSubStep = value;}
    }

    /// <summary>
    /// Subdive a lot. Will move the ref Vector3 start to the end of 
    ///  the lot and will send the value bak to the caller of this method
    /// </summary>
    /// <param name="start">Start point of the lot... will be ref back here</param>
    /// <param name="polyX">how many poly on X</param>
    /// <param name="polyZ">how many ppoly oin Z</param>
    /// <param name="stepX">how far in X a real vertices is from another</param>
    /// <param name="stepZ">how far in Z a real vertices is from another</param>
    /// <param name="inPolyDiv">in poly divisions</param>
    /// <param name="vertices">all mesh vertices[]</param>
    /// <returns>a Vector3 list with the lot</returns>
    public List<Vector3> SubDivideLot(ref Vector3 start, int polyX, int polyZ,
        float stepX, float stepZ, int inPolyDiv, Vector3[] vertices)
    {
        float localStepZ = 0;
        List<Vector3> lot = new List<Vector3>();
        Vector3 temp = start;
        Vector3 real = new Vector3();
        for (int i = 0; i < polyZ; i++)
        {
            temp = start;
            temp.z = start.z + localStepZ;
            //to make sure we are on top of a real vertex
            Vector3 realDummy = m.Vertex.BuildVertexWithXandZ(temp.x, temp.z);
            real = m.Vertex.FindClosestVertex(realDummy, vertices);

            for (int j = 0; j < polyX; j++)
            {
                List<Vector3> poly = returnPoly(real, vertices);
                List<Vector3> newLot = SubDividePoly(poly, inPolyDiv, H.Tile);

                CheckIfStopScanning(newLot, polyX, polyZ, start);

                lot = UList.AddOneListToList(lot, newLot);
                temp.x = real.x + Mathf.Abs(stepX);

                realDummy = m.Vertex.BuildVertexWithXandZ(temp.x, temp.z); //

                //in the last row has to jump in Z: temp.z - Mathf.Abs(stepZ))
                if (j == polyX - 1)
                {
                    //to make sure we are on top of a real vertex
                    realDummy = m.Vertex.BuildVertexWithXandZ(temp.x, temp.z - Mathf.Abs(stepZ)); //
                }
                real = m.Vertex.FindClosestVertex(realDummy, vertices);
            }
            //with multoplicacton was loosing big time precision...
            //that why im adding here the values... with floats avoid multiplcations
            localStepZ = localStepZ + stepZ;
        }
        start = real;
        //lot = UList.EliminateDuplicatesByDist(lot, 0.01f);
        //print(lot.Count + ".lot.count");
        return lot;
    }

    //use only now to show last row of ScanTerra() elements
    void CheckIfStopScanning(List<Vector3> newLot, int polyX, int polyZ, 
        Vector3 start)
    {
        if (newLot.Count < polyX * polyZ)
        {
            UVisHelp.CreateHelpers(start, Root.blueCubeBig);
        }
    }

    //this is a method helper
    private List<Vector3> returnPoly(Vector3 real, Vector3[] vertices)
    {
        List<Vector3> poly = UPoly.FindPoly(real, m.IniTerr.StepX, m.IniTerr.StepZ, m.Vertex, vertices);
        return poly;
    }

    // Full,//this will use create object everywere in the poly
    //Tile//this one will only fill so the next poly doesnt create any obj was created on this one 
    //already
    public List<Vector3> SubDividePoly(List<Vector3> poly, int divs, H fillStyle)
    {
        List<Vector3> filled = new List<Vector3>();
        List<Vector3> topLine = SubDivideLine(poly, 0, 1, divs, H.X);
        List<Vector3> leftLine = SubDivideLine(poly, 1, 2, divs, H.Z);

        if (fillStyle == H.Full)
        {
            List<Vector3> botLine = SubDivideLine(poly, 2, 3, divs, H.X);
            List<Vector3> rightLine = SubDivideLine(poly, 3, 0, divs, H.Z);
            filled = UList.AddManyListToList(botLine, rightLine, leftLine);
        }
        List<Vector3> fill = FillPolygonWrite(topLine, leftLine);

        filled = UList.AddManyListToList(filled, topLine, leftLine);
        filled = UList.AddOneListToList(filled, fill);
        float dist = 0.001f;
        //filled = UList.EliminateDuplicatesByDist(filled, dist);
        filled = filled.Distinct().ToList();
        //print(filled.Count + "filled");
        return filled;
    }

    List<Vector3> SubDivideLine(List<Vector3> poly, int iniPolyIndex, int endPolyIndex,
     int numberDivs, H axis)
    {
        float lenght = 0;
        List<Vector3> list = new List<Vector3>();
        float localZSteps = 0;

        if (axis == H.X)
        {
            lenght = poly[endPolyIndex].x - poly[iniPolyIndex].x;
            XSubStep = lenght / numberDivs;
        }
        else if (axis == H.Z)
        {
            lenght = poly[endPolyIndex].z - poly[iniPolyIndex].z;
            localZSteps = lenght / numberDivs;

            //if was setep already not override it to zero
            //that happens when we are closing on the scanning process at the
            //end of the mesh
                if (lenght / numberDivs != 0 && ZSubStep == 0)
                {
                    ZSubStep = lenght / numberDivs;
                }
        }

        float yTemp = FindYValueOnTerrain(poly[iniPolyIndex].x, poly[iniPolyIndex].z);
        list.Add(new Vector3(poly[iniPolyIndex].x, yTemp, poly[iniPolyIndex].z));//1st div
        for (int i = 1; i < numberDivs; i++)
        {
            //2nd div and up
            if (axis == H.X)
            {
                float y = FindYValueOnTerrain(poly[iniPolyIndex].x + XSubStep * i, poly[iniPolyIndex].z);
                list.Add(new Vector3(poly[iniPolyIndex].x + XSubStep * i, y, poly[iniPolyIndex].z));
            }
            else if (axis == H.Z)
            {
                float y = FindYValueOnTerrain(poly[iniPolyIndex].x, poly[iniPolyIndex].z + localZSteps * i);
                list.Add(new Vector3(poly[iniPolyIndex].x, y, poly[iniPolyIndex].z + localZSteps * i));
            }
        }
        //list.Add(poly[endPolyIndex]);//last div
        return list;
    }

    /// Fill the inside of a polygon given the topLine and leftLine vectors3
    List<Vector3> FillPolygonWrite(List<Vector3> topLine, List<Vector3> leftLine)
    {
        List<Vector3> fill = new List<Vector3>();
        for (int indexX = 1; indexX < leftLine.Count; indexX++)
        {
            for (int i = 1; i < topLine.Count; i++)
            {
                float y = FindYValueOnTerrain(topLine[indexX].x, leftLine[i].z);
                fill.Add(ReturnFilledPos(topLine, leftLine, indexX, y, i));
            }
        }
        return fill;
    }

    Vector3 ReturnFilledPos(List<Vector3> topLine, List<Vector3> leftLine,
        int indexX, float y, int i)
    {
        return new Vector3(topLine[indexX].x, y, leftLine[i].z);
    }

    /// <summary>
    /// Will draw a ray agaist terrain and will find Y value
    /// </summary>
    public float FindYValueOnTerrain(float x, float z)
    {
        //cant modify this numbers wihout having ScanTerra() behaving weard
        //origin Y at 100f and ratcast down to infinity is needed

        Vector3 origin = new Vector3(x, 100f, z);
        Vector3 startingRay = origin + Vector3.down * 2;
        Debug.DrawRay(startingRay, Vector3.down * 100f, Color.blue);

        float y = 0;
        RaycastHit hit;
        // Bit shift the index of the layer (8) to get a bit mask
        // This would cast rays only against colliders in layer 8.
        int layerMask = 1 << 8;
        // Does the ray intersect any objects in the layer 8 "Terrain Layer"
        if (Physics.Raycast(origin, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            y = hit.point.y;
        }
        //else{print("FindYValue() didnt hit anytihg on layer 8: Terrain");}
        return y;
    }
}