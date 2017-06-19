using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnRate : MonoBehaviour
{
    bool _isGoodPlayer;

    // Use this for initialization
    void Start()
    {
        _isGoodPlayer = PlayerPrefs.GetInt("Rate") > 0;

        //var id = SteamUser.GetSteamID().m_SteamID;

        //Debug.Log("id1:" + SteamUser.GetSteamID());
        //Debug.Log("id1 inner:" + SteamUser.GetSteamID().m_SteamID);


        if (!_isGoodPlayer)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Rate()
    {
        Application.OpenURL("http://store.steampowered.com/recommended/recommendgame/538990");
    }
}
