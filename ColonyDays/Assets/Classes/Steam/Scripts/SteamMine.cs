using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamMine : MonoBehaviour
{
    private bool _isDemo;
    void Start()
    {
        if (SteamManager.Initialized)
        {
            string name = SteamFriends.GetPersonaName();
            Debug.Log("Steam user:" + name);

            var steamId = SteamUser.GetSteamID();
            Debug.Log("Steam id :"+steamId);
        }
    }

    //void Update()
    //{
    //    if (Time.time > 5 && !_isDemo)
    //    {
    //        _isDemo = true;
    //        DemoStart();
    //    }
    //}




    ////a call Back sample
    //protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
    //private void OnEnable()
    //{
    //    if (SteamManager.Initialized)
    //    {
    //        m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
    //    }
    //}
    //private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    //{
    //    if (pCallback.m_bActive != 0)
    //    {
    //        Debug.Log("Steam Overlay has been activated");
    //    }
    //    else
    //    {
    //        Debug.Log("Steam Overlay has been closed");
    //    }
    //}
}