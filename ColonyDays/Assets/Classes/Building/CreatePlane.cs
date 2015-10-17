using UnityEngine;
using System.Collections.Generic;

/*In this class we create and update a obj that usually is a block*/

public class CreatePlane : Building
{
    private Material _material;
    private float _raisedFromFloor;

    private Vector3 _scale = new Vector3();//implemented for when the planes are loaded 
    private bool _isAnInVisiblePlane;

    private GameObject _geometry;
    /// <summary>
    /// This is the gameObj that is rendered 
    /// </summary>
    public new GameObject Geometry
    {
        get { return _geometry; }
        set { _geometry = value; }
    }

    public Material Material
    {
        get { return _material; }
        set { _material = value; }
    }

    public float RaisedFromFloor
    {
        get { return _raisedFromFloor; }
        set { _raisedFromFloor = value; }
    }

    public Vector3 Scale
    {
        get { return _scale; }
        set { _scale = value; }
    }

    public bool IsAnInVisiblePlane
    {
        get { return _isAnInVisiblePlane; }
        set { _isAnInVisiblePlane = value; }
    }

    static public CreatePlane CreatePlan(string root, string materialRoot, Vector3 origen = new Vector3(), string name = "", Transform container = null,
    H hType = H.None, float raiseFromFloor = 0.09f, Material mat = null, Vector3 scale = new Vector3(), bool isAnInvisiblePlane = false)
    {
        WAKEUP = true;
        CreatePlane obj = null;
        obj = (CreatePlane)Resources.Load(root, typeof(CreatePlane));
        obj = (CreatePlane)Instantiate(obj, origen, Quaternion.identity);
        obj.HType = hType;
        obj.transform.name = obj.MyId = obj.Rename(obj.transform.name, obj.Id, obj.HType, name);
        obj.Scale = scale;

        if (mat == null)
        {
            obj.Material = (Material)Resources.Load(materialRoot);
        }
        else obj.Material = mat;

        
        obj.RaisedFromFloor = raiseFromFloor;
        obj.IsAnInVisiblePlane = isAnInvisiblePlane;

        if (container != null) { obj.transform.parent = container; }
        return obj;
    }



    public void AssignSharedMaterial(Material mat, string materialRoot)
    {
        if (mat == null)
        {
            Geometry.GetComponent<Renderer>().sharedMaterial = (Material)Resources.Load(materialRoot);
        }
        else Geometry.GetComponent<Renderer>().sharedMaterial = mat;


    }





    /// <summary>
    /// Initialize Material Color. Define initial color as Geometry.renderer.material.color
    /// </summary>
    void InitializeMatColors()
    {
        
        //InitialColor = Geometry.GetComponent<Renderer>().material.color;
    }

    void Awake()
    {
        GameObject loc = Resources.Load<GameObject>(Root.createPlaneUnit);
        _geometry = (GameObject) Instantiate(loc);
        _geometry.name = H.Geometry.ToString();

        _geometry.transform.parent = gameObject.transform;
        _geometry.GetComponent<Renderer>().enabled = false;//doing this bz was flashing when was created close to the cam in some place
        _geometry.GetComponent<Renderer>().castShadows = false;
    }

    protected void Start()
    {
        base.Start();
        Geometry.GetComponent<Renderer>().sharedMaterial = _material;
        
        //InitialColor = _material.color;

        //This is when the Plane is called from the loading fuction
        if (_scale != new Vector3() && _scale != null)
        {
            _geometry.transform.parent = null;
            _geometry.transform.position = transform.position;
            _geometry.transform.localScale = _scale; //it will work it if is initiated with a scale 

            if (!_isAnInVisiblePlane)
            {
                _geometry.GetComponent<Renderer>().enabled = true;
            }

            _geometry.transform.parent = transform;
            InitializeMatColors();
        }
        //else print("_scale was not initialize");
    }





	// Update is called once per frame
    void Update()
    {
    }

    //if is on the BigBox Class will correct minumulliy the scale for Big Box Prev Purposes
    protected bool corretMinimuScaleOnBigBox;

    /// <summary>
    /// routine to update position.. will get geometry out of current gameobject parenting and will
    /// place current game obj in middle of newVert. Will make geometry vertices same as 
    /// newVert.. then will make geomtry child of gameObject again 
    /// </summary>
    public void UpdatePos(List<Vector3> newVert, float blockthickNess = 0, bool makeThisInvisible = false,
        bool corretMinimuScaleOnBigBoxP = false)
    {
        //if is on the BigBox Class will correct minumulliy the scale for Big Box Prev Purposes
        corretMinimuScaleOnBigBox = corretMinimuScaleOnBigBoxP;

        if (blockthickNess == 0){blockthickNess = Program.gameScene.SubDivideBlockYVal;}

        _geometry.transform.parent = null;
        //updattinf pos of gameObj : To the middle btw NW and SE points
        gameObject.transform.position = (newVert[0] + newVert[2]) / 2;
        _geometry.transform.position = (newVert[0] + newVert[2]) / 2;

        newVert = RectifyScaleRoutine(newVert);

        UpdateGeometryScale(newVert, blockthickNess);
        _geometry.transform.parent = gameObject.transform;
        if (!_geometry.GetComponent<Renderer>().enabled && !_isAnInVisiblePlane)
        {
            _geometry.GetComponent<Renderer>().enabled = true;
        }

        if (makeThisInvisible)
        {
            _geometry.GetComponent<Renderer>().enabled = false;
            _isAnInVisiblePlane = true;
        }
    }

    #region Rectifys this(class) when are used on a Road or Trail so it looks seamleas when built.
    private float rectifyOnX = 0.01374f;
    private float rectifyOnZ = 0.0125f;

    public float RectifyOnX
    {
        get { return rectifyOnX; }
        set { rectifyOnX = value; }
    }

    public float RectifyOnZ
    {
        get { return rectifyOnZ; }
        set { rectifyOnZ = value; }
    }

    /// <summary>
    /// this is created so all of them are perfect dont overlap 
    /// </summary>
    List<Vector3> RectifyScaleRoutine(List<Vector3> polyP)
    {
        //to address when build shack that CurrentSpawnBuild is null
        if (BuildingPot.Control.CurrentSpawnBuild == null)
        {
            return RectifyPolyScale(H.None, polyP);
        }
        return RectifyPolyScale(BuildingPot.Control.CurrentSpawnBuild.HType, polyP);
    }
    /// <summary>
    /// Makes poly on way seamless on terrain. This has hard coded values
    /// </summary>
    List<Vector3> RectifyPolyScale(H hTypeP, List<Vector3> polyP)
    {
        float onX = RectifyOnX;
        float onZ = RectifyOnZ;

        if (hTypeP == H.Trail || hTypeP == H.BridgeTrail)
        {
            onX = RectifyOnX;
            onZ = RectifyOnZ;
        }
        if (/*hTypeP == H.Road ||*/ hTypeP == H.BridgeRoad)
        {
            onX *= 5;
            onZ *= 5;
        }
        return UPoly.ScalePoly(polyP, onX, onZ);
    }
    #endregion

    /// <summary>
    /// Updating the scale of the propertie scale
    /// </summary>
    void UpdateGeometryScale(List<Vector3> newVert, float blockthickNess = 0.1f)
    {
        float xDim = newVert[1].x - newVert[0].x ;
        float zDim = newVert[0].z - newVert[3].z;

        //Scaling
        Vector3 scale = _geometry.transform.localScale;
        scale.x = xDim;
        scale.y = blockthickNess;
        scale.z = zDim;
        _geometry.transform.localScale = scale;

        SaveScale(scale);
    }

    /// <summary>
    /// update the tileScale in trail so can be saved on disk
    /// </summary>
    void SaveScale(Vector3 scale)
    {
        if (BuildingPot.Control.CurrentSpawnBuild == null) { return;}
        Trail t = BuildingPot.Control.CurrentSpawnBuild as Trail;
        if (t == null) { return;}
        if (t.TileScale != scale) { t.TileScale = scale; }
        BuildingPot.Control.CurrentSpawnBuild = t;
    }

    #region Used When was creating a plane mesh procedural

    //this flips the plane facing... always has to be used so the planes can be seen from user camera
    Vector3[] flipPlaneVert(Vector3[] vert)
    {
        return new Vector3[] { vert[3], vert[2], vert[1], vert[0] };
    }

    //this is converting from sequence used on Terrain: topLeft, topRoght, botRight, leftRight ,
    //to local sequence: botLeft, botRught, topRight, topLeft
    Vector3[] translateFromTerrainSeqToLocal(List<Vector3> newVert)
    {
        return new Vector3[] { newVert[3], newVert[2], newVert[1], newVert[0] };
    }

    //will raised the vector3 on Y mainly the reason is to be able to see it to the user out
    //of the terrain
    Vector3[] raisedOnY(Vector3[] pass, float raiseAmt)
    {
        for (int i = 0; i < pass.Length; i++)
        {
            pass[i].y += raiseAmt;
        }
        return pass;
    }

    private static Mesh TangentSolver(Mesh theMesh)
    {
        int vertexCount = theMesh.vertexCount;
        Vector3[] vertices = theMesh.vertices;
        Vector3[] normals = theMesh.normals;
        Vector2[] texcoords = theMesh.uv;
        int[] triangles = theMesh.triangles;
        int triangleCount = triangles.Length / 3;
        Vector4[] tangents = new Vector4[vertexCount];
        Vector3[] tan1 = new Vector3[vertexCount];
        Vector3[] tan2 = new Vector3[vertexCount];
        int tri = 0;
        for (int i = 0; i < (triangleCount); i++)
        {
            int i1 = triangles[tri];
            int i2 = triangles[tri + 1];
            int i3 = triangles[tri + 2];

            Vector3 v1 = vertices[i1];
            Vector3 v2 = vertices[i2];
            Vector3 v3 = vertices[i3];

            Vector2 w1 = texcoords[i1];
            Vector2 w2 = texcoords[i2];
            Vector2 w3 = texcoords[i3];

            float x1 = v2.x - v1.x;
            float x2 = v3.x - v1.x;
            float y1 = v2.y - v1.y;
            float y2 = v3.y - v1.y;
            float z1 = v2.z - v1.z;
            float z2 = v3.z - v1.z;

            float s1 = w2.x - w1.x;
            float s2 = w3.x - w1.x;
            float t1 = w2.y - w1.y;
            float t2 = w3.y - w1.y;

            float r = 1.0f / (s1 * t2 - s2 * t1);
            Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
            Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

            tan1[i1] += sdir;
            tan1[i2] += sdir;
            tan1[i3] += sdir;

            tan2[i1] += tdir;
            tan2[i2] += tdir;
            tan2[i3] += tdir;

            tri += 3;
        }

        for (int i = 0; i < (vertexCount); i++)
        {
            Vector3 n = normals[i];
            Vector3 t = tan1[i];

            // Gram-Schmidt orthogonalize
            Vector3.OrthoNormalize(ref n, ref t);

            tangents[i].x = t.x;
            tangents[i].y = t.y;
            tangents[i].z = t.z;

            // Calculate handedness
            tangents[i].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[i]) < 0.0) ? -1.0f : 1.0f;
        }
        theMesh.tangents = tangents;
        return theMesh;
    }
    #endregion

}
