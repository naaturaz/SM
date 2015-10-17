using System;
using UnityEngine;
using System.Collections.Generic;

//this function as a Dragabble square the farming functions are not use at all. used to be the Farm.cs
public class DragSquare : Trail
{
    private bool _isFarmOk;//will say if a farm is ok to be built in the position

    List<Vector3> soil = new List<Vector3>();//the initial center point of each tile in the farm soil
    List<CreatePlane> _planesSoil = new List<CreatePlane>();
    BigBoxPrev farmPrev;

    int maxSizeOfFarm = 300;//max size of a farm 
    private float minSideLenght = 3.5f;//min side of a farm

    private int counter;//this is the counter of enlargements of the PreviewFarm

    private float reduceOnXLocalOnScreenPoly = -0.12f;

    //this is the flag will allow CreatePlanesRoutine() executed if true...
    private bool createSoilNow;
    //used as index when creating planes for the farm
    private int loopCounter;

    public List<CreatePlane> PlanesSoil
    {
        get { return _planesSoil; }
        set { _planesSoil = value; }
    }

    /// <summary>
    /// If BuildIsOk, Farm is not colliding, not to small or big then IsFarmOk = true
    /// </summary>
    public bool IsFarmOk
    {
        get { return _isFarmOk; }
        set { _isFarmOk = value; }
    }

    public new void Drag()
    {
        if (ClosestSubMeshVert != ClosestVertOld)
        {
            counter=0;
        }

        base.Drag();
        
        if (OnScreenPoly.Count > 0 && counter==0)
        {
            UpdateFarmPrev();
        }
    }

    /// <summary>
    /// This method handles and deals with the if is to big or if a side is to small
    /// Then calls the method that Colors the Preview 
    /// </summary>
    void ChecksCollSizeCallsColor()
    {
        //doing this bz the poly on X and Z really in real term will collide with next one that why
        //is reduced a bit so it can allow right next ones to be placed
        List<Vector3> localOnScreenPoly = UPoly.ScalePoly(OnScreenPoly, reduceOnXLocalOnScreenPoly);

        List<Vector3> localSoilList = RetuFillPolyRealY(localOnScreenPoly[0], localOnScreenPoly[2], Mathf.Abs(m.SubDivide.XSubStep), 
            Mathf.Abs(m.SubDivide.ZSubStep), true);
        
        //too big
        bool isToBig = false;
        if (localSoilList.Count > maxSizeOfFarm){isToBig = true;}

        //too small
        bool isToSmall = false;
        if (!isToBig)
        {
            float xDiff = Mathf.Abs(localOnScreenPoly[0].x - localOnScreenPoly[1].x);
            float zDiff = Mathf.Abs(localOnScreenPoly[0].z - localOnScreenPoly[3].z);
            if (xDiff < minSideLenght || zDiff < minSideLenght)
            {
                if (HType != H.Road)//this means that road can have any small size
                {isToSmall = true;}
            }
        }

        //is colliding 
        bool isColliding = false;
        if (!isToBig && !isToSmall)
        { isColliding = BuildingPot.Control.Registro.IsCollidingWithExisting(localOnScreenPoly); }

        bool isEvenFarm = false;
        if (!isColliding && !isToBig && !isToSmall)
        { isEvenFarm = CheckIfIsEven(localSoilList, 0.01f); }

        //only using isEvenFarm bz if any bfeore is false. EvenFarm will be false too
        SetFarmOkAndHandleColor(isEvenFarm);
    }

    /// <summary>
    /// Sets the _isFarmOk bool and Handles the color of the preview
    /// </summary>
    void SetFarmOkAndHandleColor(bool condition)
    {
        if (IsBuildOk && condition)
        {
            farmPrev.Geometry.GetComponent<Renderer>().material.color = farmPrev.InitialColor;
            IsFarmOk = true;
        }
        else if (!IsBuildOk || !condition)
        {
            farmPrev.Geometry.GetComponent<Renderer>().material.color = Color.red;
            IsFarmOk = false;
        }
    }

    /// <summary>
    /// Called from InputBuilder.cs when the user clicked to set the farm on the spot 
    /// </summary>
    public void FinishPlacingFarm()
    {
        DestroyProjector();
        ClearPrevWay();
        DestroyPreviews();

        CreateFarmSoil();
        AddFarmToRegistro();

        if (HType == H.StockPile)
        {
            AddBoxCollider(BuildingPot.Control.Registro.AllRegFile[BuildingPot.Control.Registro.AllRegFile.Count - 1]);
        }

        MarkTerraSpawnRoutine(2f, soil);
        BuildingPot.InputU.DoneFarmRoutine();
    }

    void AfterLoopRoutine()
    {
        PositionFixed = true;
    }

    /// <summary>
    /// Destroy the farm preview and the ways preview
    /// </summary>
    public void DestroyPreviews()
    {
        farmPrev.DestroyCoolMoveFirst(H.Y, destroyCoolSpeed, destroyCoolTime);
        //needs to be destroy too bz they are spawned on Way.cs
        DestroyBigPrevBoxes();
    }

    /// <summary>
    /// This is how I send the farm to registro that later if user wants will be saved on file
    /// </summary>
    void AddFarmToRegistro()
    {
        //doing this bz the poly on X is a bit off
        List<Vector3> localOnScreenPoly = UPoly.ScalePoly(OnScreenPoly, reduceOnXLocalOnScreenPoly);
        var middleOfGameObj = MiddlePos(localOnScreenPoly);

        //this is the call when it add the collider rectangle to the world and save the RegFile in Registro
        BuildingPot.Control.Registro.AddBuildToAll(MyId, HType, localOnScreenPoly, Category, middleOfGameObj, 
            Inventory,  
            PeopleDict,
            planesOnAirPos: soil, tileScale: Program.gameScene.ScaleSmallRoadUnitFarm);
    }

    /// <summary>
    /// Need to find the middile pos of gameobj so when BoxCollider is added is on center of it
    /// </summary>
    Vector3 MiddlePos(List<Vector3> polyP)
    {
        var x = (polyP[0].x + polyP[1].x)/2;
        var z = (polyP[0].z + polyP[3].z) / 2;
        return new Vector3(x, transform.position.y, z);
    }
    
    /// <summary>
    /// defines the initial point of the farm soil which make it the center of the tile
    /// that starts in NW
    /// </summary>
    Vector3 DefineInitSuchSoilPoint(Vector3 point)
    {
        Vector3 lo = point;
        lo.x = lo.x + Mathf.Abs(m.SubDivide.XSubStep) / 3;  //used to be 2 ... put 3 
                                                            //to correct bugg that will push the farm tiles to the positive X
                                                            //and neg Z oonce i corrected the tiles to look seamless on terrain
                                                            //i remove and add this values to the initial point
        lo.z = lo.z - Mathf.Abs(m.SubDivide.ZSubStep) / 3;
        return lo;
    }

    /// <summary>
    /// Creates the initial list points for a farm. The farm tiles are biult based on the initial point
    /// Becauise this tiles have an scale
    /// </summary>
    void CreateFarmSoil()
    {
        soil = RetuFillPolyRealY(OnScreenPoly[0], OnScreenPoly[2], 
            Mathf.Abs( m.SubDivide.XSubStep),
            Mathf.Abs( m.SubDivide.ZSubStep), 
            true);

        createSoilNow = true;
    }

    /// <summary>
    /// Called from Update if createSoilNow = true
    /// Creates all the planes for the farm. Based on initial position and scale
    /// </summary>
    /// <param name="pos">The list of initial positions for each tile</param>
    /// <param name="containerP">The container will hold all tiles. Usually is this.transform</param>
    void CreatePlanesRoutine(List<Vector3> pos, Transform containerP)
    {
        if (loopCounter < pos.Count)
        {
            CreatePlane temp = CreatePlane.CreatePlan(Root.createPlane, Root.RetMaterialRoot(MaterialKey),
                pos[loopCounter], scale: Program.gameScene.ScaleSmallRoadUnitFarm, container: containerP);


            _planesSoil.Add(temp);
            loopCounter++;
        }
        else
        {
            createSoilNow = false;
            AfterLoopRoutine();
        }
    }


    /// <summary>
    /// Return a filed poly with RealYs if  bool findRealY is true. Otherwise the same but the Y is NW.y
    /// </summary>
    List<Vector3> RetuFillPolyRealY(Vector3 NW, Vector3 SE, float xStep, float zStep, bool findRealY)
    {
        List<Vector3> res = new List<Vector3>();
        NW = DefineInitSuchSoilPoint(NW);

        for (float x = NW.x; x < SE.x; x+= xStep)
        {
            for (float z = NW.z; z > SE.z; z-= zStep)
            {
                //for fill a field we shiyld use the REal Y so tiles look close to ground
                if (findRealY) { res.Add(m.Vertex.BuildVertexWithXandZ(x, z)); }
                //for find out how big is gonna be the List we dont need findRealY since that makes it slow and we just 
                //need to know a number
                else if (!findRealY) { res.Add(new Vector3(x, NW.y, z)); }
            }
        }
        return res;
    }

    /// <summary>
    /// Assign new material to all the planes
    /// </summary>
    /// <param name="newMat">New Material</param>
    public void AssignNewMaterialToPlanes(Material newMat)
    {
        for (int i = 0; i < _planesSoil.Count; i++)
        {
            _planesSoil[i].Geometry.GetComponent<Renderer>().sharedMaterial = newMat;
        }
    }

    /// <summary>
    /// Updates Farm preview
    /// </summary>
    void UpdateFarmPrev()
    {
        float diffY = UMath.ReturnDiffBetwMaxAndMin(OnScreenPoly, H.Y);
        var locPoly = EnlargePolyTowards(Dir.NE, OnScreenPoly, m.SubDivide.XSubStep, m.SubDivide.ZSubStep);
        farmPrev.UpdatePos(locPoly, diffY + 0.5f, corretMinimuScaleOnBigBoxP: true);

        ChecksCollSizeCallsColor();
    }

    /// <summary>
    /// Enlarges a poly towards a Direction. Is called from UpdateFarmPrev() because for feel natural and smooth
    /// a new row and col had to be added to the farm so it looks good 
    /// </summary>
    List<Vector3> EnlargePolyTowards(Dir towards, List<Vector3> poly, float inX, float inZ)
    {
        counter++;

        inX = Mathf.Abs(inX);
        inZ = Math.Abs(inZ);
        List<Vector3> res = poly;
        Vector3 temp = new Vector3();
        if (towards == Dir.NE)
        {
            temp = res[1];
            temp.x += inX;
            res[1] = temp;

            temp = res[2];
            temp.x += inX;
            temp.z -= inZ;
            res[2] = temp;

            temp = res[3];
            temp.z -= inZ;
            res[3] = temp;
        }
        return res;
    }

    public void Demolish()
    {
        PositionFixed = false;

        for (int i = 0; i < _planesSoil.Count; i++)
        {
            _planesSoil[i].Destroy();
        }
        
        BuildingPot.Control.Registro.RemoveItem(Category, MyId);
        _isOrderToDestroy = true;
        DestroyOrdered();
    }

	// Use this for initialization
    private void Start()
    {
        base.Start();

        //if is not position fixed alredy we create the preview
        //when is Position Fixed already here mean that was loaded from file
        if (!PositionFixed)
        {
            farmPrev = (BigBoxPrev)CreatePlane.CreatePlan(Root.bigBoxPrev, Root.matGreenSel2);
            farmPrev.transform.name = "Farm Preview: " + MyId;
        }

        if (HType == H.Road)
        {
            maxSizeOfFarm = 500;
        }
    }

    // Update is called once per frame
	void Update ()
	{
        if (!PositionFixed)
        {
            base.Update();
            if (_isOrderToDestroy)
            {
                DestroyPreviews();
            }
        }
	    if (createSoilNow)
	    {
	        CreatePlanesRoutine(soil, transform);
	    }
	}
}
