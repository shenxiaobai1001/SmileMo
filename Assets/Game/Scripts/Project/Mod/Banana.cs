using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    public PolygonCollider2D polygonCollider2D;

    private void OnEnable()
    {
        if (polygonCollider2D) polygonCollider2D.enabled = false;
        Invoke("OnShowCollider",0.25f);
    }
    void OnShowCollider()
    {
       if(polygonCollider2D) polygonCollider2D.enabled = true;
    }
    private void OnDestroy()
    {
            if (IsInvoking("OnShowCollider"))
        {
            CancelInvoke("OnShowCollider");
        }
    }
}
