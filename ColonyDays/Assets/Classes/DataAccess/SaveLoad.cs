using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class SaveLoad: MonoBehaviour {

    static List<RTSData> nfoRTS = new List<RTSData>();
    static ObjectToSerialize objectToSerialize;
    static SerializerClass serializer;



























    //static void InitiateObj()
    //{
    //    objectToSerialize = new ObjectToSerialize();
    //    serializer = new SerializerClass();
    //}

    //public static void Save(List<SaveInfoRTS> listSave)
    //{
    //    InitiateObj();

    //    //save the car list to a file
    //    objectToSerialize.SaveInfo = listSave;
    //    serializer.SerializeObject("outputFile.bin", objectToSerialize);
    //    //the car list has been saved to outputFile.txt
    //}

    ////List<string> MakeAllStrings(List<SaveInfoRTS> listPass)
    ////{
    ////    List<string> res = new List<string>();
    ////    for (int i = 0; i < listPass.Count; i++)
    ////    {
         
    ////    }
    ////}

    //public static void Load()
    //{
    //    InitiateObj();

    //    //read the file back from outputFile.txt

    //    objectToSerialize = serializer.DeSerializeObject("outputFile.bin");
    //    nfoRTS = objectToSerialize.SaveInfo;
    //    print( "loaded: " + nfoRTS[0].pos);
    //}


























    int ReturnProfileIndex(string currentUser)
    {
        int userNumber = -1;
        string[] users = { "Pepe", "Cuco" };
        for (int i = 0; i < users.Length; i++)
        {
            if (currentUser == users[i])
            {
                userNumber = i;
            }
        }
        return userNumber;
    }

	public void PlayerPrefSave(string current, bool isToResetProfile = false)
	{
        int resetValue = 1;
        if (isToResetProfile) 
        { resetValue = 0; }//will reset all values to zero 

        int indexProfile = ReturnProfileIndex(current);

        PlayerPrefs.SetFloat("Time_Played" + indexProfile, Program.THEProfile.TimePlayed * resetValue);
        PlayerPrefs.SetInt("Player_Lives" + indexProfile, Program.THEPlayer.Lives);
		/*PlayerPrefs.SetInt("TTLSCOREAMOUNT", GameController.TTLSCOREAMOUNT);
		PlayerPrefs.SetInt("TTLSTARTSAMOUNT", GameController.TTLSTARTSAMOUNT);
		
		tempKg = (float)GameController.TTLKGRECYCLEDAMOUNT;
		PlayerPrefs.SetFloat("TTLKGRECYCLEDAMOUNT", tempKg);
        */
		PlayerPrefs.Save();
	}

    public void PlayerPrefLoad(string current)
	{
        int indexProfile = ReturnProfileIndex(current);

        if (PlayerPrefs.GetString("Is_New_Profi" + indexProfile) == ""
            || PlayerPrefs.GetString("Is_New_Profi" + indexProfile) == null)
        {
            print("That was a brand new profile");
            InitializeAllStoreVars(current);
            return;
        }

        Program.THEProfile.TimePlayed = PlayerPrefs.GetFloat("Time_Played" + indexProfile);
        Program.THEPlayer.Lives = PlayerPrefs.GetInt("Player_Lives" + indexProfile);

		/*
		tempKg = PlayerPrefs.GetFloat("TTLKGRECYCLEDAMOUNT");
		string tempString = tempKg.ToString("n1");
		GameController.TTLKGRECYCLEDAMOUNT = double.Parse(tempString);

		GameController.PLAYERLEVEL = PlayerPrefs.GetInt("PLAYERLEVEL");
		GameController.MAXWORLDLEVEL = PlayerPrefs.GetInt("MAXWORLDLEVEL");
		
		//this is implemented so when loads the player has not kinematic on
		Player.ISKINEMATICON = "notkinet";*/
	}

    void InitializeAllStoreVars(string current)
    {
        int indexProfile = ReturnProfileIndex(current);
        PlayerPrefs.SetString("Is_New_Profi" + indexProfile, "Is_Old_Now");
        PlayerPrefs.SetFloat("Time_Played" + indexProfile, 0);
        PlayerPrefs.SetInt("Player_Lives" + indexProfile, 3);

    }
}
