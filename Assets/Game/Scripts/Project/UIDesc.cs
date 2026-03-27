using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDesc : MonoBehaviour
{
    public static UIDesc Instance;
    private void Awake()
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

    string[] value=new string[]
    {
        "干嘛","挂了吧","有事没事","什么身份给我打电话","有屁快放","别墨迹","少罗嗦","没看忙吗","不想听"
    };
    public GameObject txdesc;

    public Transform down;
    public Transform up;

   public void OnCreateHit()
    {
        int index=Random.Range(0,value.Length);

        GameObject obj = SimplePool.Spawn(txdesc, down.position,Quaternion.identity);
        obj.transform.parent = transform;
        obj.transform.localScale = new Vector3(0.3f,0.3f,1);
        obj.GetComponent<Text>().text = value[index];
        obj.SetActive(true);
        obj.transform.DOMoveY(up.position.y, 0.75f).onComplete += () => { SimplePool.Despawn(obj); };
    }

}
