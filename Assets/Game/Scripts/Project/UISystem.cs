using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    public GameObject btn_close;
    public GameObject btn_back;
    public GameObject btn_rest;
    public GameObject btn_closeGame;

    public Slider sl_music;
    public Slider sl_sound;

    public TMP_InputField inputField;
    public TMP_InputField inputFieldName;

    public Toggle tg_file;

    private void OnEnable()
    {
        btn_close.Click(OnClose);
        btn_back.Click(OnBackStartScence);
        btn_rest.Click(OnRestGame);
        btn_closeGame.Click(OnCloseGame);
        sl_music.onValueChanged.AddListener(OnMusicValue);
        sl_sound.onValueChanged.AddListener(OnSoundValue);
        inputField.onEndEdit.AddListener(OnInputEndEdit);
        inputFieldName.onEndEdit.AddListener(OnInputNameEndEdit);
        tg_file.onValueChanged.AddListener(OnToggleFiled);
        sl_music.value = (float)Sound.VolumeMusic / (float)1;
        sl_sound.value = (float)Sound.VolumeSound / (float)1;
        if(SystemController.Instance!=null)
        {
            if (SystemController.Instance.maxAirWallHp != 0)
            {
                inputField.text = SystemController.Instance.maxAirWallHp.ToString();
            }

            inputField.text = Config.NameValue;
        }
        tg_file.isOn = Config.isFileAdd;
        Config.isSYSTEM = true;
    }

    private void OnDisable()
    {
        Config.isSYSTEM = false;
    }
    void OnMusicValue(float valuie)
    {
        Sound.VolumeMusic= sl_music.value;
        Sound.OnSetVolume(Sound.VolumeMusic, Sound.VolumeSound);
    }
    void OnSoundValue(float valuie)
    {
        Sound.VolumeSound = sl_sound.value; 
        Sound.OnSetVolume(Sound.VolumeMusic, Sound.VolumeSound);
    }
    void OnClose()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.isSystemPanel = false;
        }
        gameObject.SetActive(false);
    }
    void OnBackStartScence()
    {
        Loaded.OnLoadScence("Assets/Game/Scenes/ReadyScence");
    }
    void OnRestGame()
    {
        Loaded.OnLoadScence("Assets/Game/Scenes/InitScence");
    }

    void OnCloseGame()
    {
        Application.Quit();
    }
    void OnInputEndEdit(string value)
    {
        if (value == string.Empty) return;

        int maxhp=int.Parse(value);
        SystemController.Instance.OnSetAirwallHp(maxhp, maxhp);
        EventManager.Instance.SendMessage(Events.AirWallStateChange, maxhp > 0);
    }
    void OnInputNameEndEdit(string value)
    {
        if (value == string.Empty) return;

        SystemController.Instance.OnSetPlayerName(value);
        EventManager.Instance.SendMessage(Events.OnPlayerNameChange);
    }
    void OnToggleFiled(bool value)
    {
        Config.isFileAdd = value;
        PlayerPrefs.SetInt("isFileAdd", Config.isFileAdd?0:1);
    }
}
