using UnityEngine;

internal class GOLookAt : General
{
    private bool _isShownNow;
    private Quaternion _targetRot;
    private Quaternion _oldRot;

    public GameObject PrevGO { get; set; }
    public GameObject NextGO { get; set; }

    private void Start()
    {
        Hide();
    }

    private void Update()
    {
        if (_isShownNow)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRot, 0.1f);

            if (PrevGO != null)
            {
                //the middle average point btw ur next and previos link object in the chain
                var movingTo = (PrevGO.transform.position + NextGO.transform.position) / 2;
                transform.position = Vector3.MoveTowards(transform.position, movingTo, .1f);
            }
        }
    }

    public void Hide()
    {
        Geometry.SetActive(false);
        _isShownNow = false;
        //restores ini rota, so it looks cool again whenloads
        transform.rotation = _oldRot;

        //sublte sound when hides
        //AudioCollector.PlayOneShot("ClickMetal1", 15f);
    }

    public void Show(Vector3 lookToPos)
    {
        //sublte sound when shows off
        AudioCollector.PlayOneShot("ClickWood4", 10f);
        _oldRot = transform.rotation;

        transform.LookAt(lookToPos);
        _targetRot = transform.rotation;

        //restores last rota
        transform.rotation = _oldRot;

        Geometry.SetActive(true);
        _isShownNow = true;
    }
}