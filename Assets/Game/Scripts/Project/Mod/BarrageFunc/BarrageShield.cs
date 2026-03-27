using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageShield : BarrageFuncBase
{
    public override void OnStart(BarrageValue barrageFuncData, int index)//¿ªÊ¼Ö´ÐÐ
    {
        ModSystemController.Instance.Protecket = true;
        base.OnStart(barrageFuncData, index);
        if (!OnCheckHasLevel())
        {
            PlayerModController.Instance.OnChangeState(true);
        }
        barrageData.BarrageState = BarrageState.Underway;
    }

    // Update is called once per frame
    void Update()
    {
        switch (barrageData.BarrageState)
        {
            case BarrageState.Underway:
                ModData.protecketTime -= Time.deltaTime;
                if (ModData.protecketTime <= 0)
                {
                    OnClose();
                }
                transform.position = PlayerController.Instance.transform.position;
                break;
        }
    }

    public override void OnClose()
    {
        if (!barrageController.OnCheckHasHighControl()
         && !OnCheckHasLevel()
         && !BarrageFuncController.Instance.OnCheckHighLevelFunc(barrageData.barrageFuncData))
        {
            PlayerModController.Instance.OnChangeState(true);
        }
        ModSystemController.Instance.Protecket = false;
        barrageData.BarrageState = BarrageState.Finsh;
        SimplePool.Despawn(gameObject);
        base.OnClose();
    }
}
