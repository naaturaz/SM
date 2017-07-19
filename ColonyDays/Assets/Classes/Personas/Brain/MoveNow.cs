using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To address the problem in where some people houses are destroyed and they get stuck either in:
/// IdleSpot, Storage or Old House 
/// </summary>
public class MoveNow
{

    Person _person;

    float _destroyedAtGameTime;
    MDate _destroyedAt;
    Vector3 _fiveSecAfterDestroyedPos;
    CryRouteManager _router;

    public MoveNow(Person person)
    {
        _person = person;
    }

    // Update is called once per frame
    public void Update()
    {
        return;
        if (_person == null || _person.Home == null || !_person.IsMajor)
        {
            var a = _person.name;
            return;
        }
        else if (_person.Home.Instruction == H.WillBeDestroy && _destroyedAt == null)
        {
            _destroyedAt = Program.gameScene.GameTime1.CurrentDate();
            _destroyedAtGameTime = Time.time;
        }
        else if(_destroyedAt != null && Time.time > _destroyedAtGameTime + 5 && _fiveSecAfterDestroyedPos == new Vector3())
        {
            _fiveSecAfterDestroyedPos = _person.transform.position;
        }
        else if(_router == null && 
            _destroyedAt != null && Program.gameScene.GameTime1.ElapsedDateInDaysToDate(_destroyedAt) > 90)
        {
            if (UMath.nearEqualByDistance(_person.transform.position, _fiveSecAfterDestroyedPos, .1f))
            {
                CreateRoute();
            }
            else
            {
                ResetThis();
            }
        }
        else if(_router != null && !_router.IsRouteReady)
        {
            _router.Update();
        }
        else if(_router != null && _router.IsRouteReady && _person.Body.GoingTo != HPers.NowToNewHome)
        {
            //reached the house 
            if (_person.Body.GoingTo == HPers.MovingToNewHome)
            {
                ResetThis();
                return;
            }
            MoveNowToNewHouse();
        }
    }

    private void ResetThis()
    {
        //Debug.Log("ResetThis: " + _person.name);
        _destroyedAt = null;
        _destroyedAtGameTime = 0;
        _fiveSecAfterDestroyedPos = new Vector3();
        _router = null;
    }

    private void CreateRoute()
    {
        _person.MyDummy.transform.position = _person.transform.position;
        _person.MyDummy.HandleLandZoning();

        if (_person.Home.Instruction != H.WillBeDestroy)
        {
            _router = new CryRouteManager(_person.MyDummy, _person.Home, _person);
        }
        else
        {
            _router = new CryRouteManager(_person.MyDummy, Brain.GetStructureFromKey(_person.IsBooked), _person);
        }
    }

    private void MoveNowToNewHouse()
    {
        _person.Body.WalkRoutine(_router.TheRoute, HPers.NowToNewHome);
    }
}
