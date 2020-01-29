using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// An Enemy will only have a MilitarBrain
/// 
/// People on the town on Combat Mode will only listen to their 
/// Militar Brain... 
/// 
/// Steps:
/// 1 - Get out of building if in one, and it sets it self in Idle
/// 2 - If engage on fight by an enemy or enemy in proximity, will fight back 
/// 3 - When enemies are all death will go back to house.
/// 4 - When in house, will reset Brain state
/// 5 - Will not listen to MilitarBrain anymore
/// 
/// The person will be listening to user commands once in ready to fight, so will go where directed and will fight 
/// if enemy in proximity
/// </summary>
public class MilitarBrain
{
    protected Person _person;

    protected CryRouteManager _cryRouteManager;
    protected TheRoute _theRoute;

    protected Structure _ini;
    protected Structure _fin;

    public MilitarBrain()
    {

    }

    protected void Update()
    {
        if (_cryRouteManager != null && !_cryRouteManager.IsRouteReady)
        {
            _cryRouteManager.Update();
        }
        else if (_theRoute == null && _cryRouteManager != null && _cryRouteManager.IsRouteReady)
        {
            _theRoute = _cryRouteManager.TheRoute;
        }

        MindState();
    }

    private void MindState()
    {
        if (_theRoute != null && _cryRouteManager.IsRouteReady && _person.Body.GoingTo == HPers.None)
        {
            _person.Body.WalkRoutine(_theRoute, HPers.Enemy);
        }
        else if(_person.Body.Location == HPers.Enemy)
        {
            _theRoute = null;
            _cryRouteManager = null;
        }
    }

    protected void SetMyDummyToMyCurrPosAndIni()
    {
        _person.MyDummy.LandZone1.Clear();
        _person.MyDummy.transform.position = _person.transform.position;
        _person.MyDummy.HandleLandZoning();

        _ini = _person.MyDummy;
    }

}

