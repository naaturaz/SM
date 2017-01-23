//life cycle corn 4 month
using UnityEngine;

public class Plant : General
{
    private P _type;//type of plant. ex : Bean
    private Building _building;//the buildign tht contains this plant 
    private FieldFarm _fieldFarm;//the dfarm tht contains this plant

    private float _growGen; //btw 90-100 will indicate how farr will go a plant just by genes

    //the duration of a plant in days
    private int _lifeDuration = 120;//120 is for corn 

    //the factor of production a carrots will produce less as a corn. in a plant in kg
    private float _productionFactor = 10;

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

    public P Type
    {
        get { return _type; }
        set { _type = value; }
    }

    public float GrowGen
    {
        get { return _growGen; }
        set { _growGen = value; }
    }

    public int LifeDuration
    {
        get { return _lifeDuration; }
        set { _lifeDuration = value; }
    }

    public MDate SeedDate
    {
        get { return _seedDate; }
        set { _seedDate = value; }
    }

    public float CurrentGrowStep
    {
        get { return _currentGrowStep; }
        set { _currentGrowStep = value; }
    }

    public float AmtToGrow
    {
        get { return _amtToGrow; }
        set { _amtToGrow = value; }
    }

    public bool IsReadyToHarvest
    {
        get { return _isReadyToHarvest; }
        set { _isReadyToHarvest = value; }
    }

    public MDate ReadyToHarvestDate
    {
        get { return _readyToHarvestDate; }
        set { _readyToHarvestDate = value; }
    }

    public int DaysToRotten
    {
        get { return _daysToRotten; }
        set { _daysToRotten = value; }
    }

    public bool IsRotten
    {
        get { return _isRotten; }
        set { _isRotten = value; }
    }

    static public Plant Create(P type, Vector3 origen, Building container, FieldFarm fieldFarm)
    {
        var root = Root.RetPrefabRoot(type);

        Plant obj = null;
        obj = (Plant)Resources.Load(root, typeof(Plant));
        obj = (Plant)Instantiate(obj, origen, Quaternion.identity);

        obj.transform.SetParent( container.transform);
        obj.ObjInit(container, fieldFarm, type);
        return obj;
    }

    public void ObjInit(Building container, FieldFarm fieldFarm, P plantType)
    {
        HType = H.Plant;
        _type = plantType;
        _building = container;
        _fieldFarm = fieldFarm;

        DefineRottingDays();
        //CreateBasePlane();

        transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));

        //define
        DefineLifeDuration();

        transform.name = "";//so it renames 
        DefineNameAndMyID();
        
        //re add
        _fieldFarm.BatchAdd(this);
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

        if (_type == P.Coconut)
        {
            _daysToRotten = 120;
        }
    }

    /// <summary>
    /// Always in days
    /// </summary>
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
        else if (_type == P.CoffeeBean)//dies with 60-80 years
        {
            _lifeDuration = 6 * 30;
        } 
        else if (_type == P.Cacao)//dies with 60-80 years
        {
            _lifeDuration = 7 * 30;
        } 
        else if (_type == P.Potato)//dies with 60-80 years
        {
            _lifeDuration = 3 * 30;
        } 
        else if (_type == P.SugarCane)//dies with 60-80 years
        {
            _lifeDuration = 7 * 30;
        }
        else if (_type == P.TobaccoLeaf)//dies with 60-80 years
        {
            _lifeDuration = 8 * 30;
        }  
        else if (_type == P.Corn)//dies with 60-80 years
        {
            _lifeDuration = 3 * 30;
        } 
        else if (_type == P.Henequen)//dies with 60-80 years
        {
            _lifeDuration = 3 * 30;
        } 
        else if (_type == P.Cotton)//dies with 60-80 years
        {
            _lifeDuration = 3 * 30;
        }
    }

    void Start()
    {
        _growGen = Random.Range(.1f, .9f);
        DetermineSeedDate();

        //sets the Production factor of this plant 
        _productionFactor = Program.gameScene.ExportImport1.ReturnProduceFactor(_type);

        SavePlant();
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

    const float GROWFACTOR = 0.001f;//0.01
    //Will grow in 10 percent increments
    private void Grow()
    {
        //divide by current step so its always not more and more
        //pls GROWFACTOR so we dont divide a zero 
        var addWorkedAmt = (_fieldFarm.GiveMeMyWorkedAmt() + GROWFACTOR) / _currentGrowStep / 1;
        _fieldFarm.PlantGrew();

        _currentGrowStep += 0.1f;
        _amtToGrow = Program.gameScene.GameTime1.TimeFactorInclSpeed()
            *(GROWFACTOR + (_growGen / 1000) + addWorkedAmt/500) ; //100      50

        //Debug.Log("amt to grow: "+_amtToGrow);
        if (float.IsInfinity(_amtToGrow))
        {
            _amtToGrow = 0f;
        }
    }




    private int checkEverySoManyFrames = 240;
    private int count;

    void Update()
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
        if (_amtToGrow > 0)
        {
            _amtToGrow -= 0.01f;
            ScaleGameObject(0.01f);

            SavePlant();
        }
    }

    void SavePlant()
    {
        if (_building.PlantSave1 == null)
        {
            _building.PlantSave1 = new PlantSave();
        }

        _building.PlantSave1.SavePlant(this);
    }

    void ScaleGameObject(float toAdd)
    {
        var localScale = gameObject.transform.localScale;
        //var singleX = localScale.x + toAdd;
        //var singleY = localScale.y + toAdd;
        //var singleZ = localScale.z + toAdd;



        var addScale = localScale * toAdd;
        var final = localScale + addScale;

        // var newScale = new Vector3(singleX, singleY, singleZ);
        gameObject.transform.localScale = final;
    }

    internal void Harvest()
    {
        if (_isRotten)
        {
            return;
        }

        var amt = WhatIProduce();
        _building.Produce(amt);
        DestroyPlant();
    }

    private void DestroyPlant()
    {
        //_fieldFarm.BatchRemove(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// The amount this plant produce 
    /// </summary>
    /// <returns></returns>
    private float WhatIProduce()
    {
        //how much grew
        var localScale = gameObject.transform.localScale;
        return localScale.y * _productionFactor * .5f; // .75f  1.25  2.5
    }


    private bool wasLoaded;
    public void LoadPlant(PlantSave plant)
    {
        if (wasLoaded)
        {
            return;
        }
        wasLoaded = true;


        _type = plant.Type;//type of plant. ex : Bean

        _growGen = plant.GrowGen; //btw 90-100 will indicate how farr will go a plant just by genes

        //the duration of a plant in days
        _lifeDuration = plant.LifeDuration;//120 is for corn 

        //when was seeded
        _seedDate = plant.SeedDate;

        //at wht step of growing is,  0.1-1
        _currentGrowStep = plant.CurrentGrowStep;

        //the amout will grow the gameObj created so it happens nie and smotth the grow 
        _amtToGrow = plant.AmtToGrow;

        _isReadyToHarvest = plant.IsReadyToHarvest;

        _readyToHarvestDate = plant.ReadyToHarvestDate;
        _daysToRotten = plant.DaysToRotten;
        _isRotten = plant.IsRotten;

        gameObject.transform.localScale = plant.LocalScale;
    }
}

public class PlantSave
{
    private P _type;//type of plant. ex : Bean

    private float _growGen; //btw 90-100 will indicate how farr will go a plant just by genes

    //the duration of a plant in days
    private int _lifeDuration = 120;//120 is for corn 

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

    private Vector3 _localScale;

    public P Type
    {
        get { return _type; }
        set { _type = value; }
    }

    public float GrowGen
    {
        get { return _growGen; }
        set { _growGen = value; }
    }

    public int LifeDuration
    {
        get { return _lifeDuration; }
        set { _lifeDuration = value; }
    }

    public MDate SeedDate
    {
        get { return _seedDate; }
        set { _seedDate = value; }
    }

    public float CurrentGrowStep
    {
        get { return _currentGrowStep; }
        set { _currentGrowStep = value; }
    }

    public float AmtToGrow
    {
        get { return _amtToGrow; }
        set { _amtToGrow = value; }
    }

    public bool IsReadyToHarvest
    {
        get { return _isReadyToHarvest; }
        set { _isReadyToHarvest = value; }
    }

    public MDate ReadyToHarvestDate
    {
        get { return _readyToHarvestDate; }
        set { _readyToHarvestDate = value; }
    }

    public int DaysToRotten
    {
        get { return _daysToRotten; }
        set { _daysToRotten = value; }
    }

    public bool IsRotten
    {
        get { return _isRotten; }
        set { _isRotten = value; }
    }

    public Vector3 LocalScale
    {
        get { return _localScale; }
        set { _localScale = value; }
    }


    public PlantSave() { }


    public void SavePlant(Plant plant)
    {
          _type = plant.Type;//type of plant. ex : Bean

    _growGen= plant.GrowGen; //btw 90-100 will indicate how farr will go a plant just by genes

    //the duration of a plant in days
    _lifeDuration = plant.LifeDuration;//120 is for corn 

    //when was seeded
    _seedDate= plant.SeedDate;

    //at wht step of growing is,  0.1-1
    _currentGrowStep= plant.CurrentGrowStep;

    //the amout will grow the gameObj created so it happens nie and smotth the grow 
    _amtToGrow= plant.AmtToGrow;

    _isReadyToHarvest= plant.IsReadyToHarvest;

    _readyToHarvestDate= plant.ReadyToHarvestDate;
     _daysToRotten= plant.DaysToRotten;
     _isRotten= plant.IsRotten;

        _localScale = plant.gameObject.transform.localScale;
    }
}


