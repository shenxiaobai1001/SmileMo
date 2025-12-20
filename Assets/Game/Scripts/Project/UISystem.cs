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

    private void OnEnable()
    {
        btn_close.Click(OnClose);
        btn_back.Click(OnBackStartScence);
        btn_rest.Click(OnRestGame);
        btn_closeGame.Click(OnCloseGame);
        sl_music.onValueChanged.AddListener(OnMusicValue);
        sl_sound.onValueChanged.AddListener(OnSoundValue);
        inputField.onEndEdit.AddListener(OnInputEndEdit);
        PFunc.Log(Sound.VolumeMusic, Sound.VolumeSound);
        PFunc.Log((float)Sound.VolumeMusic / (float)1, (float)Sound.VolumeSound / (float)1);
        sl_music.value = (float)Sound.VolumeMusic / (float)1;
        sl_sound.value = (float)Sound.VolumeSound / (float)1;
        if (SystemController.Instance.maxAirWallHp != 0)
        {
            inputField.text = SystemController.Instance.maxAirWallHp.ToString();
        }
       
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
}
