using UnityEngine;

public class Bomb : Explosive
{
    private bool isAPlayerInside;

    // Use this for initialization
    private void Start()
    { destroyStartTime = Time.time; }

    // Update is called once per frame
    private void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (Time.time > destroyStartTime + timedDestroyInSec && PositionFixed)
        {
            Explode();

            if (isAPlayerInside)
            {
                Program.THEPlayer.Lives = Program.THEPlayer.TakeOneLiveDamage();
            }

            if (isTimeDestroy)
            {
                TimerDestroy();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != null)
        {
            if (other.transform.tag == H.Player.ToString())
            {
                isAPlayerInside = true;
                print("inside bomb");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform != null)
        {
            if (other.transform.tag == H.Player.ToString())
            {
                isAPlayerInside = false;
                print("out side bomb");
            }
        }
    }
}