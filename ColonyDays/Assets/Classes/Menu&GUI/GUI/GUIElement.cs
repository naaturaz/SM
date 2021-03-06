﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIElement : General
{
    //inipos is used for Hide and show
    private Vector3 iniPos;

    protected InputField _titleInputField;
    protected GameObject _titleInputFieldGO;
    private Scrollbar _verticScrollbar;

    private RectTransform _scrollContent;
    protected Text _text;

    private float speed = .05f;
    protected bool _hideSlideToLeft;
    private RectTransform _rectTransform;

    public Vector3 IniPos
    {
        get
        {
            return iniPos;
        }

        set
        {
            iniPos = value;
        }
    }

    protected Rect _genBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    protected Rect _invBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    protected Rect _ordBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    protected Rect _prdBtnRect;//the rect area of my Gen_Btn. Must have attached a BoxCollider2D
    protected Rect _upgBtnRect;
    protected Rect _staBtnRect;

    protected GameObject _genBtn;//the btn
    protected GameObject _invBtn;//the btn
    protected GameObject _ordBtn;//the btn
    protected GameObject _upgBtn;
    protected GameObject _prdBtn;//the btn
    protected GameObject _staBtn;//the btn

    // Use this for initialization
    protected void Start()
    {
        var textGo = FindGameObjectInHierarchy("Text", gameObject);
        if (textGo != null)
            _text = textGo.GetComponent<Text>();

        _titleInputFieldGO = GetGrandChildCalled("TitleInputField");
        if (!_titleInputFieldGO) return;
        _titleInputField = _titleInputFieldGO.GetComponent<InputField>();
        _titleInputFieldGO.SetActive(false);
    }

    //called from GUI
    public void HideWindowToTheLeft()
    {
        _hideSlideToLeft = true;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (_hideSlideToLeft)
        {
            Vector2 newPos = new Vector2(transform.position.x - speed, transform.position.y);
            transform.position = newPos;
            speed *= 2.2f;

            if (transform.position.x <= IniPos.x - 400f)
            {
                _hideSlideToLeft = false;
                speed = .02f;
            }
        }
    }

    public void AdjustContentHeight(float size)
    {
        if (_scrollContent == null)
            _scrollContent = FindGameObjectInHierarchy("Content", gameObject).GetComponent<RectTransform>();

        _scrollContent.sizeDelta = new Vector2(_scrollContent.sizeDelta.x, size);
    }

    /// <summary>
    /// Will build the string to show cost
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    protected string BuildStringInv(General obj)
    {
        if (obj.Inventory == null)
            return ">*<";

        var percentOcup = obj.Inventory.CurrentVolumeOcuppied() / obj.Inventory.CapacityVol;
        percentOcup = percentOcup * 100;

        if (percentOcup > 100)
            percentOcup = 100;

        var volOccupied = Unit.VolConverted(obj.Inventory.CurrentVolumeOcuppied());
        var volCap = Unit.VolConverted(obj.Inventory.CapacityVol);

        if (volOccupied > volCap)
        {
            volOccupied = volCap;
        }
        var res = Languages.ReturnString("Occupied:") + " " + volOccupied.ToString("F0") + " " + Unit.VolumeUnit() +
            Languages.ReturnString(" of ") + volCap.ToString("F0") + " " + Unit.VolumeUnit() +
            " @ " + percentOcup.ToString("F0") + "%";

        return res;
    }

    internal void MovedTo(Vector2 anchoredPosition)
    {
        IniPos = anchoredPosition;
        ReloadTabsRects();
    }

    protected void ReloadTabsRects()
    {
        _genBtn = GetChildThatContains(H.Gen_Btn);
        _invBtn = GetChildThatContains(H.Inv_Btn);
        _ordBtn = GetChildThatContains(H.Ord_Btn);

        _upgBtn = GetChildThatContains(H.Upg_Btn);
        _prdBtn = GetChildThatContains(H.Prd_Btn);
        _staBtn = GetChildCalled("Sta_Btn");

        if (_genBtn != null)
            _genBtnRect = GetRectFromBoxCollider2D(_genBtn.transform);
        if (_invBtn != null)
            _invBtnRect = GetRectFromBoxCollider2D(_invBtn.transform);
        if (_ordBtn != null)
            _ordBtnRect = GetRectFromBoxCollider2D(_ordBtn.transform);

        if (_upgBtn != null)
            _upgBtnRect = GetRectFromBoxCollider2D(_upgBtn.transform);
        if (_prdBtn != null)
            _prdBtnRect = GetRectFromBoxCollider2D(_prdBtn.transform);
        if (_staBtn != null)
            _staBtnRect = GetRectFromBoxCollider2D(_staBtn.transform);
    }

    //called from GUI Event Element.
    //so far from NotificationWindow and Feedback Dialog
    public void CallOnMouseEnter()
    {
        MouseListener.MouseOnWindowNow = true;
        Debug.Log("Mouse Enter");
    }

    public void CallOnMouseExit()
    {
        MouseListener.MouseOnWindowNow = false;
        Debug.Log("Mouse Exit");
    }

    protected Rect GetRectFromBoxCollider2D(Transform t)
    {
        var res = new Rect();
        var min = t.GetComponent<BoxCollider2D>().bounds.min;
        var max = t.GetComponent<BoxCollider2D>().bounds.max;

        res = new Rect();
        res.min = min;
        res.max = max;

        return res;
    }

    /// <summary>
    /// Hides the element
    /// </summary>
    public virtual void Hide()
    {
        Program.UnLockInputSt();

        if (name.Contains("Bulletin"))
        {
            Program.gameScene.TutoStepCompleted("HideBulletin.Tuto");
        }
        if (name.Contains("Building"))
        {
            Program.gameScene.TutoStepCompleted("CloseDockWindow.Tuto");
        }

        if (_rectTransform == null)
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
            IniPos = _rectTransform.anchoredPosition;
        }

        Vector3 newPos = _rectTransform.anchoredPosition;
        newPos.y = -800f;
        _rectTransform.anchoredPosition = newPos;
    }

    public void Show()
    {
        
        _rectTransform.anchoredPosition = IniPos;
        SelectOkBtn();
        ReloadTabsRects();
    }

    internal void HideArrow()
    {
        var arrowGO = GetChildCalled("Arrow");

        if (arrowGO == null)
            return;

        arrowGO.SetActive(false);
    }

    public bool IsShownNow()
    {
        return transform.position.y > 0;
    }

    protected void MakeAlphaColorZero(Text g)
    {
        Color bl = Color.white;
        bl.a = 0f;
        g.color = bl;
    }

    protected void MakeAlphaColorMax(Text g)
    {
        Color bl = Color.white;
        bl.a = 255f;
        g.color = bl;
    }

    internal void ResetScroolPos()
    {
        if (_verticScrollbar == null)
        {
            _verticScrollbar = FindGameObjectInHierarchy("Scrollbar Vertical", gameObject).GetComponent<Scrollbar>();
        }

        _verticScrollbar.value = 1;
    }

    private void SelectOkBtn()
    {
        var okBtn = GetChildCalled("Ok_Btn");

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(okBtn, null);
    }
}