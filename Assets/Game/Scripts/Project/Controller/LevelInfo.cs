using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LevelInfo : MonoBehaviour
{
    public string levelName;
    public float levelLength;
    public Transform levelTransform; // 直接引用场景中已有的关卡
    [HideInInspector] public int levelIndex; // 关卡的固定索引(0-6)
    public int LeftIndex;
    public float LeftPos;
    public int RightIndex;
    public float RightPos;  
    public List<RedMonsterController> redMonsterControllers = new List<RedMonsterController>();

    public void OnRestAllMonster()
    {
        if (redMonsterControllers == null) return;
        if (redMonsterControllers .Count <= 0) return;
        for (int i = 0; i < redMonsterControllers.Count; i++)
        {
            redMonsterControllers[i].OnRest(null);
        }
    }

    public void OnSetItemMonsterShow(bool show)
    {
        Transform monster = transform.Find("Monster");
        Transform item = transform.Find("Item");

        if (monster != null) 
        { 
             if (monster.childCount >0)
            {
                for (int i = 0; i < monster.childCount; i++)
                {
                    monster.GetChild(i).gameObject.SetActive(show);
                }
            }
        }
        if (item != null)
        {
            if (item.childCount > 0)
            {
                for (int i = 0; i < item.childCount; i++)
                {
                    item.GetChild(i).gameObject.SetActive(show);
                }
            }
        }
    }
}
