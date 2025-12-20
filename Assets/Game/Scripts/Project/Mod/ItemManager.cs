using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using static UnityEngine.Rendering.DebugUI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public Transform createPos1;
    public Transform createPosLeft;
    public Transform createPosRight;

    public GameObject duckObj;
    public GameObject pdgObj;
    public GameObject Ice;
    public GameObject Shield;
    public GameObject RightLeg;
    public GameObject LeftLeg;
    public GameObject Electricity;
    public GameObject RainBowCat;
    public GameObject BoomGrandema;
    public GameObject blackHand;

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

   public int allReadyCreateDuck = 0;
    public int allCreateDuck = 0;
    bool isCreateDuck = false;
    public void OnCreateFlyItem(int count)
    {
        UIDuck.Instance.OnSetCenter(true);
        allReadyCreateDuck += count;
        PFunc.Log("抓鸭子数量：", allReadyCreateDuck);
        if (!isCreateDuck)
        {
            isCreateDuck = true;
            StartCoroutine(OnCreateDuck());
        }
    }

    IEnumerator OnCreateDuck()
    {
        while (allReadyCreateDuck > 0)
        {
            Sound.PlaySound("Sound/Mod/Duck");
            int y = Random.Range(0, 7);
            Vector3 duckCPos = new Vector3(createPos1.position.x, createPos1.position.y+y);
            GameObject duck = SimplePool.Spawn(duckObj, duckCPos, Quaternion.identity);
            duck.transform.SetParent(this.transform);
            Duck duck1 = duck.GetComponent<Duck>();
            duck1.StartMove();
            allReadyCreateDuck--;
            allCreateDuck++;

            yield return new WaitForSeconds(0.05f);
        }
        isCreateDuck= false;
        allReadyCreateDuck = 0;
        UIDuck.Instance.OnSetCenter(false);
    }

    int OnGetDuckCount()
    {
        int duckCount = 0;
        int hasDuck = Random.Range(0,20);
        if (hasDuck == 0) {
            duckCount = Random.Range(1, 5001);
        }
        else
        {
            int qian = Random.Range(0, 10);
            if (qian == 0)
            {
                duckCount = Random.Range(1, 2001);
            }
            else
            {
                int shi = Random.Range(0, 10);
                if (shi == 0)
                {
                    duckCount = Random.Range(1, 1001);
                }
                else
                {
                    int bai = Random.Range(0, 10);
                    if (bai == 0)
                    {
                        duckCount = Random.Range(1, 101);
                    }
                    else
                    {
                        duckCount = Random.Range(0, 11);
                    }
                }
            }
        }
        return duckCount;
    }
    public GameObject videoPlayer;
    public void OnCreatePDG(string callName)
    {
        Transform createPos = null;
        switch (callName)
        {
            case "左边砸平底锅":
                createPos = createPosLeft;
                break;
            case "右边砸平底锅":
                createPos = createPosRight;
                break;
        }
        Sound.PlaySound("Sound/Mod/PDG");
        GameObject obj = SimplePool.Spawn(pdgObj, createPos.position, Quaternion.identity);
        obj.transform.SetParent(this.transform);
        Pan pan = obj.GetComponent<Pan>();
        pan.StartMove(createPos == createPosLeft);
    }

    public bool Freeze = false;
    float FreezeTime = 0;
    GameObject iceObject;
    public void OnSetPlayerFreeze()
    {
        bool protect = ModSystemController.Instance.Protecket;
        if (protect) return;
         FreezeTime = 1;
        string path = $"MOD/dingshen";
        GameObject obj = SimplePool.Spawn(videoPlayer, PlayerController.Instance.transform.position, Quaternion.identity);
        VideoManager videoManager = obj.GetComponent<VideoManager>();
        obj.transform.SetParent(Camera.main.transform);
        obj.SetActive(true); 
        videoManager.OnPlayVideo(2, path, false);
        Sound.PlaySound("Sound/Mod/Freeze");
        if(!Freeze)
        {
            Invoke("OnReadyFreeze", 0.5f);
        }
            
    }
    void OnReadyFreeze()
    {
        PlayerController.Instance.isHit = true;
        Freeze = true;
        iceObject = Instantiate(Ice);
        StartCoroutine(OnChecklayerFreeze());
    }
    IEnumerator OnChecklayerFreeze()
    {
        while (FreezeTime > 0)
        {
            FreezeTime -= Time.deltaTime;
            yield return null;
        }
       if(iceObject) Destroy(iceObject);
        FreezeTime = 0;
        Freeze = false;
        PlayerController.Instance.isHit = false;
    }

    public void OnRightLegKick()
    {
        string path = $"MOD/rightleg";
        ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(-0.4f, 0.5f), new Vector3(0.6f, 0.6f), path, "Effect");
        GameObject obj= SimplePool.Spawn(RightLeg, transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
    }
    public void OnLeftLegKick()
    {
        string path = $"MOD/leftleg";
        ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(0.8f, 1), new Vector3(0.6f, 0.6f), path, "Effect");
        GameObject obj = SimplePool.Spawn(LeftLeg, transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
    }

    public void OnLightningHit()
    {
        GameObject obj = SimplePool.Spawn(Electricity, transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
        PlayerModController.Instance.TriggerModMove(MoveDirection.Left, 0.3f, 1);
    }

    public bool rainBow = false;
    float rainBowCount = 300;

    public void OnRainbowCat()
    {
        if (!rainBow)
        {
            Sound.PlaySound("Sound/Mod/chm");
            rainBow = true;
            EventManager.Instance.SendMessage(Events.RainBowCat);
            StartCoroutine(OnRainbowCatRun());
        }
    }
    IEnumerator OnRainbowCatRun()
    {
        while (rainBowCount > 0) {
            yield return new WaitForSeconds(0.1f);
            int y = Random.Range(-8, 8);
            Vector3 dCPos = new Vector3(createPos1.position.x, y);
            GameObject obj = SimplePool.Spawn(RainBowCat, dCPos, Quaternion.identity);
            obj.transform.SetParent(transform);
            rainBowCount--;
        }
        rainBowCount = 300;
        rainBow = false;
    }

    public void OnBoomGrandma()
    {
        string path = $"MOD/BOOM";
        ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(1.2F, 0), new Vector3(0.8f, 0.8f), path, "Effect");
        GameObject obj = SimplePool.Spawn(BoomGrandema, transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
    }

    public GameObject tomatoBoom;
    public void OnCreateTomatoBoom(Vector3 pos)
    {
        GameObject obj = SimplePool.Spawn(tomatoBoom, pos, Quaternion.identity);
        obj.transform.SetParent(transform);
    }

    public void OnCreateBlackHand()
    {
        int allCount = 10;
        GameObject obj = Instantiate(blackHand);
        obj.transform.SetParent(Camera.main.transform);
        obj.transform.localPosition =new Vector3(0,0,15);
        BlackHand fastRunEffect = obj.GetComponent<BlackHand>();
        fastRunEffect.OnSetTime (allCount);
        Sound.PlaySound("Sound/Mod/hs");
    }

    public Transform flowPos3;
    public GameObject normalRocket;
    public GameObject spacilRocket;
    public GameObject Banana;
    public void OnCreateRocket()
    {
        Sound.PlaySound("Sound/Mod/daodan");
        int value = Random.Range(0, 10);
        GameObject rocket = value == 5 ? spacilRocket : normalRocket;
        float x = Random.Range(-10, 10);
        x = flowPos3.position.x + x;
        Vector3 dCPos = new Vector3(x, flowPos3.position.y);
        GameObject obj = SimplePool.Spawn(rocket, dCPos, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.SetActive(true);
    }
    public void OnCreateBanana()
    {
        Sound.PlaySound("Sound/Mod/banana");
        int value = Random.Range(0, 2);
        Transform trans = value == 0 ? createPos1 : createPos4;
        Vector3 dCPos = new Vector3(trans.position.x, trans.position.y + 8);
        GameObject obj = SimplePool.Spawn(Banana, dCPos, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.SetActive(true);
        Vector2 throu = value == 0 ? new Vector2(-150,-50) : new Vector2(160, -50);
        obj.GetComponent<Rigidbody2D>().AddForce(throu, ForceMode2D.Impulse);
    }

    public GameObject bird;
    public void OnCreateBird()
    {
        string path = $"MOD/bird";
        ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(-0.5f, 0.5f), new Vector3(0.8f, 0.8f), path, "Effect");
        GameObject obj = SimplePool.Spawn(bird, Vector3.zero, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.SetActive(true);
    }

    public GameObject billiard;
    public void OnCreateBilliard()
    {
        GameObject obj = SimplePool.Spawn(billiard, PlayerController.Instance.transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.GetComponent<Billiards>().StartShow();
        obj.SetActive(true);
    }

    public GameObject SlapFace;
    public void OnCreateSlapFace()
    {
        GameObject obj = SimplePool.Spawn(SlapFace, PlayerController.Instance.transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.GetComponent<SlapFace>().OnBeginHit();
        obj.SetActive(true);
    }
    public Transform createPos4;
    public GameObject wusaqi;
    public GameObject wusaqi2;
    public void OnCreateWuSaQi()
    {
        int value = Random.Range(0, 2);
        Sound.PlaySound("Sound/Mod/wusaqi");
        GameObject www = value == 0 ? wusaqi : wusaqi2;
        Vector3 dCPos = value == 0 ? new Vector3(createPos4.position.x, createPos4.position.y): new Vector3(createPos1.position.x, createPos1.position.y);
        GameObject obj = SimplePool.Spawn(www, dCPos, Quaternion.identity);
        obj.transform.SetParent(createPos4.transform);
        obj.GetComponent<WuSaQi>().StartMove();
        obj.SetActive(true);
    }

    public GameObject tksone;
    public GameObject tkstwo;
    public GameObject tksthree;
    public GameObject tksfour;
    public GameObject tksfive;

    public void OnCreateTKS(string callName)
    {

        GameObject tksobj = null;
        switch (callName)
        {
            case "吐口水一":
                string path1 = $"MOD/tks1";
                ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(0.2f, 1.7f), new Vector3(0.8f, 0.8f), path1, "Effect");
                tksobj = tksone;
                break;
            case "吐口水二":
                string path2 = $"MOD/tks2";
                ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(3, -2.8f), new Vector3(0.8f, 0.8f), path2, "Effect");
                tksobj = tkstwo;
                break;
            case "吐口水三":
                string path3 = $"MOD/tks3";
                ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(-4, -0.8f), new Vector3(0.8f, 0.8f), path3, "Effect");
                tksobj = tksthree;
                break;
            case "吐口水四":
                string path4 = $"MOD/tks4";
                ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(1.9f, -3), new Vector3(0.8f, 0.8f), path4, "Effect");
                tksobj = tksfour;
                break;
            case "吐口水五":
                string path5 = $"MOD/tks5";
                ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(-1.7f, -1.4f), new Vector3(0.8f, 0.8f), path5, "Effect");
                tksobj = tksfive;
                break;
        }

        GameObject obj = SimplePool.Spawn(tksobj, PlayerController.Instance.transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.SetActive(true);
        
    }
    public GameObject Huoquan;
    public void OnCreateHuoquan()
    {
        Sound.PlaySound("Sound/Mod/huoquan");
        float y = Random.Range(-1, 5);
        Vector3 cpos = new Vector3(createPos1.position.x, createPos1.position.y + y);
        GameObject obj = SimplePool.Spawn(Huoquan, cpos, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.SetActive(true);
    }
    public GameObject bannedPost;
    public void OnCreateBannedPost()
    {
        string path5 = $"MOD/jinyan";
        GameObject obj = SimplePool.Spawn(videoPlayer, PlayerController.Instance.transform.position, Quaternion.identity);
        VideoManager videoManager = obj.GetComponent<VideoManager>();
        obj.transform.SetParent(transform);
        obj.SetActive(true);
        videoManager.OnPlayVideo(2, path5,false);
    }
    public GameObject gofast;
    public void OnCreateGoFast()
    {
        string path5 = $"MOD/PKD";
        ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(-0.3f, 2.2f), new Vector3(0.8f, 0.8f), path5, "Effect");
        GameObject obj = SimplePool.Spawn(gofast, PlayerController.Instance.transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.SetActive(true);
    }
    public GameObject goback;
    public void OnCreateGoBack()
    {
        string path5 = $"MOD/TTT";
        ModVideoPlayerController.Instance.OnCreateModVideoPlayer(new Vector3(-0.3f, 2.2f), new Vector3(0.8f, 0.8f), path5, "Effect");
        GameObject obj = SimplePool.Spawn(goback, PlayerController.Instance.transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
        obj.SetActive(true);
    }

    public GameObject HangSelf;
    public bool isHang = false;
    public void OnCreateHangSelf()
    {
        PFunc.Log("上吊");
        Vector3 vectorPlayer = PlayerController.Instance.transform.position;
        Vector3 createPos = new Vector3(vectorPlayer.x, 0);
        GameObject obj = SimplePool.Spawn(HangSelf, createPos, Quaternion.identity);
        obj.transform.SetParent(transform);
        Sound.PlaySound("Sound/Mod/hangself");
        isHang = true;
        PlayerModController.Instance.OnHangSelf();
    }
}
