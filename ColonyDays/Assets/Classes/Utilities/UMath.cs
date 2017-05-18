using System;
using UnityEngine;
using System.Collections.Generic;

public class UMath : MonoBehaviour {

    /// <summary>
    /// Min inclusive , Max Exclusive
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static int GiveRandom(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static float GiveRandom(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    /// <summary>
    /// Return closest Vector3 to 'Stone' must have at least 2 items the list 
    /// </summary>
    public static Vector3 ReturnClosestVector3(List<Vector3> list, Vector3 stone)
    {
        Vector3 res = list[0];

        for (int i = 1; i < list.Count; i++)
        {
            var distRes = Vector3.Distance(res, stone);
            var distCurrent = Vector3.Distance(list[i], stone);

            if (distRes > distCurrent)
            {
                res = list[i];
            }
        }

        return res;
    }

    /// <summary>
    /// Return farest Vector3 to 'Stone' must have at least 2 items the list 
    /// </summary>
    public static Vector3 ReturnFarestVector3(List<Vector3> list, Vector3 stone)
    {
        Vector3 res = list[0];

        for (int i = 1; i < list.Count; i++)
        {
            var distRes = Vector3.Distance(res, stone);
            var distCurrent = Vector3.Distance(list[i], stone);

            if (distRes < distCurrent)
            {
                res = list[i];
            }
        }

        return res;
    }

    public static float Random(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }




    public static List<float> ReturnTheMinimos(List<float> list, int howMany, float epsilon)
    {
        float min = 0;
        List<float> minimos = new List<float>();
        for (int i = 0; i < howMany; i++)
        {
            min = ReturnMinimum(list);
            minimos.Add(min);
            for (int j = 0; j < list.Count; j++)
            {
                bool minAndListI = nearlyEqual(min, list[j], epsilon);
                if (minAndListI)
                {
                    list.RemoveAt(j);
                }
            }
        }
        return minimos;
    }

    public static float RetVectorComp(Vector3 vect, H axis)
    {
        float res = 0;
        if (axis == H.X)
        {
            res = vect.x;
        }
        else if (axis == H.Y)
        {
            res = vect.y;
        }
        else if (axis == H.Z)
        {
            res = vect.z;
        }
        return res;
    }

    public static List<float> ReturnDistances(Vector3[] vertices, Vector3 to)
    {
        List<float> distances = new List<float>();
        for (int i = 0; i < vertices.Length; i++)
        {
            distances.Add(Vector3.Distance(to, vertices[i]));
        }
        return distances;
    }

    public static List<int> ReturnTheIndexes(List<float> list, int howMany, float epsilon)
    {
        float min = 0;
        List<int> indexes = new List<int>();
        for (int i = 0; i < howMany; i++)
        {
            min = ReturnMinimum(list);
            for (int j = 0; j < list.Count; j++)
            {
                bool minAndListI = nearlyEqual(min, list[j], epsilon);
                if (minAndListI)
                {
                    indexes.Add(j + indexes.Count);
                    list[j] = 999f;
                }
            }
        }
        return indexes;
    }

    //http://floating-point-gui.de/errors/comparison/
    public static bool nearlyEqual(float a, float b, float epsilon)
    {
        float absA = Mathf.Abs(a);
        float absB = Mathf.Abs(b);
        float diff = Mathf.Abs(a - b);

        if (a == b)
        { // shortcut, handles infinities
            return true;
        }
        else if (a == 0 || b == 0 || diff < Single.MinValue)
        {	// a or b is zero or both are extremely close to it
            // relative error is less meaningful here
            return diff < (epsilon * Single.MinValue);
        }
        else
        { // use relative error
            return diff / (absA + absB) < epsilon;
        }
    }

    public static bool nearlyEqualString(float a, float b, int afterComa)
    {
        bool res = false;
        string truncate = "n" + afterComa.ToString();
        string aCont = a.ToString(truncate);
        string bCont = b.ToString(truncate);
        if (aCont == bCont) {res = true;}
        return res;
    }

    public static bool nearEqualByDistance(Vector3 a, Vector3 b, float maxDist)
    {
        bool res = false;
        float distance = Vector3.Distance(a, b);
        if (distance <= maxDist){res = true;}
        return res;
    }

    public static bool nearEqualByDistance(Vector2 a, Vector2 b, float maxDist)
    {
        bool res = false;
        float distance = Vector2.Distance(a, b);
        if (distance <= maxDist) { res = true; }
        return res;
    }

    public static float ReturnClosestVal(float currentClosest, float newValToEval, float stoneVal)
    {
        float currentDif = stoneVal - currentClosest;
        float newValDif = stoneVal - newValToEval;

        if (stoneVal > 0)
        {
            float min = ReturnMinimum(currentDif, newValDif);
            if (min == newValDif)
            {
                return newValToEval;
            }
            else return currentClosest;
        }
        else if(stoneVal < 0)
        {
            float max = ReturnMax(currentDif, newValDif);
            if (max == newValDif)
            {
                return newValToEval;
            }
            else return currentClosest;
        }
        else
        {
            currentDif = stoneVal + Mathf.Abs(currentClosest);
            newValDif = stoneVal + Mathf.Abs(newValToEval);
            float min = ReturnMinimum(currentDif, newValDif);
            if (min == newValDif)
            {
                return newValToEval;
            }
            else return currentClosest;
        }
    }

    public static float ReturnMinimum(float val1, float val2, float val3, float val4)
    {
        float min = val1;
        if (val2 < min)
        {
            min = val2;
        }
        if (val3 < min)
        {
            min = val3;
        }
        if (val4 < min)
        {
            min = val4;
        }
        return min;
    }

    public static float ReturnMinimum(float val1, float val2, float val3)
    {
        float min = val1;
        if (val2 < min)
        {
            min = val2;
        }
        if (val3 < min)
        {
            min = val3;
        }
        return min;
    }

    public static float ReturnMinimum(float val1, float val2)
    {
        float min = val1;
        if (val2 < min)
        {
            min = val2;
        }
        return min;
    }

    public static float ReturnMinimum(List<float> list)
    {
        float min = list[0];
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] < min)
            {
                min = list[i];
            }
        }
        return min;
    }

    public static float ReturnMinimumDifferentThanZero(List<float> list)
    {
        float min = list[0];
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] < min && list[i] != 0)
            {
                min = list[i];
            }
        }
        return min;
    }

    public static float ReturnMax(float val1, float val2, float val3, float val4)
    {
        float max = val1;
        if (val2 > max)
        {
            max = val2;
        }
        if (val3 > max)
        {
            max = val3;
        }
        if (val4 > max)
        {
            max = val4;
        }
        return max;
    }

    public static float ReturnMax(float val1, float val2)
    {
        float max = val1;
        if (val2 > max)
        {
            max = val2;
        }
        return max;
    }

    public static float ReturnMax(List<float> list)
    {
        float max = list[0];
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] > max)
            {
                max = list[i];
            }
        }
        return max;
    }

    public static int ReturnMax(List<int> list)
    {
        int max = list[0];
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] > max)
            {
                max = list[i];
            }
        }
        return max;
    }

    public static float Clamper(float amountToChange, float currentValue, float minValue, float maxValue)
    {
        currentValue = currentValue + amountToChange;

        if (currentValue >= maxValue)
        {
            currentValue = maxValue;
        }
        else if (currentValue <= minValue)
        {
            currentValue = minValue;
        }
        return currentValue;
    }

    public static int Clamper(int amountToChange, int currentValue, int minValue, int maxValue)
    {
        currentValue = currentValue + amountToChange;

        if (currentValue >= maxValue)
        {
            currentValue = maxValue;
        }
        else if (currentValue <= minValue)
        {
            currentValue = minValue;
        }
        return currentValue;
    }

    /// <summary>
    /// Goes around the min and max are inclusive
    /// </summary>
    /// <param name="amountToChange"></param>
    /// <param name="currentValue"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public static int GoAround(int amountToChange, int currentValue, int minValue, int maxValue)
    {
        currentValue = currentValue + amountToChange;
        if (currentValue > maxValue)
        {
            currentValue = minValue;
        }
        else if (currentValue < minValue)
        {
            currentValue = maxValue;
        }
        return currentValue;
    }


    public static float ResponsiveInputAxisTo(float normalizeTo, Dir axis, float currentValue, Dir direction)
    {
        if (
            direction == Dir.Left || direction == Dir.Right ||
            (UInput.IfHorizontalKeysIsPressed() && axis == Dir.Horizontal && direction == Dir.None))
        {
            currentValue = FindMathSign(currentValue, normalizeTo, direction);
        }
        else if (
            direction == Dir.Up || direction == Dir.Down ||
            (UInput.IfVerticalKeyIsPressed() && axis == Dir.Vertical && direction == Dir.None))
        {
            currentValue = FindMathSign(currentValue, normalizeTo, direction);
        }
        else currentValue = 0;

        return currentValue;
    }

    public static float FindMathSign(float currentValue, float normalizeTo, Dir direction)
    {
        //print("FindMathSign: " + currentValue);
        if (currentValue > 0 || direction == Dir.Right || direction == Dir.Up)
        {
            currentValue = normalizeTo;
        }
        else if (currentValue < 0 || direction == Dir.Left || direction == Dir.Down)
        {
            currentValue = -normalizeTo;
        }
        return currentValue;
    }

    public static int roundAround(int currentIndex, int amount, List<Vector3> listToRound)
    {
        currentIndex = currentIndex + amount;
        if (currentIndex >= listToRound.Count)
        {
            currentIndex = 0;
        }
        else if (currentIndex <= 0)
        {
            currentIndex = listToRound.Count - 1;
        }
        return currentIndex;
    }

    public static float changeValSmooth(float currentVal, float changeAmount, float localMult,
        float min, float max, float camSensitivity)
    {
        currentVal += changeAmount * Time.deltaTime * camSensitivity * localMult;
        if(currentVal>max)
        {
            currentVal = max;
        }
        else if(currentVal<min)
        {
            currentVal = min;
        }
        return currentVal;
    }

    public static float MatchVal(float currentVal, float valToMatch, float changeAmountAbsVal)
    {
        if(currentVal < valToMatch)
        {
            currentVal += changeAmountAbsVal;
        }
        else if(currentVal > valToMatch)
        {
            currentVal -= changeAmountAbsVal;
        }
        return currentVal;
    }

    public static float ReturnMinFromVector3(Vector3 start, Vector3 end, H axis)
    {
        float min = 0;
        if (axis == H.X)
        {
            min = UMath.ReturnMinimum(start.x, end.x);
        }
        else if (axis == H.Z)
        {
            min = UMath.ReturnMinimum(start.z, end.z);
        }
        return min;
    }

    public static float ReturnMaxFromVector3(Vector3 start, Vector3 end, H axis)
    {
        float max = 0;
        if (axis == H.X)
        {
             max = UMath.ReturnMax(start.x, end.x);
        }
        else if (axis == H.Z)
        {
             max = UMath.ReturnMax(start.z, end.z);
        }
        return max;
    }

    /// <summary>
    /// Return the diff btw max value and min value 
    /// </summary>
    public static float ReturnDiffBetwMaxAndMin(List<Vector3> list, H axis )
    {
        float res = 0;
        if (axis == H.Y)
        {
            List<float> yS = UList.ReturnAxisList(list, axis);
            float min = ReturnMinimum(yS);
            float max = ReturnMax(yS);
            res = Mathf.Abs( Mathf.Abs( max )- Mathf.Abs( min) );
            
        }
        return res;
    }

    /// <summary>
    /// Scale the val by a percentage.... the sign of the percentage matters 
    /// </summary>
    /// <param name="val"></param>
    /// <param name="percentage"></param>
    /// <returns></returns>
    public static float ScalePercentage(float val, float percentage)
    {
        return val + (val/100* percentage);
    }





    static List<int> _sign = new List<int>() { -1, 1 };
    /// <summary>
    /// Will return randomly -1 or 1
    /// </summary>
    /// <returns></returns>
    public static int RandomSign()
    {
        return _sign[GiveRandom(0, _sign.Count)];
    }
}
