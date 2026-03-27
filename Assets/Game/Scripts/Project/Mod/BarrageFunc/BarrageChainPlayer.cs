using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BarrageChainPlayer : BarrageFuncBase
{
    public List<GameObject> gameObjects;
    public GameObject uiCenter;
    public GameObject objCenter;
    public Text tx_number;
    public GameObject uiobj;

    public Animator animator;
    public Transform parent;

    void Awake()
    {
        EventManager.Instance.AddListener(Events.OnLazzerHit, OnLazzerHit);
        EventManager.Instance.AddListener(Events.OnMangSengKick, OnMangSengKick);
    }

    public override void OnStart(BarrageValue barrageFuncData, int index)
    {
        base.OnStart(barrageFuncData, index);
        OnRest(); 

    }
    void OnRest()
    {
        transform.position =  new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        if (animator) animator.gameObject.SetActive(true);
        PlayerController.Instance.transform.position =
          new Vector3(animator.transform.position.x, animator.transform.position.y, animator.transform.position.z);

        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].gameObject.SetActive(false);
        }
        uiCenter.SetActive(true);
        if (!OnCheckHasLevel())
        {
            PlayerModController.Instance.OnChangeState(false, true, false);
        }
        barrageData.BarrageState = BarrageState.Underway;
    }

    void Update()
    {
        switch (barrageData.BarrageState)
        {
            case BarrageState.Ready:
            case BarrageState.Underway:
                if (Config.chainCount <= 0)
                {
                    OnClose();
                    return;
                }
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.K))
                { 
                    Sound.PlaySound("Sound/Mod/paopao");
                    OnRande();
                    Config.chainCount--;
                    if (uiobj) uiobj.transform.DOScale(1.1f, 0.025f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                    {
                        uiobj.transform.localScale = Vector3.one;
                    });
                    OnSnake();
                }
                if (OnCheckHasLevel())
                    OnPause();
                break;
            case BarrageState.Pause:
                if (!OnCheckHasLevel())
                    OnContinue();
                break;
        }

        tx_number.text = $"{Config.chainCount}";
    }

    void OnMangSengKick(object msg)
    {
        OnSnake();
    }
    void OnSnake()
    {
        objCenter.transform.DOShakePosition(0.5f, 0.2f)
                  .SetEase(Ease.OutQuad)
                  .OnComplete(() => {
                      objCenter.transform.localPosition = Vector3.zero;
                  });
    }
    public override void OnPause()
    {
        base.OnPause();
        if (animator) animator.gameObject.SetActive(false);
    }

    public override void OnContinue()
    {
        base.OnContinue();
        transform.position = BarrageFuncCreater.Instance.OnCreatePos("ио╣У");
        OnRest();
    }

    public void OnRande()
    {
        int value = UnityEngine.Random.Range(0, gameObjects.Count);
        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].gameObject.SetActive(value == i);
        }
    }

    void OnLazzerHit(object msg)
    {
        animator.SetTrigger("smallLockLazzer");
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
        base.OnClose();
        SimplePool.Despawn(gameObject);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(Events.OnLazzerHit, OnLazzerHit);
        EventManager.Instance.RemoveListener(Events.OnMangSengKick, OnMangSengKick);
    }
}
