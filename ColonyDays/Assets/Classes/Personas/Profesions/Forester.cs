using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Forester : Profession
{
    private List<TerrainRamdonSpawner> _spawnersList = new List<TerrainRamdonSpawner>();//save implem
    private Vector3 _treeCenterPos;

    private StillElement _stillElement;


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
        //in case was a Wheelbarrow the prevProfession and when home route back gives problem 
        person.PrevOrder = null;

        person.PrevJob = ProfDescription;
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

        //so the detecting CheckIfShouldReDoProf() works 
        FindSpawnersToMine();
    }

    private void Init()
    {
        //AddMeToWaitListOnSystem();
        //if (!PersonPot.Control.OnSystemNow(_person.MyId))
        //{
        //    _takeABreakNow = true;
        //    return;
        //}

        FindSpawnersToMine();
        OrderedSites = OrderSpawners(_spawnersList);

        //didnt find any tree. That means tht Trees prob have not been loaded yet 
        if (OrderedSites.Count == 0 || _person.FoodSource == null)
        {
            _takeABreakNow = true;
            return;
        }
        _treeCenterPos = DefineMiddlePos(OrderedSites);

        FinRoutePoint = OrderedSites[0].Point;
        StillElementId = OrderedSites[0].LocMyId;

        _stillElement = Program.gameScene.controllerMain.TerraSpawnController.Find(StillElementId);
        var closerAnchorToUs = _stillElement.FindCloserAnchorTo(_person.Work);

        //moving the route point a bit towards the origin so when chopping tree its not inside the tree 
        FinRoutePoint = Vector3.MoveTowards(closerAnchorToUs, _person.Work.transform.position, MoveTowOrigin * 0.05f);//2,5

        InitRoute();
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
        RouterActive = true;

        //bz dummy.DummyIdSpawner
        dummy = (Structure)Building.CreateBuild(Root.dummyBuildWithSpawnPoint, new Vector3(), H.Dummy);

        dummy.transform.position = FinRoutePoint;
        dummy.transform.LookAt(_treeCenterPos);
        dummy.HandleLandZoning();
        dummy.DummyIdSpawner = StillElementId; 

        //so it doesnt add like a door at the end when gets to tree
        Router1 = new CryRouteManager(_person.Work, dummy, _person, HPers.None, true, false);
        RouterBack = new CryRouteManager(dummy, _person.FoodSource, _person,  HPers.InWorkBack, false, true);
    }

    Structure CreateDummy()
    {
        return Program.gameScene.GimeMeUnusedDummy(ProfDescription + ".Dummy." + StillElementId);
    }
    
    void FindSpawnersToMine()
    {
        var all = Program.gameScene.controllerMain.TerraSpawnController.AllRandomObjList;

        for (int i = 0; i < all.Count; i++)
        {
            var t = all[i];

            //or if is blacklisted 
            if (t == null || _person.Brain.BlackList.Contains(t.MyId) || !t.Grown())
            {
                continue;
            }

            if (t.HType == H.Tree || t.HType == H.Stone || t.HType == H.Iron || t.HType == H.Gold)
            {
                _spawnersList.Add(t);
            }
        }
        //GameScene.print("spawners found:"+_spawnersList.Count);
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
            if (listP[i] == null || !listP[i].Grown())
            {
                continue;
            }

            var pos = listP[i].transform.position;
            var key = listP[i].MyId;
            var hType = listP[i].HType;
            var distance = Vector3.Distance(pos, _person.Work.transform.position);

            if (distance < Radius)
            {
                loc.Add(new VectorM(pos, _person.Work.transform.position, key, hType));
            }
        }
        //GameScene.print("spawners found on radius :" + loc.Count);
        return loc.OrderBy(a => a.Distance).ToList();
    }

    public override void Update()
    {
        CheckIfShouldReDoProf();

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
    /// Bz need to redo Prof if could 
    /// If StillElement is null
    /// 
    /// So people dont go to stare to an non existing tree over and over again
    /// </summary>
    private void CheckIfShouldReDoProf()
    {
        //seein the spawner list bz if last time was bigger than one I can still find prob another 
        //stillElement to mine 
        if (!_workingNow && _spawnersList.Count > 1)
        {
            CheckIfProfHasToBeReCreated();
        }
    }

    /// <summary>
    /// The specific action of a Proffession 
    /// Ex: Forester add lumber to its inventory and removed the amt from tree invetory
    /// </summary>
    void Execute()
    {
        if (ExecuteNow && ReadyToWork)
        {
            ExecuteNow = false;

            ////foresters reset when done work
            //Debug.Log("Foreset reset dummy");
            ResetDummy();

            if (_person.Work.CanTakeItOut(_person))
            {
                if (CheckIfStillElementWasDestroy())
                {
                    //so it doesnt get a null ref in below methods 
                    return;
                }
                
                P prod = FindProdImMining(StillElementId, _person);
                SetProdImMiningWeight();
                base.Execute(Job.Forester.ToString(), prod);
                RemoveWeightFromMiningEle(prod);
            }
            else
            {
                //_person.Work.AddEvacuationOrderMost();
                //todo add to notify //Forester didnt work bz its Home Storage is full
                //Debug.Log(_person.MyId +", Forester didnt work bz its Home Storage is full");
            }
        }
        else if (ExecuteNow && !ReadyToWork)
        {
            //so we leave it for next time to see if is ready
            //othwrwise gives buggg bz wherever is will do stuff as is will be inside the work place
            ExecuteNow = false;
        }
    }

    private void ReInitIfEleNull()
    {
        if (_reInit)
        {
            return;
        }

        var ele =
           Program.gameScene.controllerMain.TerraSpawnController.Find(StillElementId);

        if (ele == null)
        {
            //so find a new Object to mine 
            //do the new Routing etc
            _reInit = true;
        }
    }

    private bool CheckIfStillElementWasDestroy()
    {
        if (_person == null)
        {
            //here is not really true but can skip all bz _person is null
            return true;
        }

        //var ele =
        //    Program.gameScene.controllerMain.TerraSpawnController.Find(StillElementId);

        //when is just on site to play animation of building 
        if (_stillElement == null)//ele
        {
            //so skip all that and goes back to office 
            return true;
        }
        return false;
    }

    /// <summary>
    /// Remove the weight from the element Im mining 
    /// </summary>
    private void RemoveWeightFromMiningEle(P prod)
    {
        //var ele =
        //    Program.gameScene.controllerMain.TerraSpawnController.Find(StillElementId);
        
        _stillElement.MinedNowBy--;

        if (!_person.Inventory.Contains(prod))
        {
            //means didnt pick the Prod. Problably bz its Storage is full
            return;
        }
        _stillElement.RemoveWeight(ProdXShift, _person);
    }

    /// <summary>
    /// Sets the initial weight of the element Im Mining 
    /// </summary>
    private void SetProdImMiningWeight()
    {
        //was recently destroyed 
        if (_stillElement == null)
        {
            return;
        }

        _stillElement.SetWeight();
    }

    public static P FindProdImMining(string stillElementIdP, Person person)
    {
        var ele =
            Program.gameScene.controllerMain.TerraSpawnController.Find(stillElementIdP);

        if (ele == null)
        {
            if (person == null || person.Work == null)
            {
                return P.None;
            }

            return person.Work.CurrentProd.Product;
        }
        
        return ProcessHTypeSpawnerIntoProduct(ele.HType);
    }

    static P ProcessHTypeSpawnerIntoProduct(H hTypeP)
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

    private bool _takeABreakNow;
    private float _breakDuration = 1f;
    private float startIdleTime;
    private bool _reInit;
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
