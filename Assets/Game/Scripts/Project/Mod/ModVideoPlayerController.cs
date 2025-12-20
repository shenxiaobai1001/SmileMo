using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModVideoPlayerController : MonoBehaviour
{
    public static ModVideoPlayerController Instance;
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

    public GameObject ModVideoPlayer;

    public bool IsPlaying =false;

    private void Start()
    {
        EventManager.Instance.AddListener(Events.OnVideoPlayEnd, OnVideoPlayEnd);
    }

    public void OnCreateModVideoPlayer(Vector3 offset, Vector3 scale, string path, string layer = "Video",  bool snake = false)
    {
        GameObject vplayerObj = SimplePool.Spawn(ModVideoPlayer, transform.position, Quaternion.identity);
        ModVideoPlayer vplayer = vplayerObj.GetComponent<ModVideoPlayer>();
        vplayer.OnPlayVideo(offset, scale,path, layer,snake);
        vplayerObj.transform.SetParent(this.transform);
        IsPlaying = true;
    }

    void OnVideoPlayEnd(object msg)
    {
        IsPlaying = false;
    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(Events.OnVideoPlayEnd, OnVideoPlayEnd);
    }
}
