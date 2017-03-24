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
	void Start () 
    {
	    InitObj();

        Hide();
	}

    void InitObj()
    {
        iniPos = transform.position;

        _title = GetChildThatContains(H.Title);
        _cost = GetChildThatContains(H.Cost);
        _description = GetChildThatContains(H.Description);

        _buildBanner = GetComponent<Image>();
        _defaultSprite = _buildBanner.sprite;
    }

    public void Hide()
    {
        Vector3 newPos = transform.position;
        newPos.y = -80f;

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
        _title.GetComponent<Text>().text = val + "";
        _cost.GetComponent<Text>().text = BuildCostString(val);
        _description.GetComponent<Text>().text = Languages.ReturnString(val+".Desc") + HouseDescription(val);

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

    private string HouseDescription(H val)
    {
        if (!val.ToString().Contains("House") || val.ToString() == "LightHouse")
        {
            return "";
        }

        return "\nConfort: ";
    }

    /// <summary>
    /// Will build the string to show cost 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    string BuildCostString(H type)
    {
     

        var state = BuildingPot.UnlockBuilds1.ReturnBuildingState(type);
        if (state == H.Lock)
        {
            return BuildingPot.UnlockBuilds1.RequirementsNeeded(type);
        }
        if (state == H.Coming_Soon)
        {
            return "This building is coming soon to the game";
        }
        if (state == H.Max_Cap_Reach)
        {
            return "Can't build more houses. Max population reached";
        }

        //means Im a creating new Towns to be saved as Initial(Templates) towns
        if (Developer.IsDev && BuildingPot.Control.Registro.AllBuilding.Count == 0)
        {
            state = H.Unlock;
            return "Hope u are Dev and creating template town";
        }


        //unlock
        return CostOfABuilding(type, 2);
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

        var res = "Cost: \n";

        if (stat.Dollar != 0)
        {
            res += " Dollars: " + stat.Dollar;
            appends++;
        }
        if (stat.Gold != 0)
        {
            res += " Gold: " + Unit.WeightConverted(stat.Gold).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }

        if (stat.Iron != 0)
        {
            res += " Iron: " + Unit.WeightConverted(stat.Iron).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Stone != 0)
        {
            res += " Stone: " + Unit.WeightConverted(stat.Stone).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Brick != 0)
        {
            res += " Brick: " + Unit.WeightConverted(stat.Brick).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Wood != 0)
        {
            res += " Wood: " + Unit.WeightConverted(stat.Wood).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Nail != 0)
        {
            res += " Nail: " + Unit.WeightConverted(stat.Nail).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Furniture != 0)
        {
            res += " Furniture: " + Unit.WeightConverted(stat.Furniture).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Mortar != 0)
        {
            res += " Mortar: " + Unit.WeightConverted(stat.Mortar).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.RoofTile != 0)
        {
            res += " RoofTile: " + Unit.WeightConverted(stat.RoofTile).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.FloorTile != 0)
        {
            res += " FloorTile: " + Unit.WeightConverted(stat.FloorTile).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        if (stat.Machinery != 0)
        {
            res += " Machinery: " + Unit.WeightConverted(stat.Machinery).ToString("n1") + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res, returnEvery);

        return res;
    }


    public static string CheckIfAppend3(ref int append , string msg, int returnEvery)
    {
        if (append >= returnEvery)
        {
            append = 0;
            return msg + "\n";
        }
        return msg;
    }

	// Update is called once per frame
	void Update () 
    {
	
	}

    internal H CurrentType()
    {
        return _type;
    }
}
