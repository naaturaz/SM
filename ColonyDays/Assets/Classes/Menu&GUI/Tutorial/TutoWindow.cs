using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

class TutoWindow : GUIElement
{

    /// <summary>
    /// To add tutorial 
    /// add the corresponding entry on Languages and 
    /// then make sure the step is being completed with the call from some
    /// action .... so it needs to be called from somewhere so the step is done
    /// </summary>
    List<string> _steps = new List<string>()
    {
        "CamMov.Tuto",
        "CamMov5x.Tuto",
        "CamRot.Tuto",
        "CamHeaven.Tuto",
        "BackToTown.Tuto",
        "BuyRegion.Tuto",//they most be one after
        "Trade.Tuto",//the other
        "Dock.Tuto",
        "Dock.Placed.Tuto",
        "MaxSpeed.Tuto",
        "ShowWorkersControl.Tuto",
        "AddWorkers.Tuto",
        "HideBulletin.Tuto",
        "FinishDock.Tuto",
        "ShowMiniHelp.Tuto",
        
        "SelectDock.Tuto",
        "OrderTab.Tuto",
        "ImportOrder.Tuto",
        "AddOrder.Tuto",
        "CloseDockWindow.Tuto",
        "Rename.Tuto",
    };

    //which is being shown now 
    int _currentIndex;

    private Text _text;
    private RectTransform _rectTransform;
    private Vector3 _iniPos;
    GameObject _showAgainTuto;

    void Start()
    {
        //the helper btn
        var helper = FindGameObjectInHierarchy("Helper", ReturnMainGUI().gameObject);
        _iniPos = transform.position;

        var childText = GetChildCalled("Text");
        _text = childText.GetComponent<Text>();

        _rectTransform = transform.GetComponent<RectTransform>();


        Hide();
    }


    bool wasShown;
    void Update()
    {
        if (Program.gameScene.GameFullyLoaded() && !wasShown
            //XMLSerie.IsTutorial() &&
            )
        {
            wasShown = true;
            Show();
        }
    }

    MyForm ReturnMainGUI()
    {
        var forms = FindObjectsOfType<MyForm>();

        for (int i = 0; i < forms.Count(); i++)
        {
            if (forms[i].name.Contains("GUI"))
            {
                return forms[i];
            }
        }
        return null;
    }

    public void Hide()
    {
        if (_rectTransform == null)
        {
            return;
        }

        _rectTransform.position = new Vector3(2500, 2500);
        _text.text = "";
    }

    internal void Show()
    {
        //when retake from Skipped 
        if (_currentIndex < 0)
        {
            _currentIndex = 0;
        }

        if (_showAgainTuto == null)
        {
            _showAgainTuto = GameObject.Find("ShowAgainTuto");
        }
        _showAgainTuto.SetActive(false);

        AudioCollector.PlayOneShotFullAudio("ClickMetal2");
        Program.MouseListener.HideAllWindows();

        transform.position = _iniPos;

        var which = _steps[_currentIndex];
        _text.text = Languages.ReturnString(which);
    }

    public void Next(string step)
    {
        if (_currentIndex == -1 || step != _steps[_currentIndex])
        {
            return;
        }

        ManagerReport.AddInput("Tuto.Step.Achieved:" + _steps[_currentIndex]);
        _currentIndex++;

        if (_currentIndex >= _steps.Count)
        {
            _currentIndex = -1;
            Hide();
            //Dialog.OKDialog(H.TutoOver);
            ManagerReport.AddInput("Tutorial.Done:");
            QuestManager.QuestFinished("Tutorial");
            return;
        }
        //QuestManager.QuestFinished("Tutorial");
        Show();
    }

    public bool IsPassingTheTutoNow()
    {
        return _currentIndex != -1;
    }


    /// <summary>
    /// Called from GUI
    /// </summary>
    public void Prev()
    {
        _currentIndex = UMath.Clamper(-1, _currentIndex, 0, _steps.Count - 1);
        Show();
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void SkipTuto()
    {
        AudioCollector.PlayOneShot("ClickMetal2", 0);
        _currentIndex = -1;
        Hide();

        _showAgainTuto.SetActive(true);
    }
}







