
using System;

public class GameTime
{
    //0.0007f = 1Year at 10x 687.84sec; 
    //then decied do it 5 times faster since people reach 10Year in 2 Calendar years in Banished 
    //when put on FixedUpdate is 3 times slower... at time use to have 0.0035f
    //0.01, it was too fast. at 10 x 1 year will pass in 20 sec
    private float _timeFactor = 0.001f;

    private float _accumDays;

    private int _day = 1;
    private int _month = 1;
    private int _year;

    public float AccumDays
    {
        get { return _accumDays; }
        set { _accumDays = value; }
    }

    public int Day
    {
        get { return _day; }
        set { _day = value; }
    }

    public int Month1
    {
        get { return _month; }
        set { _month = value; }
    }

    public int Year
    {
        get { return _year; }
        set { _year = value; }
    }


    public GameTime() { }

    /// <summary>
    /// To be used when loading 
    /// </summary>
    /// <param name="month"></param>
    /// <param name="year"></param>
    public GameTime(float accum, int day, int month, int year)
    {
        _accumDays = accum;
        
        _day = day;
        _month = month;
        _year = year;
    }

    public void FixedUpdate()
    {
        _accumDays += TimeFactorInclSpeed();
        CheckIfNewDay();
    }

    /// <summary>
    /// The amount of time that pass. Includes the Speed of the game
    /// 
    /// This is the ref for all things that need a Time Factor included or Speed
    /// 
    /// return Program.gameScene.GameSpeed*_timeFactor*Program.gameScene.GameSpeed; 
    /// </summary>
    /// <returns></returns>
    public float TimeFactorInclSpeed()
    {
        return Program.gameScene.GameSpeed*_timeFactor*Program.gameScene.GameSpeed;
    }

    private void CheckIfNewDay()
    {
        if (_accumDays > 1f)
        {
            //so the one reached is removed and I keep adding 
            _accumDays = -1f;

            var oldDay = _day;
            _day = UMath.GoAround(1, _day, 1, 30);

            //if true a new month was reached 
            if (CheckIfNew(oldDay, _day))
            {
                NewMonth();
            }
        }
    }

    /// <summary>
    /// A new month was reached 
    /// </summary>
    void NewMonth()
    {
        var oldMonth = _month;
        _month = UMath.GoAround(1, _month, 1, 12);

        //if true a new year was reached 
        if (CheckIfNew(oldMonth, _month))
        {
            NewYear();
        }
    }

    private void NewYear()
    {
        _year++;
    }

    /// <summary>
    /// Will tell u if the newDate is smaller than old. that means tht a new Monht or year was reached 
    /// </summary>
    /// <param name="old"></param>
    /// <param name="?"></param>
    bool CheckIfNew(int oldDate, int newDate)
    {
        if (newDate < oldDate)
        {
            return true;
        }
        return false;
    }

    public Month MonthFormat()
    {
        return FromIntToMonth(_month);
    }

    Month FromIntToMonth(int val)
    {
        if (val == 1)
        {
            return Month.Jan;
        }
        if (val == 2)
        {
            return Month.Feb;
        }
        if (val == 3)
        {
            return Month.Mar;
        }
        
        if (val == 4)
        {
            return Month.Apr;
        }
        if (val == 5)
        {
            return Month.May;
        }
        if (val == 6)
        {
            return Month.Jun;
        }


        if (val == 7)
        {
            return Month.Jul;
        }
        if (val == 8)
        {
            return Month.Aug;
        }
        if (val == 9)
        {
            return Month.Sep;
        }

        if (val == 10)
        {
            return Month.Oct;
        }
        if (val == 11)
        {
            return Month.Nov;
        }
        if (val == 12)
        {
            return Month.Dec;
        }

        return Month.None;
    }


    public MDate CurrentDate()
    {
        return new MDate(_day, _month, _year);
    }

    public MDate ElapsedDate(MDate first, MDate last)
    {
        var days1st = FromMDateToDays(first);
        var days2nd = FromMDateToDays(last);

        var diff = days2nd - days1st;

        return FromDaysToMDate(diff);
    }

    public int ElapsedDateInDays(MDate first, MDate last)
    {
        var days1st = FromMDateToDays(first);
        var days2nd = FromMDateToDays(last);

        return days2nd - days1st;
    }

    /// <summary>
    /// Here the last param is current date 
    /// </summary>
    /// <param name="first"></param>
    /// <returns></returns>
    public int ElapsedDateInDaysToDate(MDate first)
    {
        var days1st = FromMDateToDays(first);
        var days2nd = FromMDateToDays(CurrentDate());

        return days2nd - days1st;
    }

    int FromMDateToDays(MDate val)
    {
        return val.Day + (val.Month1 * 30) + (val.Year * 360);
    }

    /// <summary>
    /// Recursive method to convert days int into MDate
    /// </summary>
    /// <param name="daysToConvert">The days to convert</param>
    /// <param name="year"></param>
    /// <param name="mon"></param>
    /// <param name="day"></param>
    /// <returns></returns>
    MDate FromDaysToMDate(float daysToConvert)
    {
        float left = 0;
        var yearFull = 0;
        var monFull = 0;
        if (daysToConvert / 360 > 0)
        {
            var year = daysToConvert/360;
            yearFull = (int)year;

            left = year - yearFull;
        }
        daysToConvert = left * 360;
        
        if (daysToConvert / 30 > 0)
        {
            var mon = daysToConvert / 30;
            monFull = (int)mon;

            left = mon - monFull;
        }
        daysToConvert = left * 30;


        //willr return the days left tht are less than a year and a month, 
        //plus the calculate amount of Year and month
        return new MDate((int)daysToConvert, monFull, yearFull);
    }

    internal string TodayYMD()
    {
        return Year + "-" + Month1 + "-" + Day;
    }



    public MDate ReturnCurrentDatePlsAdded(float daysP)
    {
        var currDaysSoFar = FromMDateToDays(CurrentDate());
        var newDate = currDaysSoFar + daysP;

        return FromDaysToMDate((int)newDate);
    }

    public static bool IsPastOrNow(MDate leaveDate)
    {
        if (leaveDate.Month1 <= Program.gameScene.GameTime1.Month1 &&
            leaveDate.Year <= Program.gameScene.GameTime1.Year)
        {
            return true;
        }
        return false;
    }
}

/// <summary>
/// My Date Class to return date 
/// </summary>
public class MDate
{
    private int _day;
    private int _month;
    private int _year;

    public int Day
    {
        get { return _day; }
        set { _day = value; }
    }

    public int Month1
    {
        get { return _month; }
        set { _month = value; }
    }

    public int Year
    {
        get { return _year; }
        set { _year = value; }
    }

    public MDate() { }

    public MDate(int day, int mon, int year)
    {
        _day = day;
        _month = mon;
        _year = year;
    }
}
