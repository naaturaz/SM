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
        _items.Clear();

        _items.Add("Age", _person.Age + "");
        _items.Add("Gender", _person.Gender + "");
        _items.Add("Height", _person.Height + "");
        _items.Add("Weight", _person.Weight + "");
        _items.Add("Calories", _person.Nutrition1.CalNeededNowUpdate().ToString("N0") + "");
        _items.Add("Nutrition", _person.NutritionLevel + "");
        _items.Add("Profession", _person.ProfessionProp.ProfDescription + "");
        _items.Add("Spouse", Family.RemovePersonIDNumberOff(_person.Spouse));
        _items.Add("Happinness", _person.Happinnes + "");
        _items.Add("Years Of School", _person.YearsOfSchool + "");
        _items.Add("Age majority reach", _person.IsMajor + "");

        _items.Add("Home", Home());
        _items.Add("Work", Work());
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
        return -3.4f * i;//-3.5
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
