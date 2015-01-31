using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 1.5f;                     // Horizontal movement speed
    private float airMoveSpeed = 0.6f;                  // Horizontal air movement speed when moving opposite to takeoff facing
    private float airMoveSpeedOffset = 1.5f;            // Value which we divide the air move speed by when the player changes directions in air
    private float maxJumpSpeed = 3f;                    // Base jumping speed, a.k.a. the base vertical movement speed
    private float maxJumpHeight = 25f;                  // A figurative gas tank which controls pressing and holding for jump
    private float minJumpDeceleration = 0.02f;          // The baseline for how much jump juice is taken out of the tank each frame
    private float jumpDecelerationIncline = 0.006f;     // Controlls the incline of juice taken out of the jump tank each frame
    private float maxLinearDrag = 13f;                  // Maximum amount of linear drag and also the grounded baseline for it
    private float minLinearDrag = 6f;                   // Minimum amound of linear drag allowed
    private float fallAcceleration = 0.4f;              // Value used to offset the player's linear drag as he is falling
    private float initialJumpBoost = 4.3f;              // Value used to boost the initial jump value to give the jump a poppy feel
    private float tempAirMoveSpeed;                     // Value used to hold the current calculated air horizontal move speed
    private float linearDrag;                           // Current linear drag of the player object
    private float jumpSpeed;                            // Current vertical movement speed of the player
    private float jumpHeight;                           // Current amount of jump juice in the jump gas tank
    private float jumpDeceleration;                     // Current amount of jump juice being removed from the tank each frame

    private bool isGrounded = false;                    // Is the player on solid standing ground
    private bool didJump = false;                       // Did the player just perform a jump action
    private bool stillJumping = false;                  // If a jump was perform and the player lets go of jump or runs out of gas this is false
    private bool falling = false;                       // In any curcumstance where the player is falling this is true
    
    private Vector2 movement;                           // Temp variable to hold the current horizontal and vertical movement of the player
    private float horizontal, vertical;                 // Temp variables to hold either horizontal or vertical movement base values
    private Direction hFacing, vFacing, takeoffDirection;    // Values storing the current horizontal, vertical and jump takeoff directions
                                                        // hFacing: 0 = left, 1 = right
                                                        // vFacing: 0 = none, 2 = up, 3 = down
                                                        // takeoffDirection: 0 = left, 1 = right

    private enum Direction { Left, Right, Up, Down };   // Values for cardinal direction value definitions
    private Animator anim;                              // Value holding the players animation object for easy editing later
    private int currentAnimation = 1;                   // Current animation with idle right facing set by default

    void Start()
    {
        // Set base values for all of our variables
        jumpSpeed = maxJumpSpeed;
        jumpHeight = maxJumpHeight;
        jumpDeceleration = minJumpDeceleration;
        linearDrag = maxLinearDrag;
        hFacing = Direction.Right;
        vFacing = Direction.Left;
        takeoffDirection = hFacing;
        anim = GetComponent<Animator>();
    }

	void Update()
    {
        // Grab the cardinal direction movement inputs
        horizontal = Input.GetAxis("Horizontal");
        float tempJump = Input.GetAxis("Jump");

        // Set the horizontal direction the player is currently facing
        if(horizontal < 0){
            hFacing = Direction.Left;
        } else
        if(horizontal > 0){
            hFacing = Direction.Right;
        }

        // Calculate the current horizontal and vertical movements based on the players grounded state
        if(isGrounded)
        {
            // If the player is grounded then they can't be falling, ensure we set that to false
            falling = false;

            // If the player is grounded then we definitely don't want any lighter linear drag values, reset that
            linearDrag = maxLinearDrag;

            // If for any reason the jump speed was not reset, ensure that when the player is grounded that value is reset
            if(jumpSpeed != maxJumpSpeed)
            {
                jumpSpeed = maxJumpSpeed;
            }

            // If for any reason the jump deceleration value was not reset, ensure that when the player is grounded it's always correct
            if(jumpDeceleration != minJumpDeceleration)
            {
                jumpDeceleration = minJumpDeceleration;
            }

            // If the player is grounded then we need their current direction for takeoff to always be accurate and ready to go
            takeoffDirection = hFacing;

            // If the jump button is not being pressed then ensure the didJump value is always false
            if(tempJump == 0f)
            {
                didJump = false;
            }

            // If the user is not currently jumping then we can check the jump key
            if(!didJump)
            {

                // If the jump key is being pressed then set the required values for a jump
                if(tempJump != 0f)
                {
                    // The initial jump vertical value is boosted to give the jump a pop up from the start
                    vertical = tempJump * initialJumpBoost;

                    // Since a jump is being intiated we need to set the jump control booleans to true
                    didJump = true;
                    stillJumping = true;
                }
            }
        }
        else
        {
            // If the player is falling we want to boost their fall speed by gradiantly controlling the linear drag of the player
            if(falling){

                // Basically the current linear drag is decreased by the fall acceleration to a minimum value, boosting terminal velocity
                linearDrag = Mathf.Clamp(linearDrag - fallAcceleration, minLinearDrag, maxLinearDrag);
            } else {

                // If the player is not falling then check the states in which the player would logically be falling
                if(tempJump == 0f || jumpHeight <= 0f){

                    // If the player should logically be falling, set that state to true so the linear drag can be modified
                    falling = true;
                }
            }

            // As long as the player is holding the jump button from the start of the jump we should be boosting that jump
            // farther into the air. But if the jumpHeight gas tank is empty or the player let go of the button in a
            // previous frame then we shouldn't add any additional boost to the jump
            if(stillJumping && tempJump != 0f && jumpHeight > 0f)
            {

                // Decrease the current vertical movement based on the jump deceleration amount.
                jumpSpeed = Mathf.Clamp(jumpSpeed - jumpDeceleration, 0f, maxJumpSpeed);

                // If the jump height would more than empty the jump boost ensure that we clamp that value to exactly empty.
                // This is super important to ensure jumps always have the same amount of boost no matter what. If this value
                // was even partially off, high precision jumps would be hard to predict for the player
                if(jumpHeight - tempJump < 0f)
                {
                    tempJump -= jumpHeight;
                }
                
                // Set the vertical movement value
                vertical = tempJump;

                // Remove jump juice from the jump gas tank
                jumpHeight -= tempJump;

                // Increase the amount of jump juice which will be removed from the jump gas tank in the next frame
                jumpDeceleration += jumpDecelerationIncline;
            } else {
                
                // Reset jump values
                vertical = 0f;
                jumpHeight = maxJumpHeight;
                jumpSpeed = maxJumpSpeed;
                stillJumping = false;
                jumpDeceleration = minJumpDeceleration;
            }

            // The move speed in the air should be offset as the linear drag decreases. If we didn't adjust this value
            // then the player would speed up incrementally horizontally as they fell through the air.
            tempAirMoveSpeed = Mathf.Clamp(airMoveSpeed - ((maxLinearDrag - linearDrag) / 100), 0f, airMoveSpeed);

            // If the player is still facing the same direction as when they jumped then their move speed in the air
            // should remain normal. Otherwise the jump would feel slower than the run speed the instant the player jumped.
            if(takeoffDirection == hFacing){

                // Given the player doesn't have the friction of running on the ground we still need to slow down the player
                // a bit which in the air in order to match the running speed on the ground.
                horizontal = horizontal * tempAirMoveSpeed;
            } else {

                // If the player reversed their direction while in the air then ensure we change to a slower in-air horizontal
                // move speed until the player touches the ground again. Changing the takeoff direction ensures that this happens.
                takeoffDirection = Direction.Up;
                horizontal = horizontal * (tempAirMoveSpeed / airMoveSpeedOffset);
            }
        }

        // Set the correct current animation to be played
        CalculateAndSetCurrentAnimation();

        // Set Linear Drag of the player to the current calculated linear drag
        rigidbody2D.drag = linearDrag;
    }

    void FixedUpdate()
    {
        // Calculate the final cardinal direction movements for this frame
        movement = new Vector2(horizontal * moveSpeed, vertical * jumpSpeed);

        // Apply that cardinal force to the players physics object for this frame
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

    void CalculateAndSetCurrentAnimation()
    {
        // Calculate the animation which should play based on the players current state
        if(hFacing == Direction.Left){
            currentAnimation = 0;
        } else
        if(hFacing == Direction.Right){
            currentAnimation = 1;
        }

        // Set on the animation object the correct current animation
        anim.SetInteger("anim_state", currentAnimation);
    }
}