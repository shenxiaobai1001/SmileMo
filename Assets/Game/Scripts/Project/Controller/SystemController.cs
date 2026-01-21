using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class SystemController : MonoBehaviour
{
    public static SystemController Instance; 
    
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
    [HideInInspector]
    public int maxAirWallHp;
    [HideInInspector]
    public int airWallHp;
    public bool airWallContin = false;

    public int scheduleDeviation;

    private void Start()
    {
        OnGetData();
    }
    public void OnGetData()
    {
        float VolumeMusic = 1;
        float VolumeSound = 1;
        if (PlayerPrefs.HasKey("VolumeMusic")) 
            VolumeMusic = PlayerPrefs.GetFloat("VolumeMusic");
        if (PlayerPrefs.HasKey("VolumeSound"))
            VolumeSound = PlayerPrefs.GetFloat("VolumeSound");
        Sound.OnSetVolume(VolumeMusic, VolumeSound);

        if (PlayerPrefs.HasKey("maxAirWallHp"))
            maxAirWallHp = PlayerPrefs.GetInt("maxAirWallHp");
        airWallHp = maxAirWallHp;

        if (PlayerPrefs.HasKey("NameValue"))
            Config.NameValue = PlayerPrefs.GetString("NameValue");

        if (PlayerPrefs.HasKey("isFileAdd"))
            Config.isFileAdd = PlayerPrefs.GetInt("isFileAdd") == 0;
        airWallContin = airWallHp > 0;
        EventManager.Instance.SendMessage(Events.AirWallStateChange, airWallContin);
        EventManager.Instance.SendMessage(Events.OnPlayerNameChange);
    }

    public void OnSetAirwallHp(int max,int now)
    {
        maxAirWallHp = max;
        airWallHp = now;
        airWallContin = airWallHp > 0;
        PlayerPrefs.SetInt("maxAirWallHp",maxAirWallHp);
    }
    public void OnSetPlayerName(string value)
    {
        Config.NameValue=value;
        PlayerPrefs.SetString("NameValue", value);
    }
    public void OnSetFile()
    {
        PlayerPrefs.SetInt("isFileAdd", Config.isFileAdd ? 0 : 1);
    }
    public void OnSetWallHp(int now)
    {
        if (airWallHp <= 0) return;
        airWallHp -= now;
        airWallContin = airWallHp > 0;
        EventManager.Instance.SendMessage(Events.AirWallStateChange, airWallContin);
    }
}
