using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerModController : MonoBehaviour
{
    public static PlayerModController Instance;
    public PlayerController playerController;
    public Rigidbody2D rigidbody;
    public BoxCollider2D box;
    public GameObject Center;
    public Transform spriteTrans;
    public GameObject BoomPre;
    public GameObject Boomeff;
    public Transform createPos;

    string[] gaiYas=new string[7] {"bishi","chaodan","huotui","mifan","jidan","jirou","mifen"};

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
    private void Start()
    {
        EventManager.Instance.AddListener(Events.OnQLMove, OnModMoveQL);
        EventManager.Instance.AddListener(Events.OnTCMove, OnModMoveTC);
        EventManager.Instance.AddListener(Events.OneFingerMove, OnModMoveFinger);
        StartCoroutine(OnCheckGround());
    }

    bool isGMControl = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isGMControl = true;
            rigidbody.isKinematic = true;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.position = new Vector3(transform.position.x+5, 0);
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
                int index=Random.Range(0, 7);
                Sound.PlaySound($"Sound/Mod/fanqie{gaiYas[index]}");
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
            if(!isGMControl)
            {
                if (transform.position.y >= playerY)
                {
                    transform.position = new Vector3(transform.position.x, playerY);
                }
            }
        }
    }

    bool isHitPlayerBack = false;
    public float backTime = 0;
    bool canAddHitTime = true;

    bool isHitPlayerForward = false;
    public float ForwardTime = 0;
    bool canAddHitTime1 = true;

    int isPassivityMove = 0;
    IEnumerator OnHitPlayerBack()
    {
        while (backTime > 0)
        {
            bool protect = ModSystemController.Instance.Protecket;
            if (protect) { 
                isPassivityMove--;
                isHitPlayerBack = false;
                if (isPassivityMove <= 0)
                {
                    backTime = 0;
                    OnChangeState(true);
                }
                yield break;
            }
            if (transform.position.y < playerY)
            {
                transform.Translate(Vector2.up * 8 * Time.deltaTime);
            }
            transform.Translate(new Vector2(-1,1) * 16 * Time.deltaTime);
            backTime -= Time.deltaTime;
            yield return null;
        }
        isPassivityMove--;
        isHitPlayerBack = false;
        if (isPassivityMove <= 0)
        {
            isPassivityMove = 0;
            backTime = 0;
            OnChangeState(true);
        }
    }
    public void OnLeftHitPlayer()
    {
        bool protect = ModSystemController.Instance.Protecket;
        if (protect) return;
        backTime += 0.055f;
        GameObject boom = SimplePool.Spawn(Boomeff, transform.position, Quaternion.identity);
        boom.transform.SetParent(createPos.transform);
        boom.SetActive(true);
        if(!isKinematic)
         OnChangeState(false);
        if (!isHitPlayerBack)
        {
            PlayerController.Instance.OnChangeHitState();
            isPassivityMove++;
            isHitPlayerBack = true;
            StartCoroutine(OnHitPlayerBack());
        }
        if (ItemManager.Instance.isHang)
        {
            OnCancelHangSelf();
        }
    }
    public void OnRightHitPlayer()
    {
        bool protect = ModSystemController.Instance.Protecket;
        if (protect) return;
        ForwardTime += 0.055f;
        GameObject boom = SimplePool.Spawn(Boomeff, transform.position, Quaternion.identity);
        boom.transform.SetParent(createPos.transform);
        boom.SetActive(true);
        if (!isKinematic)
            OnChangeState(false);
        if (!isHitPlayerForward)
        {
            isPassivityMove++;
            isHitPlayerForward = true;
            StartCoroutine(OnHitPlayerForward());
        }
        if (ItemManager.Instance.isHang)
        {
            OnCancelHangSelf();
        }
    }


    IEnumerator OnHitPlayerForward()
    {
        while (ForwardTime > 0)
        {
            bool protect = ModSystemController.Instance.Protecket;
            // 如果处于保护状态，等待直到保护结束
            if (protect)
            {
                isPassivityMove--;
                isHitPlayerForward = false;
                if (isPassivityMove <= 0)
                {
                    ForwardTime = 0;
                    OnChangeState(true);
                }
                yield break;
            }
            if (transform.position.y < playerY)
            {
                transform.Translate(Vector2.up * 8 * Time.deltaTime);
            }
            transform.Translate(new Vector2(1,1) * 16 * Time.deltaTime);
            ForwardTime -= Time.deltaTime;
            yield return null;
        }
        isPassivityMove--;
        isHitPlayerForward = false;
        if (isPassivityMove <= 0)
        {
            backTime = 0;
            OnChangeState(true);
        }
    }

    public void OnKickPlayer(Vector3 force, bool boom = false)
    {
        bool protect = ModSystemController.Instance.Protecket;
        if (protect) return;
        if (ItemManager.Instance.isHang)
        {
            OnCancelHangSelf();
        }
        if (boom)
        {
            GameObject boomobj = SimplePool.Spawn(BoomPre, transform.position, Quaternion.identity);
            boomobj.transform.SetParent(createPos.transform);
            boomobj.SetActive(true);
        }
        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }
    bool isKinematic = false;
    void OnChangeState(bool open)
    {
        box.enabled = open;
        Center.SetActive(open);
        playerController.isHit = !open;
        rigidbody.isKinematic = !open;
        isKinematic = !open;
        PlayerController.Instance.OnRest();
    }

    bool isCloaking = false;
    public float cloakingTime = 0;
    public void OnCloaking(DataInfo dataInfo)
    {
        cloakingTime += dataInfo.count * dataInfo.time;
    }

    #region 天残脚麒麟臂
    MoveType moveType = MoveType.None;
    bool isMove = false;
    bool toMove = false;
    Vector3 startPos = Vector3.zero;
    int tcCount = 0;
    int qlCount = 0;
    int fingerCount = 0;
    void OnModMoveTC(object msg)
    {
        bool show = (bool)msg;
        startPos = transform.position;
        if (show)
            tcCount++;
        else
            tcCount--;

        OnCheckMoveType();
        if (toMove)
        {
            OnChangeState(false);
            if (!isMove)
            {
                isPassivityMove++;
                isMove = true;
                StartCoroutine(OnModMove());
            }
        }
        if (ItemManager.Instance.isHang)
        {
            OnCancelHangSelf();
        }
    }
    void OnModMoveFinger(object msg)
    {
        bool show = (bool)msg;
        startPos = transform.position;
        if (show)
            fingerCount++;
        else
            fingerCount--;

        OnCheckMoveType();
        if (toMove)
        {
            OnChangeState(false);
            if (!isMove)
            {
                isPassivityMove++;
                isMove = true;
                StartCoroutine(OnModMove());
            }
        }
        if (ItemManager.Instance.isHang)
        {
            OnCancelHangSelf();
        }
    }
    void OnModMoveQL(object msg)
    {
        bool show = (bool)msg;
        startPos = transform.position;
        if (show)
            qlCount++;
        else
            qlCount--;

        OnCheckMoveType();
        if (toMove)
        {
            OnChangeState(false);
            if (!isMove)
            {
                isPassivityMove++;
                isMove = true;
                StartCoroutine(OnModMove());
            }
        }
        if (ItemManager.Instance.isHang)
        {
            OnCancelHangSelf();
        }
    }
    void OnCheckMoveType()
    {
        if (tcCount > 0 && qlCount <= 0)
        {
            moveType = MoveType.TC;
        }
        else if (tcCount <= 0 && qlCount > 0)
        {
            moveType = MoveType.QL;
        }
        else if (tcCount > 0 && qlCount > 0)
        {
            moveType = MoveType.DuiKang;
        }
        else if (fingerCount>0)
        {
            moveType = MoveType.Finger;
        }
        else
        {
            moveType = MoveType.None;
        }
        toMove = qlCount > 0 || tcCount > 0 || fingerCount > 0;
    }

    float playerY
    {
        get {             
            int value = 0;
            if (GameController.Instance!=null)
                value= GameController.Instance.gameLevel == 7 ? 190 : 5;
            else
            {
                value = 5;
            }
            return value;
        }
    }
    IEnumerator OnModMove()
    {
        PFunc.Log("OnModMove开始", isPassivityMove, toMove, moveType);
        while (toMove)
        {
            bool protect = ModSystemController.Instance.Protecket;
            // 如果处于保护状态，等待直到保护结束
            while (protect)
            {
                if (isKinematic)
                    OnChangeState(true);
                yield return new WaitForSeconds(0.1f);  // 每隔0.1秒检查一次
                protect = ModSystemController.Instance.Protecket;
            }
            if (!isKinematic)
                OnChangeState(false);
            switch (moveType)
            {
                case MoveType.None:
                    toMove = false;
                    yield return null;
                    break;
                case MoveType.DuiKang:
                    Camera.main.DOShakePosition(0.5f, new Vector3(2, 2, 2), 3, 50, true);
                    yield return new WaitForSeconds(0.7f);
                    transform.position = startPos;
                    yield return null;
                    break;
                case MoveType.TC:
                    transform.Translate(new Vector3(-1, 0.1f) * 20 * Time.deltaTime);
                    startPos = transform.position;
                    yield return null;
                    break;
                case MoveType.Finger:
                    transform.Translate(new Vector3(-1, 0.1f) * 30 * Time.deltaTime);
                    startPos = transform.position;
                    yield return null;
                    break;
                case MoveType.QL:
                    transform.Translate(new Vector3(1, 0.1f) * 20 * Time.deltaTime);
                    startPos = transform.position;
                    yield return null;
                    break;
            }
        }
       
        isPassivityMove--;
        isMove = false;
        PFunc.Log("3麒麟臂结束", isPassivityMove);
        if (isPassivityMove<=0)
        {
            isPassivityMove = 0;
            OnChangeState(true);
        }
    }
    enum MoveType
    {
        None,
        DuiKang,
        TC,
        QL,
        Finger,
    }
    #endregion

    bool isBigBetaForward = false;
    bool isBigBetaBack = false;
    public float BigBetaTime = 17;
    public float BigBetaBackTime = 17;
    public void OnBigBetaForward(bool forward)
    {
        bool protect = ModSystemController.Instance.Protecket;
        if (protect) return;
        if (forward)
        {
            if (!isBigBetaForward)
            {
                isPassivityMove++;
                OnChangeState(false);
                isBigBetaForward = true;
                StartCoroutine(BigBetaForward());
            }
        }
        else 
        {
            if (!isBigBetaBack)
            {
                isPassivityMove++;
                OnChangeState(false);
                isBigBetaBack = true;
                StartCoroutine(BigBetaBack());
            }
        }

        if (ItemManager.Instance.isHang) 
        { 
            OnCancelHangSelf();
        }
    }

    IEnumerator BigBetaForward()
    {
        while (BigBetaTime > 0)
        {
            if (transform.position.y < playerY)
            {
                transform.Translate(Vector2.up * 8 * Time.deltaTime);
            }
            transform.Translate(Vector2.right * 32 * Time.deltaTime);
            spriteTrans.Rotate(new Vector3(0,0,360)*10* Time.deltaTime);
            BigBetaTime -= Time.deltaTime;
            yield return null;
        }
        isBigBetaForward = false;
        BigBetaTime = 17;
        isPassivityMove--;
        spriteTrans.localEulerAngles = Vector3.zero;
        if (isPassivityMove<=0)
            OnChangeState(true);
    }
    IEnumerator BigBetaBack()
    {
        while (BigBetaBackTime > 0)
        {
            if (transform.position.y < playerY)
            {
                transform.Translate(Vector2.up * 8 * Time.deltaTime);
            }
            transform.Translate(Vector2.left * 32* Time.deltaTime);
            spriteTrans.Rotate(new Vector3(0, 0, -360) * 10 * Time.deltaTime);
            BigBetaBackTime -= Time.deltaTime;
            yield return null;
        }
        spriteTrans.localEulerAngles = Vector3.zero;
        isBigBetaBack = false;
        BigBetaTime = 17;
        isPassivityMove--;
        if (isPassivityMove <= 0)
            OnChangeState(true);
    }


    public GameObject Tomato;
    bool canTomto = false;
    float tomateTime = 0;
    public void OnClickToCreateTomaTo()
    {
        tomateTime += 10;
        if (!canTomto)
        {
            Sound.PlaySound("Sound/Mod/gaiya");
            EventManager.Instance.SendMessage(Events.GaiyaTomato);
            Invoke("OnReadyTomato",3);
        }
    }
    void OnReadyTomato()
    {
        canTomto = true;
        StartCoroutine(onCheckTomato());
    }
    IEnumerator onCheckTomato()
    {
        while (tomateTime>0)
        {
            tomateTime-=Time.deltaTime;
            yield return null;
        }
        EventManager.Instance.SendMessage(Events.GaiyaTomatoEnd);
        canTomto = false;
        tomateTime = 0;
    }
    #region 隐身
    bool invisibility = false;
    float visibilityTime = 0;
    public void OnInvisibility()
    {
        visibilityTime = 1;
        if (!invisibility) {
            invisibility = true;
            spriteTrans.gameObject.SetActive(false);
            StartCoroutine(OnCheckVisibility());
        }
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
    #endregion

    bool fastSpeed = false;
    float fastSpeedTime = 0;
    public void OnFastSpeed()
    {
        fastSpeedTime +=5;
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
    public  void OnHangSelf()
    {
        playerController.OnRest();
        playerController.isHit = true;
        spriteTrans.gameObject.SetActive(false);
        isPassivityMove++;
        OnChangeState(false);
    }
    public void OnCancelHangSelf()
    {
        playerController.isHit = true;
        spriteTrans.gameObject.SetActive(true);
        isPassivityMove--;
        if (isPassivityMove<=0)
        {
            isPassivityMove = 0;
            OnChangeState(true);
        }
       
        Vector3 vector = HangSelf.Instance.lastPoint.transform.position;
        if (vector != Vector3.zero) {
            playerController.transform.position = vector;
        }
        HangSelf.Instance.OnBreakeHang();
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(Events.OnTCMove, OnModMoveTC);
        EventManager.Instance.RemoveListener(Events.OnQLMove, OnModMoveQL);
    } 

}
