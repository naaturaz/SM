﻿using System;
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

        MyAnimation = "isAxe";
        _person = person;
        HandleNewProfDescrpSavedAndPrevJob(Job.Forester);

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

        //asking for grown in case is there but from an old list  
        if (!_stillElement.Grown())
        {
            StillElementId = "";
            _spawnersList.Remove(_stillElement);
            _takeABreakNow = true;
            return;
        }
        HandleStillElement();
       
        //moving the route point a bit towards the origin so when chopping tree its not inside the tree 
        FinRoutePoint = Vector3.MoveTowards(_stillElement.transform.position, _person.Work.transform.position, 0.4f);

        //correcting y
        FinRoutePoint = new Vector3(FinRoutePoint.x, _person.transform.position.y, FinRoutePoint.z);

        //Debug.Log("routerBackWasInit set false");
        routerBackWasInit = false;
        startIdleTime = 0;

        InitRoute();
    }

    void HandleStillElement()
    {
        if (_stillElement != null)
        {
            //remove element from Batch
            Program.gameScene.BatchRemove(_stillElement);
            var animator = _stillElement.GetComponent<Animator>();

            //in case was deleted right now
            if (animator != null)
            {
                animator.enabled = true;
            }
            //todo remove to all Foresters
            _spawnersList.Remove(_stillElement);
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
        RouterActive = true;
        IsRouterBackUsed = true;

        //bz dummy.DummyIdSpawner
        _person.MyDummyProf.MyId = StillElementId;
        _person.MyDummyProf.transform.position = FinRoutePoint;
        
        _person.MyDummyProf.transform.LookAt(new Vector3(_treeCenterPos.x, _person.MyDummyProf.transform.position.y,
            _treeCenterPos.z) );

        _person.MyDummyProf.LandZone1.Clear();
        _person.MyDummyProf.HandleLandZoning();
        _person.MyDummyProf.DummyIdSpawner = StillElementId;

        //UVisHelp.CreateText(FinRoutePoint, StillElementId, 60);

        //bz sometimes falls inside the Still element
        //MoveDummyAwayFromEleSoDoesntFallInsideOfIt();

        //so it doesnt add like a door at the end when gets to tree
        Router1 = new CryRouteManager(_person.Work, _person.MyDummyProf, _person,HPers.InWork, true, false);
    }
    
    void FindSpawnersToMine()
    {
        var all = Program.gameScene.controllerMain.TerraSpawnController.AllRandomObjList;

        for (int i = 0; i < all.Count; i++)
        {
            var t = all[i];
            //or if is blacklisted 
            if (t == null || _person.Brain.BlackList.Contains(t.MyId) || !t.Grown() )
            {
                continue;
            }

            if (t.HType == H.Tree || t.HType == H.Stone || t.HType == H.Iron || t.HType == H.Gold
                )
            {
                _spawnersList.Add(t);
            }
        }
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

            if (distance < Brain.Maxdistance)
            {
                loc.Add(new VectorM(pos, _person.Work.transform.position, key, hType));
            }
        }
        //GameScene.print("spawners found on radius :" + loc.Count);
        return loc.OrderBy(a => a.Distance).ToList();
    }

    public override void Update()
    {
        CheckIfRoute1IsReady();
        CheckIfStillEleWasBlackListedOrIsDiff();
        CheckIfShouldReDoProf();
        CheckIfBlackList();

        if (_takeABreakNow)
        {
            TakeABreak();
            return;
        }

        base.Update();
        Execute();
    }

    int count;
    /// <summary>
    /// bz sometimes they blacklist the Storage
    /// </summary>
    private void CheckIfBlackList()
    {
        count++;

        if (_person != null && count > 1800)
        {
            count = 0;
            _person.Brain.ClearEachBlackListedBuilding();
        }
    }

    /// <summary>
    /// Bz is he blacklisted his element he need to get a break and start all over again
    /// </summary>
    private void CheckIfStillEleWasBlackListedOrIsDiff()
    {
        if (_person==null || _stillElement==null)
        {
            return;
        }

        if (_person.Brain.BlackList.Contains(_stillElement.MyId))
        {
            Debug.Log("Forester take break:"+_person.MyId);
            _takeABreakNow = true;
        }
    }

    private bool routerBackWasInit;
    /// <summary>
    /// So it doesnt blackList nothing in the second Route if he is blackListug a tree in the Router1
    /// </summary>
    private void CheckIfRoute1IsReady()
    {
        if (RouterActive && Router1.IsRouteReady && !routerBackWasInit)
        {
            routerBackWasInit=true;
            RouterBack = new CryRouteManager(_person.MyDummyProf, _person.FoodSource, _person, HPers.InWorkBack, false, true);
        }
    }

    /// <summary>
    /// Needed so the route starts a bits away from Still element 
    /// </summary>
    private void MoveDummyAwayFromEleSoDoesntFallInsideOfIt()
    {
        _person.MyDummyProf.transform.position = 
            Vector3.MoveTowards(_person.MyDummyProf.transform.position, _stillElement.transform.position, -2);
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
        if (!_workingNow && _spawnersList.Count > 1 &&
            (prodCarrying == P.None || _person.Inventory.CurrentVolumeOcuppied() == 0))//if is carrying someting need to drop it 1st
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
            if (_stillElement!=null)
            {
                //AudioPlayer.PlaySoundOneTime(RootSound.axe, _stillElement.gameObject.transform.position);
                
            }
            ////foresters reset when done work
            ResetDummy();

            if (_person.Work.CanTakeItOut(_person))
            {
                if (ElementWasCut())
                {
                    return;
                }
                
                P prod = FindProdImMining(_stillElement.MyId, _person);
                SetProdImMiningWeight();
                base.Execute(Job.Forester.ToString(), prod);
                RemoveWeightFromMiningEle(prod);
            }
            else
            {
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

    /// <summary>
    /// Needs to check before cut if the element is not Grown
    /// </summary>
    /// <returns></returns>
    private bool ElementWasCut()
    {
        if (_person == null)
        {
            return true;
        }

        if (_stillElement == null || !_stillElement.Grown())
        {
            return true;
        }
        return false;
    }


    /// <summary>
    /// Remove the weight from the element Im mining 
    /// </summary>
    private void RemoveWeightFromMiningEle(P prod)
    {
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
