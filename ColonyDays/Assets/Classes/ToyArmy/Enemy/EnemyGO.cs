using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System;
using UnityStandardAssets.Utility;

public class EnemyGO : Shooter
{
    float _speed = 0.04f;
    NavMeshAgent _agent;

    public bool DebugWalk;
    int _leftRewards;

    GameObject _marker;


    GameObject _base;
    bool _didTargetRocket;
    Vector3 _targetPos;

    // Use this for initialization
    void Start()
    {
        StartCoroutine("OneSecUpdate");

        _base = BuildingController.FindTheClosestOfContainTypeFullyBuilt("Storage", transform.position).gameObject;
        _agent = GetComponent<NavMeshAgent>();

        base.Start();
        _marker = GetChildCalled("Marker");

        Ammo = 200;

        //Health = 6;
        StartTargetAdquired();
        LoadBulletGO();
    }

    // Update is called once per frame
    void Update()
    {
        CheckOnReward();

        if (Health == 0)
        {
            Destroy(gameObject, 15);
            return;
        }

        transform.LookAt(_targetPos);
        ShootEnemy();

        _agent.destination = _targetPos;
    }


    int wait = 10;
    private IEnumerator OneSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(wait); // wait
            wait = 1;
            CheckOnAgentAndTarget();
        }
    }

    private void CheckOnAgentAndTarget()
    {
        if (Health == 0)
        {
            return;
        }

        if (_didTargetRocket )
        {
            return;
        }

        var dist = Vector3.Distance(transform.position, _base.transform.position);
        if (dist > 40)
        {
            TargetRocket();
            return;
        }

        _targetPos = BuildingController.FindTheClosestOfContainTypeFullyBuilt("Storage", transform.position).transform.position;
    }

    /// <summary>
    /// 5 % of them will get rocket as Target
    /// </summary>
    void StartTargetAdquired()
    {
        if (UMath.GiveRandom(1, 101) > 50)//or 20 % of the time 
        {
            TargetRocket();
        }
    }

    void TargetRocket()
    {
        _didTargetRocket = true;
        _targetPos = _base.transform.position;
        _agent.destination = _targetPos;

        return;
    }

    int count;
    private void CheckOnReward()
    {
        if (_leftRewards == 0)
        {
            return;
        }

        if (count > 160)
        {
            Reward();
            _leftRewards--;
            count = 0;
        }
        count++;
    }

    void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (Health == 0 && _agent.enabled)
        {
            Destroy(gameObject);
        }
    }


    private void Reward()
    {
        var root = "Prefab/Crate/Solar_Panel_Crate";
        var Crate = General.Create(root, transform.position, root);
    }
}
