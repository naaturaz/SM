using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CryRoute
{
    private Person _person;
    private List<CheckPoint> _checkPoints = new List<CheckPoint>();
    List<int> _historicRegions = new List<int>();//the regions we are being dealing with this Route 
    List<int> _prevHistoRegions = new List<int>();//the regions we are being dealing with this Route 
    List<Crystal> _historicCrystals = new List<Crystal>();//the regions we are being dealing with this Route 

    private VectorLand _one;
    private VectorLand _two;

    private Crystal _curr = new Crystal();
    private Vector3 _next;

    private CryRect _currRect;//the current rect we are working with right now 
    List<Crystal> _crystals = new List<Crystal>();//the cryustals contain on my rect

    //this are the crystals tht will be evaluated to be move to .
    //here u will see the 2 crystals tht are closer to '_curr' being evaluated 
    List<Crystal> _eval = new List<Crystal>();

    private bool loop;

    private bool _isRouteReady;
    private TheRoute _theRoute;

    private Building _ini;
    private Building _fin;

    private float durationOfLines = 20f;

    private bool _iniDoor;
    private bool _finDoor;

    //a created _crystal tht hoold all the information of the last door, is fake in the sense that is not the
    //real one in the CrystalRegion but is here so is always in the _eval as 1st 
    private Crystal _finCrystal;
    private string _destinyKey;//to be added to TheRoute obj

    Explorer _explorer = new Explorer();

    private float _timeStamp;//when the Route was started 

    public bool IsRouteReady
    {
        get { return _isRouteReady; }
        set { _isRouteReady = value; }
    }

    public TheRoute TheRoute
    {
        get { return _theRoute; }
        set { _theRoute = value; }
    }

    public CryRoute(Structure ini, Structure fin, Person person, string destinyKey, bool iniDoor = true, bool finDoor = true)
    {
        _origenKey = ini.MyId;
        _destinyKey = destinyKey;

        _iniDoor = iniDoor;
        _finDoor = finDoor;

        _person = person;
        _one = ini.LandZone1[0];
        _two = fin.LandZone1[0];

        _ini = ini;
        _fin = fin;

        ClearOldVars();
        Init();
    }

    public CryRoute() { }

    public CryRoute(VectorLand uno, VectorLand dos, Person person, bool iniDoor = true, bool finDoor = true)
    {
        _origenKey = uno.MyBuild().MyId;
        _destinyKey = dos.MyBuild().MyId;

        _iniDoor = iniDoor;
        _finDoor = finDoor;

        _person = person;
        _one = uno;
        _two = dos;

        _ini = uno.MyBuild();
        _fin = dos.MyBuild();

        ClearOldVars();
        Init();
    }

    private void Init()
    {
        _timeStamp = Time.time;

        _curr.Position = U2D.FromV3ToV2(_one.Position);
        loop = true;

        ClearDebugLocal();

        _finCrystal = new Crystal(_two.Position, H.Door, _fin.MyId, true, false);
    }

    void ClearDebugLocal()
    {
        Crystal.DebugCrystal.Restart();
        Crystal.passes.Clear();
    }


    private void ClearOldVars()
    {
        grow = GROWC;
        _checkPoints.Clear();
        _historicRegions.Clear();
        ResetExplorer();
    }

    #region Loop on Update

    //the Method is looping now 
    private string looping = "";
    private string prevLoop = "";//the loop before this one 
    private int loopCount;


    private void CallSpecLoop()
    {
        if (looping == "TryReachEval")
        {
            TryReachEval();
        }
        else if (looping == "OrderEvalByWeight")
        {
            OrderEvalByWeight();
            loop = true;//nees to be called bz is set to true only on TryReachEval();
        }
        else if (looping == "OrderCyrstalsFromCurr")
        {
            OrderCyrstalsFromCurr();
            loop = true;
        }
        else if (looping == "DefineCrystalsOnMyRect")
        {
            DefineCrystalsOnMyRect();
            loop = true;
        }
    }

    bool CanPrepareLoop(string loopingP)
    {
        if (looping != "" && looping != loopingP)
        {
            return false;
        }

        looping = loopingP;
        prevLoop = loopingP;
        return true;
    }

    private void ResetLoop()
    {
        looping = "";
        loopCount = 0;
    }

    void ClearPrevLoop()
    {
        prevLoop = "";
    }

    #endregion

    public void AddKeyToExplorer(Crystal key, Vector3 intersection)
    {
        _explorer.AddKey(key, intersection, U2D.FromV2ToV3(_curr.Position), _two.Position);
    }

    internal void Update()
    {
        if (wasBlackListed)
        {
            return;
        }

        //so loops thru a specific loop and when is done then can move on to call Recursive again
        if (looping != "")
        {
            CallSpecLoop();
            return;
        }


        if (loop)
        {
            loop = false;
            Recursive();
        }

        //something went wrong so must likely isnt able to reach the _fin
        //so hence is BlackLIsted 
        if (_checkPoints.Count > 0 && loop == false && !IsRouteReady)
        {
            CheckIfIsToBlackList();
        }
    }

    /// <summary>
    /// Will say if has a useful way 
    /// </summary>
    /// <returns></returns>
    bool ItHasAWay()
    {
        var stepFinalPos = ReturnCorFinal();
        if (Vector3.Distance(stepFinalPos, _two.Position) < 0.1f)
        {
            //means can reach the final position already
            return false;
        }

        var wayCrystals = _crystals.Where(a => a.Type1.ToString().Contains("Way"));

        //if is a way crytal
        if (wayCrystals.Any())
        {
            return true;
        }
        return false;
    }


    //u can explore only once for a _curr
    private bool canIExplore = true;
    private void Recursive()
    {
        if (prevLoop == "")
        {
            //tha adding of a good point to the Route 
            _checkPoints.Add(new CheckPoint(U2D.FromV2ToV3(_curr.Position), _curr.Type1));

            if (CheckIfDone())
            {
                CanIReach2PointAfter();
                Ready();
                //if (_finDoor)
                //{
                //    Crystal.DebugCrystal.ShowNow();
                //}
                return;
            }

            CreateCryRect();

            DefineHistoCrys();
            DefineCrystalsOnMyRect();


        }
        else if (prevLoop == "DefineCrystalsOnMyRect")
        {
            ClearPrevLoop();
            OrderCyrstalsFromCurr();
        }
        else if (prevLoop == "OrderCyrstalsFromCurr")
        {
            ClearPrevLoop();
            DefineEvalCrystals();
            AddToEvalFromRect();//i place it here so Auto Delta Routing works

            if (OrderEvalByWeight())
            { return; }//so then is called again until I finish the loop 
        }
        else if (prevLoop == "OrderEvalByWeight")
        {
            ClearPrevLoop();
            //PushAwayToLastOnEval();
            //InsertFinDoor();
            TryReachEval();
        }
    }

    /// <summary>
    /// Will make the last door always the first Crystal to check
    /// </summary>
    private void InsertFinDoor()
    {
        _eval.Insert(0, _finCrystal);
    }

    private void DefineHistoCrys()
    {
        //if (_prevHistoRegions == _historicRegions)
        //{
        //    return;
        //}
        //Debug.Log("Set HistoCrys");

        _prevHistoRegions = _historicRegions;
        //_historicCrystals = PersonController.CrystalManager1.GiveAllCrystalsInTheseRegionsExcludLinkRects(_historicRegions);
        _historicCrystals = MeshController.CrystalManager1.GiveAllTerraCrystalsInTheseRegionsPlsObsta(_historicRegions);
    }

    /// <summary>
    /// Will add crystals from the rect to be eval in case none of the others works
    /// </summary>
    private void AddToEvalFromRect()
    {
        //if (grow == GROWC && blackCount == 0)
        //{
        //    return;
        //}

        //is added as an obstacle bz is where we are heding to so i will give less weight 
        _eval.Add(_currRect.CCrystal);
        _eval.Add(_currRect.DCrystal);
        _eval.Add(_currRect.BCrystal);
    }

    /// <summary>
    /// Created to avoid bugg where Rect was gettting created again and again bz could not find any crystalls
    /// 
    /// Will add to crystalls all the buildings I intersect crystals . So they can be considered when routing 
    /// </summary>
    private void DefineCrystalsMyRectIntersects()
    {
        var thisCrys = MeshController.CrystalManager1.ReturnCrystalsRectIntersect(_currRect.TheRect, _historicRegions);

        _crystals.AddRange(thisCrys);
        _crystals = _crystals.Distinct().ToList();
    }

    /// <summary>
    /// Will try to reach any of the _eval Crystals
    /// </summary>
    private bool TryReachEval()
    {
        var canI = CanPrepareLoop("TryReachEval");
        if (!canI) { return false; }//means tht another llop is running now 
        var i = loopCount;

        if (_explorer.IsBuildingRouting)
        {
            //case that we are hitting a builing 
            //bz only has 4 _evals to work with 
            return TryReachBuilding();
        }

        //Debug.Log("Terrain Routing. loopCount: " + loopCount);
        if (loopCount < _eval.Count)
        {
            Line aLine = new Line(U2D.FromV2ToV3(_curr.Position), U2D.FromV2ToV3(_eval[i].Position), durationOfLines);

            var linesIntersected = IntersectCount(aLine);
            var isIntersectingOnlyTheDoorSide = IsCrystalOnDoorCorners(_eval[i]) && linesIntersected == 1;
            var isCryFromOld = IsCrystalPartOfOldHome(_eval[i]);

            //is the crystal in the door side and only interset 1 line
            if ((!Intersect(aLine) || isIntersectingOnlyTheDoorSide || isCryFromOld)
                && !IsOnTheRoute(_eval[i].Position)) //they cant be the same 
            {
                //will reset black vount if 1 is true
                blackCount = 0;

                //make current _eval[i] and loop 
                _curr = _eval[i];
                //Debug.Log("_curr set on Terrain Routing");

                loop = true;
                //Crystal.DebugCrystal.AddGameObjInPosition(U2D.FromV2ToV3(_curr.Position), Root.yellowSphereHelp);

                ResetExplorer();
                //UVisHelp.CreateHelpers(U2D.FromV2ToV3(_eval[i].Position), Root.blueCube);
                //UVisHelp.CreateText(U2D.FromV2ToV3(_curr.Position), _curr.CalcWeight + "");

                ResetLoop();
                ClearPrevLoop();//so can restart Recursive()
                return true;
            }
            loopCount++;

            CheckIfIsToBlackList();
            return false;
        }
        else
        {
            ResetLoop();
            return false;
        }
    }

    bool TryReachBuilding()
    {
        //        Debug.Log("TryReachBuilding() called");

        loop = true;

        for (int i = 0; i < _eval.Count; i++)
        {
            if (i == 3 || i == 4)
            {
                //UVisHelp.CreateHelpers(U2D.FromV2ToV3(_eval[i].Position), Root.blueCube);
            }

            Line aLine = new Line(U2D.FromV2ToV3(_curr.Position), U2D.FromV2ToV3(_eval[i].Position), durationOfLines);
            var linesIntersected = IntersectCount(aLine);

            if (linesIntersected == 0 && !IsOnTheRoute(_eval[i].Position))
            {
                //                Debug.Log("TryReachBuilding() _curr set");

                ResetExplorer();
                _curr = _eval[i];
                //UVisHelp.CreateHelpers(U2D.FromV2ToV3(_curr.Position), Root.yellowCube);
                ResetLoop();
                ClearPrevLoop();//so can restart Recursive()
                return true;
            }
        }

        throw new Exception("At least the intersection should be reached. Go and investigate but at least once should " +
                            "pass this if all fail pls investigate" +
                            "\n ini:" + _ini.MyId + " mid:" + _eval[0].ParentId + " end:" + _fin.MyId + " person:" + _person.MyId + " " +
                            "eval ct:" + _eval.Count);
        return false;
    }

    /// <summary>
    /// bz a new _curr is set and a new exploration needs to be done 
    /// </summary>
    void ResetExplorer()
    {
        canIExplore = true;//
        _explorer.Restart();
    }

    /// <summary>
    /// Will tell u if the 'aLine' is only intersecting the door side of the buildin g
    /// </summary>
    /// <param name="aLine"></param>
    /// <returns></returns>
    private bool LineIntersectedIsDoorSide(Line aLine)
    {
        var lines = LinesIIntersect(aLine);

        if (lines.Count != 1)
        {
            return false;
        }



        return false;
    }

    /// <summary>
    /// Will tell u if is just crossing a brdige this time 
    /// </summary>
    /// <param name="_curr"></param>
    /// <param name="crystal"></param>
    /// <returns></returns>
    private bool IsGettingIntoBridge(Crystal currP, Crystal eval)
    {
        var samePos = UMath.nearEqualByDistance(U2D.FromV2ToV3(eval.Position), _two.Position, 0.1f);

        if (samePos)
        {
            return true;
        }
        return false;
    }

    private bool wasBlackListed;
    private int blackCount;
    //the rect will be allow to grow only 10 times. then will be black list tht building if was not reach
    private int maxCounts = 100;
    void CheckIfIsToBlackList()
    {
        blackCount++;
        //        Debug.Log("blackCount:"+blackCount);

        if (blackCount > maxCounts)
        {
            BlackList();
        }
        //is being a minute since started then can be blaclisted
        else if (Time.time > _timeStamp + 90f)//maybe can add someFactor with PC Ram and CPU Speed
        {
            BlackList();
        }
    }

    private void BlackList()
    {
        if (_fin.MyId.Contains("Tree"))
        {
            var t = this;
        }

        Debug.Log("BlackListed: " + _fin.MyId + " by: " + _person.MyId);
        _person.Brain.BlackListBuild(CryBridgeRoute.ExtractRealId((Structure)_fin));
        wasBlackListed = true;
    }



    /// <summary>
    /// Will say if 'newPos' is on the route already
    /// </summary>
    /// <param name="newPos"></param>
    /// <returns></returns>
    bool IsOnTheRoute(Vector2 newPos)
    {
        for (int i = 0; i < _checkPoints.Count; i++)
        {
            var v2 = U2D.FromV3ToV2(_checkPoints[i].Point);

            if (newPos == v2)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Will say if 'c' is one of the corners tht are in closest to the door
    /// 
    /// Those crystal if a line from door to them is intersecting can be ignore bz is in the same building
    /// and only means is getting out of building or in
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    bool IsCrystalOnDoorCorners(Crystal c)
    {
        var iniBool = _ini != null && _ini.MyId == c.ParentId;
        var finBool = _fin != null && _fin.MyId == c.ParentId;

        //if is not a Crystal of ini or fin then return 
        if (!iniBool && !finBool)
        {
            return false;
        }
        if (c.ParentId.Contains("Dummy"))//used for idle routing 
        {
            return false;
        }

        var build = Brain.GetBuildingFromKey(c.ParentId);
        var facer = build.RotationFacerIndex;

        return IsThisCrystalOnDoorSide(facer, c.AnchorIndex);
    }

    /// <summary>
    /// Will tell u if _curr is inside old home and we are trying to get out of it 
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    bool IsCrystalPartOfOldHome(Crystal c)
    {
        var cIsOldHomeCry = _person.Brain.MoveToNewHome.OldHomeKey == c.ParentId && !string.IsNullOrEmpty(c.ParentId);

        var isInside = IsCurrInsideOldHome(c);

        if (
            //cIsOldHomeCry && 
            isInside)
        {
            return true;
        }
        return false;
    }

    bool IsCurrInsideOldHome(Crystal c)
    {
        bool isInside = false;

        var oldHome = Brain.GetBuildingFromKey(_person.Brain.MoveToNewHome.OldHomeKey);
        if (oldHome != null && oldHome != _person.Home)
        {
            var oldHomRect = U2D.FromPolyToRect(oldHome.Anchors);
            oldHomRect = U2D.ReturnRectYInverted(oldHomRect);
            isInside = oldHomRect.Contains(_curr.Position);
        }

        if (isInside)
        {
            Debug.Log("Inside build");
        }

        return isInside;
    }

    /// <summary>
    /// Will tell u if 'anchorIndex' is on side of the door building
    /// </summary>
    /// <param name="facer"></param>
    /// <param name="anchorIndex"></param>
    /// <returns></returns>
    bool IsThisCrystalOnDoorSide(int facer, int anchorIndex)
    {
        //the next facer
        var nextFacer = UMath.Clamper(1, facer, 0, 3);

        if (facer == anchorIndex || nextFacer == anchorIndex)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Will order the _eval by weight from two. So when checking for intersection will start with the
    /// one tht has less wight from _two
    /// </summary>
    private bool OrderEvalByWeight()
    {
        var canI = CanPrepareLoop("OrderEvalByWeight");
        if (!canI) { return false; }//means tht another llop is running now 
        var i = loopCount;

        if (loopCount < _eval.Count)
        {
            //_eval[i].CalculateWeight(_curr.Position, U2D.FromV3ToV2(_two.Position), _curr.Id);
            //_eval[i].CalculateWeight(U2D.FromV3ToV2(_curr.Position));

            var finOrC = ReturnCorFinal();
            _eval[i].CalculateWeight(finOrC, _two.Position, U2D.FromV2ToV3(_curr.Position));

            loopCount++;
            return true;//will cut Recursive Path intentionally, bz i need to finish this loop
        }
        else
        {
            _eval = _eval.OrderBy(a => a.CalcWeight).ToList();
            ResetLoop();

            return false;//so let Recursive Keeps it course 
        }
    }

    /// <summary>
    /// if crystal in _eval further than _curr towards _two, then are removed and added to the end of _eval
    /// </summary>
    private void PushAwayToLastOnEval()
    {
        List<Crystal> temp = new List<Crystal>();

        for (int i = 0; i < _eval.Count; i++)
        {
            _eval[i].CalculateDistance(U2D.FromV3ToV2(_two.Position));
        }

        var distFromCurr = Vector2.Distance(_curr.Position, U2D.FromV3ToV2(_two.Position));

        for (int i = 0; i < _eval.Count; i++)
        {
            //if the distance is bigger tht the distace from Curr then will be removed 
            if (_eval[i].Distance > distFromCurr)
            {
                temp.Add(_eval[i]);
                _eval.RemoveAt(i);
                i--;
            }
        }

        //will add the ones tht are fuehter to last on _eval so are checked last 
        _eval.AddRange(temp);
    }

    /// <summary>
    /// Will order _crystals from _curr using Distance 
    /// </summary>
    bool OrderCyrstalsFromCurr()
    {
        var canI = CanPrepareLoop("OrderCyrstalsFromCurr");
        if (!canI) { return false; }//means tht another llop is running now 
        var i = loopCount;


        if (loopCount < _crystals.Count)
        {
            _crystals[i].CalculateDistance(_curr.Position);
            // _crystals[i].CalculateDistance(U2D.FromV3ToV2( _one.Position));
            loopCount++;
            return true;
        }
        else
        {

            _crystals = _crystals.OrderBy(a => a.Distance).ToList();
            ResetLoop();
            return false;
        }
    }

    /// <summary>
    /// Will define '_eval' , the closest 5 to '_one.Position'
    /// </summary>
    private void DefineEvalCrystals()
    {
        _eval.Clear();
        for (int i = 0; i < _crystals.Count; i++)
        {
            if (i > 30)//24,5,3
            {
                return;
            }
            //will add the first 5
            _eval.Add(_crystals[i]);
        }
    }

    /// <summary>
    /// Will define '_crystals' whicih is the Crystals on the rect 
    /// 
    /// Will return true when is done so Way Routing works 
    /// </summary>
    private void DefineCrystalsOnMyRect()
    {
        var canI = CanPrepareLoop("DefineCrystalsOnMyRect");
        if (!canI) { return; }//means tht another llop is running now 
        var i = loopCount;

        if (i == 0)//means is starting the loop now 
        {
            _crystals.Clear();
        }


        if (i < _historicCrystals.Count)
        {
            InnerLoop();
        }
        else
        {
            ResetLoop();
            Reach();//Calling here so way routing works. Its slower like this but way routing will work bz will see if has
            //crystaks tht have way
        }
    }

    /// <summary>
    /// 
    /// 
    /// All the body of this method used to be called in recursive 
    /// </summary>
    private void Reach()
    {
        //will return only if is Done. Other wise needs to be going so _crystals are set 
        //if is BuildingRouting then can return so a new _curr is added and we can keep going 

        //if doesnt have a way cna do this 
        if (canIExplore && ExploreToFin()
            //&& (CheckIfDone() || _explorer.IsBuildingRouting)
            )
        {
            return;
        }

        //if doesnt have a way cna do this 
        //if has a way will go trhu the way 
        RoutineIfBuildWasHit();
    }

    void RoutineIfBuildWasHit()
    {
        //behavoiur if a build was hit 
        if (_explorer.IsBuildingRouting)
        {
            _eval.Clear();
            //            Debug.Log("was hit:" + _explorer.Result.Key + " ct:" + _explorer.Result.Crystals.Count);
            //            Debug.Log("is building routing");

            _eval.AddRange(_explorer.Result.Crystals);

            //should jump to TryReachEval()
            ResetLoop();
            prevLoop = "OrderEvalByWeight";
            loop = true;
        }
    }

    /// <summary>
    /// Will loop 10 items to speed
    /// </summary>
    /// <param name="i"></param>
    void InnerLoop()
    {
        for (int j = 0; j < 20; j++)
        {
            var i = loopCount;
            if (j + loopCount >= _historicCrystals.Count)
            {
                return;
            }

            var histoCry = _historicCrystals[i];
            //UVisHelp.CreateHelpers(histoCry.Position, Root.blueCube);

            if (_currRect.TheRect.Contains(histoCry.Position) && histoCry.Type1 != H.LinkRect)
            {
                _explorer.AddCrystalOfRectC(histoCry);
                _crystals.Add(histoCry);
                AddToCrystalsIfNotThere(histoCry.Siblings);
            }
            loopCount++;
        }
    }




    void AddToCrystalsIfNotThere(List<Crystal> sibling)
    {
        for (int i = 0; i < sibling.Count; i++)
        {
            if (!_crystals.Contains(sibling[i]))
            {
                _crystals.Add(sibling[i]);
            }
        }
    }

    /// <summary>
    /// Depending on _curr position will return Final or C.
    /// So when Im explirng to Final takes in account that we are hitting the sea or a mountain 
    /// in some RectC
    /// </summary>
    /// <returns></returns>
    Vector3 ReturnCorFinal()
    {
        var curr = U2D.FromV2ToV3(_curr.Position);
        var two = _two.Position;
        Vector3 cVect = Vector3.MoveTowards(curr, two, howFarIsRectC);//20

        var distToFin = Vector3.Distance(curr, two);

        //if dist To Final greater that howFarIsRectC, then return C
        if (distToFin > howFarIsRectC)
        {
            return cVect;
        }
        return two;
    }

    /// <summary>
    /// Will try to reach RectC or Final and will set explorer object that will tell if are 
    /// Buidings in the middle or not
    /// </summary>
    /// <returns></returns>
    private bool ExploreToFin()
    {
        var stepFinalPos = ReturnCorFinal();

        //        Debug.Log("Exploring");
        canIExplore = false;
        Line line = new Line(U2D.FromV2ToV3(_curr.Position), stepFinalPos, durationOfLines);
        var interCount = IntersectCount(line);

        if (interCount == 0)
        {
            //            Debug.Log("Exploring went good ");
            _curr = new Crystal(stepFinalPos, H.None, "", setIdAndName: false);
            loop = true;
            //canIExplore = true;//needs to be able to keep exploring
            ResetExplorer();

            ResetLoop();
            ClearPrevLoop();

            return true;
        }
        return false;
    }

    private Crystal _oldCurr;
    private const float GROWC = 3f;
    private float grow = 3f;//3
    private float howFarIsRectC = 40f;//20 . 20 was too small bz will go trhu mountains
    /// <summary>
    /// This is the area we gonna be looking at to evaluate crystals 
    /// </summary>
    private void CreateCryRect()
    {
        var curr = U2D.FromV2ToV3(_curr.Position);
        var two = _two.Position;

        if (CheckIfNeedGrow())
        {
            maxCounts++;
            //curr = PushAwayFromCurrRectCenter(curr);
            //two = PushAwayFromCurrRectCenter(two);
            //return;
        }

        //the C Vector on the Rect
        Vector3 cVect = Vector3.MoveTowards(curr, two, howFarIsRectC);//20
        _currRect = new CryRect(U2D.FromV2ToV3(_curr.Position), cVect, grow);
        _oldCurr = _curr;

        AddToHistoricalRegions(cVect);
        AddToHistoricalRegions(U2D.FromV2ToV3(_curr.Position));
    }

    ///// <summary>
    ///// Will push the 'val' away from the center of the '_currRect.TheRect.center'
    ///// Use to grow the rectagle . Is needed when addressing delta routing 
    ///// </summary>
    ///// <param name="val"></param>
    ///// <returns></returns>
    //Vector3 PushAwayFromCurrRectCenter(Vector3 val)
    //{

    //    var center = U2D.FromV2ToV3(_currRect.TheRect.center);
    //    var dist = Vector3.Distance(center, val);//distnace to center of the rect 

    //    //so its moves away from center
    //    Vector3 res = Vector3.MoveTowards(val, center, -grow * dist);
    //    return res;
    //}

    /// <summary>
    /// Will check if _currRect needs to grow bz _curr didnt change . 
    /// 
    ///      Created to avoid bugg where Rect was gettting created again and again bz could not find any crystalls
    /// </summary>
    private bool CheckIfNeedGrow()
    {
        if (_oldCurr == null)
        {
            return false;
        }

        var closeEnogh = UMath.nearEqualByDistance(_oldCurr.Position, _curr.Position, 0.3f);
        var closeAndTerraObs = UMath.nearEqualByDistance(_oldCurr.Position, _curr.Position, 1.5f) &&
            (_oldCurr.IsTerrainObstacle() || _curr.IsTerrainObstacle());

        if (closeEnogh || closeAndTerraObs)
        {
            Debug.Log("closeEnogh:" + closeEnogh + " closeAndTerraObs:" + closeAndTerraObs);

            //Debug.Log("Rect grow");
            grow *= 2;
            //_currRect.Grow();
            return true;
        }
        grow = GROWC;
        return false;
    }

    /// <summary>
    /// So the position passed is added in the historical regions so it can be
    ///  looked at when examinating if intersecting
    /// </summary>
    /// <param name="position"></param>
    void AddToHistoricalRegions(Vector3 position)
    {
        var newRegions = MeshController.CrystalManager1.ReturnMySurroundRegions(U2D.FromV3ToV2(position));

        for (int i = 0; i < newRegions.Count; i++)
        {
            if (!_historicRegions.Contains(newRegions[i]))
            {
                _historicRegions.Add(newRegions[i]);
                //AddRegionToHistoCrystals(i);
                //                Debug.Log("added: _historicRegions.ct:" + _historicRegions.Count);
            }
        }
    }

    void AddRegionToHistoCrystals(int regionIndx)
    {
        _historicCrystals.AddRange(MeshController.CrystalManager1.GiveAllCrystalsInTheseRegionsExcludLinkRects(regionIndx));
    }

    private int count = 0;
    private string _origenKey;
    /// <summary>
    /// Will try to reach 2 points before the last one to see if a line can be draw btw them
    /// if does actually will eliminate the point in btw them and will recurse.
    /// 
    /// If cant wont do nothing 
    /// </summary>
    private void CanIReach2PointAfter()
    {
        var wayChecks = _checkPoints.Where(a => a.Speed > 1f);
        if (wayChecks.Any())
        {
            //means it has way Crystals on it 
            return;
        }


        if (_checkPoints.Count - 1 < count + 2)
        {
            count = 0;
            return;
        }

        var firstP = _checkPoints[count].Point;
        var twoAfter = _checkPoints[count + 2].Point;

        //   AddToHistoricalRegions(firstP);
        // AddToHistoricalRegions(twoAfter);

        Line draw = new Line(firstP, twoAfter, durationOfLines);

        //IF DOesnt intersect . will remove the point in the middle and will recurse here 
        if (!Intersect(draw))
        {
            _checkPoints.RemoveAt(count + 1);
            CanIReach2PointAfter();
        }
        else//if intersect
        {
            count++;
            CanIReach2PointAfter();
        }
    }

    /// <summary>
    /// Will tell if 'nLine' intersect any line on the __historicRegions
    /// </summary>
    /// <param name="nLine"></param>
    /// <returns></returns>
    bool Intersect(Line nLine)
    {
        return MeshController.CrystalManager1.DoIIntersectAnyLine(nLine, _historicRegions, this);
    }



    /// <summary>
    /// Return 0 if none was intersected 
    /// </summary>
    /// <param name="nLine"></param>
    /// <returns></returns>
    int IntersectCount(Line nLine)
    {
        return MeshController.CrystalManager1.CountLinesIIntersect(nLine, _historicRegions, this);
    }

    List<Line> LinesIIntersect(Line nLine)
    {
        return MeshController.CrystalManager1.LinesIIntersect(nLine, _historicRegions);
    }

    /// <summary>
    /// Will be called when the end its reached
    /// </summary>
    private void Ready()
    {
        loop = false;

        AddBehindDoors();

        AddAnglesToRoute();
        CreateTheRouteObj();
        _isRouteReady = true;
    }

    /// <summary>
    /// Will added only if _ini or _fin are not a bridge 
    /// </summary>
    private void AddBehindDoors()
    {
        if (_ini != null && !_ini.MyId.Contains("Bridge") && _iniDoor)
        {
            var tIni = (Structure)_ini;
            _checkPoints.Insert(0, new CheckPoint(tIni.BehindMainDoorPoint));
        }
        if (_fin != null && !_fin.MyId.Contains("Bridge") && _finDoor)
        {
            var tFin = (Structure)_fin;
            _checkPoints.Add(new CheckPoint(tFin.BehindMainDoorPoint));
        }
    }

    /// <summary>
    /// Will pass point by point and will find wht is the angle facing the next one
    /// </summary>
    void AddAnglesToRoute()
    {
        var myDummy = Program.gameScene.GimeMeUnusedDummy();

        myDummy.transform.position = _checkPoints[0].Point;

        for (int i = 0; i < _checkPoints.Count - 1; i++)
        {
            myDummy.transform.position = _checkPoints[i].Point;
            myDummy.transform.LookAt(_checkPoints[i + 1].Point);
            _checkPoints[i].QuaterniRotation = myDummy.transform.rotation;
        }

        Program.gameScene.ReturnUsedDummy(myDummy);
    }

    void CreateTheRouteObj()
    {
        TheRoute = new TheRoute(_checkPoints, _origenKey, _destinyKey);
    }

    /// <summary>
    /// Will return true if _curr and _two are really close 
    /// </summary>
    /// <returns></returns>
    bool CheckIfDone()
    {
        if (UMath.nearEqualByDistance(U2D.FromV2ToV3(_curr.Position), _two.Position, 0.1f))
        {
            return true;
        }
        return false;
    }
}
