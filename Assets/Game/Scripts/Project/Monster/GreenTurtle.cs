using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GreenTurtle : MonoBehaviour
{
    public Animator animator;
    [Header("移动参数")]
    [SerializeField] private float normalMoveSpeed = 2f;
    [SerializeField] private float shellMoveSpeed = 5f;

    [Header("检测设置")]
    [SerializeField] private float groundCheckDistance = 1;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;

    [Header("碰撞体设置")]
    [SerializeField] private Collider2D bodyCollider;  // 身体碰撞体（伤害玩家）
    [SerializeField] private Collider2D headCollider;  // 头部碰撞体（被踩检测）
    [SerializeField] private GameObject shell;  // 头部碰撞体（被踩检测）
    [SerializeField] private GameObject moveShell;  // 头部碰撞体（被踩检测）
    [SerializeField] private SpriteRenderer spriteRenderer;  // 头部碰撞体（被踩检测）

    private Rigidbody2D rb;
    private Vector2 velocity;
    private int moveDirection = 1; // 1向右，-1向左

    public enum TurtleState
    {
        None,
        Moving,      // 正常移动状态
        ShellStatic, // 龟壳静止状态
        ShellMoving, // 龟壳移动状态
        Dead         // 死亡状态
    }
    public TurtleState currentState = TurtleState.None;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false; // 使用自定义物理

    }

    private void Update()
    {
        if (transform.position.y <= -6)
        {
            Die();
        }
        if (currentState == TurtleState.Dead) return;

        HandleStateBehavior();
    }
    float shellTime = 0;
    private void HandleStateBehavior()
    {
        if (shellTime >= 60)
        {
            Die();
        }
        switch (currentState)
        {
            case TurtleState.Moving:
                transform.Translate(new Vector3(moveDirection, 0) * normalMoveSpeed * Time.deltaTime);
                CheckForTurn();
                break;
            case TurtleState.ShellMoving:
                shellTime += Time.deltaTime;
              
                transform.Translate(new Vector3(moveDirection, 0) * shellMoveSpeed * Time.deltaTime);
                CheckForTurn();
                break;
        }

        // 应用重力
       // velocity.y -= gravity * Time.deltaTime;
    }

    private void CheckForTurn()
    {
        // 射线检测前方是否有地面
        Vector2 checkDirection = moveDirection > 0 ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheckPoint.position,
            checkDirection,
            groundCheckDistance,
            groundLayer
        );

 
        // 如果没有检测到地面，则转向
        if (hit.collider != null)
        {
            moveDirection *= -1;
            spriteRenderer.flipX = moveDirection == 1;
        }
    }

    public void OnToShell()
    {

        moveDirection = 0;
        velocity = Vector2.zero;
        currentState = TurtleState.ShellStatic;
        animator.SetTrigger("Die_b");
        shell.SetActive(true);
        moveShell.SetActive(false);
        bodyCollider.gameObject.SetActive(false);
        headCollider.gameObject.SetActive(false);
    }

    public void OnToShellMove()
    {
        Transform moveVec = PlayerController.Instance.spriteTrans;
        moveDirection = moveVec.localScale.x > 0 ? 1 : -1;
        currentState = TurtleState.ShellMoving;
        gameObject.tag = "MoveShell";
        Invoke("OnShowMoveShell",0.5f);
    }

    void OnShowMoveShell()
    {
        shell.SetActive(false);
        moveShell.SetActive(true);
        bodyCollider.gameObject.SetActive(false);
        headCollider.gameObject.SetActive(false);
    }

    public void Die()
    {
        if (currentState == TurtleState.Dead) return;
        Sound.PlaySound("smb_kick");
        animator.SetTrigger("Die_b");
        currentState = TurtleState.Dead;
        velocity = Vector2.zero;

        shell.SetActive(false);
        moveShell.SetActive(false);
        bodyCollider.gameObject.SetActive(false);
        headCollider.gameObject.SetActive(false);

        spriteRenderer.flipY = true;
        Vector3 dropDir = moveDirection==1 ? new Vector3(5, 5, 0) : new Vector3(-5, 5, 0);
        rb.AddForce(dropDir, ForceMode2D.Impulse);
        // 死亡接口
        Invoke("DestroyTurtle", 2f);
    }

    public void OnStart()
    {
        shellTime = 0;
        spriteRenderer.flipY = false;
        moveDirection=Random.Range(0, 2)==0?1:-1;
        spriteRenderer.flipX = moveDirection == 1;

        shell.SetActive(false);
        moveShell.SetActive(false);
        bodyCollider.gameObject.SetActive(true);
        headCollider.gameObject.SetActive(true);
        currentState = TurtleState.Moving;
    }

    private void DestroyTurtle()
    {
        SimplePool.Despawn(gameObject);
    }

    // 调试绘制
    private void OnDrawGizmos()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheckPoint.position,
                groundCheckPoint.position + (Vector3)(Vector2.right * moveDirection * groundCheckDistance));
        }
    }
}