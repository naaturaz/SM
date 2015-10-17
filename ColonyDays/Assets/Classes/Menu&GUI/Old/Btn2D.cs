using UnityEngine;
using System.Collections;

public class Btn2D : Button
{

    //use to hold the mat of MyTransform that will be changed 
    Material[] mats;
    bool fadeComplete;
    string myTransformOriginalName = "";

    private Transform _myTransform;
    private float _floatFadeValue;
    private bool _isNowHovered;
    private bool _destroyNow;

    public Transform MyTransform
    {
        get { return _myTransform; }
        set { _myTransform = value; }
    }
    public float FloatFadeValue
    {
        get { return _floatFadeValue; }
        set { _floatFadeValue = value; }
    }

    public bool IsNowHovered
    {
        get { return _isNowHovered; }
        set { _isNowHovered = value; }
    }
    public bool DestroyNow
    {
        get { return _destroyNow; }
        set { _destroyNow = value; }
    }

	// Use this for initialization
	void Start () 
    {
        base.Start();        
	}

    //will assign materials 
    public void StartMaterial()
    {
        if (MyTransform != null)
        {
            //get the materials from MyTransform and then assign the first one...
            mats = MyTransform.GetComponent<Renderer>().materials;
            myTransformOriginalName = MyTransform.name;
            MyTransform.name = transform.name;

            //this WAS A 6 HOURS BUG ... WAS REFERENCING THEN AT THE BEGGINING THATS
            //WHY WOULD LOOK ABRUPT IN THE FADE 
            //mats[0] = materiales[0];
        } 
    }

    //fade materials at speed pass and renderer it to MyTransform if is not null
    void FadeMaterial(Material one, Material two, float speedPass)
    {
        if (one != two )
        {
            FadeDirection = "FadeIn";
            FloatFadeValue = FadeAction(FadeDirection, two.color.a, speedPass);
           // print("FloatFadeValue." + FloatFadeValue + name);

            if (!DestroyNow)
            {
                Texture newTexture = two.GetTexture(0);
                MyTransform.GetComponent<Renderer>().material.SetTexture(0, newTexture);
            }
            if (MyTransform != null)
            {
                MyTransform.GetComponent<Renderer>().material.Lerp(one, two, FloatFadeValue);
            }
        }
    }

    //deals with the fader status...
    void FadeDealer2d()
    {
        FadeDirection = "FadeIn";//ALWAYS bz we are fading in into the 2nd obj always
        if (DestroyNow)
        {
            FadeMaterial(mats[0], materiales[0], FadeSpeedProp);
        }
        else if (IsNowHovered)
        {
            FadeMaterial(mats[0], materiales[2], FadeSpeedProp);
        }
        else
        {
            FadeMaterial(mats[0], materiales[1], FadeSpeedProp);
        }
    }

    public void Destroy2dBtn()
    {
        DestroyNow = true;
    }

	// Update is called once per frame
	void Update () 
    {
        base.Update();
        FadeDealer2d();
        //base.FadeDealer(MyTransform);

        if (DestroyNow) { CheckToFinalDestroy(); }
	}

    /// <summary>
    /// If mats[0].color.a.ToString <= 0.11 will make it fully tansparent and destroy it 
    /// </summary>
    void CheckToFinalDestroy()
    {
        //////////////////////////////////////////////2 DAYS BUG//////////////////////////////////////
        //STILL DONT KNOW WHERE THE HEART OBJT AND PAUSE OBJ WHERE KEPT BUT NOW WILL FORCE THEM TO DIE
        //WITH THIS IF STATEMENT AND WE MAKE SURE TOO IS FULLY TRANSPARENT 
        float round = float.Parse(mats[0].color.a.ToString("n3"));
        if(round <= 0.11f)
        {
            round = 0;
            Color t = mats[0].color;
            t.a = 0;
            mats[0].color = t;
        }
        if (DestroyNow && round <= 0 //destroyNow && mats[0].color.a < 0.01f
            ||
            MyTransform.name != transform.name && MyTransform.name != myTransformOriginalName)
        {
            Destroy(gameObject);
            Program.twoDMHandler.UpdateList();
            //print("inDesrytuyo." + name+"." + Id);
        }
    }
}