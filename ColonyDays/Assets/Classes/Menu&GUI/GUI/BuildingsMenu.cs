using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/*
 * This is the menu where u can select Buildings from a specific category 
 * 
 */

public class BuildingsMenu : GUIElement
{
    private Vector3 iniPos;

    private GameObject slot1;
    private GameObject slot2;
    private GameObject slot3;
    private GameObject slot4;
    private GameObject slot5;

    private GameObject slot6;
    private GameObject slot7;
    private GameObject slot8;
    private GameObject slot9;
    private GameObject slot10;

    List<GameObject> slots = new List<GameObject>();
    
    private Sprite _lockedSprite; //the sprite when a Build is locked 

	// Use this for initialization
	void Start ()
	{
	    InitObj();

	    Hide();
	}

    private void InitObj()
    {
        iniPos = transform.position;

        slot1 = GetChildThatContains(H.Slot1);
        slot2 = GetChildThatContains(H.Slot2);
        slot3 = GetChildThatContains(H.Slot3);
        slot4 = GetChildThatContains(H.Slot4);
        slot5 = GetChildThatContains(H.Slot5);

        slot6 = GetChildThatContains(H.Slot6);
        slot7 = GetChildThatContains(H.Slot7);
        slot8 = GetChildThatContains(H.Slot8);
        slot9 = GetChildThatContains(H.Slot9);
        slot10= GetChildThatContains(H.Slot10);

        slots = new List<GameObject>()
        {
            slot1,slot2,slot3,slot4,slot5,
            slot6,slot7,slot8,slot9,slot10
        };

        for (int i = 0; i < slots.Count; i++)
        {
            var hoverBuild = slots[i].GetComponent<HoverBuilding>();
            //so rects are set bz is hidden
            hoverBuild.InitObjects();
        }

        var iconRoot = Root.lockedBuilding;
        _lockedSprite = (Sprite)Resources.Load(iconRoot, typeof(Sprite));
    }

    public void Show(List<H> vals)
    {
        LoadMenu(vals);

        transform.position = iniPos;
    }

    /// <summary>
    /// Loads the menu of the Buildins Menu 
    /// </summary>
    /// <param name="vals"></param>
    void LoadMenu(List<H> vals)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < vals.Count)
            {
                //find is if locked 
                bool locked = IsThisBuildLocked(vals[i]);

                if (!locked)
                {
                    MakeAlphaColorMax(slots[i]);

                    //load the root of the ico 
                    var iconRoot = Root.RetBuildingIconRoot(vals[i]);
                    var s = (Sprite) Resources.Load(iconRoot, typeof (Sprite));

                    slots[i].GetComponent<Image>().sprite = s;
                }
                //locked icon 
                else ShowLockedIcon(slots[i]);
            }
            //so if slot is empty 
            else MakeAlphaColorZero(slots[i]);
        }
    }

    private void ShowLockedIcon(GameObject g)
    {
        g.GetComponent<Image>().sprite = _lockedSprite;
    }

    void MakeAlphaColorZero(GameObject g)
    {
        Color bl = Color.white;
        bl.a = 0f;
        g.GetComponent<Image>().color = bl;
    }

    void MakeAlphaColorMax(GameObject g)
    {
        Color bl = Color.white;
        bl.a = 255f;
        g.GetComponent<Image>().color = bl;
    }

    /// <summary>
    /// Here have to implement the locking and unlocking of buildings 
    /// </summary>
    /// <param name="build"></param>
    /// <returns></returns>
    bool IsThisBuildLocked(H build)
    {
        //debug 
        if (build == H.DryDock)
        {
            return true;
        }

        return false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
