using System;
using UnityEngine;
using System.Collections.Generic;

public class TerrainSpawnerController : ControllerParent
{
    int loadingIndex;
    private SpawnedData spawnedData;

    bool isSpawned;//tells if a set of obj were spawned
    System.Random rand = new System.Random();
    float minHeightToSpawn;//min height to spawn obj on terrain
    private float maxHeightToSpawn;

    private int multiplier = 75;//75

    int howManyTreesToSpawn = 20;//50
    int howManyStonesToSpawn =3;
    int howManyIronToSpawn = 3;
    int howManyGoldToSpawn = 3;
    int howManyOrnaToSpawn = 20;//50
    int howManyGrassToSpawn = 10;

    //will be use when spawing new obj to know if that position was used alread by another one
    bool[] usedVertexPos;

    private List<string> allTrees = new List<string>()
    {
        Root.tree1,// Root.tree2, Root.tree3, 
        Root.tree4, Root.tree5, Root.tree6, 
        Root.tree7,

        Root.palm1, Root.palm2, Root.palm3, Root.palm4, Root.palm5, Root.palm6, //Root.palm10  
    };

    List<string> allStones = new List<string>()
    {
        Root.stone0, Root.stone1, Root.stone2, Root.stone3,
        Root.stone4, Root.stone5, Root.stone6, Root.stone7,
    };

    List<string> allIron = new List<string>()
    {
        Root.iron1, Root.iron2, Root.iron3 ,Root.iron4,Root.iron5
    
    };
    //gold stones here 
    List<string> allGold = new List<string>()
    {
        Root.gold0, Root.gold1, Root.gold2, Root.gold3,
        Root.gold4
    
    };

    List<string> allOrna = new List<string>()
    { 
        Root.orna1, Root.orna2, Root.orna3, Root.orna4 , Root.orna5, Root.orna6
    };

    List<string> allGrass = new List<string>() 
    { 
        //Root.grass1, 
        Root.grass2, Root.grass3  ,
        Root.grass4, Root.grass5, Root.grass6 
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

    public bool IsToSave;
    public bool IsToLoadFromFile;

    private int loadingAllowTimes = 1;//how many times system is allow to load 
    private int loadedTimes = 0;//loaded times

    // Use this for initialization
    void ManualStart()
    {
        DefineStartVoidArea(20f, 20f);
        howManySpawn = new List<int>() {
            Multiplier(howManyTreesToSpawn), Multiplier(howManyStonesToSpawn), 
            Multiplier(howManyIronToSpawn), Multiplier(howManyGoldToSpawn),
            Multiplier(howManyOrnaToSpawn), Multiplier(howManyGrassToSpawn)
        };

        float minHeightAboveSeaLevel = 1.2f;//1
        minHeightToSpawn = Program.gameScene.WaterBody.transform.position.y + minHeightAboveSeaLevel;
        maxHeightToSpawn = minHeightToSpawn + 6.9f;
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
        }

        if (!p.MeshController.IsLoading && loadedTimes < loadingAllowTimes)
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
                        CreateObjAndAddToMainList(typePass, AllVertexs[index], rootToSpawnIndex, index);
                    }
                    else i--;
                }
                isSpawned = true;
            }
        }
    }

    //Creates the main type of objects and add them to AllRandomObjList, at the end if IsToSave is true will save it on
    //SaveOnListData
    public void CreateObjAndAddToMainList(H typePass, Vector3 pos, 
        int rootToSpawnIndex, int index, Quaternion rot = new Quaternion(), bool isMarkToMine = false)
    {
        string root = ReturnRoot(typePass, rootToSpawnIndex);
        TerrainRamdonSpawner temp = null;
        if (IsToSave)
        {
            temp = TerrainRamdonSpawner.CreateTerraSpawn(root, pos, index, typePass, typePass.ToString() + ":" + index,
                transform);
            temp.transform.Rotate(new Vector3(0, rand.Next(0, 360), 0));
            usedVertexPos[index] = true;
        }
        else if (IsToLoadFromFile)
        {
            temp = TerrainRamdonSpawner.CreateTerraSpawn(root, pos, index, typePass, typePass.ToString() + ":" + index,
                transform);
            temp.transform.rotation = rot;
            if (typePass == H.Tree || typePass == H.Stone || typePass == H.Iron)
            {
                StillElement still = (StillElement)temp;
                if (isMarkToMine)
                {
                    temp.IsMarkToMine = true;
                    InputMain.InputMeshSpawnObj.AddVisHelpList(true, temp);
                }
            }
        }

        //AssignSharedMaterial(temp);

        AllRandomObjList.Add(temp);
        if(IsToSave){SaveOnListData(temp, typePass, rootToSpawnIndex, index);}
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
    void SaveOnListData(General obj, H typeP, int rootToSpawnIndex, int indexPass)
    {
        if (obj == null) { return;}
        if (obj is StillElement)
        {
            StillElement temp = (StillElement) obj;
            SpawnedData sData = new SpawnedData(obj.transform.position, obj.transform.rotation, typeP, temp.IsMarkToMine, rootToSpawnIndex, indexPass);
            AllSpawnedDataList.Add(sData);
        }
        else
        {
            SpawnedData sData = new SpawnedData(obj.transform.position, obj.transform.rotation, typeP, false, rootToSpawnIndex,
                indexPass);
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
                AllSpawnedDataList[i].IsMarkToMine = newObj.IsMarkToMine;
            }
        }
        CreateOrUpdateSpecificsList(newObj.HType, newObj, H.Update);
    }

    //***************************************************

    //resave data into file
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
                isMarkToMineLocal = still.IsMarkToMine;
            }

            tempList.Add(new SpawnedData(
                AllRandomObjList[i].transform.position, AllRandomObjList[i].transform.rotation, AllSpawnedDataList[i].Type,
                isMarkToMineLocal, AllSpawnedDataList[i].RootStringIndex, AllSpawnedDataList[i].AllVertexIndex));
        }

        ClearCurrentFileAndList();

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
        p.TerraSpawnController.CreateObjAndAddToMainList(AllSpawnedDataList[loadingIndex].Type,
            AllSpawnedDataList[loadingIndex].Pos,
            AllSpawnedDataList[loadingIndex].RootStringIndex, AllSpawnedDataList[loadingIndex].AllVertexIndex,
            AllSpawnedDataList[loadingIndex].Rot, AllSpawnedDataList[loadingIndex].IsMarkToMine);

        //will restart the value of this array so I know which ones are being used
        usedVertexPos = new bool[spawnedData.TerraMshCntrlAllVertexIndexCount];
        usedVertexPos[AllSpawnedDataList[loadingIndex].AllVertexIndex] = true;

        loadingIndex++;

        //when index is the same as couunt that it
        if (loadingIndex == AllSpawnedDataList.Count)
        {
            IsToLoadFromFile = false;
            //CreateOrUpdateSpecificsList(AllSpawnedDataList[loaded)
            //print(treeList.Count + " treeList.Count");
        }
    }

    void ClearCurrentFileAndList()
    {
        AllSpawnedDataList.Clear();
        SaveData();
    }
}