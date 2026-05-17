using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrageFuncCreater : MonoBehaviour
{
    public static BarrageFuncCreater Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public GameObject barrageIce;
    public GameObject barrageShield;
    public GameObject barrageBeta;
    public GameObject barrageBetaBack;
    public GameObject barrageHangSelf; 
    public GameObject barrageSoldier;
    public GameObject barrageMenace;
    public GameObject barrageChainplayer;
    public GameObject barrageFlogplayer;
    public GameObject barrageRopeSkip;
    public GameObject barrageShoeShine; 
    public GameObject barragePeakKuba;
    public GameObject barrageChickenEgg;
    public void OnCreateIce(BarrageValue barrageFuncData, int index)
    {
        Sound.PlaySound("Sound/Mod/Freeze");
        ModVideoPlayerController.Instance.OnCreateModVideoPlayer(Vector3.zero, Vector3.one, "MOD/dingshen");
        ModData.freezeTime += 1;
        if (BarrageFuncController.Instance.OnCheckBarrageFuncByName("±ù¶³"))
        {
            EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
        }
        else
        {
            InstantiateBarrageFunc(barrageIce, barrageFuncData, index);
        }
    }

    public void OnCreateShield(BarrageValue barrageFuncData, int index)
    {
        ModData.protecketTime += 1;
        if (BarrageFuncController.Instance.OnCheckBarrageFuncByName("ÎÞµÐ»¤¶Ü"))
        {
            EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
        }
        else
        {
            InstantiateBarrageFunc(barrageShield, barrageFuncData, index);
        }
    }
    public void OnCreateChainPlayer(BarrageValue barrageFuncData, int index)
    {
        bool protect = ModSystemController.Instance.Protecket;
        if (!protect)
        {
            Config.chainCount += 20;
        }
        Sound.PlaySound("Sound/Mod/lock");
        if (BarrageFuncController.Instance.OnCheckBarrageFuncByName("ËøÁ´"))
        {
            EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
        }
        else
        {
            InstantiateBarrageFunc(barrageChainplayer, barrageFuncData, index, barrageFuncData.name);
        }
    }

    public void OnCreateFlogPlayer(BarrageValue barrageFuncData, int index)
    {
        if (BarrageFuncController.Instance.OnCheckBarrageFuncByName("´ò°å×Ó"))
        {
            EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
        }
        else
        {
            InstantiateBarrageFunc(barrageFlogplayer, barrageFuncData, index, barrageFuncData.name);
        }
    }

    public void OnCreateRopeSkip(BarrageValue barrageFuncData, int index, int count)
    {
        Config.ropeCount += count;
        if (BarrageFuncController.Instance.OnCheckBarrageFuncByName("ÌøÉþ")
            || BarrageFuncController.Instance.OnCheckBarrageFuncByName("ÌøÉþÃ¤ºÐ"))
        {
            EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
        }
        else
        {
            InstantiateBarrageFunc(barrageRopeSkip, barrageFuncData, index, barrageFuncData.name);
        }
    }
    public void OnCreateEgg(BarrageValue barrageFuncData, int index)
    {

        if (BarrageFuncController.Instance.OnCheckBarrageFuncByName("²¹µ°Ã¤ºÐ"))
        {
            EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
        }
        else
        {
            InstantiateBarrageFunc(barrageChickenEgg, barrageFuncData, index, barrageFuncData.name);
        }
    }
    public void OnCreateShoe(BarrageValue barrageFuncData, int index)
    {
        Sound.PlaySound("Sound/Mod/capixie");
        if (BarrageFuncController.Instance.OnCheckBarrageFuncByName("²ÁÆ¤Ð¬"))
        {
            Config.shineCount += 5;
            EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
        }
        else
        {
            InstantiateBarrageFunc(barrageShoeShine, barrageFuncData, index, barrageFuncData.name);
        }
    }
    public void OnCreatePeakKuba(BarrageValue barrageFuncData, int index)
    {
        Config.kubaCount += 5;
        if (BarrageFuncController.Instance.OnCheckBarrageFuncByName("¶¥ÎÚ¹ê"))
        {
            Sound.PlaySound("smb_1-up");
            EventManager.Instance.SendMessage(Events.OnBarryExecutEnd, index);
        }
        else
        {
            InstantiateBarrageFunc(barragePeakKuba, barrageFuncData, index, barrageFuncData.name);
        }
    }
    public void OnCreateBeta(BarrageValue barrageFuncData, int index)
        => InstantiateBarrageFunc(barrageBeta, barrageFuncData, index);
    public void OnCreateBetaBack(BarrageValue barrageFuncData, int index)
    => InstantiateBarrageFunc(barrageBetaBack, barrageFuncData, index);
    public void OnCreateHangSelf(BarrageValue barrageFuncData, int index)
    => InstantiateBarrageFunc(barrageHangSelf, barrageFuncData, index, barrageFuncData.name);
    public void OnCreateSoldier(BarrageValue barrageFuncData, int index)
    => InstantiateBarrageFunc(barrageSoldier, barrageFuncData, index);
    public void OnCreateMenace(BarrageValue barrageFuncData, int index)
=> InstantiateBarrageFunc(barrageMenace, barrageFuncData, index);

    public GameObject InstantiateBarrageFunc(GameObject prefab, BarrageValue barrageFuncData, int index, string callName = "")
    {
        Vector3 createPos = OnCreatePos(callName);
        PFunc.Log("InstantiateBarrageFunc",createPos, callName);
        GameObject obj = SimplePool.Spawn(prefab, createPos, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.SetActive(true);
        OnCreateEnd(barrageFuncData.name, obj, barrageFuncData, index);
        return obj;
    }

    public Vector3 OnCreatePos(string callName)
    {
        Vector3 createPos = Vector3.zero;
        if (string.IsNullOrEmpty(callName)) return createPos;
        Vector3 vectorPlayer = PlayerController.Instance.transform.position;
        switch (callName)
        {
            case "±ù¶³":
                break;
            case "ÎÞµÐ»¤¶Ü":
                break;
            case "ÉÏµõ":
                 createPos = new Vector3(vectorPlayer.x, 0);
                break;
            case "ËøÁ´":
                createPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
                break;
            case "´ò°å×Ó":
                createPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
                break;
            case "ÌøÉþ":
            case "ÌøÉþÃ¤ºÐ":
                createPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
                break;
            case "²ÁÆ¤Ð¬":
                createPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
                break;
            case "¶¥ÎÚ¹ê":
                createPos = new Vector3(vectorPlayer.x, 4);
                break;
            case "²¹µ°Ã¤ºÐ":
                createPos = vectorPlayer;
                break;
        }
        return createPos;
    }

    public void OnCreateEnd(string callName, GameObject obj, BarrageValue barrageFuncData, int index)
    {
        switch (callName)
        {
            case "±ù¶³":
                obj.GetComponent<IceContro>().OnStart(barrageFuncData, index);
                break;
            case "ÎÞµÐ»¤¶Ü":
                obj.GetComponent<BarrageShield>().OnStart(barrageFuncData, index);
                break;
            case "´ó±´Ëþ":
                ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(0, 1), new Vector3(0.6f, 0.6f), "MOD/dabeita");
                obj.GetComponent<BarrageBeta>().OnStart(barrageFuncData, index);
                break;
            case "·´Ïò´ó±´Ëþ":
                ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(0, 1), new Vector3(0.6f, 0.6f), "MOD/dabeita");
                obj.GetComponent<BarrageBetaBack>().OnStart(barrageFuncData, index);
                break;
            case "ÉÏµõ":
                Sound.PlaySound("Sound/Mod/hangself");
                obj.GetComponent<BarrageHangSelf>().OnStart(barrageFuncData, index);
                break;
            case "´ó±ø±¨µÀ":
                obj.GetComponent<BarrageSoldier>().OnStart(barrageFuncData, index);
                break;
            case "Áé»ê¿½ÎÊ":
                obj.GetComponent<BarrageMenace>().OnStart(barrageFuncData, index);
                break;
            case "ËøÁ´":
                obj.GetComponent<BarrageChainPlayer>().OnStart(barrageFuncData, index);
                break;
            case "´ò°å×Ó":
                obj.GetComponent<BarrageFlogPlayer>().OnStart(barrageFuncData, index);
                break;
            case "ÌøÉþ":
            case "ÌøÉþÃ¤ºÐ":
                obj.GetComponent<BarrageRopeSkip>().OnStart(barrageFuncData, index);
                break;
            case "²ÁÆ¤Ð¬":
                Config.shineCount += 5;
                obj.GetComponent<BarrageShoeShine>().OnStart(barrageFuncData, index);
                break;
            case "¶¥ÎÚ¹ê":
                obj.GetComponent<BarragePeakKuba>().OnStart(barrageFuncData, index);
                break;
            case "²¹µ°Ã¤ºÐ":
                obj.GetComponent<BarrageCreateEgg>().OnStart(barrageFuncData, index);
                break;
        }
    }
}
