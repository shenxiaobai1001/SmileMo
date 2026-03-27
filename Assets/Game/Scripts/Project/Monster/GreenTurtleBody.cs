using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenTurtleBody : MonoBehaviour
{
    public GreenTurtle greenTurtle;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.CompareTag("MoveShell"))
        {
            greenTurtle.Die();
        }
    }
}
