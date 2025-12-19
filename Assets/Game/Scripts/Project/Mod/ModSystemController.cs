using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ModSystemController : MonoBehaviour
{
    public static ModSystemController Instance { get; private set; }
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

    public bool reverseJump = false;
    float reverseJumpTime = 0;
    private void Start()
    {
        if (level7) level7.SetActive(false);
    }
    public void OnSetRerverseJump(DataInfo dataInfo)
    {
        reverseJumpTime = dataInfo.count * dataInfo.time;
        if (!reverseJump)
        {
            reverseJump = true;
            StartCoroutine(OnCheckRerverseJumpTime());
        }
    }
    IEnumerator OnCheckRerverseJumpTime()
    {
        while (reverseJumpTime > 0)
        {
            reverseJumpTime -= Time.deltaTime;
            yield return null;
        }
        reverseJumpTime = 0;
        reverseJump = false;
    }

    public bool reverseCamera = false;
    float reverseCameraTime = 0;
    public GameObject videoPlayer;
    public void OnSetRerverseCamera()
    {
        string path = $"MOD/fanzhuan";
        GameObject obj = SimplePool.Spawn(videoPlayer, PlayerController.Instance.transform.position, Quaternion.identity);
        VideoManager videoManager = obj.GetComponent<VideoManager>();
        obj.transform.SetParent(transform);
        obj.SetActive(true);
        videoManager.OnPlayVideo(2, path,false);

        reverseCameraTime += 2;
        if (!reverseCamera)
        {
            Invoke("OnReadyRerverse",1.5f);
        }
    }
    void OnReadyRerverse()
    {
        reverseCamera = true;
        StartCoroutine(OnCheckReverseCamera());
    }
    IEnumerator OnCheckReverseCamera()
    {
        while (reverseCameraTime > 0)
        {
            reverseCameraTime -= Time.deltaTime;
            yield return null;
        }
        reverseCameraTime = 0;
        reverseCamera = false;
    }

    public bool Protecket = false;
    float ProtecketTime = 0;
    public GameObject shieldIPrefab;
    GameObject shieldIObj;
    public void OnSetPlayerProtecket(int giftCount, int times, float delay)
    {
        ProtecketTime = giftCount * times * 5;
        if (ItemManager.Instance.isHang)
        {
            PlayerModController.Instance.OnCancelHangSelf();
        }
        //  Sound.PlaySound("Sound/Mod/Freeze");
        if (!Protecket)
        {
            Protecket = true;
            shieldIObj = Instantiate(shieldIPrefab);
            StartCoroutine(OnCheckProtecket());
        }
    }
    IEnumerator OnCheckProtecket()
    {
        while (ProtecketTime > 0)
        {
            ProtecketTime -= Time.deltaTime;
            yield return null;
        }
        if (shieldIObj) Destroy(shieldIObj);
        ProtecketTime = 0;
        Protecket = false;
    }

    public GameObject ShakespeareLeft;
    public GameObject ShakespeareRight;
    public void OnShakespeare()
    {
        int value = Random.Range(0, 2);
        GameObject createObj = value == 0 ? ShakespeareLeft : ShakespeareRight;
        GameObject obj = SimplePool.Spawn(createObj, transform.position, Quaternion.identity);
        Sound.PlaySound("Sound/Mod/ssby");
        obj.transform.parent = transform;
    }

    public GameObject bigBeta;
    public void OnBigBetaForward()
    {
        Sound.PlaySound("Sound/Mod/dbt");
        GameObject obj = SimplePool.Spawn(bigBeta, transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        PlayerModController.Instance.OnBigBetaForward(true);
    }
    public void OnBigBetaBack()
    {
        Sound.PlaySound("Sound/Mod/dbt");
        GameObject obj = SimplePool.Spawn(bigBeta, transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        PlayerModController.Instance.OnBigBetaForward(false);
    }
    public GameObject tansfarPre;
    public void OnRandromPlayerPos()
    {
        if (transFarCount > 0) return;
        Sound.PlaySound("Sound/Mod/likehere");
        GameObject obj = SimplePool.Spawn(tansfarPre, transform.position, Quaternion.identity);
        obj.transform.parent = transform;
        InvokeRepeating("OnRandPlayer",0,0.4f);
    }
    int transFarCount = 0;
    void OnRandPlayer()
    {
        float leftValue=PlayerController.Instance.transform.position.x-200;
        float  rightValue = PlayerController.Instance.transform.position.x + 200;
        float targetX=Random.Range(leftValue,rightValue);
        PlayerController.Instance.transform.position=new Vector3(targetX,5,0);
        transFarCount++;
        if (transFarCount >= 10)
        {
            transFarCount = 0;
            CancelInvoke("OnRandPlayer");
        }
    }

    public Vector3 playerPos;
    public int oldLevel;
    public GameObject level7;
    public GameObject Map;
    public void OnTransFarSeven()
    {
        bool protect = ModSystemController.Instance.Protecket;
        if (protect) return;
        oldLevel = GameController.Instance.gameLevel;
        playerPos =PlayerController.Instance.transform.position;
        if(level7) level7.SetActive(true);

        LevelSevenController.Instance.OnSetPlayerSeven();
        if (Map) Map.SetActive(false);
    }
    public void OnTransFarSevenOut()
    {
        GameController.Instance.gameLevel  = oldLevel;
        PlayerController.Instance.transform.position  = playerPos;
        LevelSevenController.Instance.gameObject.SetActive(false);
        if (level7) level7.SetActive(false);
        if (level7) Map.SetActive(true);
    }
}
