using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 1f;
    public bool onX;
    public bool onY;
    public bool onZ;
    public bool CareAboutGameSpeed;
    public bool ByMouse;

    private float finalSpeed;


    // Use this for initialization
    void Start()
    {
        finalSpeed = speed;
        if (CareAboutGameSpeed)
        {
            finalSpeed = speed * Program.gameScene.GameSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CareAboutGameSpeed)
        {
            finalSpeed = speed * Program.gameScene.GameSpeed;
        }
        if (ByMouse)
        {
            finalSpeed = speed * Input.GetAxis("Mouse X");
        }

        if (onX)
        {
            gameObject.transform.Rotate(finalSpeed, 0, 0);
        }
        if (onY)
        {
            gameObject.transform.Rotate(0, finalSpeed, 0);
        }
        if (onZ)
        {
            gameObject.transform.Rotate(0, 0, finalSpeed);
        }
    }

    public static void SpeedChanged()
    {

    }

    internal void ToggleOn()
    {
        if (speed == 0)
        {
            speed = 0.01f;
        }
        else
        {
            speed = 0;
        }
        finalSpeed = speed;

    }



    internal void ToggleOn(char axis)
    {
        if (speed == 0)
        {
            return;
        }
        if (axis == 'X')
        {
            onX = !onX;
        } 
        if (axis == 'Y')
        {
            onY = !onY;
        }
        if (axis == 'Z')
        {
            onZ = !onZ;
        }
        finalSpeed = speed;

    }


    internal void ChangeSpeed(float p)
    {
        speed += p;
        finalSpeed = speed;

    }
}
