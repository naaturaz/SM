using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Nutrition
{
    private Person _person;

    private float _calNeededNow;

    private float _daysWithoutEat = 1f;//everytime he calls the EatFunction is going to be taken as a 
    //half a day
    //if he calls the eat and there is not food then 0.5f is added to this

    public float CalNeededNow
    {
        get { return _calNeededNow; }
        set { _calNeededNow = value; }
    }



    public Nutrition() { }

    public Nutrition(Person person)
    {
        _person = person;
    }

    float CalculateCalNeedNow()
    {
        //http://www.globalrph.com/resting_metabolic_rate.cgi
        return ((9.99f * _person.Weight) + (6.25f * _person.Height) - (4.92f * _person.Age) + GenderAdd()) * ActivityFactor();
    }

    private float ActivityFactor()
    {
        if (_person.Work == null)
        {
            return UMath.Random(1.375f, 1.55f);
        }
        return UMath.Random(1.6f, 1.8f);
    }

    int GenderAdd()
    {
        if (_person.Gender == H.Male)
        {
            return 5;
        }
        return -161;
    }

    internal float HowManyKGINeedOfThisToSupplyMyNeed(P prod)
    {
        var days = _daysWithoutEat;
        var nutritionPerKG = GetProdCalPerKG(prod);

        _calNeededNow -= CalculateCalNeedNow() * days;
        return _calNeededNow / nutritionPerKG;
    }

    /// <summary>
    /// Cal x KG of a product 
    /// </summary>
    /// <param name="prod"></param>
    /// <returns></returns>
    private float GetProdCalPerKG(P prod)
    {
        //corn is 3600 all corn
        //cob 113 calories per cob, and a cob is 1.2lbs
        return 3600;
    }

    //If you eliminate 500 kcal per day from your diet (or approximately 3500 kcal/week),
    //you should be on track to meet this degree of weight loss.
    //Note: there is approximately 3500 calories per one pound of fat (0.45 kg).
    //
    //Then 500 cal lost in a day = 0.0643KG lost in a day
    internal void AteThisMuch(P prod, float gotAmt)
    {
        _calNeededNow += GetProdCalPerKG(prod) * gotAmt;
        var kgChanged = 0f;

        //if negative should drop weight//until starvation
        if (_calNeededNow < 0)
        {
            kgChanged = ConvertCalIntoKG(_calNeededNow);
        }
        //if 0 keeps the weight 
            //
        //if cal are positive then can increase the weight//until a reasonable weight for person
        else if (_calNeededNow > 0)
        {
            kgChanged = ConvertCalIntoKG(_calNeededNow);
        }

        if (kgChanged != 0)
        {
            _person.NewWeight(kgChanged);
            _calNeededNow = 0;
            _daysWithoutEat = 1f;
        }
    }

    //Then 500 cal lost in a day = 0.0643KG lost in a day
    float ConvertCalIntoKG(float cal)
    {
        return (cal/500f)*0.0643f;
    }

    internal float CalNeededNowUpdate()
    {
        var days = _daysWithoutEat;

        var tempCalNeededNow = CalculateCalNeedNow() * days;

        return tempCalNeededNow;
    }

    /// <summary>
    /// Bz when loading the person is not saved
    /// </summary>
    /// <param name="person"></param>
    internal void SetPerson(Person person)
    {
        _person = person;
    }
}
