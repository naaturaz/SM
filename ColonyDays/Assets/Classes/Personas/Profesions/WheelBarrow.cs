﻿using System;
using UnityEngine;

public class WheelBarrow : Profession
{
    private Structure _destinyBuild;
    private Structure _sourceBuild;

    public WheelBarrow(Person person, PersonFile pF)
    {
        if (pF == null)
        {
            CreatingNew(person);
        }
        else LoadingFromFile(person, pF);
    }

    void CreatingNew(Person person)
    {
        
        _person = person;        
        Init();
    }

    void LoadingFromFile(Person person, PersonFile pF)
    {
        _person = person;
        LoadAttributes(pF.ProfessionProp);
        Init();
    }

    void Init()
    {
        //so its not using the same order over and over again in case the Dispatch is finding nothing 
        CleanOldVars();

        //Debug.Log(_person.MyId+" Init WheelB");
        _person.PrevJob = Job.WheelBarrow;

        //if did not fouind a order will return, and take a break now  
        if (!DidPickUpOrder())
        {
            _takeABreakNow = true;
            return;
        }

        ProfDescription = Job.WheelBarrow;
        MyAnimation = "isHammer";

        //means no Orders avail 
        if (_destinyBuild == null)
        {
            _takeABreakNow = true;
            return;
        }

        InitRoute();
    }

    private void CleanOldVars()
    {
        _sourceBuild = null;//from where taking the load 
        _destinyBuild = null;//where taking load 
        Order1 = null;
    }



    private bool DidPickUpOrder()
    {
        if (_person == null)
        {
            return false;
            throw new Exception();
        }
        if (_person.Work == null)
        {
            return false;
            throw new Exception();
        }
        if (_person.Work.Dispatch1 == null)
        {
            return false;
            throw new Exception();
        }

        //Order1 = BuildingPot.Control.Dispatch1.GiveMeOrder(_person);
        Order1 = _person.Work.Dispatch1.GiveMeOrder(_person);

        _person.PrevOrder = Order1;

        if (Order1 == null)
        {
            return false;
        }

        _destinyBuild = Brain.GetStructureFromKey(Order1.DestinyBuild);
        _sourceBuild = Brain.GetStructureFromKey(Order1.SourceBuild);

        DestinyBuildKey = Order1.DestinyBuild;
        SourceBuildKey = Order1.SourceBuild;

        return true;

    }

    void InitRoute()
    {
        if (_sourceBuild == null)
        {
            var t = Order1;
            _sourceBuild = Brain.GetStructureFromKey(Order1.SourceBuild);
        }

        _routerActive = true;
        Router1 = new CryRouteManager(_person.Work, _sourceBuild, _person);
       // Router1 = new RouterManager(_person.Work, _sourceBuild, _person, HPers.InWork);

        IsRouterBackUsed = true;
        RouterBack = new CryRouteManager(_sourceBuild, _destinyBuild, _person,  HPers.InWorkBack);
    }

    public override void Update()
    {
        base.Update();

        if (_takeABreakNow)
        {
            TakeABreak();
            return;
        }

        Execute();
    }

    void Execute()
    {
        if (ExecuteNow)
        {
            ExecuteNow = false;
     
            if (_sourceBuild.HasEnoughToCoverOrder(Order1))
            {
                Debug.Log(_person.MyId+ " Wheel Barr got from:"+Order1.SourceBuild + 
                    " : " +Order1.Product+".amt:"+Order1.Amount);
                _person.ExchangeInvetoryItem(_sourceBuild, _person, Order1.Product, Order1.Amount);
                _sourceBuild.CheckIfCanBeDestroyNow(Order1.Product);
            }
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

            DecideOnNextIteration();    
        }
    }

    void DecideOnNextIteration()
    {
        if (Homer.CheckIfCanBeWheelBar())
        {
            //so it restarted
            Init();
        }
        else
        {
            _person.CreateProfession(Job.Builder);
        }
    }

}
