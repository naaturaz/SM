using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Bridge : Trail
{
    private bool createAirPartsNow;
    private bool createSoilPartsNow;
    private bool showNextStage;

    public Bridge() {}

    public Bridge(H hType, H startingStageForPieces)
    {
        HType = hType;
        StartingStageForPieces = startingStageForPieces;
    }

    // Use this for initialization
	void Start ()
    {
        base.Start();
	}
	
	// Update is called once per frame
	void Update ()
    {
        base.Update();

	    if (createAirPartsNow)
	    {CreatePartListOnAir();}

	    if (createSoilPartsNow)
	    {CreatePartListOnGround();}

        if(showNextStage)
        { ShowNextStageOfPartsRoutine();}

	    AddBridgeToBuildControl();
	    Loading();
    }

    private void Loading()
    {
        if (IsLoadingFromFile && (Anchors==null || Anchors.Count == 0))
        {
            SetBridgeAnchors();
        }
    }

    private bool addedToBuildControl;
    private void AddBridgeToBuildControl()
    {
        if (PositionFixed && !createSoilPartsNow && !createAirPartsNow && Pieces.Count == PartsOnAir.Count + PartsOnSoil.Count
            && !addedToBuildControl && Pieces.Count > 0)
        {
            //is being added here bz is needs to have all parts spawned to work
            if (HType.ToString().Contains(H.Bridge.ToString()))
            {
                SetBridgeAnchors();
                print("Get Acnhor called from Bridge");

                //here so when Builders ask for anchros they have it already
                //AddBridgeToRegistro();

                addedToBuildControl = true;


                //UpdateOnBuildControl(H.Add);
            }
        }
    }

    protected void SetBridgeAnchors()
    {
        var a = GetBridgeAnchors();
        Anchors = GetBridgeAnchors();
        //    UVisHelp.CreateHelpers(Anchors, Root.redSphereHelp);
    }

    /// <summary>
    /// If call will set showNextStage true that will allow 
    /// ShowNextStageOfPartsRoutine() to be called from Update 
    /// </summary>
    public void ShowNextStageOfParts()
    {
        showNextStage = true;
    }

    /// <summary>
    /// Show next stage of all Pieces... as loopCounter adds... 
    /// This is called from Update if showNextStage = true
    /// </summary>
    private void ShowNextStageOfPartsRoutine()
    {
        if (loopCounter < Pieces.Count)
        {
            Pieces[loopCounter].ShowNextStage();
            loopCounter++;
        }
        else
        {
            StructureParent sP = new StructureParent();
            sP.ResaveOnRegistro(Pieces[0].StartingStage, MyId);
            showNextStage = false;
            loopCounter = 0;
            RemoveFromBuildersManager();
        }
    }

    /// <summary>
    /// Will remove this building from builders manager if done since this is an async building to be finished 
    /// </summary>
    void RemoveFromBuildersManager()
    {
        if (Pieces[0].CurrentStage == 4)
        {
            HandleLastStage();
        }
    }

    /// <summary>
    /// Creates the air and ground parts and addToRegistro
    /// </summary>
    public void CreatePartsRoutine()
    {
        //creates parts above river
        PlanesOnAirPos = ReturnPlanesOnAirPosAndDefinePlanesOnSoil();
        PartsOnAir = ClassifyABridgeByParts(ReturnPlanesOnAirPosAndDefinePlanesOnSoil().Count);
        
        createAirPartsNow = true;

        //creates parts on gorund
        //bz has duplicates// the big number is so close tiles dont have a part created
        //only needed if is a road
        if (HType.ToString().Contains("Road"))
        {
            PlanesOnSoil = EliminatesDuplicateDependingOnDominantSide(PlanesOnSoil);
        }
        //if is a trail...
        else if (HType.ToString().Contains("Trail"))
        {
            PlanesOnSoil = UList.EliminateDuplicatesByDist(PlanesOnSoil, 0.01f);
        }

        //bz is not ordered. They need to be ordered to be clasified
        PlanesOnSoil = OrderByXorZ(PlanesOnSoil);
        PartsOnSoil = ClassifyShorePoints();

        //must be called here... So still we are using the CurrentSpawnBuild obj
        AddBridgeToRegistro();
    }

    /// <summary>
    /// This eliminates duplicates of the bridge parts vectors3. Some pos are to close
    /// based on dominant side this will elimitaes the duplicates taht are less than 5 times the subdive x or z - 0.01f
    /// </summary>
    List<Vector3> EliminatesDuplicateDependingOnDominantSide(List<Vector3> list)
    {
        if(_dominantSide == H.Vertic)
        {
            return UList.EliminateDuplicatesByDist(list, (Mathf.Abs(m.SubDivide.ZSubStep) * 5) - 0.01f);
        }
        if (_dominantSide == H.Horiz)
        {
            return UList.EliminateDuplicatesByDist(list, (Mathf.Abs(m.SubDivide.XSubStep) * 5) - 0.01f);
        }
        return null;
    }

    /// <summary>
    /// Depending if is doninait side vertic or horiz will order by
    /// </summary>
    /// <returns></returns>
    List<Vector3> OrderByXorZ(List<Vector3> list)
    {
        if(_dominantSide == H.Vertic)
        {
            return list.OrderBy(a => a.z).ToList(); 
        }
        if (_dominantSide == H.Horiz)
        {
            return list.OrderBy(a => a.x).ToList();
        }
        return null;
    }

    /// <summary>
    /// Clasifie the shore points, the first is 10 and last one are 12, the rest are 11
    /// this method must be called after ReturnPlanesOnAirPos()
    /// </summary>
    /// <returns></returns>
    List<int> ClassifyShorePoints()
    {
        List<int> res = new List<int>();

        for (int i = 0; i < PlanesOnSoil.Count; i++)
        {
            res.Add(11);
        }
        //1st and last
        res[0] = 10;
        res[res.Count - 1] = 12;

        return res;
    }

    /// <summary>
    /// Creates the  parts of a bridge visualiiy. This is the part thats is above the river
    /// </summary>
    /// <param name="iniPos"></param>
    public List<StructureParent> CreatePartListOnAir(List<Vector3> iniPos, List<int> partsP, Transform containerP, H dominantSideP)
    {
        List<StructureParent> res = new List<StructureParent>();
        for (int i = 0; i < partsP.Count; i++)
        {
            StructureParent sP = null;
            string root = "";
            if (partsP[i] == 1)
            {
                root = Root.bridgeTrailPart1;
            }
            else if (partsP[i] == 2) 
            {
               root = Root.bridgeTrailPart2;
            }
            else if (partsP[i] == 3)
            {
                root = Root.bridgeTrailPart3;
            }
            else if (partsP[i] == 4)
            {
                root = Root.bridgeTrailPart4;
            }

            sP = StructureParent.CreateStructureParent(root, iniPos[i], BridgeUnit(), container: containerP,
                startingStage: StartingStageForPieces);

            if (dominantSideP == H.Vertic)
            {
                sP.transform.Rotate(new Vector3(0, 270, 0));
            }
            sP = ReSizeObj(sP, partsP[i], dominantSideP);
            res.Add(sP);
        }
        return res;
    }

    /// <summary>
    /// Creates the parts list on air
    /// </summary>
    private void CreatePartListOnAir()
    {
        if (loopCounter < PartsOnAir.Count)
        {
            StructureParent sP = null;
            string root = "";
            if (PartsOnAir[loopCounter] == 1)
            {
                root = Root.bridgeTrailPart1;
            }
            else if (PartsOnAir[loopCounter] == 2)
            {
                root = Root.bridgeTrailPart2;
            }
            else if (PartsOnAir[loopCounter] == 3)
            {
                root = Root.bridgeTrailPart3;
            }
            else if (PartsOnAir[loopCounter] == 4)
            {
                root = Root.bridgeTrailPart4;
            }

            sP = StructureParent.CreateStructureParent(root, PlanesOnAirPos[loopCounter], BridgeUnit(),
                container: transform,
                startingStage: StartingStageForPieces);

            if (_dominantSide == H.Vertic)
            {
                sP.transform.Rotate(new Vector3(0, 270, 0));
            }
            sP = ReSizeObj(sP, PartsOnAir[loopCounter], _dominantSide);
            Pieces.Add(sP);
            loopCounter++;
        }
        else
        {
            createAirPartsNow = false;
            loopCounter = 0;
            createSoilPartsNow = true;


            //so crystals are added to ground right away
            //MeshController.CrystalManager1.Add(this);
            PrivHandleZoningAddCrystals();
        }
    }
    /// <summary>
    /// REturn wht type of bridge unit is this one 
    /// </summary>
    /// <returns></returns>
    H BridgeUnit()
    {
        if (HType == H.BridgeRoad)
        {
            return H.BridgeRoadUnit;
        }
        if (HType == H.BridgeTrail)
        {
            return H.BridgeTrailUnit;
        }
        throw new Exception("HType must be defined if calling this method");
    }

    /// <summary>
    /// Creates the  parts of a bridge visualiiy. This is the part thats is on the ground
    /// </summary>
    /// <param name="iniPos"></param>
    public List<StructureParent> CreatePartListOnGround(List<Vector3> iniPos, List<int> partsP, Transform containerP, H dominantSideP)
    {
        List<StructureParent> res = new List<StructureParent>();
        StructureParent g = null;
        for (int i = 0; i < partsP.Count; i++)
        {
            if (partsP[i] == 10)
            {
                g = StructureParent.CreateStructureParent(Root.bridgeTrailPart12, iniPos[i], BridgeUnit(), container: containerP,
                    startingStage: StartingStageForPieces);
                g.transform.Rotate(new Vector3(0, 180, 0));
            }
            else if (partsP[i] == 11)
            {
                g = StructureParent.CreateStructureParent(Root.bridgeTrailPart11, iniPos[i], BridgeUnit(), container: containerP,
                    startingStage: StartingStageForPieces);
            }
            else if (partsP[i] == 12)
            {
                g = StructureParent.CreateStructureParent(Root.bridgeTrailPart12, iniPos[i], BridgeUnit(), container: containerP,
                    startingStage: StartingStageForPieces);
            }

            if (dominantSideP == H.Vertic) { g.transform.Rotate(new Vector3(0, 270, 0)); }
            g = ReSizeObj(g, partsP[i], dominantSideP);
            res.Add(g);
        }
        return res;
    }

    private int loopCounter;

    /// <summary>
    /// Creates the parts on ground
    /// </summary>
    private void CreatePartListOnGround()
    {
        StructureParent g = null;
        if (loopCounter < PartsOnSoil.Count)
        {
            if (PartsOnSoil[loopCounter] == 10)
            {
                g = StructureParent.CreateStructureParent(Root.bridgeTrailPart12, PlanesOnSoil[loopCounter],
                    BridgeUnit(), container: transform,
                    startingStage: StartingStageForPieces);
                g.transform.Rotate(new Vector3(0, 180, 0));
            }
            else if (PartsOnSoil[loopCounter] == 11)
            {
                g = StructureParent.CreateStructureParent(Root.bridgeTrailPart11, PlanesOnSoil[loopCounter],
                    BridgeUnit(), container: transform,
                    startingStage: StartingStageForPieces);
            }
            else if (PartsOnSoil[loopCounter] == 12)
            {
                g = StructureParent.CreateStructureParent(Root.bridgeTrailPart12, PlanesOnSoil[loopCounter],
                    BridgeUnit(),
                    container: transform,
                    startingStage: StartingStageForPieces);
            }

            if (_dominantSide == H.Vertic)
            {
                g.transform.Rotate(new Vector3(0, 270, 0));
            }
            g = ReSizeObj(g, PartsOnSoil[loopCounter], _dominantSide);
            Pieces.Add(g);
            loopCounter++;
        }
        else
        {
            createSoilPartsNow = false;
            loopCounter = 0;
        }
    }

    /// <summary>
    /// Created to resize the vertical dominant side bridges bz they are slightly smaller
    /// and rezised part 11 on the horizontal
    /// </summary>
    StructureParent ReSizeObj(StructureParent current, int objPart, H dominantSideP)
    {
        StructureParent res = null;
        //the percent from a horizntal
        float percentForVertic = 0.888889f;
        float percentForVerticPart11 = 0.82f;//0.8333333f;

        Vector3 t = current.transform.localScale;
        if (dominantSideP == H.Vertic)
        {
            if (objPart != 11)
            {
                t.x *= percentForVertic;
            }
            else
            {
                t.x *= percentForVerticPart11;
            }
        }

        //so in horizontal the 11 is smaller too
        if (dominantSideP == H.Horiz && objPart == 11)
        {
            t.x *= 0.9055f;
        }

        if (HType == H.BridgeTrail)
        {
            t.x /= 5;
            t.y /= 5;
            t.z /= 5;
        }
        if (HType == H.None) { throw new Exception("HType cant be None");}
        current.transform.localScale = t;
        return current;
    }

    /// <summary>
    /// give u the planes list pos are on the air and defines planes on soil too
    /// </summary>
    List<Vector3> ReturnPlanesOnAirPosAndDefinePlanesOnSoil()
    {
        float minHeightToCreate = 0.1f;
        List<Vector3> res = new List<Vector3>();
        List<CreatePlane> bridgePlanes = WhichPathWasUsedToBridge();
        for (int i = 0; i < bridgePlanes.Count; i++)
        {
            Vector3 planePos = bridgePlanes[i].transform.position;
            if(planePos.y - m.Vertex.BuildVertexWithXandZ(planePos.x, planePos.z).y > minHeightToCreate)
            {
                res.Add(planePos);
            }
            else PlanesOnSoil.Add(planePos);
        }
        return res;
    }

    /// <summary>
    /// Will return the path was used to create the bridge based on the _dominantSide
    /// </summary>
    List<CreatePlane> WhichPathWasUsedToBridge()
    {
        List<CreatePlane> res = new List<CreatePlane>();
        if(_dominantSide == H.Vertic)
        {
            res = PlanesListVertic;
        }
        else if(_dominantSide == H.Horiz)
        {
            res = PlanesListHor;
        }
        return res;
    }
   
    /// <summary>
    /// Will return the classification of each peace in a order of 1, 2 , 3 ,4.. Depending
    /// if is even or not will make it different
    /// The sequence of a bridge is 1,2,3,4,1,2,3,4,1... ,  1 is the tallest part and 3 the middle one 
    /// </summary>
    /// <param name="amount"></param>
    List<int> ClassifyABridgeByParts(int amount)
    {
        List<int> res = new List<int>();

        if(isAEvenNumb(amount))
        {
           res = CreateEvenSequence(amount);
        }
        else if(!isAEvenNumb(amount))
        {
            res = CreateUnEvenSequence(amount);
        }
        return res;
    }
    
    /// <summary>
    /// Will return true if the value passed was an even numb
    /// </summary>
    public static bool isAEvenNumb(int value)
    {return value % 2 == 0;}

 
    /// <summary>
    /// Create The sequence of a bridge is 1,2,3,4,1,2,3,4,1... ,  1 is the tallest part and 3 the middle one 
    /// </summary>
    List<int> CreateEvenSequence(int amount)
    {
        List<int> res = new List<int>();
        List<int> firstHalf = new List<int>();
        List<int> secondHalf = new List<int>();

        //1st half
        int current = 2;
        firstHalf.Add(current);
        for (int i = 0; i < (amount - 1) / 2; i++)
        {
            current = UMath.GoAround(-1, current, 1, 4);
            firstHalf.Add(current);
        }
        firstHalf.Reverse();
        
        //2nd half
        current = 4;
        secondHalf.Add(current);
        for (int i = 0; i < (amount - 1) / 2; i++)
        {
            current = UMath.GoAround(1, current, 1, 4);
            secondHalf.Add(current);
        }

        res.AddRange(firstHalf);
        res.AddRange(secondHalf);
        return res;
    }

    /// <summary>
    /// Create The sequence of a bridge is 1,2,3,4,1,2,3,4,1... ,  1 is the tallest part and 3 the middle one 
    /// </summary>
    List<int> CreateUnEvenSequence(int amount)
    {
        List<int> res = new List<int>();
        List<int> firstHalf = new List<int>();
        List<int> secondHalf = new List<int>();

        //1st half
        int current = 3;
        firstHalf.Add(current);
        for (int i = 0; i < (amount-1) / 2; i++)
        {
            current = UMath.GoAround(-1, current, 1, 4);
            firstHalf.Add(current);
        }
        firstHalf.Reverse();

        //2nd half
        current = 4;
        secondHalf.Add(current);
        for (int i = 0; i < (amount - 2) / 2; i++)
        {
            current = UMath.GoAround(1, current, 1, 4);
            secondHalf.Add(current);
        }

        res.AddRange(firstHalf);
        res.AddRange(secondHalf);
        return res;
    }







    #region Parts 12 

    /// <summary>
    /// Returns the 2 Parts 12 of the Bridge .. the two ends 
    /// </summary>
    /// <returns></returns>
    public List<StructureParent> GiveTheTwoParts12()
    {
        List<StructureParent> res = new List<StructureParent>();
        for (int i = 0; i < Pieces.Count; i++)
        {
            if (Pieces[i].MyId.Contains(H.Bridge_Trail_Piece_12.ToString()))
            {
                res.Add(Pieces[i]);
            }
        }
        return res;
    }

    //<summary>
    //I get the anchors of the 2 points and then I find the polygon of them thats the 
    //Acnhors of a bridge 
    //</summary>
    public List<Vector3> GetBridgeAnchors()
    {
        //var p12 = GiveTheTwoParts12();
        //var anchors1 = p12[0].GetAnchors();
        //var anchors2 = p12[1].GetAnchors();
        //anchors1.AddRange(anchors2);
        //return Registro.FromALotOfVertexToPoly(anchors1);

        if (BoundsHoriz.Count > 0)
        {
            return Registro.FromALotOfVertexToPoly(BoundsHoriz);
        }

        else if (BoundsVertic.Count > 0)
        {
            return Registro.FromALotOfVertexToPoly(BoundsVertic);
        }
        return null;
    }

    /// <summary>
    /// Used to find LandZones
    /// return new List<Vector3>(){_firstWayPoint, _secondWayPoint};
    /// 
    /// but those above are not save loaded
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GiveTwoRoughEnds()
    {
        return new List<Vector3>(){_firstWayPoint, _secondWayPoint};
    }

    /// <summary>
    /// Will give the 2 ends of a brdige 
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GiveTwoEnds()
    {
        List<StructureParent> currentBridgeParts12 = GiveTheTwoParts12();
        var part12ABotton = currentBridgeParts12[0].BottonMiddle().transform.position;
        var part12BBottom = currentBridgeParts12[1].BottonMiddle().transform.position;

        return new List<Vector3>(){part12ABotton, part12BBottom};
    }

    private List<Vector3> GiveTwoTops()
    {
        List<StructureParent> currentBridgeParts12 = GiveTheTwoParts12();
        var part12A = currentBridgeParts12[0].TopMiddle().transform.position;
        var part12 = currentBridgeParts12[1].TopMiddle().transform.position;

        return new List<Vector3>() { part12A, part12 };
    }







    /// <summary>
    /// Will find the VectorLand the bridge has in that zone 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    VectorLand ReturnMyVectorLandOnThatZone(VectorLand other)
    {
        if (other.LandZone == LandZone1[0].LandZone)
        {
            return LandZone1[0];
        }
        else if (other.LandZone == LandZone1[1].LandZone)
        {
            return LandZone1[1];
        }
        else
        {
            throw new Exception("Must have always one tht is the same" +
                                " other wise problem on BrideManager.ThereAreMoreNewConnections()");
        }
    }


    /// <summary>
    /// Will return first the botton of the vectorland, then the top of the vector land.
    /// Then the top of the other side of the bridge , and then the bottom of the other side 
    ///
    /// brVectorLand: is  vector land tht is asking for this brdige points
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public List<Vector3> ReturnBottonAndTopBasedOnVectorLand(VectorLand posVectorLand)
    {
        //bz need to get the VectorLand on the Bridge
        posVectorLand = ReturnMyVectorLandOnThatZone(posVectorLand); 
        List<Vector3> res = new List<Vector3>();

        var bots = ReturnTwoOrderedByDistance(posVectorLand.Position, H.Bottom);
        var tops = ReturnTwoOrderedByDistance(posVectorLand.Position, H.Top);

        res.AddRange(tops);
        res.Insert(0, bots[0]);//insert 1st one
        res.Add(bots[1]);//add last 

        return res;
    }

    /// <summary>
    /// Always the one on the land of the 'posVectorLand' will be returned first
    /// </summary>
    /// <param name="posVectorLand"></param>
    /// <returns></returns>
    public List<Vector3> ReturnBottonsBasedOnVectorLand(VectorLand posVectorLand)
    {
        //bz need to get the VectorLand on the Bridge
        posVectorLand = ReturnMyVectorLandOnThatZone(posVectorLand);
        List<Vector3> res = new List<Vector3>();

        var bots = ReturnTwoOrderedByDistance(posVectorLand.Position, H.Bottom);

        res.Insert(0, bots[0]);//insert 1st one
        res.Add(bots[1]);//add last 

        return res;
    }

    /// <summary>
    /// Always the one on the land of the 'posVectorLand' will be returned first
    /// </summary>
    public List<Vector3> ReturnTopsBasedOnVectorLand(VectorLand posVectorLand)
    {
        //bz need to get the VectorLand on the Bridge
        posVectorLand = ReturnMyVectorLandOnThatZone(posVectorLand);
        List<Vector3> res = new List<Vector3>();

        var tops = ReturnTwoOrderedByDistance(posVectorLand.Position, H.Top);
        res.AddRange(tops);

        return res;
    }

    /// <summary>
    /// Will return bottoms or tops ordered by dist from 'from'
    /// </summary>
    /// <param name="from"></param>
    /// <param name="which"></param>
    /// <returns></returns>
    List<Vector3> ReturnTwoOrderedByDistance(Vector3 from, H which)
    {
        List<Vector3> res = new List<Vector3>();
        List<Vector3> compare = new List<Vector3>();

        if (which == H.Bottom)
        {
            compare = GiveTwoEnds();
        }
        else
        {
            compare = GiveTwoTops();
        }

        float dist0 = Vector3.Distance(from, compare[0]);
        float dist1 = Vector3.Distance(from, compare[1]);

        if (dist0 < dist1)
        {
            res.Add(compare[0]);
            res.Add(compare[1]);
        }
        else
        {
            res.Add(compare[1]);
            res.Add(compare[0]);
        }

        return res;
    }



    #endregion
}
