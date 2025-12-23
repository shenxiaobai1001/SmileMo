using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BarrageNormalSetting
{
    public string CallName; // 功能名称
    public string Type; // 数据类型名称
    public string Message; // 触发内容
    public string Tip; // 提示
    public int Count; // 倍率
    public float Delay; // 延迟
}

[Serializable]
public class BarrageBoxSetting
{
    public string BoxName; // 盲盒名称
    public string Type; // 数据类型名称
    public string Message; // 触发内容
    public string Tip; // 提示
    public int Count; // 倍率
    public float Delay; // 延迟
    public List<string> Calls = new List<string>(); // 盲盒所有功能
}

public enum PrankType
{
    normal,
    box
}

public class BarrageNormalWrapper
{
    public List<BarrageNormalSetting> NormalConfigs;
}

public class BarrageBoxWrapper
{
    public List<BarrageBoxSetting> BoxConfigs;
}

public class BarrageController : MonoBehaviour
{
    public static BarrageController Instance { get; set; }

    // 功能名称
    public List<string> Calls = new List<string> ();

    [Tooltip("当前整蛊配置类型")]
    public PrankType prankType;

    public GameObject content;
    public GameObject item;
    public GameObject box;
    public List<BarrageNormalSetting> barrageNormalSetting = new List<BarrageNormalSetting>();
    public List<BarrageBoxSetting> barrageBoxSetting = new List<BarrageBoxSetting>();
    public bool isInit;

    private class ActionTask
    {
        public string user;
        public string avatar;
        public string callName;
        public int giftCount;
        public int times;
        public float delay;
    }

    private readonly Dictionary<string, Queue<ActionTask>> _queues = new Dictionary<string, Queue<ActionTask>>();
    private readonly Dictionary<string, Coroutine> _runners = new Dictionary<string, Coroutine>();
    private readonly Dictionary<string, float> _lastExec = new Dictionary<string, float>();

    public void EnqueueAction(string user, string avatar, string callName, int giftCount, int times, float delay)
    {
        if (string.IsNullOrEmpty(callName)) return;
        if (!_queues.TryGetValue(callName, out var q))
        {
            q = new Queue<ActionTask>();
            _queues[callName] = q;
        }
        int total = Mathf.Max(1, giftCount * times);
        for (int i = 0; i < total; i++)
        {
            q.Enqueue(new ActionTask
            {
                user = user,
                avatar = avatar,
                callName = callName,
                giftCount = giftCount,
                times = times,
                delay = delay
            });
        }
        if (!_runners.ContainsKey(callName) || _runners[callName] == null)
        {
            _runners[callName] = StartCoroutine(ProcessQueue(callName));
        }
    }

    private IEnumerator ProcessQueue(string callName)
    {
        var q = _queues[callName];
        while (q.Count > 0)
        {
            var task = q.Dequeue();
            float last = _lastExec.TryGetValue(callName, out var t) ? t : -1f;
            float elapsed = last < 0f ? float.MaxValue : (Time.time - last);
            float wait = Mathf.Max(0f, task.delay - elapsed);
            if (wait > 0f) yield return new WaitForSeconds(wait);

            ExecuteAction(task);
            _lastExec[callName] = Time.time;
        }
        _runners[callName] = null;
    }

    /// <summary>
    /// 执行功能
    /// </summary>
    /// <param name="task"></param>
    private void ExecuteAction(ActionTask task)
    {
        if(task.callName!= "美女盲盒")
             PlayerAutomaticSystem.Instance.OnStopAutomatic();
        switch (task.callName)
        {
            case "砸鸭子":
                CallManager.Instance.OnCreateDuckVideoPlayer();
                break;
            case "左边砸平底锅":
                ItemManager.Instance.OnCreatePDG(task.callName);
                break;
            case "右边砸平底锅":
                ItemManager.Instance.OnCreatePDG(task.callName);
                break;
            case "视角反转":
                ModSystemController.Instance.OnSetRerverseCamera();
                break;
            case "冰冻":
                ItemManager.Instance.OnSetPlayerFreeze();
                break;
            case "无敌护盾":
                ModSystemController.Instance.OnSetPlayerProtecket(task.giftCount, task.times, task.delay);
                break;
            case "左正蹬":
                ItemManager.Instance.OnLeftLegKick();
                break;
            case "右鞭腿":
                ItemManager.Instance.OnRightLegKick();
                break;
            case "麒麟臂":
                MeshCreateController.Instance.OnCreateQLBi();
                break;
            case "天残脚":
                MeshCreateController.Instance.OnCreateTCJiao();
                break;
            case "撞大运":
                MeshCreateController.Instance.OnCreateTrunck();
                break;
            case "莎士比亚":
                ModSystemController.Instance.OnShakespeare();
                break;
            case "大贝塔":
                ModSystemController.Instance.OnBigBetaForward();
                break;
            case "反向大贝塔":
                ModSystemController.Instance.OnBigBetaBack();
                break;
            case "电击":
                ItemManager.Instance.OnLightningHit();
                break;
            case "彩虹猫":
                ItemManager.Instance.OnRainbowCat();
                break;
            case "番茄连招":
                PlayerModController.Instance.OnClickToCreateTomaTo();
                break;
            case "Boom":
                ItemManager.Instance.OnBoomGrandma();
                break;
            //case "随机传送":
            //    ModSystemController.Instance.OnRandromPlayerPos();
            //    break;
            case "呸":
                ItemManager.Instance.OnCreateBlackHand();
                break;
            case "导弹":
                ItemManager.Instance.OnCreateRocket();
                break;
            case "隐身":
                PlayerModController.Instance.OnInvisibility();
                break;
            case "加速":
                PlayerModController.Instance.OnFastSpeed();
                break;
            case "减速":
                PlayerModController.Instance.OnMainSpeed();
                break;
            case "啄木鸟":
                ItemManager.Instance.OnCreateBird();
                break;
            case "砸落头像":
                ImageDownloader.Instance.OnRoleStar(task.user, task.avatar);
                break;
            case "打台球":
                ItemManager.Instance.OnCreateBilliard();
                break;
            case "大巴掌":
                ItemManager.Instance.OnCreateSlapFace();
                break;
            case "一阳指":
                MeshCreateController.Instance.OnCreateOneFinger();
                break;
            case "乌萨奇":
                ItemManager.Instance.OnCreateWuSaQi();
                break;
            case "传送第七关":
                ModSystemController.Instance.OnTransFarSeven();
                break;
            case "扔香蕉":
                ItemManager.Instance.OnCreateBanana();
                break;
            case "吐口水一":
                ItemManager.Instance.OnCreateTKS("吐口水一");
                break;
            case "吐口水二":
                ItemManager.Instance.OnCreateTKS("吐口水二");
                break;
            case "吐口水三":
                ItemManager.Instance.OnCreateTKS("吐口水三");
                break;
            case "吐口水四":
                ItemManager.Instance.OnCreateTKS("吐口水四");
                break;
            case "吐口水五":
                ItemManager.Instance.OnCreateTKS("吐口水五");
                break;
            case "动物怎么叫":
                CallManager.Instance.OnCreateCall();
                break;
            case "火圈":
                ItemManager.Instance.OnCreateHuoquan();
                break;
            case "禁言":
                ItemManager.Instance.OnCreateBannedPost();
                break;
            case "跑快点":
                ItemManager.Instance.OnCreateGoFast();
                break;
            case "退退退":
                ItemManager.Instance.OnCreateGoBack();
                break;
            case "美女盲盒":
                CallManager.Instance.OnCreateVideoPlayer("美女盲盒", 1);
                break;
            case "动感DJ":
                CallManager.Instance.OnCreateVideoPlayer("动感DJ", 2);
                break;
            case "上吊":
                ItemManager.Instance.OnCreateHangSelf();
                break;
            case "加一万米":
                SystemController.Instance.scheduleDeviation += 10000;
                break;
            case "减一万米":
                SystemController.Instance.scheduleDeviation -= 10000;
                break;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        InitializeAllConfigs();
    }

    void Update()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.transform as RectTransform);
    }

    /// <summary>
    /// 切换配置类型
    /// </summary>
    /// <param name="type"></param>
    public void ChangePrankType(int type)
    {
        isInit = false;
        prankType = (PrankType)type;
        LoadDataFromJson();
        if (type == (int)PrankType.normal)
        {
            RemoveAllItem();
            InitNormalConfig();
        }
        else
        {
            RemoveAllItem();
            InitBoxConfig();
        }
        SaveDataToJson();
    }

    /// <summary>
    /// 添加配置
    /// </summary>
    public void AddItem()
    {
        if(prankType == PrankType.normal)
        {
            GameObject obj = Instantiate(item, content.transform);
            Dropdown dropdown = obj.transform.GetChild(1).GetComponent<Dropdown>();

            dropdown.ClearOptions();
            dropdown.AddOptions(Calls);

            BarrageNormalSetting config = new BarrageNormalSetting();
            config.CallName = dropdown.options[dropdown.value].text;
            config.Count = 1;
            barrageNormalSetting.Add(config);
        }
        else
        {
            GameObject obj = Instantiate(box, content.transform);

            BarrageBoxSetting config = new BarrageBoxSetting();
            config.Count = 1;
            barrageBoxSetting.Add(config);
        }
    }

    /// <summary>
    /// 清空配置
    /// </summary>
    public void RemoveAllItem()
    {
        foreach(Transform obj in content.transform)
        {
            Destroy(obj.gameObject);
        }
    }

    /// <summary>
    /// 初始化所有配置（加载或创建默认配置）
    /// </summary>
    public void InitializeAllConfigs()
    {
        Debug.Log("开始初始化配置...");

        try
        {
            string configDir = Path.Combine(Directory.GetCurrentDirectory(), "Config");
            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
                Debug.Log($"创建配置目录: {configDir}");
            }

            //// 2. 初始化普通配置
            //InitializeNormalConfig(configDir);

            //// 3. 初始化盲盒配置
            //InitializeBoxConfig(configDir);

            Debug.Log("所有配置初始化完成");
        }
        catch (Exception ex)
        {
            Debug.LogError($"初始化配置失败: {ex.Message}");
        }
    }

    /// <summary>
    /// 保存配置到本地JSON
    /// </summary>
    public void SaveDataToJson()
    {
        BarrageNormalWrapper wrapper = new BarrageNormalWrapper();
        wrapper.NormalConfigs = barrageNormalSetting;

        string filePath1 = Path.Combine(Directory.GetCurrentDirectory(),"Config" , "NormalData.json");

        string jsonData1 = JsonUtility.ToJson(wrapper, true); 

        File.WriteAllText(filePath1, jsonData1);

        Debug.Log("普通配置数据已保存到: " + filePath1);

        BarrageBoxWrapper barrageBoxWrapper = new BarrageBoxWrapper();
        barrageBoxWrapper.BoxConfigs = barrageBoxSetting;

        string filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "Config", "BoxData.json");
        string jsonData2 = JsonUtility.ToJson(barrageBoxWrapper, true);

        File.WriteAllText(filePath2, jsonData2);

        Debug.Log("盲盒配置数据已保存到: " + filePath2);
    }

    /// <summary>
    /// 读取本地JSON数据
    /// </summary>
    public void LoadDataFromJson()
    {
        if(prankType == PrankType.normal)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Config", "NormalData.json");

            if (!File.Exists(filePath))
            {
                Debug.LogWarning("未找到配置文件: " + filePath);
                return;
            }

            try
            {
                string jsonData = File.ReadAllText(filePath);

                BarrageNormalWrapper wrapper = JsonUtility.FromJson<BarrageNormalWrapper>(jsonData);
                barrageNormalSetting = wrapper.NormalConfigs;

                Debug.Log($"成功加载 {wrapper.NormalConfigs.Count} 条普通配置数据");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"加载失败: {e.Message}");
            }
        }
        else
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Config", "BoxData.json");
            if (!File.Exists(filePath))
            {
                Debug.LogWarning("未找到配置文件: " + filePath);
                return;
            }

            try
            {
                string jsonData = File.ReadAllText(filePath);

                BarrageBoxWrapper wrapper = JsonUtility.FromJson<BarrageBoxWrapper>(jsonData);
                barrageBoxSetting = wrapper.BoxConfigs;

                Debug.Log($"成功加载 {wrapper.BoxConfigs.Count} 条盲盒配置数据");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"加载失败: {e.Message}");
            }
        }

    }

    /// <summary>
    /// 初始化普通配置item
    /// </summary>
    public void InitNormalConfig()
    {
        for (int i = 0; i < barrageNormalSetting.Count; i++)
        {
            GameObject itemObj = Instantiate(item, content.transform);

            Dropdown dropdown1 = itemObj.transform.GetChild(1).GetComponent<Dropdown>();
            dropdown1.ClearOptions();
            dropdown1.AddOptions(Calls);
            Dropdown dropdown2 = itemObj.transform.GetChild(2).GetComponent<Dropdown>();

            itemObj.transform.GetChild(3).GetComponent<InputField>().text = barrageNormalSetting[i].Message;
            itemObj.transform.GetChild(5).GetComponent<InputField>().text = barrageNormalSetting[i].Tip;
            itemObj.transform.GetChild(7).GetComponent<InputField>().text = barrageNormalSetting[i].Count.ToString();
            itemObj.transform.GetChild(9).GetComponent<InputField>().text = barrageNormalSetting[i].Delay.ToString();

            ChoiceCall(dropdown1, barrageNormalSetting[i].CallName);
            ChoiceCall(dropdown2, barrageNormalSetting[i].Type);

        }
        isInit = true;
    }

    /// <summary>
    /// 初始化盲盒配置box
    /// </summary>
    public void InitBoxConfig()
    {
        RemoveAllItem();
        for (int i = 0; i < barrageBoxSetting.Count; i++)
        {
            GameObject itemObj = Instantiate(box, content.transform);
            GameObject lineObj = itemObj.transform.GetChild(0).gameObject;
            Dropdown dropdown = lineObj.transform.GetChild(2).GetComponent<Dropdown>();


            lineObj.transform.GetChild(1).GetComponent<InputField>().text = barrageBoxSetting[i].BoxName;
            lineObj.transform.GetChild(3).GetComponent<InputField>().text = barrageBoxSetting[i].Message;
            lineObj.transform.GetChild(5).GetComponent<InputField>().text = barrageBoxSetting[i].Tip;
            lineObj.transform.GetChild(7).GetComponent<InputField>().text = barrageBoxSetting[i].Count.ToString();
            lineObj.transform.GetChild(9).GetComponent<InputField>().text = barrageBoxSetting[i].Delay.ToString();

            ChoiceCall(dropdown, barrageBoxSetting[i].Type);
        }
        isInit = true;
    }


    public void ChoiceCall(Dropdown dropdown, string name)
    {
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            if(dropdown.options[i].text == name)
            {
                dropdown.value = i;
                return;
            }
        }
    }


    /// <summary>
    /// 执行功能
    /// </summary>
    /// <param name="callName"></param>
    public IEnumerator CallFunction(string user, string avatar, string callName, int giftCount, int times, float delay)
    {
        PlayerAutomaticSystem.Instance.OnStopAutomatic();
        for (int i = 0; i < giftCount * times; i++)
        {
            switch (callName)
            {
                case "砸鸭子":
                    CallManager.Instance.OnCreateDuckVideoPlayer();
                    break;
                case "左边砸平底锅":
                    ItemManager.Instance.OnCreatePDG(callName);
                    break;
                case "右边砸平底锅":
                    ItemManager.Instance.OnCreatePDG(callName);
                    break;
                case "视角反转":
                    ModSystemController.Instance.OnSetRerverseCamera();
                    break;
                case "冰冻":
                    ItemManager.Instance.OnSetPlayerFreeze();
                    break;
                case "无敌护盾":
                    ModSystemController.Instance.OnSetPlayerProtecket(giftCount, times, delay);
                    break;
                case "左正蹬":
                    ItemManager.Instance.OnLeftLegKick();
                    break;
                case "右鞭腿":
                    ItemManager.Instance.OnRightLegKick();
                    break;
                case "麒麟臂":
                    MeshCreateController.Instance.OnCreateQLBi();
                    break;
                case "天残脚":
                    MeshCreateController.Instance.OnCreateTCJiao();
                    break;
                case "撞大运":
                    MeshCreateController.Instance.OnCreateTrunck();
                    break;
                case "莎士比亚":
                    ModSystemController.Instance.OnShakespeare();
                    break;
                case "大贝塔":
                    ModSystemController.Instance.OnBigBetaForward();
                    break;
                case "反向大贝塔":
                    ModSystemController.Instance.OnBigBetaBack();
                    break;
                case "电击":
                    ItemManager.Instance.OnLightningHit();
                    break;
                case "彩虹猫":
                    ItemManager.Instance.OnRainbowCat();
                    break;
                case "番茄连招":
                    PlayerModController.Instance.OnClickToCreateTomaTo();
                    break;
                case "Boom":
                    ItemManager.Instance.OnBoomGrandma();
                    break;
             //   case "随机传送":
                   // ModSystemController.Instance.OnRandromPlayerPos();
                   // break;
                case "呸":
                    ItemManager.Instance.OnCreateBlackHand();
                    break;
                case "导弹":
                    ItemManager.Instance.OnCreateRocket();
                    break;
                case "隐身":
                    PlayerModController.Instance.OnInvisibility();
                    break;
                case "加速":
                    PlayerModController.Instance.OnFastSpeed();
                    break;
                case "减速":
                    PlayerModController.Instance.OnMainSpeed();
                    break;
                case "啄木鸟":
                    ItemManager.Instance.OnCreateBird();
                    break;
                case "砸落头像":
                    ImageDownloader.Instance.OnRoleStar(user, avatar);
                    break;
                case "打台球":
                    ItemManager.Instance.OnCreateBilliard();
                    break;
                case "大巴掌":
                    ItemManager.Instance.OnCreateSlapFace();
                    break;
                case "一阳指":
                    MeshCreateController.Instance.OnCreateOneFinger();
                    break;
                case "乌萨奇":
                    ItemManager.Instance.OnCreateWuSaQi();
                    break;
                case "传送第七关":
                    ModSystemController.Instance.OnTransFarSeven();
                    break;
                case "扔香蕉":
                    ItemManager.Instance.OnCreateBanana();
                    break;
                case "吐口水一":
                    ItemManager.Instance.OnCreateTKS("吐口水一");
                    break;
                case "吐口水二":
                    ItemManager.Instance.OnCreateTKS("吐口水二");
                    break;
                case "吐口水三":
                    ItemManager.Instance.OnCreateTKS("吐口水三");
                    break;
                case "吐口水四":
                    ItemManager.Instance.OnCreateTKS("吐口水四");
                    break;
                case "吐口水五":
                    ItemManager.Instance.OnCreateTKS("吐口水五");
                    break;
                case "动物怎么叫":
                    CallManager.Instance.OnCreateCall();
                    break;
                case "火圈":
                    ItemManager.Instance.OnCreateHuoquan();
                    break;
                case "禁言":
                    ItemManager.Instance.OnCreateBannedPost();
                    break;
                case "跑快点":
                    ItemManager.Instance.OnCreateGoFast();
                    break;
                case "退退退":
                    ItemManager.Instance.OnCreateGoBack();
                    break;
                case "美女盲盒":
                    CallManager.Instance.OnCreateVideoPlayer("美女盲盒", 1);
                    break;
                case "动感DJ":
                    CallManager.Instance.OnCreateVideoPlayer("动感DJ", 2);
                    break;
                case "上吊":
                    ItemManager.Instance.OnCreateHangSelf();
                    break;
            }

            yield return new WaitForSeconds(delay);
        }
    }
}