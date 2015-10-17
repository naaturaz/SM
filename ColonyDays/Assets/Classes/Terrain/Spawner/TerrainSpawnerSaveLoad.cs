using System.Collections.Generic;
using System;

public class TerrainSpawnerSaveLoad : General {

    private int loadingIndex;

    public void ReSaveData(ref SpawnedData _spawnedData, ref List<TerrainRamdonSpawner> _allRandomObjList,
        ref List<SpawnedData> _allSpawnedDataList,
        ref List<TreeVeget> _treeList, ref List<StoneRock> _stoneList, ref List<IronRock> _ironList,
        ref List<StillElement> _ornaList, ref List<StillElement> _grassList)
    {
        List<SpawnedData> tempList = new List<SpawnedData>();

        for (int i = 0; i < _allRandomObjList.Count; i++)
        {
            bool isMarkToMineLocal = false;
            if (_allRandomObjList[i] is StillElement)
            {
                print("ReSaveData() stillEle");
                StillElement still = (StillElement)_allRandomObjList[i];
                isMarkToMineLocal = still.IsMarkToMine;
            }

            tempList.Add(new SpawnedData(
                _allRandomObjList[i].transform.position, _allRandomObjList[i].transform.rotation, _allSpawnedDataList[i].Type,
                isMarkToMineLocal, _allSpawnedDataList[i].RootStringIndex, _allSpawnedDataList[i].AllVertexIndex));
        }

        ClearCurrentFileAndList(ref _spawnedData, ref _allSpawnedDataList);

        _allSpawnedDataList = tempList;
        SaveData(ref _spawnedData, ref _allSpawnedDataList);
        UpdateSpecificLists(_allRandomObjList, _allSpawnedDataList,
            ref  _treeList, ref _stoneList, ref  _ironList,
            ref  _ornaList, ref  _grassList);
    }

    public void UpdateSpecificLists(List<TerrainRamdonSpawner> _allRandomObjList,
        List<SpawnedData> _allSpawnedDataList,
        ref List<TreeVeget> _treeList, ref List<StoneRock> _stoneList, ref List<IronRock> _ironList,
        ref List<StillElement> _ornaList, ref List<StillElement> _grassList)
    {
        for (int i = 0; i < _allRandomObjList.Count; i++)
        {
            CreateSpecificsList(_allSpawnedDataList[i].Type, _allRandomObjList[i], ref _treeList, ref _stoneList,
                ref _ironList, ref _ornaList, ref _grassList);
        }
    }

    void CreateSpecificsList(H typePass, General temp, 
        ref List<TreeVeget> _treeList, ref List<StoneRock> _stoneList, ref List<IronRock> _ironList,
        ref List<StillElement> _ornaList, ref List<StillElement> _grassList)
    {
        if (typePass == H.Tree)
        {
            _treeList.Add((TreeVeget)temp);
        }
        else if (typePass == H.Stone)
        {
            _stoneList.Add(temp as StoneRock);
        }
        else if (typePass == H.Iron)
        {
            _ironList.Add(temp as IronRock);
        }
        else if (typePass == H.Ornament)
        {
            _ornaList.Add((StillElement)temp);
        }
        else if (typePass == H.Grass)
        {
            _grassList.Add((StillElement)temp);
        }
    }

    public void SaveData(ref SpawnedData _spawnedData, ref List<SpawnedData> _allSpawnedDataList)
    {
        _spawnedData = new SpawnedData();
        _spawnedData.AllSpawnedObj = _allSpawnedDataList;
        _spawnedData.TerraMshCntrlAllVertexIndexCount = p.MeshController.AllVertexs.Count;
        XMLSerie.WriteXMLSpawned(_spawnedData);
    }

    public void LoadData(ref SpawnedData _spawnedData, ref List<SpawnedData> _allSpawnedDataList)
    {
        try { _spawnedData = XMLSerie.ReadXMLSpawned(); }
        catch (Exception exception)
        { print("error loading XMLSerie.ReadXMLSpawned()." + exception.GetBaseException().Message); }

        //print(spawnedData.TerraMshCntrlAllVertexIndexCount + "spawnedData.TerraMshCntrlAllVertexIndexCount");
        //print(Terreno.MeshController.AllVertexs.Count + "Terreno.MeshController.AllVertexs.Count");

        if (_spawnedData.TerraMshCntrlAllVertexIndexCount != p.MeshController.AllVertexs.Count)
        {
            print("subMesh loaded not the same as the one was the spawned obj created with");
            p.TerraSpawnController.IsToSave = true;
            ClearCurrentFileAndList(ref _spawnedData, ref _allSpawnedDataList);
            return;
        }

        if (_spawnedData == null) return;
        _allSpawnedDataList = _spawnedData.AllSpawnedObj;
        p.TerraSpawnController.IsToLoadFromFile = true;
    }

    public void LoadFromFile(ref bool[] _usedVertexPos, ref List<SpawnedData> _allSpawnedDataList,
        ref List<TerrainRamdonSpawner> _allRandomObjList, ref SpawnedData _spawnedData,
        ref List<TreeVeget> _treeList, ref List<StoneRock> _stoneList, ref List<IronRock> _ironList,
        ref List<StillElement> _ornaList, ref List<StillElement> _grassList)
    {
        p.TerraSpawnController.CreateObjAndAddToMainList(_allSpawnedDataList[loadingIndex].Type, 
            _allSpawnedDataList[loadingIndex].Pos,
            _allSpawnedDataList[loadingIndex].RootStringIndex, _allSpawnedDataList[loadingIndex].AllVertexIndex,
            _allSpawnedDataList[loadingIndex].Rot, _allSpawnedDataList[loadingIndex].IsMarkToMine);

        //will restart the value of this array so I know which ones are being used
        _usedVertexPos = new bool[_spawnedData.TerraMshCntrlAllVertexIndexCount];
        _usedVertexPos[_allSpawnedDataList[loadingIndex].AllVertexIndex] = true;

        loadingIndex++;

        //when index is the same as couunt that it
        if (loadingIndex == _allSpawnedDataList.Count)
        {
            p.TerraSpawnController.IsToLoadFromFile = false;
            UpdateSpecificLists(_allRandomObjList, _allSpawnedDataList, ref _treeList, ref _stoneList,
                ref _ironList, ref _ornaList, ref _grassList);
            
            print(_treeList.Count + " treeList.Count");
        }
    }

    void ClearCurrentFileAndList(ref SpawnedData _spawnedData, ref List<SpawnedData> _allSpawnedDataList)
    {
        _allSpawnedDataList.Clear();
        SaveData(ref _spawnedData, ref _allSpawnedDataList);
    }
}
