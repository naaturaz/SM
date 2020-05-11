/// <summary>
/// Script Created by Daniel Brookshaw[King Charizard] Copyright 2012
/// This script was created to make the selection square show.
///Please Note this script is not complete or commented so you'll have to figure things out yourself.
/// </summary>

using UnityEngine;

public class UnitSelect : MonoBehaviour
{
    private Vector2 lmbDown;
    private Vector2 lmbUp;
    private Vector3 lmbDownGroundHit;
    private Vector3 startSelection;
    private Vector3 endSelection;
    private float raycastLength = 200f;
    private bool lmbDrag = false;

    public Texture selectionTexture;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            LMBDown(Input.mousePosition);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            LMBUp(Input.mousePosition);
        }
        if (Input.GetButton("Fire1"))
        {
            LMBDownDrag(Input.mousePosition);
        }
    }

    private void LMBDown(Vector2 screenPosition)
    {
        lmbDown = screenPosition;

        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(lmbDown);
        if (Physics.Raycast(ray, out hit, raycastLength))
        {
            if (hit.collider.name == "Floor")
            {
                lmbDownGroundHit = hit.point;
                startSelection = hit.point;
            }
        }
    }

    private void LMBUp(Vector2 screenPosition)
    {
        lmbUp = screenPosition;
        RaycastHit hit;

        lmbDrag = false;
    }

    private void LMBDownDrag(Vector2 screenPosition)
    {
        if (screenPosition != lmbDown)
        {
            lmbDrag = true;
            lmbUp = screenPosition;

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out hit, raycastLength))
            {
                endSelection = hit.point;
            }
        }
    }

    private void OnGUI()
    {
        if (lmbDrag)
        {
            float width = lmbUp.x - lmbDown.x;
            float height = (Screen.height - lmbUp.y) - (Screen.height - lmbDown.y);
            Rect rect = new Rect(lmbDown.x, Screen.height - lmbDown.y, width, height);
            GUI.DrawTexture(rect, selectionTexture, ScaleMode.StretchToFill, true);
        }
    }
}