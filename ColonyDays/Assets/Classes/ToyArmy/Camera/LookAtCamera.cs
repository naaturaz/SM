using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public bool InvertY;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        transform.LookAt(Camera.main.transform);

        if (InvertY)
        {
            transform.Rotate(0, 180, 0);//(0, 180, 0)
        }
    }
}