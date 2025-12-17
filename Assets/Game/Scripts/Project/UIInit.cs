using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class UIInit : MonoBehaviour
{
    public Slider loadingSlider;      // 进度条
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(OnInit());
    }

    IEnumerator OnInit()
    {
        loadingSlider.value = 0;

        // 初始化音乐
        yield return OnInitMusic();
        loadingSlider.value = 0.25f;

        // 加载场景
        SceneHandle load = Loaded.OnLoadScence("Assets/Game/Scenes/Main");

        // 不要在这里yield return，而是用while循环检查加载状态
        while (!load.IsDone)
        {
            // 这里使用load.Progress，注意它是0-1的值
            // 但YOOAsset的Progress可能在0-0.9之间
            float progress = load.Progress;

            // 确保在0-1范围内
            float displayProgress = Mathf.Clamp01(progress);

            // 映射到合适的进度范围（0.25-1.0）
            loadingSlider.value = 0.25f + (displayProgress * 0.75f);

            // 每帧更新一次
            yield return null;
        }

        loadingSlider.value = 1f;

        // 等待场景完全激活
        yield return new WaitForSeconds(0.1f);
    }
    string level1 = "Bgm/bgm_0101world";
    string level2 = "Bgm/bgm_ElectroBully";
    string level4 = "Bgm/bgm_ElectroScream";
    string level5 = "Bgm/bgm_FollowMe";
    string level6 = "Bgm/bgm_Akatuski";
    string level7 = "Bgm/bgm_hybrid";
    IEnumerator OnInitMusic()
    {
        List<string> list = new List<string>();
        list.Add(level1); list.Add(level2); list.Add(level4); list.Add(level5); list.Add(level6); list.Add(level7);
        for (int i = 0; i < list.Count; i++)
        {
            string path = list[i];
            var clipau = Loaded.Load<AudioClip>(path);
            if (clipau!=null)
            {
                Sound.musicBGM.Add(path, clipau);
            }
            yield return null;
        }

    }
}
