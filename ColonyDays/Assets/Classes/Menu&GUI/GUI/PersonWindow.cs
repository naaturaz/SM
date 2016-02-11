using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PersonWindow : GUIElement {

    private Text _title;
    private Text _info;
    private Text _inv;

    private Person _person;

    private Vector3 iniPos;

    private Rect _genBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    private Rect _invBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D

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

        StartCoroutine("ThreeSecUpdate");
    }

    private IEnumerator ThreeSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f); // wait

            //means is showing 
            if (transform.position == iniPos)
            {
                LoadMenu(); 
                //print("Reloaded");
            }
        }
    }

    void InitObj()
    {
        iniPos = transform.position;

        _title = GetChildThatContains(H.Title).GetComponent<Text>();
        _info = GetChildThatContains(H.Info).GetComponent<Text>();
        _inv = GetChildThatContains(H.Bolsa).GetComponent<Text>();

        var genBtn = GetChildThatContains(H.Gen_Btn).transform;
        var invBtn = GetChildThatContains(H.Inv_Btn).transform;

        _genBtnRect = GetRectFromBoxCollider2D(genBtn);
        _invBtnRect = GetRectFromBoxCollider2D(invBtn);
    }


    private Person oldPerson;
    void CheckIfIsDiffNewPerson()
    {
        if (_person != oldPerson)
        {
            Person.UnselectPerson();
        }
        oldPerson = _person;
    }

    public void Show(Person val)
    {
        _person = val;
        CheckIfIsDiffNewPerson();

        LoadMenu();
        MakeAlphaColorZero(_inv);

        transform.position = iniPos;

        _person.SelectPerson();
    }

    private ShowAInventory _showAInventory;

    private void LoadMenu()
    {
        if (_person==null)
        {
            return;
        }

        _title.text = _person.Name + "";
        _info.text = BuildPersonInfo();

        if (_showAInventory == null)
        {
            _showAInventory = new ShowAInventory(_person.Inventory, gameObject, _inv.transform.localPosition);
        }
        else if (_showAInventory != null && _showAInventory.Inv != _person.Inventory)
        {
            _showAInventory.DestroyAll();
            _showAInventory = new ShowAInventory(_person.Inventory, gameObject, _inv.transform.localPosition);
        }
        _showAInventory.ManualUpdate();
        _inv.text = BuildStringInv(_person);
    }

    string BuildPersonInfo()
    {
        string res = "Age:" + _person.Age + "\n Gender:" + _person.Gender
                     + "\n Nutrition:" + _person.NutritionLevel + "\n Profession:" +
                     _person.ProfessionProp.ProfDescription
                     + "\n ID:" + _person.MyId
                     + "\n FamID:" + _person.FamilyId
                     + "\n Spouse:" + _person.Spouse;

        if (_person.Home!=null)
        {
            res+= "\n Home:" + _person.Home.MyId;
        }
        else res += "\n Home:null";


        res += DebugInfo();

        return res;
    }

    string DebugInfo()
    {
        var res = "\n_________________________________\n GoMindState:" + _person.Brain.GoMindState +
                  "\n fdRouteChks:" + _person.Brain._foodRoute.CheckPoints.Count
                  + "\n movToNwHomRtChks:" + _person.Brain.MoveToNewHome.RouteToNewHome.CheckPoints.Count
                  + "\n CurTask:" + _person.Brain.CurrentTask
                  + "\n PrevTask:" + _person.Brain.PreviousTask
                  + "\n IsBooked:" + _person.IsBooked
                  + "\n Major:" + _person.IsMajor
                  + "\n BodyLoc:" + _person.Body.Location
                  + "\n BodyGngTo:" + _person.Body.GoingTo
                  + "\n BornInfo:" + _person.DebugBornInfo
                  + "\n wrkRouteChks:" + _person.Brain._workRoute.CheckPoints.Count
                  + "\n Profession:" + _person.ProfessionProp
                  + "\n Waiting:" + _person.Brain.Waiting
                  + "\n TimesCall:" + _person.Brain.TimesCall
                  + "\n OnSysNow:" + PersonPot.Control.OnSystemNow(_person.MyId)
                  + "\n OnWaitNow:" + PersonPot.Control.OnWaitListNow(_person.MyId);

        if (_person.ProfessionProp != null)
        {
            res += "\n ProfessionReady:" + _person.ProfessionProp.ReadyToWork;
        }
        else
        {
            res += "\n ProfessionReady: prof is null" ;
        }


        if (_person.FoodSource != null)
        {
            res += "\n FoodSrc:" + _person.FoodSource.MyId;
        }
        else res += "\n FoodSrc:null";

        if (_person.Work != null)
        {
            res += "\n Work:" + _person.Work.MyId;
        }
        else res += "\n Work:null";

        return res;
    }

    // Update is called once per frame
    void Update()
    {
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

        Person.UnselectPerson();
    }
}
