using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakKuba : MonoBehaviour
{
    public Transform kubaPos;
    public Transform kubaCreateos;
    public GameObject KUBA;
    public Transform uiNumber;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.tag.Contains("Player"))
        {
            Config.kubaCount--;
            Config.hasKubaCount++;
            if (uiNumber) uiNumber.transform.DOScale(1.1f, 0.025f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
            {
                uiNumber.transform.localScale = Vector3.one;
            });
            Sound.PlaySound("smb_1-up");
            kubaPos.DOShakePosition(0.2f, 0.1f).onComplete+=()=>{ kubaPos.localPosition = Vector3.zero; } ;
            GameObject kuba = SimplePool.Spawn(KUBA, kubaCreateos.position,Quaternion.identity);
            kuba.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 8), ForceMode2D.Impulse);
            kuba.GetComponent<GreenTurtle>().OnStart();
        }
    }
}