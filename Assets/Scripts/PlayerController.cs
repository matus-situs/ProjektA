using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharInput ci;
    private Rigidbody2D rb2d;
    private CharIsGrounded cig;
    private Animator charAnimator;
    private BoxCollider2D charCol;
    private CharIsObstructed cio;

    [SerializeField]
    private CameraFollow cf;
    private Vector2 movementDirs;

    [SerializeField]
    private float movementMultiX = 1;
    private float allowedMaxSpeed=1;
    [SerializeField]
    private float movementMultiY = 1;

    private bool mayMove = true;
    private bool isGrounded = false;

    bool lookingRight = true;

    bool isMoving = false;
    bool isIdling = true;

    bool ctrlIsBeingHeld = false;
    bool isCrouching = false;
    bool mayGetUp = true;

    Vector2 crouchOffset = new Vector2(0.005f, 0.045f);
    Vector2 crouchSize = new Vector2(0.1f,0.06f);
    Vector2 normalColSize = Vector2.zero;
    Vector2 normalColOffset= Vector2.zero;

    //private bool wantsToJump = false;

    /*
    [SerializeField]
    private Vector2 clampVel=new Vector2(4f,6f);
    private Vector2 tempVel;
    */


    // Start is called before the first frame update
    void Start()
    {
        ci = GetComponent<CharInput>();
        rb2d = GetComponent<Rigidbody2D>();
        cig = transform.GetChild(1).GetComponent<CharIsGrounded>();
        charAnimator = transform.GetChild(0).GetComponent<Animator>();
        charCol = charAnimator.transform.GetComponent<BoxCollider2D>();
        normalColSize = charCol.size;
        normalColOffset = charCol.offset;
        cio = transform.GetChild(2).GetComponent<CharIsObstructed>();
    }


    private void Update()
    {
        CrouchCheck();
        if (Input.GetButtonDown("Jump"))
            PlayerJump();

        //Debug.Log("The velocity is: "+rb2d.velocity);
        //ClampVelocity();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mayMove)
            MoveChar();
    }

    void CrouchCheck()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!ctrlIsBeingHeld)
            {
                ctrlIsBeingHeld = true;
            }
        }
        else
        {
            if (ctrlIsBeingHeld)
            {
                ctrlIsBeingHeld = false;
            }
        }

        if (!ctrlIsBeingHeld)
        {
            mayGetUp = !cio.ReturnObstructionInfo();
        }
    }

    private void MoveChar()
    {
        movementDirs = ci.GetMovementDir();
        movementDirs.y = rb2d.velocity.y;
        movementDirs.x *= allowedMaxSpeed;

        if((movementDirs.x>0.01f) && !lookingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            lookingRight = true;
            cf.ShouldFaceRight(true);
        }
        else if((movementDirs.x < -0.01f) && lookingRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            lookingRight = false;
            cf.ShouldFaceRight(false);
        }

        //Player is moving
        if (Mathf.Abs(rb2d.velocity.x) > 0.2f)
        {
            //Enter crouch state while running
            if (ctrlIsBeingHeld && !isCrouching)
            {
                isCrouching = true;
                charAnimator.SetTrigger("Crawl");
                EnterCrawlState(true);
            }
            if (!ctrlIsBeingHeld && isCrouching && mayGetUp)
            {
                isCrouching = false;
                charAnimator.SetTrigger("Run");
                EnterCrawlState(false);
            }

            //If the player isn't already moving
            if (!isMoving)
            {
                //Should player be crouching?
                if (isCrouching)
                {
                    //EnterCrouchState
                    charAnimator.SetTrigger("Crawl");
                    EnterCrawlState(true);
                    isIdling = false;
                    isMoving = true;
                }
                else
                {
                    //Regular run
                    charAnimator.SetTrigger("Run");
                    EnterCrawlState(false);
                    isIdling = false;
                    isMoving = true;
                }
            }


            charAnimator.speed = Mathf.Abs(rb2d.velocity.x);
        }
        else
        {
            //Player is standing still
            
            if (ctrlIsBeingHeld && !isCrouching)
            {
                isCrouching = true;
                charAnimator.SetTrigger("Crawl");
                EnterCrawlState(true);
                charAnimator.speed = 0;
                //isIdling = true;
            }
            if (!ctrlIsBeingHeld && isCrouching && mayGetUp)
            {
                isCrouching = false;
                charAnimator.SetTrigger("Idle");
                EnterCrawlState(false);
                charAnimator.speed = 1;
                //isIdling = true;
            }
            

            //Player not really moving
            if (!isIdling)
            {
                //Should player be crouching?
                if (isCrouching)
                {
                    charAnimator.SetTrigger("Crawl");
                    EnterCrawlState(true);
                    isIdling = true;
                    isMoving = false;
                    charAnimator.speed = 0;
                }
                else
                {
                    //Regular idle
                    isIdling = true;
                    isMoving = false;
                    charAnimator.SetTrigger("Idle");
                    EnterCrawlState(false);
                    charAnimator.speed = 1;
                }
            }
        }

        //rb2d.AddForce(Vector2.right * movementDirs.x * movementMultiX, ForceMode2D.Force);

        rb2d.velocity = movementDirs;
    }

    private void PlayerJump()
    {
        IsPlayerGrounded();
        if (isGrounded && !isCrouching)
        {
            rb2d.AddForce(Vector2.up * movementMultiY, ForceMode2D.Impulse);
        }
    }


    private void EnterCrawlState(bool should)
    {
        switch (should)
        {
            case true:
                charCol.size = crouchSize;
                charCol.offset = crouchOffset;
                allowedMaxSpeed = movementMultiX / 2;
                break;

            case false:
                charCol.size = normalColSize;
                charCol.offset = normalColOffset;
                allowedMaxSpeed = movementMultiX;
                break;
        }
    }

    /*
    private void ClampVelocity()
    {
        tempVel = rb2d.velocity;
        tempVel.x = Mathf.Clamp(tempVel.x, -clampVel.x, clampVel.x);
        tempVel.y = Mathf.Clamp(tempVel.y, -clampVel.y, clampVel.y);
        rb2d.velocity = tempVel;
    }
    */

    private void IsPlayerGrounded()
    {
        isGrounded = cig.ReturnGroundInfo();
    }

    public void PlayerMayMove(bool may)
    {
        mayMove = may;
    }


    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("I am most definatelly in a trigger");
    }
    */
}
