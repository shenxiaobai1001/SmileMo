using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePlayer : MonoBehaviour
{
    public Animator _playerAnim;
   public Rigidbody2D _playerRb;

    bool _isOnGround;

    private void Start()
    {
        EventManager.Instance.AddListener(Events.OnLazzerHit, OnLazzerHit);
    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(Events.OnLazzerHit, OnLazzerHit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isOnGround)
        {
            OnJumpAuto();
        }
    }
    public void OnJumpAuto(bool auto = false)
    {
        _isOnGround = false;
        _playerRb.velocity = Vector3.zero;
        OnJump();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("RopeGround"))
        {
            _isOnGround = true;
            _playerAnim.SetBool("Jump", false);
        }
    }

    void OnLazzerHit(object msg)
    {
        _playerAnim.SetTrigger("RopeDemage");
    }

    void OnJump()
    {
        Sound.PlaySound("Sound/PlayerJump");
        _playerAnim.SetBool("Jump", true);
        _playerRb.gravityScale = 3.5f;
        _playerRb.velocity = new Vector2(_playerRb.velocity.x, 4); // 设置初始跳跃速度
    }
}
