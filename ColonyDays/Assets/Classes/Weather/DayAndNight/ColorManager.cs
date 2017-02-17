using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class ColorManager : MonoBehaviour
{
    public DayStage[] Day = new DayStage[6];
    public DayStage[] Night = new DayStage[1];

    public Color DayGrass;

    void Start()
    {

    }

    void Update()
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

