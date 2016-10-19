﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PersonWindow : GUIElement
{

    private Text _title;
    private Text _info;
    private Text _inv;

    private Person _person;

    private Vector3 iniPos;

    private Rect _genBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    private Rect _invBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D

    private GameObject _invIniPos;

    private ShowAInventory _showAInventory;


    private ShowAPersonBuildingDetails _aPersonBuildingDetails;

    public Person Person1
    {
        get { return _person; }
        set { _person = value; }
    }

    // Use this for initialization
    void Start()
    {
        InitObj();
        Hide();
    }

    void InitObj()
    {
        _invIniPos = GetGrandChildCalled(H.Inv_Ini_Pos);

        iniPos = transform.position;

        _title = GetChildThatContains(H.Title).GetComponent<Text>();
        _info = GetChildThatContains(H.Info).GetComponent<Text>();
        _inv = GetChildThatContains(H.Bolsa).GetComponent<Text>();

        var genBtn = GetChildThatContains(H.Gen_Btn).transform;
        var invBtn = GetChildThatContains(H.Inv_Btn).transform;

        _genBtnRect = GetRectFromBoxCollider2D(genBtn);
        _invBtnRect = GetRectFromBoxCollider2D(invBtn);
    }


    //private Person oldPerson;
    //void CheckIfIsDiffNewPerson()
    //{
    //    if (_person != oldPerson)
    //    {
    //        _person.UnselectPerson();
    //    }
    //    oldPerson = _person;
    //}

    public void Show(Person val)
    {
        if (_person != null)
        {
            _person.UnselectPerson();
        }

        _person = val;
        //CheckIfIsDiffNewPerson();

        LoadMenu();
        MakeAlphaColorZero(_inv);

        transform.position = iniPos;
        _person.SelectPerson();
    }


    private void LoadMenu()
    {
        if (_person == null)
        {
            return;
        }

        _title.text = _person.Name + "";
        _info.text = BuildPersonInfo();

        if (_showAInventory == null)
        {
            _showAInventory = new ShowAInventory(_person.Inventory, gameObject, _invIniPos.transform.localPosition);
        }
            //diff  person
        else if (_showAInventory != null && _showAInventory.Inv != _person.Inventory)
        {
            _showAInventory.DestroyAll();
            _showAInventory = new ShowAInventory(_person.Inventory, gameObject, _invIniPos.transform.localPosition);

            if (_aPersonBuildingDetails != null)
            {
                _aPersonBuildingDetails.ManualUpdate(_person, true);
            }
        
        }
        _showAInventory.ManualUpdate();
        _inv.text = BuildStringInv(_person);

        if (_aPersonBuildingDetails == null)
        {
            _aPersonBuildingDetails = new ShowAPersonBuildingDetails(_person, gameObject, _invIniPos.transform.localPosition);
        }
        else
        {
            //manual update
            _aPersonBuildingDetails.ManualUpdate(_person);
        }

    }


    string BuildPersonInfo()
    {


        return "";
        string res = "Age: " + _person.Age + "\n Gender: " + _person.Gender
                     + "\n Nutrition: " + _person.NutritionLevel.ToString("N0")
                     + "\n Profession: " + _person.ProfessionProp.ProfDescription

                     + "\n Spouse: " + Family.RemovePersonIDNumberOff(_person.Spouse)
                     + "\n Happinness: " + _person.Happinnes
                     + "\n Years Of School: " + _person.YearsOfSchool
                     + "\n Age majority reach: " + _person.IsMajor;


        if (_person.Home != null)
        {
            res += "\n Home: " + _person.Home.HType;
        }
        else res += "\n Home: None";
        if (_person.Work != null)
        {
            res += "\n Work place: " + _person.Work.HType;
        }
        else res += "\n Work place: None";
        if (_person.FoodSource != null)
        {
            res += "\n Food Source: " + _person.FoodSource.HType;
        }
        else res += "\n Food Source: None";



#if UNITY_EDITOR
        res += DebugInfo();
#endif
        return res;
    }

    string DebugInfo()
    {
        var res = "\n___________________\n" +
            "\n currentAni:" + _person.Body.CurrentAni +

            "\n PrevJob:" + _person.PrevJob
            + "\n ID:" + _person.MyId
            + "\n FamID:" + _person.FamilyId
            + "\n UnHappyYears:" + _person.UnHappyYears;




        res += "___________________\n GoMindState:" + _person.Brain.GoMindState +
                  "\n fdRouteChks:" + _person.Brain._foodRoute.CheckPoints.Count +
                  "\n idleRouteChks:" + _person.Brain._idleRoute.CheckPoints.Count
                  + "\n movToNwHomRtChks:" + _person.Brain.MoveToNewHome.RouteToNewHome.CheckPoints.Count
                  + "\n CurTask:" + _person.Brain.CurrentTask
                  + "\n PrevTask:" + _person.Brain.PreviousTask
                  + "\n IsBooked:" + _person.IsBooked
                  + "\n BodyLoc:" + _person.Body.Location
                  + "\n BodyGngTo:" + _person.Body.GoingTo
                  + "\n BornInfo:" + _person.DebugBornInfo
                  + "\n wrkRouteChks:" + _person.Brain._workRoute.CheckPoints.Count;



        if (_person.ProfessionProp != null)
        {
            res += "\n Profession ReadyToWork:" + _person.ProfessionProp.ReadyToWork;
            res += "\n Profession workerTask:" + _person.ProfessionProp.WorkerTask;
            res += "\n Profession workingNow:" + _person.ProfessionProp.WorkingNow;
        }
        else
        {
            res += "\n ProfessionReady: prof is null";
        }



        res += "\n Waiting:" + _person.Brain.Waiting
                  + "\n TimesCall:" + _person.Brain.TimesCall
                  + "\n OnSysNow:" + PersonPot.Control.OnSystemNow(_person.MyId)
                  + "\n OnWaitNow:" + PersonPot.Control.OnWaitListNow(_person.MyId);

        return res;
    }

    private int updCount;
    // Update is called once per frame
    void Update()
    {
        updCount++;
        //means is showing 
        if (Vector3.Distance(transform.position, iniPos) < 0.1f)
        {
            if (updCount > 6)
            {
                updCount = 0;
                LoadMenu();
                //print("Reloaded");
            }
        }



        //if click gen
        if (_genBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0))
        {
            MakeAlphaColorMax(_info);
            MakeAlphaColorZero(_inv);
        }
        //ig click inv
        else if (_invBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0))
        {
            MakeAlphaColorMax(_inv);
            MakeAlphaColorZero(_info);
        }

        //then update inv info all the time 
        if (_person != null && _inv != null && !string.IsNullOrEmpty(_inv.text))
        {
            _inv.text = BuildStringInv(_person);
        }

      
    }

    public override void Hide()
    {
        base.Hide();

        if (_person != null)
        {
            _person.UnselectPerson();
        }
    }

    /// <summary>
    /// Called from GUI
    /// 
    /// Or added event to ShowPersonBuildingTile
    /// </summary>
    /// <param name="which"></param>
    public void ShowPath(string which)
    {
        _person.ToggleShowPath(which);
    }


}
