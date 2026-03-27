using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTurtleShellHead : MonoBehaviour
{
    public GreenTurtle greenTurtle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.CompareTag("PlayerFoot"))
        {
            greenTurtle.OnToShell();
            PlayerController.Instance.OnArrowUp(6);
        }
    }
}
