using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharIsGrounded : MonoBehaviour
{
    private bool isGrounded = false;
    public LayerMask lm1;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(((1 << collision.gameObject.layer) &lm1) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & lm1) != 0)
        {
            isGrounded = false;
        }
    }

    public bool ReturnGroundInfo()
    {
        return isGrounded;
    }
}
