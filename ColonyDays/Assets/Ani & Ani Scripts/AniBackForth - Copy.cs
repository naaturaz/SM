using UnityEngine;
using System.Collections;


//git testing 
public class AniBackForth : MonoBehaviour {
	
	public GameObject gameObjToAnimate;
	string state = "closed"; 
	public float aniLenght;
	float creationTime;
	public float animationTimeAllowed; //should be greater than aniLeghnt
	public string stringAnimation;
	
	static public bool ISREWIND = false;
	
	private	float tempAniTime =0 ;

	// Use this for initialization
	void Start () 
	{
		creationTime = Time.time;
	}
	
	private float reverser = 0;
	
	void Rewinder()
	{
		if(!ISREWIND)
		{
			reverser=1;
		}
		else if(ISREWIND)
		{
			if(state=="opened")
			{
				state="closed";
			}
			else if(state=="closed")
			{
				state="opened";
			}
			reverser=-1;
			ISREWIND = false;
		}
	}

	void BackAndForthAnimator()
	{
		if(!Settings.ISPAUSED)
		{
			//heere is not needed to touch the animatio[].time... will be started always
			//when the animtion was left
			if(state == "closed" && Time.time > creationTime + animationTimeAllowed)
			{
			
				creationTime = Time.time;
				state = "opened";
				//calls rewinder... will have impact if ISREWIND Is true ...
				Rewinder();
				gameObjToAnimate.GetComponent<Animation>()[stringAnimation].speed = reverser * 1 / aniLenght;
				gameObjToAnimate.GetComponent<Animation>().Play(stringAnimation);
				
			}
			//since here we need to play the animation backwards we have to deal
			//with the time of the animation
			else if(state == "opened" && Time.time > creationTime + animationTimeAllowed)
			{
				//ready to start  ani... in the very begginning
				if(gameObjToAnimate.GetComponent<Animation>()[stringAnimation].time == 0)
				{
					//initial time of the ani is set to the end
					gameObjToAnimate.GetComponent<Animation>()[stringAnimation].time = 
						gameObjToAnimate.GetComponent<Animation>()[stringAnimation].length;	
				}
				//is in btw ,,, bz need to be rewineded
				else if(gameObjToAnimate.GetComponent<Animation>()[stringAnimation].time != 0)
				{
					//initial time of the ani is set to the same was stoped at
					gameObjToAnimate.GetComponent<Animation>()[stringAnimation].time = tempAniTime;
				}
				
				creationTime = Time.time;
				state = "closed";
				Rewinder();
				//calls rewinder... will have impact if ISREWIND Is true ...
				gameObjToAnimate.GetComponent<Animation>()[stringAnimation].speed = reverser * -1 / aniLenght;
				gameObjToAnimate.GetComponent<Animation>().Play(stringAnimation);
			}
		}
		else if(Settings.ISPAUSED)
		{
			//all we do is set speed to zero.. and cpature the time the ani was at
			//so we can retake later from there
			tempAniTime = gameObjToAnimate.GetComponent<Animation>()[stringAnimation].time;
			gameObjToAnimate.GetComponent<Animation>()[stringAnimation].speed = 0;
		}
	}

	// Update is called once per frame
	void LateUpdate () 
	{
		BackAndForthAnimator();		
	}
}
