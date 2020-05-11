using UnityEngine;

public class Explosive : Element
{
    protected float destroyStartTime;//the time starter for the timed Destroy
    public bool isTimeDestroy = true;//destroys in some point by time passing
    public float timedDestroyInSec = 5f;//time will take to get auto timed exploded this obj

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
    }

    protected virtual void FixedUpdate()
    {
        if (isTimeDestroy)
        { TimerDestroy(); }
    }

    public void Explode()
    {
        Passive t = Passive.Create(RootParticle.explosionOriginal, transform.position);

        if (gameObject.GetComponent<Renderer>() != null)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).transform.GetComponent<Renderer>() != null)
                {
                    transform.GetChild(i).transform.GetComponent<Renderer>().enabled = false;
                }
            }
        }
    }

    protected void TimerDestroy()
    {
        if (Time.time > destroyStartTime + timedDestroyInSec)
        {
            Destroy(gameObject);
        }
    }
}