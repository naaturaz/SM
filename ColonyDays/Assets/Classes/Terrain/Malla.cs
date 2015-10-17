using System.Collections.Generic;
using UnityEngine;

public class Malla : General
{

	public static Mesh mesh = null;
	Vector3[] vertices = null;
	Vector3[] normals = null;
	int[] triangles = null;

    private int _xLotColumns;
    private int _zLotRows;

    public bool IsXLotColumnsLocked = false;
    public bool IsZLotRowsLocked = false;

    public List<Lot> Lots = new List<Lot>(); 

    public int XLotColumns
    {
        get { return _xLotColumns; }
        set
        {
            if(!IsXLotColumnsLocked)
            _xLotColumns = value;
        }
    }

    public int ZLotRows
    {
        get { return _zLotRows; }
        set
        {
            if (!IsZLotRowsLocked)
            _zLotRows = value;
        }
    }

    // Use this for initialization
	void Start () {}	
	
	// Update is called once per frame
	void Update () 
	{
		//if(Input.GetKeyUp("h")){SpawnHelp();}
		if(Input.GetMouseButtonUp (0)){}
		if(Input.GetMouseButtonUp (1)){}
	}

	void SpawnHelp()
	{
		GameObject cubo = (GameObject)Resources.Load("Prefab/Misc/RefMeshCube", typeof(GameObject));
		GameObject texto = (GameObject)Resources.Load("Prefab/Misc/3dText", typeof(GameObject));
		
		for (int i = 0; i < vertices.Length; i++) 
		{
			GameObject cloneCubo = Instantiate(cubo, vertices[i], Quaternion.identity) as GameObject;
			GameObject cloneTexto = Instantiate(texto, vertices[i], Quaternion.identity) as GameObject;
			cloneCubo.name = i.ToString();
			cloneTexto.name = "Texto: " + i.ToString();
			cloneTexto.GetComponent<TextMesh>().text = i.ToString();
		}
        print(vertices.Length + " Length ");
	}
}