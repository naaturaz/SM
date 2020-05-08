/// <summary>
/// Sometimes developing mode is needed . to add new terrains for example.
/// </summary>
internal class Developer
{
    //if is dev true will be able to select terrain
    private static bool _isDev = false;

    public static bool IsDev
    {
        get { return _isDev; }
        set { _isDev = value; }
    }
}