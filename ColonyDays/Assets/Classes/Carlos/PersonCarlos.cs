using UnityEngine;
using System.Collections;

public enum PersonType
{
  Soldier,
  Civilian,
  Weitress
}


public class PersonCarlos : MonoBehaviour 
{
    public static PersonCarlos Create(PersonType personType)
    {
        PersonCarlos p = null;
        
         switch(personType)
         {
           case PersonType.Soldier:
			p = (PersonCarlos)Resources.Load("Soldier", typeof(PersonCarlos));
			break;
			
			case  PersonType.Civilian:
			p = (PersonCarlos)Resources.Load("Civilian", typeof(PersonCarlos));
			break;
			
			default:
			p = (PersonCarlos)Resources.Load("Civilian", typeof(PersonCarlos));
			break;
			
         }
            
        p = (PersonCarlos)Instantiate(p);
        
        return p;
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	 void Update () {
	  print("Hello from base!!");
	}
}
