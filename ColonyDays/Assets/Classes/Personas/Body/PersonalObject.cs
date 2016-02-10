using System.Collections.Generic;
using UnityEngine;

public class PersonalObject
{
    private General _current;
    //as I spawn them Will add it here so can be reuse it for GC pupose
    private Dictionary<string, General> _allPersonalObjects = new Dictionary<string, General>(); 

    private string _currentRoot;
    private string _currentAni;

    private Person _person;

    private GameObject _currentPoint;
    private GameObject _rightHand;
    private GameObject _stomach;
    //different roots for personc carrying stuff
    Dictionary<P, string> _prodCarry = new Dictionary<P, string>() { };

    private Renderer renderer;


    public PersonalObject() { }

    /// <summary>
    /// NEw Obj
    /// </summary>
    /// <param name="person"></param>
    public PersonalObject(Person person)
    {
        _person = person;
        Init();
    }

    /// <summary>
    /// Used to loading
    /// </summary>
    /// <param name="person"></param>
    /// <param name="currAni"></param>
    public PersonalObject(Person person, string currAni, bool hide)
    {
        _person = person;
        Init(); 
        AddressNewAni(currAni, hide);
    }

    private void Init()
    {
        LoadCarrying();
        _rightHand = General.FindGameObjectInHierarchy("RightHand", _person.gameObject);
        _stomach = General.FindGameObjectInHierarchy("Stomach", _person.gameObject);
    }

    void LoadCarrying()
    {
        _prodCarry.Add(P.Coal, Root.coal);
        _prodCarry.Add(P.Crate, Root.crate);
        _prodCarry.Add(P.Ore, Root.ore);
        _prodCarry.Add(P.Tonel, Root.tonel);
        _prodCarry.Add(P.Wood, Root.wood);
    }


    public void AddressNewAni(string newAni, bool hide)
    {
        Hide();//will hide current 


        _currentAni = newAni;
        SetNewPersonalObject();
        AddressNewCurrentRoot(hide);
    }

    private void AddressNewCurrentRoot(bool hide)
    {
        //means was used already once . could have this object we are looking for spawnerd 
        if (_current != null)
        {
            _current = null;
        }

        if (_allPersonalObjects.ContainsKey(_currentRoot))
        {
            _current = _allPersonalObjects[_currentRoot];
            CheckIfHide(hide);
            return;
        }

        if (string.IsNullOrEmpty(_currentRoot))
        {
            return;
        }
        
        _current = General.Create(_currentRoot, _currentPoint.transform.position, "", _currentPoint.transform);
        _current.transform.rotation = _currentPoint.transform.rotation;
        _allPersonalObjects.Add(_currentRoot, _current);

        CheckIfHide(hide);
    }

    void CheckIfHide(bool hide)
    {
        if (hide)
        {
            Hide();
        }
    }

    /// <summary>
    /// Sets personal object and where will spanw _currentPoint
    /// </summary>
    void SetNewPersonalObject()
    {
        _currentPoint = _rightHand;

        if (_currentAni == "isHammer")
        {
            _currentRoot = Root.hammer;
        }
        else if (_currentAni == "isFarming")
        {
            _currentRoot = Root.guataca;
        }
        else if (_currentAni == "isFishing")
        {
            _currentRoot = Root.vara;
        }
        else if (_currentAni == "isCarry")
        {
            _currentPoint = _stomach;
            SetRootForCarrying();
        }
        else _currentRoot = "";
    }

    /// <summary>
    /// Only use for when people is carrying something 
    /// </summary>
    void SetRootForCarrying()
    {
        P prod = P.None;
        //find the prod is carrying in person inv
        if (!_person.Inventory.IsEmpty())
        {
            prod = _person.Inventory.InventItems[0].Key;
        }

        if (_prodCarry.ContainsKey(prod))
        {
            //find the root, set it
            _currentRoot = _prodCarry[prod];     
        }
        //has not val in _prodCarry 
        //then will assign crate
        else
        {
            _currentRoot = Root.crate;
        }
    }

    internal void Show()
    {
        if (_current != null && _current.Renderer1 == null)
        {
            var gO = General.FindGameObjectInHierarchy("Geometry", _current.gameObject);
            if (gO != null)
            {
                _current.Renderer1 = gO.GetComponent<Renderer>();
            }
        }

        if (_current != null &&  _current.Renderer1 != null)
        {
            _current.Renderer1.enabled = true;
        }
    }

    internal void Hide()
    {
        if (_current != null && _current.Renderer1 == null)
        {
            var gO = General.FindGameObjectInHierarchy("Geometry", _current.gameObject);
            if (gO != null)
            {
                _current.Renderer1 = gO.GetComponent<Renderer>();
            }
        }

        if (_current != null && _current.Renderer1 != null)
        {
            _current.Renderer1.enabled = false;
        }
    }
}
