using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Forester : Profession
{
    private List<TerrainRamdonSpawner> _spawnersList = new List<TerrainRamdonSpawner>();//save implem
    private Vector3 _treeCenterPos;


    public Forester(Person person, PersonFile pF)
    {
        if (pF == null)
        {
            CreatingNew(person);
        }
        else LoadingFromFile(person, pF);
    }

    void CreatingNew(Person person)
    {
        ProfDescription = Job.Forester;
        IsRouterBackUsed = true;
        MyAnimation = "isSummon";
        _person = person;
        Lock();
        Init();
    }

    void LoadingFromFile(Person person, PersonFile pF)
    {
        _person = person;
        LoadAttributes(pF.ProfessionProp);
    }

    private void Init()
    {
        if (_spawnersList.Count == 0)
        {
            FindSpawnersToMine();
            OrderedSites = OrderSpawners(_spawnersList);

            //didnt find any tree. That means tht Trees prob have not been loaded yet 
            if (OrderedSites.Count == 0)
            {
                _takeABreakNow = true;
                return;
            }
            _treeCenterPos = DefineMiddlePos(OrderedSites);

            FinRoutePoint = OrderedSites[0].Point;
            StillElementId = OrderedSites[0].LocMyId;
            //moving the route point a bit towards the origin so when chopping tree its not inside the tree 
            FinRoutePoint = Vector3.MoveTowards(FinRoutePoint, _person.Work.transform.position, MoveTowOrigin * 2.5f);

            InitRoute();
        }
    }

    Vector3 DefineMiddlePos(List<VectorM> list)
    {
        Vector3 res = new Vector3();

        for (int i = 0; i < list.Count; i++)
        {
            res += list[i].Point;
        }
        return res/list.Count;
    }

    void InitRoute()
    {
        _routerActive = true;

        dummy = (Structure)Building.CreateBuild(Root.dummyBuildWithSpawnPoint, new Vector3(), H.Dummy);
        dummy.transform.position = FinRoutePoint;
        dummy.transform.LookAt(_treeCenterPos);
        dummy.HandleLandZoning();

        //so it doesnt add like a door at the end when gets to tree
        Router1 = new CryRouteManager(_person.Work, dummy, _person, HPers.None, true, false);
        RouterBack = new CryRouteManager(dummy, _person.FoodSource, _person,  HPers.InWorkBack, false, true);
    }

    P FromHEnumToP(H val)
    {
        var content = (P)Enum.Parse(typeof(H), val.ToString());
        return content;
    }

    P ProcessHTypeSpawnerIntoProduct(H hTypeP)
    {
        if (hTypeP == H.Tree)
        {
            return P.Wood;
        }

        if (hTypeP == H.Stone)
        {
            return P.Stone;
        }
        else
        {
            return P.Ore;
        }
    }
    
    void FindSpawnersToMine()
    {
        var all = Program.gameScene.controllerMain.TerraSpawnController.AllRandomObjList;

        for (int i = 0; i < all.Count; i++)
        {
            var t = all[i];

            if (t.HType == H.Tree || t.HType == H.Stone || t.HType == H.Iron || t.HType == H.Gold)
            {
                _spawnersList.Add(t);
            }
        }
        GameScene.print("spawners found:"+_spawnersList.Count);
    }

    List<VectorM> OrderSpawners(List<TerrainRamdonSpawner> listP)
    {
        List<VectorM> loc = new List<VectorM>();

        if (listP == null)
        {
            _takeABreakNow = true;
            return loc;
        }

        for (int i = 0; i < listP.Count; i++)
        {
            //means that tree was deleted but the list has not being updated 
            if (listP[i] == null)
            {
                continue;
            }

            var pos = listP[i].transform.position;
            var key = listP[i].MyId;
            var hType = listP[i].HType;
            var distance = Vector3.Distance(pos, _person.Work.transform.position);

            if (!listP[i].IsMarkToMine && distance < Radius)
            {
                loc.Add(new VectorM(pos, _person.Work.transform.position, key, hType));
            }
        }
        GameScene.print("spawners found on radius :" + loc.Count);
        return loc.OrderBy(a => a.Distance).ToList();
    }

    TerrainRamdonSpawner GetFromKey(string keyP)
    {
        return Program.gameScene.controllerMain.TerraSpawnController.AllRandomObjList[keyP] ;
    }

    public override void Update()
    {
        if (_takeABreakNow)
        {
            TakeABreak();
            return;
        }

        base.Update();
        Execute();

        //GameScene.print("Update on Foreset ");
    }

    /// <summary>
    /// The specific action of a Proffession 
    /// Ex: Forester add lumber to its inventory and removed the amt from tree invetory
    /// </summary>
    void Execute()
    {
        if (ExecuteNow)
        {
            ExecuteNow = false;

            if (_person.Work.CanTakeItOut(_person))
            {
                P prod = FindProdImMining();
                base.Execute(Job.Forester.ToString(), prod);
            }
            else
            {
                //_person.Work.AddEvacuationOrderMost();
                //todo add to notify //Forester didnt work bz its Home Storage is full
                Debug.Log(_person.MyId +", Forester didnt work bz its Home Storage is full");
            }
        }
    }

    private P FindProdImMining()
    {
        var ele =
            Program.gameScene.controllerMain.TerraSpawnController.FindThis(StillElementId);
        
        return ProcessHTypeSpawnerIntoProduct(ele.HType);
    }


    private bool _takeABreakNow;
    private float _breakDuration = 5f;
    private float startIdleTime;
    /// <summary>
    /// Used so a person is asking for bridges anchors takes a break and let brdige anchors complete then can 
    /// work on it
    /// </summary>
    void TakeABreak()
    {
        if (startIdleTime == 0)
        { startIdleTime = Time.time; }

        if (Time.time > startIdleTime + _breakDuration)
        {
            _takeABreakNow = false;
            startIdleTime = 0;

            //so it restarted 
            Init();
        }
    }
}
