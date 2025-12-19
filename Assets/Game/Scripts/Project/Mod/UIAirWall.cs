using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAirWall : MonoBehaviour
{
    public Slider sl_hp;
    public Text tx_hp;

    void Start()
    {
        
    }

    void Update()
    {
        int maxhp=SystemController.Instance.maxAirWallHp;
        int hp = SystemController.Instance.airWallHp;

        tx_hp.text = $"HP:{hp}/{maxhp}";
        sl_hp.value = (float)hp/ (float)maxhp;
    }
}
