﻿/*
* IMPORTANT: IF LANDZONES ARE NOT SET THE ROUTING SYSTEM WONT WORK
*
*
*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrystalManager
{
    private SMe m = new SMe();

    //all the regions
    private List<CrystalRegion> _crystalRegions = new List<CrystalRegion>();

    private bool load;

    //all the crystals on the regions . for GC purpose
    private List<Crystal> _all = new List<Crystal>();

    //only use for finding them
    private List<Crystal> _allObstas = new List<Crystal>();

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

        RemoveFromAllObstas(myIdP);
    }

    /// <summary>
    /// Other wise the crystal stay in _allobsta
    /// </summary>
    /// <param name="myIdP"></param>
    private void RemoveFromAllObstas(string myIdP)
    {
        var crystals = _allObstas.Where(a => a.ParentId == myIdP).ToList();

        for (int i = 0; i < crystals.Count; i++)
        {
            _allObstas.Remove(crystals[i]);
            //Debug.Log("Crystal removed: " + myIdP + ". from _allObsta");
        }
    }

    /// <summary>
    /// Is the Spawner contained in the Rect
    /// </summary>
    /// <param name="spawnerKey"></param>
    /// <param name="buildRect"></param>
    /// <returns></returns>
    public bool AreTheyContained(string spawnerKey, Rect buildRect)
    {
        var crystals = _allObstas.Where(a => a.ParentId == spawnerKey).ToList();

        for (int i = 0; i < crystals.Count; i++)
        {
            if (buildRect.Contains(crystals[i].Position))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Will return the region index where all the StillElement points are
    /// </summary>
    /// <param name="still"></param>
    /// <returns></returns>
    private List<int> ReturnRegionsOfPointsInStructure(StillElement still)
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
        List<Crystal> res = new List<Crystal>();
        //search _crystalRegions points are
        var indexes = ReturnRegionsOfPointsInStructure(building);
        for (int i = 0; i < indexes.Count; i++)
        {
            var indexLoc = indexes[i];
            res.AddRange(CrystalRegions[indexLoc].ObstaCrystals());
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
        List<Crystal> res = new List<Crystal>();
        //search _crystalRegions points are
        var indexes = ReturnRegionsOfPointsInStillElement(still);
        for (int i = 0; i < indexes.Count; i++)
        {
            var indexLoc = indexes[i];
            res.AddRange(CrystalRegions[indexLoc].ObstaCrystals());
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
    private H WhichType(bool includeDoor)
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
    private List<int> ReturnRegionsOfPointsInStructure(Building building)
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
            var obstas = _allObstas.Where(a => a.ParentId == building.MyId).ToList();

            for (int i = 0; i < obstas.Count; i++)
            {
                points.Add(U2D.FromV2ToV3(obstas[i].Position));
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
    private List<int> ReturnRegionsOfPointsInStillElement(StillElement still)
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
    private List<Crystal> GetBridgeEntries(Bridge b)
    {
        List<Crystal> re = new List<Crystal>();

        re.Add(new Crystal(b.LandZone1[0].Position, H.Obstacle, b.MyId, true));
        re.Add(new Crystal(b.LandZone1[1].Position, H.Obstacle, b.MyId, true));

        return re;
    }

    //private string _info;//info that will be added to the crystal
    //the siblings of current crustal
    private List<Crystal> _siblings = new List<Crystal>();

    public void Add(Building building)
    {
        _siblings.Clear();

        if (building.MyId.Contains("Bridge"))
        {
            AddBridge((Bridge)building);
        }
        else AddBuilding(building);
    }

    public void Add(StillElement still)
    {
        //_info = "Still";
        _siblings.Clear();

        if (still.Anchors.Count > 4)
        {
            Debug.Log("acnhors:" + still.Anchors.Count + " " + still.MyId);
        }

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
    private void AddBuilding(Building building)
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

    private void AddBridge(Bridge b)
    {
        Vector3[] copy = b.GetBridgeAnchorsCheckIfSave();

        AddPoly(copy.ToList(), b.MyId);
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
    private void AddPoly(List<Vector3> anchors, string parentId, bool debug = false)
    {
        var lines = U2D.FromPolyToLines(anchors);
        var scale = PassAnchorsGetPositionForCrystals(anchors);

        if (debug)
        {
            Debug.Log("crystal added by:" + parentId);
            UVisHelp.CreateHelpers(scale, Root.yellowCube);
        }

        for (int i = 0; i < lines.Count; i++)
        {
            CreateAndAddPolyCrystal(scale[i], lines[i], parentId, i);
        }

        SetSiblings();
    }

    private float polyScale = 0.04f;

    //pushing them way from building center
    private List<Vector3> PassAnchorsGetPositionForCrystals(List<Vector3> anchors)
    {
        //pushing them way from building center
        return UPoly.ScalePoly(anchors, polyScale);//0.04
    }

    /// <summary>
    /// This is to create and add to its respective Region a Crystal tht is part of a building or a
    /// still element
    /// </summary>
    private void CreateAndAddPolyCrystal(Vector3 pos, Line line, string parentID, int i, bool isDoor = false)
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

    private H CrystaType(bool isDoor)
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
    private void LoopCreateRegions(float xStp, float zStp, Vector3 iniP, Vector3 endP)
    {
        for (float x = iniP.x; x < endP.x; x += xStp)
        {
            for (float z = iniP.z; z > endP.z; z -= zStp)
            {
                //UVisHelp.CreateHelpers(new Vector3(x, m.IniTerr.MathCenter.y, z), Root.blueCubeBig);
                CreateRegion(index, x, z, xStp, zStp);
                index++;
            }
        }
    }

    private void CreateRegion(int index, float iniX, float iniZ, float len, float hei)
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

    public int ReturnMyRegion(Vector3 v3)
    {
        return ReturnMyRegion(U2D.FromV3ToV2(v3));
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

    //Dictionary<string, List<int>> SurroundIndRegGC = new Dictionary<string, List<int>>();
    /// <summary>
    /// Given the current index will look for the other 8 blocks tht surrounding him
    ///
    /// muchTiles is like 3x3, or 5x5 etc, number must be impar
    /// </summary>
    /// <param name="curr"></param>
    /// <returns></returns>
    private List<int> ReturnCurrentSurroundIndexRegions(int curr, int muchTiles)
    {
        //if (SurroundIndRegGC.ContainsKey(curr + "." + muchTiles))
        //{
        //    return SurroundIndRegGC[curr + "." + muchTiles];
        //}

        List<int> re = new List<int>() { curr };
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
        //SurroundIndRegGC.Add(curr+"."+muchTiles, re);
        return re;
    }

    /// <summary>
    /// Will make index 0, and will add index, and remove indexes to create a midle row.
    ///
    /// For ex if 5 is sent. will ret: -2,-1,0,1,2
    /// </summary>
    /// <returns></returns>
    private List<int> FindMiddleOneAndRetSurr(int much, int addition, int curr)
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
    private bool IsAValidIndex(int index)
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

    public List<int> ReturnSurroundingsOfInitTownRegions(int muchTiles = 3)
    {
        var curr = ReturnMyRegion(ReturnTownIniPos());

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
                if (rect.Contains(line.A1) || rect.Contains(line.B1))
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
    public bool DoIIntersectAnyLine(Line line, H typeOfTerra)
    {
        var crystals = GiveMeAllTerraCrystalsInTheLine(line);

        //Debug.Log("crystals counts:"+crystals.Count);

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

    #region CPU too high

    /// <summary>
    /// Will return all crsytals in the regions around that surround the line
    /// </summary>
    /// <returns></returns>
    private List<Crystal> GiveMeAllTerraCrystalsInTheLine(Line line)
    {
        var regions = ReturnRegionsOfALine(line);
        return GiveAllCrystalsInTheseRegionsExcludLinkRects(regions);
    }

    /// <summary>
    /// given a line will find out which are the regions this line is draw into
    ///
    /// Will go trhu steps and will find the 3x3 round it, if is doubled wont be added
    /// to final result
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private List<int> ReturnRegionsOfALine(Line line)
    {
        List<int> re = new List<int>();

        //the step is the size doiagonal of a Region
        var step = (CrystalRegions[0].Region.height + CrystalRegions[0].Region.width) / 2;
        var pointsInTheLine = ReturnPointsAcrossAline(line, step);

        for (int i = 0; i < pointsInTheLine.Count; i++)
        {
            var region = ReturnMyRegion(pointsInTheLine[i]);

            re.AddRange(ReturnCurrentSurroundIndexRegions(region, 3));
        }
        return re.Distinct().ToList();
    }

    /// <summary>
    /// Will find the point across a line until we reach final point will return all points at the end
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private List<Vector2> ReturnPointsAcrossAline(Line line, float step = 10f)
    {
        List<Vector2> res = new List<Vector2>() { };
        var dist = Vector2.Distance(line.A1, line.B1);
        var howManySteps = int.Parse((dist / step).ToString("n0"));

        for (int i = 0; i < howManySteps + 1; i++)
        {
            var v2 = Vector2.MoveTowards(line.A1, line.B1, step * i);
            res.Add(v2);
        }

        res.Add(line.B1);
        return res;
    }

    #endregion CPU too high

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
                    //UVisHelp.CreateText(U2D.FromV2ToV3(lineOnCrys.A1), crystals[i].ParentId);

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

    //Dictionary<List<int> ,List<Crystal> > terraCrystalRegGC = new Dictionary<List<int>, List<Crystal>>();
    private List<Crystal> GiveAllTerraCrystalsInTheseRegions(List<int> regions)
    {
        //if (terraCrystalRegGC.ContainsKey(regions))
        //{
        //    return terraCrystalRegGC[regions];
        //}

        List<Crystal> res = new List<Crystal>();
        for (int i = 0; i < regions.Count; i++)
        {
            var index = regions[i];
            res.AddRange(CrystalRegions[index].TerraCrystals);
        }
        //terraCrystalRegGC.Add(regions,res);
        return res;
    }

    //todo GC
    public List<Crystal> GiveAllTerraCrystalsInTheseRegionsPlsObsta(List<int> regions)
    {
        List<Crystal> res = new List<Crystal>();
        for (int i = 0; i < regions.Count; i++)
        {
            var locIndex = regions[i];
            res.AddRange(CrystalRegions[locIndex].TerraCrystals);
            res.AddRange(CrystalRegions[locIndex].ObstaCrystals());
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

            res.AddRange(CrystalRegions[index].ObstaCrystals());

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

    ////todo GC
    //internal List<Crystal> GiveAllCrystalsInTheseRegionsExcludLinkRects(int regionIndx)
    //{
    //    List<Crystal> res = new List<Crystal>();

    //    var index = regionIndx;

    //    res.AddRange(CrystalRegions[index].ObstaCrystals);

    //    //wont add LinkRects bz they have line all over the plcace
    //    for (int j = 0; j < CrystalRegions[index].TerraCrystals.Count; j++)
    //    {
    //        var cry = CrystalRegions[index].TerraCrystals[j];
    //        if (cry.Type1 != H.LinkRect)
    //        {
    //            res.Add(cry);
    //        }
    //    }

    //    return res;
    //}

    #endregion REGIONS

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
    private List<Crystal> _bounds = new List<Crystal>();

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

    private void CleanBoundsList()
    {
        _bounds.Clear();
    }

    public void StopLinking(H type)
    {
        Debug.Log("StopLinking t:" + type);
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

            //m.MeshController.WaterBound1.Create();
        }
        else if (type == H.LinkRect)
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
    private void LinkCrystalsNow()
    {
        for (int i = 0; i < 100; i++)
        {
            if (_bounds.Count > 0)
            {
                if (_bounds[0].Type1 == H.LinkRect || _bounds[0].Type1 == H.LandZone)
                {
                    RamdonLinking();
                }
                else RegularLinking();
            }
            else
            {
                i = 200;
            }
        }
    }

    private void RegularLinking()
    {
        _bounds[0].Link(_bounds);
    }

    //which count at the same time is making the function of index for 'checks'
    private int count;

    private List<int> checks = new List<int>();
    private int voltas;

    /// <summary>
    /// For this linkin I cant remove the used Crystals they all must stay so they get named
    /// correctly
    /// </summary>
    private void RamdonLinking()
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

    private void Voltas()
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

    private int DefineTopVoltas()
    {
        if (_bounds[0].Type1 == H.LandZone)
        {
            return 14;//7
        }
        return 1;//1
    }

    /// <summary>
    /// So they can relink again
    ///
    /// This is done so they get name it the same
    /// </summary>
    private void CleanAllLines()
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
        //Debug.Log("Added a crystal:"+c.ParentId);

        AddToAllObstas(c);
    }

    private void AddToAllObstas(Crystal c)
    {
        if (c.Type1 == H.Obstacle || c.Type1.ToString().Contains("Way"))
        {
            _allObstas.Add(c);
        }
    }

    private bool IsBrandNewTerrain()
    {
        return _bounds.Count == 0;
    }

    private void OrganizeByDist()
    {
        ////to stop this when called from LandZone
        //if (IsBrandNewTerrain())
        //{
        //    return;
        //}

        var firstCry = _bounds[0];

        for (int i = 0; i < _bounds.Count; i++)
        {
            _bounds[i].CalculateDistance(firstCry.Position);
        }

        _bounds = _bounds.OrderBy(a => a.Distance).ToList();
    }

    #endregion Marine Terra Obstacles

    #region SaveLoad

    private void Save()
    {
        m.SubMesh.CrystalManager1 = this;
        m.MeshController.WriteXML();
    }

    private void Load()
    {
        if (m.SubMesh == null)
        {
            return;
        }

        load = false;
        CrystalRegions = m.SubMesh.CrystalManager1.CrystalRegions;
        Program.gameScene.BatchManagerCreate();

        if (CrystalRegions.Count == 0)
        {
            InitRegions();
        }
        else
        {
            AfterLoaded();
            isFullyLoaded = true;
        }
    }

    private void AfterLoaded()
    {
        for (int i = 0; i < CrystalRegions.Count; i++)
        {
            CrystalRegions[i].StartWithAudioReport();
        }
        HandleLaterAudioRegions();
    }

    private void HandleLaterAudioRegions()
    {
        for (int i = 0; i < CrystalRegions.Count; i++)
        {
            var Index = i;
            if (MeshController.CrystalManager1.CrystalRegions[Index].WhatAudioIReport == "Later")
            {
                MeshController.CrystalManager1.CrystalRegions[Index].DoAroundAudioSurvey();
                //MeshController.CrystalManager1.CrystalRegions[Index].DebugHere();
            }

            //Debug.Log("Reg Audio Report: " + CrystalRegions[Index].WhatAudioIReport);
        }
    }

    private int count1;
    private List<CrystalRegion> _inLands;
    private int _initialRegionIndex;//only used for reporting purposees

    public int InitialRegionIndex
    {
        get { return _initialRegionIndex; }
        set { _initialRegionIndex = value; }
    }

    public Vector3 ReturnTownIniPos()
    {
        if (_inLands == null)
        {
            _inLands = CrystalRegions.Where(a => a.WhatAudioIReport == "InLand").ToList();
        }
        var indexA = UMath.GiveRandom(0, _inLands.Count);
        var point = _inLands[indexA].Position();
        var realIndex = _inLands[indexA].Index;

        //UVisHelp.CreateHelpers(point, Root.yellowCube);
        var inTerrain = UTerra.IsOnTerrainManipulateTerrainSize(point, -40f);//-1

        if (!inTerrain || !IsAdjacentToShore(realIndex))
        {
            if (count1 > 500)
            {
                throw new Exception("500 times ReturnTownIniPos() CrystalManager");
            }
            count1++;
            _inLands.RemoveAt(indexA);

            return ReturnTownIniPos();
        }
        Debug.Log("Count1: " + count1 + " ..ReturnTownIniPos() CrystalManager");
        count1 = 0;
        _initialRegionIndex = realIndex;
        return point;
    }

    /// <summary>
    /// Will say if the region pass is adjacent to a OceanShore region
    /// </summary>
    /// <param name="regionIndex"></param>
    /// <returns></returns>
    private bool IsAdjacentToShore(int regionIndex)
    {
        var adjacents = ReturnCurrentSurroundIndexRegions(regionIndex, 3);
        var shoresCount = 0;

        for (int i = 0; i < adjacents.Count; i++)
        {
            var curIndex = adjacents[i];
            if (CrystalRegions[curIndex].WhatAudioIReport == "OceanShore")
            {
                shoresCount++;

                //at least 2 OceanShores regiosn should be adjacent
                if (shoresCount > 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    #endregion SaveLoad

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

    //Dictionary<List<int>, string> regionsLandZoGC = new Dictionary<List<int>, string>();
    private string ReturnRegionsLandZones(List<int> regions)
    {
        //if (regionsLandZoGC.ContainsKey(regions))
        //{
        //    return regionsLandZoGC[regions];
        //}

        var res = "";
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
            res = lis[0];
        }

        //regionsLandZoGC.Add(regions,res);
        return res;
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

    private List<string> ReturnCrystalsLinksLandZones(Crystal c)
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

    #endregion Find Landing Zone

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
    private List<Line> ReturnAllLines(List<Vector3> points, Vector3 iniPos)
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
    private List<int> ReturnPolySurroundingRegions(List<Vector3> poly)
    {
        List<int> res = new List<int>();

        for (int i = 0; i < poly.Count; i++)
        {
            var currRegion = ReturnMyRegion(U2D.FromV3ToV2(poly[i]));

            res.AddRange(ReturnCurrentSurroundIndexRegions(currRegion, 3));
        }

        return res.Distinct().ToList();
    }

    public string WhatAudioIPlay(Vector3 camPos)
    {
        //todo make sure they start with out ForSaleRegionGO
        var v2 = U2D.FromV3ToV2(camPos);
        var index = ReturnMyRegion(v2);

        if (index == -1)
        {
            return "OutOfTerrain";
        }

        return CrystalRegions[index].WhatAudioIReport;
    }

    /// <summary>
    /// Center of the region the camera is on top of now
    /// </summary>
    /// <param name="camPos"></param>
    /// <returns></returns>
    internal Vector3 CurrentRegionPos(Vector3 camPos)
    {
        var v2 = U2D.FromV3ToV2(camPos);
        var index = ReturnMyRegion(v2);

        if (index == -1)
        {
            return camPos;
        }

        return U2D.FromV2ToV3(CrystalRegions[index].Region.center);
    }

    /// <summary>
    /// Will return the closest FullOcoean position to param
    ///
    ///
    /// </summary>
    /// <param name="iniPosP"></param>
    /// <returns></returns>
    internal Vector3 GiveMeTheClosestSeaRegionToMe(Vector3 iniPosP)
    {
        var fullOcean = _crystalRegions.Where(a => a.WhatAudioIReport == "FullOcean").ToList();

        for (int i = 0; i < fullOcean.Count; i++)
        {
            fullOcean[i].TempDistance = Vector3.Distance(fullOcean[i].Position(), iniPosP);
        }
        fullOcean = fullOcean.OrderBy(a => a.TempDistance).ToList();

        return fullOcean[0].Position();
    }
}