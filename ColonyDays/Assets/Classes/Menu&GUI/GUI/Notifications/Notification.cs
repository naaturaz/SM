using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Notification
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string NotificationKey { get; set; }

    public Notification(string key)
    {
        Name = Languages.ReturnString(key+".Noti.Name");
        Description = Languages.ReturnString(key + ".Noti.Desc");
        NotificationKey = key;
    }
}

