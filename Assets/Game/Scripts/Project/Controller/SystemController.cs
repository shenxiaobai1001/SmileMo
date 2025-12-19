using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}
