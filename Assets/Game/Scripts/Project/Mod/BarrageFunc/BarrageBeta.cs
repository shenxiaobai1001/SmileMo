using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageBeta : BarrageFuncBase
{
    Transform playerTrans;
    Transform spriteTrans;
    Vector3 moveDir;

    private void Awake()
    {
        moveDir = new Vector3(1, 0.3f)*20;
    }
    public override void OnStart(BarrageValue barrageFuncData, int index)//开始执行
    {
        playerTrans = PlayerController.Instance.transform;
        spriteTrans = PlayerModController.Instance.spriteTrans;
        base.OnStart(barrageFuncData, index);
        if (!OnCheckHasLevel())
        {
            PlayerModController.Instance.OnChangeState(false);
        }
        barrageData.BarrageState = BarrageState.Underway;
  
        Invoke("OnClose",17.5f);
    }

    void Update()
    {
        switch (barrageData.BarrageState)
        {
            case BarrageState.Underway:
                if (playerTrans.position.y < PlayerModController.Instance.playerY)
                {
                    playerTrans.Translate(Vector2.up * 6 * Time.deltaTime);
                }
                playerTrans.Translate(moveDir * Time.deltaTime);
                spriteTrans.Rotate(new Vector3(0, 0, -360) * 5 * Time.deltaTime);
                break;
        }
    }


    public override void OnClose()
    {
        barrageData.BarrageState = BarrageState.Finsh;
        PFunc.Log("大贝塔结束", barrageController.OnCheckHasHighControl(),
           OnCheckHasLevel(), BarrageFuncController.Instance.OnCheckHighLevelFunc(barrageData.barrageFuncData));
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
