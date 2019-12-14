﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

class MiniHelper : GUIElement
{
    //helps available. u can add anything here, but need to be add on Langugaes.cs
    List<string> _helps = new List<string>()
    {
        "Construction.HoverMed",
        "Camera",
        "SeaPath",
        "PeopleRange",
        "PirateThreat.Help",
        "PortReputation.Help",
        //"Emigrate.Help",
        "Food.Help",
        "Weight.Help",
        "More.Help"
    };

    //which is being shown now 
    private int _currentIndex;

    private Text _text;
    private RectTransform _rectTransform;
    private Vector3 _iniPos;

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
        AudioCollector.PlayOneShotFullAudio("ClickMetal2");
        Program.MouseListener.HidePersonBuildOrderNotiWindows();

        transform.position = _iniPos;

        var which = _helps[_currentIndex];
        _text.text = Languages.ReturnString(which);

        Program.MouseListener.TutoWindow1.HideArrow();
        Program.MouseListener.BulletinWindow.Hide();
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void Next()
    {
        _currentIndex = UMath.GoAround(1, _currentIndex, 0, _helps.Count-1);
        Show();
    }

    /// <summary>
    /// Called from GUI
    /// </summary>
    public void Prev()
    {
        _currentIndex = UMath.GoAround(-1, _currentIndex, 0, _helps.Count - 1);
        Show();
    }

    public void ShowConstructionHelp()
    {
        _currentIndex = 0;
        Show();
    }
}

