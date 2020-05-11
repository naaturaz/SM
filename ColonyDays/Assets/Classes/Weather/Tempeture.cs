using System.Collections.Generic;

public class Tempeture
{
    //http://www.holiday-weather.com/havana/averages/
    private static List<int> _min = new List<int>() { 16, 17, 18, 19, 21, 22, 22, 22, 22, 20, 19, 18 };

    private static List<int> _max = new List<int>() { 26, 27, 28, 30, 31, 32, 32, 32, 31, 30, 28, 27 };

    private static float oldTemp;

    public static float Current()
    {
        if (Program.gameScene.GameSpeed == 0)
        {
            return oldTemp;
        }

        var min = _min[Program.gameScene.GameTime1.Month1 - 1];
        var max = _max[Program.gameScene.GameTime1.Month1 - 1];

        oldTemp = UMath.GiveRandom(min, max);

        return ConvertToImperialIfNeeded(oldTemp);
    }

    private static float ConvertToImperialIfNeeded(float curr)
    {
        if (!Unit.IsCurrentSystemMetric())
        {
            return Unit.CelsiusToFarenheit(curr);
        }
        return curr;
    }
}