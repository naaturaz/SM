﻿using UnityEngine;

public class MouseOrbit : MonoBehaviour
{
    public Transform Target;
    public float Distance = 5.0f;
    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -20.0f;
    public float yMaxLimit = 80.0f;

    private float x;
    private float y;

    private void Awake()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;

        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    private void LateUpdate()
    {
        Awake();

        if (GameObject.FindGameObjectWithTag("Player") != null && Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        }

        //if target not null and we hold left mouse click down
        if (Target != null && Input.GetMouseButton(2))
        {
            x += (float)(Input.GetAxis("Mouse X") * xSpeed * 0.02f);
            y -= (float)(Input.GetAxis("Mouse Y") * ySpeed * 0.02f);

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * (new Vector3(0.0f, 0.0f, -Distance)) + Target.position;
            //Vector3 position = rotation * transform.position + Target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }
}