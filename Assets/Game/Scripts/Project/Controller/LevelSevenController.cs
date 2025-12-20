using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSevenController : MonoBehaviour
{
    public static LevelSevenController Instance { get; private set; }
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

    public List<GameObject> Monsters;
    public Transform startPos;
    public LevelInfo levelInfo;
    public bool InSeven = false;
    public int targetKillMonster=50;
    public int killMonster = 0;

    Vector3 outTrans = new Vector3(1132,144);
    Vector3 outTrans2 = new Vector3(1639, 144);

    void Start()
    {
        EventManager.Instance.AddListener(Events.GameRest, GameRest);
    }

    private void Update()
    {
        if (!InSeven) return;

        Vector3 playerPos = PlayerController.Instance.transform.position;
        if (playerPos.x < outTrans.x)
        {
            OnOutPlayerSevenFail();
        }
        if (playerPos.x > outTrans2.x) 
        {
            OnOutPlayerSevenSucc();
        }
    }
    void GameRest(object msg)
    {
        if (Monsters == null || Monsters.Count < 0) return;
        for (int i = 0; i < Monsters.Count; i++) {
            Monsters[i].gameObject.SetActive(true);
            MonsterBase monsterBase = Monsters[i].GetComponent<MonsterBase>();
            if (monsterBase != null) {
                monsterBase.OnRest();
            }
        }
    }

    public void OnKillMonster()
    {
        killMonster++;
        EventManager.Instance.SendMessage(Events.SevenMonsterDie);
        if (killMonster >= targetKillMonster) {
            OnOutPlayerSevenSucc();
        }
    }

    public void OnSetPlayerSeven()
    {
        GameController.Instance.isAutomatic = false;
        PlayerAutomaticSystem.Instance.OnStopAutomatic();
        GameController.Instance.OnSetGameLevel(7);
        PlayerController.Instance.transform.position = startPos.position;
        InSeven = true;
        CatFowlloer.Instance.GameRest();
        GameRest(null);
        UISevenSchedule.Instance.OnRestSchedule();
        killMonster=0;
    }

    public void OnOutPlayerSevenFail()
    {
        InSeven = false;
        CatFowlloer.Instance.isFollow = false;
        ModSystemController.Instance.OnTransFarSevenOut();
        UISevenSchedule.Instance.OnCloseSchedule();
        killMonster = 0;
        if (SystemController.Instance.airWallContin)
            GameController.Instance.PlayerRestToSavePos(null);
        else
            GameController.Instance.OnMoveToOldSavePos();
    }

    public void OnOutPlayerSevenSucc()
    {
        InSeven = false;
        CatFowlloer.Instance.isFollow = false;
        ModSystemController.Instance.OnTransFarSevenOut();
        UISevenSchedule.Instance.OnCloseSchedule();
        killMonster = 0;
        GameController.Instance.OnMoveToNextSavePos();
    }
}
