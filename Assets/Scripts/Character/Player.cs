using UnityEngine;

public class Player : MonoBehaviour
{
    private float moveSpeed = 1.5f;
    private float airMoveSpeed = 0.666f;
    private float jumpSpeed = 5f;
    private float jumpDeceleration = 0.25f;
    private float jumpHeight = 10f;
    private float maxJumpSpeed = 5f;
    private float maxJumpHeight = 10f;

    private bool isGrounded = false;
    private bool didJump = false;
    private bool stillJumping = false;
    
    private Vector2 movement;
    private float horizontal, vertical;

	void Update()
    {
        float tempJump = Input.GetAxis("Jump");
        if(isGrounded)
        {
            if (tempJump == 0f)
            {
                didJump = false;
            }

            if (!didJump)
            {
                if (tempJump != 0f)
                {
                    vertical = tempJump;
                    didJump = true;
                    stillJumping = true;
                }
            }
            
            horizontal = Input.GetAxis("Horizontal");
        }
        else
        {
            if(stillJumping && tempJump != 0f && jumpHeight > 0f)
            {
                jumpSpeed = Mathf.Clamp(jumpSpeed - jumpDeceleration, 0f, maxJumpSpeed);

                if(jumpHeight - tempJump < 0f)
                {
                    tempJump = tempJump - jumpHeight;
                }
                
                vertical = tempJump;
                jumpHeight -= tempJump;
            }
            else
            {
                // Reset jump values
                vertical = 0f;
                jumpHeight = maxJumpHeight;
                jumpSpeed = maxJumpSpeed;
                stillJumping = false;
            }

            horizontal = Input.GetAxis("Horizontal") * airMoveSpeed;
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