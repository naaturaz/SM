using UnityEngine;
using System.Collections;
/// <summary>
/// Each Person , Still element will have one instance of this class
/// </summary>
public class OutOfScreen
{

    //there are 2 types now, Person and StillElement
    private H _type;

    private Person _person;
    private Animal _animal;
    private Animator _animator;

    private BoxCollider _boxCollider;
    private Renderer _renderer;

    private StillElement _stillElement;

    private bool _onScreenRenderNow;
    private bool _onScreenRectNow;
    private bool _oldState;

    private H _currentLOD = H.LOD0;

    public OutOfScreen(Person person)
    {
        _type = H.Person;
        _person = person;
        InitPerson();
    }

    public OutOfScreen(Animal animal)
    {
        _type = H.Animal;
        _animal = animal;
        InitAnimal();
    }

    /// <summary>
    /// Says if is on Screen and renderer is active Now
    /// </summary>
    public bool OnScreenRenderNow
    {
        get { return _onScreenRenderNow; }
    }

    /// <summary>
    /// Says if is on the Screen Rect now 
    /// </summary>
    public bool OnScreenRectNow
    {
        get { return _onScreenRectNow; }
    }

    private void InitPerson()
    {
        _animator = _person.gameObject.GetComponent<Animator>();
        _boxCollider = _person.gameObject.GetComponent<BoxCollider>();
        _renderer = _person.Geometry.gameObject.GetComponent<Renderer>();
    }



    private void InitAnimal()
    {
        _animator = _animal.gameObject.GetComponent<Animator>();
        _boxCollider = _animal.gameObject.GetComponent<BoxCollider>();
        _renderer = _animal.Geometry.gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    public void A45msUpdate()
    {
        if (_person == null 
            //|| Program.gameScene.Fustrum1 == null 
            //|| _renderer == null || _person.Body == null
            )
        {
             return;
        }

        _onScreenRectNow = Program.gameScene.Fustrum1.OnScreen(ExtractObjPos());
        //if is moving now can be reshown bz might be on his way somewhere 
        if (_onScreenRectNow &&
            (_renderer.isVisible || _person.Body.MovingNow || _person.Body.Location == HPers.InWork)//inwork is for forester 
            && !_onScreenRenderNow)
        {
            _onScreenRenderNow = true;
            SwitchNow();
        }
        else if (_person != null && 
            (!_onScreenRectNow || !_renderer.isVisible) &&
            _onScreenRenderNow && _person.Body.Location != HPers.InWork)
        {
            _onScreenRenderNow = false;
            //wont deactivate the animator if is on RectNow and close enough to the camera 
            if (_onScreenRectNow && _currentLOD == H.LOD0)
            {
                return;
            }
            SwitchNow();
        }
        HideShow();
	}

    //void UpdateOnObjt()
    //{
    //    if (_type == H.Person)
    //    {
    //        _person.Body.UpdateTheOnScreenRenderNowLocalVar(_onScreenRenderNow);
    //    }
    //}

    Vector3 ExtractObjPos()
    {
        if (_type == H.Person)
        {
            return _person.Body.CurrentPosition;
        }
        return _animator.transform.position;
    }

    public void SetNewLOD(H newLOD)
    {
        _currentLOD = newLOD;
        SwitchNow();
    
    }

    private void SwitchNow()
    {
        if ((_onScreenRenderNow || _onScreenRectNow) && _currentLOD == H.LOD0)
        {
            OnBecameVisible();
        }
        else
        {
            OnBecameInvisible();
        }
        
        HideShow();
    }


    private bool oldScreenRectState;
    /// <summary>
    /// so it gets hidden when camera lets him outOfScreen 
    /// </summary>
    private void HideShow()
    {
        if (OnScreenRectNow == oldScreenRectState)
        {
            return;
        }
        oldScreenRectState = OnScreenRectNow;

        if (OnScreenRenderNow)
        {
            if (_type == H.Person)
            {
               _person.Body.Show();
            }
        }
        else if (!OnScreenRenderNow)
        {
            if (_type == H.Person)
            {
                _person.Body.HideNoQuestion();
            }
        }
    }

    void OnBecameVisible()
    {
        if (_person!=null)
        {
            //Debug.Log("became visible " + _person.MyId);
            _person.Body.EnableAnimator();
        }
        else if (_animal != null)
        {
            _animator.enabled = true;
        }
    }

    void OnBecameInvisible()
    {
        if (_person != null)
        {
            //Debug.Log("became Invisible " + _person.MyId);
            _person.Body.DisAbleAnimator();
        }
        else if(_animal != null)
        {
            _animator.enabled = false;
        }
    }
}
