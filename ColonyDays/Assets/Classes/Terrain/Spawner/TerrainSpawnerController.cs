using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TerrainSpawnerController : ControllerParent
{
    int loadingIndex;
    private SpawnedData spawnedData;

    bool isSpawned;//tells if a set of obj were spawned
    System.Random rand = new System.Random();
    float minHeightToSpawn;//min height to spawn obj on terrain
    private float maxHeightToSpawn;

    //UNITY EDITOR ManualStart()
    private int multiplier = 80;//75 /80  10

    int howManyTreesToSpawn = 20;//50
    int howManyStonesToSpawn =3;//3
    int howManyIronToSpawn = 3;//3
    int howManyGoldToSpawn = 3;//3
    int howManyOrnaToSpawn = 30;//50  20
    int howManyGrassToSpawn = 0;//40

    List<TerrainRamdonSpawner> _treesPool = new List<TerrainRamdonSpawner>(); 

    //will be use when spawing new obj to know if that position was used alread by another one
    bool[] usedVertexPos;

    private List<string> allTrees = new List<string>()
    {
        // Root.tree1,
        // Root.tree2, Root.tree3, 
        //Root.tree4,
 
        //Root.tree5, 
        //Root.tree6, 
        //Root.tree7,

        
        Root.palm1, Root.palm2,
        Root.palm3, Root.palm4, Root.palm5, Root.palm6, 
        //Root.palm10  ,
        Root.palm20, Root.palm21, Root.palm22, Root.palm23,

        Root.tree21,
        Root.tree22,
        Root.tree23,
        Root.tree24,
        Root.tree25,
    };

    List<string> allStones = new List<string>()
    {
        //Root.stone0, 
        Root.stone1,
        //Root.stone2
        //, Root.stone3,
        //Root.stone4, Root.stone5, Root.stone6, Root.stone7,
    };

    List<string> allIron = new List<string>()
    {
        Root.iron1
        //, Root.iron2, Root.iron3 ,Root.iron4,Root.iron5
    
    };
    //gold stones here 
    List<string> allGold = new List<string>()
    {
        //Root.gold0
         Root.gold1
        //Root.gold2, Root.gold3,
        //Root.gold4
    };

    public static  List<string> allOrna = new List<string>(){ };

    public static List<string> allGrass = new List<string>() 
    { 
        ////Root.grass1, 
        //Root.grass2, Root.grass3  ,
        //Root.grass4, Root.grass5, Root.grass6,
        //Root.grass7, 
        //Root.grass8,
        //Root.grass9,
        ////Root.grass10,
        //Root.grass11, Root.grass12,
    };
    
    List<H> toSpawnList = new List<H>() { H.Tree, H.Stone, H.Iron, H.Gold, H.Ornament, H.Grass };
    private List<int> howManySpawn;//will containt a list in serie of how many spawn for each type

    int toSpawnListCounter;

    public TerrainRamdonSpawnerKey AllRandomObjList = new TerrainRamdonSpawnerKey();//containts all spawned obj
    public List<SpawnedData> AllSpawnedDataList = new List<SpawnedData>();//containts all spawned data serie saved

    private Vector3 centerOfTerrain;//center of terrain, defined in Terreno.MeshController.iniTerr.MathCenter
    private Vector3 voidNWCorner, voidSECorner;//starting zone, will not spawn anything btw them. 

    //List that holds the spawned objects 

    List<TreeVeget> treeList = new List<TreeVeget>();
    List<StoneRock> stoneList = new List<StoneRock>();
    List<IronRock> ironList = new List<IronRock>();
    List<StillElement> ornaList = new List<StillElement>();
    List<StillElement> grassList = new List<StillElement>();
    private bool _isToLoadFromFile;

    public bool IsToLoadFromFile
    {
        get { return _isToLoadFromFile; }
        set { _isToLoadFromFile = value; }
    }

    public void RemoveStillElement(StillElement ele)
    {
        var index = AllRandomObjList.IndexOf(ele);

        AllSpawnedDataList.RemoveAt(index);
        AllRandomObjList.RemoveAt(index);
    }

    public void ReSaveStillElement(StillElement ele)
    {
        //return;

        var index = AllRandomObjList.IndexOf(ele);

        if (index == -1)
        {
            index = AllRandomObjList.ToList().FindIndex(a => a.MyId == ele.MyId);
        }


        AllRandomObjList[index].MaxHeight = ele.MaxHeight;

        AllSpawnedDataList[index].TreeHeight = ele.Height;
        AllSpawnedDataList[index].SeedDate = ele.SeedDate;
        AllSpawnedDataList[index].MaxHeight = ele.MaxHeight;
        AllSpawnedDataList[index].TreeFall = ele.TreeFall;
        AllSpawnedDataList[index].Weight = ele.Weight;
    }

    //void ReSaveStillElement(StillElement ele, int indexOnLists)
    //{
    //    var index = indexOnLists;

    //    AllRandomObjList[index].MaxHeight = ele.MaxHeight;

    //    AllSpawnedDataList[index].TreeHeight = ele.Height;
    //    AllSpawnedDataList[index].SeedDate = ele.SeedDate;
    //    AllSpawnedDataList[index].MaxHeight = ele.MaxHeight;
    //    AllSpawnedDataList[index].TreeFall = ele.TreeFall;
    //    AllSpawnedDataList[index].Weight = ele.Weight;
    //}

    public StillElement Find(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return null;
        }

        if (!AllRandomObjList.Contains(key))
        {
            return null;
        }

        return AllRandomObjList[key] as StillElement;

        //StillElement res = null;
        //res = (StillElement)list.Find(a => a.MyId == key);

        //return res;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="position">The position this obj is around</param>
    /// <returns></returns>
    public StillElement Find(string key, Vector3 position)
    {
        if (string.IsNullOrEmpty(key))
        {
            return null;
        }

        if (!AllRandomObjList.Contains(key))
        {
            return null;
        }

        var ele = AllRandomObjList[key] as StillElement;

        if (Vector3.Distance(ele.transform.position, position) <.5f)
        {
            return ele;
        }
        return null;
    }


    public bool IsToSave;



    private int loadingAllowTimes = 1;//how many times system is allow to load 
    private int loadedTimes = 0;//loaded times

    // Use this for initialization
    void ManualStart()
    {
        //CreateTreePool();

#if UNITY_EDITOR
        multiplier = 2;//20
        howManyGrassToSpawn = 0;//40
#endif

        DefineAllOrnaRoots();
        DefineAllLawnRoots();

        DefineStartVoidArea(20f, 20f);//20,20
        howManySpawn = new List<int>() {
            Multiplier(howManyTreesToSpawn), Multiplier(howManyStonesToSpawn), 
            Multiplier(howManyIronToSpawn), Multiplier(howManyGoldToSpawn),
            Multiplier(howManyOrnaToSpawn), Multiplier(howManyGrassToSpawn)
        };

        float minHeightAboveSeaLevel = 1.2f;//1
        minHeightToSpawn = Program.gameScene.WaterBody.transform.position.y + minHeightAboveSeaLevel;
        maxHeightToSpawn = minHeightToSpawn + 6.9f;


    }

    private void DefineAllLawnRoots()
    {
        var howManyLawnInEachFolder = 15+1;
        var add = "";

        //one ofr each folder
        //for (int i = 1; i < howManyLawnInEachFolder; i++)
        //{
        //    if (i < 10)
        //    {
        //        add = "0";
        //    }
        //    else add = "";

        //    allGrass.Add("Prefab/Terrain/Spawner/LawnBrown/Lawn" + add + i);
        //}
        for (int i = 1; i < howManyLawnInEachFolder; i++)
        {
            if (i < 10)
            {
                add = "0";
            }
            else add = "";

            allGrass.Add("Prefab/Terrain/Spawner/LawnGreenDark/Lawn" + add + i);
        }
        //for (int i = 1; i < howManyLawnInEachFolder; i++)
        //{
        //    if (i < 10)
        //    {
        //        add = "0";
        //    }
        //    else add = "";

        //    allGrass.Add("Prefab/Terrain/Spawner/LawnGreenLight/Lawn" + add + i);
        //}
    }

    private void DefineAllOrnaRoots()
    {
        for (int i = 1; i < 26+1; i++)
        {
            allOrna.Add("Prefab/Terrain/Spawner/Orna/Orna"+i);
        }
    }

    /// <summary>
    /// So I dont have to change from 10 to 10,000 everytime a need to spawn more or less lawn for ex
    /// </summary>
    /// <returns></returns>
    int Multiplier(int mul)
    {


        return mul*multiplier;
    }

    private bool wasStarted;
    // Update is called once per frame
    void Update()
    {
        if (MeshController.CrystalManager1 == null || MeshController.CrystalManager1.CrystalRegions.Count==0)
        {
            return;
        }

        if (!wasStarted)
        {
            wasStarted = true;
            ManualStart();
        }


        if (centerOfTerrain == new Vector3() && m.IniTerr.MathCenter != null
            && m != null && m.IniTerr != null)
        {   //define center of terrain
            centerOfTerrain = m.IniTerr.MathCenter;
        }
        //will proceed once Terreno.MeshController is done loading.Otherwise freeze machine
        if (!p.MeshController.IsLoading && IsToSave)
        {
            SpawnAllObj();
            print(IsToSave+ " isToSave = true, we are generating all spanwened obj now ");

        }

        if (toSpawnListCounter == toSpawnList.Count && IsToSave && toSpawnList.Count > 0)
        {
            SaveData();
            IsToSave = false;

            //CreateTreePool();
        }

        if (p.MeshController.IsFullyLoaded() && loadedTimes < loadingAllowTimes)
        {
            LoadData();
            if (spawnedData == null || spawnedData.AllSpawnedObj.Count == 0)
            {IsToSave = true;}
            loadedTimes++;
        }

        if (!p.MeshController.IsLoading && IsToLoadFromFile) { LoadFromFile(); }

        if (AllRandomObjList.Count > 0 && !IsToSave && !IsToLoadFromFile && treeList.Count == 0)
        {
            for (int i = 0; i < AllRandomObjList.Count; i++)
            {
                CreateOrUpdateSpecificsList(AllRandomObjList[i].HType, AllRandomObjList[i], H.Create);
                //print(treeList.Count + " treeList.Count");
                //print(stoneList.Count + " stoneList.Count");
            }
        }
    }

    /// <summary>
    /// Spawn all objects routine, trys to spawn current obj list.
    /// if was sueccesfull will move on to next obj list
    /// </summary>
    void SpawnAllObj()
    {
        if (toSpawnListCounter < toSpawnList.Count)
        {
            isSpawned = false;
            //trys to spawn current obj
            SpawnObj(toSpawnList[toSpawnListCounter], howManySpawn[toSpawnListCounter]);
            //if was sueccesfull will move on to next obj list
            if (isSpawned) { toSpawnListCounter++; }
        }
    }

    /// <summary>
    /// Spawns specific type of object 
    /// </summary>
    /// <param name="typePass">type of object</param>
    /// <param name="amountToSpawn">how many will spawn</param>
    void SpawnObj(H typePass, int amountToSpawn)
    {
        if (!isSpawned)
        {
            if (p.MeshController.AllVertexs.Count > 0)
            {
                List<Vector3> AllVertexs = p.MeshController.AllVertexs;
                //print(AllVertexs.Count + ".|AllVertexs.count");

                if (usedVertexPos == null) { usedVertexPos = new bool[AllVertexs.Count]; }

                for (int i = 0; i < amountToSpawn; i++)
                {
                    int index = rand.Next(0, AllVertexs.Count);
                    //print(index + " index");

                    //this is where I ask for a root and will return me a random string root
                    int rootToSpawnIndex = ReturnRandomRootIndex(typePass);
                    bool isOnTheStartZone = UMesh.Contains(AllVertexs[index], voidNWCorner, voidSECorner);
                    bool regionContainTerraCry =
                        MeshController.CrystalManager1.DoesMyRegionHasTerraCrystal(AllVertexs[index]);

                    if (AllVertexs[index].y > minHeightToSpawn && AllVertexs[index].y < maxHeightToSpawn &&
                        !usedVertexPos[index] && !isOnTheStartZone && !regionContainTerraCry)
                    {
                        Vector3 finaPos = AssignRandomIniPosition(AllVertexs[index], 0);
                        
                        CreateObjAndAddToMainList(typePass, finaPos, rootToSpawnIndex, index);
                    }
                    else i--;
                }
                isSpawned = true;
            }
        }
    }

    /// <summary>
    /// Call to replant a tree
    /// 
    /// 
    /// </summary>
    /// <param name="pos"></param>
    public void SpawnRandomTreeInThisPos(Vector3 pos, string oldTreeId)
    {
        int rootToSpawnIndex = ReturnRandomRootIndex(H.Tree);

        //so is saved and created
        IsToSave = true;
        CreateObjAndAddToMainList(H.Tree, pos, rootToSpawnIndex, 0, replantedTree: true, oldTreeID: oldTreeId);
        IsToSave = false;
    }

    //Creates the main type of objects and add them to AllRandomObjList, at the end if IsToSave is true will save it on
    //SaveOnListData
    public void CreateObjAndAddToMainList(H typePass, Vector3 pos, 
        int rootToSpawnIndex, int index, Quaternion rot = new Quaternion(), bool replantedTree = false,
        float treeHeight = 0, MDate seedDate = null, float maxHeight = 0, bool treeFall=false, float weight=0,
        string oldTreeID = "")
    {
        string root = ReturnRoot(typePass, rootToSpawnIndex);
        TerrainRamdonSpawner temp = null;
        if (IsToSave)
        {
            temp = TerrainRamdonSpawner.CreateTerraSpawn(root, pos, new Vector3(0, rand.Next(0, 360), 0),
                index, typePass, typePass.ToString(),
                transform, replantedTree, treeHeight, seedDate, maxHeight);
            usedVertexPos[index] = true;

            //Debug.Log("Obj Spwned:"+temp.MyId);
        }
        else if (IsToLoadFromFile)
        {
            temp = TerrainRamdonSpawner.CreateTerraSpawn(root, pos, new Vector3(), 
                index, typePass, typePass.ToString(),
                transform, replantedTree, treeHeight, seedDate, maxHeight, rot);

            if (typePass==H.Tree)
            {
                var st = (StillElement) temp;
                st.Weight = weight;
                st.TreeFall = treeFall;
            }
        }
        //AssignSharedMaterial(temp);
        //temp.AssignToAllGeometryAsSharedMat(temp.gameObject, "Enviroment");

        //if is replant tree we want to place it first so when loading is faster 
        if (replantedTree)
        {
            temp.MyId = oldTreeID;
            temp.name = oldTreeID;
            AllRandomObjList.Insert(0,temp);
        }
        else
        {
            Program.gameScene.BatchAdd(temp);
            AllRandomObjList.Add(temp);
        }
        
        if (IsToSave)
        {
            SaveOnListData(temp, typePass, rootToSpawnIndex, index, replantedTree);
        }
    }


    /// <summary>
    /// Will asign a shared material to the Control.CurrentSpawnBuild
    /// </summary>
    void AssignSharedMaterial(TerrainRamdonSpawner t)
    {
        Material n = Resources.Load<Material>(Root.RetMaterialRoot("Enviroment"));
        n.name = "Enviroment";

        t.Geometry.GetComponent<Renderer>().sharedMaterial = n;
    }
    

    

    //Save all the data into AllSpawnedDataList
    void SaveOnListData(General obj, H typeP, int rootToSpawnIndex, int indexPass, bool replantTree)
    {
        if (obj == null) { return;}
        if (obj is StillElement)
        {
            SpawnedData sData = new SpawnedData(obj.transform.position, obj.transform.rotation, typeP, rootToSpawnIndex, indexPass);
            AddToAllSpawnedDataOnSpecificIndex(sData, replantTree);
        }
        else
        {
            SpawnedData sData = new SpawnedData(obj.transform.position, obj.transform.rotation, typeP, rootToSpawnIndex,
                indexPass);
            AddToAllSpawnedDataOnSpecificIndex(sData, replantTree);
        }
    }

    void AddToAllSpawnedDataOnSpecificIndex(SpawnedData sData, bool replantTree)
    {
        if (replantTree)
        {
            AllSpawnedDataList.Insert(0,sData);
        }
        else
        {
            AllSpawnedDataList.Add(sData);
        }
    }



    /// <summary>
    /// Will return a random root of a specific type of obj
    /// </summary>
    /// <param name="typePass">Tpye of object</param>
    /// <returns>Random string root</returns>
    int ReturnRandomRootIndex(H typePass)
    {
        int index = 0;
        if (typePass == H.Tree) { index = rand.Next(0, allTrees.Count); }
        else if (typePass == H.Stone) { index = rand.Next(0, allStones.Count); }
        else if (typePass == H.Iron) { index = rand.Next(0, allIron.Count); }
        else if (typePass == H.Gold) { index = rand.Next(0, allGold.Count); }
        else if (typePass == H.Ornament) { index = rand.Next(0, allOrna.Count); }
        else if (typePass == H.Grass) { index = rand.Next(0, allGrass.Count); }
        return index;
    }

    /// <summary>
    /// Will return a random root of a specific type of obj
    /// </summary>
    /// <param name="typePass">Tpye of object</param>
    /// <returns>Random string root</returns>
    string ReturnRoot(H typePass, int index)
    {
        string rootToSpawn = "";
        if (typePass == H.Tree) { rootToSpawn = allTrees[index]; }
        else if (typePass == H.Stone) { rootToSpawn = allStones[index]; }
        else if (typePass == H.Iron) { rootToSpawn = allIron[index]; }
        else if (typePass == H.Gold) { rootToSpawn = allGold[index]; }
        else if (typePass == H.Ornament) { rootToSpawn = allOrna[index]; }
        else if (typePass == H.Grass) { rootToSpawn = allGrass[index]; }
        return rootToSpawn;
    }

    //thisis the area where not trees or rocks or anything else will be spawned
    //bz will be the starting poiint og the game 
    void DefineStartVoidArea(float x, float z)
    {
        voidNWCorner = new Vector3(centerOfTerrain.x - x, centerOfTerrain.y, centerOfTerrain.z + z);
        voidSECorner = new Vector3(centerOfTerrain.x + x, centerOfTerrain.y, centerOfTerrain.z - z);
    }

    //updates local list, AllRandomObjList, AllSpawnedDataList, and all individuals lists
    public void UpdateLocalLists(TerrainRamdonSpawner newObj)
    {
        if (AllRandomObjList.Count == 0) { return; }
        for (int i = 0; i < AllRandomObjList.Count; i++)
        {
            if(AllRandomObjList[i].IndexAllVertex == newObj.IndexAllVertex)
            {
                AllRandomObjList[i] = newObj;
            }
        }
        CreateOrUpdateSpecificsList(newObj.HType, newObj, H.Update);
    }

    //***************************************************

    /// <summary>
    /// The data is recreated here from  AllRandomObjList
    /// </summary>
    public void ReSaveData()
    {
        List<SpawnedData> tempList = new List<SpawnedData>();

        for (int i = 0; i < AllRandomObjList.Count; i++)
        {
            bool isMarkToMineLocal = false;
            if (AllRandomObjList[i] is StillElement)
            {
                print("ReSaveData() stillEle");
                StillElement still = (StillElement)AllRandomObjList[i];
            }

            //if is null was deelleted by user 
            if (AllRandomObjList[i] != null)
            {
                tempList.Add(new SpawnedData(
                AllRandomObjList[i].transform.position, AllRandomObjList[i].transform.rotation, 
                AllSpawnedDataList[i].Type, AllSpawnedDataList[i].RootStringIndex, 
                AllSpawnedDataList[i].AllVertexIndex,
                AllSpawnedDataList[i].TreeHeight, AllSpawnedDataList[i].SeedDate, AllSpawnedDataList[i].MaxHeight,
                AllSpawnedDataList[i].TreeFall, AllSpawnedDataList[i].Weight
                ));
            }
        }

        AllSpawnedDataList.Clear();
        AllSpawnedDataList = tempList;
        SaveData();
    }

    //create or updates specific lists 
    void CreateOrUpdateSpecificsList(H typePass, TerrainRamdonSpawner temp, H action)
    {
        //print(typePass + " " + action);

        if (typePass == H.Tree)
        {
            if (action == H.Create) { treeList.Add((TreeVeget)temp); }
            else if (action == H.Update){treeList = UList.UpdateAList(treeList, temp);}
        }
        else if (typePass == H.Stone)
        {
            if (action == H.Create) { stoneList.Add(temp as StoneRock); }
            else if (action == H.Update) { stoneList = UList.UpdateAList(stoneList, temp); }
        }
        else if (typePass == H.Iron)
        {
            if (action == H.Create) { ironList.Add(temp as IronRock); }
            else if (action == H.Update) { ironList = UList.UpdateAList(ironList, temp); }
        }
        else if (typePass == H.Ornament)
        {
            if (action == H.Create) { ornaList.Add((StillElement)temp); }
            else if (action == H.Update) { ornaList = UList.UpdateAList(ornaList, temp); }
        }
        else if (typePass == H.Grass)
        {
            if (action == H.Create) { grassList.Add((StillElement)temp); }
            else if (action == H.Update) { grassList = UList.UpdateAList(grassList, temp); }
        }
    }

    public void SaveData()
    {
       //Debug.Log("Called SaveData");

        spawnedData = new SpawnedData();
        spawnedData.AllSpawnedObj = AllSpawnedDataList;
        spawnedData.TerraMshCntrlAllVertexIndexCount = p.MeshController.AllVertexs.Count;
        XMLSerie.WriteXMLSpawned(spawnedData);
    }

    public void LoadData()
    {
        try { spawnedData = XMLSerie.ReadXMLSpawned(); }
        catch (Exception exception)
        { print("error loading XMLSerie.ReadXMLSpawned()." + exception.GetBaseException().Message); }

        //print(spawnedData.TerraMshCntrlAllVertexIndexCount + "spawnedData.TerraMshCntrlAllVertexIndexCount");
        //print(Terreno.MeshController.AllVertexs.Count + "Terreno.MeshController.AllVertexs.Count");

        if (spawnedData == null) { return; }
        if (spawnedData.TerraMshCntrlAllVertexIndexCount != p.MeshController.AllVertexs.Count)
        {
            print("subMesh loaded not the same as the one was the spawned obj created with");
            IsToSave = true;
            ClearCurrentFileAndList();
            return;
        }

        AllSpawnedDataList = spawnedData.AllSpawnedObj;
        p.TerraSpawnController.IsToLoadFromFile = true;
    }

    public void LoadFromFile()
    {
        CreateObjAndAddToMainList(AllSpawnedDataList[loadingIndex].Type,
            AllSpawnedDataList[loadingIndex].Pos,
            AllSpawnedDataList[loadingIndex].RootStringIndex, AllSpawnedDataList[loadingIndex].AllVertexIndex,
            AllSpawnedDataList[loadingIndex].Rot, 
            
            false, 
            AllSpawnedDataList[loadingIndex].TreeHeight, AllSpawnedDataList[loadingIndex].SeedDate, 
            AllSpawnedDataList[loadingIndex].MaxHeight,
            AllSpawnedDataList[loadingIndex].TreeFall, AllSpawnedDataList[loadingIndex].Weight);

        //will restart the value of this array so I know which ones are being used
        usedVertexPos = new bool[spawnedData.TerraMshCntrlAllVertexIndexCount];
        usedVertexPos[AllSpawnedDataList[loadingIndex].AllVertexIndex] = true;

        loadingIndex++;


        //when index is the same as couunt that it
        if (loadingIndex >= AllSpawnedDataList.Count)
        {
            IsToLoadFromFile = false;
            //CreateOrUpdateSpecificsList(AllSpawnedDataList[loaded)
            print(treeList.Count + " treeList.Count IsToLoadFromFile-false");
            
            Program.gameScene.BatchInitial();

            //CreateTreePool();
        }
    }

    public string PercentageLoaded()
    {
        if (AllSpawnedDataList.Count==0)
        {
            return "Wait Loading List on TerrainController";
        }

        return ((loadingIndex/AllSpawnedDataList.Count)*100).ToString("n1");
    }

    void ClearCurrentFileAndList()
    {
        AllSpawnedDataList.Clear();
        SaveData();
    }













    private static int secCount;
    private static Vector3 originalPoint;
    /// <summary>
    /// Returns Random position from origin. If fell inside a building will find another spot
    /// until is in a clear zone
    /// 
    /// If origin is not specified will use MeshController.AllVertexs.Count
    /// 
    /// Will throw new Exception("Cant be use if MeshController.AllVertexs.Count == 0");
    /// </summary>
    /// <param name="howFar">How far will go</param>
    public static Vector3 AssignRandomIniPosition(Vector3 origin = new Vector3(), float howFar = 1)
    {
        if (origin == new Vector3())
        {
            origin = ReturnIniPos();
        }

        //so origin is not changed in every recursive
        if (originalPoint == new Vector3())
        {
            originalPoint = origin;
            origin = ReturnRandomPos(origin, howFar);
        }
        else
        {
            origin = ReturnRandomPos(originalPoint, howFar);
        }
        //will add one unit to how far so can move further
        howFar += 0.1f;

        //to check if the poly ard it is free of obstacles 
        var polyToCheck = UPoly.CreatePolyFromVector3(origin, 1f, 1f);

        if (MeshController.CrystalManager1.IntersectAnyLine(polyToCheck, origin) || !IsOnTerrain(origin))
        {
            secCount++;
            if (secCount > 1000)
            {
                throw new Exception("Infinite loop terraSpawContrl");
            }
            origin = AssignRandomIniPosition(origin, howFar);
        }

        originalPoint = new Vector3();
        secCount = 0;
        return origin;
    }

    

    private static Vector3 ReturnIniPos()
    {
        SPr pp = new SPr();

        if (pp.MeshController.AllVertexs.Count == 0)
        {
            throw new Exception("Cant be use if MeshController.AllVertexs.Count == 0");
        }

        return pp.MeshController.AllVertexs[UMath.GiveRandom(0, pp.MeshController.AllVertexs.Count)];
    }

    private static Vector3 ReturnRandomPos(Vector3 origin, float howFar)
    {
        float x = UMath.Random(-howFar, howFar);
        float z = UMath.Random(-howFar, howFar);
        origin = new Vector3(origin.x + x, origin.y, origin.z + z);
        return origin;
    }

    /// <summary>
    /// Will say if origin is on terrain 
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public static bool IsOnTerrain(Vector3 origin)
    {
        SMe mm = new SMe();
        var hit = mm.SubDivide.FindYValueOnTerrain(origin.x, origin.z);
        var terrCenter = mm.IniTerr.MathCenter;

        if (Mathf.Abs(terrCenter.y - hit) < 0.1f)
        {
            return true;
        }

        return false;
    }


    public bool HasLoadedOrLoadedTreesAndRocks()
    {
        return !IsToLoadFromFile || IsOrnamenting();//means is spwaning ornaments 
    }


    private bool isOrnamentingNow;
    /// <summary>
    /// Will tell u if while loading is ornamienting (spawning ornaments or grass )
    /// </summary>
    /// <returns></returns>
    bool IsOrnamenting()
    {
        if (isOrnamentingNow)
        {
            return true;
        }

        isOrnamentingNow = AllSpawnedDataList[loadingIndex].Type == H.Ornament
                           || AllSpawnedDataList[loadingIndex].Type == H.Grass;

        return isOrnamentingNow;
    }
}