using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Classes.Menu_GUI.GUI.Bulletin;
using UnityEngine.UI;


public class BulletinWindow : GUIElement
{
    //bulletin fields
    private Text _body;


    //subBulletins
    private SubBulletinWorkers _workers;

    void Start()
    {
        _body =  GetChildCalled("Body_Lbl").GetComponent<Text>();

        iniPos = transform.position;
        Hide();

        _workers = new SubBulletinWorkers(this);

    }

    void Update()
    {
        
    }


    public void Show()
    {
        base.Show();

        _workers.Show();
    }

    public void ShowInBody(string text)
    {
        _body.text = text;
    }
}

