using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BarrageData
{
    public string Type;
    public string name;
    public string message;
    public string userAvatar;
    public int num; // 礼物数量
    public int count; // 点赞数量
}

public class BarrageBase : MonoBehaviour
{
    public Dictionary<string, int> likeCount = new Dictionary<string, int>();


    public void HandleAttention(string json)
    {
        BarrageData info = JsonUtility.FromJson<BarrageData>(json);
        string user = info.name;
        string avatar = info.userAvatar;

        BarrageController barrageConfigs = FindAnyObjectByType<BarrageController>();
        if (barrageConfigs == null)
        {
            Debug.LogWarning("未找到 BarrageController 配置，忽略关注事件");
            return;
        }
        // 普通触发逻辑
        foreach (BarrageNormalSetting config in barrageConfigs.barrageNormalSetting)
        {
            if (config.Type == "关注")
            {
                barrageConfigs.EnqueueAction(user, avatar, config.CallName, 1, config.Count, config.Delay);
            }
        }

        // 盲盒触发逻辑
        foreach (BarrageBoxSetting config in barrageConfigs.barrageBoxSetting)
        {
            if (config.Type == "关注")
            {
                int index = UnityEngine.Random.Range(0, config.Calls.Count);
                string CallName = config.Calls[index];
                barrageConfigs.EnqueueAction(user, avatar, CallName, 1, config.Count, config.Delay);
            }
        }
    }

    public void HandleBarrage(string json)
    {
        BarrageData info = JsonUtility.FromJson<BarrageData>(json);
        string user = info.name;
        string avatar = info.userAvatar;
        string content = info.message;

        BarrageController barrageConfigs = FindAnyObjectByType<BarrageController>();
        if (barrageConfigs == null)
        {
            Debug.LogWarning("未找到 BarrageController 配置，忽略弹幕事件");
            return;
        }
        // 普通触发逻辑
        foreach (BarrageNormalSetting config in barrageConfigs.barrageNormalSetting)
        {
            if (config.Type == "弹幕" && config.Message == content)
            {
                barrageConfigs.EnqueueAction(user, avatar, config.CallName, 1, config.Count, config.Delay);
            }
        }

        // 盲盒触发逻辑
        foreach (BarrageBoxSetting config in barrageConfigs.barrageBoxSetting)
        {
            if (config.Type == "弹幕" && config.Message == content)
            {
                int index = UnityEngine.Random.Range(0, config.Calls.Count);
                string CallName = config.Calls[index];
                barrageConfigs.EnqueueAction(user, avatar, CallName, 1, config.Count, config.Delay);
            }
        }
    }

    public void HandleGift(string json)
    {
        BarrageData info = JsonUtility.FromJson<BarrageData>(json);
        string giftName = info.message;
        string user = info.name;
        string avatar = info.userAvatar;
        int giftCount = info.num;

        BarrageController barrageConfigs = FindAnyObjectByType<BarrageController>();
        if (barrageConfigs == null)
        {
            Debug.LogWarning("未找到 BarrageController 配置，忽略礼物事件");
            return;
        }
        // 普通触发逻辑
        foreach (BarrageNormalSetting config in barrageConfigs.barrageNormalSetting)
        {
            if (config.Type == "礼物" && config.Message == giftName)
            {
                barrageConfigs.EnqueueAction(user, avatar, config.CallName, giftCount, config.Count, config.Delay);
            }
        }

        // 盲盒触发逻辑
        foreach (BarrageBoxSetting config in barrageConfigs.barrageBoxSetting)
        {
            if (config.Type == "礼物" && config.Message == giftName)
            {
                int index = UnityEngine.Random.Range(0, config.Calls.Count);
                string CallName = config.Calls[index];
                barrageConfigs.EnqueueAction(user, avatar, CallName, giftCount, config.Count, config.Delay);
            }
        }
    }

    public void HandleJoin(string json)
    {
        BarrageData info = JsonUtility.FromJson<BarrageData>(json);
        string user = info.name;
        string avatar = info.userAvatar;

        BarrageController barrageConfigs = FindAnyObjectByType<BarrageController>();
        if (barrageConfigs == null)
        {
            Debug.LogWarning("未找到 BarrageController 配置，忽略进入事件");
            return;
        }
        // 普通触发逻辑
        foreach (BarrageNormalSetting config in barrageConfigs.barrageNormalSetting)
        {
            if (config.Type == "进入")
            {
                barrageConfigs.EnqueueAction(user, avatar, config.CallName, 1, config.Count, config.Delay);
            }
        }

        // 盲盒触发逻辑
        foreach (BarrageBoxSetting config in barrageConfigs.barrageBoxSetting)
        {
            if (config.Type == "进入")
            {
                int index = UnityEngine.Random.Range(0, config.Calls.Count);
                string CallName = config.Calls[index];
                barrageConfigs.EnqueueAction(user, avatar, CallName, 1, config.Count, config.Delay);
            }
        }
    }

    public void handleLike(string json)
    {
        BarrageData info = JsonUtility.FromJson<BarrageData>(json);
        string user = info.name;
        string avatar = info.userAvatar;
        int count = info.count;

        if (!likeCount.ContainsKey(user))
        {
            likeCount.Add(user, count);
        }
        else
        {
            likeCount[user] += count;
        }

        BarrageController barrageConfigs = FindAnyObjectByType<BarrageController>();
        if (barrageConfigs == null)
        {
            Debug.LogWarning("未找到 BarrageController 配置，忽略点赞事件");
            return;
        }
        // 普通触发逻辑
        foreach (BarrageNormalSetting config in barrageConfigs.barrageNormalSetting)
        {
            if (config.Type == "点赞" && likeCount[user] > int.Parse(config.Message))
            {
                barrageConfigs.EnqueueAction(user, avatar, config.CallName, 1, config.Count, config.Delay);
                likeCount[user] -= int.Parse(config.Message);
            }
        }

        // 盲盒触发逻辑
        foreach (BarrageBoxSetting config in barrageConfigs.barrageBoxSetting)
        {
            if (config.Type == "点赞" && likeCount[user] > int.Parse(config.Message))
            {
                int index = UnityEngine.Random.Range(0, config.Calls.Count);
                string CallName = config.Calls[index];
                barrageConfigs.EnqueueAction(user, avatar, CallName, 1, config.Count, config.Delay);
                likeCount[user] -= int.Parse(config.Message);
            }
        }
    }

}
