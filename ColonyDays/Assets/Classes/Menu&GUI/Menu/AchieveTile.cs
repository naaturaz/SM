using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The tile that display an achievement 
/// </summary>
public class AchieveTile : GUIElement
{
    private SteamStatsAndAchievements.Achievement_t _achievement;

    private Image _backOwn;
    private Image _icon;
    private Text _title;
    private Text _description;

    public SteamStatsAndAchievements.Achievement_t Achievement
    {
        get { return _achievement; }
        set { _achievement = value; }
    }

    void Start()
    {
        var backO = GetChildCalled("BackOwn");
        _backOwn = backO.GetComponent<Image>();  
        
        var icon = GetChildCalled("Icon");
        _icon = icon.GetComponent<Image>();
        
        var titleLbl = GetChildCalled("Title");
        _title = titleLbl.GetComponentInChildren<Text>();

        var descLbl = GetChildCalled("Desc");
        _description = descLbl.GetComponentInChildren<Text>();

        Set();   
    }

     void Set()
     {
        //if is not achieved disable stuff
        if (!_achievement.m_bAchieved)
        {
            _backOwn.gameObject.SetActive(false);
            //todo-- Disable or change Icon image
        }
        _icon.sprite = LoadIcons();


         _title.text = _achievement.m_strName;
         _description.text = _achievement.m_strDescription;
    }

    /// <summary>
     /// An Icon is named for ex: ACH_TRAVEL_FAR_ACCUM_WON, ACH_TRAVEL_FAR_ACCUM_DOLL
    /// </summary>
    /// <returns></returns>
     private Sprite LoadIcons()
     {
         var root = "Prefab/Menu/Achieve_Icons/"
             +_achievement.m_eAchievementID+"_"+IsDollOrWon();
         Sprite sp = Resources.Load<Sprite>(root);

         //debug only. if is new will use the standard one
         if (sp == new Sprite())
         {
             root = "Prefab/Menu/Achieve_Icons/STANDARD_" + IsDollOrWon();
             sp = Resources.Load<Sprite>(root);
         }

         return sp;
     }

    /// <summary>
    /// Doll is not won
    /// </summary>
    /// <returns></returns>
    string IsDollOrWon()
    {
        if (_achievement.m_bAchieved)
        {
            return "WON";
        }
        return "DOLL";
    }


    /// <summary>
    /// For show Save Load Tiles
    /// </summary>
    /// <param name="container"></param>
    /// <param name="invItem"></param>
    /// <param name="iniPos"></param>
    /// <param name="parent"></param>
    /// <param name="invType"></param>
    /// <returns></returns>
    static public AchieveTile Create(string root, Transform container, Vector3 iniPos, 
        SteamStatsAndAchievements.Achievement_t achievement)
    {
        AchieveTile obj = null;

        obj = (AchieveTile)Resources.Load(root, typeof(AchieveTile));
        obj = (AchieveTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var localScale = obj.transform.localScale;

        obj.transform.position = iniPos;
        obj.transform.parent = container;

        obj.transform.localScale = localScale;
        obj.Achievement = achievement;

        return obj;
    }

 




    void Update()
    {
        
    }
}

