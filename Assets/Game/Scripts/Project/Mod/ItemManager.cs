using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

            yield return new WaitForSeconds(0.1f);
        }
        isCreateDuck= false;
        allReadyCreateDuck = 0;
        allCreateDuck = 0;
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
    public void OnCreatePDG(DataInfo dataInfo)
    {
        int allDuck = dataInfo.count * dataInfo.time;

        Transform createPos = null;
        switch (dataInfo.call)
        {
            case "左边砸平底锅":
                createPos = createPosLeft;
                break;
            case "右边砸平底锅":
                createPos = createPosRight;
                break;
        }

        for (int i = 0; i < allDuck; i++)
        {
            Sound.PlaySound("Sound/Mod/PDG");
            GameObject obj = SimplePool.Spawn(pdgObj, createPos.position, Quaternion.identity);
            obj.transform.SetParent(this.transform);
            Pan pan = obj.GetComponent<Pan>();
            pan.StartMove(createPos == createPosLeft);
        }
        allDuck = 0;
    }

    public bool Freeze = false;
    float FreezeTime = 0;
    GameObject iceObject;
    public void OnSetPlayerFreeze(DataInfo dataInfo)
    {
        bool protect = ModSystemController.Instance.Protecket;
        if (protect) return;
         FreezeTime = dataInfo.count * 1;
        Sound.PlaySound("Sound/Mod/Freeze");
        if (!Freeze)
        {
            PlayerController.Instance.isHit = true;
            Freeze = true;
            iceObject=Instantiate(Ice);
            StartCoroutine(OnChecklayerFreeze());
        }
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

    public void OnRightLegKick(DataInfo dataInfo)
    {
        int allCount = dataInfo.count * dataInfo.time;
        for (int i = 0; i < allCount; i++) {
        
            GameObject obj=  Instantiate(RightLeg);
            obj.transform.SetParent(transform);
            Sound.PlaySound("Sound/Mod/rightLeg");
        }
    }
    public void OnLeftLegKick(DataInfo dataInfo)
    {
        int allCount = dataInfo.count * dataInfo.time;
        for (int i = 0; i < allCount; i++)
        {
            GameObject obj = Instantiate(LeftLeg);
            obj.transform.SetParent(transform);
            Sound.PlaySound("Sound/Mod/leftLeg");
        }
    }

    public void OnLightningHit()
    {
        GameObject obj = SimplePool.Spawn(Electricity, transform.position, Quaternion.identity);
        obj.transform.SetParent(transform);
        PlayerModController.Instance.OnKickPlayer(new Vector3(Random.Range(-3,3),10));
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

    public void OnBoomGrandma(DataInfo dataInfo)
    {
        int allCount = dataInfo.count * dataInfo.time;
        for (int i = 0; i < allCount; i++)
        {
            Sound.PlaySound("Sound/Mod/BoomGrandma");
            GameObject obj = SimplePool.Spawn(BoomGrandema, transform.position, Quaternion.identity);
            obj.transform.SetParent(transform);
        }
    }

    public GameObject tomatoBoom;
    public void OnCreateTomatoBoom(Vector3 pos)
    {
        GameObject obj = SimplePool.Spawn(tomatoBoom, pos, Quaternion.identity);
        obj.transform.SetParent(transform);
    }

    public void OnCreateBlackHand(DataInfo dataInfo)
    {
        int allCount = dataInfo.time;
        GameObject obj = Instantiate(blackHand);
        obj.transform.SetParent(Camera.main.transform);
        obj.transform.position = Vector3.zero;
        BlackHand fastRunEffect = obj.GetComponent<BlackHand>();
        fastRunEffect.OnSetTime (allCount);
        Sound.PlaySound("Sound/Mod/hs");
    }

    public Transform flowPos3;
    public GameObject normalRocket;
    public GameObject spacilRocket;
    public GameObject Banana;
    public void OnCreateRocket(DataInfo dataInfo)
    {
        int allCount = dataInfo.count * dataInfo.time;
        for (int i = 0; i < allCount; i++)
        {
            int value = Random.Range(0, 10);
            GameObject rocket = value == 5 ? spacilRocket : normalRocket;
            float x = Random.Range(-10, 10);
            x = flowPos3.position.x + x;
            Vector3 dCPos = new Vector3(x, flowPos3.position.y);
            GameObject obj = SimplePool.Spawn(rocket, dCPos, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.SetActive(true);
        }
         
    }
    public void OnCreateBanana(DataInfo dataInfo)
    {
        int allCount = dataInfo.count * dataInfo.time;
        for (int i = 0; i < allCount; i++)
        {
            int value = Random.Range(0, 2);
            Transform trans = value == 0 ? createPos1 : createPos4;
            Vector3 dCPos = new Vector3(trans.position.x, trans.position.y + 8);
            GameObject obj = SimplePool.Spawn(Banana, dCPos, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.SetActive(true);
            Vector2 throu = value == 0 ? new Vector2(-150,-50) : new Vector2(160, -50);
            obj.GetComponent<Rigidbody2D>().AddForce(throu, ForceMode2D.Impulse);
        }
    }

    public GameObject bird;
    public void OnCreateBird(DataInfo dataInfo)
    {
        int allCount = dataInfo.count * dataInfo.time;
        for (int i = 0; i < allCount; i++)
        {
            Sound.PlaySound("Sound/Mod/brid");
            GameObject obj = SimplePool.Spawn(bird, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.SetActive(true);
        }
    }

    public GameObject billiard;
    public void OnCreateBilliard(DataInfo dataInfo)
    {
        int allCount = dataInfo.count * dataInfo.time;
        for (int i = 0; i < allCount; i++)
        {
            GameObject obj = SimplePool.Spawn(billiard, PlayerController.Instance.transform.position, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.GetComponent<Billiards>().StartShow();
            obj.SetActive(true);
        }
    }

    public GameObject SlapFace;
    public void OnCreateSlapFace(DataInfo dataInfo)
    {
        int allCount = dataInfo.count * dataInfo.time;
        for (int i = 0; i < allCount; i++)
        {
            GameObject obj = SimplePool.Spawn(SlapFace, PlayerController.Instance.transform.position, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.GetComponent<SlapFace>().OnBeginHit();
            obj.SetActive(true);
        }

    }
    public Transform createPos4;
    public GameObject wusaqi;
    public GameObject wusaqi2;
    public void OnCreateWuSaQi(DataInfo dataInfo )
    {
        int allCount = dataInfo.count * dataInfo.time;

     
        for (int i = 0; i < allCount; i++)
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
    }

    public GameObject tksone;
    public GameObject tkstwo;
    public GameObject tksthree;
    public GameObject tksfour;
    public GameObject tksfive;

    public void OnCreateTKS(DataInfo dataInfo)
    {
        int allCount = dataInfo.count * dataInfo.time;

        GameObject tksobj = null;
        switch (dataInfo.call)
        {
            case "吐口水一":
                Sound.PlaySound("Sound/Mod/tks1");
                tksobj = tksone;
                break;
            case "吐口水二":
                Sound.PlaySound("Sound/Mod/tks2");
                tksobj = tkstwo;
                break;
            case "吐口水三":
                Sound.PlaySound("Sound/Mod/tks3");
                tksobj = tksthree;
                break;
            case "吐口水四":
                Sound.PlaySound("Sound/Mod/tks4");
                tksobj = tksfour;
                break;
            case "吐口水五":
                Sound.PlaySound("Sound/Mod/tks5");
                tksobj = tksfive;
                break;
        }
        for (int i = 0; i < allCount; i++)
        {
            GameObject obj = SimplePool.Spawn(tksobj, PlayerController.Instance.transform.position, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.SetActive(true);
        }
    }
    public GameObject Huoquan;
    public void OnCreateHuoquan(DataInfo dataInfo)
    {
        int allCount = dataInfo.count * dataInfo.time;
      
        for (int i = 0; i < allCount; i++)
        {
            Vector3 cpos = new Vector3(createPos1.position.x, createPos1.position.y + 1);
            GameObject obj = SimplePool.Spawn(Huoquan, cpos, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.SetActive(true);
        }
    }
    public GameObject bannedPost;
    public void OnCreateBannedPost(DataInfo dataInfo)
    {
        int allCount = dataInfo.count * dataInfo.time;

        for (int i = 0; i < allCount; i++)
        {
            Sound.PlaySound("Sound/Mod/jy");
            GameObject obj = SimplePool.Spawn(bannedPost,Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(Camera.main.transform);
            obj.transform.localPosition = new Vector3(0, 0,5);
            obj.transform.localEulerAngles = Vector3.zero;
            obj.SetActive(true);
        }
    }
    public GameObject gofast;
    public void OnCreateGoFast(DataInfo dataInfo)
    {
        int allCount = dataInfo.count * dataInfo.time;

        for (int i = 0; i < allCount; i++)
        {
            Sound.PlaySound("Sound/Mod/pkd");
            GameObject obj = SimplePool.Spawn(gofast, PlayerController.Instance.transform.position, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.SetActive(true);
        }
    }
    public GameObject goback;
    public void OnCreateGoBack(DataInfo dataInfo)
    {
        int allCount = dataInfo.count * dataInfo.time;

        for (int i = 0; i < allCount; i++)
        {
            Sound.PlaySound("Sound/Mod/ttt");
            GameObject obj = SimplePool.Spawn(goback, PlayerController.Instance.transform.position, Quaternion.identity);
            obj.transform.SetParent(transform);
            obj.SetActive(true);
        }
    }
}
