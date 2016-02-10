using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/* Find and the bound with between wwater and land
 * and created colliders there for the Router be able to 
 * detect is colliding with Water
 * 
 * Important: Remember that each hole in the terrain needs water .. 
 * other wise this wont be detect it by Routes.cs
 */
public class WaterBound  {

    SMe m = new SMe();
    List<Vector3> _marineBounds = new List<Vector3>();
    List<Vector3> _mountainBounds = new List<Vector3>();

    List<Building> _marineHelper = new List<Building>();

    private List<Vector2> _seaPath = new List<Vector2>(); 

    public List<Vector3> MarineBounds
    {
        get { return _marineBounds; }
        set { _marineBounds = value; }
    }

    public List<Vector3> MountainBounds
    {
        get { return _mountainBounds; }
        set { _mountainBounds = value; }
    }

    public List<Vector2> SeaPath
    {
        get { return _seaPath; }
        set { _seaPath = value; }
    }

    private bool load;
    private bool create;

    public WaterBound()
    {
        load = true;
    }

    //Creates the bounds
    internal void Create()
    {
        if (m.AllVertexs.Count > 0 &&( _marineBounds.Count == 0 || _mountainBounds.Count == 0))
        {
            MarineBounds.Clear();
            MountainBounds.Clear();

            //create = false;
            FindVertexUnderNeathWater();
        }
        //DebugLines();
    }

    void DebugLines()
    {
        Line one = new Line(new Vector2(), new Vector2(0,1) );
        Line two = new Line(new Vector2(2,0), new Vector2(2, 1));

       //Debug.Log("Interse:"+one.IsIntersecting(two));
    }

    /// <summary>
    /// Used to Load the Bounds
    /// 
    /// Will load and then will create the bounds 
    /// </summary>
    internal void LoadBounds(List<Vector3> marineBounds, List<Vector3> mountBounds)
    {
        _marineBounds = marineBounds;
        _mountainBounds = mountBounds;
    }


#region Marine

    /// <summary>
    /// This are the vertices that create the marine bound 
    /// </summary>
    void FindVertexUnderNeathWater()
    {
        var yS = UList.FindYAxisCommonValues(m.AllVertexs, H.Descending);
        float firstUnder = UList.FindFirstYBelow(yS, Program.gameScene.WaterBody.transform.position.y);
        
        _marineBounds = UList.FindVectorsOnSameHeight(m.AllVertexs, firstUnder, 0.05f);//0.07f
        _marineBounds = UList.EliminateDuplicatesByDist(_marineBounds, 0.3f);//0.2



        
        float firstClos = UList.FindFirstYBelow(yS, m.IniTerr.MathCenter.y - 1f);
        var closer = UList.FindVectorsOnSameHeight(m.AllVertexs, firstClos, 0.02f);//0.07f
        closer = UList.EliminateDuplicatesByDist(closer, 0.3f);//0.2

        _marineBounds.AddRange(closer);



        
        float lowest = UMath.ReturnMinimumDifferentThanZero(yS)+ 1f  ;
        var low = UList.FindVectorsOnSameHeight(m.AllVertexs, lowest, 0.01f);//0.07f
        low = UList.EliminateDuplicatesByDist(low, 0.4f);//0.2

        _marineBounds.AddRange(low);





        //RenderMarineBounds();
       //Debug.Log(_marineBounds.Count + " _marineBounds");
        //AddMarinePositions();

        DefineSeaPath(_marineBounds);
        addMarine = true;

        //AddMarineBoundsToCrystal();
    }



    private bool addMarine;
    private int counter;
    void AddMarinePositions()
    {
        if (counter <  _marineBounds.Count)
        {
            AddEachCrystal(_marineBounds[counter], Time.time.ToString(), H.WaterObstacle);
            counter++;
        }
        else
        {
            counter = 0;
            addMarine = false;
            MeshController.CrystalManager1.LinkCrystals();
        }
    }

    void AddEachCrystal(Vector3 pos, string parentID, H typeCrys)
    {
        Crystal crystal = new Crystal(pos, typeCrys, parentID);

        MeshController.CrystalManager1.AddCrystal(crystal);
    }
    
    void RenderMarineBounds()
    {
        for (int i = 0; i < _marineBounds.Count; i++)
        {
            //_marineHelper.Add((Building) 
             //   General.Create(Root.tallBoxR, _marineBounds[i], container: Program.gameScene.Terreno.transform));
            
            General.Create(Root.yellowCube, _marineBounds[i], container: Program.gameScene.Terreno.transform);
        }
        //UVisHelp.CreateHelpers(_marineBounds, Root.tallBoxR);
    }

    /// <summary>
    /// Will define roughly wht is the river  and sea path 
    /// 
    /// tht list is just the whole shore order by distance from a point 
    /// </summary>
    public void DefineSeaPath(List<Vector3> list)
    {
        int local = 0;
        Vector2 accum = new Vector3();

        for (int i = 0; i < list.Count; i++)
        {
            if (local == 25)//10
            {
                _seaPath.Add(accum / 25);

                accum = new Vector3();
                local = 0;
            }
            accum = accum + U2D.FromV3ToV2(list[i]);
            local++;
        }

        //UVisHelp.CreateHelpers(_seaPath, Root.blueCube);
    }


    List<VectorM> _seaM = new List<VectorM>();
    /// <summary>
    /// Will return a new Vector 3 pushed away from the Bottom Sea. The botton of the water 
    /// </summary>
    /// <param name="compare"></param>
    /// <param name="much"></param>
    /// <returns></returns>
    public Vector2 ReturnPushMeAwayFromSeaBottom(Vector2 compare, float much)
    {
        InitSeaM(compare);
        Vector2 closest = new Vector2(_seaM[0].Point.x, _seaM[0].Point.z); ;

        return Vector2.MoveTowards(compare, closest, -much);
    }

    void InitSeaM(Vector2 compare)
    {
        _seaM.Clear();
        for (int i = 0; i < _seaPath.Count; i++)
        {
            var v3 = new Vector3(_seaPath[i].x, m.IniTerr.MathCenter.y, _seaPath[i].y);
            _seaM.Add(new VectorM((v3), U2D.FromV2ToV3(compare)));
        }
        _seaM = _seaM.OrderBy(a => a.Distance).ToList();
    }

#endregion




    public void Update()
    {
        if (addMarine)
        {
            AddMarinePositions();
        }
        if (addMount)
        {
            AddMountPositions();
        }

        if (load)
        {
            Load();
        }
    }








#region Mountain
    //the path mountains follow. this is to puch lines a bit towards the outside from the mountain 
    private List<Vector3> _mountainPath = new List<Vector3>(); 

    public void FindVertexAboveTerrainLevel()
    {
        var yS = UList.FindYAxisCommonValues(m.AllVertexs, H.Ascending);
        float firstAbove = UList.FindFirstYAbove(yS, m.IniTerr.MathCenter.y + 2f);
      
        _mountainBounds = UList.FindVectorsOnSameHeight(m.AllVertexs, firstAbove, 0.07f);//0.07f
        _mountainBounds = UList.EliminateDuplicatesByDist(_mountainBounds, 1f);


        //a layer 2 toward the top 
        float lay2 = UList.FindFirstYAbove(yS, m.IniTerr.MathCenter.y + 3f);
        var layer2 = UList.FindVectorsOnSameHeight(m.AllVertexs, lay2, 0.07f);//0.07f
        layer2 = UList.EliminateDuplicatesByDist(layer2, 1f);
        _mountainBounds.AddRange(layer2);

        //a layer 3 toward the top 
        float lay3 = UList.FindFirstYAbove(yS, m.IniTerr.MathCenter.y + 4f);
        var layer3 = UList.FindVectorsOnSameHeight(m.AllVertexs, lay3, 0.07f);//0.07f
        layer3 = UList.EliminateDuplicatesByDist(layer3, 1f);
        _mountainBounds.AddRange(layer3);



        DefineMountPath(yS);

        _mountainBounds.AddRange(_mountainPath);
        //bz was used by marine b4

        Save();

        addMount = true;

        //RenderMountainBounds();
    }

    void DefineMountPath(List<float> yS)
    {
        float highest = UMath.ReturnMax(yS);
        _mountainPath = UList.FindVectorsOnSameHeight(m.AllVertexs, highest, 0.03f);//0.03f
        _mountainPath = UList.EliminateDuplicatesByDist(_mountainPath, 0.4f);//0.2
    }

    List<VectorM> _mountainM = new List<VectorM>();
    private bool addMount;
    /// <summary>
    /// Will return a new Vector 3 pushed away from the Bottom Sea. The botton of the water 
    /// </summary>
    /// <param name="compare"></param>
    /// <param name="much"></param>
    /// <returns></returns>
    public Vector2 ReturnPushMeAwayFromMountTop(Vector2 compare, float much)
    {
        InitMountainM(compare);
        Vector2 closest = new Vector2(_mountainM[0].Point.x, _mountainM[0].Point.z); ;

        return Vector2.MoveTowards(compare, closest, -much);
    }

    /// <summary>
    /// Will order _mountainM by distance from 'compare'
    /// </summary>
    /// <param name="compare"></param>
    void InitMountainM(Vector2 compare)
    {
        _mountainM.Clear();
        for (int i = 0; i < _mountainPath.Count; i++)
        {
            var v3 = new Vector3(_mountainPath[i].x, m.IniTerr.MathCenter.y, _mountainPath[i].y);
            _mountainM.Add(new VectorM((v3), U2D.FromV2ToV3(compare)));
        }
        _mountainM = _mountainM.OrderBy(a => a.Distance).ToList();
    }





    private void AddMountPositions()
    {
        if (counter < _mountainBounds.Count)
        {
            AddEachCrystal(_mountainBounds[counter], Time.time.ToString(), H.MountainObstacle);
            counter++;
        }
        else
        {
            counter = 0;
            addMount = false;
            MeshController.CrystalManager1.LinkCrystals();
        }
    }


    void RenderMountainBounds()
    {
        for (int i = 0; i < _mountainBounds.Count; i++)
        {
            General.Create(Root.blueCube, _mountainBounds[i], container: Program.gameScene.Terreno.transform);
        }
    }


#endregion

    void Save()
    {
        //Save the Terrain bounds 
        m.SubMesh.MarineBounds = MarineBounds;
        m.SubMesh.MountainBounds = MountainBounds;
        m.MeshController.WriteXML();
    }

    private void Load()
    {
        if (m.SubMesh == null)
        {
            return;
        }

        load = false;

        MarineBounds = m.SubMesh.MarineBounds ;
        MountainBounds = m.SubMesh.MountainBounds ;

        //if (MarineBounds.Count == 0 || MountainBounds.Count==0)
        //{
        //    Create();
        //}
    }
}
