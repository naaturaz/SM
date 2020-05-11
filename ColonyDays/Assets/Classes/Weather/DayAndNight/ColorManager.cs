using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public DayStage[] Day = new DayStage[6];
    public DayStage[] Night = new DayStage[1];

    public Color DayGrass;

    public Color[] BackGroundCam = new Color[2];

    private void Start()
    {
    }

    private void Update()
    {
    }

    internal Color GetMeMainColor(int _currentStage, H dayOrNight)
    {
        if (dayOrNight == H.Day)
        {
            return Day[_currentStage].Main;
        }
        return Night[0].Main;
    }

    internal Color GetMeAmbienceColor(int _currentStage, H dayOrNight)
    {
        if (dayOrNight == H.Day)
        {
            return Day[_currentStage].Ambience;
        }
        return Night[0].Ambience;
    }

    internal float GetMeMainIntensity(int _currentStage, H dayOrNight)
    {
        if (dayOrNight == H.Day)
        {
            return Day[_currentStage].MainIntense;
        }
        return Night[0].MainIntense;
    }

    internal float GetMeWestIntensity(int _currentStage, H dayOrNight)
    {
        if (dayOrNight == H.Day)
        {
            return Day[_currentStage].WestIntense;
        }
        return Night[0].WestIntense;
    }

    internal float GetMeEastIntensity(int _currentStage, H dayOrNight)
    {
        if (dayOrNight == H.Day)
        {
            return Day[_currentStage].EastIntense;
        }
        return Night[0].EastIntense;
    }

    internal Color GetMeCameraBackGroundColor(H dayOrNight)
    {
        if (dayOrNight == H.Day)
        {
            return BackGroundCam[0];
        }
        return BackGroundCam[1];
    }
}

[System.Serializable]
public class DayStage
{
    public Color Main;
    public float MainIntense;
    public float WestIntense;
    public float EastIntense;
    public Color Ambience;
}