﻿/*
 * IMPORTANT: IF LANDZONES ARE NOT SET THE ROUTING SYSTEM WONT WORK
 * 
 * 
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrystalManager  {

    SMe m = new SMe();

    //all the regions 
    List<CrystalRegion> _crystalRegions = new List<CrystalRegion>();

    private bool load;

    //all the crystals on the regions . for GC purpose
    List<Crystal> _all = new List<Crystal>(); 
    
    //only use for finding them 
    List<Crystal> _allObstas = new List<Crystal>(); 


    public List<CrystalRegion> CrystalRegions
    {
        get { return _crystalRegions; }
        set { _crystalRegions = value; }
    }

    public CrystalManager()
    {
        load = true;
    }

    /// <summary>
    /// Will remove the crystals from _crystalRegions
    /// </summary>
    /// <param name="building"></param>
    public void Delete(Building building)
    {
        //search _crystalRegions points are
        var indexes = ReturnRegionsOfPointsInStructure(building);
        DeleteCrystals(indexes, building.MyId);
    }   
    
    public void Delete(StillElement still)
    {
        //search _crystalRegions points are
        var indexes = ReturnRegionsOfPointsInStructure(still);
        DeleteCrystals(indexes, still.MyId);
    }

    /// <summary>
    /// Will delete all crystals related to tht building 
    /// </summary>
    private void DeleteCrystals(List<int> indexes, string myIdP)
    {
        //remove from each _crystalRegion
        for (int i = 0; i < indexes.Count; i++)
        {
            var indexLoc = indexes[i];
            CrystalRegions[indexLoc].RemoveCrystal(myIdP);
        }
        //dont need to resave bz the adding of a build is done once is spwaned 
    }



    /// <summary>
    /// Will return the region index where all the StillElement points are
    /// </summary>
    /// <param name="still"></param>
    /// <returns></returns>
    List<int> ReturnRegionsOfPointsInStructure(StillElement still)
    {
        List<Vector3> points = new List<Vector3>();

        var obstas = _allObstas.Where(a => a.ParentId == still.MyId).ToList();
        for (int i = 0; i < obstas.Count; i++)
        {
            points.Add(U2D.FromV2ToV3(obstas[i].Position));
            _allObstas.Remove(obstas[i]);
        }

        List<int> res = new List<int>();
        for (int i = 0; i < points.Count; i++)
        {
            res.Add(ReturnMyRegion(U2D.FromV3ToV2(points[i])));
        }
        return res.Distinct().ToList();
    }


    /// <summary>
    /// Will return the Crystals that belong to the building pass aas param
    /// </summary>
    public List<Crystal> ReturnCrystalsThatBelongTo(Building building, bool includeDoor)
    {
        List<Crystal> res =new List<Crystal>();
        //search _crystalRegions points are
        var indexes = ReturnRegionsOfPointsInStructure(building);
        for (int i = 0; i < indexes.Count; i++)
        {
            var indexLoc = indexes[i];
            res.AddRange(CrystalRegions[indexLoc].ObstaCrystals);
        }

        var type = WhichType(includeDoor);

        if (res.Count > 0)
        {
            res = res.Where(a => a.ParentId == building.MyId && a.Type1 == type).ToList();
        }
        return res;
    }  
    
    public List<Crystal> ReturnCrystalsThatBelongTo(StillElement still, bool includeDoor)
    {
        List<Crystal> res =new List<Crystal>();
        //search _crystalRegions points are
        var indexes = ReturnRegionsOfPointsInStillElement(still);
        for (int i = 0; i < indexes.Count; i++)
        {
            var indexLoc = indexes[i];
            res.AddRange(CrystalRegions[indexLoc].ObstaCrystals);
        }

        var type = WhichType(includeDoor);

        if (res.Count > 0)
        {
            res = res.Where(a => a.ParentId == still.MyId && a.Type1 == type).ToList();
        }
        return res;
    }

    /// <summary>
    /// Will return H.Door if include door was marked on
    /// </summary>
    /// <param name="includeDoor"></param>
    /// <returns></returns>
    H WhichType(bool includeDoor)
    {
        if (includeDoor)
        {
            return H.Door;
        }
        return H.Obstacle;
    }

    /// <summary>
    /// Will return the region index where all the Structure points are
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    List<int> ReturnRegionsOfPointsInStructure(Building building)
    {
        //nt for Trails 
        List<Vector3> points = new List<Vector3>();
        if (!building.MyId.Contains("Trail"))
        {
            points = PassAnchorsGetPositionForCrystals(building.Anchors);
        }


        //for buildings
        if (!building.MyId.Contains("Bridge") && !building.MyId.Contains("Trail"))
        {
            Structure st = (Structure)building;
            if (st.SpawnPoint != null)
            {
                points.Add(st.SpawnPoint.transform.position);
            }
        }
        //for bridges
        if (building.MyId.Contains("Bridge"))
        {
            var entries = GetBridgeEntries((Bridge)building);

            points.Add(U2D.FromV2ToV3(entries[0].Position));
            points.Add(U2D.FromV2ToV3(entries[1].Position));
        }       
        //for Ways that are Trails
        if (building.MyId.Contains("Trail"))
        {
            var obstas = _allObstas.Where(a=>a.ParentId==building.MyId).ToList();

            for (int i = 0; i < obstas.Count; i++)
            {
                points.Add(U2D.FromV2ToV3( obstas[i].Position));
                _allObstas.Remove(obstas[i]);
            }
        }

        List<int> res = new List<int>();
        for (int i = 0; i < points.Count; i++)
        {
            res.Add(ReturnMyRegion(U2D.FromV3ToV2(points[i])));
        }
        return res.Distinct().ToList();
    }

    /// <summary>
    /// Will return the region index where all the Structure points are
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    List<int> ReturnRegionsOfPointsInStillElement(StillElement still)
    {
        var points = PassAnchorsGetPositionForCrystals(still.Anchors);

        List<int> res = new List<int>();
        for (int i = 0; i < points.Count; i++)
        {
            res.Add(ReturnMyRegion(U2D.FromV3ToV2(points[i])));
        }
        return res.Distinct().ToList();
    }

    //the bridges entries will act like doors 
    List<Crystal> GetBridgeEntries(Bridge b)
    {
        List<Crystal> re = new List<Crystal>();

        re.Add(new Crystal(b.LandZone1[0].Position, H.Obstacle, b.MyId, true));
        re.Add(new Crystal(b.LandZone1[1].Position, H.Obstacle, b.MyId, true));

        return re;
    }





    //private string _info;//info that will be added to the crystal
    //the siblings of current crustal
    List<Crystal> _siblings = new List<Crystal>(); 

    public void Add(Building building)
    {
        _siblings.Clear();

        if (building.MyId.Contains("Bridge"))
        {
            AddBridge((Bridge) building);
        }
        else AddBuilding(building);
    }

    public void Add(StillElement still)
    {
        //_info = "Still";
        _siblings.Clear();
        AddPoly(still.Anchors, still.MyId);
    }

    /// <summary>
    /// For adding ways 
    /// </summary>
    /// <param name="wayPos">The position</param>
    public void Add(Vector3 wayPos, Trail trail)
    {
        //if (_siblings.Count > 0 && _siblings[0].ParentId != trail.MyId)
        //{
        //    _siblings.Clear();
        //}

        Crystal c = new Crystal(wayPos, H.Way1, trail.MyId, false);

        AddCrystalToItsRegion(c);

//       //Debug.Log("Crys added:" + trail.MyId);
        //_siblings.Add(c);
    }

    /// <summary>
    /// When adding a building Crystals to its region
    /// </summary>
    /// <param name="building"></param>
    void AddBuilding(Building building)
    {
        AddPoly(building.Anchors, building.MyId);

        //adding a door
        Structure st = (Structure)building;
        if (st.SpawnPoint == null)
        {
            CreateAndAddPolyCrystal(st.SpawnPoint.transform.position, null, st.MyId, -100, true);
        }
    }

    /// <summary>
    /// Will set the siblings in each sibling member 
    /// </summary>
    private void SetSiblings()
    {
        for (int i = 0; i < _siblings.Count; i++)
        {
            _siblings[i].SetSiblings(_siblings);
        }
    }

    void AddBridge(Bridge b)
    {
        AddPoly(b.GetBridgeAnchors(), b.MyId);
        AddBridgeEnds(b);
    }

    /// <summary>
    /// Add the bridge 2 ends to crystals 
    /// 
    /// not adding this to siblings 
    /// </summary>
    /// <param name="b"></param>
    private void AddBridgeEnds(Bridge b)
    {
        //the bridges entries will act like doors 
        Crystal entry1 = new Crystal(b.LandZone1[0].Position, H.Obstacle, b.MyId, true);
        Crystal entry2 = new Crystal(b.LandZone1[1].Position, H.Obstacle, b.MyId, true);

        AddCrystalToItsRegion(entry1);
        AddCrystalToItsRegion(entry2);
    }


    /// <summary>
    /// Adding a polygon type of object to crystalss used for Buildings and StillElement
    /// </summary>
    /// <param name="anchors"></param>
    /// <param name="parentId"></param>
    void AddPoly(List<Vector3> anchors, string parentId)
    {
        var lines = U2D.FromPolyToLines(anchors);
        var scale = PassAnchorsGetPositionForCrystals(anchors);

        //UVisHelp.CreateHelpers(scale, Root.yellowCube);
        for (int i = 0; i < lines.Count; i++)
        {
            CreateAndAddPolyCrystal(scale[i], lines[i], parentId, i);
        }

        SetSiblings();
    }

    private float polyScale = 0.04f;
    //pushing them way from building center
    List<Vector3> PassAnchorsGetPositionForCrystals(List<Vector3> anchors)
    {
        //pushing them way from building center
        return UPoly.ScalePoly(anchors, polyScale);//0.04
    }


    /// <summary>
    /// This is to create and add to its respective Region a Crystal tht is part of a building or a
    /// still element 
    /// </summary>
    void CreateAndAddPolyCrystal(Vector3 pos, Line line, string parentID, int i, bool isDoor = false)
    {
        var crystalType = CrystaType(isDoor);

        Crystal c = new Crystal(pos, crystalType, parentID, isDoor);

        c.AnchorIndex = i;

        if (line != null)
        {
            c.Lines.Add(line);  
        }

        AddCrystalToItsRegion(c);
        _siblings.Add(c);

        

        //UVisHelp.CreateHelpers(U2D.FromV2ToV3(c.Position), Root.blueCube);
        //if (isDoor)
        //{
        //    UVisHelp.CreateHelpers(U2D.FromV2ToV3(c.Position), Root.blueSphereHelp);
        //}
    }

    H CrystaType(bool isDoor)
    {
        if (isDoor)
        {
            return H.Door;
        }
        return H.Obstacle;
    }



#region REGIONS

    //how many tiles on x and z the whole terrain is divided by 
    private int tiles = 12;

    private void InitRegions()
    {
        var len = m.IniTerr.Lenght;
        var hei = m.IniTerr.Height;

        var xStep = Mathf.Abs(len / tiles);
        var zStep = Mathf.Abs(hei / tiles);

        var iniPoint = m.MeshController.wholeMalla[0];
        var endPoint = m.MeshController.wholeMalla[2];

        //Debug.Log("iniPoint:" + iniPoint);
        //Debug.Log("endPoint:" + endPoint);

        LoopCreateRegions(xStep, zStep, iniPoint, endPoint);
    }

    private int index;
    /// <summary>
    /// Loop thru all terrain creating the regions 
    /// </summary>
    /// <param name="xStp"></param>
    /// <param name="zStp"></param>
    /// <param name="iniP"></param>
    /// <param name="endP"></param>
    void LoopCreateRegions(float xStp, float zStp, Vector3 iniP, Vector3 endP)
    {
        for (float x = iniP.x; x < endP.x; x+= xStp)
        {
            for (float z = iniP.z; z > endP.z; z-= zStp)
            {
                //UVisHelp.CreateHelpers(new Vector3(x, m.IniTerr.MathCenter.y, z), Root.blueCubeBig);
                CreateRegion(index, x, z, xStp, zStp);
                index++;
            }
        }
    }

    void CreateRegion(int index, float iniX, float iniZ, float len, float hei)
    {
        Rect rect = new Rect();
        rect.xMin = iniX;
        rect.yMin = iniZ;
        rect.width = len;
        rect.height = hei;
        rect = U2D.ReturnRectYInverted(rect);

        _crystalRegions.Add(new CrystalRegion(rect, index));
    }

    /// <summary>
    /// Will return in which region the 'pos' is 
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int ReturnMyRegion(Vector2 pos)
    {
        for (int i = 0; i < _crystalRegions.Count; i++)
        {
            if (_crystalRegions[i].Region.Contains(pos))
            {
                return i;
            }
        }
        return -1;
    }

    public bool DoesMyRegionHasTerraCrystal(Vector3 pos)
    {
        pos = U2D.FromV3ToV2(pos);
        var regionIndex = ReturnMyRegion(pos);

        //tht the pos didnt fall inside any region
        if (regionIndex == -1)
        {
            //will act as if will have terraCrystals on it 
            return true;
        }
        return _crystalRegions[regionIndex].ItHasATerraCristal();
    }

    public bool DoesMyRegionHasWaterCrystal(Vector3 pos)
    {
        pos = U2D.FromV3ToV2(pos);
        var regionIndex = ReturnMyRegion(pos);





        return false;
    }

    /// <summary>
    /// Given the current index will look for the other 8 blocks tht surrounding him 
    /// 
    /// muchTiles is like 3x3, or 5x5 etc, number must be impar
    /// </summary>
    /// <param name="curr"></param>
    /// <returns></returns>
    List<int> ReturnCurrentSurroundIndexRegions(int curr, int muchTiles)
    {
        List<int> re = new List<int>(){curr};
        List<int> tem = new List<int>() { curr };

        if (muchTiles % 2 == 0)
        {
            return null;
        }

        //wil;l get middle row
        re.AddRange(FindMiddleOneAndRetSurr(muchTiles, 1, curr));

        //will loop to milde row and will get all of the numbers up and down in tiles
        for (int i = 0; i < re.Count; i++)
        {
            tem.AddRange(FindMiddleOneAndRetSurr(muchTiles, tiles, re[i]));
        }

        re.AddRange(tem);
        re = re.Distinct().ToList();

        for (int i = 0; i < re.Count; i++)
        {
            if (!IsAValidIndex(re[i]))
            {
                re.RemoveAt(i);
                i--;
            }
        }
        return re;
    }

    /// <summary>
    /// Will make index 0, and will add index, and remove indexes to create a midle row.
    /// 
    /// For ex if 5 is sent. will ret: -2,-1,0,1,2
    /// </summary>
    /// <returns></returns>
    List<int> FindMiddleOneAndRetSurr(int much, int addition, int curr)
    {
        List<int> re = new List<int>();

        var variance = much;
        variance -= 1;//remove the middle one amt
        variance /= 2;//make it the half

        for (int i = -variance; i < variance + 1; i += 1)
        {
            //so it doesnt add same number again
            if (i != 0)
            {
                re.Add(curr + (i * addition));
            }
        }
        return re;
    }

    /// <summary>
    /// Will tell u if the pass index is valid on _crystalRegions 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    bool IsAValidIndex(int index)
    {
        if (index < _crystalRegions.Count && index > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Will return the index of surroindg regions
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="muchTiles">how many tiles . for ex the default is 3x3</param>
    /// <returns></returns>
    public List<int> ReturnMySurroundRegions(Vector2 pos, int muchTiles = 3)
    {
        var curr = ReturnMyRegion(pos);

        return ReturnCurrentSurroundIndexRegions(curr, muchTiles);
    }

    /// <summary>
    /// Will tell if rect contain any line point, A or B, if does. then is colliding  
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public bool DoIContainAnyCrystal(Rect rect, Vector2 pos)
    {
        var list = ReturnMySurroundRegions(pos);
        var crystals = GiveAllTerraCrystalsInTheseRegions(list);

        for (int i = 0; i < crystals.Count; i++)
        {
            for (int j = 0; j < crystals[i].Lines.Count; j++)
            {
                var line = crystals[i].Lines[j];
                if (rect.Contains( line.A1 ) || rect.Contains( line.B1 ))
                {
                    return true;
                }
            }
        }
        return false;
    }



    /// <summary>
    /// Will tell if rect intersect any line on the Terra Crystals.   
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public bool DoIIntersectAnyLine(Rect rect, Vector2 pos)
    {
        var list = ReturnMySurroundRegions(pos);
        var crystals = GiveAllTerraCrystalsInTheseRegions(list);

        var rectlines = U2D.FromRectToLines(rect);

        for (int i = 0; i < crystals.Count; i++)
        {
            for (int j = 0; j < crystals[i].Lines.Count; j++)
            {
                var line = crystals[i].Lines[j];

                for (int k = 0; k < rectlines.Count; k++)
                {
                    if (rectlines[k].IsIntersecting(line))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    /// <summary>
    /// Will tell if line intersect any line on the Terra Lines  
    /// 
    /// typeOfTerra: the type of terraObstacle we are considering 
    /// </summary>
    /// <returns></returns>
    public bool DoIIntersectAnyLine(Line line, Vector2 pos, H typeOfTerra)
    {
        var crystals = GiveMeAllTerraCrystals();

        for (int i = 0; i < crystals.Count; i++)
        {
            for (int j = 0; j < crystals[i].Lines.Count; j++)
            {
                var terraLine = crystals[i].Lines[j];
                if (line.IsIntersecting((terraLine)) && crystals[i].Type1 == typeOfTerra)
                {
                    return true;
                }
            }
        }
        return false;
    }



    /// <summary>
    /// Will tell u if intersect anyline at all 
    /// 
    /// will look only at the regions tht were pased in the 'histoRegions'
    /// 
    /// Will evaluate Obstacle and TerraCrsyrstals
    /// </summary>
    /// <param name="line"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool DoIIntersectAnyLine(Line line, List<int> histoRegions, CryRoute cryRoute)
    {
        var crystals = GiveAllCrystalsInTheseRegionsExcludLinkRects(histoRegions);

        for (int i = 0; i < crystals.Count; i++)
        {
            for (int j = 0; j < crystals[i].Lines.Count; j++)
            {
                var lineOnCrys = crystals[i].Lines[j];
                if (line.IsIntersecting((lineOnCrys)))
                {


                    lineOnCrys.DebugRender(Color.red);
                    return true;
                }
            }
        }
        return false;
    }


    public int CountLinesIIntersect(Line line, List<int> histoRegions, CryRoute cryRoute)
    {
        int res = 0;
        var crystals = GiveAllCrystalsInTheseRegionsExcludLinkRects(histoRegions);

        for (int i = 0; i < crystals.Count; i++)
        {
            for (int j = 0; j < crystals[i].Lines.Count; j++)
            {
                var lineOnCrys = crystals[i].Lines[j];
                if (line.IsIntersecting((lineOnCrys)))
                {
//                   //Debug.Log("Intersected: " + crystals[i].ParentId + " tp: " +crystals[i].Type1);
                    Vector3 intersection = U2D.FromV2ToV3(line.FindIntersection(lineOnCrys));
                    //add key to explorer on the CryRoute
                    cryRoute.AddKeyToExplorer(crystals[i], intersection);  

                    lineOnCrys.DebugRender(Color.black);
                    res++;
                }
            }
        }
        return res;
    }


    public List<Line> LinesIIntersect(Line line, List<int> histoRegions)
    {
        List<Line> res = new List<Line>();
        var crystals = GiveAllCrystalsInTheseRegionsExcludLinkRects(histoRegions);

        for (int i = 0; i < crystals.Count; i++)
        {
            for (int j = 0; j < crystals[i].Lines.Count; j++)
            {
                var lineOnCrys = crystals[i].Lines[j];
                if (line.IsIntersecting((lineOnCrys)))
                {
                    lineOnCrys.DebugRender(Color.blue);
                    res.Add(lineOnCrys);
                }
            }
        }
        return res;
    }


    /// <summary>
    /// Will return all crsytalls the Rect Lines intersect
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="histoRegions"></param>
    /// <returns></returns>
    public List<Crystal> ReturnCrystalsRectIntersect(Rect rect, List<int> histoRegions)
    {
        List<Crystal> res = new List<Crystal>();

        var crystals = GiveAllCrystalsInTheseRegionsExcludLinkRects(histoRegions);
        var rectlines = U2D.FromRectToLines(rect);

        for (int i = 0; i < crystals.Count; i++)
        {
            for (int j = 0; j < crystals[i].Lines.Count; j++)
            {
                var lineOnCrys = crystals[i].Lines[j];

                for (int k = 0; k < rectlines.Count; k++)
                {
                    var line = rectlines[k];

                    if (line.IsIntersecting((lineOnCrys)))
                    {
                        lineOnCrys.DebugRender(Color.grey, 10);
                        res.Add(crystals[i]);
                    }
                }
            }
        }
        return res;
    }





    private List<Crystal> GiveMeAllTerraCrystals()
    {
        //so we only create a list with all once then retun _all for the rest of the times 
        if (_all.Count > 0)
        {
            return _all;
        }


        List<Crystal> res = new List<Crystal>();
        for (int i = 0; i < CrystalRegions.Count; i++)
        {
            res.AddRange(CrystalRegions[i].TerraCrystals);
        }

        _all = res;

        return res;

    }

    List<Crystal> GiveAllTerraCrystalsInTheseRegions(List<int> regions)
    {
        List<Crystal>res = new List<Crystal>();
        for (int i = 0; i < regions.Count; i++)
        {
            var index = regions[i];
            res.AddRange(CrystalRegions[index].TerraCrystals);
        }
        return res;
    }


    public List<Crystal> GiveAllTerraCrystalsInTheseRegionsPlsObsta(List<int> regions)
    {
        List<Crystal> res = new List<Crystal>();
        for (int i = 0; i < regions.Count; i++)
        {
            var locIndex = regions[i];
            res.AddRange(CrystalRegions[locIndex].TerraCrystals);
            res.AddRange(CrystalRegions[locIndex].ObstaCrystals);
        }
        return res;
    }



    /// <summary>
    /// Including the Terra Crystals and Building Crystals bz I need to check on all of them
    /// 
    /// </summary>
    /// <param name="regions"></param>
    /// <returns></returns>
    public List<Crystal> GiveAllCrystalsInTheseRegionsExcludLinkRects(List<int> regions)
    {
        List<Crystal> res = new List<Crystal>();
        for (int i = 0; i < regions.Count; i++)
        {
            var index = regions[i];

            res.AddRange(CrystalRegions[index].ObstaCrystals);

            //wont add LinkRects bz they have line all over the plcace 
            for (int j = 0; j < CrystalRegions[index].TerraCrystals.Count; j++)
            {
                var cry = CrystalRegions[index].TerraCrystals[j];
                if (cry.Type1 != H.LinkRect)
                {
                    res.Add(cry);
                }
            }
        }
        return res;
    }

    internal List<Crystal> GiveAllCrystalsInTheseRegionsExcludLinkRects(int regionIndx)
    {
        List<Crystal> res = new List<Crystal>();

        var index = regionIndx;

        res.AddRange(CrystalRegions[index].ObstaCrystals);

        //wont add LinkRects bz they have line all over the plcace 
        for (int j = 0; j < CrystalRegions[index].TerraCrystals.Count; j++)
        {
            var cry = CrystalRegions[index].TerraCrystals[j];
            if (cry.Type1 != H.LinkRect)
            {
                res.Add(cry);
            }
        }

        return res;
    }
    
    public List<Crystal> GiveAllCrystalsInTheseRegionsExcludLinkRects(List<int> regions, ref List<Crystal> obsta)
    {
        List<Crystal> res = new List<Crystal>();
        for (int i = 0; i < regions.Count; i++)
        {
            var index = regions[i];

            obsta.AddRange(CrystalRegions[index].ObstaCrystals);

            //wont add LinkRects bz they have line all over the plcace 
            for (int j = 0; j < CrystalRegions[index].TerraCrystals.Count; j++)
            {
                var cry = CrystalRegions[index].TerraCrystals[j];
                if (cry.Type1 != H.LinkRect)
                {
                    res.Add(cry);
                }
            }
        }
        return res;
    }






#endregion



    private int counter;
    public void Update()
    {
        if (linkNow)
        {


            LinkCrystalsNow();
        }
        if (load)
        {
            Load();
        }
    }



#region Marine Terra Obstacles

    /// <summary>
    /// The crystals need to be added and then Link
    /// </summary>
    List<Crystal> _bounds = new List<Crystal>();
    private bool linkNow; 

    /// <summary>
    /// To add the Marine bounds
    /// </summary>
    internal void AddCrystal(Crystal crystal)
    {
        if (crystal == null)
        {
            return;
        }

        _bounds.Add(crystal);
    }

    /// <summary>
    /// Will organize Crystals and will make linkNow = true;
    /// </summary>
    public void LinkCrystals()
    {
        OrganizeByDist();

        linkNow = true;
    }

    void CleanBoundsList()
    {
        _bounds.Clear();
    }

    public void StopLinking(H type)
    {
        linkNow = false;
        Crystal.ResetAccumNumbers();
        CleanBoundsList();

        //if the one stped was water now Terrain needs to be initiated 
        if (type == H.WaterObstacle)
        {
            m.MeshController.WaterBound1.FindVertexAboveTerrainLevel();
        }
        else if (type == H.MountainObstacle)
        {
            //Creates the land zones
            m.MeshController.LandZoneManager1.Create();

            Save();
        }
        else if (type==H.LinkRect)
        {
            m.MeshController.LandZoneManager1.FirstLinkRectsLinkDone(type);
        }
        else if (type == H.LandZone)
        {
            m.MeshController.LandZoneManager1.FirstLinkRectsLinkDone(type);
        }
        else if (type == H.Poll)
        {
            
        }
    }

    /// <summary>
    /// Will define to which land zone each regions belongs to
    /// </summary>
    public void DefineRegionLandZone()
    {
        for (int i = 0; i < _crystalRegions.Count; i++)
        {
            _crystalRegions[i].DefineWhichLandZoneIBelongTo();
        }

        Save();
    }

    /// <summary>
    /// Remove crystal from _bounds
    /// </summary>
    /// <param name="crystal"></param>
    /// <param name="type"></param>
    public void RemoveCrystal(Crystal crystal, H type)
    {
        //Called here bz it has its final position and eveyrhint elese
        AddCrystalToItsRegion(_bounds[0]);

        _bounds.Remove(crystal);

        if (_bounds.Count == 0)
        {
            StopLinking(type);
           //Debug.Log("stopped on manager:" + _bounds.Count);
        }

       //Debug.Log("count:" + _bounds.Count);
    }


    /// <summary>
    /// Will Link the crystals on _bounds, one by one
    /// </summary>
    void LinkCrystalsNow()
    {
        if (_bounds.Count > 0)
        {
            if (_bounds[0].Type1 == H.LinkRect || _bounds[0].Type1 == H.LandZone)
            {
                RamdonLinking();
            }
            else RegularLinking();
        }
    }

    void RegularLinking()
    {
        _bounds[0].Link(_bounds);        
    }

    //which count at the same time is making the function of index for 'checks'
    private int count;
    List<int> checks=new List<int>();
    private int voltas ;
    /// <summary>
    /// For this linkin I cant remove the used Crystals they all must stay so they get named
    /// correctly
    /// </summary>
    void RamdonLinking()
    {
        if (!checks.Contains(count))
        {
            if (checks.Count < _bounds.Count)
            {
                checks.Add(count);
                _bounds[count].Link(_bounds, count);
                //AddCrystalToItsRegion(_bounds[count]);
            }
        }
        else if (checks.Count >= _bounds.Count)
        {
            voltas++;
            
            count = 0;
            checks.Clear();

            Voltas();
        }
        else count = Random.Range(0, _bounds.Count);
    }

    void Voltas()
    {
        var topVoltas = DefineTopVoltas();
       //Debug.Log("Voltas:"+voltas);

        if (voltas == topVoltas)
        {
            StopLinking(_bounds[0].Type1);
            voltas = 0;
        }
        else
        {
            CleanAllLines();
        }
    }

    int DefineTopVoltas()
    {
        if (_bounds[0].Type1 == H.LandZone)
        {
            return 7;
        }
        return 1;
    }

    /// <summary>
    /// So they can relink again
    /// 
    /// This is done so they get name it the same 
    /// </summary>
    void CleanAllLines()
    {
        for (int i = 0; i < _bounds.Count; i++)
        {
            _bounds[i].Clean();
        }
    }

    /// <summary>
    /// Will add the crystal into its correspoding region 
    /// </summary>
    public void AddCrystalToItsRegion(Crystal c)
    {
        var myRegionIndex = ReturnMyRegion(c.Position);
        if (myRegionIndex == -1)
        {
            return;
        }

        _crystalRegions[myRegionIndex].AddCrystal(c);


        AddToAllObstas(c);
    }

    void AddToAllObstas(Crystal c)
    {
        if (c.Type1 == H.Obstacle || c.Type1.ToString().Contains("Way"))
        {
            _allObstas.Add(c);
        }
    }

    private void OrganizeByDist()
    {
        var firstCry = _bounds[0];

        for (int i = 0; i < _bounds.Count; i++)
        {
            _bounds[i].CalculateDistance(firstCry.Position);
        }

        _bounds = _bounds.OrderBy(a => a.Distance).ToList();
    }

#endregion


#region SaveLoad

    void Save()
    {
        m.SubMesh.CrystalManager1 = this;
        m.MeshController.WriteXML();
    }

    void Load()
    {
        if (m.SubMesh == null)
        {
            return;
        }

        load = false;
        CrystalRegions = m.SubMesh.CrystalManager1.CrystalRegions;

        if (CrystalRegions.Count == 0)
        {
            InitRegions();
        }
        else
        {
            isFullyLoaded = true;

            for (int i = 0; i < CrystalRegions.Count; i++)
            {
                //CrystalRegions[i].DebugHere();
            }
        }
    }


#endregion

#region Find Landing Zone

    private string pollResult;

    /// <summary>
    /// Will return the landing zone of the 'position'
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public string ReturnLandingZone(Vector3 position)
    {
        var regs = ReturnMySurroundRegions(U2D.FromV3ToV2(position));

        var regionsLandZoneId = ReturnRegionsLandZones(regs);
        if (!string.IsNullOrEmpty(regionsLandZoneId))
        {
            return regionsLandZoneId;
        }

        InitCrystalPoll(position);

        return pollResult;
    }

    /// <summary>
    /// Will init the process to gathering all crystals in surrouding areas and from this position
    /// will try to link to 3 rectlinks in the surrouding areas
    /// </summary>
    /// <param name="position"></param>
    private void InitCrystalPoll(Vector3 position)
    {
        var crysSurroInt = ReturnMySurroundRegions(U2D.FromV3ToV2(position));
        var crystals = GiveAllTerraCrystalsInTheseRegions(crysSurroInt);
        crystals = RefineCrystalsForType(crystals, H.LinkRect);

        Crystal cryPoll = new Crystal(position, H.Poll, "");

        _bounds.AddRange(crystals);
        _bounds.Insert(0, cryPoll);

        RegularLinking();
    }

    /// <summary>
    /// Will leave only the crystal of 'type' on the result list 
    /// </summary>
    /// <param name="crystals"></param>
    /// <param name="h"></param>
    /// <returns></returns>
    private List<Crystal> RefineCrystalsForType(List<Crystal> crystals, H type)
    {
        List<Crystal> res = new List<Crystal>();
        
        for (int i = 0; i < crystals.Count; i++)
        {
            if (crystals[i].Type1 == type)
            {
                res.Add(crystals[i]);
            }
        }

        return res;
    }

    string ReturnRegionsLandZones(List<int> regions)
    {
        List<string> lis = new List<string>();

        for (int i = 0; i < regions.Count; i++)
        {
            var index = regions[i];
            var cryRegion = CrystalRegions[index];

            if (!lis.Contains(cryRegion.LandZoneId) && !string.IsNullOrEmpty(cryRegion.LandZoneId))
            {
                lis.Add(cryRegion.LandZoneId);
            }
        }

        //if we only have one around then 
        if (lis.Count == 1)
        {
            return lis[0];
        }
        return "";
    }

    /// <summary>
    /// When the crystall poll ended will call this method
    /// </summary>
    public void RemoveCrystalPoll()
    {
        ExtractPollInfo();
        //SetBuildingLandZone();

        StopLinking(H.Poll);
    }
    
    /// <summary>
    /// Will find out wht the Crystall Poll Found in its linking 
    /// </summary>
    private void ExtractPollInfo()
    {
        var res = ReturnCrystalsLinksLandZones(_bounds[0]);

        var mostCommom = UString.ReturnMostCommonName(res);
        pollResult = mostCommom;
    }

    List<string> ReturnCrystalsLinksLandZones(Crystal c)
    {
        List<string> lis = new List<string>();
        var links = c.Links();


        for (int i = 0; i < links.Count; i++)
        {
            if (!lis.Contains(links[i].Name) && !string.IsNullOrEmpty(links[i].Name))
            {
                lis.Add(links[i].Name);
            }
        }

        return lis;
    }


#endregion


    private bool isFullyLoaded;
    internal bool IsFullyLoaded()
    {
        return isFullyLoaded;
    }

    /// <summary>
    /// Will tell if those 'poly' intersect any line btw them and the 'iniPos'
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public bool IntersectAnyLine(List<Vector3> points, Vector3 iniPos)
    {
        //get the indexes of regions the poly is 
        var indexes = ReturnPolySurroundingRegions(points);
        //lines formed from iniPos to each points elements
        var lines = ReturnAllLines(points, iniPos);

        for (int i = 0; i < lines.Count; i++)
        {
            //means one the lines is intesecting a line 
            if (DoIIntersectAnyLine(lines[i], indexes, new CryRoute()))
            {
                return true;
            }
        }
        return false;
    } 
    
    ///<summary>
    /// 
    /// Final is the final position
    /// </summary>
    public bool IntersectAnyLine(Vector3 final, Vector3 iniPos)
    {
        List<Vector3> points = new List<Vector3>() { final, iniPos };

        //get the indexes of regions the poly is 
        var indexes = ReturnPolySurroundingRegions(points);
        //lines formed from iniPos to each points elements
        Line line = new Line(final, iniPos, 20f);

        if (DoIIntersectAnyLine(line, indexes, new CryRoute()))
        {
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// Will return all lines formed from 'ini' to each point of poly
    /// </summary>
    /// <param name="points"></param>
    /// <param name="iniPos"></param>
    /// <returns></returns>
    List<Line> ReturnAllLines(List<Vector3> points, Vector3 iniPos)
    {
        List<Line> res = new List<Line>();

        for (int i = 0; i < points.Count; i++)
        {
            res.Add(new Line(points[i], iniPos, 10f));
        }

        return res;
    }

        /// <summary>
    /// Return the regions surrouding each point of the poly 
    /// </summary>
    /// <param name="poly"></param>
    /// <returns></returns>
    List<int> ReturnPolySurroundingRegions(List<Vector3> poly)
    {
        List<int> res = new List<int>();

        for (int i = 0; i < poly.Count; i++)
        {
            var currRegion = ReturnMyRegion(U2D.FromV3ToV2(poly[i]));

            res.AddRange(ReturnCurrentSurroundIndexRegions(currRegion, 3));    
        }

        return res.Distinct().ToList();
    }
}


