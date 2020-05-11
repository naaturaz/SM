﻿using System.Collections.Generic;
using UnityEngine;

//http://ralphbarbagallo.com/2012/04/09/3-ways-to-capture-a-screenshot-in-unity3d/
public class MiniMapRTS : GenericCameraComponent
{
    public static bool isMouseOnGUI;
    public static bool isOnTheLimits;

    private Vector3 mP;//mouse pos

    //Rect mapDimRect; the rect of the MiniMap
    private float xStart = 20;

    private float yStart = 20;

    public Texture2D tex;
    public static Texture2D tex2;//debug green semitransparant
    public Texture2D tex3;//debug red semitransparant

    protected float oldScreenWidth;
    protected float oldScreenHeight;

    protected float widthProportionSize = 8;
    protected float heightProportionSize = 6;

    private List<Transform> cardinalPointsTransf;
    private Vector2 NW, NE, SW, SE;

    //reducing the normal size of map
    private Vector2 reducedNE, reducedSW, reducedNW;

    //this is the Variable that reduce the map limits in where the camera can go
    //todo a X and Z reduction is needed
    private float reductionX = 30;//50

    private float reductionY = 60;//50

    private float terraStartX;
    private float terraStartY;
    private float terraEndX;
    private float terraEndY;
    private float terraWidth;
    private float terraHeight;

    private Vector3 oldPos;

    private Transform centerTarget;

    // Use this for initialization
    private void Start()
    {
        tex2 = tex;

        oldScreenWidth = Screen.width;
        oldScreenHeight = Screen.height;

        //mapDimRect = new Rect(xStart, yStart, oldScreenWidth / widthProportionSize, oldScreenHeight / heightProportionSize);

        GetCardinals();
        SetReducedCardinals();
        GetTerrainSpecs();
    }

    private Rect mapRect = new Rect();

    private void SetReducedCardinals()
    {
        reducedNE = new Vector2(NE.x - reductionX, NE.y - reductionY);
        reducedSW = new Vector2(SW.x + reductionX, SW.y + reductionY);

        reducedNW = new Vector2(NW.x + reductionX, NW.y - reductionY);

        mapRect = Registro.FromALotOfVertexToRect(new List<Vector3>() { reducedNW, reducedNE, reducedSW });
    }

    public bool IsOnMapConstraints(Vector3 pos)
    {
        return mapRect.Contains(pos);
    }

    // Update is called once per frame
    private void Update()
    {
        if (CamControl.CAMRTS.centerTarget != null && centerTarget == null)
        {
            centerTarget = CamControl.CAMRTS.centerTarget.transform;
        }

        CheckIfMouseInMap();
        UpdateRectDim();
    }

    public Vector3 ConstrainLimits(Vector3 newPos)
    {
        Vector3 limit = newPos;
        if (newPos.x < reducedSW.x)
        {
            limit.x = reducedSW.x;
        }
        if (newPos.z < reducedSW.y)
        {
            limit.z = reducedSW.y;
        }
        if (newPos.x > reducedNE.x)
        {
            limit.x = reducedNE.x;
        }
        if (newPos.z > reducedNE.y)
        {
            limit.z = reducedNE.y;
        }
        return limit;
    }

    private void UpdateRectDim()
    {
        if (oldScreenHeight != Screen.height || oldScreenWidth != Screen.width)
        {
            //mapDimRect.width = Screen.width / widthProportionSize;
            //mapDimRect.height = Screen.height / heightProportionSize;

            oldScreenWidth = Screen.width;
            oldScreenHeight = Screen.height;
        }
    }

    private void GetCardinals()
    {
        cardinalPointsTransf = new List<Transform>();
        Transform terrain = Program.gameScene.Terreno.transform;
        //the gameChilds need to be like following order in game hierachy:
        // NW, NE, SW, SE
        for (int i = 0; i < terrain.transform.childCount; i++)
        {
            cardinalPointsTransf.Add(terrain.transform.GetChild(i));
        }
        NW = new Vector2(cardinalPointsTransf[0].transform.position.x,
cardinalPointsTransf[0].transform.position.z);

        NE = new Vector2(cardinalPointsTransf[1].transform.position.x,
cardinalPointsTransf[1].transform.position.z);

        SW = new Vector2(cardinalPointsTransf[2].transform.position.x,
cardinalPointsTransf[2].transform.position.z);

        SE = new Vector2(cardinalPointsTransf[3].transform.position.x,
cardinalPointsTransf[3].transform.position.z);
    }

    /// <summary>
    /// Get all the terrain specs
    /// </summary>
    private void GetTerrainSpecs()
    {
        terraStartX = SW.x;
        terraStartY = SW.y;
        terraEndX = NE.x;
        terraEndY = NE.y;

        terraWidth = terraEndX - terraStartX;
        terraHeight = terraEndY - terraStartY;
    }

    /// <summary>
    /// If mouse in on minimap rectangle and was clicked
    /// this method will return the world pos the cam has to be moved to
    /// </summary>
    /// <returns></returns>
    private Vector3 ReturnWorldPos()
    {
        //float unitPerPixelsX = terraWidth / mapDimRect.width;
        //float unitPerPixelsY = terraHeight / mapDimRect.height;

        //float scaleLengthX = (mP.x - mapDimRect.x) * unitPerPixelsX;
        //float scaleLengthY = (mP.y - mapDimRect.y) * unitPerPixelsY;

        //float scaleX = SW.x + scaleLengthX;
        //float scaleY = SW.y + scaleLengthY;

        //return new Vector3(scaleX, centerTarget.position.y,
        //    scaleY);

        return new Vector3();
    }

    private void CheckIfMouseInMap()
    {
        mP = Input.mousePosition;
        //if (mapDimRect.Contains(mP))
        //{
        //    if(Input.GetMouseButtonUp(0) && !InputRTS.isFollowingPersonNow)
        //    {
        //        centerTarget.position = ReturnWorldPos();
        //    }
        //    isMouseOnGUI = true;
        //}
        //else isMouseOnGUI = false;
    }

    private void OnGUI()
    {
        if (tex == null)
        {
            return;
        }

        //Rect mapDimRectDraw = U2D.ReturnDrawRectYInverted(mapDimRect);
        //GUI.DrawTexture(mapDimRectDraw, tex);

        //DebugAllBuildingOnTerrainColl();
        //DebugFirstPersonRoutesArea();
    }

    private void DebugAllBuildingOnTerrainColl()
    {
        for (int i = 0; i < Registro.toDraw.Count; i++)
        {
            int mangifyMultpiplier = 10;//used to be 10
            Rect bh = (Registro.toDraw[i]);
            bh = Magnify(bh, mangifyMultpiplier);
            //other buildings
            GUI.DrawTexture(bh, tex);
            //current building
            GUI.DrawTexture(Magnify(Registro.curr, mangifyMultpiplier), tex2);
        }
    }

    private void DrawMagnify(Rect toDrawP, int xZoom, Texture2D texP)
    {
        Rect mag = (toDrawP);
        mag = Magnify(mag, xZoom);
        GUI.DrawTexture(Magnify(toDrawP, xZoom), texP);
    }

    private Rect Magnify(Rect to, int times)
    {
        to = new Rect(to.x * times, to.y * times, to.width * times, to.height * times);
        return to;
    }

    //private void DebugFirstPersonRoutesArea()
    //{
    //    if (PersonPot.Control != null)
    //    {
    //        Person p = PersonPot.Control.All.ElementAt(0).Value;
    //        if (p != null && p.Brain.IsAllSet)
    //        {
    //            for (int i = 0; i < p.Brain.DebugRect.Length; i++)
    //            {
    //                DrawMagnify(p.Brain.DebugRect[i], 10, tex3);
    //            }
    //        }
    //    }
    //}
}