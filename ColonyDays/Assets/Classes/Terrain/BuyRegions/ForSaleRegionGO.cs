using UnityEngine;

public class ForSaleRegionGO : Hoverable
{
    private bool _isUpNow;

    private int _index;
    private Rect _region;

    private BigBoxPrev buildingPrev;
    private Animator _animator;

    //GameObject _hover;

    /// <summary>
    /// The index of the region
    /// </summary>
    public int Index
    {
        get { return _index; }
        set { _index = value; }
    }

    public Rect Region
    {
        get { return _region; }
        set { _region = value; }
    }

    static public ForSaleRegionGO CreateForSaleRegionGO(string root, int index, Rect region, string name = "", Transform container = null,
    H hType = H.None)
    {
        ForSaleRegionGO obj = null;
        obj = (ForSaleRegionGO)Resources.Load(root, typeof(ForSaleRegionGO));
        obj = (ForSaleRegionGO)Instantiate(obj, U2D.FromV2ToV3(region.center), Quaternion.identity);
        obj.HType = hType;

        obj.MyId += " " + index;

        obj.Index = index;
        obj.Region = region;

        if (container != null) { obj.transform.SetParent(container); }
        return obj;
    }

    private void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        //_hover = GetChildCalled("Hover");
        //_hover.gameObject.SetActive(false);

        var poly = U2D.FromRectToPoly(Region);
        buildingPrev = (BigBoxPrev)CreatePlane.CreatePlan(Root.bigBoxPrev, Root.dashedLinedSquare, container: transform);
        buildingPrev.UpdatePos(poly, .25f);
        buildingPrev.transform.position = U2D.FromV2ToV3(Region.center);

        //MeshController.CrystalManager1.CrystalRegions[Index].StartWithAudioReport();
        Name = "Buy region";
        HType = H.BuyRegion;

        transform.position += new Vector3(0, 10000, 0);
    }

    private bool _isShown;

    private void Update()
    {
        if (!_isShown && MeshController.BuyRegionManager1.IsToShowNow())
        {
            _isShown = true;
        }

        if (_isShown && !MeshController.BuyRegionManager1.IsToShowNow())
        {
            _isShown = false;
        }

        AddressShow();
        AddressHide();
        //CheckMouse();
    }

    private void AddressHide()
    {
        if (_isUpNow && !_isShown)
        {
            transform.position += new Vector3(0, 10000, 0);
            _isUpNow = false;
        }
    }

    private void AddressShow()
    {
        if (!_isUpNow && _isShown)
        {
            transform.position += new Vector3(0, -10000, 0);
            _isUpNow = true;
        }
    }

    /// <summary>
    /// On mouse click
    /// </summary>
    public void ClickOnMe()
    {
        if (Program.IsInputLocked || Dialog.IsActive() || BuildingPot.Control.CurrentSpawnBuild != null)
        {
            return;
        }

        //in case one is up. gets destroyed
        Dialog.Listen("Dialog.Cancel");
        MeshController.BuyRegionManager1.SetCurrentRegion(Index);
        Dialog.OKCancelDialog(H.BuyRegion);
    }

    #region Hover All Objects. All objects that have a collider will be hoverable

    private float lastMouseOver;

    private void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    //protected void OnMouseOver()
    //{
    //    lastMouseOver = Time.time;
    //    _hover.SetActive(true);
    //}

    protected void OnMouseExit()
    {
        base.OnMouseExit();
    }

    //void CheckMouse()
    //{
    //    if (Time.time > lastMouseOver + 0.5f)
    //    {
    //        _hover.SetActive(false);
    //    }
    //}

    #endregion Hover All Objects. All objects that have a collider will be hoverable
}