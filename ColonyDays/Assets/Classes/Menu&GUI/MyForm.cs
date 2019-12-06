﻿using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class MyForm : General
{
    GameObject _canvas;
    GameObject _panel;
    GameObject _resources;

    private GameObject _startPosIni;

    private ShowPathTo _showPathToSea;
    private MiniHelper _miniHelper;

    public GameObject Canvas
    {
        get
        {
            if (_canvas == null)
            {
                return GetChildCalled(H.Canvas, gameObject);
            }
            return _canvas;
        }
        set
        {
            if (_canvas == null)
            {
                _canvas = GetChildCalled(H.Canvas, gameObject);
            }
            _canvas = value;
        }
    }

    public GameObject Resources
    {
        get
        {
            if (_resources == null)
            {
                return GetChildCalled(H.Resources, Canvas);
            }
            return _resources;
        }
        set
        {
            if (_resources == null)
            {
                _resources = GetChildCalled(H.Resources, Canvas);
            }
            _resources = value;
        }
    }

    public GameObject Panel
    {
        get
        {
            if (_panel == null)
            {
                return GetChildCalled(H.Panel, Canvas);
            }
            return _panel;
        }
        set
        {
            if (_panel == null)
            {
                _panel = GetChildCalled(H.Panel, Canvas);
            }
            _panel = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine("UpdateEveryMinute");
        StartCoroutine("UpdateEvery2Sec");

        _canvas = GetChildCalled(H.Canvas, gameObject);
        _resources = FindGameObjectInHierarchy("Resources", Canvas);


        //if resource is null is another Form. The MainMenu Form 
        if (_resources != null)
        {
            _startPosIni = GetChildCalled(H.Start, Resources);
            //LoadMainInventory();
        }

        //is the main gui 
        if (transform.name.Contains("MainGUI"))
        {
            _showPathToSea = new ShowPathTo();
            _miniHelper = FindObjectOfType<MiniHelper>();
        }

        InitIconsAndNotis();
    }

    #region GUI Hides Shows
    static GameObject _autoSaveIcon;
    static GameObject _warModeNoti;

    void InitIconsAndNotis()
    {
        if (name.Contains("GUI"))
        {
            _autoSaveIcon = General.FindGameObjectInHierarchy("AutoSave_Icon", gameObject);
            _warModeNoti = General.FindGameObjectInHierarchy("WarMode", gameObject);
        }

        if (_autoSaveIcon != null)
        {
            _autoSaveIcon.SetActive(false);
            _warModeNoti.SetActive(false);
        }
    }

    public void ShowAutoSave()
    {
        if (_autoSaveIcon == null)
        {
            return;
        }

        _autoSaveIcon.SetActive(true);
    }

    public void HideAutoSaveIcon()
    {
        if (_autoSaveIcon == null)
        {
            return;
        }

        _autoSaveIcon.SetActive(false);
    }

    public void ShowWarMode()
    {
        if (_warModeNoti == null)
        {
            return;
        }

        _warModeNoti.SetActive(true);
    }

    public void HideWarMode()
    {
        if (_warModeNoti == null)
        {
            return;
        }

        _warModeNoti.SetActive(false);
    }
    #endregion

    private IEnumerator UpdateEvery2Sec()
    {
        while (true)
        {
            yield return new WaitForSeconds(2); // wait
            //if (_showAInventory != null)
            //{
            //    _showAInventory.UpdateEvery2Sec();
            //}
        }
    }

    private IEnumerator UpdateEveryMinute()
    {
        while (true)
        {
            yield return new WaitForSeconds(67); // wait
            //if (_showAInventory != null)
            //{
            //    _showAInventory.UpdateEveryMinute();
            //}
        }
    }

    bool isShownNow = true;
    // Update is called once per frame
    void Update()
    {
        //if (_showAInventory != null)
        //{
        //    _showAInventory.Update();
        //}
        if (_showPathToSea != null)
        {
            _showPathToSea.Update();
        }

        if (_miniHelper!= null && Input.GetKeyUp(KeyCode.F1))
        {
            ManagerReport.AddInput("KeyCode.F1");
            Program.MouseListener.HelpWindow.Show("");
        }


        if (Input.GetKeyUp(KeyCode.F9))
        {
            if (isShownNow)
                HideCanvas();
            else
                ShowCanvas();

           isShownNow = !isShownNow;
        }

    }

    private void ShowCanvas()
    {
        _canvas.SetActive(true);
    }

    private void HideCanvas()
    {
        _canvas.SetActive(false);
    }

    protected GameObject GetChildCalled(H childName, GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).name == childName.ToString())
            {
                return parent.transform.GetChild(i).gameObject;
            }
        }
        print("Obj doesnt have a child called " + childName);
        return null;
    }

    //Returns true if is overlapping the Panel 2dCollider
    //Panel must have a 2d collidaer attached
    public bool IsOverLapingPanel(Vector2 pos)
    {
        Collider2D c = Panel.GetComponent<Collider2D>();

        if (c.OverlapPoint(pos))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Hides the element 
    /// </summary>
    public virtual void Hide()
    {
        Vector3 newPos = transform.position;
        newPos.y = -1400f;

        transform.position = newPos;
    }

    internal void Show()
    {
        Vector3 newPos = transform.position;
        newPos.y = +1400f;

        transform.position = newPos;
    }

    #region Set thru events on Editor

    /// <summary>
    /// Called by the WhereIsSea on GUI
    /// Set thru events on Editor
    /// </summary>
    public void ShowHideSeaPathToggle()
    {
        if (_showPathToSea != null)
        {
            _showPathToSea.Toggle();
        }
    }

    public void ShowRegions()
    {
        MeshController.BuyRegionManager1.ShowRegionsToggle();
    }

    /// <summary>
    /// Called by the WhereIsTown on Gui
    /// </summary>
    public void CenterCamToTown()
    {
        CamControl.CAMRTS.InputRts.CenterCam(true);
    }

    /// <summary>
    /// Called by the Helper on Gui
    /// </summary>
    public void Helper()
    {
        _miniHelper.Show();
    }

    #endregion
}


