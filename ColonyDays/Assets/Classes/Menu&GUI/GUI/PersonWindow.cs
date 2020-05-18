using UnityEngine;
using UnityEngine.UI;

public class PersonWindow : Window
{
    private Text _title;
    private Rect _genBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    private Rect _invBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D

    private GameObject _invIniPos;
    private GameObject _inv_Ini_Pos_Gen;

    private ShowAPersonBuildingDetails _aPersonBuildingDetails;

    private GameObject _general;
    private GameObject _genBtn;//the btn

    public Person Person1 { get; set; }
    private string _oldPersonMyId;

    private GameObject oldTabActive;
    private GameObject _debugger;
    private Text _debugger_Text;

    // Use this for initialization
    private void Start()
    {
        base.Start();
        InitObj();
        Hide();
    }

    private void InitObj()
    {
        _general = GetChildThatContains(H.General);

        _invIniPos = GetGrandChildCalled(H.Inv_Ini_Pos);
        _inv_Ini_Pos_Gen = GetGrandChildCalled("Inv_Ini_Pos_Gen");
        _title = GetChildThatContains(H.Title).GetComponent<Text>();

        _genBtn = GetChildThatContains(H.Gen_Btn);
        _genBtnRect = GetRectFromBoxCollider2D(_genBtn.transform);
        var img = _genBtn.GetComponent<Image>();
        _initialTabColor = img.color;

        _debugger = GetChildCalled("Debugger");
        _debugger_Text = GetChildCalledOnThis("Debugger_Text", _debugger).GetComponent<Text>();
    }

    public void Show(Person val)
    {
        base.Show();

        Program.MouseListener.HideBuildingsMenu();

        if (Person1 != null)
        {
            Person1.UnselectPerson();
            if (_oldPersonMyId != Person1.MyId)
            {
                CleanPersonDetails();
                //so if its a diff person will redo _aPersonBuildingDetails
                _aPersonBuildingDetails = null;
                _oldPersonMyId = Person1.MyId;
            }
        }

        MakeThisTabActive(_general);
        Person1 = val;

        UpdateInputTitle();

        //for GC reason was moved here. Was allocating ~100KB per frame
        MakeThisTabActive(oldTabActive);
        LoadMenu();

        Person1.SelectPerson();

        LoadOrHideDebuggerTab();
    }

    private void LoadMenu()
    {
        if (Person1 == null)
            return;

        _title.text = Person1.Name + "";

        if (_aPersonBuildingDetails == null)
        {
            _aPersonBuildingDetails = new ShowAPersonBuildingDetails(Person1, _general, _inv_Ini_Pos_Gen.transform.localPosition);
        }
        else
        {
            //manual update
            _aPersonBuildingDetails.ManualUpdate(Person1);
        }
    }

    private int updCount;

    // Update is called once per frame
    private void Update()
    {
        updCount++;
        //means is showing
        if (Vector3.Distance(transform.position, IniPos) < 0.1f)
        {
            if (updCount > 6)
            {
                updCount = 0;
                LoadMenu();
            }
        }

        //if click gen
        if (_genBtnRect.Contains(Input.mousePosition) && Input.GetMouseButtonUp(0))
            MakeThisTabActive(_general);
    }

    /// <summary>
    /// Use to swith Tabs on Window. Will hide all and make the pass one as active
    /// </summary>
    /// <param name="g"></param>
    private void MakeThisTabActive(GameObject g)
    {
        if (Person1 == null) return;

        //first time loaded ever in game
        if (g == null) g = _general;

        _general.SetActive(false);
        ColorTabInactive(_genBtn);

        g.SetActive(true);

        if (g == _general)
            ColorTabActive(_genBtn);

        oldTabActive = g;
    }

    public override void Hide()
    {
        base.Hide();

        if (Person1 != null)
            Person1.UnselectPerson();

        CleanPersonDetails();
    }

    private void CleanPersonDetails()
    {
        if (_aPersonBuildingDetails != null)
        {
            for (int i = 0; i < _aPersonBuildingDetails.Tiles.Count; i++)
            {
                //_aPersonBuildingDetails.Tiles[i].Destroy();
                Destroy(_aPersonBuildingDetails.Tiles[i].gameObject);
                Destroy(_aPersonBuildingDetails.Tiles[i]);
            }
            _aPersonBuildingDetails.Tiles.Clear();
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
        Person1.ToggleShowPath(which);
    }

    protected void UpdateInputTitle()
    {
        _titleInputFieldGO.SetActive(true);

        _titleInputField.text = Person1.Name;
        _titleInputFieldGO.SetActive(false);
    }

    public void NewAlias()
    {
        var oldName = Person1.Name;
        Person1.Name = _titleInputField.text;
        _titleInputFieldGO.SetActive(false);
        _title.text = Person1.Name;
        Program.UnLockInputSt();

        if (oldName != Person1.Name)
            Program.gameScene.TutoStepCompleted("Rename.Tuto");
    }

    public void LockInput()
    {
        Program.LockInputSt();
    }

    //Debug
    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    private string BuildPersonInfo()
    {
        if (Person1 == null) return "";

        string res = "Age: " + Person1.Age + "\n Gender: " + Person1.Gender
                     + "\n Nutrition: " + Person1.NutritionLevel
                     + "\n Profession: " + Person1.ProfessionProp.ProfDescription

                     + "\n Spouse: " + Family.GetPersonName(Person1.Spouse)
                     + "\n Happinness: " + Person1.Happinnes
                     + "\n Years Of School: " + Person1.YearsOfSchool
                     + "\n Age majority reach: " + Person1.IsMajor;

        if (Person1.Home != null)
        {
            res += "\n Home: " + Person1.Home.HType;
        }
        else res += "\n Home: None";
        if (Person1.Work != null)
        {
            res += "\n Work place: " + Person1.Work.HType;
        }
        else res += "\n Work place: None";
        if (Person1.FoodSource != null)
        {
            res += "\n Food Source: " + Person1.FoodSource.HType;
        }
        else res += "\n Food Source: None";

#if UNITY_EDITOR
        res += DebugInfo();

        res += "\n\n" + DebugAgentInfo();
#endif
        return res;
    }

    private string DebugAgentInfo()
    {
        return Person1.Body.BodyAgent.DebugInfo();
    }

    private string DebugInfo()
    {
        var res = "\n___________________\n" +
            "\n currentAni:" + Person1.Body.CurrentAni +

            "\n PrevJob:" + Person1.PrevJob
            + "\n ID:" + Person1.MyId
            + "\n FamID:" + Person1.FamilyId
            + "\n UnHappyYears:" + Person1.UnHappyYears;

        res += "___________________\n GoMindState:" + Person1.Brain.GoMindState +
                  "\n fdRouteChks:" + Person1.Brain._foodRoute.CheckPoints.Count +
                  "\n idleRouteChks:" + Person1.Brain._idleRoute.CheckPoints.Count
                  + "\n movToNwHomRtChks:" + Person1.Brain.MoveToNewHome.RouteToNewHome.CheckPoints.Count
                  + "\n CurTask:" + Person1.Brain.CurrentTask
                  + "\n PrevTask:" + Person1.Brain.PreviousTask
                  + "\n IsBooked:" + Person1.IsBooked
                  + "\n BodyLoc:" + Person1.Body.Location
                  + "\n BodyGngTo:" + Person1.Body.GoingTo
                  + "\n BornInfo:" + Person1.DebugBornInfo
                  + "\n wrkRouteChks:" + Person1.Brain._workRoute.CheckPoints.Count
                  + "\n Body MovingNow:" + Person1.Body.MovingNow;

        if (Person1.ProfessionProp != null)
        {
            res += "\n Profession ReadyToWork:" + Person1.ProfessionProp.ReadyToWork;
            res += "\n Profession workerTask:" + Person1.ProfessionProp.WorkerTask;
            res += "\n Profession workingNow:" + Person1.ProfessionProp.WorkingNow;
        }
        else
        {
            res += "\n ProfessionReady: prof is null";
        }

        res += "\n Waiting:" + Person1.Brain.Waiting
                  + "\n TimesCall:" + Person1.Brain.TimesCall
                  + "\n OnSysNow:" + PersonPot.Control.OnSystemNow(Person1.MyId)
                  + "\n OnWaitNow:" + PersonPot.Control.OnWaitListNow(Person1.MyId);

        return res;
    }

    public void ToggleDebugger()
    {
        _debugger.SetActive(!_debugger.activeSelf);
        PlayerPrefs.SetString("PersonWindow_Debugger", _debugger.activeSelf + "");

        LoadOrHideDebuggerTab();
    }

    private void LoadOrHideDebuggerTab()
    {
        if (!Developer.IsDev) return;

        var sav = PlayerPrefs.GetString("PersonWindow_Debugger");
        if (sav == "True")
        {
            _debugger_Text.text = BuildPersonInfo();
            _debugger.SetActive(true);
        }
        else
            _debugger.SetActive(false);
    }

    public void DebugWasDestSetToFalse()
    {
        Person1.Body.BodyAgent.DebugWasDestSetToFalse();
        Debug.Log("DebugWasDestSetToFalse()");
    }

    public void DebugMakePersonIsCartAni()
    {
        Person1.Body.TurnCurrentAniAndStartNew("isCart");
        Debug.Log("DebugMakePersonIsCartAni()");
    }
}