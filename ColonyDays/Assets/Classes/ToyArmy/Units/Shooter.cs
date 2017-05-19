using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Shooter : General
{

    private GameObject _badUnitGO;

    HealthBar _healthBar;

    private float _fireTime;
    private float _fireRate = .2f;
    GameObject _bulletSpawn;
    GameObject _bullet;
    string _bulletRoot;
    private int _health = 5;

    int _ammo = 200;

    float _bulletRange = 1.5f;
    float _bulletSpeed = 15;

    public int Ammo
    {
        get
        {
            return _ammo;
        }

        set
        {
            _ammo = value;
        }
    }

    static float _lastShoot;

    public int Health
    {
        get
        {
            return _health;
        }

        set
        {
            _health = value;
        }
    }

    public GameObject BulletSpawn
    {
        get
        {
            return _bulletSpawn;
        }

        set
        {
            _bulletSpawn = value;
        }
    }


    public float FireRate
    {
        get
        {
            return _fireRate;
        }

        set
        {
            _fireRate = value;
        }
    }

    public string BulletRoot
    {
        get
        {
            return _bulletRoot;
        }

        set
        {
            _bulletRoot = value;
        }
    }

    public float BulletRange
    {
        get
        {
            return _bulletRange;
        }

        set
        {
            _bulletRange = value;
        }
    }

    public float BulletSpeed
    {
        get
        {
            return _bulletSpeed;
        }

        set
        {
            _bulletSpeed = value;
        }
    }

    public HealthBar HealthBar
    {
        get
        {
            return _healthBar;
        }

        set
        {
            _healthBar = value;
        }
    }



    protected void Start()
    {
        base.Start();

        var pos = transform.position + new Vector3(0, 0.4f, 0);
        if (name.Contains("Tank"))
        {
            pos += new Vector3(0, 0.15f, 0);
        }

        HealthBar = (HealthBar)Create("Prefab/TA/GUI/Health_Bar", pos, "H_Bar", transform);
        HealthBar.PassShooter(this);


        _badUnitGO = GetChildCalled("Bad_Unit");
        if (_badUnitGO != null && IsGood)
        {
            _badUnitGO.SetActive(false);
        }



        if (BulletSpawn == null)
        {
            BulletSpawn = GetChildCalled("Bullet_Spawn");
        }

        if (BulletSpawn == null)
        {
            BulletSpawn = GetGrandChildCalled("Bullet_Spawn");

            //a building that doesnt shoot 
            if (BulletSpawn == null)
            {
                return;
            }
        }

        if (IsGood)
        {
            BulletRoot = "Militar/Bullet";
        }
        else
        {
            BulletRoot = "Militar/Bullet_Bad";
        }
    }

    /// <summary>
    /// Has to be loaded mannually . so call this 
    /// </summary>
    protected void LoadBulletGO()
    {
        _bullet = (GameObject)Resources.Load(BulletRoot);
    }

    protected void Shoot()
    {
        if ((Input.GetButton("Fire1") || Program.gameScene.EnemyManager.ThereIsAnAttackNow()) && Time.time > _fireTime)
        {
            //SpawnBullet();
        }
    }

    protected void ShootEnemy()
    {
        if (Time.time > _fireTime)
        {
            //SpawnBullet();
        }
    }

    void SpawnBullet()
    {
        if (_ammo < 1)
        {
            return;
        }

        //if _bullet is null might be bz was not loaded LoadBulletGO()
        GameObject bullet = Instantiate(_bullet, BulletSpawn.transform.position, BulletSpawn.transform.rotation);
        bullet.name = BulletRoot;
        _fireTime = Time.time + FireRate;
        bullet.GetComponent<Bullet>().Fire(BulletRange, BulletSpeed, IsGood, name);

        if (!IsGood)
        {
            return;
        }

        if (Time.time > _lastShoot + 0.5f)
        {
            //Program.gameScene.SoundManager.PlaySound(0);
            _lastShoot = Time.time;
        }



    }

    protected void OnTriggerEnter(Collider other)
    {
        //touching a building
        if (other.gameObject.layer == 10)
        {
            Debug.Log("10");
        }
        //touching a person
        else if(other.gameObject.layer == 11)
        {
            Debug.Log("11");
        }



        if (Health > 1)
        {
            Health--;
            ShowText("-1");
        }
        else
        {
            if (Health == 1)
            {
                Health = 0;
            }
        }
    }

    internal bool IsDeath()
    {
        return _health == 0;
    }


}



