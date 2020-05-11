using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The Description Window Script
///
/// When a building  is hover this description windows pop up
/// </summary>
public class DescriptionWindow : GUIElement
{
    private GameObject _title;
    private GameObject _cost;
    private GameObject _description;
    private Image _buildBanner;

    private Vector3 iniPos;

    private H _type = H.None;
    private Sprite _defaultSprite;//the one is loaded withthe game

    // Use this for initialization
    private void Start()
    {
        InitObj();

        Hide();
    }

    private void InitObj()
    {
        iniPos = transform.position;

        _title = GetChildCalled(H.Title);
        _cost = GetChildThatContains(H.Cost);
        _description = GetChildThatContains(H.Description);

        _buildBanner = GetChildCalled("Banner").GetComponent<Image>();
        _defaultSprite = _buildBanner.sprite;
    }

    public void Hide()
    {
        Vector3 newPos = transform.position;
        newPos.y = -200f;//80

        transform.position = newPos;
        _type = H.None;
    }

    public void Show(H val)
    {
        if (Program.MyScreen1.IsMainMenuOn())
        {
            return;
        }

        _type = val;
        LoadMenu(val);

        transform.position = iniPos;
    }

    private void LoadMenu(H val)
    {
        _title.GetComponent<Text>().text = Languages.ReturnString(val + "");
        _cost.GetComponent<Text>().text = BuildCostString(val);
        _description.GetComponent<Text>().text = Languages.ReturnString(val + ".Desc") + HouseDescription(val);

        ////means Im a creating new Towns to be saved as Initial(Templates) towns
        //if (Developer.IsDev && BuildingPot.Control.Registro.AllBuilding.Count == 0)
        //{
        //    BuildingPot.CreateUnlockBuilds();
        //}

        var state = BuildingPot.UnlockBuilds1.ReturnBuildingState(val);
        Sprite s = null;

        if (state == H.Unlock)
        {
            //load the root of the banner
            var iconRoot = Root.RetBuildingBannerRoot(val);
            s = (Sprite)Resources.Load(iconRoot, typeof(Sprite));
        }
        else if (state == H.Lock)
        {
            s = (Sprite)Resources.Load("Prefab/Building/Lock_Banner", typeof(Sprite));
        }
        else if (state == H.Coming_Soon)
        {
            s = (Sprite)Resources.Load("Prefab/Building/Coming_Soon_Banner", typeof(Sprite));
        }
        else if (state == H.Max_Cap_Reach)
        {
            s = (Sprite)Resources.Load("Prefab/Building/Max_Cap_Reach_Banner", typeof(Sprite));
        }

        //debug
        if (s == null)
        {
            _buildBanner.sprite = _defaultSprite;
            return;
        }

        _buildBanner.sprite = s;
    }

    /// <summary>
    /// If house should say the comfort
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    private string HouseDescription(H val)
    {
        if ((!val.ToString().Contains("House") && !val.ToString().Contains("Shack")) || val.ToString() == "LightHouse")
        {
            return "";
        }

        return ". Comfort: " + Building.ReturnHouseConfort(val);
    }

    /// <summary>
    /// Will build the string to show cost
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private string BuildCostString(H type)
    {
        var state = BuildingPot.UnlockBuilds1.ReturnBuildingState(type);
        if (state == H.Lock)
        {
            return BuildingPot.UnlockBuilds1.RequirementsNeeded(type);
        }
        if (state == H.Coming_Soon)
        {
            return Languages.ReturnString("Coming.Soon");// "This building is coming soon to the game";
        }
        if (state == H.Max_Cap_Reach)
        {
            return Languages.ReturnString("Max.Population");// "Can't build more houses. Max population reached";
        }

        //means Im a creating new Towns to be saved as Initial(Templates) towns
        if (Developer.IsDev && BuildingPot.Control.Registro.AllBuilding.Count == 0)
        {
            state = H.Unlock;
            return "Hope u are Dev and creating template town";
        }

        //unlock
        return CostOfABuilding(type, 2) + AddPerUnitIfNeeded(type);
    }

    private string AddPerUnitIfNeeded(H typeP)
    {
        if (typeP == H.Road || typeP.ToString().Contains("Bridge"))
        {
            return " (per unit)";
        }
        return "";
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    /// <param name="returnEvery">When appending every how many lines will do a return, To show in
    /// Descriptions window the normal used until now is : 3</param>
    /// <returns></returns>
    public static string CostOfABuilding(H type, int returnEvery)
    {
        var stat = Book.GiveMeStat(type);
        int appends = 0;

        var res = Languages.ReturnString("Resources") + ": \n";

        if (stat.Dollar != 0)
        {
            res += " " + Languages.ReturnString("Dollars") + ": " + stat.Dollar;
            appends++;
        }
        if (stat.Gold != 0)
        {
            res += " " + Languages.ReturnString("Gold") + ": " + Unit.WeightConverted(stat.Gold).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }

        if (stat.Iron != 0)
        {
            res += " " + Languages.ReturnString("Iron") + ": " + Unit.WeightConverted(stat.Iron).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Stone != 0)
        {
            res += " " + Languages.ReturnString("Stone") + ": " + Unit.WeightConverted(stat.Stone).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Brick != 0)
        {
            res += " " + Languages.ReturnString("Brick") + ": " + Unit.WeightConverted(stat.Brick).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Wood != 0)
        {
            res += " " + Languages.ReturnString("Wood") + ": " + Unit.WeightConverted(stat.Wood).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Nail != 0)
        {
            res += " " + Languages.ReturnString("Nail") + ": " + Unit.WeightConverted(stat.Nail).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Furniture != 0)
        {
            res += " " + Languages.ReturnString("Furniture") + ": " + Unit.WeightConverted(stat.Furniture).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Mortar != 0)
        {
            res += " " + Languages.ReturnString("Mortar") + ": " + Unit.WeightConverted(stat.Mortar).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.RoofTile != 0)
        {
            res += " " + Languages.ReturnString("RoofTile") + ": " + Unit.WeightConverted(stat.RoofTile).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.FloorTile != 0)
        {
            res += " " + Languages.ReturnString("FloorTile") + ": " + Unit.WeightConverted(stat.FloorTile).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Machinery != 0)
        {
            res += " " + Languages.ReturnString("Machinery") + ": " + Unit.WeightConverted(stat.Machinery).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        return res;
    }

    public static string CheckIfAppend3(ref int append, string msg, int returnEvery)
    {
        if (append >= returnEvery)
        {
            append = 0;
            return msg + "\n";
        }
        return msg;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    internal H CurrentType()
    {
        return _type;
    }
}