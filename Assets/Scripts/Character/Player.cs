using UnityEngine;

public class Player : MonoBehaviour
{
    private float moveSpeed = 1.5f;                   // Horizontal movement speed
    private float airMoveSpeed = 0.6f;                // Horizontal air movement speed when moving opposite to takeoff facing
    private float maxJumpSpeed = 3f;                  //
    private float maxJumpHeight = 25f;
    private float minJumpDeceleration = 0.02f;
    private float jumpDecelerationIncline = 0.006f; // I have no idea
    private float maxLinearDrag = 13f;
    private float minLinearDrag = 6f;
    private float fallAcceleration = 0.4f;
    private float tempAirMoveSpeed;
    private float linearDrag;
    private float jumpSpeed;
    private float jumpHeight;
    private float jumpDeceleration;

    private bool isGrounded = false;
    private bool didJump = false;
    private bool stillJumping = false;
    private bool falling = false;
    
    private Vector2 movement;
    private float horizontal, vertical;
    private byte hFacing, vFacing, takeoffDirection;
    private Animator anim;

    void Start()
    {
        jumpSpeed = maxJumpSpeed;
        jumpHeight = maxJumpHeight;
        jumpDeceleration = minJumpDeceleration;
        linearDrag = maxLinearDrag;
        hFacing = 1; // 0 = left, 1 = right
        vFacing = 0; // 0 = none, 1 = up, 2 = down
        takeoffDirection = hFacing;
        anim = GetComponent<Animator>();
    }

	void Update()
    {
        // Directional Calculation
        horizontal = Input.GetAxis("Horizontal");
        if(horizontal < 0){
            hFacing = 0;
        } else
        if(horizontal > 0){
            hFacing = 1;
        }

        // Animation Control
        anim.SetInteger("anim_state", hFacing);

        // Set Linear Drag
        rigidbody2D.drag = linearDrag;

        // Movement Calculation
        float tempJump = Input.GetAxis("Jump");
        if(isGrounded)
        {
            falling = false;
            linearDrag = maxLinearDrag;
            // Correction values in case a glitch even happens with the landing sequence
            if(jumpSpeed != maxJumpSpeed)
            {
                jumpSpeed = maxJumpSpeed;
            }

            if(jumpDeceleration != minJumpDeceleration)
            {
                jumpDeceleration = minJumpDeceleration;
            }

            // Facing Control
            takeoffDirection = hFacing;

            // Jump Calculations
            if(tempJump == 0f)
            {
                didJump = false;
            }

            if(!didJump)
            {
                if(tempJump != 0f)
                {
                    vertical = tempJump * 4f;
                    didJump = true;
                    stillJumping = true;
                }
            }
        }
        else
        {
            if(falling){
                linearDrag = Mathf.Clamp(linearDrag - fallAcceleration, minLinearDrag, maxLinearDrag);
            } else {
                if(tempJump == 0f || jumpHeight <= 0f){
                    falling = true;
                }
            }

            if(stillJumping && tempJump != 0f && jumpHeight > 0f)
            {
                jumpSpeed = Mathf.Clamp(jumpSpeed - jumpDeceleration, 0f, maxJumpSpeed);

                if(jumpHeight - tempJump < 0f)
                {
                    tempJump = tempJump - jumpHeight;
                }
                
                vertical = tempJump;
                jumpHeight -= tempJump;
                jumpDeceleration += jumpDecelerationIncline;
            }
            else
            {
                // Reset jump values
                vertical = 0f;
                jumpHeight = maxJumpHeight;
                jumpSpeed = maxJumpSpeed;
                stillJumping = false;
                jumpDeceleration = minJumpDeceleration;
            }

            tempAirMoveSpeed = Mathf.Clamp(airMoveSpeed - ((maxLinearDrag - linearDrag) / 100), 0f, airMoveSpeed);
            if(takeoffDirection == hFacing){
                horizontal = horizontal * tempAirMoveSpeed;
            } else {
                takeoffDirection = 2; // Outside the range of hFacing
                horizontal = horizontal * (tempAirMoveSpeed / 1.5f);
            }
        }
    }

    void FixedUpdate()
    {
        movement = new Vector2(horizontal * moveSpeed, vertical * jumpSpeed);
        rigidbody2D.AddForce(movement, ForceMode2D.Impulse);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        isGrounded = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        isGrounded = false;
    }
}