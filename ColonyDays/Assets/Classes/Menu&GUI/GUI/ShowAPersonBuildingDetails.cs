using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Given a Person or Building will Display Details
/// </summary>
public class ShowAPersonBuildingDetails
{
    private GameObject _containr;
    private Vector3 _iniPos;

    //this is the items they are a Key and a Value
    Dictionary<string, string> _items = new Dictionary<string, string>();
    private Person _person;
    List<PersonBuildingDetailTile> _tiles = new List<PersonBuildingDetailTile>();

    public List<PersonBuildingDetailTile> Tiles
    {
        get
        {
            return _tiles;
        }

        set
        {
            _tiles = value;
        }
    }

    public ShowAPersonBuildingDetails(Person per, GameObject container, Vector3 iniPos)
    {
        _iniPos = iniPos;
        _person = per;
        _containr = container;

        InitItems();
    }

    private void InitItems()
    {
        if (_person != null)
            InitPerson();
    }

    void CreateItemsPerson()
    {
        //return;

        _items.Clear();

        AddToItems("Age", _person.Age + "");
        AddToItems("Gender", _person.Gender + "");
        AddToItems("Height", Unit.ConvertFromCMToCurrent(_person.Height).ToString("n1") + "");
        AddToItems("Weight", Unit.WeightConverted(_person.Weight).ToString("n1") + "");
        AddToItems("Thirst", _person.Thirst);//thirst quenched
        AddToItems("Calories", _person.Nutrition1.CalNeededNowUpdate().ToString("N0") + "");
        AddToItems("Nutrition", _person.NutritionLevel + "");
        AddToItems("Profession", _person.ProfessionProp.ProfessionDescriptionToShow());
        AddToItems("Spouse", Family.GetPersonName(_person.Spouse));
        AddToItems("Happinness", _person.Happinnes.ToString("n1") + "");
        AddToItems("Years of school", _person.YearsOfSchool + "");
        AddToItems("House comfort", _person.Home.Comfort + "");

        //AddToItems("Age majority reach", _person.IsMajor + "");
        //AddToItems("Account", MyText.DollarFormat(_person.PersonBank1.CheckingAcct));

        AddToItems("Home", Home());
        AddToItems("Work", Work());
        AddToItems("Food source", Food());
        AddToItems("Religion", Religion());
        AddToItems("Relax", Chill());
    }

    void AddToItems(string key, string val)
    {
        _items.Add(key, val);
    }

    private void InitPerson()
    {
        CreateItemsPerson();
        ShowAllItems();
    }

    public void DestroyTiles()
    {

    }

    private void ShowAllItems()
    {
        var iForSpwItem = 0;//so ReturnIniPos works nicely

        for (int i = 0; i < _items.Count; i++)
        {
            _tiles.Add(PersonBuildingDetailTile.Create(_containr.transform, _items.ElementAt(i),
                ReturnIniPos(iForSpwItem), this, _person));
            iForSpwItem++;
        }
    }

    Vector3 ReturnIniPos(int i)
    {
        return new Vector3(_iniPos.x, ReturnY(i) + _iniPos.y, _iniPos.z);
    }

    private int _mainLines = 18;//24
    float ReturnX(int i)
    {
        return 0;
    }

    float ReturnY(int i)
    {
        return -3.6f * i;
    }

    string Home()
    {
        if (_person.Home != null)
        {
            return _person.Home.Name;
        }
        return "None";
    }

    string Work()
    {
        if (_person.Work != null)
        {
            return _person.Work.Name;
        }
        return "None";
    }

    string Food()
    {
        if (_person.FoodSource != null)
        {
            return _person.FoodSource.Name;
        }
        return "None";
    }

    string Religion()
    {
        if (_person.Religion != null)
        {
            return _person.Religion.Name;
        }
        return "None";
    }

    string Chill()
    {
        if (_person.Chill != null)
        {
            return _person.Chill.Name;
        }
        return "None";
    }

    private int count;



    /// <summary>
    /// it is updated every 20 calls 
    /// </summary>
    /// <param name="_person"></param>
    internal void ManualUpdate(Person person, bool forced = false)
    {
        count++;
        if (count > 20 || forced)//forced is used when a new person is clicked
        {
            _person = person;
            count = 0;
            CreateItemsPerson();
            UpdateTiles();
        }
    }

    private void UpdateTiles()
    {
        for (int i = 0; i < _tiles.Count; i++)
        {
            _tiles[i].ManualUpdate(_person, _items.ElementAt(i));
        }
    }
}
