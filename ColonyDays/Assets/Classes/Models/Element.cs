/*Doc: All elements need to have a child that holds  collider in the obj that will be the one 
 * preventing from build on top of them, so this collider will be  on if the player
 * is spwaning new obj otherwise false, hence elements obj need to have the permanets
 * collider separated from ths child obj
 * */

using UnityEngine;
using System.Collections;



public class Element : Model {

    bool isPlayingAni;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	protected void Update () 
    {
        CheckOnMouse();

        if(Program.THEPlayer.isSpawningModel)//if  
        {
            SwitchSphereCollider(true);
        }
        else
        {
            SwitchSphereCollider(false);
        }
	}

    /// <summary>
    /// Will turn on this collider when player is spwaning new obj so playewer
    /// doesnt build on top of current obj
    /// </summary>
    void SwitchSphereCollider(bool isOn)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i ) != null)
            {
                if(transform.GetChild(i ).name == "Blocker_Collider")
                {
                    transform.GetChild(i).GetComponent<Collider>().enabled = isOn;
                }
            }
        }
    }
}
