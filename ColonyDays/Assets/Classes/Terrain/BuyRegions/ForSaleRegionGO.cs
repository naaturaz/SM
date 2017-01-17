using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ForSaleRegionGO : Hoverable
{
    private int _index;
    private Rect _region;

    BigBoxPrev buildingPrev;


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
        obj = (ForSaleRegionGO)Instantiate(obj, U2D.FromV2ToV3(region.center) , Quaternion.identity);
        obj.HType = hType;

        obj.MyId += " " + index;

        obj.Index = index;
        obj.Region = region;

        if (container != null) { obj.transform.SetParent( container); }
        return obj;
    }

    void Start()
    {
        var poly = U2D.FromRectToPoly(Region);
        buildingPrev = (BigBoxPrev)CreatePlane.CreatePlan(Root.bigBoxPrev, Root.dashedLinedSquare, container: transform);
        buildingPrev.UpdatePos(poly, .25f);
        buildingPrev.transform.position = U2D.FromV2ToV3(Region.center);
        
        //MeshController.CrystalManager1.CrystalRegions[Index].StartWithAudioReport();
        Name = "Buy region";
        HType = H.BuyRegion;
    }

    void Update()
    {

    }

    void OnMouseUp()
    {
        if (Program.IsInputLocked || Dialog.IsActive())
        {
            return;
        }

        //in case one is up. gets destroyed
        Dialog.Listen("Dialog.Cancel");

        MeshController.BuyRegionManager1.SetCurrentRegion(Index);

        Debug.Log(name);
        Dialog.OKCancelDialog(H.BuyRegion);
    }


    #region Hover All Objects. All objects that have a collider will be hoverable

    protected void OnMouseEnter()
    {
        base.OnMouseEnter();
    }

    protected void OnMouseExit()
    {
        base.OnMouseExit();
    }

    #endregion
}
