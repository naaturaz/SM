using UnityEngine;
using System.Collections;

//collidaer utility
public class UColi : MonoBehaviour {

    public static void SetColiState(Transform[] array, H action)
    {
        bool state = false;
        if (action == H.Enable) state = true;

        for (int i = 0; i < array.Length; i++)
        {
            array[i].gameObject.GetComponent<Collider>().enabled = state;
        }
    }
}
