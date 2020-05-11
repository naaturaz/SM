using UnityEngine;

public class Main : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        Soldier s = (Soldier)PersonCarlos.Create(PersonType.Soldier);
        Civilian c = (Civilian)PersonCarlos.Create(PersonType.Civilian);

        Civilian dd = (Civilian)Civilian.Create(PersonType.Civilian);

        Soldier s2 = (Soldier)PersonCarlos.Create(PersonType.Soldier);

        //Person s1 = Person.Create(PersonType.Soldier);
        ///Person c1 = Person.Create(PersonType.Civilian);

        PersonCarlos[] persons = new PersonCarlos[] { s, c, dd, s2 };

        for (int i = 0; i < persons.Length; i++)
        {
            //persons[i].Run();
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}