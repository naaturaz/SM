using UnityEngine;
using System.Collections;

public class MouseAimCamera : MonoBehaviour
{
    GameObject target;
    public float rotateSpeed = 5;
    Vector3 offset;

    void Start()
    {
        if (target != null)
        {
            offset = target.transform.position - transform.position;
        }
        //target = SmoothFollow.TARGET.gameObject;
    }

    void LateUpdate()
    {
        target = SmoothFollow.TARGET.gameObject;

        if (target != null)
        {
            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            target.transform.Rotate(0, horizontal, 0);

            float desiredAngle = target.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
            transform.position = target.transform.position - (rotation * offset);

            transform.LookAt(target.transform);
        }
    }
}