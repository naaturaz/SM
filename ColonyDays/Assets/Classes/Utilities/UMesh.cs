using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class UMesh : MonoBehaviour {

 

    /// <summary>
    /// 
    /// </summary>
    /// <param name="objPos"></param>
    /// <param name="lotStart">NW</param>
    /// <param name="lotEnd">SE</param>
    /// <returns></returns>
    public static bool Contains(Vector3 objPos, Vector3 lotStart, Vector3 lotEnd)
    {
        if (objPos.x >= lotStart.x && objPos.x <= lotEnd.x &&
            objPos.z <= lotStart.z && objPos.z >= lotEnd.z)
        {
            return true;
        }
        return false;
    }

    public static bool Contains(Vector3 objPos, List<Vector3> poly)
    {
        if (Contains(objPos, poly[0], poly[2]))
        {
            return true;
        }
        return false;
    }

    public static int ReturnInitialColOrRow(int colOrRow)
    {
        int iniColMult = -1;
        if (colOrRow == 1)
        {
            iniColMult = colOrRow;
        }
        else if (colOrRow % 2 == 1)
        {
            iniColMult = ((colOrRow - 1) / 2);
        }
        else
        {
            iniColMult = colOrRow / 2;
        }
        return iniColMult;
    }

    public static List<Vector3> ReturnThePos(Vector3 objPosCenter, float lotStepX, float lotStepZ, int col, int row)
    {
        List<Vector3> list = new List<Vector3>();
        int iniColMult = ReturnInitialColOrRow(col);
        int iniRowMult = ReturnInitialColOrRow(row);

        //print(iniColMult + "iniColMult");
        //print(iniRowMult + "iniRowMult");

        float iniX = objPosCenter.x - lotStepX * iniColMult;
        float iniZ = objPosCenter.z + lotStepZ * iniRowMult;
        Vector3 ini = new Vector3(iniX, objPosCenter.y, iniZ);
        Vector3 temp = ini;

        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                list.Add(temp);
                temp.z -= lotStepZ;
            }
            temp.x += lotStepX;
            temp.z = ini.z;
        }
        return list;
    }

    public static List<int> ReturnIndexesContain(List<Vector3> posList, List<Lot> squares)
    {
        List<int> list = new List<int>();

        for (int i = 0; i < squares.Count; i++)
        {
            for (int j = 0; j < posList.Count; j++)
            {
                if (Contains(posList[j], squares[i].squareStart, squares[i].squareEnd))
                {
                    list.Add(squares[i].IndexOfCurrentLotBeingHover(posList[j]));
                }
            }
        }
        
        return list;
    }

    public static List<int> ReturnIndexesContainDistinct(List<Vector3> posList, List<Lot> squares)
    {
        List<int> list = new List<int>();

        for (int i = 0; i < squares.Count; i++)
        {
            for (int j = 0; j < posList.Count; j++)
            {
                if (Contains(posList[j], squares[i].squareStart, squares[i].squareEnd))
                {
                    list.Add(squares[i].IndexOfCurrentLotBeingHover(posList[j]));
                }
            }
        }
        list = list.Distinct().ToList();
        return list;
    }

    public static List<Vector3> ReturnCurrentLotsVertex(List<int> currentIndexes, List<Lot> squares)
    {
        List<Vector3> vertexs = new List<Vector3>();

        for (int i = 0; i < currentIndexes.Count; i++)
        {
            vertexs = UList.AddOneListToList(vertexs, squares[currentIndexes[i]].LotVertices);
        }
        return vertexs;
    }
}