using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [Header("πÿø®≈‰÷√")]
    public List<LevelInfo> levels = new List<LevelInfo>();
    private int lastGameLevel = -1;
    private float totalCycleLength;

    private void Start()
    {
        EventManager.Instance.AddListener(Events.MapChanged, OnMapChanged);

    }

    void OnMapChanged(object msg)
    {
        int level = GameController.Instance.gameLevel;
        if (level == 7) return;
        LevelInfo levelInfo = levels[level-1];
        LevelInfo leftInfo = levels[levelInfo.LeftIndex - 1];
        LevelInfo rightInfo = levels[levelInfo.RightIndex - 1];
        float levelInfoX = levelInfo.levelTransform.position.x;
        float leftX = levelInfoX + levelInfo.LeftPos;
        float rightX = levelInfoX + levelInfo.RightPos;
        leftInfo.levelTransform.position = new Vector3(leftX,0);
        rightInfo.levelTransform.position = new Vector3(rightX, 0);
        for (int i = 0; i < levels.Count; i++) {
            int index = i;
            bool show = levels[i] == levels[i] || levels[i] == leftInfo || levels[i] == rightInfo;
            levels[i].gameObject.SetActive(show);
            if (show&& index== level-1) levels[i].OnRestAllMonster();
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(Events.MapChanged, OnMapChanged);
    }
}
