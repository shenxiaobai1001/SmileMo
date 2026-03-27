using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTurtleHead : MonoBehaviour
{
    public GreenTurtle greenTurtle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        PFunc.Log("GreenTurtleHead", collision.gameObject.tag);
        if (collision.gameObject.CompareTag("PlayerFoot"))
        {
            greenTurtle.OnToShell();
            PlayerController.Instance.OnArrowUp(6);
        }
    }
}
