using RenderHeads.Media.AVProVideo;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class VideoManager : MonoBehaviour
{
    public MediaPlayer mainPlayer;//播放器
    public DisplayUGUI displayUGUI;
    public DisplayUGUI candisplayUGUI;
    public Canvas mCanvas;

    bool snakeScene = false;
    void Start()
    {
        mainPlayer.Events.AddListener(OnVideoEvent);         // 订阅播放器本身提供的事件
        mCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        mCanvas.worldCamera = Camera.main;
        mCanvas.sortingLayerName = "Effect";  // Sorting Layer 名称
        mCanvas.sortingOrder = 0;         // Order in Layer

    }
    int videoType = 0;
    public void OnPlayVideo(int type, string title, bool snake = true)
    {
        videoType = type;
        pathTitle = title;
        displayUGUI.gameObject.SetActive(videoType == 1);
        candisplayUGUI.gameObject.SetActive(videoType == 2);
        snakeScene = snake;
        OnBeginGetVideo();
    }

    private void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType evt, ErrorCode errorCode)
    {
        PFunc.Log("VideoPlayerState", evt);

        //当视频加载完毕开始播放视频
        if (evt == MediaPlayerEvent.EventType.FirstFrameReady)
        {
            OnShow();
        }

        // 当视频暂停
        if (evt == MediaPlayerEvent.EventType.Paused)
        {
        
        }

        // 当视频播放完
        if (evt == MediaPlayerEvent.EventType.FinishedPlaying)
        {
            OnEnd();
        }
    }
    //播放
    public void OnShow()
    {
        Sound.PauseOrPlayVolumeMusic(true);
        mainPlayer.Play();
        mainPlayer.Control.SetVolume(Sound.VideoVolume);
        if (snakeScene)
        {
            EventManager.Instance.SendMessage(Events.BeginSnakeMap, true);
        }
    }

    //跳到结束
    public void OnEnd()
    {
        Sound.PauseOrPlayVolumeMusic(false);
        EventManager.Instance.SendMessage(Events.BeginSnakeMap, false);
        EventManager.Instance.SendMessage(Events.OnVideoPlayEnd);
        mainPlayer.CloseMedia();
        SimplePool.Despawn(this.gameObject);
    }

    string pathTitle = "";
    /// <summary>本地视频启用</summary>
    private async void OnBeginGetVideo()
    {
        string videoData = await OnGetPlayBytes(pathTitle);
        if (mainPlayer)
        {
            mainPlayer.gameObject.SetActive(true);
            mainPlayer.OpenMedia(MediaPathType.AbsolutePathOrURL, videoData, false);
        }
    }
    /// <summary> 获取本地视频数据 </summary>
    public async Task<string> OnGetPlayBytes(string path)
    {
        string video = await Loaded.OnLoadVideoAsync(path);
        return video;
    }
}
