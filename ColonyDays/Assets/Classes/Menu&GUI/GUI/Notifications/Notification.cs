using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Notification
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string NotificationKey { get; set; }

    /// <summary>
    /// Additonal Param 1
    /// </summary>
    public string Param1 { get; set; }


    public Notification(string key)
    {
        NotificationKey = key;
    }

    internal void SetParam1(string p)
    {
        Param1 = p;
        Init();
    }

    void Init()
    {
        Name = Languages.ReturnString(NotificationKey + ".Noti.Name");
        Description = Languages.ReturnString(NotificationKey + ".Noti.Desc");

        //string s = string.Format("Hey, {0} it is the {1}st day of {2}.  I feel {3}!", _name, _day, _month, _feeling);
        if (!String.IsNullOrEmpty(Param1))
        {
            Description = string.Format(Description, Param1);
        }

        NotificationKey = NotificationKey;
    }


}

