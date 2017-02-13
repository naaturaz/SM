using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationWindowGO : GUIElement
{
    private GameObject _content;
    private RectTransform _contentRectTransform;

    private GameObject _scroll_Ini_PosGO;

    private List<NotificationTile> _tilesSpawn = new List<NotificationTile>();
    private Vector3 _scrollIniPos;

    private Scrollbar _verticScrollbar;

    private bool _hideSlideToLeft;

    //todo saveload
    List<string> _allNotifications = new List<string>(){}; 

    /// <summary>
    /// IMPORTANT
    /// To add notification :
    /// Add name and desc and Languages.cs
    /// Add sound file on Prefab/audio/sound/other and icon on GUI/Notification_Icon/
    /// Then add it in sound name added it on _rootsToSpawn on AudioCollector.cs
    /// 
    /// Also if want to have a sound place the sound in Prefab/Audio/Sound/Other with the
    /// same name of the Notification as called in Languages.cs
    /// </summary>
    //Dictionary<string, Notification> _bank = new Dictionary<string, Notification>()
    //{
    //    {"BabyBorn", new Notification("BabyBorn")},
    //    {"PirateUp", new Notification("PirateUp")},            
    //    {"PirateDown", new Notification("PirateDown")},
        
    //    {"Emigrate", new Notification("Emigrate")},
    //    {"PortUp", new Notification("PortUp")},
    //    {"PortDown", new Notification("PortDown")},
    //    {"BoughtLand", new Notification("BoughtLand")},
    //    {"ShipPayed", new Notification("ShipPayed")},
    //    {"ShipArrived", new Notification("ShipArrived")},

    //    {"AgeMajor", new Notification("AgeMajor")},
    //    {"PersonDie", new Notification("PersonDie")},


    //}; 

    void Start()
    {
        iniPos = transform.position;
        Hide();

        var scroll = GetChildCalled("Scroll_View");

        _content = GetGrandChildCalledFromThis("Content", scroll);
        _contentRectTransform = _content.GetComponent<RectTransform>();

        _scroll_Ini_PosGO = GetChildCalledOnThis("Scroll_Ini_Pos", _content);
    }

    float _showedAt;
    public void Show(string which)
    {
        ClearForm();
        PopulateScrollView();
        Show();
        _showedAt = Time.time;
    }

    void ClearForm()
    {
        if (_verticScrollbar != null)
        {
            _verticScrollbar.value = 1;
        }
    }

    /// <summary>
    /// So as changes size will be available or not. 
    /// We need this ref ' _verticScrollbar ' to set it to defauitl value 
    /// </summary>
    void TakeScrollVerticBar()
    {
        var vert = GetGrandChildCalled("Scrollbar Vertical");
        if (vert != null)
        {
            _verticScrollbar = vert.GetComponent<Scrollbar>();
        }
        else
        {
            _verticScrollbar = null;
        }
    }

    private float speed = .05f;
    void Update()
    {
        if (transform.position == iniPos && Input.GetKeyUp(KeyCode.Return))
        {
            MouseListen("Achieve.OKBtn");
        }

        if (_hideSlideToLeft)
        {
            Vector2 newPos = new Vector2(transform.position.x - speed, transform.position.y);
            transform.position = newPos;
            speed *= 2.2f;

            if (transform.position.x <= iniPos.x - 400f)
            {
                _hideSlideToLeft = false;
                speed = .02f;
            }
        }

        HideIfIsBeingOutTooLong();
    }

    private void HideIfIsBeingOutTooLong()
    {
        var show = IsShownNow();
        var time = Time.time > _showedAt + 20;

        if (show && time && !_hideSlideToLeft)
        {
            _hideSlideToLeft = true;
        }
    }

    internal void MouseListen(string sub)
    {
        sub = sub.Substring(8);

        if (sub == "OKBtn")
        {
            Hide();
            DestroyPrevTiles();
            Program.MyScreen1.HideWindowShowMain(this);
        }

    }

    void DestroyPrevTiles()
    {
        for (int i = 0; i < _tilesSpawn.Count; i++)
        {
            _tilesSpawn[i].Destroy();
        }
        _tilesSpawn.Clear();
    }

    void PopulateScrollView()
    {
        SetTileIniPos();

        DestroyPrevTiles();

        SetHeightOfContentRect(_allNotifications.Count);
        ShowAllItems();

        TakeScrollVerticBar();
    }

    private void ShowAllItems()
    {
        for (int i = 0; i < _allNotifications.Count; i++)
        {
            var key = GetKey(_allNotifications[i]);
            var noti = new Notification(key);

            var param1 = GetParam1(_allNotifications[i]);
            noti.SetParam1(param1);

            var iniPos = ReturnIniPos(i);
            var tile = NotificationTile.Create(Root.notificationTile, _content.transform, iniPos,
                noti);

            _tilesSpawn.Add(tile);
        }
    }

    string GetKey(string notificationString)
    {
        var arr = notificationString.Split('|');

        //when has additional 
        if (arr.Length > 1)
        {
            return arr[0]; 
        }
        //doesnt have additional info
        return notificationString;
    }

    string GetParam1(string notificationString)
    {
        var arr = notificationString.Split('|');

        //when has additional 
        if (arr.Length > 1)
        {
            return arr[1];
        }
        //doesnt have additional info
        return "";
    }

    /// <summary>
    /// need to be called to set the Ini POs everytime 
    /// </summary>
    void SetTileIniPos()
    {
        //var ySpace = Screen.height / 59.46667f;    //15 on editor

        _scrollIniPos = _scroll_Ini_PosGO.transform.position;
        //_scrollIniPos = new Vector3(_scrollIniPos.x, _scrollIniPos.y - ySpace, _scrollIniPos.z);
    }

    private void SetHeightOfContentRect(int tiles)
    {
        //892
        var tileYSpace = 5.57f * 2.8f;
        //var tileYSpace = Screen.height / 160.1436f;//5.57f on editor

        //5.57f the space btw two of them 
        var size = (tileYSpace * tiles) + tileYSpace;
        _contentRectTransform.sizeDelta = new Vector2(0, size);
    }

    Vector3 ReturnIniPos(int i)
    {
        //var xAddVal = Screen.width / 5.87407f;
        return new Vector3(_scrollIniPos.x, ReturnY(i) + _scrollIniPos.y, _scrollIniPos.z);
    }

    float ReturnY(int i)
    {
        if (i == 0)
        {
            return 0;
        }
        //30
        var y = (Screen.height * 52) / 892;
        return -y * i;
    }

    //called from GUI 
    public void HideWindow()
    {
        _hideSlideToLeft = true;
    }

    //called from GUI 
    public void ShowWindow()
    {
        Show();
        Show("");
    }    
    
    //called from GUI 
    public void CleanAll()
    {
        _allNotifications.Clear();
        Show("");
    }

    /// <summary>
    /// Shows notification
    /// </summary>
    /// <param name="notiKey">The key of the notification like 'PirateUp'</param>
    public void Notify(string notiKey)
    {
        //plays the sound of the notification
        AudioCollector.PlayOneShot(notiKey, 15);

        _allNotifications.Insert(0, notiKey);
        Show("");
    }

    public void Notify(string notiKey, string addP)
    {
        //plays the sound of the notification
        AudioCollector.PlayOneShot(notiKey, 15);

        _allNotifications.Insert(0, notiKey+"|"+addP);
        Show("");
    }
}

