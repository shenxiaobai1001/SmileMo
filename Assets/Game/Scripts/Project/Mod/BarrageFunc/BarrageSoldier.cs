using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageSoldier : BarrageFuncBase
{
    public GameObject image;
    private void Awake()
    {
        if (image) image.SetActive(false);
    }
    public override void OnStart(BarrageValue barrageFuncData, int index)//¿ªÊ¼Ö´ÐÐ
    {
        base.OnStart(barrageFuncData, index);
        if (!OnCheckHasLevel())
        {
            PlayerModController.Instance.OnTiggerJingli(true);
        }
        OnShowImage();
        barrageData.BarrageState = BarrageState.Underway;
        Invoke("OnClose",1.1f);
    }

    public void OnShowImage()
    {
        if (image)
        {
            if (image) image.SetActive(true);
            image.transform.localScale = new Vector3(1.3f, 1.3f, 1);
            image.transform.DOScale(1, 0.3f);
        }
        Invoke("OnCloseImage", 1);
    }

    public override void OnClose()
    {
        if (!barrageController.OnCheckHasHighControl()
         && !OnCheckHasLevel()
         && !BarrageFuncController.Instance.OnCheckHighLevelFunc(barrageData.barrageFuncData))
        {
            PlayerModController.Instance.OnTiggerJingli(false);
        }
        if (image) image.SetActive(false);
        barrageData.BarrageState = BarrageState.Finsh;
        SimplePool.Despawn(gameObject);
        base.OnClose();
    }
}
