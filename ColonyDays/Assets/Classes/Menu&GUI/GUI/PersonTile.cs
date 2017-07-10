using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class PersonTile : GUIElement
{
    private Text _descText;
    private Text _valText;

    //private UnityEngine.UI.Button _showLoc;

    private Person _person;

    public Person Person1
    {
        get { return _person; }
        set { _person = value; }
    }


    internal static PersonTile Create(Transform container, Vector3 iniPos, Person person)
    {
        PersonTile obj = null;

        var root = "";

        obj = (PersonTile)Resources.Load(Root.person_Tile, typeof(PersonTile));
        obj = (PersonTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var iniScale = obj.transform.localScale;
        obj.transform.SetParent(container);
        obj.transform.localPosition = iniPos;
        obj.transform.localScale = iniScale;

        obj.Person1 = person;

        return obj;
    }

    void Start()
    {
        //_showLoc = FindGameObjectInHierarchy("ShowLocation", gameObject).GetComponent<UnityEngine.UI.Button>();

        //set values on the Tile
        _descText = FindGameObjectInHierarchy("Item_Desc", gameObject).GetComponent<Text>();
        _valText = FindGameObjectInHierarchy("Item_Value", gameObject).GetComponent<Text>();

        var goBtnLoc = FindGameObjectInHierarchy("ShowLocation", gameObject);

    }

    void Update()
    {
        _descText.text = _person.Name;
        _valText.text = _person.Age + "";
    }

    //Called from GUI
    public void SelectThisPerson()
    {
        Transform t;

        //means is a kids
        //doing this bz they are in -10 in y.... didnt look for cause, I just fixed  
        if (_person.transform.localPosition.y < 0)
        {
            t = _person.Home.transform;
        }
        else
        {
            t = _person.transform;
        }
        Program.MouseListener.SelectPerson(_person);
        CamControl.CAMRTS.InputRts.CenterCamTo(t);
    }

}

