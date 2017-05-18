using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Commandable : UnitT
{
    NavMeshAgent _agent;
    float _initialAgentSpeed;

    Vector3 _agentDest;
    float _startTime;
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

        Agent = GetComponent<NavMeshAgent>();
        _initialAgentSpeed = Agent.speed;

        _startTime = Time.time;

       // LoadBulletGO();
        _distToDisable = UMath.GiveRandom(0.4f + Program.gameScene.UnitsManager.Units.Count / 200,
            1.2f + Program.gameScene.UnitsManager.Units.Count / 200);

        _militarBody = new MilitarBody(gameObject);

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

        if (1==1)
        {
            //_agentDest = Program.gameScene.CameraK.Hit.point;
            //var a = General.Create("Prefab/Debug/Sphere", Program.GameScene.CameraK.Hit.transform.position, "Debug");
            //ActivateAgent();
        }
        if (Enemy != null)
        {
            ActivateAgent();
            transform.LookAt(Enemy);

            if (IsGood)
            {

            }
            else if(!IsGood && !CheckOnShortDest())
            {
                _agentDest = Enemy.position;
            }
        }

        CheckOnShortDest();
    }


    static float _lastScream;

    public NavMeshAgent Agent
    {
        get
        {
            return _agent;
        }

        set
        {
            _agent = value;
        }
    }

    void ActivateAgent()
    {
        if (Time.time > _startTime + 10 && Time.time > _lastScream + 10 && !Agent.enabled)
        {
            //scream
            //Program.GameScene.SoundManager.PlaySound(7, 0.05f);
            _lastScream = Time.time;
        }

        Agent.enabled = true;

        if (Agent.isOnNavMesh)
        {
            if (!IsGood)
            {
                Agent.SetDestination(_agentDest + new Vector3(5, 0, 5));
                _militarBody.Run();
            }
            else
            {
                Agent.SetDestination(_agentDest);
                _militarBody.Run();

            }
        }
        else
        {
            Program.gameScene.UnitsManager.RemoveUnit(this);
            Destroy(gameObject);
        }

    }





    bool CheckOnShortDest()
    {
        if (UMath.nearEqualByDistance(transform.position, _agentDest, _distToDisable))
        {
            Agent.enabled = false;
            return true;
        }
        return false;
    }

}
