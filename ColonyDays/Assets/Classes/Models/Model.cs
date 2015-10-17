using UnityEngine;
using System.Collections;

public class Model : General {



    //hovers around the model created and if lefft click will put it in soil
    public void CheckOnMouse()
    {
        //if (Program.MOUSEHITTHIS.transform != null)
        //{
            

        //    //if this is not null, Program.MOUSEHITTHIS.transform is not this, position was not fixed
        //    //and Program.MOUSEHITTHIS.transform is not a menu
        //    if (this != null && Program.MOUSEHITTHIS.transform != transform
        //        && Program.MOUSEHITTHIS.transform.tag != "Model"//we need to address this wit tag, due to the childs
        //        && !PositionFixed
        //        && !Program.MOUSEHITTHIS.transform.name.Contains("Menu")
        //        && !Program.MOUSEHITTHIS.transform.name.Contains("GUI")
        //        && Program.MOUSEHITTHIS.transform.tag != "Player"
        //        && Program.MOUSEHITTHIS.transform.tag != "Element")//so it does nt build on top of elements

        //    {
        //        transform.position = Program.MOUSEHITTHIS.point;//this position is = to Program.MOUSEHITTHIS.point
        //        Program.THEPlayer.ShowBlockedArea = true;//we show the player blocked area
        //        Program.THEPlayer.isSpawningModel = true;
        //    }

        //    if (Input.GetMouseButtonUp(0) && !PositionFixed)
        //    {
        //        PositionFixed = true;//the model position is fixed and cant be move on future
                
        //        if(gameObject.tag == "Model")//if is tagged model
        //        {
        //            tag = "ModelFixed";//we change the tag... so we can build on top of others models
        //        }

        //        //Play Animation on Player
        //        Program.THEPlayer.SetCurrentAni("isSummon", Program.THEPlayer.FindCurrentAni());
        //        Program.THEPlayer.ShowBlockedArea = false;//we show the player blocked area
        //        Program.THEPlayer.isSpawningModel = false;
        //    }
        //}
    }

    // Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () 
    {

	}
}
