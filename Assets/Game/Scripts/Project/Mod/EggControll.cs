using UnityEngine;

public class EggController : MonoBehaviour
{
    private bool hasBeenDespawned;
    //private Rigidbody2D rb;
    //private bool wasOnScreen;

    //void Awake() => rb = GetComponent<Rigidbody2D>();

    void OnEnable()
    {
        hasBeenDespawned = false;
        //wasOnScreen = false;
        //if (EggManager.InstanceExists)
        //    EggManager.Instance.Register(this);
    }

    void Update()
    {
        if (transform.position.y < -12 && !hasBeenDespawned)
        {
            hasBeenDespawned = true;
            GameObject thisObj = gameObject;
            //if (EggManager.InstanceExists)
            //    EggManager.Instance.Unregister(this); // 只有这里才注销
            SimplePool.Despawn(thisObj);
            return;
        }
    }

    //void OnDisable()
    //{
    //    if (EggManager.InstanceExists)
    //        EggManager.Instance.Unregister(this);
    //}

    //public Vector3 GetPosition() => transform.position;

    //public void SetOnScreenState(bool onScreen)
    //{
    //    if (wasOnScreen == onScreen) return;
    //    wasOnScreen = onScreen;

    //    PFunc.Log(onScreen ? "回到屏幕" : "离开屏幕");

    //    if (!onScreen)
    //    {
    //        // 离屏：只冻结，不移除！管理器会继续跟踪它
    //        if (rb != null)
    //        {
    //            rb.velocity = Vector2.zero;
    //            rb.isKinematic = true;
    //        }
    //    }
    //    else
    //    {
    //        // 回屏：解冻，重力会自动生效
    //        if (rb != null) rb.isKinematic = false;
    //    }
    //}
}