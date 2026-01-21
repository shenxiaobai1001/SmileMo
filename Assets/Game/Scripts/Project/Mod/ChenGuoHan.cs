using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // 需要导入DOTween

public class ChenGuoHan : MonoBehaviour
{
    public GameObject chenGuoHan;
    public PolygonCollider2D polygonCollider;
    public GameObject tanwu;
    public Transform trans;
    public Animator animator;
    public float Speed = 10;
    bool canMove = false;

    // 记录初始位置和缩放
    private Vector3 originalPosition;
    private Vector3 originalScale;

    private void Awake()
    {
        if(tanwu) tanwu.SetActive(false);
        // 记录初始位置和缩放
        originalPosition = chenGuoHan.transform.localPosition;
        originalScale = chenGuoHan.transform.localScale;
    }

    public void OnStartMove()
    {
        if (animator)
        {
            animator.Rebind();
            animator.Update(0f);
        }
        tanwu.SetActive(false);
        polygonCollider.enabled = false;    
        // 还原初始位置和缩放
        chenGuoHan.transform.localPosition = originalPosition;

        chenGuoHan.transform.localScale = originalScale;

        canMove = true;
        StartCoroutine(OnMoveToTargetPos());
    }

    void Update()
    {
        // 可以保留原来的Update逻辑
    }

    IEnumerator OnMoveToTargetPos()
    {
        // 第一步：挤压动画
        yield return StartCoroutine(SqueezeAnimation());

        // 第二步：抛物线移动
        yield return StartCoroutine(ParabolicMovement());

        // 第三步：到达目标位置后的挤压
        yield return StartCoroutine(FinalSqueeze());
    }

    IEnumerator SqueezeAnimation()
    {
        // 使用DOTween进行Y轴挤压动画
        Sequence squeezeSequence = DOTween.Sequence();

        // 先缩小到0.6
        squeezeSequence.Append(chenGuoHan.transform.DOScale(new Vector3(1,0.6f,1), 0.3f)
            .SetEase(Ease.OutBack));

        // 再还原
        squeezeSequence.Append(chenGuoHan.transform.DOScale(new Vector3(1, 1, 1), 0.3f)
            .SetEase(Ease.OutBack));

        yield return squeezeSequence.WaitForCompletion();
    }

    IEnumerator ParabolicMovement()
    {
        // 计算目标位置（水平移动6.5单位）
        Vector3 startPos = chenGuoHan.transform.localPosition;
        Vector3 targetPos = startPos + Vector3.left * 6.5f; // 向左移动

        // 抛物线参数
        float jumpHeight = 4;
        float duration = 6.5f / Speed; // 根据速度和距离计算时间

        // 记录时间
        float timeElapsed = 0f;

        // 调试：记录起始位置
        Debug.Log($"抛物线开始 - 起始本地坐标: {startPos}");
        Debug.Log($"抛物线目标位置: {targetPos}");

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration);

            // 水平移动（线性）
            float xPos = Mathf.Lerp(startPos.x, targetPos.x, t);

            // 垂直移动（抛物线方程：y = -4h * t^2 + 4h * t）
            // 这个公式在 t=0 和 t=1 时 y=0，在 t=0.5 时 y=h
            float yOffset = -4f * jumpHeight * t * t + 4f * jumpHeight * t;
            float yPos = startPos.y + yOffset;

            // 调试：查看计算过程
            if (timeElapsed < Time.deltaTime || timeElapsed > duration - Time.deltaTime)
            {
                Debug.Log($"t={t:F3}, yOffset={yOffset:F3}, yPos={yPos:F3}");
            }

            // 应用新位置
            chenGuoHan.transform.localPosition = new Vector3(xPos, yPos, startPos.z);

            yield return null;
        }

        // 确保到达精确的目标位置
        chenGuoHan.transform.localPosition = targetPos;

        // 调试：检查最终位置
        Debug.Log($"抛物线结束 - 最终本地坐标: {chenGuoHan.transform.localPosition}");
        Debug.Log($"期望Y值: {startPos.y}, 实际Y值: {chenGuoHan.transform.localPosition.y}");
    }

    IEnumerator FinalSqueeze()
    {
        polygonCollider.enabled = true;
        // 到达目标位置后再次挤压到0.6
        yield return chenGuoHan.transform.DOScale(new Vector3(1, 0.6f, 1), 0.3f)
            .SetEase(Ease.OutBack)
            .WaitForCompletion();
        tanwu.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        tanwu.SetActive(false);
        SimplePool.Despawn(gameObject);
    }
}