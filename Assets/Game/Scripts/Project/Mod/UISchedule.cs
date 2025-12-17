using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISchedule : MonoBehaviour
{
    public Text tx_mpos;
    void Update()
    {
        if (PlayerController.Instance == null) return;

        int value = (int)PlayerController.Instance.transform.position.x;
        if (tx_mpos) tx_mpos.text = $"{value}รื";
    }
}
