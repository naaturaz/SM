using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crystal
{
    private string _id;
    private Vector2 _position;

    private string _name;//for creating lland zones

    //are 3 types:
    //Obstacles: tht including buildings and still elements
    //TerraObstacle:  montain obstacles
    //WaterObstacle: tht include water and
    //Way: for all way types
    private H _type;

    //who is the parent Id of this crystal
    private string _parentId;

    
    //Section use to order the routing 

    //the base weight for a crystal
    private int _baseWeight = 1000;
    //this like distance is calculated from other Vector3 and is only good if was recently set up
    private float _calcWeight;

    private float _sine;


    private bool _isDoor;

    //the lines tht this Crystal connect to
    //normally Cyrstals have only 3 lines. But LinkRect will need more than tht 
    List<Line> _lines = new List<Line>();
    private int _maxAmtLines = 3;

    //to help when organizing to link the marines, and others. Distance is a relaticve number
    //so should be used only if is recently set 
    private float _distance;

    SMe m = new SMe();

    //this is the stored positoin for Terra Obstavles to take after the Lines were created
    //this is for Lines dont be in the same spot tht the cristals . bz people walk alone crystals
    private Vector2 _positionAfterLines;

    //the crystals im linked to
    List<Crystal> _links = new List<Crystal>();
  

    //will hold extra information abt this crystal, created to hold wht crystal is in a buildin like NW,NE, etc
    private string _info;

    //will hold extra information abt this crystal, created to hold wht crystal is in a buildin 
    //wht his index in the anchors is from 0-3, from NW to SW
    private int _anchorIndex;


    List<Crystal> _siblings = new List<Crystal>();


    public float Distance
    {
        get { return _distance; }
        set { _distance = value; }
    }

    public Vector2 Position
    {
        get { return _position; }
        set { _position = value; }
    }

    public string ParentId
    {
        get { return _parentId; }
        set { _parentId = value; }
    }

    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public List<Line> Lines
    {
        get { return _lines; }
        set { _lines = value; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public H Type1
    {
        get { return _type; }
        set { _type = value; }
    }

    public float CalcWeight
    {
        get { return _calcWeight; }
        set { _calcWeight = value; }
    }

    public float Sine
    {
        get { return _sine; }
        set { _sine = value; }
    }

    public bool IsDoor
    {
        get { return _isDoor; }
        set { _isDoor = value; }
    }

    public string Info
    {
        get { return _info; }
        set { _info = value; }
    }

    /// <summary>
    ///     will hold extra information abt this crystal, created to hold wht crystal is in a buildin 
    /// wht his index in the anchors is from 0-3, from NW to SW
    /// </summary>
    public int AnchorIndex
    {
        get { return _anchorIndex; }
        set { _anchorIndex = value; }
    }

    /// <summary>
    /// if is a obstacle cristal will have siblings. the other crystalks belong to the same polygon
    /// building or stil element
    /// 
    /// dor or entry points of brdiges are not consiedred siblings only the poly crystals 
    /// </summary>
    /// <returns></returns>
    public List<Crystal> Siblings
    {
        get { return _siblings; }
    }




    public Crystal() { }

    public Crystal(Vector3 pos, H type, string parId, bool isDoor = false, bool setIdAndName = true)
    {
        _type = type;

        SetPosition(new Vector2(pos.x, pos.z));

        _parentId = parId;
        DefineBaseWeight();
        DefineMaxAmtLines();
        _isDoor = isDoor;

        //bz for when is creating crystrals for the CryRect is not need created a lot of garbage
        if (setIdAndName)
        {
            Id = Person.GiveRandomID();
            Name = Person.GiveRandomName();
        }
    }

    private void DefineMaxAmtLines()
    {
        if (_type == H.LinkRect)
        {
            _maxAmtLines = 15;
        }
        if (_type == H.LandZone)
        {
            _maxAmtLines = 8;
        }
        if (_type == H.Poll)
        {
            _maxAmtLines = 4;
        }
    }

    void SetPosition(Vector2 pos)
    {
        //if is a water obstavle need to push it away a bit from the sea bottom. so lines are closer to the shore line 
        //this position is only to create the lines 
        if (_type == H.WaterObstacle)
        {
            pos = m.MeshController.WaterBound1.ReturnPushMeAwayFromSeaBottom(pos, 1f);
        }
        else if (_type == H.MountainObstacle)
        {
            pos = m.MeshController.WaterBound1.ReturnPushMeAwayFromMountTop(pos, 1f);
        }

        _position = pos;
    }

    public float ReturnCalculateDistance(Vector2 otherPos)
    {
        return Vector2.Distance(_position, otherPos);
    }

    /// <summary>
    /// Will set the distance on the Cristal from 'otherPos'
    /// </summary>
    /// <param name="otherPos"></param>
    public void CalculateDistance(Vector2 otherPos)
    {
        _distance = Vector2.Distance(_position, otherPos);
    }


    private int weightFactor = 1;
    private int sineFactor = 10;
    internal void CalculateWeight(Vector3 vector3)
    {
        Distance = Vector3.Distance(U2D.FromV2ToV3(_position), vector3);
       // _calcWeight = (_baseWeight * weightFactor) + Distance;
        _calcWeight = _baseWeight + Distance;
    }

    public static DebugCrystal DebugCrystal = new DebugCrystal();
    public static List<string> passes = new List<string>(); 
    /// <summary>
    /// Will set the _calcWeight on the Cristal from 'otherPos'
    /// </summary>
    /// <param name="otherPos"></param>
    public void CalculateWeight(Vector2 curr, Vector2 final, string cryId)//loopCount only use for debug purpose
    {
        var pilotDistance = CalcPilotDistance(curr, final);
        //var pilot = Vector2.MoveTowards(curr, final, pilotDistance);

        var angle = AbsoluteAngleFrom3PointsInDegrees(final, Position, curr);
        var sine =  Math.Abs( (float)Math.Sin(ConvertToRadians(angle)));
        //Debug.Log("Angle: " + angle);

        var furtherWeight = CheckIfFurtherThanCurr(curr, final);

        //var posiToFin = Math.Abs( Vector2.Distance(Position, final));
        //var currToPosit =  Math.Abs(Vector2.Distance(curr, Position));

        var posiToFin = Math.Abs( Vector3.Distance(U2D.FromV2ToV3(Position), U2D.FromV2ToV3(final)));
        
        var currToPosit =  Math.Abs(Vector3.Distance(U2D.FromV2ToV3(curr), U2D.FromV2ToV3(Position)));

        //Distance = 3 * Vector2.Distance(_position, curr);//pilot
        Distance =  ( posiToFin + currToPosit );//pilot

        _calcWeight = // sine * sineFactor + 
            (Distance) 
            //+ furtherWeight
            ;

        var msg = "1st";
        if (cryId != null)
        {
            msg = cryId.Substring(0, 3);
        }
        passes.Add(msg);
        passes = passes.Distinct().ToList();

        //DebugCrystal.AddNewCrystals(passes.Count + ":: " + sine.ToString("F2") + " | " + angle.ToString("F1") + " | "
        //    + Distance.ToString("F1") + " |w: " +_calcWeight.ToString("f1"), Position);
    }

    /// <summary>
    /// If the position of the crystal we are now is further than curr to Final will 
    /// return a 100
    /// </summary>
    /// <param name="curr"></param>
    /// <param name="final"></param>
    /// <returns></returns>
    int CheckIfFurtherThanCurr(Vector2 curr, Vector2 final)
    {
        var distToFinalFromCurr = Vector2.Distance(final, curr);
        var distToFinalFromPosit = Vector2.Distance(final, Position);

        if (distToFinalFromPosit > distToFinalFromCurr)
        {
            return 1000;
        }
        return 0;
    }

    double ConvertToRadians(double angle)
    {
        return (Math.PI / 180) * angle;
    }

    private static float yDebug ;
    static Vector3 oldPos = new Vector3();
    void Debug()
    {
        var pos = new Vector3(_position.x, yDebug, _position.y);
        if (Vector3.Distance(oldPos, pos) < 0.2f)
        {
            yDebug += 0.2f;
        }
        else
        {
            yDebug = m.IniTerr.MathCenter.y + 0.2f;
        }
        pos = new Vector3(_position.x, yDebug, _position.y);
        oldPos = pos;

        UVisHelp.CreateHelpers(pos, Root.largeBlueCube);
        UVisHelp.CreateText(pos, _calcWeight.ToString("F1"), 40);
    }


    public float AbsoluteAngleFrom3PointsInDegrees(Vector2 oldPoint, Vector2 centerPoint, Vector2 newPoint)
    {
        double a = centerPoint.x - oldPoint.x;
        double b = centerPoint.y - oldPoint.y;
        double c = newPoint.x - centerPoint.x;
        double d = newPoint.y - centerPoint.y;

        double atanA = Math.Atan2(a, b);
        double atanB = Math.Atan2(c, d);

        return (float)Math.Abs((atanA - atanB) * (-180 / Math.PI));
        // if Second line is counterclockwise from 1st line angle is 
        // positive, else negative
    }


    private float CalcPilotDistance(Vector2 curr, Vector2 final)
    {
        var dist = Vector2.Distance(curr, final);
        //Debug.Log("dist"+dist);

        var medi = dist/2;



        //if distnace longee thn 5 will ret 5. otherwise retu dist
        if (medi > 5)
        {
            return 5;
        }
        if (medi < 1)
        {
            return 0;
        }
        return medi;
    }



    /// <summary>
    /// Depending on the type will define base weight 
    /// </summary>
    private void DefineBaseWeight()
    {
        if (_type == H.Obstacle)
        {
            _baseWeight = 40;
        }
        else if (_type == H.MountainObstacle || _type == H.WaterObstacle)
        {
            _baseWeight = 100;
        }
        else if (_type == H.Way3)//best way
        {
            _baseWeight = 5;
        }
        else if (_type == H.Way2)
        {
            _baseWeight = 6;
        }
        else if (_type == H.Way1)//worst way
        {
            _baseWeight = 7;
        }       
        //will rather to go to a RectCorner thn a TerraObstacle 
        else if (_type == H.RectCorner)
        {
            _baseWeight = 200;
        }    
        // bz always should be the 1st to try 
        else if (_type == H.Door)
        {
            _baseWeight = 0;
        }
        else
        {
            _baseWeight = 1000;
        }
    }






    private List<Crystal> _pool;
    //this is the index of the current Crystal on the _pool
    //use to organize the pool correctcly for LinkRect
    private int _poolIndex ;
    private int countCapacity = 50;
    /// <summary>
    /// Will link with the two closest tht dont have the same parent ID and 
    /// that dont have a same line ID 
    /// 
    /// the one is gonna be link is always gonna be element 0 of the pool ,
    /// will try to link to element 1,2, etc
    /// </summary>
    internal void Link(List<Crystal> pool, int index = 0)
    {
        _poolIndex = index;
        _pool = pool;
        OrganizeByDist();
        LinkTo();
    }

    /// <summary>
    /// Will reorgainze the first 100 so we allways keep closer to the ones we should be too
    /// </summary>
    private void OrganizeByDist()
    {
        var firstCry = this;

        var top = _pool.Count;
        if (top > DefineTop())
        {
            top = DefineTop();
        }

        for (int i = DefineStartPoolIndex(); i < top; i++)
        {
            _pool[i].CalculateDistance(firstCry.Position);
        }

        _pool = _pool.OrderBy(a => a.Distance).ToList();
    }

    /// <summary>
    /// Will define the top for OrganizeByDist()
    /// </summary>
    /// <returns></returns>
    int DefineTop()
    {
        int res = 0;

        var poolIndexStart = DefineStartPoolIndex();

        if (poolIndexStart == 0)
        {
            res=countCapacity;
        }
        else
        {
            //the strart pls the half of its caapacity
            res=poolIndexStart + (countCapacity / 2);
        }

        if (res > _pool.Count)
        {
            return _pool.Count;
        }
        return res;
    }

    int DefineStartPoolIndex()
    {
        if (_poolIndex == 0)
        {
            return 1;
        }
        else
        {
            //will remove 25 so we include the 25 before and the 25 after on the OrganizeByDist()
            _poolIndex -= (countCapacity/2);

            if (_poolIndex > -1)
            {
                return _poolIndex;
            }
            else
            {
                return 0;
            }
        }
    }

    private int index = 1;
    void LinkTo()
    {
        var add = IndexAddition();

        while (_pool.Count > 0 && index < _pool.Count)
        {
            var nextCrystal = _pool[index];

            if (CanILinkToThis(nextCrystal))
            {
                LinkAction(nextCrystal);
            }

            if (Lines.Count == _maxAmtLines )
            {
                index = 1;
                RemoveMySelFromPool();
                break;
            }

            index += add;
        }

        if (_pool.Count == 0)
        {
            MeshController.CrystalManager1.StopLinking(_type);
        }
        if (index >= _pool.Count)
        {
            index = 1;
            RemoveMySelFromPool();
        }
    }

    /// <summary>
    /// bz dep[ending on the type of cristals the addition on the while of index will be diff
    /// 
    /// for ex: in water bouns is used +4 so it goes further and make lines longer 
    /// ex: in mountains is +1 bz u need to have mountains togther
    /// </summary>
    /// <returns></returns>
    int IndexAddition()
    {
        if (_type == H.WaterObstacle)
        {
            return 4;
        }
        else if (_type == H.LinkRect || _type == H.LandZone)
        {
            return 1;
        }

        return 4;
    }

    /// <summary>
    /// the action of getting linked 
    /// </summary>
    /// <param name="nextCrystal"></param>
    private void LinkAction(Crystal nextCrystal)
    {
        //not render if is poll
        bool render = Type1 != H.Poll;



        Line tLine = new Line(this, nextCrystal, render);

        AssignLine(tLine);
        nextCrystal.AssignLine(tLine);

        AddToAccum(Vector2.Distance(Position, nextCrystal.Position));
        AddToLinks(nextCrystal);

        GetLinksFromNext(nextCrystal);
        RenameAsNextLinkRect(nextCrystal);
    }

    /// <summary>
    /// Take care of the linking of a LandZone 
    /// </summary>
    private void LinkALandZone(string nextCrystal)
    {
        if (_type != H.LandZone)
        {
            return;
        }

        var l = (LandZone) this;
        l.LandZoneName = nextCrystal;
        l.RenameAllMyLinkRects(nextCrystal);
    }

    /// <summary>
    /// Will add all my links to next so we are connected forever
    /// </summary>
    /// <param name="nextCrystal"></param>
    private void GetLinksFromNext(Crystal nextCrystal)
    {
        AddExternalLinks(nextCrystal.Links());
    }

    public List<Crystal> Links()
    {
        return _links;
    }

    /// <summary>
    /// Will add to _links if is not there already
    /// </summary>
    /// <param name="newLink"></param>
    void AddToLinks(Crystal newLink)
    {
        if (!_links.Contains(newLink))
        {
            _links.Add(newLink);
        }
    }

    /// <summary>
    /// Another Crystal adding all its links to this obj
    /// </summary>
    /// <param name="newLinks"></param>
    void AddExternalLinks(List<Crystal> newLinks)
    {
        for (int i = 0; i < newLinks.Count; i++)
        {
            AddToLinks(newLinks[i]);
        }
    }

    /// <summary>
    /// Link Rects will be giving to linked new Crystal thir own name, bz I need to have all wth the same name 
    /// </summary>
    /// <param name="nextCrystal"></param>
    private void RenameAsNextLinkRect(Crystal nextCrystal)
    {
        if (_type == H.LinkRect || _type ==H.LandZone)
        {
            Rename(nextCrystal.Name);
        }
    }

    /// <summary>
    /// Will rename the obj to new name and all its links crystalls
    /// </summary>
    /// <param name="Name"></param>
    private void Rename(string newName)
    {
        Name = newName;

        for (int i = 0; i < _links.Count; i++)
        {
            if (newName != _links[i].Name)
            {
                _links[i].Rename(newName);    
            }
        }

        LinkALandZone(newName);
    }

    private void SetNewNameAndLinks(string newName)
    {
        Name = newName;

        for (int i = 0; i < _links.Count; i++)
        {
            _links[i].Name = newName;
        }
    }

    /// <summary>
    /// Will assigin line to the available line spot 
    /// </summary>
    public void AssignLine(Line tLine)
    {
        if (Lines.Count < _maxAmtLines)
        {
            Lines.Add(tLine);;
        }

        //else
        //{
        //    throw new Exception("One line must be null. Then when asking CanILinkToThis() is not good");
        //}
    }

    /// <summary>
    /// Will return true if can be linked 
    /// </summary>
    /// <param name="nextCrystal"></param>
    /// <returns></returns>
    bool CanILinkToThis(Crystal nextCrystal)
    {
        if (!CanLinkRectOrLandZone(nextCrystal))
        {
            return false;
        }

        if (Type1 == H.Poll)
        {
            return PollConditions();
        }

        //is thesame crystal
        if (nextCrystal.Id == Id)
        {
            return false;
        }

        if (IsACousin(nextCrystal))
        {
            return false;
        }

        if (Lines.Count == _maxAmtLines || nextCrystal.Lines.Count == _maxAmtLines)
        {
            return false;
        }

        if (HasAnyLineConnectedWithSameId(nextCrystal))
        {
            return false;
        }
        if (IsTooFar( nextCrystal))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Created bz poll Crystal doesnt need anything else to be checked
    /// </summary>
    /// <returns></returns>
    private bool PollConditions()
    {
        if (Lines.Count == _maxAmtLines)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Will tell u if can link a LinkRect. They cant intersect Terra Bound Lines
    /// </summary>
    /// <param name="nextCrystal"></param>
    /// <returns></returns>
    bool CanLinkRectOrLandZone(Crystal nextCrystal)
    {
        if (_type != H.LinkRect && _type != H.LandZone && _type != H.Poll)
        {
            return true;
        }

        Line tLine = new Line(this, nextCrystal, false);

        return !MeshController.CrystalManager1.DoIIntersectAnyLine(tLine, Position, H.WaterObstacle);
    }

    /// <summary>
    /// To avoid the linking of far mountains
    /// </summary>
    /// <param name="nextCrystal"></param>
    /// <returns></returns>
    private bool IsTooFar(Crystal nextCrystal)
    {
        int tolerance = DefineAverageTolerance();
        DefineAveraReadyAmt();

        var dist = Vector2.Distance(Position, nextCrystal.Position);
        var aver = AverageDist();

        //if the distance is twice the average then not . no linking
        if (dist > aver * tolerance && IsAverReady())//6
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// BZ for exam for LinkRect dont need to wait for 40 elements for have a good average amout
    /// </summary>
    private void DefineAveraReadyAmt()
    {
        if (_type == H.LinkRect || _type == H.LandZone)
        {
            averaReadyAmt = 30;
        }
    }

    private int DefineAverageTolerance()
    {
        if (_type == H.LinkRect || _type == H.LandZone)
        {
            return 4;
        }
        return 6;
    }

    /// <summary>
    /// if the other is link by a line to any of the one im conected to then is a cousin
    /// </summary>
    /// <param name="nextCrystal"></param>
    /// <returns></returns>
    private bool IsACousin(Crystal nextCrystal)
    {
        if (Lines.Count == 0)
        {
            return false;
        }
        //if the lines contain the id on next then are cousing so no linking allow 
        for (int i = 0; i < Lines.Count; i++)
        {
            if (Lines[i].Id.Contains(nextCrystal.Id))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Bz if is a Water obstacle for ex. the crystal cant be on the line. other wise will intersect
    /// while a person is walking alone the line 
    /// </summary>
    void SetPositionAfterLines()
    {
        //if is a water obstavle need to push it away a bit from the sea bottom. So people can use the crystals 
        //if they are a bit awy from the seaBoundsLines 
        if (_type == H.WaterObstacle)
        {
            _position = m.MeshController.WaterBound1.ReturnPushMeAwayFromSeaBottom(_position, 2.5f); 
            UVisHelp.CreateHelpers(new Vector3(_position.x, m.IniTerr.MathCenter.y, _position.y), Root.yellowCube);
        }
        else if (_type == H.MountainObstacle)
        {
            _position = m.MeshController.WaterBound1.ReturnPushMeAwayFromMountTop(_position, 2.5f);
            UVisHelp.CreateHelpers(new Vector3(_position.x, m.IniTerr.MathCenter.y, _position.y), Root.yellowCube);
        }
    }

    /// <summary>
    /// will remove this Crystal form pool 
    /// </summary>
    private void RemoveMySelFromPool()
    {

        if (_type == H.Poll)
        {
            MeshController.CrystalManager1.RemoveCrystalPoll();
            return;
        }

        if (_type != H.LinkRect && _type != H.LandZone)
        {
            MeshController.CrystalManager1.RemoveCrystal(this, _type);
        }
        
        SetPositionAfterLines();

    }

    /// <summary>
    /// Will return true if the other param has any line coneected with this obj
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool HasAnyLineConnectedWithSameId(Crystal other)
    {
        if (Lines.Count == 0 || other.Lines.Count == 0)
        {
            return false;
        }

        for (int i = 0; i < Lines.Count; i++)
        {
            for (int j = 0; j < other.Lines.Count; j++)
            {
                if (Lines[i].Id == other.Lines[j].Id)
                {
                    return true;
                }
            }
        }

        
   
        return false;
    }







    #region Accumulative Numbers


    private static int crystalsAccum;
    private static float distanceAccum;
    private int averaReadyAmt = 40;

    static void AddToAccum(float dist)
    {
        crystalsAccum++;
        distanceAccum += dist;
    }

    /// <summary>
    /// Will tell u the average distance of a crystal to other in current Set of Cristals 
    /// 
    /// use to avoid mountains far away linking to each other 
    /// </summary>
    /// <returns></returns>
    float AverageDist()
    {
        return distanceAccum/crystalsAccum;
    }

    bool IsAverReady()
    {
        //must be at least 20 to give a number
        if (crystalsAccum < averaReadyAmt)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Reset the accummulation stats. used for average calc
    /// </summary>
    static public void ResetAccumNumbers()
    {
        crystalsAccum = 0;
        distanceAccum = 0;
    }

#endregion


    /// <summary>
    /// Created for LinkRect they need to be relink a few times to be named all by Zones
    /// </summary>
    public void Clean()
    {
        //Links.Clear();
        Lines.Clear();
    }

    public bool IsTerrainObstacle()
    {
        if (Type1 == H.WaterObstacle || Type1 == H.MountainObstacle)
        {
            return true;
        }
        return false;
    }

    public void SetSiblings(List<Crystal> allPolyCrystals)
    {


        for (int i = 0; i < allPolyCrystals.Count; i++)
        {
            if (this != allPolyCrystals[i])
            {
                _siblings.Add(allPolyCrystals[i]);
            }
        }
    }
}

public class DebugCrystal
{
    SMe m = new SMe();
    public string Info;
    public Vector3 _position;

    List<DebugCrystal> debug = new List<DebugCrystal>();
    List<General> gameObjects = new List<General>();

    private bool shownNow;

    public DebugCrystal() { }

    public DebugCrystal(string info, Vector3 pos)
    {
        Info = info;
        _position = pos;
    }


    public void AddNewCrystals( string addInfo, Vector3 pos)
    {
        //if (shownNow)
        //{
        //    Restart();
        //}

        var index = IsHereAlready(pos);

        if (index == -1)
        {
            debug.Add(new DebugCrystal(addInfo, pos));
        }
        else
        {
            debug[index].Info = addInfo + "\n" + debug[index].Info;
        }
    }

    public void ShowNow()
    {
        for (int i = 0; i < debug.Count; i++)
        {
            ShowEach(debug[i]);
        }
        Debug.Log(debug.Count+" debuc ct");
        //shownNow = true;
    }

    void ShowEach(DebugCrystal each)
    {
        var locPos = each._position;
        var locInf = each.Info;

        var yDebug = m.IniTerr.MathCenter.y + 0.2f;
        var pos3 = new Vector3(locPos.x, yDebug, locPos.y);

        gameObjects.Add( UVisHelp.CreateHelpers(pos3, Root.largeBlueCube));
        gameObjects.Add(UVisHelp.CreateText(pos3, locInf, 15));
    }

    public void Restart()
    {
        debug.Clear();

        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].Destroy();
        }
        gameObjects.Clear();
    }

    /// <summary>
    /// Will return position on List if exist other wise will return -1
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    int IsHereAlready(Vector3 pos)
    {
        for (int i = 0; i < debug.Count; i++)
        {
            if (Vector3.Distance(debug[i]._position,  pos) < 1f)
            {
                return i;
            }
        }
        return -1;
    }

    public void AddGameObjInPosition(Vector3 pos, string root)
    {
        gameObjects.Add(UVisHelp.CreateHelpers(pos, root));
    }


}
