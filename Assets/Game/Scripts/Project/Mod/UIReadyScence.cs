using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReadyScence : MonoBehaviour
{
    public Button btn_start;
    public Button btn_Close;
    public Button btn_system;
    public GameObject systemPanel;
    private void Start()
    {
        Sound.PlayMusic("Bgm/bgm_SleepMode");
        
        btn_start.onClick.AddListener(OnClickStart);
        btn_Close.onClick.AddListener(OnClickClose);
        btn_system.onClick.AddListener(OnClickSystem);
        if (systemPanel) systemPanel.SetActive(false);
    }

    void OnClickStart()
    {
        Debug.Log("µã»÷¿ªÊ¼");
        Loaded.OnLoadScence("Assets/Game/Scenes/InitScence");
    }

    void OnClickClose()
    {
        Application.Quit();
    }
    bool systemShow = false;
    void OnClickSystem()
    {
        systemPanel.SetActive(true);
    }
}
