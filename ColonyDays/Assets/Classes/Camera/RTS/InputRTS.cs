using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InputRTS : GenericCameraComponent
{
    Vector3 lastClosedPos;
    List<RTSData> list = new List<RTSData>();

    public static bool isFollowingPersonNow;
    Vector3 storedPos;
    private Transform personToFollow;

    // Use this for initialization
    void Start()
    {
        InitializeList();
       
        

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isFollowingPersonNow && BuildingPot.InputMode == Mode.None)
        { CheckIfKeyWasPressed(); }

        CenterCam();
        FollowPersonCam();

        if (isFollowingPersonNow)
        {
            CamFollowAction(personToFollow);
        }
    }

    /// <summary>
    /// When camera is following a personToFollow
    /// </summary>
    /// <param name="personToFollow"></param>
    void CamFollowAction(Transform personToFollow)
    {
        if (personToFollow == null)
        {
            return;
        }

        Vector3 compPos = personToFollow.position;
        compPos.y = storedPos.y;
        CamControl.CAMRTS.centerTarget.transform.position = personToFollow.position;
    }

    /// <summary>
    /// initialize the list of the 5 elements with camera save,
    /// is mapping the keyCode will save and load each one. In future
    /// the keyCode to save have to be replaced with a new type of class
    /// that could be Comands.cs typeof
    /// </summary>
    void InitializeList()
    {
        list.Add(new RTSData(KeyCode.Alpha1, KeyCode.Alpha6, TransformCam, CenterTarget));
        list.Add(new RTSData(KeyCode.Alpha2, KeyCode.Alpha7, TransformCam, CenterTarget));
        list.Add(new RTSData(KeyCode.Alpha3, KeyCode.Alpha8, TransformCam, CenterTarget));
        list.Add(new RTSData(KeyCode.Alpha4, KeyCode.Alpha9, TransformCam, CenterTarget));
        list.Add(new RTSData(KeyCode.Alpha5, KeyCode.Alpha0, TransformCam, CenterTarget));

        list.Add(new RTSData(KeyCode.None, KeyCode.None, TransformCam, CenterTarget));
    }

    void CheckIfKeyWasPressed()
    {
        //didnt load so one need to be redone
        if (list==null)
        {
            return;
        }

        foreach (var item in list)
        {
            if (Input.GetKeyUp(item.saveKeyC))
            {
                ManagerReport.AddInput("SaveCamPos:" + item.saveKeyC);
                SaveCamPos(item.saveKeyC, TransformCam.position, TransformCam.rotation);
            }
        }
        foreach (var item in list)
        {
            if (Input.GetKeyUp(item.loadKeyC))
            {
                ManagerReport.AddInput("LoadCamPos:" + item.saveKeyC);

                LoadCamPos(item.loadKeyC);
            }
        }
    }

    /// <summary>
    /// if the specific key was pressed will save the cam pos in the coorrespondant 
    /// list item
    /// </summary>
    /// <param name="saveKeyC"></param>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    void SaveCamPos(KeyCode saveKeyC, Vector3 pos, Quaternion rot)
    {
        if (list == null)
        {
            list= new List<RTSData>();
            InitializeList();
        }

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].saveKeyC == saveKeyC)
            {
                list[i].pos = pos;
                list[i].rot = rot;
                list[i].FOV = TransformCam.GetComponent<Camera>().fieldOfView;
                list[i].CenterTargetPos = CenterTarget.position;
            }
        }

        //SaveLoad.Save(list);
        //saving the whole list to ...
        XMLSerie.WriteXML(list);
    }

    /// <summary>
    /// load a specific camera saved pos if existed
    /// </summary>
    /// <param name="loadKeyC"></param>
    void LoadCamPos(KeyCode loadKeyC, int listIdx = -1)
    {
        if (Dialog.IsActive())
        {
            return;
        }

        //is loading the whole list to 
        list = XMLSerie.ReadXML();

        //no file was found 
        if (list==null)
        {
            return;
        }

        for (int i = 0; i < list.Count; i++)
        {
            if ((list[i].loadKeyC == loadKeyC || listIdx == i)
                && list[i].pos != Vector3.zero)
            {
                CenterTarget.rotation = list[i].rot;
                CenterTarget.position = list[i].CenterTargetPos;

                TransformCam.position = list[i].pos;
                TransformCam.rotation = list[i].rot;
                TransformCam.GetComponent<Camera>().fieldOfView = list[i].FOV;
            }
            else if (list[i].pos != Vector3.zero)
            {
//                print("Cam pos was saved in this slot ??");
            }
        }
    }

    /// <summary>
    /// saves camera last position in the last element to the list 
    /// </summary>
    public void SaveLastCamPos()
    {
        SaveCamPos(KeyCode.None, TransformCam.position, TransformCam.rotation);
        //list.Add(new RTSData(KeyCode.None, KeyCode.None, TransformCam, CenterTarget));
        //list.Add(new RTSData(TransformCam, CenterTarget));
    }

    /// <summary>
    /// Load las position of camera, saved in last element of the list
    /// </summary>
    public void LoadLastCamPos()
    {
        LoadCamPos(KeyCode.None);
    }

    /// <summary>
    /// centeer the cam to the newTarget position loops to gets the most accurate center 
    /// position (due to algorithm is not really accurate)
    /// </summary>
    /// <param name="newTarget"></param>
    /// <param name="loop"></param>
    public void CenterCamTo(Transform newTarget, int loop = 4)
    {
        //it will lopp few times so ensure the cam is really centered to newTarget
        for (int i = 0; i < loop; i++)
        {
            CamControl.CAMRTS.CreateTargetAndUpdate();
            Vector3 storedPos = TransformCam.position;
            Quaternion storedRot = TransformCam.rotation;

            CamControl.CAMRTS.centerTarget.transform.position = newTarget.position;
            Vector3 compPos = TransformCam.position;
            compPos.y = storedPos.y;
            TransformCam.position = compPos;
            TransformCam.rotation = storedRot;

            CamControl.CAMRTS.CleanUpRotHelp();
        }
    }

    //Center cam command
    public void CenterCam(bool fakedPressKeyP = false)
    {
        if (Dialog.IsActive())
        {
            return;
        }

        var yes = Input.GetKeyUp(KeyCode.P) || fakedPressKeyP;

        if (yes && !isFollowingPersonNow)
        {
            ManagerReport.AddInput("CenterCam to 1st Building");
            CenterCamTo(BuildingPot.Control.Registro.AllBuilding.ElementAt(0).Value.transform);
        }
    }

    /// <summary>
    /// Camera will follow a person.First will center cam to person and then
    /// will make camera child of the CamControl.CAMRTS.centerOfRotY,
    /// then once isFollowingPersonNow = true will follow the person until 
    /// the flag is false. Once flag is up camera cant move or rotate 
    /// </summary>
    void FollowPersonCam()
    {
        if (Dialog.IsActive())
        {
            return;
        }

        personToFollow = GetSelectedPerson();
        if (personToFollow == null)
        {
            return;
        }
        
        if (Input.GetKeyUp(KeyCode.O) && !isFollowingPersonNow)
        {
            CenterCamTo(personToFollow);
            CamControl.CAMRTS.CreateTargetAndUpdate();

            if (storedPos == Vector3.zero)
            {
                storedPos = CamControl.CAMRTS.centerTarget.transform.position;
            }

            isFollowingPersonNow = true;
        }
        else if (Input.GetKeyUp(KeyCode.O) && isFollowingPersonNow)
        {
            CamControl.CAMRTS.CleanUpRotHelp();
            isFollowingPersonNow = false;
        }
    }

    Transform GetSelectedPerson()
    {
        if (Program.MouseListener == null || Program.MouseListener.PersonWindow1 == null)
        {
            return null;
        }

        var person = Program.MouseListener.PersonWindow1.Person1;

        if (person==null )
        {
            return null;
        }
        return person.transform;
    }
}
