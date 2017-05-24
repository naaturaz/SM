using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        {
            InitPerson();
        }
    }

    void CreateItemsPerson()
    {
        //return;

        _items.Clear();

        AddToItems("Age", _person.Age + "");
        AddToItems("Gender", _person.Gender + "");
        AddToItems("Height", Unit.ConvertFromCMToCurrent( _person.Height).ToString("n1") + "");
        AddToItems("Weight",  Unit.WeightConverted(_person.Weight).ToString("n1") + "");
        AddToItems("Thirst", _person.Thirst);//thirst quenched
        AddToItems("Calories", _person.Nutrition1.CalNeededNowUpdate().ToString("N0") + "");
        AddToItems("Nutrition", _person.NutritionLevel + "");
        AddToItems("Profession", _person.ProfessionProp.ProfDescription + "");
        AddToItems("Spouse", Family.GetPersonName(_person.Spouse));
        AddToItems("Happinness", _person.Happinnes.ToString("n1") + "");
        AddToItems("Years Of School", _person.YearsOfSchool + "");
        //AddToItems("Age majority reach", _person.IsMajor + "");
        AddToItems("Account", _person.PersonBank1.CheckingAcct.ToString("C"));

        AddToItems("Home", Home());
        AddToItems("Work", Work());
        AddToItems("Food Source", Food());
        AddToItems("Religion", Religion());
        AddToItems("Chill", Chill());
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
            return _person.Home.HType+"";
        }
        return "None";
    }  

    string Work()
    {
        if (_person.Work != null)
        {
            return _person.Work.HType+"";
        }
        return "None";
    }   
    
    string Food()
    {
        if (_person.FoodSource != null)
        {
            return _person.FoodSource.HType+"";
        }
        return "None";
    }   
    
    string Religion()
    {
        if (_person.Religion != null)
        {
            return _person.Religion.HType + "";
        }
        return "None";
    }  
    
    string Chill()
    {
        if (_person.Chill != null)
        {
            return _person.Chill.HType + "";
        }
        return "None";
    }

    private int count;
    /// <summary>
    /// //its been called every 6 frames
    /// </summary>
    /// <param name="_person"></param>
    internal void ManualUpdate(Person person, bool forced = false)
    {
        count++;
        if (count> 20 || forced)//forced is used when a new person is clicked
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
