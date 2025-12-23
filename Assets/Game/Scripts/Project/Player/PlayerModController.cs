using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class PlayerModController : MonoBehaviour
{
    public static PlayerModController Instance;

    // 组件引用
    public PlayerController playerController;
    public Rigidbody2D rigidbody;
    public BoxCollider2D box;
    public GameObject Center;
    public Transform spriteTrans;
    public GameObject BoomPre;
    public GameObject Boomeff;
    public Transform createPos;
    public GameObject Tomato;
    public GameObject videoPlayer;

    // 爆炸类型
    public enum BoomType
    {
        None = 0,
        BoomEffect = 1,
        BoomPreEffect = 2
    }

    // 移动数据
    private class MoveData
    {
        public MoveType type = MoveType.None;
        public MoveDirection direction = MoveDirection.Left;
        public float moveTime = 0f;
        public float maxMoveTime = 0f;
        public bool rotate = false;

        public void Reset()
        {
            type = MoveType.None;
            moveTime = 0f;
            maxMoveTime = 0f;
        }

        public bool IsActive => moveTime > 0f;
    }

    public MoveType moveType1 = MoveType.None;

    // 新增：长时移动记忆
    private class LongTimeMove
    {
        public MoveDirection direction = MoveDirection.Left;
        public float remainingTime = 0f;  // 剩余移动时间
        public float totalTime = 0f;      // 总移动时间
        public float interruptedAt = 0f;  // 被打断时的时间点

        public void Set(MoveDirection dir, float time)
        {
            direction = dir;
            remainingTime = time;
            totalTime = time;
        }

        public void ReduceTime(float deltaTime)
        {
            if (remainingTime > 0)
            {
                remainingTime -= deltaTime;
                if (remainingTime < 0) remainingTime = 0;
            }
        }

        public bool HasRemainingTime => remainingTime > 0f;
    }

    private LongTimeMove longTimeMove = new LongTimeMove();

    // 移动数据存储
    private MoveData currentMove = new MoveData();
    private Dictionary<MoveType, float> storedMoveTimes = new Dictionary<MoveType, float>();

    // 状态控制
    private int isPassivityMove = 0;
    private bool isGMControl = false;
    private float playerY => GameController.Instance?.gameLevel == 7 ? 190 : 5;

    // 音效数组
    private string[] gaiYas = new string[7] { "bishi", "chaodan", "huotui", "mifan", "jidan", "jirou", "mifen" };

    // 其他功能状态
    private bool canTomto = false;
    private float tomateTime = 0;
    private float visibilityTime = 0;
    private float fastSpeedTime = 0;
    private bool invisibility = false;
    private bool fastSpeed = false;
    private Coroutine modMoveCoroutine;

    // 等待结束相关状态
    private bool isWaitingToEnd = false;
    private float waitStartTime = 0f;

    // 添加一个移动完全结束的标志
    private bool isMoveCompletelyEnded = true;

    // 新增：Normal方向时间记录
    private Dictionary<MoveDirection, float> normalDirectionTimes = new Dictionary<MoveDirection, float>()
    {
        { MoveDirection.Left, 0f },
        { MoveDirection.Right, 0f }
    };

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

        // 初始化移动时间字典
        foreach (MoveType type in System.Enum.GetValues(typeof(MoveType)))
        {
            storedMoveTimes[type] = 0f;
        }
    }

    private void Start()
    {
        // 移除所有移动相关的事件监听
        StartCoroutine(OnCheckGround());

        // 启动统一的移动协程
        if (modMoveCoroutine != null)
        {
            StopCoroutine(modMoveCoroutine);
        }
        modMoveCoroutine = StartCoroutine(ModMoveContinue());
    }

    float lastAddTime = 0;
    float lastNormalMoveTime = 0;
    #region 统一的移动系统

    /// <summary>
    /// 统一的外部移动触发方法
    /// </summary>
    public void TriggerModMove(MoveDirection direction, float moveTime, int boomType, MoveType moveType = MoveType.Normal, bool rotate = false)
    {
        // 检查保护状态
        bool protect = ModSystemController.Instance.Protecket;
        if (protect) return;

        // 播放爆炸特效
        PlayBoomEffect(boomType);

        // 取消挂起状态
        if (ItemManager.Instance != null && ItemManager.Instance.isHang)
        {
            OnCancelHangSelf();
        }

        // 重置移动完全结束标志
        isMoveCompletelyEnded = false;

        // 重置等待状态，因为有新的移动加入
        isWaitingToEnd = false;

        // 处理不同移动类型的逻辑
        switch (moveType)
        {
            case MoveType.Normal:
                // 新增：处理Normal移动
                HandleNormalMove(direction, moveTime);
                lastNormalMoveTime = Time.time;
                break;

            case MoveType.TC:
                // TC移动重置时间
                storedMoveTimes[MoveType.TC] = moveTime;
                break;

            case MoveType.QL:
                // QL移动重置时间
                storedMoveTimes[MoveType.QL] = moveTime;
                break;

            case MoveType.DuiKang:
                // 对抗状态，双方都设置时间
                storedMoveTimes[MoveType.TC] = moveTime;
                storedMoveTimes[MoveType.QL] = moveTime;
                break;
        }
        // 重置等待状态
        isWaitingToEnd = false;
        isWait = false;
        if (moveTime >= lastAddTime)
        {
            lastAddTime =  moveTime ;
            waitStartTime = Time.time;
        }
    
        currentMove.direction = direction;
        currentMove.rotate = rotate;
    }

    /// <summary>
    /// 新增：处理Normal移动
    /// </summary>
    private void HandleNormalMove(MoveDirection direction, float time)
    {
        // 累加对应方向的时间
        normalDirectionTimes[direction] += time;
    }
    /// <summary> 播放爆炸特效 </summary>
    private void PlayBoomEffect(int boomType)
    {
        if (boomType == 0) return;

        GameObject boomPrefab = boomType == 1 ? Boomeff : BoomPre;
        if (boomPrefab != null)
        {
            GameObject boom = SimplePool.Spawn(boomPrefab, transform.position, Quaternion.identity);
            boom.transform.SetParent(createPos.transform);
            boom.SetActive(true);
        }
    }

    /// <summary>统一的移动协程</summary>
    private IEnumerator ModMoveContinue()
    {
        while (true)
        {
            // 检查是否有活动的移动
            if (HasActiveMove())
            {
                // 重置移动完全结束标志
                isMoveCompletelyEnded = false;

                // 开始移动
                if (!currentMove.IsActive)
                {
                    DetermineCurrentMove();
                    StartMove();
                    yield return null;
                }

                // 确定当前移动类型
                DetermineCurrentMove();

                // 执行移动
                if (currentMove.IsActive)
                {
                    // 检查保护状态
                    bool protect = ModSystemController.Instance.Protecket;
                    if (!protect)
                    {
                        if (!isKinematic)
                        {
                            OnChangeState(false);
                            yield return null;
                            bool isfip = currentMove.type == MoveType.QL ||
                                       (currentMove.type == MoveType.Normal && currentMove.direction == MoveDirection.Right);
                            playerController.OnChangeHitState(isfip);
                        }
                        ExecuteMove();
                    }
                    else
                    {
                        if (isKinematic) OnChangeState(true);
                    }

                    // 减少所有移动时间
                    ReduceAllMoveTimes(Time.deltaTime);

                    PFunc.Log("减少所有移动时间", Time.time, lastNormalMoveTime,(Time.time - lastNormalMoveTime), (lastAddTime + 0.1f), currentMove.type, ModVideoPlayerController.Instance.IsPlaying);

                    if (Time.time - lastNormalMoveTime > (lastAddTime + 0.1f)
                        && (currentMove.type == MoveType.Normal|| currentMove.type == MoveType.NormalDuiKang)
                        && !ModVideoPlayerController.Instance.IsPlaying)
                    {
                        storedMoveTimes[MoveType.Normal] = 0f;
                        normalDirectionTimes[MoveDirection.Right] =0;
                        normalDirectionTimes[MoveDirection.Left] = 0;
                        isWaitingToEnd=true;
                    }
                }
            }
            else
            {
                // 没有活动移动，检查是否在等待结束
                if (isWaitingToEnd)
                {
                    PFunc.Log("检查是否等待了1秒", waitStartTime  ,lastAddTime, (waitStartTime-Time.time ), (lastAddTime + 0.1f));
                    // 检查是否等待了1秒
                    if (waitStartTime  >= (lastAddTime + 0.1f))
                    {
                        // 等待结束，执行真正的结束
                        EndMove();
                        isWaitingToEnd = false;
                        waitStartTime = 0f;
                        isMoveCompletelyEnded = true;
                        yield return null;
                    }
                    else
                    {
                        if (!isWait)
                        {
                            OnDeepMove();
                        }
                    }
                }
                else if (!isMoveCompletelyEnded)
                {
                    // 第一次检测到没有活动移动，开始等待
                    isWaitingToEnd = true;
                    waitStartTime = Time.time;
                }
            }
            yield return null;
        }
    }

    bool isWait = false;

    void OnDeepMove()
    {
        if (!currentMove.IsActive) return;
        bool protect = ModSystemController.Instance.Protecket;
        if (protect) return;

        if (SystemController.Instance.airWallContin)
        {
            if ((currentMove.type == MoveType.TC || currentMove.type == MoveType.Normal && currentMove.direction == MoveDirection.Left)
             && transform.position.x <= -6) return;
        }
        isWait = true;
        if (isKinematic) OnChangeState(true);
        Vector3 moveDir = currentMove.direction == MoveDirection.Left ?
                new Vector3(-10, 0) : new Vector3(10, 0);
        rigidbody.AddForce(moveDir, ForceMode2D.Impulse);
    }

    /// <summary> 检查是否有活动的移动 </summary> 
    private bool HasActiveMove()
    {
        // 检查TC/QL/对抗
        foreach (var kvp in storedMoveTimes)
        {
            if (kvp.Value > 0f && kvp.Key != MoveType.None)
            {
                return true;
            }
        }
        if (normalDirectionTimes[MoveDirection.Left] > 0f) return true;
        if (normalDirectionTimes[MoveDirection.Right] > 0f) return true;

        return false;
    }

    /// <summary> 确定当前应该执行的移动类型</summary>
    private void DetermineCurrentMove()
    {
        // 优先级1：TC/QL对抗
        if (storedMoveTimes[MoveType.TC] > 0f && storedMoveTimes[MoveType.QL] > 0f)
        {
            currentMove.type = MoveType.DuiKang;
            CameraShaker.Instance.SetShakeParameters(0.2f, 2,new Vector3(0,1.2f));
            currentMove.moveTime = Mathf.Min(storedMoveTimes[MoveType.TC], storedMoveTimes[MoveType.QL]);
        }
        // 优先级2：TC移动
        else if (storedMoveTimes[MoveType.TC] > 0f)
        {
            currentMove.type = MoveType.TC;
            currentMove.direction = MoveDirection.Left; // TC固定向左
            currentMove.moveTime = storedMoveTimes[MoveType.TC];
        }
        // 优先级3：QL移动
        else if (storedMoveTimes[MoveType.QL] > 0f)
        {
            currentMove.type = MoveType.QL;
            currentMove.direction = MoveDirection.Right; // QL固定向右
            currentMove.moveTime = storedMoveTimes[MoveType.QL];
        }
        // 优先级4：Normal对抗
        else if (normalDirectionTimes[MoveDirection.Left] > 0 &&  normalDirectionTimes[MoveDirection.Right] > 0)
        {
            CameraShaker.Instance.SetShakeParameters(0.2f, 2, new Vector3(0, 0.8f));
            currentMove.type = MoveType.NormalDuiKang;
            float duikangTime = normalDirectionTimes[MoveDirection.Left] > normalDirectionTimes[MoveDirection.Right] ? 
                normalDirectionTimes[MoveDirection.Right] : normalDirectionTimes[MoveDirection.Left];
            currentMove.moveTime = duikangTime;
        }
        // 优先级5：Normal向左移动
        else if (normalDirectionTimes[MoveDirection.Left] > 0f)
        {
            currentMove.type = MoveType.Normal;
            currentMove.direction = MoveDirection.Left;
            currentMove.moveTime = normalDirectionTimes[MoveDirection.Left];
        }
        // 优先级6：Normal向右移动
        else if (normalDirectionTimes[MoveDirection.Right] > 0f)
        {
            currentMove.type = MoveType.Normal;
            currentMove.direction = MoveDirection.Right;
            currentMove.moveTime = normalDirectionTimes[MoveDirection.Right];
        }
        else
        {
            currentMove.Reset();
        }

        moveType1 = currentMove.type;
    }

    float airMintime = 0.1f;
    float airtime = 0;
    float snakeTargetTime = 1;
    float snakeTime = 2;

    /// <summary> 执行移动逻辑 </summary>
    private void ExecuteMove()
    {
        if (!currentMove.IsActive) return;

        if (SystemController.Instance.airWallContin)
        {
            if ((currentMove.type == MoveType.TC ||
                 (currentMove.type == MoveType.Normal && currentMove.direction == MoveDirection.Left))
                && transform.position.x <= -6)
            {
                airtime += Time.deltaTime;
                if (airtime > airMintime)
                {
                    SystemController.Instance.OnSetWallHp(1);
                    airtime = 0;
                }
                if (transform.position.x <= -6)
                {
                    transform.position = new Vector3(-6, transform.position.y);
                }
                spriteTrans.transform.localEulerAngles = Vector3.zero;
                return;
            }
        }

        // 根据移动类型执行不同的移动逻辑
        switch (currentMove.type)
        {
            case MoveType.DuiKang:
                OnNormalDuiKangScence();
                break;

            case MoveType.NormalDuiKang:  // 新增：Normal对抗状态
                OnNormalDuiKangScence();
                break;

            case MoveType.TC:
                MovePlayer(new Vector3(-1, 0f) * 20);
                break;

            case MoveType.QL:
                MovePlayer(new Vector3(1, 0f) * 20);
                break;

            case MoveType.Normal:
                float moveSpeed = 16f;
                Vector3 moveDir = currentMove.direction == MoveDirection.Left ?
                    new Vector3(-1, 0.3f) : new Vector3(1, 0.3f);
                MovePlayer(moveDir * moveSpeed);
                break;
        }
    }

    /// <summary>
    /// 新增：Normal对抗状态处理
    /// </summary>
    void OnNormalDuiKangScence()
    {
        // Normal对抗状态，摄像机抖动，不移动
        snakeTime += Time.deltaTime;
        if (snakeTime > snakeTargetTime)
        {
            CameraShaker.Instance.StartShake(snakeTargetTime);
            snakeTime = 0;
        }
    }

    /// <summary> 移动玩家 </summary>
    private void MovePlayer(Vector3 velocity)
    {
        if (transform.position.y < playerY)
        {
            transform.Translate(Vector2.up * 6 * Time.deltaTime);
        }
        transform.Translate(velocity * Time.deltaTime);
    }

    /// <summary> 开始移动 </summary>
    private void StartMove()
    {
        if (!isKinematic)
        {
            OnChangeState(false);
            bool isfip = currentMove.type == MoveType.QL ||
                       (currentMove.type == MoveType.Normal && currentMove.direction == MoveDirection.Right);
            playerController.OnChangeHitState(isfip);
        }
    }

    /// <summary> 结束移动</summary>
    private void EndMove()
    {
        isPassivityMove = 0;
        OnChangeState(true);
        PFunc.Log("移动完全结束");
    }

    /// <summary> 减少所有移动时间</summary>
    private void ReduceAllMoveTimes(float deltaTime)
    {
        // 减少TC/QL移动时间
        foreach (var type in new List<MoveType>(storedMoveTimes.Keys))
        {
            if (storedMoveTimes[type] > 0f)
            {
                storedMoveTimes[type] -= deltaTime;
                if (storedMoveTimes[type] < 0f)
                    storedMoveTimes[type] = 0f;
            }
        }

        if (normalDirectionTimes[MoveDirection.Left]>0)
        normalDirectionTimes[MoveDirection.Left] -= deltaTime;
        if (normalDirectionTimes[MoveDirection.Left] < 0f)
            normalDirectionTimes[MoveDirection.Left] = 0f;
        if (normalDirectionTimes[MoveDirection.Right] > 0)
            normalDirectionTimes[MoveDirection.Right] -= deltaTime;
        if (normalDirectionTimes[MoveDirection.Right] < 0f)
            normalDirectionTimes[MoveDirection.Right] = 0f;
    }

    #endregion

    #region 原有非移动功能保持

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isGMControl = true;
            rigidbody.isKinematic = true;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.position = new Vector3(transform.position.x + 5, 0);
        }
        if (Input.GetKey(KeyCode.L))
        {
            transform.Translate(Vector3.up * 15 * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            isGMControl = false;
            if (isPassivityMove <= 0)
            {
                rigidbody.isKinematic = false;
            }
        }
        if (canTomto)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject obj = SimplePool.Spawn(Tomato, transform.position, Quaternion.identity);
                obj.transform.parent = createPos;
                obj.SetActive(true);
                obj.GetComponent<Tomate>().OnStartMove();
                int index = Random.Range(0, 7);
                Sound.PlaySound($"Sound/Mod/fanqie{gaiYas[index]}");
            }
        }
        if (!isGMControl && !GameController.Instance.isAutomatic)
        {
            if (transform.position.y >= playerY)
            {
                transform.position = new Vector3(transform.position.x, playerY);
            }
        }
    }

    IEnumerator OnCheckGround()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (transform.position.y < -4)
            {
                transform.position = new Vector3(transform.position.x, 0);
            }
        }
    }

    bool isKinematic = false;

    void OnChangeState(bool open)
    {
        box.enabled = open;
        Center.SetActive(open);
        if (playerController != null)
        {
            playerController.isHit = !open;
        }
        rigidbody.isKinematic = !open;
        isKinematic = !open;
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.OnRest();
        }
    }


    public void OnClickToCreateTomaTo()
    {
        tomateTime += 10;
        if (!canTomto)
        {
            Sound.PlaySound("Sound/Mod/gaiya");
            EventManager.Instance.SendMessage(Events.GaiyaTomato);
            Invoke("OnReadyTomato", 3);
        }
    }

    void OnReadyTomato()
    {
        canTomto = true;
        StartCoroutine(onCheckTomato());
    }

    IEnumerator onCheckTomato()
    {
        while (tomateTime > 0)
        {
            tomateTime -= Time.deltaTime;
            yield return null;
        }
        EventManager.Instance.SendMessage(Events.GaiyaTomatoEnd);
        canTomto = false;
        tomateTime = 0;
    }

    public void OnInvisibility()
    {
        visibilityTime = 1;
        string path = $"MOD/yinshen";
        GameObject obj = SimplePool.Spawn(videoPlayer, PlayerController.Instance.transform.position, Quaternion.identity);
        VideoManager videoManager = obj.GetComponent<VideoManager>();
        obj.transform.SetParent(Camera.main.transform);
        obj.SetActive(true);
        videoManager.OnPlayVideo(2, path, false);
        if (!invisibility)
        {
            Invoke("OnReadyVisibility", 1);
        }
    }

    void OnReadyVisibility()
    {
        invisibility = true;
        spriteTrans.gameObject.SetActive(false);
        StartCoroutine(OnCheckVisibility());
    }

    IEnumerator OnCheckVisibility()
    {
        while (visibilityTime > 0)
        {
            visibilityTime -= Time.deltaTime;
            yield return null;
        }
        spriteTrans.gameObject.SetActive(true);
        invisibility = false;
        visibilityTime = 0;
    }

    public void OnFastSpeed()
    {
        fastSpeedTime += 5;
        PlayerData.Instance.moveSpeed += 5;
        PlayerData.Instance.fmoveSpeed += 5;
        if (!fastSpeed)
        {
            fastSpeed = true;
            StartCoroutine(OnCheckFastSpeed());
        }
    }

    public void OnMainSpeed()
    {
        fastSpeedTime = 5;
        PlayerData.Instance.moveSpeed -= 5;
        PlayerData.Instance.fmoveSpeed -= 5;
        if (PlayerData.Instance.moveSpeed < 0)
        {
            PlayerData.Instance.moveSpeed = 1;
        }
        if (PlayerData.Instance.fmoveSpeed < 0)
        {
            PlayerData.Instance.fmoveSpeed = 1;
        }
        if (!fastSpeed)
        {
            fastSpeed = true;
            StartCoroutine(OnCheckFastSpeed());
        }
    }

    IEnumerator OnCheckFastSpeed()
    {
        while (fastSpeedTime > 0)
        {
            fastSpeedTime -= Time.deltaTime;
            yield return null;
        }
        PlayerData.Instance.moveSpeed = 6.5f;
        PlayerData.Instance.fmoveSpeed = 12.4f;
        fastSpeed = false;
        fastSpeedTime = 0;
    }

    public void OnHangSelf()
    {
        OnChangeState(false);
        if (playerController != null)
        {
            playerController.isHit = true;
        }
        spriteTrans.gameObject.SetActive(false);
        isPassivityMove++;
    }

    public void OnCancelHangSelf()
    {
        spriteTrans.gameObject.SetActive(true);
        isPassivityMove--;
        if (isPassivityMove <= 0)
        {
            isPassivityMove = 0;
            OnChangeState(true);
        }

        if (HangSelf.Instance != null && HangSelf.Instance.lastPoint != null)
        {
            Vector3 vector = HangSelf.Instance.lastPoint.transform.position;
            if (vector != Vector3.zero)
            {
                transform.position = vector;
            }
            HangSelf.Instance.OnBreakeHang();
        }
    }

    #endregion
}