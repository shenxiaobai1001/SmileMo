using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrageCallPhone : MonoBehaviour
{
    public Button btn_click;
    public Transform animator;

    Color startColor = new Color(255, 0, 115, 255);
    Color endColor = new Color(0, 255, 19, 255);
    // 屏幕边距，防止弹幕出现在屏幕边缘
    public float screenMargin = 2;
    public float diction = 2;
    private void Awake()
    {

        btn_click.onClick.AddListener(OnClick);
    }

    private void OnEnable()
    {
        SetRandomPosition();
        StartCoroutine(RollObjects());
    }


    public GameObject[] objects;  // 物体数组
    public float switchTime = 1f; // 切换间隔时间

    private int currentIndex = 0; // 当前激活的物体索引

    private void Start()
    {
        
    }

    IEnumerator RollObjects()
    {
        while (true)
        {
            // 关闭所有物体
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(false);
            }

            // 开启当前物体
            objects[currentIndex].SetActive(true);

            // 计算下一个物体索引
            currentIndex ++;
            if (currentIndex >= objects.Length)
            {
                currentIndex=0;
            }

            // 等待指定时间
            yield return new WaitForSeconds(switchTime);
        }
    }
    private void SetRandomPosition()
    {
        if (animator == null) return;

        // 计算屏幕可用的随机位置范围
        float randomX = Random.Range(screenMargin, Screen.width - screenMargin);
        float randomY = Random.Range(screenMargin, Screen.height - screenMargin);

        // 将屏幕坐标转换为世界坐标
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(randomX, randomY, 10f));
        worldPosition.z = 0; // 如果是2D，确保z坐标为0

        // 设置animator的位置
        animator.position = worldPosition;


        // 设置按钮位置与animator重合
        if (btn_click != null)
        {
            // 否则使用世界坐标
            btn_click.transform.position = animator.position;
        }
    }

    private void OnClick()
    {
        UIDesc.Instance.OnCreateHit();
        StopCoroutine(RollObjects());
        SimplePool.Despawn(gameObject);
    }

    private void OnDestroy()
    {
        if (btn_click != null)
        {
            btn_click.onClick.RemoveListener(OnClick);
        }
    }
}