using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/* Find the route of Point A and B if they need to go trhu bridges
 * 
 * In the algorithm Point A will throw ray then if water will reach the closest
 * bridge to collision point. If that bridge is not marked as used and that bridge
 * can see other bridges, will store new Position, then it will move on
 * 
 * The point B do will do the same will trhow ray to current position on point A and will 
 * follow same algorithm 
 * 
 * If they cant connect then that building is unreacheable to Person at least with this brdsiges setup
 * so the building is blackListed in the person object 
 * 
 * The final product of this class is a collection of pairs of _ini, _fin Structures so they are pass to 
 * Diff routers so the routers can build the route on terrrain
 * 
 * Really happy and proud of the algorithm I created on Paper 
 * 
 * Jun 11 2015
 * Regardless the algorithm in this class is only covering the case of two extensions of land.
 * Could be two islands or a terrain with only 1 river 
 */
public class BridgeRouter : MonoBehaviour  {

    private Router _router;
    private Vector3 _iniPos;
    private Vector3 _finPos;
    private Person _person;
    //the brdiges we havent marked as used yet
    List<Bridge> _virginBridges = new List<Bridge>();
   
    /*the bridges are used are the ones crossed, or the ones seen while crossing.
     *Ex: When in an Island a house has access to 3 diff bridges and decides to cross one,
     *the other two plus the one crossed gets added to below list and removed from __virginBridges
     */
    List<Bridge> _usedBridges = new List<Bridge>();
    
    List<Bridge> _reachBridges = new List<Bridge>();//brdiges I can reach from '_currentIni' point
    List<Bridge> _unReachBridges = new List<Bridge>();//brdiges I can not reach from '_currentIni' point
  
    //brdiges I can reach from '_currentIni' point but cant walk directly from them to "_currentFin'
    List<Bridge> _reachBridgesFail = new List<Bridge>();

    //From each _reachBridgesFail eleemtn I will try to reach the left bridges at _virginBridges... If I cant reach them, 
    //I will be adding on this list temporaraly for each element of _reachBridgesFail
    List<Bridge> _triedBridges = new List<Bridge>();

    private bool _recurseNow;

    //will be used recursively
    private Vector3 _currentHitOnWater;
    private Bridge _currentBridge;
    private Vector3 _currentIni;//current Ini point that is changeable as we aproach the fin
    private Vector3 _currentFin;
    private Structure _ini;
    private Structure _fin;

    private bool lookingForSeeNow;//Says if is looking for all Posible bridges that can walk to now 
    HPers _routeType = HPers.None;

    private List<CheckPoint> bucket = new List<CheckPoint>();//checks points to be validated 
    List<CheckPoint> validCheckPoints = new List<CheckPoint>();//checkpoint validated they will be used for last result
    
    TheRoute _theRoute = new TheRoute();//the final route of this classs

    public Router FinalRouter
    {
        get { return _finalRouter; }
        set { _finalRouter = value; }
    }

    Router _finalRouter = new Router();

    public BridgeRouter() { }

    public BridgeRouter(Structure ini, Structure fin, Person person, HPers routeType)
    {
        _ini = ini;
        _fin = fin;
        _iniPos = _ini.SpawnPoint.transform.position;


        
        _finPos = _fin.SpawnPoint.transform.position;
        
        
        
        _person = person;
        _routeType = routeType;
        _currentIni = _iniPos;
        _currentFin = _finPos;
        
        Init();
    }

    void RestartVar()
    {
        //all can Reach Started bool
        _canIReach.SetIsStartedToFalse();
        _canIReachCross.SetIsStartedToFalse();

        nullBridgeCount = 0;
    }

    private void Init()
    {
        _recurseNow = true;
       
        RestartVar();
        DefineBridges(_currentIni);

        //is asked since I could stoped on 'DefineBridges()' if called 'BlackList()'
        if (_recurseNow)
        {
            LeaveOnlyTheOnesFullyBuiltAndUnMarked();
            //could stop '_recurseNow' below too
            CheckAmountOfBridgesNow();
        }
    }

    /// <summary>
    /// Created to see how many brdiges are after they are descriminated... 
    /// 
    /// If they are fully built and Unmarked as WillBeDestroy
    /// 
    /// Will stop the router right here if brdiges are
    /// zero other wise will activate recursion with '_recurseNow = true'
    /// </summary>
    private void CheckAmountOfBridgesNow()
    {
        if (_virginBridges.Count==0)
        {
            BlackListBuild();
            Stop();
        }
    }

    void Stop()
    {
        _recurseNow = false;
    }

    /// <summary>
    /// Main Method
    /// 
    /// The recursive routine on the class 
    /// </summary>
    void RecurseRoutine()
    {
        if (_currentHitOnWater == new Vector3())
        {
            _currentHitOnWater = TryReachOther(_currentIni, _currentFin).point;
            print("_currentHitOnWater:" + _currentHitOnWater);
        }
        if ((lookingForSeeNow || canIReach == -5) && _currentBridge != null)
        {
            Find1stReachBrdge();
            return;
        }

        if (_currentBridge == null )
        {
            _currentBridge = FindClosestBridge(_virginBridges, _currentHitOnWater);
            AvoidInfiniteRecursion();
        }
        //means we wnt thru all bridges and we could not route 
        else if (_currentBridge == null && _virginBridges.Count == 0)
        {
            BlackListBuild();
            Stop();
        }
        else if (_currentBridge != null && _virginBridges.Count > 0)
        {
            CrossBridge();
            DecideIfCrossingLastBridgeWasEnough();
        }
    }

    private int nullBridgeCount;
    /// <summary>
    /// Really bad way to avoid InfiniteRecursion when keeps returining null bridge bz CanIReach cant determine 
    /// </summary>
    void AvoidInfiniteRecursion()
    {
        nullBridgeCount++;

        if (nullBridgeCount > 100)
        {
            BlackListBuild();
            Stop();
        }
    }

    #region Reachable Brdiges

    private int canIReach = -5;
    CanIReach _canIReach = new CanIReach();
    private void Find1stReachBrdge()
    {
        CheckFindReachBrigeStatus();
       
        //wont go in if lookingForSeeNow is false 
        if (!lookingForSeeNow)
        {
            return;
        }
        canIReach = _canIReach.Bean.Result;

        if (!_canIReach.IsStarted && canIReach==-5)
        {
            _canIReach = new CanIReach(_currentIni, _currentBridge, _person);
            print("_canIReach assignemtn:" + _currentBridge.MyId);
        }
        else if (canIReach == 1  )
        {
            lookingForSeeNow = false;
        }
        else if(canIReach == -1)
        {
            throw new Exception("All Bridges should be reacheable ");
        }
    }

    /// <summary>
    /// Says , and control if we are init or finish witthj FindAllReachBrdges()
    /// </summary>
    void CheckFindReachBrigeStatus()
    {
        if (_reachBridges.Count + _unReachBridges.Count == _virginBridges.Count || _virginBridges.Count == 0)
        {
            lookingForSeeNow = false;
            return;
        }
        if (_reachBridges.Count == 0 && _unReachBridges.Count == 0)
        {
            lookingForSeeNow = true;
        }
    }
    #endregion


    #region Crossing Brdige
    private int canIWalk = -5;
    CanIReach _canIReachCross = new CanIReach();

    void CrossBridge()
    {
        print("_currentBridge found:" + _currentBridge.name);
        if(!_canIReachCross.IsStarted)
        {
            print("Assigning _canIReachCross");
            _canIReachCross = new CanIReach(_currentFin, _currentBridge, _person, true);
        }
        //wants to check and update again to see if any new value was set
        canIWalk = _canIReachCross.Bean.Result;
    }

    void DecideIfCrossingLastBridgeWasEnough()
    {
        if (canIWalk == 1)
        {
            print("canIWalkThereNow:" + IsIntTrue(canIWalk) + ":" + _currentBridge.name);
            ConformRoute();
            _canIReachCross.Bean.CleanBean();
            Stop();
        }
        else if(canIWalk == -1)
        {
            throw new Exception("Must be able to walk there, only 2 land extensions are allowed per Terrain");
        }
    }

    private void ConformRoute()
    {
        AddToBucket();
        AddFirtAndLastToBucket();
        ValidateRoute();
        _theRoute = new TheRoute(validCheckPoints, _ini.MyId, _currentBridge.MyId, _fin.MyId);
        AddPersonOnBridgeDict();

        _finalRouter.TheRoute = _theRoute;
        _finalRouter.IsRouteReady = true;
    }

    private void AddPersonOnBridgeDict()
    {
        _person.Brain.AddToBridgePeopleList(_currentBridge.MyId, _person.MyId);
    }

    /// <summary>
    /// Adds the first with Rotation. 
    /// Set the Rotation of last before last one
    /// Adn then add last one 
    /// </summary>
    void AddFirtAndLastToBucket()
    {
        //add first with rotation and all
        var chkPoint = ReturnFacingTo(_ini.BehindMainDoorPoint, bucket[0].Point, _ini.BehindMainDoorPoint);

        bucket.Insert(0, chkPoint);

        //get the Rotation of the one b4  last one 
        var chkPoint2 = ReturnFacingTo(bucket[bucket.Count - 1].Point, _fin.BehindMainDoorPoint
            , bucket[bucket.Count - 1].Point);

        //remove it 
        bucket.RemoveAt(bucket.Count - 1);

        bucket.Add(chkPoint2);

        //last One
        bucket.Add(new CheckPoint(_fin.BehindMainDoorPoint));
    }

    /// <summary>
    /// Creates a CheckPoint
    /// </summary>
    /// <param name="from">Where will stan to look at from</param>
    /// <param name="facingTo">Point that will look at</param>
    /// <param name="iniPos">The Point of the CHeckPoint, which most of the time is the same as 'from'</param>
    /// <returns></returns>
    CheckPoint ReturnFacingTo(Vector3 from, Vector3 facingTo, Vector3 iniPos)
    {
        CheckPoint re = new CheckPoint(iniPos);
        GameScene.dummyBlue.transform.position = from;

        //so it doesnt tilt when going up or down the brdige hill 
        //im putting in the same height on Y as the next point 
        GameScene.dummyBlue.transform.position = new Vector3(GameScene.dummyBlue.transform.position.x, facingTo.y, GameScene.dummyBlue.transform.position.z);
        GameScene.dummyBlue.transform.LookAt(facingTo);

        re.QuaterniRotation = GameScene.dummyBlue.transform.rotation;

        GameScene.dummyBlue.transform.position = new Vector3();
        return re;
    }

    void AddToBucket()
    {
        bucket.Clear();
        
        bucket.AddRange(_canIReach.Bean.Route.CheckPoints);
    
        //UVisHelp.CreateTextEnumarate(_canIReach.Bean.Route.CheckPoints);
        //UVisHelp.CreateTextEnumarate(_canIReach.Bean.TheRouteInBridge.CheckPoints, "b");

        //works perfect for instances where DeltaCapsule is not used 
        //so the last point on the brdige is not diff than first on _canIReachCross
        var l = _canIReach.Bean.TheRouteInBridge.CheckPoints;
        l.RemoveAt(l.Count - 1);
        bucket.AddRange(l);
        
        bucket.AddRange(_canIReachCross.Bean.Route.CheckPoints);
    }

    void ValidateRoute()
    {
        validCheckPoints.Clear();
        for (int i = 0; i < bucket.Count - 1; i++)
        {
            //if is not the same 
            if (!UMath.nearEqualByDistance(bucket[i].Point, bucket[i + 1].Point, 0.1f))
            {
                validCheckPoints.Add(bucket[i]);
            }
        }
        //add the last one 
        validCheckPoints.Add(bucket[bucket.Count - 1]);
    }

    /// <summary>
    /// Converts the class integer Async language to Bool... if Int ==1 is true otherwise false
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    bool IsIntTrue(int i)
    {
        if (i==1)
        {
            return true;
        }
        return false;
    }
    #endregion

    RaycastHit TryReachOther(Vector3 a, Vector3 b)
    {
        return URayCast.FindObjOnMyWay(a, b, 4, Color.magenta);//4 is the Water Layer index
    }

    /// <summary>
    /// Will Define the birdges in a order from the 'iniPos' param
    /// </summary>
    void DefineBridges(Vector3 iniPos)
    {
        _virginBridges = FindBridges();
        if (_virginBridges.Count == 0)
        {
            BlackListBuild();
            Stop();
            return;
        }
        _virginBridges = OrderBridgesFrom(_virginBridges, iniPos);
    }

    /// <summary>
    /// Wil leave only the fully built brdiges in '_virginBridges'
    /// and will remove too the ones has a mark on 'Instruction = H.WillBeDestroy'
    /// </summary>
    void LeaveOnlyTheOnesFullyBuiltAndUnMarked()
    {
        for (int i = 0; i < _virginBridges.Count; i++)
        {
            //current stage 4 is done
            if (_virginBridges[i].Pieces[0].CurrentStage != 4 || _virginBridges[i].Instruction == H.WillBeDestroy) 
            {
                print("Bridg Removed:" + _virginBridges[i].MyId);
                _virginBridges.RemoveAt(i);
                i--;
            }
        }
    }

    List<Bridge> FindBridges()
    {
        List<Bridge> res = new List<Bridge>();
        for (int i = 0; i < BuildingPot.Control.Registro.Ways.Count; i++)
        {
            Way way = BuildingPot.Control.Registro.Ways.ElementAt(i).Value;

            if (way ==null)
            {
                continue;
            }

            if (way.name.Contains(H.Bridge.ToString()))
            {
                res.Add(way as Bridge);
            }
        }
        return res;
    }

    private List<Bridge> OrderBridgesFrom(List<Bridge> list, Vector3 from)
    {
        List<Bridge> res = new List<Bridge>();
        List<BridgeHelp> help = new List<BridgeHelp>();

        //finds the distance by Creating BridgeHelp instances
        for (int i = 0; i < list.Count; i++)
        {
            help.Add(new BridgeHelp(list[i], from));
        }
        //ordered by Dist
        help = help.OrderBy(a => a.Dist).ToList();

        //take them out of the help
        for (int i = 0; i < help.Count; i++)
        {
            res.Add(help[i].Bridge);
        }
        return res;
    }


    /// <summary>
    /// Find closest bridge to 'from'
    /// and orders the list from 'from'
    /// </summary>
    /// <param name="list"></param>
    /// <param name="from"></param>
    /// <returns></returns>
    private Bridge FindClosestBridge(List<Bridge> list, Vector3 from)
    {
        //and order 'param list' 
        list = OrderBridgesFrom(list, from);
        //we ned to use here the _iniPos bz wil start walkign from there 
        Bridge res = GetClosestBridgeICanWalkTo(list, _currentIni);
        return res;
    }


    CanIReach _canIReachBridge = new CanIReach();
    private int index;
    /// <summary>
    /// This recursive will keep asking him self first if current brdige can be 
    /// walked to... and after the Async call to CanIWalkToBridge have decided if can or not
    /// 
    /// Then will move to the next brdige 
    /// </summary>
    /// <param name="br">A llist of brdiges ordered 'from'</param>
    /// <param name="from">The from </param>
    /// <returns>A bridge we can walk to if is one, otherwise null</returns>
    Bridge GetClosestBridgeICanWalkTo(List<Bridge> br, Vector3 from)
    {
        if (index < br.Count)
        {
            if (!_canIReachBridge.IsStarted)
            {
                _canIReachBridge = new CanIReach(from, br[index], _person);
            }
            //wants to check and update again to see if any new value was set
            int flag = _canIReachBridge.Bean.Result;


            //means CanIWalkToBridge is posbile 
            if (flag == 1)
            {
                _canIReachBridge.Restart();
                _canIReachBridge.Bean.CleanBean();
                return br[index];
            }
            //means cant walk there
            else if (flag == -1)
            {
                index++;
                _canIReachBridge.Bean.CleanBean();
                GetClosestBridgeICanWalkTo(br, from);
            }
            //mens is still processing so I need to ask again
            else if (flag == -5)
            {
                //onces the Delta Capsule has found is routable or not will call again RecursiveRoutine()
                return null;
            }
        }
        //means that either we went thru all bridges or flag is -100
        index = 0;
        _canIReachBridge.Restart();
        return null;
    }

    /// <summary>
    /// Black List a building on the Person Object so it knows cant reach that building at least with the current
    /// set up of bridges
    /// </summary>
    private void BlackListBuild()
    {
        _person.Brain.BlackListBuild(_fin.MyId);
    }

    public void Update()
    {
        if (!_recurseNow)
        {
            return;
        }

        _canIReach.Update();
        _canIReachBridge.Update();
        _canIReachCross.Update();

        bool activeReaches = _canIReach.IsActive() || _canIReachBridge.IsActive()
                             || _canIReachCross.IsActive();

        //will recurse only if...
        if (_recurseNow && !activeReaches)
        {
            RecurseRoutine();
        }
    }
}
