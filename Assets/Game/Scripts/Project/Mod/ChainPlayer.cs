
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainPlayer : MonoBehaviour
{
    public static ChainPlayer Instance;
    public Animator animator;
    void Awake()
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
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddListener(Events.OnLazzerHit,OnLazzerHit);
    }
    private void OnEnable()
    {
        if (!ModSystemController.Instance.Protecket)
        {
            PlayerModController.Instance.OnSetspriteTrans(false);
            PlayerModController.Instance.OnChangeState(false);
            PlayerController.Instance.transform.position = new Vector3(animator.transform.position.x,
                animator.transform.position.y, animator.transform.position.z);
        }
    }

    void OnLazzerHit(object msg)
    {
        animator.SetTrigger("smallLockLazzer");
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(Events.OnLazzerHit, OnLazzerHit);
    }
}
