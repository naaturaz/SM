using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Commandable : UnitT
{
    Vector3 _agentDest;
    Vector3 _oldDest = Vector3.forward;


    float _distToDisable;
    MilitarBody _militarBody;

    // Use this for initialization
    protected void Start()
    {
        if (!name.Contains("Enemy"))
        {
            IsGood = true;
        }
        base.Start();

        // LoadBulletGO();
        _distToDisable = 0.1f;
        MilitarBody = new MilitarBody(gameObject);
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
        if (Time.time < 1.1f)
        {
            return;
        }

        //var good = Input.GetMouseButtonUp(1) && !Program.gameScene.BuildingManager.IsBuildingNow() &&
        //    Program.gameScene.CameraK.Hit.transform != null && SelectedGO != null && SelectedGO.activeSelf;

        if (1 == 1)
        {
            //_agentDest = Program.gameScene.CameraK.Hit.point;
            //var a = General.Create("Prefab/Debug/Sphere", Program.GameScene.CameraK.Hit.transform.position, "Debug");
            //ActivateAgent();
        }
        if (Enemy != null && _oldDest != _agentDest)
        {
            if (IsGood)
            {

            }
            else if (!IsGood && !CheckOnShortDest())
            {
                _agentDest = Enemy.position;
            }

            _oldDest = _agentDest;
            MilitarBody.ActivateAgent(_agentDest);
        }

        CheckOnShortDest();
    }

    public MilitarBody MilitarBody
    {
        get
        {
            return _militarBody;
        }
        set
        {
            _militarBody = value;
        }
    }

    bool CheckOnShortDest()
    {
        if (UMath.nearEqualByDistance(transform.position, _agentDest, _distToDisable))
        {
            MilitarBody.DisableAgent();
            return true;
        }
        return false;
    }

}
