using UnityEngine;
using System.Collections;
using System;

public class Bullet : General
{
    private float Range = 1.5f;
    public AudioClip ShootSound = null;
    private bool canMove;
    float _speed = 15;
    float _startTime;
    string _whoShootMe;

    private void Start()
    {
        _startTime = Time.time;
        if (_whoShootMe.Contains("RPG"))
        {
            //Program.GameScene.SoundManager.PlaySound(8, 0.09f);
        }
        else if(_whoShootMe.Contains("Sniper"))
        {
            //Program.GameScene.SoundManager.PlaySound(9, 0.09f);
        }
        else
        {
            //Program.GameScene.SoundManager.PlaySound(UMath.GiveRandom(10, 14), 0.09f);
        }
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (canMove)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        }

        if (Time.time > _startTime + Range)
        {
            RPG();

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Terrain" || other.tag == "Player")
        {
            return;
        }

        RPG();

        Destroy(gameObject);
        return;
    }

    void RPG()
    {
        if (name.Contains("RPG"))
        {
           // Program.GameScene.SoundManager.PlaySound(2, 0.05f);
            var exp = General.Create("Prefab/Particles/Explosion_Small", transform.position, "Explosion_Small");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Destroy(gameObject);

    }

    internal void Fire(float bulletRange, float bulletSpeed, bool isGood, string whoShoots)
    {
        Range = bulletRange;
        _speed = bulletSpeed;
        _whoShootMe = whoShoots;
        IsGood = isGood;
        canMove = true;
    }


}
