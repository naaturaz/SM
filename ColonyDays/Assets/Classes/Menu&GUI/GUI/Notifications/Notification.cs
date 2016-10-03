using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Notification
{
    public string Name { get; set; }

    public string Description { get; set; }

    public string NotificationKey { get; set; }

    public Notification(string name, string desc, string key)
    {
        Name = name;
        Description = desc;
        NotificationKey = key;
    }
}

