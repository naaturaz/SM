/// <summary>
/// Script Created by Daniel Brookshaw[King Charizard] Copyright 2012
/// This script was created to mimic the RTS style camera controls.
/// </summary>

using UnityEngine;
using System.Collections;

public class CameraRTS : MonoBehaviour
{
    //public vaiables
    public float cameraPosX, cameraPosY, cameraPosZ;
    public float cameraRotX, cameraRotY, cameraRotZ;
    public float cameraSpeed = 5f, scrollSpeed = 25f, pixelEdge = 10f, minZoom = 10f, maxZoom = 30f;

    //Private variables, on one line seperated by a ',' to take up less lines of code.
    private Vector3 moveRight = Vector3.right, moveLeft = Vector3.left, moveUp = Vector3.up, moveDown = Vector3.down;
    private float currenZoom;


    // Called when the game starts up.
    void Start()
    {
        //Sets transfrom.position/rotation to the values you set in the inspector.
        transform.position = new Vector3(cameraPosX, cameraPosY, cameraPosZ);
        transform.rotation = Quaternion.Euler(cameraRotX, cameraRotY, cameraRotZ);
    }

    // LateUpdate is Better for these camera controls.
    void LateUpdate() {
        //Vector3 zoom variable.
        Vector3 zoom = transform.position;
       
       
        if(zoom.y < minZoom){
            zoom.y = minZoom;
            transform.position = zoom;
        }
        if(zoom.y > maxZoom){
            zoom.y = maxZoom;
            transform.position = zoom;
        }
       
       
        //Moves camera when on edge of screen.
        if(Input.mousePosition.x > Screen.width - pixelEdge || Input.GetKey(KeyCode.D)){
            transform.Translate(moveRight * Time.deltaTime * cameraSpeed, Space.Self);
        }
        if(Input.mousePosition.x < 0 + pixelEdge || Input.GetKey(KeyCode.A)){
            transform.Translate(moveLeft * Time.deltaTime * cameraSpeed, Space.Self);
        }
        if(Input.mousePosition.y > Screen.height - pixelEdge || Input.GetKey(KeyCode.W)){
            transform.Translate(moveUp * Time.deltaTime * cameraSpeed, Space.Self);
        }
        if(Input.mousePosition.y < 0 + pixelEdge || Input.GetKey(KeyCode.S)){
            transform.Translate(moveDown * Time.deltaTime * cameraSpeed, Space.Self);
        }
       
        //Rotates the camera
        if(Input.GetKey(KeyCode.Q)){
            transform.Rotate(0, -2.5f * Time.deltaTime * cameraSpeed, 0, Space.World);
        }
        if(Input.GetKey(KeyCode.E)){
            transform.Rotate(0, 2.5f * Time.deltaTime * cameraSpeed, 0, Space.World);
        }
       
        //Zooms the camera with the scroll wheel or the number 1 key and the number 2 key.
        if(Input.GetAxis("Mouse ScrollWheel")< 0 || Input.GetKey(KeyCode.Alpha2)){
            if(zoom.y < maxZoom){
            transform.Translate(Vector3.back * Time.deltaTime * scrollSpeed);
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKey(KeyCode.Alpha1)){
            if(zoom.y > minZoom){
            transform.Translate(Vector3.forward * Time.deltaTime * scrollSpeed);
            }
        }
        if(Input.mousePosition.x > Screen.width - pixelEdge || Input.GetKey(KeyCode.LeftShift)){
            transform.Rotate(0, 2.5f * Time.deltaTime * cameraSpeed, 0, Space.World);
        }
        if(Input.mousePosition.x < 0 + pixelEdge || Input.GetKey(KeyCode.LeftShift)){
            transform.Rotate(0, -2.5f * Time.deltaTime * cameraSpeed, 0, Space.World);
        }
       
       
           
       
   
    }



}


