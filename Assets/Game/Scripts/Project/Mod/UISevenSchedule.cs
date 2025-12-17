using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISevenSchedule : MonoBehaviour
{
    public static UISevenSchedule Instance { get; private set; }
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

    public GameObject Center;
    public Text tx_schedule;
    LevelSevenController levelSeven ;
    private void Start()
    {
        levelSeven = LevelSevenController.Instance;
        Center.SetActive(false);
        EventManager.Instance.AddListener(Events.SevenMonsterDie, OnRefreshSchedule);
    }
    public void OnRestSchedule()
    {
        Center.SetActive(true);
        if(tx_schedule) tx_schedule.text = $"{levelSeven.killMonster}/{levelSeven.targetKillMonster}";
    }
    public void OnCloseSchedule()
    {
        Center.SetActive(false);
    }
    public void OnRefreshSchedule(object msg)
    {
        if (tx_schedule) tx_schedule.text = $"{levelSeven.killMonster}/{levelSeven.targetKillMonster}";
    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(Events.SevenMonsterDie, OnRefreshSchedule);
    }
}
