using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJingLi : MonoBehaviour
{
    public GameObject image;

    private void Start()
    {
        EventManager.Instance.AddListener(Events.OnJingLi,OnShowImage);
        if (image) image.SetActive(false);
    }
    public void OnShowImage(object msg)
    {
        if (image)
        {
            if (image) image.SetActive(true);
            image.transform.localScale = new Vector3(1.3f,1.3f,1);
            image.transform.DOScale(1, 0.3f);

        }
        PlayerModController.Instance.OnTiggerJingli(true);
        Invoke("OnCloseImage",1);
    }

    public void OnCloseImage()
    {
        if (image) image.SetActive(false);
        PlayerModController.Instance.OnTiggerJingli(false);
    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(Events.OnJingLi, OnShowImage);
    }
}
