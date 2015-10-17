using UnityEngine;
using System.Collections;

public class Bomb : Explosive {

    bool isAPlayerInside;

	// Use this for initialization
    void Start() { destroyStartTime = Time.time; }
	
	// Update is called once per frame
	void Update () 
    {
        base.Update();
	}

    protected override void FixedUpdate()
    {
        if (Time.time > destroyStartTime + timedDestroyInSec && PositionFixed)
        {
            Explode();

            if(isAPlayerInside)
            {
                Program.THEPlayer.Lives = Program.THEPlayer.TakeOneLiveDamage();
            }

            if (isTimeDestroy)
            {
                TimerDestroy(); 
            }
        }
    }

    void OnTriggerEnter(Collider other)
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

    void OnTriggerExit(Collider other)
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
