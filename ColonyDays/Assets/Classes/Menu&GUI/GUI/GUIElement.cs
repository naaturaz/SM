using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIElement : General
{

    //inipos is used for Hide and show 
    protected Vector3 iniPos;

    protected InputField _titleInputField;
    protected GameObject _titleInputFieldGO;
    private Scrollbar _verticScrollbar;

    RectTransform _scrollContent;

    // Use this for initialization
    protected void Start()
    {

        _titleInputFieldGO = GetGrandChildCalled("TitleInputField");
        _titleInputField = _titleInputFieldGO.GetComponent<InputField>();
        _titleInputFieldGO.SetActive(false);

    }




    // Update is called once per frame
    protected void Update()
    {


    }

    public void Show()
    {
        transform.position = iniPos;
    }

    public bool IsShownNow()
    {
        return transform.position.y > 0;
        //return UMath.nearEqualByDistance(transform.position, iniPos, 0.3f);
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
        if (name.Contains("Bulletin"))
        {
            Program.gameScene.TutoStepCompleted("HideBulletin.Tuto");
        }
        if (name.Contains("Building"))
        {
            Program.gameScene.TutoStepCompleted("CloseDockWindow.Tuto");
        }

        Vector3 newPos = transform.position;
        newPos.y = -800f;
        transform.position = newPos;
    }


    /// <summary>
    /// Will build the string to show cost 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    protected string BuildStringInv(General obj)
    {
        if (obj.Inventory == null)
        {
            return ">*<";
        }

        var percentOcup = obj.Inventory.CurrentVolumeOcuppied() / obj.Inventory.CapacityVol;
        percentOcup = percentOcup * 100;

        if (percentOcup > 100)
        {
            percentOcup = 100;
        }

        var volOccupied = Unit.VolConverted(obj.Inventory.CurrentVolumeOcuppied());
        var volCap = Unit.VolConverted(obj.Inventory.CapacityVol);

        if (volOccupied > volCap)
        {
            volOccupied = volCap;
        }
        var res = Languages.ReturnString("Occupied:") + volOccupied.ToString("F0") + " " + Unit.VolumeUnit() +
            " of " + volCap.ToString("F0") + " " + Unit.VolumeUnit() +
            " @ " + percentOcup.ToString("F0") + "%";

        return res;
    }

    internal void ResetScroolPos()
    {

        if (_verticScrollbar == null)
        {
            _verticScrollbar = FindGameObjectInHierarchy("Scrollbar Vertical", gameObject).GetComponent<Scrollbar>();
        }

        _verticScrollbar.value = 1;
    }

    public void AdjustContentHeight(float size)
    {
        if (_scrollContent == null)
        {
            _scrollContent = FindGameObjectInHierarchy("Content", gameObject).GetComponent<RectTransform>();
        }

        _scrollContent.sizeDelta = new Vector2(_scrollContent.sizeDelta.x, size);
    }


    internal void HideArrow()
    {
        var arrowGO = GetChildCalled("Arrow");

        if (arrowGO == null)
        {
            return;
        }

        arrowGO.SetActive(false);
    }







    //called from GUI Event Element. 
    //so far from NotificationWindow and Feedback Dialog
    public void CallOnMouseEnter()
    {
        MouseListener.MouseOnWindowNow = true;
        Debug.Log("Mouse Eneter");
    }

    public void CallOnMouseExit()
    {
        MouseListener.MouseOnWindowNow = false;
        Debug.Log("Mouse Exit");
    }






}
