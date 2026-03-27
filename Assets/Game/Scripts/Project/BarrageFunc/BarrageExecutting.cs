using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BarrageExecutting 
{
    public static void OnExecutingBarrage(string callName, int index, BarrageValue barrageFuncData)
    {
        switch (callName)
        {
            case "砸鸭子":
                CallManager.Instance.OnCreateDuckVideoPlayer(index);
                break;
            case "左边砸平底锅":
                ItemManager.Instance.OnCreatePDG(callName); 
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "右边砸平底锅":
                ItemManager.Instance.OnCreatePDG(callName); 
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "视角反转":
                ModSystemController.Instance.OnSetRerverseCamera();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "冰冻":
                BarrageFuncCreater.Instance.OnCreateIce(barrageFuncData, index);
                break;
            case "无敌护盾":
                BarrageFuncCreater.Instance.OnCreateShield(barrageFuncData, index);
                break;
            case "左正蹬":
                ItemManager.Instance.OnLeftLegKick();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "右鞭腿":
                ItemManager.Instance.OnRightLegKick();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "麒麟臂":
                MeshCreateController.Instance.OnCreateQLBi();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "天残脚":
                MeshCreateController.Instance.OnCreateTCJiao();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "撞大运":
                MeshCreateController.Instance.OnCreateTrunck();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "莎士比亚":
                ModSystemController.Instance.OnShakespeare();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "大贝塔":
                BarrageFuncCreater.Instance.OnCreateBeta(barrageFuncData, index);
                break;
            case "反向大贝塔":
                BarrageFuncCreater.Instance.OnCreateBetaBack(barrageFuncData, index);
                break;
            case "电击":
                ItemManager.Instance.OnLightningHit();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "彩虹猫":
                ItemManager.Instance.OnRainbowCat();
                break;
            case "番茄连招":
                PlayerModController.Instance.OnClickToCreateTomaTo();
                break;
            case "Boom":
                ItemManager.Instance.OnBoomGrandma();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "呸":
                ItemManager.Instance.OnCreateBlackHand();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "导弹":
                ItemManager.Instance.OnCreateRocket();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "隐身":
                PlayerModController.Instance.OnInvisibility();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "加速":
                PlayerModController.Instance.OnFastSpeed();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "减速":
                PlayerModController.Instance.OnMainSpeed();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "啄木鸟":
                ItemManager.Instance.OnCreateBird();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "砸落头像":
               // ImageDownloader.Instance.OnRoleStar(task.user, task.avatar);
                break;
            case "打台球":
                ItemManager.Instance.OnCreateBilliard();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "大巴掌":
                ItemManager.Instance.OnCreateSlapFace();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "一阳指":
                MeshCreateController.Instance.OnCreateOneFinger();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "乌萨奇":
                ItemManager.Instance.OnCreateWuSaQi();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "传送第七关":
                ModSystemController.Instance.OnTransFarSeven();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "扔香蕉":
                ItemManager.Instance.OnCreateBanana();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "吐口水一":
                ItemManager.Instance.OnCreateTKS("吐口水一");
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "吐口水二":
                ItemManager.Instance.OnCreateTKS("吐口水二");
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "吐口水三":
                ItemManager.Instance.OnCreateTKS("吐口水三");
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "吐口水四":
                ItemManager.Instance.OnCreateTKS("吐口水四");
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "吐口水五":
                ItemManager.Instance.OnCreateTKS("吐口水五");
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "动物怎么叫":
                CallManager.Instance.OnCreateCall();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "火圈":
                ItemManager.Instance.OnCreateHuoquan();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "禁言":
                ItemManager.Instance.OnCreateBannedPost();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "跑快点":
                ItemManager.Instance.OnCreateGoFast();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "退退退":
                ItemManager.Instance.OnCreateGoBack();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "美女盲盒":
                CallManager.Instance.OnCreateVideoPlayer("美女盲盒", 1);
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "动感DJ":
                CallManager.Instance.OnCreateVideoPlayer("动感DJ", 2);
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "上吊":
                BarrageFuncCreater.Instance.OnCreateHangSelf(barrageFuncData, index); 
               // ItemManager.Instance.OnCreateHangSelf();
                break;
            case "加一万米":
                Sound.PlaySound("Sound/Mod/AddTen");
                SystemController.Instance.scheduleDeviation += 10000;
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "减一万米":
                Sound.PlaySound("Sound/Mod/MinTen");
                SystemController.Instance.scheduleDeviation -= 10000;
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "大兵报道":
                BarrageFuncCreater.Instance.OnCreateSoldier(barrageFuncData, index);
                break;
            case "乌鸦坐飞机":
                ItemManager.Instance.OnCreateChenGuoHan();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "灵魂拷问":
                BarrageFuncCreater.Instance.OnCreateMenace(barrageFuncData, index);
                //ModSystemController.Instance.OnPlayMenace();
                break;
            case "埋坟":
                CallManager.Instance.OnKuFen(index);
                break;
            case "万剑齐发":
                ItemManager.Instance.OnCreateManayArrow();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "锁链":
                BarrageFuncCreater.Instance.OnCreateChainPlayer(barrageFuncData, index);
                //ItemManager.Instance.OnCreateChainPlayer();
                break;
            case "一库":
                ItemManager.Instance.OnCreateMangSeng();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "打板子":
                CallManager.Instance.OnCreateFlog(barrageFuncData, index);
                break;
            case "帅哥盲盒":
                CallManager.Instance.OnCreateManVideoPlayer();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
            case "抓（1）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 1, index);
                break;
            case "抓（2）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 2, index);
                break;
            case "抓（11）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 11, index);
                break;
            case "抓（15）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 15, index);
                break;
            case "抓（20）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 20, index);
                break;
            case "抓（40）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 40, index);
                break;
            case "抓（50）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 50, index);
                break;
            case "抓（80）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 80, index);
                break;
            case "抓（100）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 100, index);
                break;
            case "抓（200）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 200, index);
                break;
            case "抓（300）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 300, index);
                break;
            case "抓（500）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 500, index);
                break;
            case "抓（1000）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 1000, index);
                break;
            case "抓（2000）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 2000, index);
                break;
            case "抓（3000）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 3000, index);
                break;
            case "抓（4000）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 4000, index);
                break;
            case "抓（5000）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 5000, index);
                break;
            case "抓（6000）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 6000, index);
                break;
            case "抓（7000）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 7000, index);
                break;
            case "抓（8000）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 8000, index);
                break;
            case "抓（9000）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 9000, index);
                break;
            case "抓（10000）":
                CallManager.Instance.OnCreateDuckVideoPlayer(true, 10000, index);
                break;
            case "没（随机）":
                CallManager.Instance.OnCreateDuckVideoPlayer(false, 1, index);
                break;
            case "跳绳":
                BarrageFuncCreater.Instance.OnCreateRopeSkip(barrageFuncData, index,1);
                break;
            case "跳绳盲盒":
                ModSystemController.Instance.OnCreateRopeVideoPlayer(barrageFuncData, index);
                break;
            case "擦皮鞋":
                BarrageFuncCreater.Instance.OnCreateShoe(barrageFuncData, index);
                break;
            case "顶乌龟":
                BarrageFuncCreater.Instance.OnCreatePeakKuba(barrageFuncData, index);
                break;
            case "补蛋盲盒":
                ModSystemController.Instance.OnCreateAddEggVideoPlayer(barrageFuncData, index);
                break;
            case "减蛋盲盒":
                ModSystemController.Instance.OnCreateMinEggVideoPlayer(barrageFuncData, index);
                break;
            case "打电话":
                ItemManager.Instance.OnCreatePhoneg();
                EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
                break;
        }
    }

    public static void OnCheckBarrageFunc(ActionTask task)
    {
     
    }
}
