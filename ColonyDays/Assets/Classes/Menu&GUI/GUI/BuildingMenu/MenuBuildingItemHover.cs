using UnityEngine;
using UnityEngine.EventSystems;

public class MenuBuildingItemHover : General, IPointerEnterHandler, IPointerExitHandler
{
    private Transform _parent;
    private Transform _grandParent;

    private void Start()
    {
        _parent = gameObject.transform.parent;
        _grandParent = _parent.parent;
    }

    private void Update()
    {
        var pmin = _parent.transform.GetComponent<BoxCollider>().bounds.min;
        var pmax = _parent.transform.GetComponent<BoxCollider>().bounds.max;
        var pDist = Vector3.Distance(pmin, pmax);

        var gpmin = _grandParent.transform.GetComponent<BoxCollider>().bounds.min;
        var gpmax = _grandParent.transform.GetComponent<BoxCollider>().bounds.max;
        var gpDist = Vector3.Distance(gpmin, gpmax);

        if (pDist > gpDist)
            _parent.localScale *= 0.95f;
    }

    private void DestroyHelp()
    {
        Debug.Log("ex");
    }

    private void SpawnHelp()
    {
        Debug.Log("e");
    }

    private void OnMouseDown()
    {
        Debug.Log("down ");
    }

    private void OnMouseEnter()
    {
        Debug.Log("enter");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("enter");

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("ex");

    }
}