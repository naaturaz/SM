using UnityEngine;

public class BtnRate : MonoBehaviour
{
    private bool _isGoodPlayer;

    // Use this for initialization
    private void Start()
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
    private void Update()
    {
    }

    public void Rate()
    {
        Application.OpenURL("http://store.steampowered.com/recommended/recommendgame/538990");
    }
}