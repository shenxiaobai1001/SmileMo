using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class Normal : MonoBehaviour
{
    private BarrageController barrageConfig;

    void Awake()
    {
        barrageConfig = FindAnyObjectByType<BarrageController>();
    }


    void Update()
    {
        
    }

    public void Remove()
    {
        Destroy(gameObject);
        barrageConfig.barrageNormalSetting.RemoveAt(transform.GetSiblingIndex());
    }

    /// <summary>
    /// 修改配置
    /// </summary>
    public void ChangeConfig()
    {
        if(barrageConfig.isInit)
        {
            BarrageNormalSetting barrageNormalSetting = barrageConfig.barrageNormalSetting[transform.GetSiblingIndex()];

            foreach (Transform child in transform.transform)
            {
                if (child.gameObject.name == "Dropdown1") barrageNormalSetting.CallName = child.gameObject.GetComponent<Dropdown>().options[child.gameObject.GetComponent<Dropdown>().value].text;
                if (child.gameObject.name == "Dropdown2") barrageNormalSetting.Type = child.gameObject.GetComponent<Dropdown>().options[child.gameObject.GetComponent<Dropdown>().value].text;
                if (child.gameObject.name == "InputField1") barrageNormalSetting.Message = child.gameObject.GetComponent<InputField>().text;
                if (child.gameObject.name == "InputField2") barrageNormalSetting.Tip = child.gameObject.GetComponent<InputField>().text;
                if (child.gameObject.name == "InputField3") barrageNormalSetting.Count = int.Parse(child.gameObject.GetComponent<InputField>().text);
                if (child.gameObject.name == "InputField4") barrageNormalSetting.Delay = int.Parse(child.gameObject.GetComponent<InputField>().text);
            }
        }
    }

    /// <summary>
    /// 测试功能
    /// </summary>
    public void TestCall()
    {
        Dropdown dropdown = transform.GetChild(1).gameObject.GetComponent<Dropdown>();
        string callName = dropdown.options[dropdown.value].text;

        InputField inputField1 = transform.GetChild(7).gameObject.GetComponent<InputField>();
        int times = int.Parse(inputField1.text);
        InputField inputField2 = transform.GetChild(9).gameObject.GetComponent<InputField>();
        float delay = int.Parse(inputField2.text);

        BarrageController.Instance.StartCoroutine(BarrageController.Instance.CallFunction("测试用户", "", callName, 1, times, delay));
    }
}
