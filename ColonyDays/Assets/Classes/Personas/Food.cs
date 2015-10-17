using System.Collections.Generic;
using System.Linq;

public class Food
{
    List<NutritionValue> _nValues = new List<NutritionValue>();//nutrition values

    public Food()
    {
        LoadNutriVals();
    }

    public List<NutritionValue> NValues
    {
        get { return _nValues; }
    }

    private void LoadNutriVals()
    {
        _nValues.Add(new NutritionValue(P.Bean, 0.9f));
        _nValues.Add(new NutritionValue(P.Potato, 0.7f));
        _nValues.Add(new NutritionValue(P.SugarCane, 0.5f));
        _nValues.Add(new NutritionValue(P.Corn, 0.6f));

        _nValues.Add(new NutritionValue(P.Chicken, 2f));
        _nValues.Add(new NutritionValue(P.Egg, 1.5f));
        _nValues.Add(new NutritionValue(P.Pork, 2f));
        _nValues.Add(new NutritionValue(P.Beef, 3f));

        _nValues.Add(new NutritionValue(P.Fish, 3f));

        _nValues.Add(new NutritionValue(P.Sugar, 0.5f));

        _nValues.OrderBy(a => a.NutritionVal).ToList();
    }

    public List<P> OrderListByNutriVal(List<P> list)
    {
        List<NutritionValue> loc = new List<NutritionValue>();

        for (int i = 0; i < list.Count; i++)
        {
            var nutriVal = PullNutriVal(list[i]);
            loc.Add(new NutritionValue(list[i], nutriVal));
        }

        loc = loc.OrderByDescending(a => a.NutritionVal).ToList();

        List<P> res = new List<P>();

        for (int i = 0; i < loc.Count; i++)
        {
            res.Add(loc[i].Nutrient);
        }
        return res;
    }

    float PullNutriVal(P ele)
    {
        for (int i = 0; i < _nValues.Count; i++)
        {
            if (ele == _nValues[i].Nutrient)
            {
                return _nValues[i].NutritionVal;
            }
        }
        return -1f;
    }
}

public class NutritionValue
{

    private P _nutrient;//the food itself like : egg
    private float _nutritionVal;

    public float NutritionVal
    {
        get { return _nutritionVal; }
        set { _nutritionVal = value; }
    }

    public P Nutrient
    {
        get { return _nutrient; }
        set { _nutrient = value; }
    }

    public NutritionValue(){}

    public NutritionValue(P nutrient, float nutriVal)
    {
        _nutrient = nutrient;
        _nutritionVal = nutriVal;
    }

   
}

