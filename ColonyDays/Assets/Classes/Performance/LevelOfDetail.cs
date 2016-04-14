using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LevelOfDetail
{
    private H _type;
    private GameObject _gO;
    private GameObject _geometry;
    private GameObject _bip;
    private H _oldLOD;

    private OutOfScreen _outOfScreen;


    public LevelOfDetail(Person person)
    {
        _gO = person.gameObject;
        _type = H.Person;
        _geometry = person.Geometry;
        _outOfScreen = new OutOfScreen(person);

        SetLOD();
    }

    public LevelOfDetail(Animal animal)
    {
        _gO = animal.gameObject;
        _type = H.Animal;
        _geometry = animal.Geometry;
        _outOfScreen = new OutOfScreen(animal);

        SetLOD();
    }

    public OutOfScreen OutOfScreen1
    {
        get { return _outOfScreen; }
    }

    void SetLOD()
    {
        _bip = General.GetChildCalledOnThis( "Bip001", _gO);
    }



    void LODCheck()
    {
        var newLOD = ReturnCurrentLOD();

        if (_oldLOD == newLOD || _bip == null)
        {
            return;
        }

        _oldLOD = newLOD;
        _outOfScreen.SetNewLOD(newLOD);

        if (newLOD == H.LOD1)
        {
            LOD1();
        }
        else if (newLOD == H.LOD2)
        {
            LOD2();
        }
        else
        {
            LODBest();
        }
    }

    H ReturnCurrentLOD()
    {
        //var dist = Vector3.Distance(_gO.transform.position, Camera.main.transform.position);
        var dist = Vector3.Distance(_gO.transform.position, CamControl.CAMRTS.transform.position);

        if (dist > CloserDistLOD1() && dist <= 66)//25 66     35 66     45 76
        {
            return H.LOD1;
        }
        if (dist > 65)//65      75
        {
            return H.LOD2;
        }
        return H.LOD0;
    }

    float CloserDistLOD1()
    {
        if (_type == H.Person)
        {
            return 32f;
        }
        return 25;
    }

    void LOD1()
    {
        _bip.SetActive(false);
        //_geometry.SetActive(true);
    }

    void LOD2()
    {
        //_geometry.SetActive(false);
    }

    void LOD3()
    {
        _geometry.SetActive(false);
    }

    void LODBest()
    {
        _bip.SetActive(true);
        //_geometry.SetActive(true);
    }

    public void A45msUpdate()
    {
        _outOfScreen.A45msUpdate();
        LODCheck();
    }
}

