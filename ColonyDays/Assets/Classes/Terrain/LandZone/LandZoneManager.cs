using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LandZoneManager
{
    SMe m  = new SMe();
    private List<Vector3> _commomLayer; 

    List<LandZone> _landZones = new List<LandZone>(); 
    LandZone _current = new LandZone();

    private bool load;

    public LandZoneManager()
    {
        load = true;
    }



    public void AddANewLandZone()
    {
        _current = new LandZone(_commomLayer);
    }


    public List<Vector3> CommomLayer
    {
        get { return _commomLayer; }
        set { _commomLayer = value; }
    }

    public List<LandZone> LandZones
    {
        get { return _landZones; }
        set { _landZones = value; }
    }



    private int secCount;
    private float valYSearch = 0.0001f;//0.05f
    private void PullMostCommonYLayer()
    {
        var yS = UList.FindYAxisCommonValues(m.AllVertexs, H.Descending);
        float yCommon = UList.FindFirstYBelow(yS, m.IniTerr.MathCenter.y);

        _commomLayer = UList.FindVectorsOnSameHeight(m.Vertices.ToList(), yCommon, valYSearch);

        Debug.Log("_commomLayer.Count:"+_commomLayer.Count);

        if (_commomLayer.Count == 0 && secCount < 1000)
        {
            valYSearch += 0.001f;
            secCount++;
            PullMostCommonYLayer();
        }
        else if (_commomLayer.Count == 0 && secCount >= 1000)
        {
            throw new Exception("Not pull anything from CommonY Layer");
        }
        
    }


    private bool addNew;
	// Update is called once per frame
	public void Update () 
    {
	    if (load)
	    {
	        Load();
	    }

        _current.Update();

	    if (addNew)
	    {
	        addNew = false;
            _current.StartLinking(_commomLayer);
           //Debug.Log("nw: cnt:" + _commomLayer.Count);
	    }
	}

    internal void Create()
    {
        PullMostCommonYLayer();
        AddANewLandZone();
    }

    public void AddNewLinkRectToCurrentLandZone()
    {
        addNew = true;
    }

    /// <summary>
    /// This is call when all the linking of the LinkRects is done
    /// </summary>
    public void FirstLinkRectsLinkDone(H type)
    {
        CreateDiffLandZones();

        //if is linkRect calling this will try to simplified
        if (type == H.LinkRect)
        {
            SimpliFyLandZones();  
        }
        //else will save and show 
        else
        {
            AddAllLandZonesLinkRectsToItsRegions();
            MeshController.CrystalManager1.DefineRegionLandZone();

            Save();
            DebugShowNames();

            //save is handled in CrystalManager     
            //m.MeshController.WaterBound1.Create();
        }
    }

    /// <summary>
    /// Will add all the LinkRects in LandZones in their respective Region
    /// </summary>
    private void AddAllLandZonesLinkRectsToItsRegions()
    {
        for (int i = 0; i < LandZones.Count; i++)
        {
            for (int j = 0; j < LandZones[i].LinkRects.Count; j++)
            {
                var crysta = LandZones[i].LinkRects[j];

                MeshController.CrystalManager1.AddCrystalToItsRegion(crysta);
            }
        }
    }

    /// <summary>
    /// Will try to link the LandZones. Since they always come split on the same pysical land
    /// </summary>
    private void SimpliFyLandZones()
    {
        for (int i = 0; i < _landZones.Count; i++)
        {
            _landZones[i].SetCrystalProps();
            MeshController.CrystalManager1.AddCrystal(_landZones[i]);
        }
        MeshController.CrystalManager1.LinkCrystals();
    }

    /// <summary>
    /// Will show names on game 
    /// </summary>
    void DebugShowNames()
    {
        for (int i = 0; i < _current.LinkRects.Count; i++)
        {
            //   //Debug.Log(_current.LinkRects[i].Name);
            UVisHelp.CreateText(U2D.FromV2ToV3(_current.LinkRects[i].Position), _current.LinkRects[i].Name, 150);
        }

        for (int i = 0; i < _landZones.Count; i++)
        {
            //   //Debug.Log(_current.LinkRects[i].Name);
            UVisHelp.CreateText(U2D.FromV2ToV3(_landZones[i].CalcPosition()) + new Vector3(0,15,0), _landZones[i].LandZoneName, 1400);
        }
    }

    /// <summary>
    /// Will create diff land zones based on the different names found on LinkRects
    /// </summary>
    private void CreateDiffLandZones()
    {
        //bz is called twice 
        _landZones.Clear();

        var zoneNames = ReturnDiffZonesNames();

        for (int i = 0; i < zoneNames.Count; i++)
        {
            _landZones.Add(new LandZone(zoneNames[i]));
        }

        AddToRespectiveZone();
    }

    /// <summary>
    /// Will add the llinkRect to respective zone
    /// </summary>
    void AddToRespectiveZone()
    {
        for (int i = 0; i < _current.LinkRects.Count; i++)
        {
            var curr = _current.LinkRects[i];

            for (int j = 0; j < _landZones.Count; j++)
            {
                if (_landZones[j].LandZoneName == curr.Name)
                {
                    _landZones[j].LinkRects.Add(curr);
                }
            }
        }
    }

    List<string> ReturnDiffZonesNames()
    {
        List<string>res=new List<string>();

        for (int i = 0; i < _current.LinkRects.Count; i++)
        {
            if (!res.Contains(_current.LinkRects[i].Name))
            {
                res.Add(_current.LinkRects[i].Name);
            }
        }

        return res;
    }

    #region SaveLoad

    void Save()
    {
        m.SubMesh.LandZoneManager1 = this;
        m.MeshController.WriteXML();
    }

    void Load()
    {
        if (m.SubMesh == null)
        {
            return;
        }

        load = false;

        LandZones = m.SubMesh.LandZoneManager1.LandZones;

        //if (LandZones.Count == 0)
        //{
            //Create();
        //}
    }


    #endregion
}
