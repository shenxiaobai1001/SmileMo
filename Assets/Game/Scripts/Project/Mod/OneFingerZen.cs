using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerModController;

public class OneFingerZen : MonoBehaviour
{
    public Transform boomPos;
    public GameObject boom;

    Vector3 startPos;
    float allTime = 2.5f;
    float time = 0;
    float boomTime = 0;
    void Awake()
    {
        startPos = new Vector3(20, 0);
    }
    public void OnReadyStarMove()
    {
        boomTime = 0;
        time = 0;
        transform.localPosition = startPos;
        transform.DOLocalMove(Vector3.zero, 0.5f).OnComplete(() => { OnBeginCreateBoom(); });
    }
    void OnBeginCreateBoom()
    {
        PlayerModController.Instance.TriggerModMove(MoveDirection.Left, allTime, 0, MoveType.TC);
        CameraShaker.Instance.StartShake(2);
        StartCoroutine(OnCreateBoom());
    }
    IEnumerator OnCreateBoom()
    {
        while (time <= allTime)
        {
            time += Time.deltaTime;
            boomTime += Time.deltaTime;
            if (boomTime > 0.15f)
            {
                Sound.PlaySound("Sound/Mod/Boom");
                GameObject bb = SimplePool.Spawn(boom, boomPos.transform.position, Quaternion.identity);
                bb.transform.parent = ItemManager.Instance.transform;
                bb.SetActive(true);
                boomTime = 0;
            }
            yield return null;
        }
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
