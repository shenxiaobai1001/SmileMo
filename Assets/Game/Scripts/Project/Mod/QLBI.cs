using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerModController;

public class QLBI : MonoBehaviour
{
    public Transform boomPos;
    public GameObject boom;

    Vector3 startPos;
    float allTime = 1.5f;
    float time = 0;
    float boomTime = 0;
    void Awake()
    {
        startPos = new Vector3(-15,0);
    }

    public void OnStarMove()
    {
        boomTime = 0;
        time = 0;
        transform.localPosition = startPos;
        transform.DOLocalMove(Vector3.zero, 0.2f).OnComplete(() => { OnBeginCreateBoom(); });
    }

    void OnBeginCreateBoom()
    {
        PlayerModController.Instance.TriggerModMove(MoveDirection.Right, allTime, 0, MoveType.QL);
        StartCoroutine(OnCreateBoom());
    }

    IEnumerator OnCreateBoom()
    {
        while (time <= allTime)
        { 
            time += Time.deltaTime;
            boomTime += Time.deltaTime;
            if (boomTime > 0.15f) {
                Sound.PlaySound("Sound/Mod/Boom");
                GameObject bb = SimplePool.Spawn(boom, boomPos.transform.position, Quaternion.identity);
                bb.transform.parent = ItemManager.Instance.transform;
                Camera.main.DOShakePosition(0.1f, new Vector3(0, 1.2f, 0), 0, 0f, false);
                bb.SetActive(true);
                boomTime = 0;
            }
            yield return null;
        }
        PFunc.Log("OnCreateBoom", time);
        if (IsInvoking("OnBeginCreateBoom"))
        {
            CancelInvoke("OnBeginCreateBoom");
        }
        if (IsInvoking("OnCreateBoom"))
        {
            CancelInvoke("OnCreateBoom");
        }
        SimplePool.Despawn(this.gameObject);
    }
    private void OnDestroy()
    {
        if (IsInvoking("OnBeginCreateBoom"))
        {
            CancelInvoke("OnBeginCreateBoom");
        }
        if (IsInvoking("OnCreateBoom"))
        {
            CancelInvoke("OnCreateBoom");
        }
    }
}
