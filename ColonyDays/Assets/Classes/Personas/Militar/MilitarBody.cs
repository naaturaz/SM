using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MilitarBody
{
    NavMeshAgent _agent;
    GameObject _go;


    Animator _myAnimator;
    private string _currentAni;
    private string savedAnimation;

    public MilitarBody(GameObject go)
    {
        _go = go;
        SetScaleByAge();
        _myAnimator = _go.GetComponent<Animator>();
        SetCurrentAni("isIdle", _currentAni);

    }

    public void NewSpeed(int newSpeed)
    {
        _myAnimator.speed = newSpeed;
    }

    public void MusketAttack()
    {
        SetCurrentAni("isShot", _currentAni);
    }

    public void SwordAttack()
    {
        SetCurrentAni("isSword", _currentAni);
    }

    public void Run()
    {
        SetCurrentAni("isRun", _currentAni);
    }

    public void SetCurrentAni(string animationPass, string oldAnimation)
    {
        if (!_myAnimator.enabled)
        {
            savedAnimation = animationPass;
            return;
        }
        if (string.IsNullOrEmpty(animationPass))
        {
            return;
        }
        savedAnimation = "";

        _currentAni = animationPass;
        _myAnimator.SetBool(animationPass, true);

        //otherwise will stop the one intended to be playing now 
        if (_currentAni != oldAnimation)
        {
            _myAnimator.SetBool(oldAnimation, false);
        }
    }





    //the yearly grow for each Gender. For this be effective the GameObj scale must
    // be initiated at 0.26f in all axis
    private float maleGrow = 0.01333f;
    private float femaleGrow = 0.01111f;


    /// <summary>
    /// Will set the body scale by Gender to this be effective the GameObj scale must
    /// be initiated at 0.26f in all axis
    /// </summary>
    void SetScaleByAge()
    {
        var toAdd = 0f;
        var addAmnt = maleGrow;
        int ageHere = 20;

        //starting age is always 2 .. bz thas the calculus i was based on 
        for (int i = 2; i < ageHere + 1; i++)
        { toAdd += addAmnt; }

        AddToBodyScale(toAdd);
    }

    /// <summary>
    /// Will add the scale phisically to the body
    /// </summary>
    /// <param name="toAdd"></param>
    void AddToBodyScale(float toAdd)
    {
        var localScale = _go.transform.localScale;
        var singleS = localScale.x + toAdd;
        var newScale = new Vector3(singleS, singleS, singleS);
        _go.transform.localScale = newScale;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
