using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrageCreateEgg : BarrageFuncBase
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public Text tx_allEgg;
    public Text tx_hasgg;
    public Rigidbody2D rb;
    public GameObject eggObj;
    public Transform eggPos;
    public Transform uiObj;

    public override void OnStart(BarrageValue barrageFuncData, int index)//开始执行
    {
        base.OnStart(barrageFuncData, index);
        if (!OnCheckHasLevel())
        {
            PlayerModController.Instance.OnChangeState(false, false, false);
        }
        barrageData.BarrageState = BarrageState.Underway;
    }

    void Update()
    {
        switch (barrageData.BarrageState)
        {
            case BarrageState.Ready:
            case BarrageState.Underway:
                if (Config.eggCount <= 0)
                {
                    Config.eggCount = 0;
                    OnClose();
                    return;
                }
                if (transform.position.y>4)
                {
                    transform.position = new Vector3(transform.position.x, 4);
                }
                PlayerController.Instance.transform.position=transform.position;

                if (Input.GetKey(KeyCode.A))
                {
                    animator.SetBool("Move", true);
                    transform.Translate(Vector3.left * 3 * Time.deltaTime);
                    transform.localScale = new Vector3(-1, 1);
                    uiObj.localEulerAngles = new Vector3(0, 180);
                }
            
                if (Input.GetKey(KeyCode.D))
                {
                    animator.SetBool("Move", true);
                    transform.Translate(Vector3.right * 3 * Time.deltaTime);
                    transform.localScale = new Vector3(1, 1);
                    uiObj.localEulerAngles = new Vector3(0, 0);
                }

                if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                {
                    animator.SetBool("Move", false);
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetBool("Egg",true);
                }
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    animator.SetBool("Egg", false);
                }

                if (OnCheckHasLevel())
                    OnPause();
                break;
            case BarrageState.Pause:
                if (!OnCheckHasLevel())
                    OnContinue();
                break;
        }

        tx_allEgg.text = $"待下蛋：{Config.eggCount}";

        tx_hasgg.text = $"已下蛋：{Config.hasEggCount}";
    }

    void OnCreateEgg()
    {
        OnCreateEggG();
        OnCreateEggG();
    }
    void OnCreateEggG()
    {
        Config.eggCount--;
        Config.hasEggCount++;
        Sound.PlaySound("Sound/Mod/Egg");
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector3(0, 2), ForceMode2D.Impulse);
        GameObject egg = SimplePool.Spawn(eggObj, eggPos.position, Quaternion.identity);
        egg.transform.SetParent(MeshCreateController.Instance.transform);
        egg.SetActive(true);
        if (tx_hasgg) tx_hasgg.transform.DOScale(1.1f, 0.025f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            tx_hasgg.transform.localScale = Vector3.one;
        });
    }

    public override void OnPause()
    {
        base.OnPause();
        animator.SetBool("Egg", false);
        spriteRenderer.enabled = false;
        uiObj.gameObject.SetActive(false);
    }

    public override void OnContinue()
    {
        base.OnContinue();
        spriteRenderer.enabled = true;
        uiObj.gameObject.SetActive(true);
        if (!OnCheckHasLevel())
        {
            PlayerModController.Instance.OnChangeState(false, false, false);
        }
    }

    public override void OnClose()
    {
        barrageData.BarrageState = BarrageState.Finsh;
        if (!barrageController.OnCheckHasHighControl()
         && !OnCheckHasLevel()
         && !BarrageFuncController.Instance.OnCheckHighLevelFunc(barrageData.barrageFuncData))
        {
            PlayerModController.Instance.OnChangeState(true);
        }

        SimplePool.Despawn(gameObject);
        base.OnClose();
    }
}
