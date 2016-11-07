using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SpawnPool : General
{
    //List<GameObject> _list = new List<GameObject>();
    List<TerrainRamdonSpawner> _tree = new List<TerrainRamdonSpawner>();
    List<TerrainRamdonSpawner> _lawn = new List<TerrainRamdonSpawner>();
    List<TerrainRamdonSpawner> _stone = new List<TerrainRamdonSpawner>();
    List<TerrainRamdonSpawner> _iron = new List<TerrainRamdonSpawner>();
    List<TerrainRamdonSpawner> _gold = new List<TerrainRamdonSpawner>();
    List<TerrainRamdonSpawner> _orna = new List<TerrainRamdonSpawner>();

    void Start()
    {
        //var all = FindAllChildsGameObjectInHierarchy(gameObject);
        var all = GetAllChilds(gameObject);

        for (int i = 0; i < all.Count(); i++)
        {
            if (all[i].name.Contains("Tree") || all[i].name.Contains("Palm"))
            {
                AddToItsList(all[i].GetComponent<TreeVeget>(), _tree);
            }
            else if (all[i].name.Contains("Lawn"))
            {
                AddToItsList(all[i].GetComponent<StillElement>(), _lawn);
            }
            else if (all[i].name.Contains("Stone"))
            {
                AddToItsList(all[i].GetComponent<StoneRock>(), _stone);
            }
            else if (all[i].name.Contains("Iron"))
            {
                AddToItsList(all[i].GetComponent<IronRock>(), _iron);
            }
            else if (all[i].name.Contains("Gold"))
            {
                AddToItsList(all[i].GetComponent<GoldRock>(), _gold);
            }
            else if (all[i].name.Contains("Orna"))
            {
                AddToItsList(all[i].GetComponent<StillElement>(), _orna);
            }
        }
    }

    void AddToItsList(TerrainRamdonSpawner terra, List<TerrainRamdonSpawner> listP)
    {
        if (terra != null)
        {
            //_list.Add(terra);
            listP.Add(terra);
        }
    }


    void Update()
    {

    }

    TerrainRamdonSpawner OneByType(H hType)
    {
        TerrainRamdonSpawner res = null;

        if (hType == H.Tree)
        {
            res = HandleTheList(_tree);
        }
        else if (hType == H.Grass)
        {
            res = HandleTheList(_lawn);
        }
        else if (hType == H.Stone)
        {
            res = HandleTheList(_stone);
        }
        else if (hType == H.Iron)
        {
            res = HandleTheList(_iron);
        }
        else if (hType == H.Gold)
        {
            res = HandleTheList(_gold);
        }  
        else if (hType == H.Ornament)
        {
            res = HandleTheList(_orna);
        }

        return res;
    }

    internal void AddToPool(StillElement stillElement)
    {
        TerrainRamdonSpawner res = stillElement;
        res.enabled = false;
        res.transform.SetParent(transform);
        res.transform.position = new Vector3();
        //_list.Add(res.gameObject);

        if (res.HType == H.Tree)
        {
            _tree.Add(res);
        }
        else if (res.HType == H.Grass)
        {
            _lawn.Add(res);
        }
        else if (res.HType == H.Stone)
        {
            _stone.Add(res);
        }
        else if (res.HType == H.Iron)
        {
            _iron.Add(res);
        }
        else if (res.HType == H.Gold)
        {
            _gold.Add(res);
        } 
        else if (res.HType == H.Ornament)
        {
            _orna.Add(res);
        }
    }


    TerrainRamdonSpawner HandleTheList(List<TerrainRamdonSpawner> listP)
    {
        if (listP.Count == 0)
        {
            return null;
        }
        var index = UMath.GiveRandom(0, listP.Count);
        var res = listP[index];
        listP.RemoveAt(index);
        //_list.Remove(res.gameObject);
        return res;
    }

    public TerrainRamdonSpawner RetTerraSpawn(Vector3 origen, Vector3 rotation,
      int indexAllVertex, H hType,
      string name = "", Transform container = null, bool replantedTree = false,
      float height = 0, MDate seedDate = null, float maxHeight = 0,
      Quaternion rot = new Quaternion())
    {
        TerrainRamdonSpawner obj = OneByType(hType);

        if (obj == null)
        {
            //Debug.Log("needed on Pool: " + hType);
            return obj;
        }

        obj.enabled = true;

        if (name != "") { obj.name = name; }
        obj.IndexAllVertex = indexAllVertex;
        obj.HType = hType;
        obj.Category = obj.DefineCategory(hType);
        obj.MyId = obj.Rename(name, obj.Id, obj.HType);
        obj.transform.name = obj.MyId;

        //here to avoid rotating object after spwaned
        //for loading
        obj.transform.rotation = rot;
        //for new obj
        obj.transform.Rotate(rotation);
        obj.transform.position = origen;

        obj.ReplantedTree = replantedTree;
        obj.Height = height;
        obj.SeedDate = seedDate;

        if (container != null) { obj.transform.SetParent(container); }

        return obj;
    }


}

