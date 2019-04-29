﻿using System;
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
    private int multiplier = 40;//80  

    int howManyTreesToSpawn = 75;//30  20    50
    int howManyStonesToSpawn = 1;//3
    int howManyIronToSpawn = 1;//3
    int howManyGoldToSpawn = 1;//3
    int howManyOrnaToSpawn = 1;//30  
    int howManyGrassToSpawn = 5;//20  40
    //the ones spawn in the marine bounds 
    int howManyMarineBoundsToSpawn = 1;//1
    int howManyMountainBoundsToSpawn = 0;//

    private SpawnPool _spawnPool;

    //will be use when spawing new obj to know if that position was used alread by another one
    bool[] usedVertexPos;

    private List<string> allTrees = new List<string>()
    {
        //Root.palm1, Root.palm2,
        //Root.palm3, Root.palm4, Root.palm5, Root.palm6, 
        //Root.palm10  ,Root.palm11  ,
        Root.palm20, Root.palm21, 
        Root.palm22, Root.palm23,

    };

    List<string> allStones = new List<string>() { };

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

    public static List<string> allOrna = new List<string>() { };
    public static List<string> allGrass = new List<string>() { };
    public static List<string> allMarine = new List<string>() { };
    public static List<string> allMountain = new List<string>() { };

    List<H> toSpawnList = new List<H>()
    {
        H.Tree, H.Stone, H.Iron, H.Gold, H.Ornament, H.Grass, H.Marine, H.Mountain
        
    };
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
    List<StillElement> marineList = new List<StillElement>();
    List<StillElement> mountainList = new List<StillElement>();
    private bool _isToLoadFromFile;

    public bool IsToLoadFromFile
    {
        get { return _isToLoadFromFile; }
        set { _isToLoadFromFile = value; }
    }

    public void RemoveStillElement(StillElement ele, Vector3 savedPos)
    {
        var index = AllRandomObjList.IndexOf(ele);
        AllRandomObjList.RemoveAt(index);

        //SpawnedData sData = new SpawnedData(ele.transform.position, ele.transform.rotation, ele.HType, 
        //    ele.RootToSpawnIndex, ele.IndexAllVertex);

        var sData = AllSpawnedDataList.Where(a => a.Pos == savedPos).FirstOrDefault();

        //a not valid Spawner calling this 
        if (sData == null)
        {
            return;
        }
        index = AllSpawnedDataList.IndexOf(sData);
        AllSpawnedDataList.RemoveAt(index);
    }

    public void ReSaveStillElement(StillElement ele)
    {
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
    }

    public bool IsToSave;

    private int loadingAllowTimes = 1;//how many times system is allow to load 
    private int loadedTimes = 0;//loaded times



    internal void SendAllToPool()
    {
        for (int i = 0; i < AllRandomObjList.Count; i++)
        {
            MeshController.CrystalManager1.Delete((StillElement)AllRandomObjList[i]);
            Program.gameScene.BatchRemoveNotRedo((StillElement)AllRandomObjList[i]);

            var still =(StillElement)AllRandomObjList[i];
            still.DestroyCool();
        }
    } 
    
    internal void SendToPool(StillElement stillElement)
    {
        _spawnPool.AddToPool(stillElement);
    }

    void LeaveEditorPool(SpawnPool[] pools)
    {
        for (int i = 0; i < pools.Length; i++)
        {
            if (pools[i].name.Contains("Editor"))
            {
                _spawnPool = pools[i];
            }
            else
            {
                pools[i].Destroy();
            }
        }
    }    
    
    void LeaveStandPool(SpawnPool[] pools)
    {
        //in editor but bz Unity bugg was in here 
        if (_spawnPool!=null)
        {
            return;
        }

        for (int i = 0; i < pools.Length; i++)
        {
            if (!pools[i].name.Contains("Editor"))
            {
                _spawnPool = pools[i];
            }
            else
            {
                pools[i].Destroy();
            }
        }
    }

    // Use this for initialization
    void ManualStart()
    {
        var pools = GameObject.FindObjectsOfType<SpawnPool>();



#if UNITY_EDITOR
        multiplier = 2;//2
        howManyGrassToSpawn = 2;//40

        LeaveEditorPool(pools);
#endif
#if UNITY_STANDALONE 
        LeaveStandPool(pools);


#endif



        AddTreesToTreesRoots();
        DefineAllOrnaRoots();
        DefineAllStoneRoots();
        DefineAllLawnRoots();
        DefineMarinesToRoots();
        DefineMountainsToRoots();

        DefineStartVoidArea(1f, 1f);//20,20
        howManySpawn = new List<int>() {
            Multiplier(howManyTreesToSpawn), Multiplier(howManyStonesToSpawn), 
            Multiplier(howManyIronToSpawn), Multiplier(howManyGoldToSpawn),
            Multiplier(howManyOrnaToSpawn), Multiplier(howManyGrassToSpawn),
            //Marines
            Multiplier(howManyMarineBoundsToSpawn) / 2,
            Multiplier(howManyMountainBoundsToSpawn),

        };

        float minHeightAboveSeaLevel = 1.4f;//1.2
        minHeightToSpawn = Program.gameScene.WaterBody.transform.position.y + minHeightAboveSeaLevel;
        maxHeightToSpawn = minHeightToSpawn + 2.4f;//6.9

    }

    private void DefineAllLawnRoots()
    {
        var howManyLawnInEachFolder = 1 + 1;
        var add = "";
        for (int i = 1; i < howManyLawnInEachFolder; i++)
        {
            if (i < 10)
            {
                add = "0";
            }
            else add = "";

            allGrass.Add("Prefab/Terrain/Spawner/LawnGreenDark/Lawn" + add + i);
        }
    }

    /// <summary>
    /// Anything u chahnge here might need to be changed on Decoration.cs
    /// 
    /// bz some ornaments are not suitable to be beside a building 
    /// </summary>
    private void DefineAllOrnaRoots()
    {
        for (int i = 1; i < 9 + 1; i++)
        {
            allOrna.Add("Prefab/Terrain/Spawner/Orna/Orna" + i);
        }
    }

    private void DefineAllStoneRoots()
    {
        for (int i = 1; i < 4 + 1; i++)
        {
            allStones.Add("Prefab/Terrain/Spawner/Stone/Stone" + i);
        }
    }

    private void AddTreesToTreesRoots()
    {
        for (int i = 1; i < 4 + 1; i++)
        {
            allTrees.Add("Prefab/Terrain/Spawner/Tree" + i);
        }
    }

    private void DefineMarinesToRoots()
    {
        for (int i = 1; i < 4 + 1; i++)
        {
            allMarine.Add("Prefab/Terrain/Spawner/Marine/Marine" + i);
        }
    }

    private void DefineMountainsToRoots()
    {
        for (int i = 1; i < 3 + 1; i++)
        {
            allMountain.Add("Prefab/Terrain/Spawner/Mountain/Mountain" + i);
        }
    }


    /// <summary>
    /// So I dont have to change from 10 to 10,000 everytime a need to spawn more or less lawn for ex
    /// </summary>
    /// <returns></returns>
    int Multiplier(int mul)
    {
        return mul * multiplier;
    }



    int frameCount;
    //everyhow many frames will spawn an StilElement 
    int everyFrames = 1;

    int loopSize = 100;
    private bool wasStarted;
    // Update is called once per frame
    void Update()
    {
        if (MeshController.CrystalManager1 == null || MeshController.CrystalManager1.CrystalRegions.Count == 0
            || 
            (!TownLoader.TownLoaded && !TownLoader.IsTemplate
            && BuildingPot.Control.Registro.AllBuilding.Count == 0)
            )
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
            print(IsToSave + " isToSave = true, we are generating all spanwened obj now ");
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
            { IsToSave = true; }
            loadedTimes++;
        }

        if (!p.MeshController.IsLoading && IsToLoadFromFile && frameCount > everyFrames)
        {
            frameCount = 0;
            OrganizeSavedData();

            for (int i = 0; i < loopSize; i++)//100
            {
                if (IsToLoadFromFile)
                {
                    LoadFromFile();
                }
                else
                {
                    break;
                }
            }
        }
        frameCount++;

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

                    bool isHasMinHeight = AllVertexs[index].y > minHeightToSpawn;
                    bool isLowerThanMaxHeight = AllVertexs[index].y < maxHeightToSpawn;

                    if (isHasMinHeight && isLowerThanMaxHeight &&
                        !usedVertexPos[index] && !isOnTheStartZone && !regionContainTerraCry)
                    {
                        Vector3 finaPos = ReturnIniPosOfSpawn(typePass, AllVertexs[index], 0);
                        CreateObjAndAddToMainList(typePass, finaPos, rootToSpawnIndex, index);
                    }
                    else
                    {
                        //todo fix
                        //showing rejected positions 
                        //UVisHelp.CreateText(AllVertexs[index], index + "");
                        //Debug.Log("terra: " + index + "." + isOnTheStartZone + "." + regionContainTerraCry + "." +
                        //    isHasMinHeight + "." + isLowerThanMaxHeight + "." + !usedVertexPos[index]);
                        i--;
                    }
                }
                isSpawned = true;
            }
        }
    }

    /// <summary>
    /// Will valorate if is a Regular type or a marine or mountain type.
    /// If is marine will only look into the positions of marine bounds 
    /// </summary>
    /// <param name="typeP"></param>
    /// <param name="iniPos"></param>
    /// <param name="howFar"></param>
    /// <returns></returns>
    Vector3 ReturnIniPosOfSpawn(H typeP, Vector3 iniPos, float howFar)
    {
        //if is a marine will 
        if (typeP == H.Marine && m.SubMesh.MarineBounds.Count > 0)
        {
            return m.SubMesh.MarineBounds[rand.Next(0, m.SubMesh.MarineBounds.Count)];
        }
        if (typeP == H.Mountain && m.SubMesh.MountainBounds.Count > 0)
        {
            var v3 = m.SubMesh.MountainBounds[rand.Next(0, m.SubMesh.MountainBounds.Count)];

            return new Vector3(v3.x, m.IniTerr.MathCenter.y, v3.z);
        }
        return AssignRandomIniPosition(iniPos, howFar);
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
        float treeHeight = 0, MDate seedDate = null, float maxHeight = 0, bool treeFall = false, float weight = 0,
        string oldTreeID = "")
    {
        var region = MeshController.CrystalManager1.ReturnMyRegion(pos);

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
            temp = _spawnPool.RetTerraSpawn(pos, new Vector3(),
                index, typePass, typePass.ToString(),
                transform, replantedTree, treeHeight, seedDate, maxHeight, rot);

            //pool emptied
            if (temp == null)
            {
                temp = TerrainRamdonSpawner.CreateTerraSpawn(root, pos, new Vector3(),
                    index, typePass, typePass.ToString(),
                    transform, replantedTree, treeHeight, seedDate, maxHeight, rot);
            }

            if (typePass == H.Tree)
            {
                var st = (StillElement)temp;
                st.Weight = weight;
                st.TreeFall = treeFall;
            }
        }
        temp.Region = region;

        //if is replant tree we want to place it first so when loading is faster 
        if (replantedTree)
        {
            temp.MyId = oldTreeID;
            temp.name = oldTreeID;
            AllRandomObjList.Insert(0, temp);
        }
        else
        {
            if (typePass != H.Grass)
            {
                Program.gameScene.BatchAdd(temp);
            }
            AllRandomObjList.Add(temp);
        }

        if (IsToSave)
        {
            SaveOnListData(temp, typePass, rootToSpawnIndex, index, replantedTree, region);
        }
        else
        {
            var a = 1;
        }

        StillElement still = (StillElement) temp;
        if (still!=null)
        {
            still.Start();
        }
    }

    //Save all the data into AllSpawnedDataList
    void SaveOnListData(General obj, H typeP, int rootToSpawnIndex, int indexPass, bool replantTree, int region)
    {
        if (obj == null) { return; }
        if (obj is StillElement)
        {
            SpawnedData sData = new SpawnedData(obj.transform.position, obj.transform.rotation, typeP, 
                rootToSpawnIndex, indexPass, region: region);
            AddToAllSpawnedDataOnSpecificIndex(sData, replantTree);
        }
        else
        {
            SpawnedData sData = new SpawnedData(obj.transform.position, obj.transform.rotation, typeP, rootToSpawnIndex,
                indexPass, region: region);
            AddToAllSpawnedDataOnSpecificIndex(sData, replantTree);
        }
    }

    void AddToAllSpawnedDataOnSpecificIndex(SpawnedData sData, bool replantTree)
    {
        if (replantTree)
        {
            AllSpawnedDataList.Insert(0, sData);
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
        else if (typePass == H.Marine) { index = rand.Next(0, allMarine.Count); }
        else if (typePass == H.Mountain) { index = rand.Next(0, allMountain.Count); }
        return index;
    }

    /// <summary>
    /// Will return a random root of a specific type of obj
    /// </summary>
    /// <param name="typePass">Tpye of object</param>
    /// <returns>Random string root</returns>
    string ReturnRoot(H typePass, int indexP)
    {
        //Coomment out if not need pls
        indexP = CorrectRootIfAmountOfObjectsChanged(typePass, indexP);

        string rootToSpawn = "";
        if (typePass == H.Tree) { rootToSpawn = allTrees[indexP]; }
        else if (typePass == H.Stone) { rootToSpawn = allStones[indexP]; }
        else if (typePass == H.Iron) { rootToSpawn = allIron[indexP]; }
        else if (typePass == H.Gold) { rootToSpawn = allGold[indexP]; }
        else if (typePass == H.Ornament) { rootToSpawn = allOrna[indexP]; }
        else if (typePass == H.Grass) { rootToSpawn = allGrass[indexP]; }
        else if (typePass == H.Marine) { rootToSpawn = allMarine[indexP]; }
        else if (typePass == H.Mountain) { rootToSpawn = allMountain[indexP]; }
        return rootToSpawn;
    }

    /// <summary>
    /// When changed the amout of Trees or Orna then a Save file will brake 
    /// bz they save which one was there. Now with this method I will reassign 
    /// a new int value if the numner pass is too big
    /// 
    /// Addressing now only Tree and Ornament changed
    /// 
    /// Coomment out if not need pls
    /// </summary>
    /// <param name="typePass"></param>
    /// <param name="indexP"></param>
    /// <returns></returns>
    int CorrectRootIfAmountOfObjectsChanged(H typePass, int indexP)
    {
        if (typePass == H.Tree)
        {
            indexP = UMath.GiveRandom(0, allTrees.Count);
        }
        else if (typePass == H.Ornament) 
        {
            indexP = UMath.GiveRandom(0, allOrna.Count);
        }
        else if (typePass == H.Grass )
        {
            indexP = UMath.GiveRandom(0, allGrass.Count);
        }
        return indexP;
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
            if (AllRandomObjList[i].IndexAllVertex == newObj.IndexAllVertex)
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
            //if is null was deelleted by user 
            if (AllRandomObjList[i] != null)//bz bugg there is more Rand than Data
            {
                var ele = AllRandomObjList[i];

                var sData = CreateApropData(ele);

                tempList.Add(sData);
            }
        }

        AllSpawnedDataList.Clear();
        AllSpawnedDataList = tempList;
        SaveData();
    }

    SpawnedData CreateApropData(TerrainRamdonSpawner ele)
    {
        SpawnedData sData = null;

        if (ele.HType == H.Tree)
        {
            var still = (StillElement)ele;

            sData = new SpawnedData(ele.transform.position, ele.transform.rotation, ele.HType,
            ele.RootToSpawnIndex, ele.IndexAllVertex, still.Height, still.SeedDate, still.MaxHeight,
            still.TreeFall, still.Weight, ele.Region);
        }
        else
        {
            sData = new SpawnedData(ele.transform.position, ele.transform.rotation, ele.HType,
            ele.RootToSpawnIndex, ele.IndexAllVertex, ele.Region);
        }

        return sData;
    }

    //create or updates specific lists 
    void CreateOrUpdateSpecificsList(H typePass, TerrainRamdonSpawner temp, H action)
    {
        //print(typePass + " " + action);

        if (typePass == H.Tree)
        {
            if (action == H.Create) { treeList.Add((TreeVeget)temp); }
            else if (action == H.Update) { treeList = UList.UpdateAList(treeList, temp); }
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
        else if (typePass == H.Marine)
        {
            if (action == H.Create) { marineList.Add((StillElement)temp); }
            else if (action == H.Update) { marineList = UList.UpdateAList(marineList, temp); }
        }
        else if (typePass == H.Mountain)
        {
            if (action == H.Create) { mountainList.Add((StillElement)temp); }
            else if (action == H.Update) { mountainList = UList.UpdateAList(mountainList, temp); }
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

    /// <summary>
    /// How To create new Spawned Data for terrain
    ///  
    /// make  :
    ///spawnedData = XMLSerie.ReadXMLSpawned(true)
    ///spawnedData = XMLSerie.ReadXMLSpawned() 
    /// 
    /// </summary>
    public void LoadData()
    {
        try
        {
            if (!Program.gameScene.IsDefaultTerreno())
            {
                spawnedData = XMLSerie.ReadXMLSpawned();
                
                if (spawnedData == null)
                {
                    Debug.Log("spawnedData == null big");
                }
            }
            else//the first teraain to load 
            {
                spawnedData = XMLSerie.ReadXMLSpawned(true);//true once Terrain.Spawned is created  

                if (spawnedData == null)
                {
                    Debug.Log("spawnedData == null DefaultLoad");
                }
            }
        }
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

    #region Regions 

    bool wasDataOrganized;
    List<RegionD> _closest9 = new List<RegionD>();
    List<RegionD> _rest = new List<RegionD>();

    bool _releaseLoadingScreen;

    public bool ReleaseLoadingScreen
    {
        get { return _releaseLoadingScreen; }
        set { _releaseLoadingScreen = value; }
    }

    /// <summary>
    /// Based on the Initial town the AllSpawnedDataList will be organize so it loads from the
    /// initial position up to the farthest
    /// </summary>
    private void OrganizeSavedData()
    {
        if (wasDataOrganized)
        {
            return;
        }
        wasDataOrganized = true;
        HandleRegions();
        
        //organize data by regions, split in 2 lists, 9 regions and the rest 
        //AllSpawnedDataList = AllSpawnedDataList.OrderBy(a => a.RegionDistanceToInit()).ToList();
    }

    /// <summary>
    /// Will orginze regions by distance to initial region 
    /// 
    /// also defines the closest9 regions to init 
    /// </summary>
    void HandleRegions()
    {
        //find regions distances
        for (int i = 0; i < MeshController.CrystalManager1.CrystalRegions.Count; i++)
        {
            _rest.Add(new RegionD(MeshController.CrystalManager1.CrystalRegions[i].Index,
                MeshController.CrystalManager1.CrystalRegions[i].Position()));
        }
        //organize regions by distances
        _rest = _rest.OrderBy(a => a.DistanceToInit).ToList();

        //get closer 9 regions 3x3
        for (int i = 0; i < 9; i++)
        {
            _closest9.Add(_rest[i]);
        }

        //remove from rest of regions
        for (int i = 0; i < 9; i++)
        {
            //_rest.RemoveAt(0);
        }
    }

    int lastRegion = -1;
    List<int> regionsLoaded = new List<int>();
    /// <summary>
    /// This will release loading screeen when the x region was loaded and will
    /// keep loading the rest 
    /// </summary>
    void HandleLoadingRegions()
    {
        if (lastRegion != AllSpawnedDataList[loadingIndex].Region)
        {
            if (lastRegion != -1 && !regionsLoaded.Contains(lastRegion))
            {
                regionsLoaded.Add(lastRegion);
                //destroy loading sign on Region GO

            }
            lastRegion = AllSpawnedDataList[loadingIndex].Region;
        }

        BuildingPot.Control.Registro.MarkTerraIfNeeded(AllSpawnedDataList[loadingIndex], _closest9);

        if (regionsLoaded.Count>55)//55
        {
            //release loading screen
            //and then is loading slowly the other elements 
            ReleaseLoadingScreen = true;
            loopSize = 1;
            everyFrames = 10;
        }

        if (regionsLoaded.Count == _rest.Count)
        {
            //done loading 
        }
    }

#endregion

    void LoadFromFile()
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
            print(treeList.Count + " treeList.Count IsToLoadFromFile-false");
            Program.gameScene.BatchInitial();

            ReleaseLoadingScreen = true;
        }
    }

    private int ttlToSpawn = 0;
    public string PercentageLoaded()
    {
        if (AllSpawnedDataList.Count == 0)
        {
            return "Wait Loading List on TerrainController";
        }

        if (ttlToSpawn == 0)
        {
            for (int i = 0; i < howManySpawn.Count; i++)
            {
                ttlToSpawn += howManySpawn[i];
            }
        }

        var perc = ((float)loadingIndex / (float)ttlToSpawn) * 100;

        return (perc - 1).ToString("n0") + " %";
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
    Vector3 AssignRandomIniPosition(Vector3 origin = new Vector3(), float howFar = 1)
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
        //doesnt matter that is positive bz in ReturnRandomPos goes in the range of the same number negative and positive
        howFar += .1f;//.1f

        //to check if the poly ard it is free of obstacles 
        var polyToCheck = UPoly.CreatePolyFromVector3(origin, 1f, 1f);//1, 1

        if (MeshController.CrystalManager1.IntersectAnyLine(polyToCheck, origin) || !IsOnTerrain(origin)
            || !ComplyWithTerraRules(origin))
        {
            secCount++;
            if (secCount > 1000)
            {
                throw new Exception("Infinite loop terraSpawContrl");
            }
            //UVisHelp.CreateText(origin, "R" + "");
            origin = AssignRandomIniPosition(origin, howFar);
        }

        originalPoint = new Vector3();
        secCount = 0;
        return origin;
    }

    bool ComplyWithTerraRules(Vector3 toEval)
    {
        bool isOnTheStartZone = UMesh.Contains(toEval, voidNWCorner, voidSECorner);
        bool regionContainTerraCry =
            MeshController.CrystalManager1.DoesMyRegionHasTerraCrystal(toEval);

        bool isHasMinHeight = toEval.y > minHeightToSpawn;
        bool isLowerThanMaxHeight = toEval.y < maxHeightToSpawn;

        return !isOnTheStartZone && !regionContainTerraCry && isHasMinHeight && isLowerThanMaxHeight;
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

/// <summary>
/// This is to contain the int id and distance from the initial region .
/// D for dummy
/// </summary>
class RegionD
{
    int _region;

    public int Region
    {
        get { return _region; }
        set { _region = value; }
    }
    float _distanceToInit;

    public float DistanceToInit
    {
        get { return _distanceToInit; }
        set { _distanceToInit = value; }
    }

    Vector3 _pos;

    public Vector3 Pos
    {
        get { return _pos; }
        set { _pos = value; }
    }

    public RegionD(int region, Vector3 pos)
    {
        _region = region;
        _pos = pos;

        var cryRegion = MeshController.CrystalManager1.CrystalRegions[TownLoader.InitRegion];
        _distanceToInit = Vector3.Distance(_pos, cryRegion.Position());
    }
}