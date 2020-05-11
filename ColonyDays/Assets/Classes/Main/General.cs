﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class General : MonoBehaviour
{
    #region Field and Properties

    private string _name;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    private Renderer _renderer;

    public Renderer Renderer1
    {
        get { return _renderer; }
        set { _renderer = value; }
    }

    //Shortcuts to Mesch Controller
    private SMe sMe = new SMe();

    public SMe m
    {
        get { return sMe; }
        set { sMe = value; }
    }

    private SPr sPr = new SPr();

    public SPr p
    {
        get { return sPr; }
        set { sPr = value; }
    }

    private SBu sBu = new SBu();

    public SBu b
    {
        get { return sBu; }
        set { sBu = value; }
    }

    //the geometry of the obj... for work the obj always need the 3d model be called Geometry
    private GameObject _geometry = null;

    //should be fisrst assigned in the class is gonna be used
    private Color _initialColor = Color.gray; //initial tint color geometry gameobj had

    public Color InitialColor
    {
        get { return _initialColor; }
        set { _initialColor = value; }
    }

    /// <summary>
    /// Geomtry GameObject on the Prefab
    /// if is null we defined first.. other wise is just straight forward property
    /// If 'Geomtrey' doesnt exist in the Prefab will use 'Main'
    /// </summary>
    public GameObject Geometry
    {
        get
        {
            if (_geometry == null)
            {
                _geometry = GetChildCalled(H.Geometry);

                //here im overriding Geomtrey bz I have a lot of objects that their Geometry 3d obj name
                //is far from Geomtry is the Scene Name + Main
                if (_geometry == null)
                {
                    _geometry = GetChildLastWordIs(H.Main);
                }
            }
            return _geometry;
        }
        set
        {
            if (_geometry == null)
            {
                _geometry = GetChildCalled(H.Geometry);
                if (_geometry == null)
                {
                    _geometry = GetChildLastWordIs(H.Main);
                }
            }
            _geometry = value;
        }
    }

    //USE it to initialize stuff, flagged tru in the Create() method
    public static bool WAKEUP = false;

    //so it can be seen from the inspector. any sort of info needed to be displayed
    public string info;

    private string _dummyIdSpawner;

    //if is a dummy here u can place the ID of the real object spawned the Dummy
    public string DummyIdSpawner
    {
        get { return _dummyIdSpawner; }
        set { _dummyIdSpawner = value; }
    }

    ////Constructor////
    public General()
    { AutoNumber(); }

    //variables
    private bool _isActive = false;

    private Vector3 _initalPosition = new Vector3();
    private string _root;
    private int _id = 0;

    private string _type;
    private bool _positionFixed;

    private string _myId;//this is the id use for the game Registry
    private string _batchRegionId;//the id where this was batched in, in BatchManager
    private H _hType;
    private Ca _category;

    private int _indexAllVertex;//correspondent index of subMesh AllVertex List

    private AudioReporter _audioReporter;

    public int IndexAllVertex
    {
        get { return _indexAllVertex; }
        set { _indexAllVertex = value; }
    }

    public H HType
    {
        get { return _hType; }
        set { _hType = value; }
    }

    public string MyId
    {
        get { return _myId; }
        set { _myId = value; }
    }

    public Vector3 InitialPosition
    {
        get { return _initalPosition; }
        set { _initalPosition = value; }
    }

    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }

    public bool IsActive
    {
        get { return _isActive; }
        set { _isActive = value; }
    }

    public string Type
    {
        get { return _type; }
        set { _type = value; }
    }

    public bool PositionFixed
    {
        get { return _positionFixed; }
        set { _positionFixed = value; }
    }

    public Ca Category
    {
        get { return _category; }
        set { _category = value; }
    }

    //the start rotation facing for builginds. is created and used so building rotation is remembebr on game.
    //So user can have the same rotation on building that last one had
    protected static int _univRotationFacer;

    public static int UnivRotationFacer
    {
        get { return _univRotationFacer; }
        set { _univRotationFacer = value; }
    }

    #endregion Field and Properties

    ////Methods////

    //create method
    static public General Create(string root, Vector3 origen = new Vector3(), string name = "", Transform container = null,
        H hType = H.None)
    {
        WAKEUP = true;
        General obj = null;
        obj = (General)Resources.Load(root, typeof(General));
        obj = (General)Instantiate(obj, origen, Quaternion.identity);
        obj.HType = hType;
        obj.transform.name = obj._myId = obj.Rename(obj.transform.name, obj.Id, obj.HType, name);

        if (container != null) { obj.transform.SetParent(container); }
        return obj;
    }

    public void DefineNameAndMyID()
    {
        transform.name = _myId = Rename(transform.name, Id, HType, name);
    }

    public Ca DefineCategory(H hTypeP)
    {
        Ca res = Ca.None;
        if (hTypeP == H.Trail ||/* hTypeP == H.Road ||*/ hTypeP == H.BridgeTrail || hTypeP == H.BridgeRoad)
        {
            res = Ca.Way;
        }
        else if (
            //hTypeP == H.Farm ||
            hTypeP == H.StockPile || hTypeP == H.Road)
        {
            res = Ca.DraggableSquare;
        }
        else if (hTypeP == H.Dock || hTypeP == H.Shipyard || hTypeP == H.Supplier
            || hTypeP == H.FishingHut
            || hTypeP == H.ShoreMine)
        {
            res = Ca.Shore;
        }
        else if (hTypeP == H.None || hTypeP == H.BridgeRoadUnit || hTypeP == H.BridgeTrailUnit)
        {
            res = Ca.None;
        }
        else if (hTypeP == H.Tree || hTypeP == H.Stone || hTypeP == H.Iron || hTypeP == H.Gold
            || hTypeP == H.Ornament || hTypeP == H.Grass || hTypeP == H.Decoration
            || hTypeP == H.Marine || hTypeP == H.Mountain)
        {
            res = Ca.Spawn;
        }
        else if (hTypeP == H.Person) res = Ca.Person;
        else res = Ca.Structure;

        return res;
    }

    public string Rename(string objDefaultName, int objId, H objHType, string newNameIfExist = "")
    {
        string res = "";
        if (newNameIfExist != "")
        {
            res = newNameIfExist;
        }
        else
        {
            if (objDefaultName.Length > 7)
            {
                //removeing the  '(clone)'
                objDefaultName = objDefaultName.Remove(objDefaultName.Length - 7);
            }
            res = objDefaultName + " | " + objHType + " | " + objId;
        }
        //print("Rename:"+ res );
        return res;
    }

    //autoNumber function
    private void AutoNumber()
    {
        Program.UNIVERSALID = Program.UNIVERSALID + 1;
        _id = Program.UNIVERSALID;
    }

    /// <summary>
    /// This is a static function not related to General ID
    /// </summary>
    /// <returns></returns>
    public static int GiveMeAutoNumber()
    {
        Program.UNIVERSALID++;
        return Program.UNIVERSALID;
    }

    protected static int GiveRandom(int min, int max)
    {
        min = Random.Range(min, max);
        return min;
    }

    // Use this for initialization
    protected void Start()
    {
        _category = DefineCategory(_hType);

        if (AudioCollector.Roots.ContainsKey(HType + ""))
        {
            _audioReporter = new AudioReporter(this);
        }

        StartCoroutine("TwoSecUpdate");
    }

    private IEnumerator TwoSecUpdate()
    {
        while (true && _audioReporter != null)
        {
            yield return new WaitForSeconds(2); // wait
            _audioReporter.A2SecUpdate();
        }
    }

    protected void Update()
    {
    }

    /// <summary>
    /// Get the grand child obj called "" in this Transform
    /// </summary>
    /// <returns></returns>
    protected GameObject GetGrandChildCalled(H grandName)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            var child = gameObject.transform.GetChild(i);
            for (int j = 0; j < child.transform.childCount; j++)
            {
                var grandChild = child.transform.GetChild(j).gameObject;
                if (grandChild.name == grandName.ToString())
                {
                    return grandChild;
                }
            }
        }
        //print("Obj doesnt have a child called: " + childName );
        return null;
    }

    /// <summary>
    /// Get the grand child obj called "" in this Transform
    /// </summary>
    /// <returns></returns>
    protected GameObject GetGrandChildCalled(string grandName)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            var child = gameObject.transform.GetChild(i);
            for (int j = 0; j < child.transform.childCount; j++)
            {
                var grandChild = child.transform.GetChild(j).gameObject;
                if (grandChild.name == grandName)
                {
                    return grandChild;
                }
            }
        }
        //print("Obj doesnt have a child called: " + childName );
        return null;
    }

    /// <summary>
    /// Get the grand child obj called "" in the param 'thisGameObject'
    /// </summary>
    /// <returns></returns>
    protected GameObject GetGrandChildCalledFromThis(string grandName, GameObject thisGameObject)
    {
        for (int i = 0; i < thisGameObject.transform.childCount; i++)
        {
            var child = thisGameObject.transform.GetChild(i);
            for (int j = 0; j < child.transform.childCount; j++)
            {
                var grandChild = child.transform.GetChild(j).gameObject;
                if (grandChild.name == grandName)
                {
                    return grandChild;
                }
            }
        }
        //print("Obj doesnt have a child called: " + childName );
        return null;
    }

    /// <summary>
    /// Get the child obj called "" in this Transform
    /// </summary>
    /// <returns></returns>
    public static GameObject GetChildCalledOnThis(string childName, GameObject thisGameOb)
    {
        for (int i = 0; i < thisGameOb.transform.childCount; i++)
        {
            if (thisGameOb.transform.GetChild(i).name == childName.ToString())
            {
                return thisGameOb.transform.GetChild(i).gameObject;
            }
        }
        //print("Obj doesnt have a child called: " + childName );
        return null;
    }

    /// <summary>
    /// Get the child obj called "" in this Transform
    /// </summary>
    /// <returns></returns>
    protected GameObject GetChildCalled(H childName)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name == childName.ToString())
            {
                return gameObject.transform.GetChild(i).gameObject;
            }
        }
        //print("Obj doesnt have a child called: " + childName );
        return null;
    }

    /// <summary>
    /// Get the child obj called "" in this Transform
    /// </summary>
    /// <returns></returns>
    protected GameObject GetChildCalled(string childName)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name == childName)
            {
                return gameObject.transform.GetChild(i).gameObject;
            }
        }
        //print("Obj doesnt have a child called: " + childName );
        return null;
    }

    /// <summary>
    /// Get the child obj called "" in this Transform
    /// </summary>
    public static List<GameObject> GetChildsNameEqual(GameObject gO, string childName)
    {
        List<GameObject> res = new List<GameObject>();
        for (int i = 0; i < gO.transform.childCount; i++)
        {
            if (gO.transform.GetChild(i).name == childName)
            {
                res.Add(gO.transform.GetChild(i).gameObject);
            }
        }
        return res;
    }

    public static List<GameObject> GetAllChilds(GameObject gO)
    {
        List<GameObject> res = new List<GameObject>();
        for (int i = 0; i < gO.transform.childCount; i++)
        {
            res.Add(gO.transform.GetChild(i).gameObject);
        }
        return res;
    }

    /// <summary>
    /// Be carefull u name too things apparently different but they end with the exact same
    /// string, That will giv u bugg
    /// </summary>
    protected GameObject GetChildLastWordIs(H childName)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            int subStringStartsAt = gameObject.transform.GetChild(i).name.Length - childName.ToString().Length;
            if (subStringStartsAt > -1)//this is here so we dont check a word that is actually smallr thant the param
            {
                string last = gameObject.transform.GetChild(i)
                    .name.Substring(subStringStartsAt, childName.ToString().Length);

                if (last == childName.ToString())
                {
                    return gameObject.transform.GetChild(i).gameObject;
                }
            }
        }
        //print("Obj doesnt contain a child with: " + childName);
        return null;
    }

    /// <summary>
    /// Get the child obj called "" in this Transform
    /// </summary>
    /// <returns></returns>
    protected GameObject GetChildThatContains(H childName)
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name.Contains(childName.ToString()))
            {
                return gameObject.transform.GetChild(i).gameObject;
            }
        }
        //print("Obj doesnt have a child that contains: " + childName);
        return null;
    }

    public static GameObject GetChildThatContains(string childName, GameObject go)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).name.Contains(childName))
            {
                return go.transform.GetChild(i).gameObject;
            }
        }
        //print("Obj doesnt have a child that contains: " + childName);
        return null;
    }

    /// <summary>
    /// will add a Zero to the end of the MyId
    /// </summary>
    public void AddZeroToMyID() { MyId += "0"; }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }

    public void DestroySafe()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    public virtual void DestroyCool()
    {
        Destroy(gameObject);
    }

    /////Persons and Buldlings Share

    //this is what a person carries with them
    private Inventory _inventory;

    public Inventory Inventory
    {
        get { return _inventory; }
        set { _inventory = value; }
    }

    public string BatchRegionId
    {
        get { return _batchRegionId; }
        set { _batchRegionId = value; }
    }

    #region Search GameObj in GameObject until find it

    static private int count;
    private static List<GameObject> list = new List<GameObject>();
    private static string toFind;
    private static GameObject result;

    public static GameObject FindGameObjectInHierarchy(string find, GameObject gameO)
    {
        toFind = find;
        AddToList(gameO);
        RecuLoop();

        return result;
    }

    private static void AddToList(GameObject gameO)
    {
        for (int i = 0; i < gameO.transform.childCount; i++)
        {
            list.Add(gameO.transform.GetChild(i).gameObject);
        }
    }

    private static void RecuLoop()
    {
        if (count > 1500) { throw new Exception("infinete loop general.cs"); }

        if (count < list.Count)
        {
            if (list[count].transform.childCount > 0)
            {
                AddToList(list[count]);
            }
            count++;
            //recursive
            RecuLoop();
        }
        else
        {
            FindResult();
        }
    }

    private static void FindResult()
    {
        allChilds = list.ToArray();
        result = list.Find(a => a.transform.name == toFind);

        count = 0;
        list.Clear();
        toFind = "";
    }

    private static GameObject[] allChilds;

    //--
    public static GameObject[] FindAllChildsGameObjectInHierarchy(GameObject gameO)
    {
        AddToList(gameO);
        RecuLoop();

        return allChilds;
    }

    public static GameObject[] FindAllChildsGameObjectInHierarchyContain(GameObject gameO, string pass)
    {
        return FindAllChildsGameObjectInHierarchy(gameO).Where(a => a.name.Contains(pass)).ToArray();
    }

    public void AssignToAllGeometryAsSharedMat(GameObject gameO, string MaterialKey)
    {
        var childs = FindAllChildsGameObjectInHierarchy(gameObject);

        for (int i = 0; i < childs.Length; i++)
        {
            var obj = childs[i];
            var render = obj.GetComponent<Renderer>();

            if (render != null)
            {
                render.sharedMaterial = Resources.Load(Root.RetMaterialRoot(MaterialKey)) as Material;
            }
        }
    }

    #endregion Search GameObj in GameObject until find it

    //toy army

    private bool _isGood = true;

    public bool IsGood
    {
        get
        {
            return _isGood;
        }

        set
        {
            _isGood = value;
        }
    }

    /// <summary>
    /// most ve a 3dText attached
    /// </summary>
    /// <param name="newText"></param>
    internal void SetText(string newText)
    {
        var t = GetComponent<TextMesh>();
        t.text = newText + "";
    }

    protected void ShowText(string pass)
    {
        var a = General.Create("Prefab/TA/GUI/3dText", transform.position, "3dText");
        a.SetText(pass);
    }
}