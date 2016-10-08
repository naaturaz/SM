using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


class HoverWindowMed : MonoBehaviour
{
    private string _msg;

    //windows props
    private Vector3 _min;
    private Vector3 _max;

    private GameObject _geometry;
    private Text _text;

    private RectTransform _rectTransform;

    //the Key that is showing this HoverWindow. 
    //needed to pull the string from Languages.cs
    private string _key;

    //the medium hover window

    // Use this for initialization
    void Start()
    {
        _geometry = General.FindGameObjectInHierarchy("Geometry", gameObject);
        var textGO = General.FindGameObjectInHierarchy("Text", gameObject);

        if (textGO != null)
        {
            _text = textGO.GetComponent<Text>();
        }


        _rectTransform = transform.GetComponent<RectTransform>();
        Hide();
    }
    public void Hide()
    {
        _rectTransform.position = new Vector3(500, 500);
        _msg = "";
        _text.text = "";
        _geometry.SetActive(false);
    }
    public void Show(Vector3 pos, string key)
    {
        _key = key;
        _msg = Languages.ReturnString(key + ".HoverMed");
        _rectTransform.position = pos;

        _text.text = _msg;
        _geometry.SetActive(true);

        diffToMouse = pos - Input.mousePosition;
        showedAt = Time.time;
    }

    public string CurrentMsg()
    {
        return _msg;
    }


    //so mouse never touches this one. so it doesnt trigger a OnMouseExit on the spawer
    //of this window
    private Vector3 diffToMouse;

    //when was showed
    private float showedAt;

    // Update is called once per frame
    void Update()
    {
        //means is hiding.
        //and wont let any other click happen if is on front of everything 
        if (string.IsNullOrEmpty(_msg))
        {
            return;
        }

        _rectTransform.position = Input.mousePosition + diffToMouse;

        //after 3 seconds of being show
        if (Time.time > showedAt + 3)
        {
            //SpawnMedHover();
        }
    }

    /// <summary>
    /// To show small tutorials like how to place a building 
    /// </summary>
    /// <param name="key"></param>
    internal void ShowSemiTut(string key)
    {
        Show( Input.mousePosition + new Vector3(150,150,150), key);
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.KeyUp || Event.current.type == EventType.MouseUp)
        {
            Hide();
        }
    }

}

