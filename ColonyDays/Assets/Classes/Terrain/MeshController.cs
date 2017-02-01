using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshController : ControllerParent
{
    /// <summary>
    /// Shared from MeshCrontoller
    /// </summary>
    public  RaycastHit HitMouseOnTerrain;

    //Mesh classes helpers, these all inherit from General: Monobehaviuor
    public  Malla Malla;
    public  UPoly Poly;
    public  SubDivider subDivide;
    public  Vertexer Vertex;
    public  InitializerTerrain iniTerr;
    public  SubPolyr SubPolyr;

    //dont inherit
    public SubMeshData subMesh;

    public bool IsMouseOnTerrain;

    //Malla the big lot vertices held in current hovering
    public List<Vector3> CurrentHoverVertices = new List<Vector3>();

    //SubPolygons
    public List<Vector3> SubPolysList = new List<Vector3>();
    public Vector3[] Vertices;
    //---------------------------------------------------

    private RaycastHit hitMouse;
    private CamControl camera;

    private Mesh mesh;
    List<Vector3> dirs90 = new List<Vector3>();

    //Lot
    private float lotStepZ;
    private float lotStepX;
    private List<Lot> _lots = new List<Lot>();
    public List<Vector3> wholeMalla;
    private Vector3 nextStart;
    float zLot;
    private int lastRowCounter;
    private bool isTerraScanning;
    
    private bool isToSetRealVerticesOnLots;//after terra scaning is done we want to set the vertices real on all lots
    private int realVertiesDoneCount;//all the lots that the real vertices were defined

    //for Lot obj ///Defined in ScanTerra()
    Vector3 lotStart = new Vector3();
    Vector3 lotEnd = new Vector3();

    //whole
    public bool IsLoading;
    public List<Vector3> AllVertexs = new List<Vector3>();

    Grid grid = new Grid();

    WaterBound _waterBound = new WaterBound();
    LandZoneManager _landZoneManager = new LandZoneManager();



    private static CrystalManager _crystalManager = new CrystalManager();
    private static BuyRegionManager _buyRegionManager;
    
    public static CrystalManager CrystalManager1
    {
        get { return _crystalManager; }
        set { _crystalManager = value; }
    }



    public List<Lot> Lots
    {
        get { return _lots; }
        set { _lots = value; }
    }

    public WaterBound WaterBound1
    {
        get { return _waterBound; }
        set { _waterBound = value; }
    }

    public LandZoneManager LandZoneManager1
    {
        get { return _landZoneManager; }
        set { _landZoneManager = value; }
    }

    public static BuyRegionManager BuyRegionManager1
    {
        get { return _buyRegionManager; }
        set { _buyRegionManager = value; }
    }

    public void Destroy()
    {
        Malla.Destroy();
        base.Destroy();
    }

    public static void InitBuyRegions()
    {
        _buyRegionManager = new BuyRegionManager();
    }

    // Use this for initialization
    void Start()
    {
        camera = USearch.FindCurrentCamera();

        Malla = (Malla)General.Create(Root.malla, container: Program.ClassContainer.transform);
        Poly = new UPoly();
        subDivide = new SubDivider();
        Vertex = new Vertexer();
        iniTerr = new InitializerTerrain();

        iniTerr.Initializer(ref Vertices, ref mesh);
        iniTerr.InitializeMallaStats(Vertices, ref wholeMalla, ref nextStart, ref zLot);

        SubPolyr = new SubPolyr();
        subMesh = new SubMeshData();
        IsLoading = true;

        //bz is static and if a new game is started needs to clean up and start again 
        CrystalManager1 = new CrystalManager();
    }

    void Update()
    {
        int globalInPolyDiv = 5;//how many div ex 5x5 a real poly will have
        int globalXStep = 5;//real polys in X will cover 
        int globalZStep = 5;//real polys in Z will cover

        int currentVertColAmount = 7;//used to be 3
        int currentVertRowAmount = 7;//used to be 3

        UpdateHitMouseOnTerrain();
        DrawDebug90Deg();

        if (_buyRegionManager!=null)
        {
            _buyRegionManager.Update();
        }


        if (isTerraScanning && IsLoading)
        {
            ScanProcedure(globalInPolyDiv, globalXStep, globalZStep);
        }
        else if (!isTerraScanning)
        {
            LoadingVertexMesh(globalInPolyDiv, globalXStep, globalZStep);
        }


        //other wise is not needed 
        if (BuildingPot.Control!=null && BuildingPot.Control.CurrentSpawnBuild!=null)
        {
            CurrentHoverVertices = Vertex.UpdateCurrentVertices(Malla, currentVertColAmount, currentVertRowAmount, lotStepX, lotStepZ, HitMouseOnTerrain);
        }

        if (isToSetRealVerticesOnLots)
        {
            DefineRealVerticesOnLots();
        }

        if (Developer.IsDev && Input.GetKeyUp(KeyCode.F12))
        {
            // grid.LoadGridRoutine();
            _waterBound.Create();

            //PersonController.CrystalManager1.ShowLines();
            //_landZoneManager.Create();

            //PersonController.CrystalManager1.DefineRegionLandZone();

            //MakeBridgeReqDebug();
        }
        grid.Update();

        _waterBound.Update();
        _landZoneManager.Update();
        CrystalManager1.Update();
    }

    /// <summary>
    /// the 2 first building must be in diff lands and most be at least 1 brdige 
    /// </summary>
    private void MakeBridgeReqDebug()
    {
        var builA = BuildingPot.Control.Registro.AllBuilding.ElementAt(0).Value;
        var builB = BuildingPot.Control.Registro.AllBuilding.ElementAt(1).Value;

        var a = BuildingPot.Control.BridgeManager1.ReturnBestPath
            (builA.LandZone1[0], builB.LandZone1[0]);

        var aa = a;

        //CryRouteManager c = new CryRouteManager(builA, builB);
    }

    public bool IsFullyLoaded()
    {
        if (subMesh == null || Malla == null)
        {
            return false;
        }

        return AllVertexs.Count == subMesh.amountOfSubVertices && AllVertexs.Count > 0
               && Malla.Lots.Count > 0 && !IsLoading;
    }


    /// <summary>
    /// Will try to load from LoadMeshFromFile() if cant will throw ScanProcedure()
    /// then will when the scan is done will WriteXMLMesh()
    /// </summary>
    /// <param name="inPolyDiv">how many div ex 5x5 a real poly will have</param>
    /// <param name="polyX">real polys in X will cover in each scan pass</param>
    /// <param name="polyZ">real polys in Z will cover in each scan pass</param>
    void LoadingVertexMesh(int inPolyDiv, int polyX, int polyZ)
    {
        //print(iniTerr.Columns + ".Columns | " + iniTerr.Rows + ".Rows");
        if (IsLoading)
        {
            LoadMeshFromFile(inPolyDiv, polyX, polyZ);
            if (AllVertexs.Count == subMesh.amountOfSubVertices && AllVertexs.Count > 0
                && Malla.Lots.Count > 0)
            {
                IsLoading = false;
                //Program.gameScene.controllerMain.TerraSpawnController.Release();

                return;
            }
            nextStart = wholeMalla[0];
            ScanProcedure(inPolyDiv, polyX, polyZ);
        }

        if (!isTerraScanning && !IsLoading && Malla.Lots.Count == 0)
        {
           //_waterBound.Create();
           FinalWrite();
        }
    }

    /// <summary>
    /// If realvertices in lots were not found will flagged. And when all were done will finally write the XML
    /// </summary>
    void FinalWrite()
    {
        if (!isToSetRealVerticesOnLots && realVertiesDoneCount == 0)
        {
            isToSetRealVerticesOnLots = true;
        }
        else if (isToSetRealVerticesOnLots && realVertiesDoneCount == Lots.Count)
        {
            Malla.Lots = Lots;
            subMesh.AllSubMeshedLots = Malla.Lots;
            subMesh.amountOfSubVertices = AllVertexs.Count;
            //subMesh.mostCommonYValue = UList.FindMostCommonValue(H.Y, Vertices.ToList());
           
            WriteXML();
            //print(subMesh.amountOfSubVertices + " subMesh.amountOfSubVertices ");
            isToSetRealVerticesOnLots = false;

            //subMesh.grid.LoadGridRoutine();//will load the grid routine
        }
    }

    public void WriteXML()
    {
        XMLSerie.WriteXMLMesh(subMesh);
    }

    /// <summary>
    /// Will define real vertices on Lot this method is called from update if isToSetRealVerticesOnLots=true
    /// </summary>
    void DefineRealVerticesOnLots()
    {
        if (realVertiesDoneCount < Lots.Count)
        {
            Lots[realVertiesDoneCount].SetRealVertices();
            realVertiesDoneCount++;
        }
    }

    /// <summary>
    /// Trys to load submesh from file.. if cant will scan the terrain
    /// </summary>
    /// <param name="inPolyDiv">how many div ex 5x5 a real poly will have</param>
    /// <param name="polyX">real polys in X will cover in each scan pass</param>
    /// <param name="polyZ">real polys in Z will cover in each scan pass</param>
    void LoadMeshFromFile(int inPolyDiv, int polyX, int polyZ)
    {
        iniTerr.InitializeLotStepVal(ref nextStart, subDivide, wholeMalla, Vertices, ref lotStepX, ref lotStepZ,
            inPolyDiv, polyX, polyZ);

        subMesh = XMLSerie.ReadXMLMesh();
        if (subMesh == null)
        {
            print("error loading .XML: ");
            IsLoading = true;
            isTerraScanning = true;
            subMesh = new SubMeshData();
            return;
        }
        //print(subMesh.amountOfSubVertices + " subMesh.amountOfSubVertices");
        float dist = 0.001f;

        Malla.Lots = subMesh.AllSubMeshedLots;

        for (int i = 0; i < Malla.Lots.Count; i++)
        {
            for (int j = 0; j < Malla.Lots[i].LotVertices.Count; j++)
            {
                AllVertexs.Add(Malla.Lots[i].LotVertices[j]);
            }
        }
        //print(AllVertexs.Count + ".AllVertexs.count. in mshContrl");
    }

    /// <summary>
    /// Scan Routine will add new lots to lots list from the result of ScanTerraRetLotVertex()
    /// </summary>
    /// <param name="inPolyDiv">how many div ex 5x5 a real poly will have</param>
    /// <param name="polyX">real polys in X will cover in each scan pass</param>
    /// <param name="polyZ">real polys in Z will cover in each scan pass</param>
    void ScanProcedure(int inPolyDiv, int polyX, int polyZ)
    {
        //return;

        List<Vector3> newLotVertex = new List<Vector3>();
        newLotVertex = ScanTerraRetLotVertex(inPolyDiv, polyX, polyZ);
        Lots.Add(new Lot(newLotVertex, Lots.Count, lotStart, lotEnd));
        //grid.SortThemOut(newLotVertex);
        //print("LotsScanned:" + Lots.Count);
        AllVertexs = UList.AddOneListToList(AllVertexs, newLotVertex);
    }

    public void ForcedTerraScanning()
    {
        //stop forced terra scanning
        if (isTerraScanning)
        {
            IsLoading = false;
            isTerraScanning = false;
        }
        else
        {   //starts forced scan terra 
            nextStart = wholeMalla[0];
            IsLoading = true;
            isTerraScanning = true;
            ReinitalizeAllMeshVars();
        }
    }

    void ReinitalizeAllMeshVars()
    {
        AllVertexs.Clear();
        Lots.Clear();
        Malla.Lots.Clear();

        if (subMesh != null)
        {
            subMesh.AllSubMeshedLots.Clear();
            subMesh.amountOfSubVertices = 0;
        }
    }

    /// <summary>
    /// It shows the abanico on the mouse on the Scene View Windows 
    /// </summary>
    void DrawDebug90Deg()
    {
        dirs90 = new List<Vector3>();
        dirs90.Add(new Vector3(0, 0, 1));
        if (dirs90.Count < 41)
        {
            for (float i = 0; i > -1f; i = i - 0.05f)
            {
                dirs90.Add(new Vector3(i, 0, 1f));
            }

            for (float i = 1; i > 0; i = i - 0.05f)
            {
                dirs90.Add(new Vector3(-1f, 0, i));
            }
        }
        for (int i = 0; i < dirs90.Count; i++)
        {
            Vector3 startingRay = HitMouseOnTerrain.point + dirs90[i] * 1;
            Debug.DrawRay(startingRay, dirs90[i] * 5, Color.green);
        }
    }

    public void UpdateHitMouseOnTerrain()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        // This would cast rays only against colliders in layer 8.
        int layerMask = 1 << 8;
        // Does the ray intersect any objects in the layer 8 "Terrain Layer"
        if (Physics.Raycast(camera.transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out HitMouseOnTerrain,
            Mathf.Infinity, layerMask))
        {
            IsMouseOnTerrain = true;
        }
        else
        {
            //Debug.Log("Mouse Did not Hit Layer 8: Terrain");
            IsMouseOnTerrain = false;
        }
    }

    /// <summary>
    /// Scan all the terrain and will return the lot vertexs.. will do the sequence
    /// too to scan all the terrain 
    /// </summary>
    /// <param name="inPolyDiv">how many div ex 5x5 a real poly will have</param>
    /// <returns>List Vector3 with a Lot full of fake vertices</returns>
    List<Vector3> ScanTerraRetLotVertex(int inPolyDiv /*ex 5x5 inside the polygon*/, 
        int polyX, int polyZ)
    {
        isTerraScanning = true;
        List<Vector3> lot = new List<Vector3>();

        if (nextStart.x < wholeMalla[1].x)
        {
            if (lotStepX == 0)
            {
                iniTerr.InitializeLotStepVal(ref nextStart, subDivide, wholeMalla, Vertices, 
                    ref lotStepX, ref lotStepZ);
            }
            if (nextStart != wholeMalla[0])
            {
                print("nextStart != wholeMalla[0]");
                nextStart.z = zLot;
            }

            lotStart = nextStart;//nextstart get updated once goes into tht method
            lot = subDivide.SubDivideLot(ref nextStart, polyX, polyZ, 
                iniTerr.StepX, iniTerr.StepZ, inPolyDiv, Vertices);
            lotEnd = nextStart;
            Malla.XLotColumns++;
        }
        if (nextStart.x  >= wholeMalla[1].x)
        {
            Malla.IsXLotColumnsLocked = true;
            zLot = zLot - lotStepZ;
            nextStart.x = wholeMalla[0].x;
            Malla.ZLotRows++;
        }
        if (nextStart.z - 0.1f <= wholeMalla[3].z)
        {
            lastRowCounter++;
            if (lastRowCounter == Malla.XLotColumns)
            {
                isTerraScanning = false;
                IsLoading = false;
                Malla.IsZLotRowsLocked = true;
            }
        }
        return lot;
    }
}