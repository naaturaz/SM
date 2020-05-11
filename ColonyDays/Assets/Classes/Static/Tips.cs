using System.Collections.Generic;
using UnityEngine;

//THIS CLASS ONLY HOLD STATIC VALUES
public class Tips : MonoBehaviour
{
    //will hold diferent word that can be change to address
    //correctly the description given the hardware
    //ex: On Windows - 'Click' ==> Xbox - 'Button A'
    public static List<List<string>> hardwares = new List<List<string>>();

    public static List<string> win = new List<string>();
    public static List<string> xBox = new List<string>();

    public static List<string> tips = new List<string>();
    public static List<string> spawner = new List<string>();

    public static bool isStarted = false;

    // Use this for initialization
    public static void Start()
    {
        //HARDWARES LIST
        //windows LIST
        win.Add("click");//index 0

        //xbox LIST
        xBox.Add("press 'Button A'");//index 0

        //HardWares list
        hardwares.Add(win);//index 0
        hardwares.Add(xBox);//index 1

        //Main Menu/////////////
        tips.Add("play the campaign");
        spawner.Add("Select_Campaign_Btn_Main_3dMenu");
        tips.Add("your numbers");
        spawner.Add("Select_Profile_Btn_Main_3dMenu");
        tips.Add("will notify you when we find an opponent");
        spawner.Add("Select_MultiPlayer_Btn_Main_3dMenu");
        tips.Add("change some stuff");
        spawner.Add("Select_Settings_Btn_Main_3dMenu");
        tips.Add("exit");
        spawner.Add("Select_Exit_Btn_Main_3dMenu");
        tips.Add("Our Titles");
        spawner.Add("Select_More_Games_Btn_Main_3dMenu");

        //Setting
        tips.Add("turn on/off the game sounds");
        spawner.Add("aaa");
        tips.Add("turn on/off the game music");
        spawner.Add("aaa");

        //Back Btn
        tips.Add("go back there");
        spawner.Add("aaa");

        //GAMEPLAY
        //Select type of objecto to create
        tips.Add("diferents basic objects");//Raw
        spawner.Add("Select_Raw_Btn_NewMenu_3dMenu");
        tips.Add("diferents elements");//Element
        spawner.Add("Select_Element_Btn_NewMenu_3dMenu");
        tips.Add("they will help you");//Helpers
        spawner.Add("Select_Helper_Btn_NewMenu_3dMenu");

        //Raw
        tips.Add(hardwares[0][0] + " on the icon and then place it on any surface, then " + hardwares[0][0] + " again");
        spawner.Add("aaa");

        //Elements

        //Helpers

        //Pause Menu Message
        //tips.Add("If you exit will lose all the progress");//Helpers
        //spawner.Add("");
    }

    public static string ReturnTip(string spawnPass)
    {
        if (!isStarted)
        {
            Start();
            isStarted = true;
        }

        //print(spawnPass);

        string s = "No tips";
        for (int i = 0; i < tips.Count; i++)
        {
            if (spawnPass.Contains(spawner[i]))
            {
                s = tips[i];
            }
        }
        return s;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}