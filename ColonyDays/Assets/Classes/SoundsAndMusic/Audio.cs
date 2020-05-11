using UnityEngine;

public class Audio : General
{
    public bool isToFollowCamera = true;
    private CamControl mainCamera;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    new internal void Update()
    {
        if (isToFollowCamera)
        {
            if (Camera.main != null)
            {
                transform.position = Camera.main.transform.position;
            }
            else
            {
                mainCamera = USearch.FindCurrentCamera();
                transform.position = mainCamera.transform.position;
            }
        }
    }
}