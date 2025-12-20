using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWallCollider : MonoBehaviour
{
    public static AirWallCollider Instance;
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

    public bool hasPlayer = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
