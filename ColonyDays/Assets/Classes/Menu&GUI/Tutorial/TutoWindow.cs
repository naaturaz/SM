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
        "BackToTown.Tuto",
        "BuyRegion.Tuto",//they most be one after

        "CamHeaven.Tuto",
        "Trade.Tuto",//the other

        "Dock.Tuto",
        "Dock.Placed.Tuto",
        "2XSpeed.Tuto",
        "ShowWorkersControl.Tuto",
        "AddWorkers.Tuto",
        "HideBulletin.Tuto",
        "FinishDock.Tuto",
        "ShowHelp.Tuto",
        
        "SelectDock.Tuto",
        "OrderTab.Tuto",
        "ImportOrder.Tuto",
        "AddOrder.Tuto",
        "CloseDockWindow.Tuto",
        "Rename.Tuto",

        "Budget.Tuto",
        "Prod.Tuto",
        "Spec.Tuto",
        "Exports.Tuto",


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

        if (Program.WasTutoPassed || !string.IsNullOrEmpty(PlayerPrefs.GetString("Tuto")))
        {
            SkipTuto();
        }
    }


    bool wasShown;
    void Update()
    {
        if (Program.gameScene.GameFullyLoaded() && !wasShown && string.IsNullOrEmpty(PlayerPrefs.GetString("Tuto")))
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

        _rectTransform.position = new Vector3(0, -2500);
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

        AudioCollector.PlayOneShotFullAudio("ClickWood1");
        Program.MouseListener.HidePersonBuildOrderNotiWindows();

        transform.position = _iniPos;

        var which = _steps[_currentIndex];
        _text.text = Languages.ReturnString(which);
    }



    public void Next(string step)
    {
        if (_showAgainTuto == null)
        {
            return;
        }

        if (_currentIndex == -1 || step != _steps[_currentIndex] || _showAgainTuto.activeSelf)
        {
            return;
        }

        HideArrow();
        ManagerReport.AddInput("Tuto.Step.Achieved:" + _steps[_currentIndex]);
        _currentIndex++;

        if (_currentIndex >= _steps.Count)
        {
            _currentIndex = -1;
            Hide();
            
            //temporal
            Dialog.OKDialog(H.TutoOver);

            if (!Program.WasTutoPassed)
            {
                Program.gameScene.GameController1.Dollars += 10000;
                AudioCollector.PlayOneShot("BoughtLand", 0);
                BulletinWindow.SubBulletinFinance1.FinanceLogger.AddToAcct("Quests Completion", 10000);
            }
            
            ManagerReport.AddInput("Tutorial.Done:");
            Program.gameScene.QuestManager.QuestFinished("Tutorial");

            Program.WasTutoPassed = true;
            PlayerPrefs.SetString("Tuto", "Done");


            return;
        }
        //QuestManager.QuestFinished("Tutorial");
        Show();
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
        AudioCollector.PlayOneShot("ClickWood1", 0);
        //_currentIndex = -1;
        Hide();

        if (_showAgainTuto == null)
        {
            _showAgainTuto = GameObject.Find("ShowAgainTuto");
        }
        _showAgainTuto.SetActive(true);

        Program.gameScene.QuestManager.TutoCallWhenDone();
        PlayerPrefs.SetString("Tuto", "Skip");

    }


    #region Manage delay on tutorials

    static float _thisStepStartedAt = -1;
    static string _thisStepIs = "";

    static public bool IsStepReady(string step)
    {
        if (_thisStepStartedAt == -1 && _thisStepIs == "")
        {
            _thisStepIs = step;
            _thisStepStartedAt = Time.time;
            return false;
        }

        if (Time.time > _thisStepStartedAt + 5)
        {
            _thisStepIs = "";
            _thisStepStartedAt = -1;
            return true;
        }

        return false;
    }

    #endregion

}







