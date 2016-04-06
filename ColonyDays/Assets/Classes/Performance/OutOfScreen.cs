using UnityEngine;
using System.Collections;

public class OutOfScreen
{

    //there are 2 types now, Person and StillElement
    private H _type;

    private Person _person;
    private Animator _animator;

    private BoxCollider _boxCollider;
    private Renderer _renderer;

    private StillElement _stillElement;

    private bool _onScreenNow;
    private bool _oldState;


    public OutOfScreen(Person person)
    {
        _type = H.Person;
        _person = person;
        InitPerson();
    }

    private void InitPerson()
    {
        _animator = _person.gameObject.GetComponent<Animator>();
        _boxCollider = _person.gameObject.GetComponent<BoxCollider>();
        _renderer = _person.Geometry.gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
	public void Update ()
	{
        if (_renderer.isVisible && !_onScreenNow)
        {
            _onScreenNow = true;
            SwitchNow();
        }
        else if (!_renderer.isVisible && _onScreenNow)
        {
            _onScreenNow = false;
            SwitchNow();
        }
	}


    private void SwitchNow()
    {
        if (_onScreenNow)
        {
            OnBecameVisible();
        }
        else OnBecameInvisible();
    }

    void OnBecameVisible()
    {
        //Debug.Log("became visible " + _person.MyId);
        Activate();
    }

    void OnBecameInvisible()
    {
        //Debug.Log("became Invisible " + _person.MyId);
        DeActivate();
    }

    internal void Activate()
    {
        _boxCollider.enabled = true;

        if (_type==H.Person)
        {
            ActivatePerson();
        }
    }

    private void ActivatePerson()
    {
        _animator.enabled = true;
    }

    internal void DeActivate()
    {
        _boxCollider.enabled = false;

        if (_type == H.Person)
        {
            DeActivatePerson();
        }
    }

    private void DeActivatePerson()
    {
        _animator.enabled = false;

    }
}
