using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Forester : Profession
{
    private List<TerrainRamdonSpawner> _treesList = new List<TerrainRamdonSpawner>();//save implem

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
        if (_treesList.Count == 0)
        {
            FindTreeToCut();
            OrderedSites = OrderTrees(_treesList);

            //didnt find any tree. That means tht Trees prob have not been loaded yet 
            if (OrderedSites.Count == 0)
            {
                _takeABreakNow = true;
                return;
            }


            FinRoutePoint = OrderedSites[0].Point;
            //moving the route point a bit towards the origin so when chopping tree its not insed the tree 
            FinRoutePoint = Vector3.MoveTowards(FinRoutePoint, _person.Work.transform.position, MoveTowOrigin);

            InitRoute();
        }
    }

    void InitRoute()
    {
        _routerActive = true;

        dummy = (Structure)Building.CreateBuild(Root.dummyBuildWithSpawnPoint, new Vector3(), H.Dummy);
        dummy.transform.position = FinRoutePoint;
        dummy.HandleLandZoning();

        Router1 = new CryRouteManager(_person.Work, dummy, _person);
        RouterBack = new CryRouteManager(dummy, _person.FoodSource, _person,  HPers.InWorkBack, false, true);
    }

    void FindTreeToCut()
    {
        var all = Program.gameScene.controllerMain.TerraSpawnController.AllRandomObjList;

        for (int i = 0; i < all.Count; i++)
        {
            var t = all[i];

            if (t.HType == H.Tree)
            {
                _treesList.Add(t);
            }
        }
        GameScene.print("trees found:"+_treesList.Count);
    }

    List<VectorM> OrderTrees(List<TerrainRamdonSpawner> treeListP)
    {
        var loc = new List<VectorM>();

        for (int i = 0; i < treeListP.Count; i++)
        {
            var pos = treeListP[i].transform.position;
            var key = treeListP[i].MyId;
            var distance = Vector3.Distance(pos, _person.Work.transform.position);

            if (!treeListP[i].IsMarkToMine && distance < Radius)
            {
                loc.Add(new VectorM(pos, _person.Work.transform.position, key));
            }
        }
        GameScene.print("trees found on radius :" + loc.Count);
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
            base.Execute();
            //do stuff
        }
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
