using System.Collections.Generic;
using UnityEngine;

public class UMove : MonoBehaviour
{
    public static Vector3 MoveSmooth(Transform currentTransf, Vector3 targetPos, float smoothTimePass, ref Vector3 velocity)
    {
        Vector3 newPos = Vector3.zero;

        newPos.x = Mathf.SmoothDamp(currentTransf.position.x, targetPos.x, ref velocity.x, smoothTimePass);
        newPos.y = Mathf.SmoothDamp(currentTransf.position.y, targetPos.y, ref velocity.y, smoothTimePass);
        newPos.z = Mathf.SmoothDamp(currentTransf.position.z, targetPos.z, ref velocity.z, smoothTimePass);

        Vector3 newPosNow = new Vector3(newPos.x, newPos.y, newPos.z);
        currentTransf.position = Vector3.Slerp(currentTransf.position, newPosNow, Time.time);

        return currentTransf.position;
    }

    /// <summary>
    /// If a point is close enough less thann minDiff will be pushed in the Axis
    /// </summary>
    public static List<Vector3> PushPointFromOrderedList(List<Vector3> list, float minDiff, H axisToBePushed, float howFarShouldBePush)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            float distance = Vector3.Distance(list[i], list[i + 1]);
            if (distance < minDiff)
            {
                if (axisToBePushed == H.Y)
                {
                    Vector3 temp = list[i];
                    temp.y += howFarShouldBePush;
                    list[i] = temp;
                }
            }
        }
        return list;
    }

    /// <summary>
    /// Will push all the points on a list toward the param
    /// </summary>
    public static List<Vector3> PushAllPointFromOrderedList(List<Vector3> list, H axisToBePushed, float howFarShouldBePush)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (axisToBePushed == H.Y)
            {
                Vector3 temp = list[i];
                temp.y += howFarShouldBePush;
                list[i] = temp;
            }
        }
        return list;
    }

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}