using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The Description Window Script
/// 
/// When a building  is hover this description windows pop up
/// </summary>
public class DescriptionWindow : General
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
        _type = val;
        LoadMenu(val);

        transform.position = iniPos;
    }

    private void LoadMenu(H val)
    {
        _title.GetComponent<Text>().text = val + "";
        _cost.GetComponent<Text>().text = BuildCostString(val);
        _description.GetComponent<Text>().text = Languages.ReturnString(val+".Desc");


        //load the root of the banner 
        var iconRoot = Root.RetBuildingBannerRoot(val);
        var s = (Sprite) Resources.Load(iconRoot, typeof (Sprite));


        //debug
        if (s == null)
        {
            _buildBanner.sprite = _defaultSprite;
            return;
        }

        _buildBanner.sprite = s;
    }

    /// <summary>
    /// Will build the string to show cost 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    string BuildCostString(H type)
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
            res += " Gold: " + stat.Gold + " " + Unit.WeightUnit();
            appends++;
        }
        
        if (stat.Iron != 0)
        {
            res += " Iron: " + stat.Iron + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res);

        if (stat.Stone != 0)
        {
            res += " Stone: " + stat.Stone + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res);
 
        if (stat.Brick != 0)
        {
            res += " Brick: " + stat.Brick + " " + Unit.WeightUnit();
            appends++;
        }
        res = CheckIfAppend3(ref appends, res);

        if (stat.Wood != 0)
        {
            res += " Wood: " + stat.Wood + " " + Unit.WeightUnit();
            appends++;
        } 
        res = CheckIfAppend3(ref appends, res);

        return res;

        //private int _capacity;//how many units of good can hold a building


    }

    string CheckIfAppend3(ref int append , string msg )
    {
        if (append >= 3)
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
