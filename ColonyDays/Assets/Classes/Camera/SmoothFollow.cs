using UnityEngine;
using System.Collections;

public class SmoothFollow : CamControl {

	public bool isToFollowPlayer;
    public Transform target ;
    public static Transform TARGET;
    public float smoothTime = 0.3f;

    private Vector3 velocity;
    public float yOffset = 0.7f;
	public float xOffset = 0.0f;
    public float zOffset = 0.0f;
	
    public bool useSmoothing = false;
	
	public float velocityX = 0.5f;
	public float velocityY = 0.5f;
    public float velocityZ = 0.5f;

    float cameraAngleX = 0;

    Transform[] arrayChilds = null;

    void Start()
    {
        velocity = new Vector2(velocityX, velocityY);

        if(Application.loadedLevelName == "Lobby")
        {
            HideAllHelpers();
        }
    }

    /// <summary>
    /// Hides all helpers Tagged with Visual_Helper and disables the collider too 
    /// </summary>
    void HideAllHelpers()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag(S.Visual_Helper.ToString());
        
        for (int i = 0; i < array.Length; i++)
        {
            for (int k = 0; k < array[i].transform.childCount; k++)
            {
                array[i].transform.GetChild(k).transform.GetComponent<Collider>().enabled = false;
                array[i].transform.GetChild(k).GetComponent<Renderer>().enabled = false;
            }
        }
    }

    void LateUpdate() 
    {
        if (target == null && isToFollowPlayer)
        {
            target = GameObject.FindGameObjectWithTag("Camera_Guide").gameObject.transform;
            TARGET = target;
        }

        if (target != null)
        {
            //Position
            Vector3 newPos = Vector3.zero;

            if (useSmoothing)
            {
                newPos.x = Mathf.SmoothDamp(transform.position.x, target.position.x + xOffset, ref velocity.x, smoothTime);
                newPos.y = Mathf.SmoothDamp(transform.position.y, target.position.y + yOffset, ref velocity.y, smoothTime);
                newPos.z = Mathf.SmoothDamp(transform.position.z, target.position.z + zOffset, ref velocity.z, smoothTime);
            }
            else
            {
                newPos.x = target.position.x + xOffset;
                newPos.y = target.position.y + yOffset;
                newPos.z = target.position.z + zOffset;
            }
            Vector3 newPosNow = new Vector3(newPos.x, newPos.y, newPos.z);
            transform.position = Vector3.Slerp(transform.position, newPosNow, Time.time);

            //Rotation
            // Get the target rotation
            var newRotation = Quaternion.LookRotation(target.forward, Vector3.up);

            // Smoothly rotate towards the target //speedPass is the retardation, the higher the speedy is
            //in temp is stored the new rotation only for y and z 
            Quaternion temp = Quaternion.Slerp(target.rotation, newRotation, 2f * Time.deltaTime);

            if (cameraAngleX == 0)//if is zero we assign it
            {
                cameraAngleX = transform.rotation.x;//we keep the x value the same
            }
            transform.rotation = temp;
        }
        else print("target is null");
    }
}