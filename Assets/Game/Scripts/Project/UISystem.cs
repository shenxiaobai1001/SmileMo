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

    private void Start()
    {
        btn_close.Click(OnClose);
        btn_back.Click(OnBackStartScence);
        btn_rest.Click(OnRestGame);
        btn_closeGame.Click(OnCloseGame);
        sl_music.onValueChanged.AddListener(OnMusicValue);
        sl_sound.onValueChanged.AddListener(OnSoundValue);
        inputField.onEndEdit.AddListener(OnInputEndEdit);
    }

    void OnMusicValue(float valuie)
    {
        Sound.VolumeMusic= sl_music.value;
        Sound.OnSetVolume(Sound.VolumeMusic, Sound.VolumeSound);
    }
    void OnSoundValue(float valuie)
    {
        Sound.VolumeSound = sl_music.value;
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
        SystemController.Instance.maxAirWallHp = maxhp;
        SystemController.Instance.airWallHp = maxhp;
    }
}
