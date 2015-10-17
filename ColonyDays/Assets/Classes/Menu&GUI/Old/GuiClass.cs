using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuiClass : General {

    private float _fadeSpeedProp;

    public float FadeSpeedProp
    {
        get { return _fadeSpeedProp; }
        set { _fadeSpeedProp = value; }
    }

    //they will hold the materials the object had in the beggining however when u use it 
    //have to be used in the same order they are in the inspector in the renderer area 
    public List<Material> materiales = new List<Material>();

	// Use this for initialization
	public void Start () 
    {

        //will address if the object has more that one material in the renderer 
        if (GetComponent<Renderer>() != null)
        {
            //if more thn material is assigned inthe renderer and public materiales is empty 
            if (GetComponent<Renderer>().materials.Length > 1 && materiales.Count == 0)
            {
                for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++)
                {
                    materiales.Add(GetComponent<Renderer>().materials[i]);
                }
            }
        }

        if(FadeSpeedProp == 0)
        { FadeSpeedProp = 25f; }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
