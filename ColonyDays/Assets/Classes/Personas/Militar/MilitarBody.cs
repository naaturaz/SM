using System;
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

    private float _initialAgentSpeed;

    public NavMeshAgent Agent
    {
        get
        {
            return _agent;
        }

        set
        {
            _agent = value;
        }
    }

    public MilitarBody(GameObject go)
    {
        _go = go;
        SetScaleByAge();
        _myAnimator = _go.GetComponent<Animator>();
        SetCurrentAni("isIdle", _currentAni);
        _agent = _go.GetComponent<NavMeshAgent>();
        _initialAgentSpeed = Agent.speed;

    }

    public void NewSpeed()
    {
        //like when running needs to speed up a bit;
        var speedAdd = CalculateNewAdd(_currentAni);
        var speedFin = (_initialAgentSpeed + speedAdd) * Program.gameScene.GameSpeed;

        _myAnimator.speed = speedFin;
        //_agent.speed = speedFin;
    }

    static private float CalculateNewAdd(string currentAni)
    {
        if (currentAni == "isRun")
        {
            return 0.4f;
        }
        return 0;
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
        NewSpeed();
    }

    internal void DisableAgent()
    {
        Agent.enabled = false;
    }


    General deb;

    void Debugg(Vector3 pt)
    {
        if (deb != null)
        {
            deb.Destroy();
        }
        deb = UVisHelp.CreateHelpers(pt, Root.redCube);
    }

    internal void ActivateAgent(Vector3 dest)
    {
        Agent.enabled = true;
        if (Agent.isOnNavMesh)
        {
            Debugg(dest);
            Agent.SetDestination(dest);
            Run();
        }
    }







    #region Scale
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
    #endregion

}
