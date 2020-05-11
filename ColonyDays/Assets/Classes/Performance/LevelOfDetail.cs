﻿using UnityEngine;

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

    private void SetLOD()
    {
        _bip = General.GetChildCalledOnThis("Bip001", _gO);
    }

    private void LODCheck()
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

    private H ReturnCurrentLOD()
    {
        //var dist = Vector3.Distance(_gO.transform.position, Camera.main.transform.position);
        var dist = Vector3.Distance(_gO.transform.position, CamControl.CAMRTS.transform.position);

        if (dist > CloserDistLOD1() && dist <= 76)//66
        {
            return H.LOD1;
        }
        if (dist > 75)//65      75
        {
            return H.LOD2;
        }
        return H.LOD0;
    }

    private float CloserDistLOD1()
    {
        if (_type == H.Person)
        {
            return 42f;//32
        }
        return 25;
    }

    private void LOD1()
    {
        _bip.SetActive(false);
        //_geometry.SetActive(true);
    }

    private void LOD2()
    {
        //_geometry.SetActive(false);
    }

    private void LOD3()
    {
        _geometry.SetActive(false);
    }

    private void LODBest()
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