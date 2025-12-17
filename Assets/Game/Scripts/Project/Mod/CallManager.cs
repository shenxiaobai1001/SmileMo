using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallManager : MonoBehaviour
{
    public static CallManager Instance;

    public List<GameObject> smallCalls;

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

    Dictionary<int, float> waiTime = new Dictionary<int, float>();
    private void Start()
    {
        OnInitWaitTime();
        EventManager.Instance.AddListener(Events.BeginSnakeMap, OnBeginSnakeMap);
        EventManager.Instance.AddListener(Events.OnVideoPlayEnd, OnBeginCreateDuck);
    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(Events.BeginSnakeMap, OnBeginSnakeMap);
        EventManager.Instance.RemoveListener(Events.OnVideoPlayEnd, OnBeginCreateDuck);
    }
    void OnInitWaitTime()
    {
        waiTime.Add(1, 3);
        waiTime.Add(2, 3.5f);
        waiTime.Add(5, 1.5f);
        waiTime.Add(8, 1);
        waiTime.Add(6, 2);
    }


    public GameObject videoPlayer;
    string box = "Belle";
    string dj = "DJ";
    string nullDUCK = "Duck/Null";
    string getDUCK = "Duck/Get";
    int normalCount = 77;
    int greenCount = 9;
    int boxIndex = 0;
    int videoType = 0;
    public void OnCreateVideoPlayer(string callName ,int type)
    {
        string title = callName == "美女盲盒" ? box : dj;
        int allCount = type == 1 ? normalCount : greenCount;
        boxIndex = Random.Range(1, allCount + 1);
        string path = $"{title}/{boxIndex}";
        GameObject obj = SimplePool.Spawn(videoPlayer, PlayerController.Instance.transform.position, Quaternion.identity);
        VideoManager videoManager = obj.GetComponent<VideoManager>();
        obj.transform.SetParent(transform);
        obj.SetActive(true);
        videoManager.OnPlayVideo(type, path);
        videoType= type;
    }
    Queue<int> onCreate=new Queue<int>();
    public void OnCreateDuckVideoPlayer()
    {
        boxIndex = 0;
        int index = Random.Range(0, 82);
        bool getduck = index >= 17;
        string title = getduck ? getDUCK : nullDUCK;
        int duckPath = 0;
        if (getduck) { 
            if(index>=17&& index < 21)
            {
                duckPath = 1;
            }
            else if(index >= 21 && index < 25)
            {
                duckPath = 1;
            }
            else if(index >= 25 && index < 29)
            {
                duckPath = 2;
            }
            else if (index >= 29 && index < 33)
            {
                duckPath = 11;
            }
            else if (index >= 33 && index < 37)
            {
                duckPath = 11;
            }
            else if (index >= 37 && index < 41)
            {
                duckPath = 15;
            }
            else if (index >= 41 && index < 45)
            {
                duckPath = 20;
            }
            else if (index >= 45 && index < 49)
            {
                duckPath = 40;
            }
            else if (index >= 49 && index < 53)
            {
                duckPath = 50;
            }
            else if (index >= 53 && index < 57)
            {
                duckPath = 80;
            }
            else if (index >= 57 && index < 61)
            {
                duckPath = 100;
            }
            else if (index >= 61 && index < 65)
            {
                duckPath = 200;
            }
            else if (index >= 65 && index < 69)
            {
                duckPath = 300;
            }
            else if (index >= 69 && index < 73)
            {
                duckPath = 500;
            }
            else if (index >= 73 && index < 77)
            {
                duckPath = 500;
            }
            else if (index == 77)
            {
                duckPath = 1000;
            }
            else if (index == 79)
            {
                duckPath = 3000;
            }
            else if (index == 80)
            {
                duckPath = 5000;
            }
            else if (index == 81)
            {
                duckPath = 10000;
            }
            else {
                duckPath = 1;
            }
        }
        else 
            duckPath = Random.Range(1, 24);
        string path = $"{title}/{duckPath}";
        PFunc.Log("鸭子视频路径", path, duckPath);
        GameObject obj = SimplePool.Spawn(videoPlayer, PlayerController.Instance.transform.position, Quaternion.identity);
        VideoManager videoManager = obj.GetComponent<VideoManager>();
        obj.transform.SetParent(transform);
        obj.SetActive(true);
        videoManager.OnPlayVideo(2, path,false);
        duckPath = getduck ? duckPath : 0;
        onCreate.Enqueue(duckPath);
        videoType = 2;
    }

    void OnBeginCreateDuck(object msg)
    {
        if (onCreate == null || onCreate.Count <= 0) return;
        int getduck = onCreate.Dequeue();
        PFunc.Log("OnBeginCreateDuck", getduck);
        if (getduck > 0)
        {
            ItemManager.Instance.OnCreateFlyItem(getduck);
        }
    }
    public void OnCreateCall()
    {

        int index = Random.Range(0, smallCalls.Count);
        GameObject tksobj = smallCalls[index];
        switch (index)
        {
            case 0:
                Sound.PlaySound("Sound/Mod/xy1");
                break;
            case 1:
                Sound.PlaySound("Sound/Mod/lh1");
                break;
            case 2:
                Sound.PlaySound("Sound/Mod/qw1");
                break;
            case 3:
                Sound.PlaySound("Sound/Mod/xj1");
                break;
            case 4:
                Sound.PlaySound("Sound/Mod/xm1");
                break;
            case 5:
                Sound.PlaySound("Sound/Mod/xn1");
                break;
            case 6:
                Sound.PlaySound("Sound/Mod/xxy");
                break;
        }

        GameObject obj = SimplePool.Spawn(tksobj, PlayerController.Instance.transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.SetActive(true);
    }
    Tween tweenCamera;
    Tween tweenPlayer;
    bool isSnake = false;
    Vector3 originalPosition;  // 保存原始位置
    void OnBeginSnakeMap(object msg)
    {
        bool snake = (bool)msg;
        if (snake)
        {
            if (videoType == 2)
            {
                if (waiTime.ContainsKey(boxIndex))
                {
                    Invoke("OnBieginSnakeScence", waiTime[boxIndex]);
                }
                else
                {
                    Invoke("OnBieginSnakeScence", 0.3f);
                }
            }
            else
            {
                Invoke("OnBieginSnakeScence", 0.3f);
            }
        }
        else
        {
            OnStopSnakeScence();
        }
    }

    void OnBieginSnakeScence()
    {
        if (!isSnake)
        {
            isSnake = true;
            originalPosition = transform.position;
            tweenCamera = Camera.main.transform.DOMoveY(originalPosition.y + 0.5f, 0.2f)  // 向上移动
                    .SetEase(Ease.OutFlash)                                // 缓动效果
                    .SetLoops(-1, LoopType.Yoyo)                            // 无限循环，往返运动
                    .OnKill(() => transform.position = originalPosition);   // 动画结束时重置位置
            tweenPlayer = PlayerController.Instance.transform.DOMoveY(originalPosition.y - 0.1f, 0.2f)  // 向上移动
                .SetEase(Ease.OutFlash)                                // 缓动效果
                .SetLoops(-1, LoopType.Yoyo)                            // 无限循环，往返运动
                .OnKill(() => transform.position = originalPosition);   // 动画结束时重置位置
        }
    }

    void OnStopSnakeScence( )
    {
        isSnake = false;
        if (tweenCamera != null) tweenCamera.Kill();
        if (tweenPlayer != null) tweenPlayer.Kill();
    }
}
