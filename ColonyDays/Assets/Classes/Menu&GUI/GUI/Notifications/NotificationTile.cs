using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The tile that display an achievement
/// </summary>
public class NotificationTile : GUIElement
{
    private Notification _notification;

    private Image _backOwn;
    private Image _icon;
    private Text _title;
    private Text _description;

    public Notification Notification1
    {
        get { return _notification; }
        set { _notification = value; }
    }

    private void Start()
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

    private void Set()
    {
        _icon.sprite = LoadIcons();
        _title.text = _notification.Name;
        _description.text = _notification.Description;
    }

    /// <summary>
    /// An Icon is named for ex: ACH_TRAVEL_FAR_ACCUM_WON, ACH_TRAVEL_FAR_ACCUM_DOLL
    /// </summary>
    /// <returns></returns>
    private Sprite LoadIcons()
    {
        var root = "Prefab/GUI/Notification_Icons/"
            + _notification.NotificationKey;
        Sprite sp = Resources.Load<Sprite>(root);

        if (sp == null)
        {
            root = "Prefab/GUI/Notification_Icons/Default";
            sp = Resources.Load<Sprite>(root);
        }

        return sp;
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
    static public NotificationTile Create(string root, Transform container, Vector3 iniPos,
        Notification notification)
    {
        NotificationTile obj = null;

        obj = (NotificationTile)Resources.Load(root, typeof(NotificationTile));
        obj = (NotificationTile)Instantiate(obj, new Vector3(), Quaternion.identity);

        var localScale = obj.transform.localScale;

        obj.transform.position = iniPos;
        obj.transform.SetParent(container);

        obj.transform.localScale = localScale;
        obj.Notification1 = notification;

        return obj;
    }

    private void Update()
    {
    }
}