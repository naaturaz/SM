using UnityEngine;

public class Button : GuiClass
{
    public Btn btnAction;

    internal int autoDestroyInSec = 1;
    internal bool isAutoDestroy;
    internal float destroyStartTime;

    private string _fadeDirection;
    private string _fadeState;
    private float _moveSpeed;
    public bool isClickAble = true;//if is false wont do anything on click event

    //use for Btn3D
    internal bool isStarting;//allows rotation and move once the object is created

    internal bool isDestroying;

    public string FadeDirection
    {
        get { return _fadeDirection; }
        set { _fadeDirection = value; }
    }

    public string FadeState
    {
        get { return _fadeState; }
        set { _fadeState = value; }
    }

    // Use this for initialization
    public void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public void Update()
    {
        if (isAutoDestroy)
        { AutoDestroy(); }
    }

    private void AutoDestroy()
    {
        if (Time.time > destroyStartTime + autoDestroyInSec)
        {
            Destroy();
        }
    }

    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    //will fade a current color given in the direction of "FadeDirection"
    public Color FadeAlphaChannel(Color currentColor)
    {
        currentColor.a = FadeAction(FadeDirection, currentColor.a, FadeSpeedProp);
        return currentColor;
    }

    /// <summary>
    /// Will fade value btw 0-1 or 1-0 depedning the direction
    /// </summary>
    /// <param name="fadeDirectionPass">will indicate if is going towards 0 or 1 the fade value</param>
    /// <param name="targetMatchValue">this is the value we want to match</param>
    /// <param name="speedPass">the speed will be matched</param>
    /// <returns>from zero to one </returns>
    public float FadeAction(string fadeDirectionPass, float targetMatchValue, float speedPass = 30f)
    {
        if (fadeDirectionPass == "FadeIn")
        {
            if (targetMatchValue < 1f)
            {
                targetMatchValue = targetMatchValue + 0.1f * Time.deltaTime * speedPass;
                FadeState = "FadingIn";
            }
            else if (targetMatchValue >= 1f)
            {
                FadeDirection = "";
                FadeState = "FadedIn";
            }
        }
        else if (fadeDirectionPass == "FadeOut")
        {
            if (targetMatchValue > 0)
            {
                targetMatchValue = targetMatchValue - 0.1f * Time.deltaTime * speedPass;
            }
            else if (targetMatchValue <= 0)
            {
                FadeDirection = "";
                FadeState = "FadedOut";
                Destroyer();
            }
        }
        return targetMatchValue;
    }

    ///will fade color in objects
    ///so far includes:
    ///     1 - guiText,
    ///     2 - 3dModel with a Transpareent Material
    ///     3 - Child same 3dModels
    ///     4 - If a specific Transofrm obj is specified will fade tht one
    internal void FadeDealer(Transform myTransform = null)
    {
        if (myTransform == null)
        {
            //if FadeDirection not equal "" and is not null...
            if (FadeDirection != "" && FadeDirection != null)
            {
                //check if is guiText
                if (this.GetComponent<GUIText>() != null)
                {
                    this.GetComponent<GUIText>().color = FadeAlphaChannel(this.GetComponent<GUIText>().color);
                }
                else if (this.GetComponent<Renderer>().material != null)
                {
                    this.GetComponent<Renderer>().material.color = FadeAlphaChannel(this.GetComponent<Renderer>().material.color);
                }
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    this.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.color =
                        FadeAlphaChannel(this.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.color);
                }
            }
        }
        else
        {
            myTransform.GetComponent<Renderer>().material.color = FadeAlphaChannel(myTransform.GetComponent<Renderer>().material.color);
        }
    }

    public void Destroyer(bool isToDestroyInmediate = false)
    {
        isStarting = false;//needs to be false when we kill so moveTo InitialPosition
        isDestroying = true;//

        if (FadeState == "" || FadeState == null)
        {
            base.Destroy();
        }
        if (FadeState != "")
        {
            FadeDirection = "FadeOut";
        }
        if (FadeState == "FadedOut" && isToDestroyInmediate)
        {
            Destroy(gameObject);
        }
    }
}