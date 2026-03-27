using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageMenace : BarrageFuncBase
{
    public override void OnStart(BarrageValue barrageFuncData, int index)
    {
        base.OnStart(barrageFuncData, index);
        int number = Random.Range(1, 39);
        ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(-0.5f, 0.4f, 0), 
            Vector3.one, $"MOD/Question/{number}", "Video", false);
        if (!OnCheckHasLevel())
        {
            PlayerModController.Instance.OnChangeState(false, false, true,true);
        }
        OnEnterResult();
    }

    public override void OnEnterResult()
    {
        Invoke("OnClose", 1.5f);
    }

    public override void OnClose()
    {
        barrageData.BarrageState = BarrageState.Finsh;
        if (!barrageController.OnCheckHasHighControl()
           && !OnCheckHasLevel()
           && !BarrageFuncController.Instance.OnCheckHighLevelFunc(barrageData.barrageFuncData))
        {
            PlayerModController.Instance.OnSetModSprite(false);
        }
        base.OnClose();
        CancelInvoke();
        SimplePool.Despawn(gameObject);
    }
}
