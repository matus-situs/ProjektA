using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharInput : MonoBehaviour
{
    //Left-Right moves
    private float movementX = 0;
    //Down-Up moves
    private float movementY = 0;

    private Vector2 tempMovement = Vector2.zero;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetPlayerInputs();
    }


    private void GetPlayerInputs()
    {
        movementX = Input.GetAxis("Horizontal");
        movementY = Input.GetAxis("Vertical");
    }

    public Vector2 GetMovementDir()
    {
        tempMovement.Set(movementX, movementY);
        return tempMovement;
    }
}
