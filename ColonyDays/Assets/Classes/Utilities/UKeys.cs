using UnityEngine;

public class UKeys : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    { }

    // Update is called once per frame
    private void Update()
    { }

    public static string FindBtnKeyUP()
    {
        string t = null;
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            t = BtnsE.Select_Cube_Btn_Raw_3dMenu.ToString();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            t = BtnsE.Select_Cylinder_Btn_Raw_3dMenu.ToString();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            t = BtnsE.Select_Sphere_Btn_Raw_3dMenu.ToString();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            t = BtnsE.Select_Pyramid_Btn_Raw_3dMenu.ToString();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            t = BtnsE.Select_Cone_Btn_Raw_3dMenu.ToString();
        }
        ////////
        else if (Input.GetKeyUp(KeyCode.Alpha6))
        {
            t = BtnsE.Select_Bomb_Btn_Element_3dMenu.ToString();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            t = BtnsE.Select_Elevator_Btn_Element_3dMenu.ToString();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            t = BtnsE.Select_Mine_Btn_Element_3dMenu.ToString();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            t = BtnsE.Select_Spike_Btn_Element_3dMenu.ToString();
        }
        else if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            t = BtnsE.Select_Spring_Btn_Element_3dMenu.ToString();
        }
        return t;
    }
}