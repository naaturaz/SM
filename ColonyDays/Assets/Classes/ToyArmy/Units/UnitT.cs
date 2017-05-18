using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class UnitT : Shooter
{
    private int _rank;

    private bool _wasFixed;
    private GameObject _rocket;

    private Transform enemy;
    float _enemyDist = 100;

    protected Building _building;

    GameObject _selectedGO;
    float _selectedAt;



    public string Root { get; private set; }

    protected Transform Enemy
    {
        get
        {
            return enemy;
        }

        set
        {
            enemy = value;
        }
    }

    public GameObject SelectedGO
    {
        get
        {
            return _selectedGO;
        }

        set
        {
            _selectedGO = value;
        }
    }

    public bool WasFixed
    {
        get
        {
            return _wasFixed;
        }

        set
        {
            _wasFixed = value;
        }
    }


    // Use this for initialization
    protected void Start()
    {
        base.Start();
        StartCoroutine("RandomSecUpdate");

        SelectedGO = GetChildCalled("Selected");
        if (SelectedGO != null)
        {
            SelectedGO.SetActive(false);
        }

        if (name.Contains("Enemy"))
        {
            var a = name.Substring(5).Split('/');
            var n = a[a.Length - 1];
            Health = 8;
        }
        else
        {
            Health = 9;
        }

    }

    float nextRand = 2;


    private IEnumerator RandomSecUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(nextRand); // wait

            if (IsGood)
            {
                if (Enemy == null || _enemyDist > 20)
                {
                    Enemy = Program.gameScene.EnemyManager.GiveMeClosestEnemy(transform.position);
                }
            }
            else
            {
                Enemy = Program.gameScene.UnitsManager.GiveMeClosestUnit(transform.position);
            }

            nextRand = UMath.GiveRandom(2, 3);

            if (Enemy != null)
            {
                _enemyDist = Vector3.Distance(transform.position, Enemy.position);
            }
        }
    }

    General grade;
    SpriteRenderer spRend;
    internal void RankUp()
    {
        if (transform == null)
        {
            return;
        }

        _rank++;

        if (grade == null)
        {
            grade = General.Create("Prefab/GUI/Rank_Template", transform.position + new Vector3(0, 0.6f, 0), "", transform);
        }
        spRend = grade.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!WasFixed)
        {
        }
        if (!WasFixed && Input.GetMouseButtonUp(0))
        {


        }

        if (Input.GetMouseButtonUp(0) && Time.time > _selectedAt + 0.2f)
        {
            _selectedGO.SetActive(false);
        }


        if (Enemy != null && _enemyDist < 12)
        {
            if (!IsGood)
            {
                transform.LookAt(Enemy);
            }

            ShootEnemy();
        }
    }

    internal void SetToSoil()
    {
        WasFixed = true;
        Program.gameScene.UnitsManager.AddToAll(this);
        RemoveCost();
        //Program.gameScene.SoundManager.PlaySound(3);
    }



    private void RemoveCost()
    {
        //Building.RemoveCost(name);
    }

    internal static UnitT CreateU(string root, Vector3 point, string buildingPath, Transform transform)
    {
        var obj = (UnitT)Create(root, point, buildingPath, transform);
        obj.Root = root;
        return obj;
    }


    bool didDied;

    protected void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (IsDeath() && !didDied)
        {
            if (IsGood)
            {
                Program.gameScene.UnitsManager.RemoveUnit(this);
            }
            else
            {
                Program.gameScene.EnemyManager.RemoveMeFromEnemiesList(this);
            }

            //if (_building != null)
            //{
            //    Program.gameScene.BuildingManager.RemoveBuilding(_building);
            //}

            didDied = true;
            Destroy(gameObject);
        }
    }

    internal void Selected()
    {
        if (SelectedGO != null)
        {
            SelectedGO.SetActive(true);
            _selectedAt = Time.time;
        }
    }


    #region Enemy Units


    #endregion

    internal void MaxHealth()
    {
        ShowText("+10");
        Health = 10;
    }
}
