//life cycle corn 4 month
using UnityEngine;

public class Plant : MonoBehaviour
{
    private P _type;//type of plant. ex : Bean
    private Building _building;//the buildign tht contains this plant 
    private FieldFarm _fieldFarm;//the dfarm tht contains this plant

    private int _growGen; //btw 90-100 will indicate how farr will go a plant just by genes

    //the duration of a plant in days
    private int _lifeDuration = 120;//120 is for corn 

    //the factor of production a carrots will produce less as a corn. in a plant in kg
    private int _productionFactor = 10;

    //when was seeded
    private MDate _seedDate;

    //at wht step of growing is,  0.1-1
    private float _currentGrowStep;

    //the amout will grow the gameObj created so it happens nie and smotth the grow 
    private float _amtToGrow;

    private bool _isReadyToHarvest;

    private MDate _readyToHarvestDate;
    private int _daysToRotten;
    private bool _isRotten;

    public Plant() { }

    static public Plant Create(P type, Vector3 origen, Building container, FieldFarm fieldFarm)
    {
        var root = Root.RetPrefabRoot(type);

        Plant obj = null;
        obj = (Plant)Resources.Load(root, typeof(Plant));
        obj = (Plant)Instantiate(obj, origen, Quaternion.identity);

        obj.transform.parent = container.transform;
        obj.ObjInit(container, fieldFarm, type);
        return obj;
    }

    public void ObjInit(Building container, FieldFarm fieldFarm, P plantType)
    {
        _type = plantType;
        _building = container;
        _fieldFarm = fieldFarm;

        DefineRottingDays();
        //CreateBasePlane();

        transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));

        //define
        DefineLifeDuration();
        //_productionFactor    
        //grow factor 

        
    }

    void CreateBasePlane()
    {
        var dimen = _fieldFarm.SpaceBtwPlants - 0.5f;

        var locPoly = UPoly.CreatePolyFromVector3(transform.position, dimen, dimen);
        var basePlane = CreatePlane.CreatePlan(Root.createPlane, Root.matFarmSoil, raiseFromFloor: 0.08f, container: transform);
        basePlane.UpdatePos(locPoly);
    }

    //how long take to a plant to rotten its fruits 
    private void DefineRottingDays()
    {
        _daysToRotten = 30;
    }

    void DefineLifeDuration()
    {
        if (_type == P.Bean)
        {
            _lifeDuration = 120;
        }
        //http://homeguides.sfgate.com/long-banana-tree-live-46735.html
        else if (_type == P.Banana)//at 4th year is better take it down and reseed
        {
            _lifeDuration = 30 * 17;
        }
        //http://www.ask.com/science/life-cycle-coconut-tree-580819cec6ff3539
        else if (_type == P.Coconut)//dies with 60-80 years
        {
            _lifeDuration = 8 * 360;
        }
    }

    void Start()
    {
        _growGen = Random.Range(90, 100);
        DetermineSeedDate();
    }

    private void DetermineSeedDate()
    {
        _seedDate = Program.gameScene.GameTime1.CurrentDate();
    }

    void CheckIfCanGrow()
    {
        var timeInSoil = Program.gameScene.GameTime1.ElapsedDateInDaysToDate(_seedDate);
        float advance = (float)timeInSoil / (float)_lifeDuration;

        if (advance > _currentGrowStep && advance < 1f)
        {
            Grow();
        }
        else if (advance > 1)
        {
            MarkToHarvest();
        }
    }

    private void MarkToHarvest()
    {
        if (_isReadyToHarvest)
        {
            return;
        }

        _isReadyToHarvest = true;
        _fieldFarm.HarvestCheck();
        _readyToHarvestDate = Program.gameScene.GameTime1.CurrentDate();
    }

    const float GROWFACTOR = 0.01f;
    //Will grow in 10 percent increments
    private void Grow()
    {
        //divide by current step so its always not more and more
        //pls GROWFACTOR so we dont divide a zero 
        var addWorkedAmt = (_fieldFarm.GiveMeMyWorkedAmt() + GROWFACTOR) / _currentGrowStep / 100000;
        _fieldFarm.PlantGrew();

        _currentGrowStep += 0.1f;
        _amtToGrow = GROWFACTOR + ((float)_growGen / 10000) + addWorkedAmt;

        if (_amtToGrow > 1)
        {
            _amtToGrow = 0.01f;
        }
    }




    private int checkEverySoManyFrames = 120;
    private int count;

    void FixedUpdate()
    {
        count++;

        if (count > checkEverySoManyFrames)
        {
            CheckIfCanGrow();
            CheckIfIsRottenByNow();

            count = 0;
        }

        CouldGrowPlantNow();
        
    }

    /// <summary>
    /// This will take care of if the plant is being ready for harvest for a long time 
    /// </summary>
    private void CheckIfIsRottenByNow()
    {
        if (_isRotten || _readyToHarvestDate == null)
        {
            return;
        }

        var days = Program.gameScene.GameTime1.ElapsedDateInDaysToDate(_readyToHarvestDate);

        if (days > _daysToRotten)
        {
            _isRotten = true;
            DestroyPlant();
        }
    }

    /// <summary>
    /// So the grows looks cool and smooth
    /// </summary>
    private void CouldGrowPlantNow()
    {
        if (_amtToGrow > 0.01)
        {
            _amtToGrow -= 0.01f;
            ScaleGameObject(0.01f);
        }
    }

    void ScaleGameObject(float toAdd)
    {


        var localScale = gameObject.transform.localScale;
        var singleX = localScale.x + toAdd;
        var singleY = localScale.y + toAdd;
        var singleZ = localScale.z + toAdd;

        var newScale = new Vector3(singleX, singleY, singleZ);
        gameObject.transform.localScale = newScale;
    }

    internal void Harvest()
    {
        if (_isRotten)
        {
            return;
        }

        var amt = WhatIProduce();
        //_fieldFarm.AddProducedByPlant(amt);

        _building.Produce(amt);

        DestroyPlant();
    }

    private void DestroyPlant()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// bz plant is rotten
    /// </summary>
    private void Hide()
    {
        GetComponent<Renderer>().enabled = false;
    }

    /// <summary>
    /// The amount this plant produce 
    /// </summary>
    /// <returns></returns>
    private int WhatIProduce()
    {
        var localScale = gameObject.transform.localScale;
        return  (int)(localScale.y *10) * _productionFactor;
    }
}


