using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerTile : GUIElement
{
    private string _workType;

    public string WorkType
    {
        get { return _workType; }
        set { _workType = value; }
    } 
    private Text _descText;
    private Text _totalText;
    private Text _currentText;

    List<Structure> _buildings;
    int _employ = -1;//total employ by this types of works
    int _oldEmploy = -1;

    GameObject _plusBtn;
    GameObject _lessBtn;

    internal static WorkerTile CreateTile(Transform container,
    string workType, Vector3 iniPos)
    {
        WorkerTile obj = null;

        var root = "";

        obj = (WorkerTile)Resources.Load(Root.worker_Tile, typeof(WorkerTile));
        obj = (WorkerTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var iniScale = obj.transform.localScale;
        obj.transform.SetParent(container);
        obj.transform.localPosition = iniPos;
        obj.transform.localScale = iniScale;

        obj.WorkType = workType;

        return obj;
    }

    void Start()
    {
        _descText = FindGameObjectInHierarchy("Item_Desc", gameObject).GetComponent<Text>();
        _totalText = FindGameObjectInHierarchy("Total", gameObject).GetComponent<Text>();
        _currentText = FindGameObjectInHierarchy("Current_Amount", gameObject).GetComponent<Text>();

        var goP = GetChildCalled("More");
        var goL = GetChildCalled("Less");
        
        _plusBtn = GetChildCalled("More");
        _lessBtn = GetChildCalled("Less");

        _descText.text = Languages.ReturnString(_workType);

        Init();
    }

    private void Init()
    {
        if (_buildings == null)
        {
            _buildings = BuildingController.FindAllStructOfThisTypeAndFullyBuilt(_workType);
        }

        if (_employ == -1)
        {
            _employ = MaxPeople();
        }

        if (_oldEmploy != _employ && _oldEmploy != -1)
        {
            TakeActionOnNewNumber();
            _employ = MaxPeople();
        }
        _oldEmploy = _employ;

        _currentText.text = _employ + "";
        _totalText.text = "" + AbsMaxPeople()+"";
    }

    private void TakeActionOnNewNumber()
    {
        if (_oldEmploy > _employ)
        {
            FirePeople();
        }
        else
        {
            HirePeople();
        }
    }

    private void HirePeople()
    {
        var newHires = _employ - _oldEmploy;
        ChangeEmployesBy(newHires);
    }

    private void ChangeEmployesBy(int newEmploys)
    {
        if (newEmploys == 0)
        {
            return;
        }

        var indexBuild = UMath.GiveRandom(0, _buildings.Count);
        var build = _buildings[indexBuild];
        newEmploys = build.CanYouChangeOne(newEmploys);

        ChangeEmployesBy(newEmploys);
    }

    private void FirePeople()
    {
        var firePeople = _oldEmploy - _employ;
        ChangeEmployesBy(-firePeople);
    }

    int MaxPeople()
    {
        int res = 0;
        for (int i = 0; i < _buildings.Count; i++)
        {
            res += _buildings[i].MaxPeople;
        }
        return res;
    }

    int AbsMaxPeople()
    {
        int res = 0;
        for (int i = 0; i < _buildings.Count; i++)
        {
            res += _buildings[i].AbsMaxPeople;
        }
        return res;
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ClickLessSign()
    {
        _employ--;
        Init();
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void ClickPlusSign()
    {
        if (WorkType == "Masonry")
            Program.gameScene.TutoStepCompleted("AddWorkers.Tuto");

        _employ++;
        Init();
    }

    private void CheckIfPlusIsActive()
    {
        if (MaxPeople() >= AbsMaxPeople() || MyText.Lazy() == 0)
        {
            MakeInactiveButton(_plusBtn);
        }
        else
        {
            MakeActiveButton(_plusBtn);
        }
    }

    private void CheckIfLessIsActive()
    {
        if (MaxPeople() == 0)
        {
            MakeInactiveButton(_lessBtn);
        }
        else
        {
            MakeActiveButton(_lessBtn);
        }
    }

    private void MakeInactiveButton(GameObject btn)
    {
        btn.SetActive(false);
    }

    private void MakeActiveButton(GameObject btn)
    {
        btn.SetActive( true);
    }

    void Update()
    {
        if (_buildings==null)
        {
            return;
        }
        CheckIfLessIsActive();
        CheckIfPlusIsActive();
    }
}
