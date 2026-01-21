using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIChain : MonoBehaviour
{
    public static UIChain Instance;
    public List<GameObject> gameObjects;
    public GameObject center;
    public Text tx_number;
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
        center.SetActive(false);
    }

    public void OnStartMove()
    {
        bool protect = ModSystemController.Instance.Protecket;
        if (protect) {
            Invoke("OnChekcMinZero",0.1f);
        }
        else
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].gameObject.SetActive(false);
            }
            center.SetActive(true);
        }
           
    }
    // Update is called once per frame
    void Update()
    {
        if (Config.chainCount <= 0) return;
        if (ModSystemController.Instance.Protecket)
        {
            OnChekcMinZero();
        }
        if (Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.J))
        {
            Sound.PlaySound("Sound/Mod/paopao");
            OnRande();
            Config.chainCount--;
            if (Config.chainCount <= 0)
            {
                OnChekcMinZero();
                return;
            }
            if (ChainPlayer.Instance != null)
            {
                ChainPlayer.Instance.transform.DOShakePosition(0.5f, 0.2f)
           .SetEase(Ease.OutQuad)
           .OnComplete(() => {
               ChainPlayer.Instance.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
           });
            }
        }
        tx_number.text = $"{Config.chainCount}";
    }

   public void OnChekcMinZero()
    {
        ItemManager.Instance.lockPlayer = false;
        Config.chainCount = 0;
        center.SetActive(false);
        if (ChainPlayer.Instance != null)
        {
            SimplePool.Despawn(ChainPlayer.Instance.gameObject);
        }
        PlayerModController.Instance.OnSetspriteTrans(true);
        PlayerModController.Instance.OnChangeState(true);
    }

    public void OnRande()
    {
        int value = UnityEngine.Random.Range(0, gameObjects.Count);
        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].gameObject.SetActive(value == i);
        }
    }
}
