using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PersonalObject
{
    private FollowObject _current;

    //as I spawn them Will add it here so can be reuse it for GC pupose
    private Dictionary<string, FollowObject> _allPersonalObjects = new Dictionary<string, FollowObject>();

    private string _currentRoot;
    private string _currentAni;

    private Person _person;

    private GameObject _currentPoint;
    private GameObject _rightHand;
    private GameObject _stomach;
    private GameObject _bip001Pelvis;

    //different roots for personc carrying stuff
    private Dictionary<P, string> _prodCarry = new Dictionary<P, string>() { };

    private Renderer renderer;

    public PersonalObject()
    {
    }

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
        _rightHand = General.FindGameObjectInHierarchy("RightHand", _person.gameObject);//Bip001 R Hand    RightHand
        _stomach = General.FindGameObjectInHierarchy("Stomach", _person.gameObject);//Bip001 Spine    Stomach
        _bip001Pelvis = General.FindGameObjectInHierarchy("Culo", _person.gameObject);//Bip001 Spine    Stomach
    }

    private void LoadCarrying()
    {
        _prodCarry.Add(P.Coal, Root.coal);
        _prodCarry.Add(P.Crate, Root.crate);
        _prodCarry.Add(P.Ore, Root.ore);
        _prodCarry.Add(P.Stone, Root.ore);
        _prodCarry.Add(P.Barrel, Root.tonel);
        _prodCarry.Add(P.Wood, Root.wood);

        _prodCarry.Add(P.Water, Root.tonel);
    }

    public void AddressNewAni(string newAni, bool hide)
    {
        //return;

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

            //for hammer so is shown
            Show();

            CheckIfHide(hide);
            return;
        }

        if (string.IsNullOrEmpty(_currentRoot))
        {
            return;
        }

        //ResetPersonPosition();

        _current = FollowObject.Create(_currentRoot, _currentPoint, //Program.PersonObjectContainer.transform,
            _currentPoint.transform,
            _person.MyId);

        _current.transform.rotation = _currentPoint.transform.rotation;
        _current.transform.position = _currentPoint.transform.position;

        _allPersonalObjects.Add(_currentRoot, _current);
        CheckIfHide(hide);

        //ReloadPersonPosition();
    }

    /// <summary>
    /// Bz this objects are not childs of _person . bz Transform child weird stuff
    /// </summary>
    public void DestroyAllGameObjs()
    {
        for (int i = 0; i < _allPersonalObjects.Count; i++)
        {
            if (_allPersonalObjects.ElementAt(i).Value != null)
            {
                _allPersonalObjects.ElementAt(i).Value.Destroy();
            }
        }
    }

    private GameObject _toFollow;
    private Quaternion _saveQuaternion;
    private Vector3 _savePosition;

    private void ResetPersonPosition()
    {
        _saveQuaternion = _person.transform.rotation;
        _savePosition = _person.transform.position;

        _person.transform.rotation = new Quaternion();
        _person.transform.position = new Vector3();
    }

    private void CheckIfHide(bool hide)
    {
        if (hide)
        {
            Hide();
        }
    }

    /// <summary>
    /// Sets personal object and where will spanw _currentPoint
    /// </summary>
    private void SetNewPersonalObject()
    {
        _currentPoint = _rightHand;

        if (_currentAni == "isHammer")
        {
            _currentRoot = Root.hammer;
        }
        else if (_currentAni == "isHoe")
        {
            _currentRoot = Root.hoe;
        }
        else if (_currentAni == "isAxe")
        {
            _currentRoot = Root.axe;
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
        //else if (_currentAni == "isBucket")
        //{
        //    _currentRoot = Root.bucket;
        //}
        else if (_currentAni == "isWheelBarrow")
        {
            _currentPoint = _person.gameObject;
            _currentRoot = DefineCurrentWheelBarrowRoot();
        }
        else if (_currentAni == "isCartRide")
        {
            _currentPoint = _bip001Pelvis;
            _currentRoot = DefineCurrentCartRoot();
            GameController.ResumenInventory1.Remove(P.Cart, .01f);
        }
        else _currentRoot = "";
    }

    private string DefineCurrentWheelBarrowRoot()
    {
        if (!_person.Inventory.IsEmpty())
        {
            if (_person.Inventory.IsCarryingLiquid())
            {
                return Root.wheelBarrowWithBarrels;
            }
            return Root.wheelBarrowWithBoxes;
        }
        return Root.wheelBarrow;
    }

    private List<string> _heavy = new List<string>()
    {
        //Root.wagon,
        Root.cart,
        //Root.conestogo
    };

    private int _myCart = -1;

    private string DefineCurrentCartRoot()
    {
        string res = "";
        if (_myCart == -1)
        {
            _myCart = UMath.GiveRandom(0, _heavy.Count);
        }

        res = _heavy[_myCart];

        if (!_person.Inventory.IsEmpty())
        {
            res += WithBoxesOrBarrels();
        }
        return res;
    }

    private string WithBoxesOrBarrels()
    {
        if (_person.Inventory.IsCarryingLiquid())
        {
            return "WithBarrels";
        }
        return "WithBoxes";
    }

    /// <summary>
    /// Only use for when people is carrying something
    /// </summary>
    private void SetRootForCarrying()
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

        if (_current != null
            //&&  _current.Renderer1 != null
            )
        {
            SetScaleOfCurrent();
            //so its shown at first close to the person
            _current.UpdatePosition();
            _current.gameObject.SetActive(true);

            //_current.Renderer1.enabled = true;
        }
    }

    /// <summary>
    /// bz youger guys carry boxes too
    /// </summary>
    private void SetScaleOfCurrent()
    {
        //bz all PersonalObjects were scaled initialiy for adults
        if (_person.Age > 20)
        {
            _current.ReloadOriginalObjectDim();
            return;
        }

        var dif = 20 - _person.Age;
        _current.ReloadOriginalObjectDim();
        ScaleGameObject(dif * -0.07f);
    }

    private void ScaleGameObject(float toAdd)
    {
        var localScale = _current.gameObject.transform.localScale;

        var final = localScale + new Vector3(toAdd, toAdd, toAdd);

        _current.gameObject.transform.localScale = final;
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

        if (_current != null// && _current.Renderer1 != null
            )
        {
            _current.gameObject.SetActive(false);
            //_current.Renderer1.enabled = false;
        }
    }

    internal void Reset()
    {
        Hide();
        _currentAni = "";
        _current = null;
    }
}