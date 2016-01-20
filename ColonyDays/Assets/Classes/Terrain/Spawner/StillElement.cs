using UnityEngine;
using System.Collections.Generic;

public class StillElement : TerrainRamdonSpawner {
    private Vector3 _min;
    private Vector3 _max;
    private bool addCrystals;

    //this is not saveLoad
    private float _weight;//the weight of the element //

    //says how many foreseter are mining this now
    private int _minedNowBy; 

    List<Vector3> _anchors = new List<Vector3>();

    /// <summary>
    /// the height of the elements they start always with 1. if a tree is replanted is 
    /// set to zero and has to reegrow
    /// </summary>
    public float _height = .25f;

    private float _maxHeight;


    public List<Vector3> Anchors
    {
        get { return _anchors; }
        set { _anchors = value; }
    }

    public int MinedNowBy
    {
        get { return _minedNowBy; }
        set { _minedNowBy = value; }
    }

    public float Height
    {
        get { return _height; }
        set { _height = value; }
    }

    // Use this for initialization
	protected void Start ()
	{
	    addCrystals = true;
        base.Start();//intended to call TerrainRandomSpawner.cs

	    if (ReplantedTree)
	    {
	        ReplantThisTree();
	    }

	    LoadGrowingTree();
	}

    /// <summary>
    /// To address the loading of a growing tree
    /// </summary>
    private void LoadGrowingTree()
    {
        if (!ReadyToMine())
        {
            ScaleGameObjectToZero();
            SetGameObjectScaleTo(Height);
        }
    }

    void AddCrystals()
    {
        //ornaments and grass wont be added 
        if (name.Contains("Orna") || name.Contains("Grass"))
        {
            return;
        }

        UpdateMinAndMaxVar();
        var bou = FindBounds(_min, _max);
        Anchors = FindAnchors(bou);

        MeshController.CrystalManager1.Add(this);
    }

	// Update is called once per frame
	protected void Update () 
    {
	    if (addCrystals && MeshController.CrystalManager1.CrystalRegions.Count>0)
	    {
	        addCrystals = false;
            AddCrystals();
	    }

        CheckIfCanGrow();
	}

    protected void UpdateMinAndMaxVar()
    {
        _min = gameObject.transform.GetComponent<Collider>().bounds.min;
        _max = gameObject.transform.GetComponent<Collider>().bounds.max;
        //UVisHelp.CreateText(_min, "min", 60);
        //UVisHelp.CreateText(_max, "max", 60);
    }

    /// <summary>
    ///will find the farest point in a gameObj is lokking for the NW, NE, SE, SW . will retu
    ///in that sequence. 
    /// </summary>
    /// <param name="min">Bound.min, will work to if we pass SW</param>
    /// <param name="max">Bound.max, will work to if we pass NE</param>
    /// <returns>a List Vector3 wit sequence: NW, NE, SE, SW</returns>
    protected List<Vector3> FindBounds(Vector3 min, Vector3 max)//
    {
        float yMed = (min.y + max.y) / 2;
        Vector3 NW = new Vector3(min.x, yMed, max.z);
        Vector3 NE = new Vector3(max.x, yMed, max.z);
        Vector3 SE = new Vector3(max.x, yMed, min.z);
        Vector3 SW = new Vector3(min.x, yMed, min.z);
        List<Vector3> res = new List<Vector3>() { NW, NE, SE, SW };
        //UVisHelp.CreateHelpers(NW, Root.redSphereHelp);
        //UVisHelp.CreateHelpers(SE, Root.redSphereHelp);
        //UVisHelp.CreateHelpers(res, Root.blueCubeBig);
        return res;
    }

    /// <summary>
    /// Find where this point are hitting the ground
    /// </summary>
    protected List<Vector3> FindAnchors(List<Vector3> list)
    {
        List<Vector3> res = new List<Vector3>();
        for (int i = 0; i < list.Count; i++)
        {
            res.Add(m.Vertex.BuildVertexWithXandZ(list[i].x, list[i].z));
        }
        //UVisHelp.CreateHelpers(res, Root.blueCube);
        return res;
    }

    public override void DestroyCool()
    {
        //cool stuff

        //remove from CrystalManager
        MeshController.CrystalManager1.Delete(this);

        base.DestroyCool();

        //removes from List in TerraSpawnerController
        Program.gameScene.controllerMain.TerraSpawnController.RemoveStillElement(this);
    }

    /// <summary>
    /// So a still element is not mined endessly this 
    /// gives the elements a weight so when is depleted is deleted 
    /// and foreseted can move to next item
    /// </summary>
    internal void SetWeight()
    {
        //was set already
        if (_weight != 0)
        {
            return;
        }


        SetSpecWeight();
    }

    private void SetSpecWeight()
    {
        if (HType.ToString().Contains("Tree"))
        {
            _weight = Random.Range(1, 5);
            _weight = 5;
        }
        else//ore. stone
        {
            _weight = Random.Range(150, 200);
            _weight = 5;
        }
    }

    /// <summary>
    /// Called when  a Forester iis has mined 
    /// </summary>
    /// <param name="ProdXShift"></param>
    internal void RemoveWeight(float ProdXShift, Person pers)
    {
        _weight -= ProdXShift;
        
        //depleted
        //mined now only by one person . The person calling this Method 
        if (_weight < 0)
        {
            DestroyCool();

            if (HType==H.Tree)
            {
                Program.gameScene.controllerMain.TerraSpawnController.
                    SpawnRandomTreeAroundThisPos(pers);
            }
        }
    }

    #region Grow
    //when was seeded
    private MDate _seedDate;
    private int _lifeDuration = 1800;//5 years to be fully grown 
    public MDate SeedDate
    {
        get { return _seedDate; }
        set { _seedDate = value; }
    }

    public float MaxHeight
    {
        get { return _maxHeight; }
        set { _maxHeight = value; }
    }


    void ReplantThisTree()
    {
        if (HType != H.Tree)
        {
            return;
        }

        _seedDate = Program.gameScene.GameTime1.CurrentDate();
        Height = 0;
        _maxHeight = gameObject.transform.localScale.y;
        ScaleGameObjectToZero();

        Program.gameScene.controllerMain.TerraSpawnController.ReSaveStillElement(this);
    }

    void CheckIfCanGrow()
    {
        if (_seedDate==null)
        {
            return;
        }

        var timeInSoil = Program.gameScene.GameTime1.ElapsedDateInDaysToDate(_seedDate);
        float advance = (float)timeInSoil / (float)_lifeDuration;

        if (advance > Height)
        {
            GrowPlantNow();
        }
    }

    /// <summary>
    /// So the grows looks cool and smooth
    /// </summary>
    private void GrowPlantNow()
    {
        if (Height > _maxHeight)
        {
            return;
        }

        Height += 0.01f;

        ScaleGameObject(0.01f);
        Program.gameScene.controllerMain.TerraSpawnController.ReSaveStillElement(this);
    }

    void ScaleGameObject(float toAdd)
    {
        var localScale = gameObject.transform.localScale;
        var singleX = localScale.x + toAdd;
        var singleY = localScale.y + toAdd;
        var singleZ = localScale.z + toAdd;

        var newScale = new Vector3(singleX, singleY, singleZ);
        gameObject.transform.localScale = newScale;
    }

    void SetGameObjectScaleTo(float val)
    {
        var newScale = new Vector3(val, val, val);
        gameObject.transform.localScale = newScale;
    }  
    
    void ScaleGameObjectToZero()
    {
        var newScale = new Vector3(0, 0, 0);
        gameObject.transform.localScale = newScale;
    }

    internal bool ReadyToMine()
    {
        return Height >= MaxHeight;
    }
    #endregion
}
