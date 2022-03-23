using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharIsObstructed : MonoBehaviour
{
    private bool isObstructed = false;
    public LayerMask lm1;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & lm1) != 0)
        {
            isObstructed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & lm1) != 0)
        {
            isObstructed = false;
        }
    }

    public bool ReturnObstructionInfo()
    {
        return isObstructed;
    }
}
