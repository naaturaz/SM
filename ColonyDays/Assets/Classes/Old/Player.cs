using UnityEngine;
using System.Collections;

public class Player : Character {

    private Animator myAnimator;
    private string currentAniNow;
    private string prevAniNow;
    private float creationTime;

    private bool graceTime;
    float flickTimer;

    private int max = 8;
    private string[] descrip ;
    private bool[] arrayOne;

    private bool isFalling = false;
    private bool isOnGround = true;
    private bool isMoving = false;
    //this is use to elements model activate blocker collider to avoid being built on top of them 
    public bool isSpawningModel = false;

    //elevator vars
    bool isOnElevatorNow;
    Transform tempElevator;
    float elevatorDistance;
    float onElevatorTime = 0;
    bool isElevatorTimeSet;

    int _playerLevel;
    int _playerXP;

    public int PlayerLevel
    {
        get { return _playerLevel; }
        set { _playerLevel = value; }
    }

    public int PlayerXP
    {
        get { return _playerXP; }
        set { _playerXP = value; }
    }

	// Use this for initialization
	void Start () 
    {
        RestartDefaultIfNeeded();

        descrip = new string[max];
        arrayOne = new bool[max];

        /* BUG REMEMBERANCE
         * WAS LIKE THIS B4 AND WAS NOT TAKING CARE OF THE GLOABAL VAR
         *  Animator myAnimator = gameObject.GetComponent<Animator>();
         */
        myAnimator = gameObject.GetComponent<Animator>();
        myAnimator.SetBool("isIdle", true);
        currentAniNow = "isIdle";

        //use for find current ani
        descrip[0] = "isIdle";
        descrip[1] = "isWalk";
        descrip[2] = "isRun";
        descrip[3] = "isJump";
        descrip[4] = "isWalkBack";
        descrip[5] = "isYoga";
        descrip[6] = "isSummon";
        descrip[7] = "isFall";
    }

    void RestartDefaultIfNeeded()
    {
        if (Lives == 0)
        {
            Lives = 3;
        }
    }

    private void ControlInput()
    {



        Vector3 idleJump = new Vector3(0, 4f, 0);
        Vector3 walkJump = new Vector3(0, 4.5f, 0);
        Vector3 runJump = new Vector3(0, 5f, 0);

        CheckIfFalling();
        CheckIfOnGround();

        //Ctrl
        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)))
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey("up"))
            {
                SetCurrentAni("isRun", FindCurrentAni());
                transform.position = transform.position + transform.forward * 3.2f * Time.deltaTime;
                //Rotates 
                transform.Rotate(0, Input.GetAxis("Horizontal") * 100f * Time.deltaTime, 0);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    transform.position = transform.position + transform.forward * 3.2f * Time.deltaTime;
                    Jump(runJump);
                }
            }
            else
            {
                SetCurrentAni("isIdle", FindCurrentAni());
            }
        }
        //W or UP
        else if ((Input.GetKey(KeyCode.W)) || Input.GetKey("up"))
        {
            SetCurrentAni("isWalk", FindCurrentAni());
            transform.position = transform.position + transform.forward * 1.2f * Time.deltaTime;
            //Rotates 
            transform.Rotate(0, Input.GetAxis("Horizontal") * 100f * Time.deltaTime, 0);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.position = transform.position + transform.forward * 2f * Time.deltaTime;
                Jump(walkJump);
            }

            //transform.Rotate(0, Input.GetAxis("Horizontal") * 100f * Time.deltaTime, 0);
        }
        else if ((Input.GetKeyUp(KeyCode.W)) || (Input.GetKeyUp(KeyCode.S))
             || Input.GetKeyUp("up") || Input.GetKeyUp("down"))
        {
            SetCurrentAni("isIdle", FindCurrentAni());
        }
        //S or cursor down
        else if ((Input.GetKey(KeyCode.S)) || Input.GetKey("down"))
         {
             SetCurrentAni("isWalkBack", FindCurrentAni());
             transform.position = transform.position - transform.forward * 1.2f * Time.deltaTime;
             //Rotates
             transform.Rotate(0, Input.GetAxis("Horizontal") * 100f * Time.deltaTime, 0);

             if (Input.GetKeyDown(KeyCode.Space))
             {
                 SetCurrentAni("isJump", FindCurrentAni());
                 transform.position = transform.position - transform.forward * 2f * Time.deltaTime;
             }
         }
        //Space
        else if (Input.GetKeyDown(KeyCode.Space) && FindCurrentAni() != "isJump")
        {
            Jump(idleJump);
        }
        //addressing jump loop
        else if (Time.time > creationTime + 0.75f && FindCurrentAni() == "isJump")
        {
            SetCurrentAni("isIdle", FindCurrentAni());
        }
        //addressing summon loop
        else if (Time.time > creationTime + 0.84f && FindCurrentAni() == "isSummon")
        {
            SetCurrentAni("isIdle", FindCurrentAni());
        }
        //Die
        else if (Input.GetKeyDown(KeyCode.K))
        {
            SetCurrentAni("isYoga", FindCurrentAni());
        }
        //Rotates 
        transform.Rotate(0, Input.GetAxis("Horizontal") * 100f * Time.deltaTime, 0);


        //print("y:" + rigidbody.velocity.y);
    }
	
	// Update is called once per frame
	new void Update ()
    {
        base.Update();//execute the update method in base
        if(!Settings.ISPAUSED)
        { 
            ControlInput();
        }
        else
        {
            if (FindCurrentAni() != "isIdle")
            { 
                SetCurrentAni("isIdle", FindCurrentAni());
            }
        }

        CheckIfLoseLive();
        if (graceTime) { Flick(); }

        CheckIfPlayerMoving();

        if(isOnElevatorNow && !isMoving)
        {
            if (!isElevatorTimeSet)
            {
                isElevatorTimeSet = true;     
                onElevatorTime = Time.time; 
            }
            OnElevatorBehavior(elevatorDistance, tempElevator);
        }
	}

    void CheckIfPlayerMoving()
    {
        if (FindCurrentAni() != "isIdle")
        {
            isMoving = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            isMoving = false;
        }
    }

    //checks if we are hitting something tagged: Spike
    private void CheckIfLoseLive()
    {
        Vector3 rayOrigin = transform.position;
        rayOrigin.y = rayOrigin.y + .4f;

        //Debug.DrawRay(rayOrigin, transform.forward, Color.yellow, 20f);

        RaycastHit hitFront;
        Ray rayFront = new Ray(rayOrigin, transform.forward);
        if (Physics.Raycast(rayFront, out hitFront))
        {

        }
        if (hitFront.transform != null)
        {
            //print(hitFront.distance + "." + hitFront.transform.name);
            if (hitFront.distance < 0.25f && (hitFront.transform.name.Contains("Spike")
                || hitFront.transform.name.Contains("Mine")))
            {
                if(!graceTime){ Lives = TakeOneLiveDamage(); }
                if(hitFront.transform.name.Contains("Mine"))
                {
                    Mine touchedMine = (Mine)USearch.FindTransfInList(Program.twoDMHandler.allModels, hitFront.transform);
                    if (touchedMine != null)
                    {
                        touchedMine.Explode();
                        touchedMine = null;
                        int indexToRemove = USearch.GiveMeIndexInList(Program.twoDMHandler.allModels, hitFront.transform);
                        Program.twoDMHandler.allModels.RemoveAt(indexToRemove);
                    }
                }
            }
        }
    }

    //takes one live as damage
    public int TakeOneLiveDamage()
    {
        if (Lives > 0)
        {
            Lives = Lives - 1;
            Program.twoDMHandler.DestroyIndividualHeart(Lives);

            //if is zero already then...
            if (Lives == 0)
            {
                SetCurrentAni("isYoga", FindCurrentAni());
                return Lives;
            }

            graceTime = true;
            flickTimer = Time.time;
            creationTime = Time.time;
        }
        return Lives;
    }

    public void TakeOneLiveDamageVoid()
    {
        print(Lives + "Lvies");
        if (Lives > 0)
        {
            Lives = Lives - 1;
            Program.twoDMHandler.DestroyIndividualHeart(Lives);

            if (Lives == 0)
            {
                SetCurrentAni("isYoga", FindCurrentAni());
            }

            graceTime = true;
            flickTimer = Time.time;
            creationTime = Time.time;
        }
    }

    //handles the flick of this class
    void Flick()
    {
        if(graceTime && Time.time < creationTime + 3f)
        {
            //print("Flick");
            FlickRender(1);//this 1 represents the model child is the geometry to flick
        }
        else if(Time.time >= creationTime + 3f)
        {
            graceTime = false;
            transform.GetChild(1).GetComponent<Renderer>().enabled = true;
        }
    }

    //flick the render of the child object index 
    void FlickRender(int index)
    {
        if(Time.time - flickTimer > 0.1f)
        {
            if (transform.GetChild(index).GetComponent<Renderer>().enabled)
            {
                transform.GetChild(index).GetComponent<Renderer>().enabled = false;
            }
            else
            {
                transform.GetChild(index).GetComponent<Renderer>().enabled = true;
            }
            flickTimer = Time.time;
        }
    }



    void OnElevatorBehavior(float distance, Transform objToFollow)
    {
        float waitTimeBeforeApplyBehavior = 0.1f;
        if (Time.time > onElevatorTime + waitTimeBeforeApplyBehavior)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;
         
            Vector3 temp = new Vector3();
            temp.y = objToFollow.position.y + distance;
            temp.x = transform.position.x;
            temp.z = transform.position.z;

            transform.position = temp;
        }
    }

    public void CheckIfBeingElevated(RaycastHit[] fourHits)
    {
        int counter = 0;
        for (int i = 0; i < fourHits.Length; i++)
        {
            if (fourHits[i].transform != null)
            {
                if (fourHits[i].distance < 0.76f && fourHits[i].transform.name == S.Up_Side_Elevator.ToString())
                {
                    //print("on elevator now");
                    isOnElevatorNow = true;
                    tempElevator = fourHits[i].collider.transform;
                    elevatorDistance = 0.1f;
                }
                else counter++;
            }
        }
        //print("counter." + counter);
        //those are all the rays pass
        if (fourHits.Length == counter)
        {
            isOnElevatorNow = false;
            tempElevator = null;
            isElevatorTimeSet = false;
        }
    }

    public void CheckIfOnGround()
    {
        //front 
        Vector3 frontRayOrigin = transform.position;
        frontRayOrigin.y = frontRayOrigin.y + 0.1f;//to avoid start the ray inside the collider we are on
        frontRayOrigin.z = frontRayOrigin.z + 0.15f;        //to cover the front 

        //rear
        Vector3 rearRayOrigin = transform.position;
        rearRayOrigin.y = rearRayOrigin.y + 0.1f;//to avoid start the ray inside the collider we are on
        rearRayOrigin.z = rearRayOrigin.z - 0.1f;        //to cover the back 

        //right
        Vector3 rightRayOrigin = transform.position;
        rightRayOrigin.x = rightRayOrigin.x - 0.15f;//so is at players right 
        rightRayOrigin.y = rightRayOrigin.y + 0.1f;//to avoid start the ray inside the collider we are on

        //left
        Vector3 leftRayOrigin = transform.position;
        leftRayOrigin.x = leftRayOrigin.x + 0.15f;//so is at players right 
        leftRayOrigin.y = leftRayOrigin.y + 0.1f;//to avoid start the ray inside the collider we are on

        //Debug.DrawRay(frontRayOrigin, -Vector3.up, Color.red, 20f);
        //Debug.DrawRay(rearRayOrigin, -Vector3.up, Color.red, 20f);
        //Debug.DrawRay(rightRayOrigin, -Vector3.up, Color.red, 20f);
        //Debug.DrawRay(leftRayOrigin, -Vector3.up, Color.red, 20f);



        RaycastHit hitFront;
        Ray downRayFront = new Ray(frontRayOrigin, -Vector3.up);
        if (Physics.Raycast(downRayFront, out hitFront))
        {
            
        }
        RaycastHit hitRear;
        Ray downRayRear = new Ray(rearRayOrigin, -Vector3.up);
        if (Physics.Raycast(downRayRear, out hitRear))
        {
            
        }
        RaycastHit hitRight;
        Ray rightRayFront = new Ray(rightRayOrigin, -Vector3.up);
        if (Physics.Raycast(rightRayFront, out hitRight))
        {

        }
        RaycastHit hitLeft;
        Ray leftRayRear = new Ray(leftRayOrigin, -Vector3.up);
        if (Physics.Raycast(downRayRear, out hitLeft))
        {

        }

        RaycastHit[] hits = { hitFront, hitRear, hitRight, hitLeft };

        //if is not moving check we are on a elevator

        CheckIfBeingElevated(hits);


        //print(hits[0].distance + "." + hits[0].transform.name);
        int raysInTheAirCount = 0;

        for (int i = 0; i < hits.Length; i++)  
        {
            //if this...                                  or this...
            if ((hits[i].distance > 0.179f && isOnGround) || hits[i].transform == null)
            {
                raysInTheAirCount++;
                if (raysInTheAirCount == 4)
                {
                    isOnGround = false;
                }
            }
            else if (hits[i].distance < 0.179f && !isOnGround)
            {
                isOnGround = true;
            }
        }
    }

    public void CheckIfFalling()
    {
        //if speed is so this is falling
        if (GetComponent<Rigidbody>().velocity.y < -7f && !isFalling)
        {
            isFalling = true;
            SetCurrentAni("isFall", FindCurrentAni());
            print("falling");
        }
        else if (GetComponent<Rigidbody>().velocity.y > -7f && isFalling)
        {
            isFalling = false;
            SetCurrentAni("isIdle", FindCurrentAni());
        }
    }

    public void Jump(Vector3 height)
    {
        if (!isFalling && isOnGround)
        {
            SetCurrentAni("isJump", FindCurrentAni());
            isMoving = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;

            GetComponent<Rigidbody>().velocity = height;
        }
    }

    public void SetCurrentAni(string animationPass, string oldAnimation)
    {
        currentAniNow = animationPass;
        myAnimator.SetBool(animationPass, true);
        myAnimator.SetBool(oldAnimation, false);
    }

    public string FindCurrentAni()
    {
        string temp = "";

        for (int i = 0; i < descrip.Length; i++)
        {
            arrayOne[i] = myAnimator.GetBool(descrip[i]);
        }

        for (int i = 0; i < arrayOne.Length; i++)
        {
            if (arrayOne[i])//if is on
            {
                temp = descrip[i];
            }
        }
        return temp;
    }
}