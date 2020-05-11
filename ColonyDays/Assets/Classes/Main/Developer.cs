/// <summary>
/// Sometimes developing mode is needed . to add new terrains for example.
/// </summary>
internal class Developer
{
    //if is dev true will be able to select terrain
    private static bool _isDev;

    public static bool IsDev
    {
        //get       {            return SteamUser.GetSteamID().m_SteamID == 76561198245800476 || SteamUser.GetSteamID().m_SteamID == 76561197970401438;        }
        //        get {
        //            var isOn = false;

        //#if UNITY_EDITOR
        //            isOn = true;
        //#endif
        //            return isOn;
        //        }
        get { return _isDev; }
        set { _isDev = value; }
    }
}