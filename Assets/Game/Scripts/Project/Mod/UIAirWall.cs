using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAirWall : MonoBehaviour
{
    public Slider sl_hp;
    public Text tx_hp;
    public GameObject airwall;
    public GameObject airWallCollier;

    public bool airwalls = true;    

    void Start()
    {
        EventManager.Instance.AddListener(Events.AirWallStateChange, OnAirWallState);
    }

    void Update()
    {
        if (!SystemController.Instance.airWallContin) return;
        if (GameController.Instance.gameLevel != 1)
        {
            airwall.SetActive(false);
            airWallCollier.SetActive(false);
            return;
        }
        int maxhp=SystemController.Instance.maxAirWallHp;
        int hp = SystemController.Instance.airWallHp;

        tx_hp.text = $"HP:{hp}/{maxhp}";
        sl_hp.value = (float)hp/ (float)maxhp;

     
    }

    void OnAirWallState(object msg)
    {
        bool show = (bool)msg;

        airwall.SetActive(show);
        airWallCollier.SetActive(show);
    }
}
