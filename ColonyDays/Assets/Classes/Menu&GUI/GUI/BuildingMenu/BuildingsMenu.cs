using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This is the menu where u can select Buildings from a specific category
 *
 */

public class BuildingsMenu : GUIElement
{
    private Vector3 iniPosition;

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

    private List<GameObject> slots = new List<GameObject>();

    private Sprite _lockedSprite; //the sprite when a Build is locked

    // Use this for initialization
    private void Start()
    {
        InitObj();

        Hide();
    }

    private void InitObj()
    {
        iniPosition = transform.position;

        slot1 = GetChildThatContains(H.Slot1);
        slot2 = GetChildThatContains(H.Slot2);
        slot3 = GetChildThatContains(H.Slot3);
        slot4 = GetChildThatContains(H.Slot4);
        slot5 = GetChildThatContains(H.Slot5);

        slot6 = GetChildThatContains(H.Slot6);
        slot7 = GetChildThatContains(H.Slot7);
        slot8 = GetChildThatContains(H.Slot8);
        slot9 = GetChildThatContains(H.Slot9);
        slot10 = GetChildThatContains(H.Slot10);

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
        locked = (Sprite)Resources.Load(Root.lockedBuilding, typeof(Sprite));
    }

    private Sprite locked;

    public void Show(List<H> vals)
    {
        //means Im a creating new Towns to be saved as Initial(Templates) towns
        if (Developer.IsDev && BuildingPot.Control.Registro.AllBuilding.Count == 0)
        {
            BuildingPot.CreateUnlockBuilds();
        }

        BuildingPot.UnlockBuilds1.UpdateBuildsStatuses();
        LoadMenu(vals);
        transform.position = iniPosition;
    }

    /// <summary>
    /// Loads the menu of the Buildins Menu
    /// </summary>
    /// <param name="vals"></param>
    private void LoadMenu(List<H> vals)
    {
        Program.MouseListener.HidePersonBuildingOrderBulletin();

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < vals.Count)
            {
                MakeAlphaColorMax(slots[i]);

                //find is if locked
                bool unlocked = H.Unlock == BuildingPot.UnlockBuilds1.ReturnBuildingState(vals[i]);

                if (unlocked)
                {
                    //load the root of the ico
                    var iconRoot = Root.RetBuildingIconRoot(vals[i]);
                    var s = (Sprite)Resources.Load(iconRoot, typeof(Sprite));

                    //didnt found an Icon
                    if (s == null)//new Sprite()
                    {
                        s = (Sprite)Resources.Load("Prefab/Building/gameIcon", typeof(Sprite));
                    }

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
        g.GetComponent<Image>().sprite = locked;
    }

    private void MakeAlphaColorZero(GameObject g)
    {
        Color bl = Color.white;
        bl.a = 0f;
        g.GetComponent<Image>().color = bl;
    }

    private void MakeAlphaColorMax(GameObject g)
    {
        Color bl = Color.white;
        bl.a = 255f;
        g.GetComponent<Image>().color = bl;
    }

    /// <summary>
    /// Here have to implement the locking and unlocking of buildings
    /// todo
    /// </summary>
    /// <param name="build"></param>
    /// <returns></returns>
    private bool IsThisBuildLocked(H build)
    {
        //debug
        if (build == H.Shipyard)
        {
            return true;
        }

        return false;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}