using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReadyScence : MonoBehaviour
{
    public GameObject btn_start;
    public GameObject btn_Close;
    public GameObject btn_system;
    public GameObject systemPanel;
    private void Start()
    {
        Sound.PlayMusic("Bgm/bgm_SleepMode");
        Debug.Log("UIReadyScence");
        Debug.Log("btn_start:" + (btn_start==null));
        Debug.Log("btn_Close:"+( btn_Close == null));
        Debug.Log("btn_system:"+ (btn_system == null));
        if (btn_start) btn_start.Click(OnClickStart);
        if (btn_Close) btn_Close.Click(OnClickClose);
        if (btn_system) btn_system.Click(OnClickSystem);
        if (systemPanel) systemPanel.SetActive(false);
    }

    public void OnClickStart()
    {
        Debug.Log("µã»÷¿ªÊ¼");
        Loaded.OnLoadScence("Assets/Game/Scenes/InitScence");
    }

    public void OnClickClose()
    {
        Application.Quit();
    }
    bool systemShow = false;
    public void OnClickSystem()
    {
        systemPanel.SetActive(true);
    }
}
