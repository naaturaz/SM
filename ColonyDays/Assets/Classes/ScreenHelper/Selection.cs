using UnityEngine;
using System.Collections;

public class Selection : General {

    int _indexAllVertex;

    public int IndexAllVertex
    {
        get { return _indexAllVertex; }
        set { _indexAllVertex = value; }
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int AllVertexIndex;//this is the correspondent allvertex index

    static public Selection CreateSelection(string root, Vector3 origen, int indexAllVertex,
       string name = "", Transform container = null)
    {
        WAKEUP = true;
        Selection obj = null;
        obj = (Selection)Resources.Load(root, typeof(Selection));
        obj = (Selection)Instantiate(obj, origen, Quaternion.identity);
        if (name != "") { obj.name = name; }
        if (container != null) { obj.transform.parent = container; }
        obj.IndexAllVertex = indexAllVertex;
        return obj;
    }
}
