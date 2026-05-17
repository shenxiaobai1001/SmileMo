using System.Collections.Generic;
using UnityEngine;

public class EggManager : MonoBehaviour
{
    public static EggManager Instance { get; private set; }
    public static bool InstanceExists => Instance != null;

    [SerializeField] private Camera mainCam;
    [SerializeField] private float screenMargin = 0.1f;

    private readonly HashSet<IEggControllable> activeEggs = new();
    private readonly HashSet<IEggControllable> pendingAdd = new();
    private readonly HashSet<IEggControllable> pendingRemove = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start() => mainCam ??= Camera.main;

    void LateUpdate()
    {
        ApplyPendingChanges();

        if (activeEggs.Count == 0) return;

        // 复制当前集合进行遍历（避免迭代中集合被修改）
        foreach (var egg in new List<IEggControllable>(activeEggs))
        {
            if (pendingRemove.Contains(egg)) continue;

            Vector3 viewPos = mainCam.WorldToViewportPoint(egg.GetPosition());
            if (viewPos.z <= 0) continue;

            bool insideX = viewPos.x >= -screenMargin && viewPos.x <= 1 + screenMargin;
            bool insideY = viewPos.y >= -screenMargin && viewPos.y <= 1 + screenMargin;
            bool isOnScreen = insideX && insideY;

            egg.SetOnScreenState(isOnScreen);

            // 离屏且未被标记：交给管理器调度移除
            if (!isOnScreen && !pendingRemove.Contains(egg))
                ScheduleRemove(egg);
        }
    }

    private void ApplyPendingChanges()
    {
        if (pendingAdd.Count > 0)
        {
            activeEggs.UnionWith(pendingAdd);
            pendingAdd.Clear();
        }

        if (pendingRemove.Count > 0)
        {
            activeEggs.ExceptWith(pendingRemove);
            pendingRemove.Clear();
        }
    }

    public void Register(IEggControllable egg)
    {
        if (egg == null) return;
        pendingAdd.Add(egg);
        pendingRemove.Remove(egg);
    }

    public void Unregister(IEggControllable egg)
    {
        if (egg == null) return;
        pendingRemove.Add(egg);
        pendingAdd.Remove(egg);
    }

    public void ScheduleRemove(IEggControllable egg) => pendingRemove.Add(egg);
}

public interface IEggControllable
{
    Vector3 GetPosition();
    void SetOnScreenState(bool onScreen);
}