using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAirWall : MonoBehaviour
{
    public Slider sl_hp;
    public Text tx_hp;
    public GameObject airwall;
    public GameObject airwallui;
    public GameObject airWallCollier;

    public bool airwalls = false;
    float showTime = 2;
    float useTime = 0;

    void Start()
    {
        EventManager.Instance.AddListener(Events.AirWallStateChange, OnAirWallState); 
        airwall.SetActive(false);
        airWallCollier.SetActive(false);
    }

    void LateUpdate()
    {
        if (!SystemController.Instance.airWallContin) return;
        if (GameController.Instance.gameLevel > 1)
        {
            airwall.SetActive(false);
            airWallCollier.SetActive(false);
            return;
        }
        int maxhp=SystemController.Instance.maxAirWallHp;
        int hp = SystemController.Instance.airWallHp;

        tx_hp.text = $"HP:{hp}/{maxhp}";
        sl_hp.value = (float)hp/ (float)maxhp;
        useTime += Time.deltaTime;
        if (useTime > showTime)
        {
            if (airwallui) airwallui.SetActive(false);
        }
    }

    void OnAirWallState(object msg)
    {
        bool show = (bool)msg;
        airwall.SetActive(show);
        airWallCollier.SetActive(show);
        airwallui.SetActive(show);
        if (show)
        {
            if (tx_hp) tx_hp.transform.DOScale(0.6f, 0.03f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
            {
                tx_hp.transform.localScale = new Vector3(0.5f,0.5f);
            });
        }
        useTime = 0;
    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(Events.AirWallStateChange, OnAirWallState);
    }
}
