using UnityEngine;
using System.Collections;

public class AnimateTexture : MonoBehaviour
{

    public float offSetX;
    public float offSetY;

    private Material _material;

	// Use this for initialization
	void Start ()
	{
	    _material = gameObject.GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update ()
	{
        Vector2 newV = new Vector2(_material.mainTextureOffset.x + offSetX, _material.mainTextureOffset.y + offSetY);

	    _material.mainTextureOffset = newV;
	}
}
