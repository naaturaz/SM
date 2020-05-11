using UnityEngine;

public class Bullet : General
{
    private float Range = 1.5f;
    public AudioClip ShootSound = null;
    private bool canMove;
    private float _speed = 15;
    private float _startTime;
    private string _whoShootMe;

    private void Start()
    {
        _startTime = Time.time;
        if (_whoShootMe.Contains("RPG"))
        {
            //Program.GameScene.SoundManager.PlaySound(8, 0.09f);
        }
        else if (_whoShootMe.Contains("Sniper"))
        {
            //Program.GameScene.SoundManager.PlaySound(9, 0.09f);
        }
        else
        {
            //Program.GameScene.SoundManager.PlaySound(UMath.GiveRandom(10, 14), 0.09f);
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Terrain" || other.tag == "Player")
        {
            return;
        }

        RPG();

        Destroy(gameObject);
        return;
    }

    private void RPG()
    {
        if (name.Contains("RPG"))
        {
            // Program.GameScene.SoundManager.PlaySound(2, 0.05f);
            var exp = General.Create("Prefab/Particles/Explosion_Small", transform.position, "Explosion_Small");
        }
    }

    private void OnCollisionEnter(Collision collision)
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