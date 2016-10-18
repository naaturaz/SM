using UnityEngine;
using System.Collections;

public class LineRender : General
{
    private Vector3 vector1;
    private Vector3 vector2;
    private Person _person;
    private H _type;//type of line home ex: will show line to home
    private MeshFilter _mesh;

    private float _withd = .2f;

    public Person Person1
    {
        get { return _person; }
        set { _person = value; }
    }

    public H Type
    {
        get { return _type; }
        set { _type = value; }
    }

    public static LineRender CreateLineRender(Person pers, H type)
    {
        LineRender obj = null;
        obj = (LineRender)Resources.Load("Prefab/GUI/3dHelpers/LineRender", typeof(LineRender));
        obj = (LineRender)Instantiate(obj, new Vector3(), Quaternion.identity);
        obj.transform.name = "LineRender";
        obj.Person1 = pers;
        obj.Type = type;

        //if (container != null) { obj.transform.SetParent(container); }
        return obj;
    }
    
    void Start()
    {
        var render = Geometry.GetComponent<MeshRenderer>();
        //_mesh = render.
        //SetPositions();

        //for (int i = 0; i < _mesh.mesh.vertices.Length; i++)
        //{
        //    UVisHelp.CreateText(_mesh.mesh.vertices[i], i + "");
        //}

    }
    
    void Update()
    {
        if (_person == null)
        {
            return;
        }

        vector1 = _person.transform.position;
        if (_type == H.Home)
        {
            vector2 = _person.Home.transform.position;
        }
        SetPositions();
    }

    void SetPositions()
    {
        _mesh.mesh.vertices[0] = new Vector3(vector1.x + _withd, vector1.y, vector1.z + _withd);
        _mesh.mesh.vertices[3] = new Vector3(vector1.x - _withd, vector1.y, vector1.z - _withd);
        
        _mesh.mesh.vertices[2] = new Vector3(vector2.x + _withd, vector2.y, vector2.z + _withd);
        _mesh.mesh.vertices[1] = new Vector3(vector2.x - _withd, vector2.y, vector2.z - _withd);

        
    }
}