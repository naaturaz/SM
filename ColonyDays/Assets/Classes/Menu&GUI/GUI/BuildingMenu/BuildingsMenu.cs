using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This is the menu where u can select Buildings from a specific category
 */

public class BuildingsMenu : GUIElement
{
    private Vector3 iniPosition;

    private List<GameObject> builds = new List<GameObject>();

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

        var build1 = FindGameObjectInHierarchy("Build_1", gameObject);
        var build2 = FindGameObjectInHierarchy("Build_2", gameObject);
        var build3 = FindGameObjectInHierarchy("Build_3", gameObject);
        var build4 = FindGameObjectInHierarchy("Build_4", gameObject);
        var build5 = FindGameObjectInHierarchy("Build_5", gameObject);

        var build6 = FindGameObjectInHierarchy("Build_6", gameObject);
        var build7 = FindGameObjectInHierarchy("Build_7", gameObject);
        var build8 = FindGameObjectInHierarchy("Build_8", gameObject);
        var build9 = FindGameObjectInHierarchy("Build_9", gameObject);
        var build10 = FindGameObjectInHierarchy("Build_10", gameObject);

        builds = new List<GameObject>()
        {
            build1,build2,build3,build4,build5,
            build6,build7,build8,build9,build10
        };

        //for (int i = 0; i < builds.Count; i++)
        //{
        //    var hoverBuild = builds[i].GetComponent<HoverBuilding>();
        //    //so rects are set bz is hidden
        //    hoverBuild.InitObjects();
        //}
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
        //transform.position = iniPosition;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public new void Hide()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < builds.Count; i++)
        {
            if(builds[i].transform.childCount > 0   )
                Destroy(builds[i].transform.GetChild(0).gameObject);
        }
    }

    /// <summary>
    /// Loads the menu of the Buildins Menu
    /// </summary>
    /// <param name="vals"></param>
    private void LoadMenu(List<H> vals)
    {
        Program.MouseListener.HidePersonBuildingOrderBulletin();

        for (int i = 0; i < builds.Count; i++)
        {
            if (i < vals.Count)
            {
                //MakeAlphaColorMax(builds[i]);

                //find is if locked
                bool unlocked = H.Unlock == BuildingPot.UnlockBuilds1.ReturnBuildingState(vals[i]);

                if (unlocked)
                {
                    //load the root of the ico
                    //var iconRoot = Root.RetBuildingIconRoot(vals[i]);
                    var root = Root.RetBuildingRoot(vals[i]);
                    var load = (GameObject)Resources.Load(root);

                    var build = Instantiate(load, builds[i].transform);
                    build.GetComponent<Structure>().enabled = false;
                    build.transform.position = builds[i].transform.position;
                    build.transform.localScale = builds[i].transform.localScale;

                    build.AddComponent<Rotate>();
                    //build.GetComponent<Rotate>().onY = true;
                    //build.GetComponent<Rotate>().speed = 0.2f;

                    var main = GetChildThatContains("Main", build);
                    main.AddComponent<MenuBuildingItemHover>();
                }
                //locked icon
                //else ShowLockedIcon(builds[i]);
            }
            //so if build is empty
            //else MakeAlphaColorZero(builds[i]);
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

    // Update is called once per frame
    private void Update()
    {
    }
}