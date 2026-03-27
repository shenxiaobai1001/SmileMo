using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceContro : BarrageFuncBase
{
    public override void OnStart(BarrageValue barrageFuncData, int index)//¿ªÊ¼Ö´ÐÐ
    {
       base.OnStart(barrageFuncData, index);
        PlayerController.Instance.isHit = true;
        barrageData.BarrageState = BarrageState.Ready;
    }

    // Update is called once per frame
    void Update()
    {
        switch (barrageData.BarrageState)
        {
            case BarrageState.Tigger:
                break;
            case BarrageState.Ready:
            case BarrageState.Underway:
                ModData.freezeTime-=Time.deltaTime;
                if (ModData.freezeTime <= 0)
                { 
                    OnClose(); 
                } 
                transform.position = PlayerController.Instance.transform.position;
                break;
            case BarrageState.Pause:
                break;
            case BarrageState.Finsh:
                break;
            case BarrageState.Close:
                break;
        }
    }

    public override void OnClose()
    {
        barrageData.BarrageState = BarrageState.Finsh;
        if (!barrageController.OnCheckHasHighControl()
        && !OnCheckHasLevel()
        && !BarrageFuncController.Instance.OnCheckHighLevelFunc(barrageData.barrageFuncData))
        {
            PlayerController.Instance.isHit = false;
        }
        SimplePool.Despawn(gameObject);
        base.OnClose();
    }
}
