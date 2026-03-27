using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrageShoeShine : BarrageFuncBase
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameObject center;
    public Text tx_number;

    string aniType = "";
    public override void OnStart(BarrageValue barrageFuncData, int index)//开始执行
    {
        base.OnStart(barrageFuncData, index);
        OnRest();
        barrageData.BarrageState = BarrageState.Underway;
    }

    private void Update()
    {
        switch (barrageData.BarrageState)
        {
            case BarrageState.Ready:
            case BarrageState.Underway:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.K))
                {
                    Sound.PlaySound("Sound/Mod/ca");
                    if (animator != null) animator.SetTrigger("shoe");
                    Config.shineCount--;
                    if (tx_number) tx_number.transform.DOScale(1.1f, 0.025f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                    {
                        tx_number.transform.localScale = Vector3.one;
                    });
                    if (Config.shineCount <= 0)
                    {
                        OnClose();
                        return;
                    }
                }
                if (OnCheckHasLevel())
                    OnPause();
                break;
            case BarrageState.Pause:
                if (!OnCheckHasLevel())
                    OnContinue();
                break;
        }

        if (tx_number) tx_number.text = $"剩余次数{Config.shineCount}";
    }

    public override void OnPause()
    {
        base.OnPause();
        spriteRenderer.enabled = false;
        center.SetActive(false);
    }
    public override void OnContinue()
    {
        base.OnContinue();
        transform.position = BarrageFuncCreater.Instance.OnCreatePos("擦皮鞋");
        spriteRenderer.enabled = true;
        center.SetActive(true);
        OnRest();
    }

    public override void OnEnterResult() { }

    public void OnRest()
    {
        if (!OnCheckHasLevel())
        {
            PlayerModController.Instance.OnChangeState(false,true,false);
        }

        spriteRenderer.enabled = true;
        center.SetActive(true);
        PlayerController.Instance.transform.position = new Vector3(animator.transform.position.x, animator.transform.position.y, animator.transform.position.z);

        if (animator != null) animator.SetTrigger("Idle");
    }

    public override void OnClose()//执行完毕
    {
        barrageData.BarrageState = BarrageState.Finsh;
        base.OnClose();
        if (!barrageController.OnCheckHasHighControl()
           && !OnCheckHasLevel()
           && !BarrageFuncController.Instance.OnCheckHighLevelFunc(barrageData.barrageFuncData))
        {
            PlayerModController.Instance.OnChangeState(true);
        }
        SimplePool.Despawn(gameObject);
    }
}
