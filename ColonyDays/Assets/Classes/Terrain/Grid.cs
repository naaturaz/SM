using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    SMe m = new SMe();

    private Vector3[,] _myGrid;

    private float _xSubMeshStep;
    private float _zSubMeshStep;
    private int _rows;
    private int _col;
    private int _totalVectorsCount;

    private int vertexCounter;
    private int lotCounter;

    private int currentRow;
    private int currentCol;

    private bool iterateNow;

    public Grid() { }

    public Vector3[,] MyGrid
    {
        get { return _myGrid; }
        set { _myGrid = value; }
    }

    public void LoadGridRoutine()
    {
        if (_myGrid == null)
        {
            SetLocalVars();
        }
    }

    void SetLocalVars()
    {
        _xSubMeshStep = m.SubDivide.XSubStep;
        _zSubMeshStep = m.SubDivide.ZSubStep;
        _rows = (m.IniTerr.Rows) * 5;
        _col = (m.IniTerr.Columns) * 5;
        _totalVectorsCount = m.SubMesh.amountOfSubVertices;

        _myGrid = new Vector3[_rows, _col];
        iterateNow = true;

        GameScene.ScreenPrint("_xSubMeshStep:"+_xSubMeshStep);
        GameScene.ScreenPrint("_zSubMeshStep:" + _zSubMeshStep);
        GameScene.ScreenPrint("_rows:" + _rows);
        GameScene.ScreenPrint("_col:" + _col);
    }

    void Iterator()
    {
        for (int i = 0; i < 250; i++)
        {
            if (vertexCounter < m.AllVertexs.Count)
            {
                print("currentRow:" + currentRow);
                print("currentCol:" + currentCol);
                print("vertexCounter:" + vertexCounter);

                _myGrid[currentRow, currentCol] = m.AllVertexs[vertexCounter];

                //UVisHelp.CreateHelpers(_myGrid[currentRow, currentCol], Root.largeBlueCube);
                vertexCounter++;

                UpdateCurrents(m.AllVertexs, vertexCounter);
            }
            else iterateNow = false;
        }
    }

    public void SortThemOut(List<Vector3> list)
    {
        LoadGridRoutine();
        iterateNow = false;

        while (lotCounter < list.Count)
        {
            print("currentRow:" + currentRow);
            print("currentCol:" + currentCol);
            print("lotCounter:" + lotCounter);

            _myGrid[currentRow, currentCol] = list[lotCounter];

            //UVisHelp.CreateHelpers(_myGrid[currentRow, currentCol], Root.largeBlueCube);
            lotCounter++;

            if (lotCounter < list.Count)
            {
                UpdateCurrents(list, lotCounter);
            }
        }
        lotCounter = 0;
    }



    //void Iterator()
    //{
    //    if (vertexCounter < m.AllVertexs.Count)
    //    {
    //        print("currentRow:" + currentRow);
    //        print("currentCol:" + currentCol);
    //        print("vertexCounter:" + vertexCounter);

    //        _myGrid[currentRow, currentCol] = m.AllVertexs[vertexCounter];

    //        //UVisHelp.CreateHelpers(_myGrid[currentRow, currentCol], Root.blueSphereHelp);
    //        vertexCounter++;

    //        UpdateCurrents();
    //    }
    //    else iterateNow = false;
    //}

    void UpdateCurrents(List<Vector3> list, int countP)
    {
        string msg = "";

        //Dir movedTo = UDir.ItMovedTowards(m.AllVertexs[countP - 1], m.AllVertexs[countP]);

        float movedOnX = list[countP - 1].x - list[countP].x;
        float movedOnZ = list[countP - 1].z - list[countP].z;

        double floatColChange = Math.Round( movedOnX/_xSubMeshStep, 0);
        double floatRowChange = Math.Round( movedOnZ/_zSubMeshStep, 0);//bz we are inverting the Y in the array from world cord

        int currentColChange = (int)floatColChange * -1;
        int currentRowChange = (int) floatRowChange * -1;

        //msg ="movedTo:" + "movedTo" + "\n" +
        //    "movedOnX:" + movedOnX + "\n" +
        //    "movedOnZ:" + movedOnZ + "\n" +
        //     "currentColChange:" + currentColChange + "\n" +
        //     "currentRowChange:" + currentRowChange + "\n" 
        //    ;

        //print(msg);

        currentCol += currentColChange;
        currentRow += currentRowChange;

        //ChangeCurrents(movedTo, currentColChange, currentRowChange);
    }

    /// <summary>
    /// </summary>
    void ChangeCurrents(Dir movedTo, int currentColChange, int currentRowChange)
    {
        if (movedTo == Dir.S)
        {
            currentRow += currentRowChange;
            return;
        }
        if (movedTo == Dir.E)
        {
            currentCol += currentColChange;
            return;
        }
        if (movedTo == Dir.W)
        {
            currentCol -= currentColChange;
            return;
        }
       
        if (movedTo == Dir.N)
        {
            currentRow -= currentRowChange;
            return;
        }


        if (movedTo == Dir.NE)
        {
            currentCol += currentColChange;
            currentRow -= currentRowChange;
            return;
        }
        if (movedTo == Dir.NW)
        {
            currentCol -= currentColChange;
            currentRow -= currentRowChange;
            return;
        }
        if (movedTo == Dir.SE)
        {
            currentCol += currentColChange;
            currentRow += currentRowChange;
            return;
        }
        if (movedTo == Dir.SW)
        {
            currentCol -= currentColChange;
            currentRow += currentRowChange;
            return;
        }
 
    }




    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () 
    {
	    if (iterateNow)
	    {
	        
            Iterator();
	    }
	}
}
