using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakKubaFllow : MonoBehaviour
{
    private void OnEnable()
    {
        transform.position = new Vector3(0, 15);
        oldPos = transform.position;
    }

    [Header("跟随目标")]
    public Transform target; // 要跟随的目标

    [Header("偏移量设置")]
    public Vector3 offset; // 可自定义的跟随偏移量

    [Header("Y轴跟随限制")]
    public float minY = -10f; // Y轴下降的最低限度

    Vector3 oldPos;
    bool isRest = false;
    private void LateUpdate()
    {
        if (target == null)
        {
            if (PlayerController.Instance != null)
            {
                target = PlayerController.Instance.transform;

                transform.position = new Vector3(0, 15);
                oldPos = transform.position;
                isRest = true;
            }
        }
        //if (GameStatusController.isDead)
        //{
        //    transform.position = new Vector3(0, 15);
        //    oldPos = transform.position;
        //    return;
        //}
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        float newX = targetPosition.x;

        if (target.position.y > minY)
        {
            transform.position = new Vector3(transform.position.x, targetPosition.y);
            return;
        }
        else
        {
            transform.position = new Vector3(newX, minY, transform.position.z);
        }


        if (transform.position.y > oldPos.y)
        {
            Vector3 vec = new Vector3(newX, oldPos.y, targetPosition.z);
            transform.position = vec;
        }
        else
            oldPos = transform.position;
    }
}
