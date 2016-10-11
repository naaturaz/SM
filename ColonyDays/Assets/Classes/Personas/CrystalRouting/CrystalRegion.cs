using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The terrain is gonna be divided in varius regions and they will have the
/// Crytals 
/// </summary>
public class CrystalRegion
{
    private int _index;//the region index 

    //the region 
    private Rect _region;

    List<Crystal> _terraCrystals = new List<Crystal>(); 
    List<Crystal> _obstaCrystals = new List<Crystal>();

    private string _landZoneID;

    //will indicate if this region has a Water Crystal
    //null not set, Yes : it has it, No: doesnt have it 
    private string _waterCrystalInfo;
    private string _mountCrystalInfo;

    private string _whatAudioIReport;

    private General _debugText;

    /// <summary>
    /// This are the Terrain Crystals they get setup once and thats it. They
    /// dont need to be changed ever again. Contains the bounds of water and mountain and the LinkRects
    /// as well. Those conform the LandZones
    /// </summary>
    public List<Crystal> TerraCrystals
    {
        get { return _terraCrystals; }
        set { _terraCrystals = value; }
    }

    public int Index
    {
        get { return _index; }
        set { _index = value; }
    }

    public Rect Region
    {
        get { return _region; }
        set { _region = value; }
    }

    public string LandZoneId
    {
        get { return _landZoneID; }
        set { _landZoneID = value; }
    }

    public string WhatAudioIReport
    {
        get { return _whatAudioIReport; }
        set { _whatAudioIReport = value; }
    }

    //public List<Crystal> ObstaCrystals
    //{
    //    get { return _obstaCrystals; }
    //    set { _obstaCrystals = value; }
    //}

    /// <summary>
    /// OMG 1 day of work to find out that ObstaCrystals were being saved and loaded 
    /// 
    /// They were actually saved on the XML file 'Bay_And_Mountain_1_River'  >
    /// </summary>
    /// <returns></returns>
    public List<Crystal> ObstaCrystals()
    {
        return _obstaCrystals;
    }

    public CrystalRegion()
    {
    }

    public CrystalRegion(Rect region, int index)
    {
        _index = index;
        _region = region;

        WhatAudioIReport = DefineWhichAudioIReport();
    }

    public void StartWithAudioReport()
    {
        if (string.IsNullOrEmpty(WhatAudioIReport))
        {
            WhatAudioIReport = DefineWhichAudioIReport();
        }

        if (WhatAudioIReport == "Later")
        {
            return;
        }
        
        //DebugHere();
    }

    public void DebugHere()
    {
        UVisHelp.CreateDebugLines(Region, Color.cyan);
        UVisHelp.CreateText(U2D.FromV2ToV3(Region.center),
            Index + "|" + WhatAudioIReport, 300);
    }

    /// <summary>
    /// Will return true if has at least one terra crystal
    /// </summary>
    /// <returns></returns>
    public bool ItHasATerraCristal()
    {
        if (_mountCrystalInfo == null)
        {
            _mountCrystalInfo = DefineIfCrystal(H.MountainObstacle);
        }

        return _mountCrystalInfo == "Yes" || ItHasAWaterCristal();
    }






    /// <summary>
    /// Will tell u if this Region contains at least one Water Crystal
    /// </summary>
    /// <returns></returns>
    public bool ItHasAWaterCristal()
    {
        if (_waterCrystalInfo == null)
        {
            _waterCrystalInfo = DefineIfCrystal(H.WaterObstacle);
        }

        return _waterCrystalInfo == "Yes";
    }

    /// <summary>
    /// Will define if this Region has at least one water crystal
    /// </summary>
    private string DefineIfCrystal(H type)
    {
        for (int i = 0; i < TerraCrystals.Count; i++)
        {
            if (TerraCrystals[i].Type1 == type)
            {
                return "Yes";
            }
        }
        return "No";
    }




    /// <summary>
    /// Add crystals to TerraCrystal
    /// </summary>
    /// <param name="c"></param>
    internal void AddCrystal(Crystal c)
    {
        if (c.Type1 == H.WaterObstacle || c.Type1 == H.MountainObstacle || c.Type1 == H.LinkRect)
        {
            AddToTerraCrystals(c);
        }
        //if is way or obstacle will put it here
        else if (c.Type1 == H.Obstacle || c.Type1.ToString().Contains("Way"))
        {
            AddToObstacleCrystals(c);
        }
    }

    /// <summary>
    /// This are the crystals tht get added by buildins
    /// </summary>
    /// <param name="c"></param>
    private void AddToObstacleCrystals(Crystal c)
    {
        if (!_obstaCrystals.Contains(c))
        {
            _obstaCrystals.Add(c);
        }
    }

    void AddToTerraCrystals(Crystal c)
    {
        if (!_terraCrystals.Contains(c))
        {
            _terraCrystals.Add(c);
        }
    }

    /// <summary>
    /// Define to which Land Zone I belong to
    /// </summary>
    public void DefineWhichLandZoneIBelongTo()
    {
        // Will return different names of the Crystall types H.LinkRect I have in this region
        //ReturnDiffZonesNames
        var myZones = ReturnDiffZonesNames();

        if (myZones.Count == 1)
        {
            _landZoneID = myZones[0];
        }
        else if(myZones.Count > 1)
        {
            _landZoneID = "Mixed";
        }
    }

    /// <summary>
    /// Will return the different ZOnes names. 
    /// </summary>
    /// <returns></returns>
    List<string> ReturnDiffZonesNames()
    {
        return ReturnDiffCrystals(_terraCrystals, H.LinkRect);
    }

        /// <summary>
    /// Will return diff crystals of the type pass, crystals wont be repeated
    /// </summary>
    /// <returns></returns>
    List<string> ReturnDiffCrystals(List<Crystal> list, H typeP)
    {
        List<string> res = new List<string>();

        for (int i = 0; i < list.Count; i++)
        {
            if (!res.Contains(list[i].Name) && list[i].Type1 == typeP)
            {
                res.Add(list[i].Name);
            }
        }

        return res;
    }

    /// <summary>
    /// Created to Report Ambience Sound. I need to see how many diff crsytals are
    /// to detect River, Ocean shore and Full Ocean
    /// </summary>
    /// <returns></returns>
    string DefineWhichAudioIReport()
    {
        //106 ocean shore, 84 river , 64 riv
        if (Index == 106 || Index == 84 || Index == 64)
        {
            var a = 1;
        }

        var myCrsytals = ReturnDiffCrystalsNames();
        //crystals that are seaType 
        var seaTypes = myCrsytals.Where(a => a.Type1 == H.WaterObstacle).ToList();
        var mountTypes = myCrsytals.Where(a => a.Type1 == H.MountainObstacle).ToList();

        var linkTypes = myCrsytals.Where(a => a.Type1 == H.LinkRect).ToList();
        var uniqueZones = ReturnDiffCrystals(linkTypes, H.LinkRect);

        if (myCrsytals.Count == 1)
        {
            return "InLand";
        }
        else if (mountTypes.Count > 0)
        {
            return "Mountain";
        }
        else if (myCrsytals.Count > 1 && uniqueZones.Count == 0)
        {
            return "Later";//OceanSHore
        }   
        else if (myCrsytals.Count > 1 && uniqueZones.Count == 1)
        {
            //this ones will find later if have a Full Ocean around if they do that
            //it is OceanShore. Otherwise is River
            return "Later";
        }
        else if (myCrsytals.Count > 1 && uniqueZones.Count > 1)
        {
            return "River";
        }

        if (!UTerra.IsOnTerrain(U2D.FromV2ToV3(Region.center)))
        {
            return "OutOfTerrain";
        }

        return "FullOcean";
    }

    /// <summary>
    /// Will return Crystalls are in this region
    /// </summary>
    /// <returns></returns>
    List<Crystal> ReturnDiffCrystalsNames()
    {
        List<Crystal> res = new List<Crystal>();
        List<string> keyers = new List<string>();

        for (int i = 0; i < _terraCrystals.Count; i++)
        {
            if (!keyers.Contains(_terraCrystals[i].Name))
            {
                keyers.Add(_terraCrystals[i].Name);
                res.Add(_terraCrystals[i]);
            }
        }

        return res;
    }

    /// <summary>
    /// Will remove all crsutals in the region that match the parentID
    /// </summary>
    /// <param name="parentId"></param>
    internal void RemoveCrystal(string parentId)
    {
        var crystals = _obstaCrystals.Where(a => a.ParentId == parentId).ToList();

        for (int i = 0; i < crystals.Count; i++)
        {
            _obstaCrystals.Remove(crystals[i]);
            Debug.Log("Crystal removed: " + parentId+".date:"+Program.gameScene.GameTime1.TodayYMD());
        }
    }

    /// <summary>
    /// For the ones are not determined yet.
    /// If has a FullOcean around it is a Ocean shore. Otherwise
    /// a rive
    /// </summary>
    internal void DoAroundAudioSurvey()
    {
        var around = MeshController.CrystalManager1.ReturnMySurroundRegions(_region.center);

        for (int i = 0; i < around.Count; i++)
        {
            var crystalIndex = around[i];
            if (MeshController.CrystalManager1.CrystalRegions[crystalIndex].WhatAudioIReport == "FullOcean")
            {
                WhatAudioIReport = "OceanShore";
                return;
            }
        }
        WhatAudioIReport = "River";
        
    }

    public float TempDistance { get; set; }

    /// <summary>
    /// The center position of the rect of the region 
    /// </summary>
    /// <returns></returns>
    internal Vector3 Position()
    {
        return U2D.FromV2ToV3(Region.center);
    }
}
