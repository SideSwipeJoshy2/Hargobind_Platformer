using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PlayerController;

public class PlayerController : MonoBehaviour
{


    public enum DashDirection
    {
        left, right, none
            }
    float timer;
    float dashDura;
    public DashDirection direction;


    public enum FacingDirection
    {
        left, right
    }
    public FacingDirection currentFacingDirection = FacingDirection.right;

    public enum CharacterState
    {
        idle, walk, jump, die
    }
    public CharacterState currentCharacterState = CharacterState.idle;
    public CharacterState previousCharacterState = CharacterState.idle;

 


    public float accelerationTime;
    public float decelerationTime;
    public float maxSpeed;
    public float ApexHeight;
    public float ApexTime;
    private float jumpTime;
    public float terminalSpeed = 3;
    public float magnitudeCap = 3f;
  public  float dashSpeed = 30;
    float dashH = 3;
    
    public int health = 10;
    private float dashCalc;
    private Rigidbody2D playerRB;
    private float acceleration;
    private bool isJumping = false;

    public LayerMask groundLayer;
    public Transform wallCheck;
    private bool isClimbing;
    private bool climbC;
   public float wallSpeed = 0.5f;
    
    Vector2 force;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        acceleration = maxSpeed / accelerationTime;
        jumpTime = ApexHeight / ApexTime;
        direction = DashDirection.none;

    }

    // Update is called once per frame
    void Update()
    {

        previousCharacterState = currentCharacterState;
        isClimbing = Physics2D.OverlapBox(wallCheck.position, new Vector2(2f, 0.5f), 0, groundLayer);

        if (IsGrounded() && Input.GetKeyDown(KeyCode.UpArrow))
        {
            isJumping = true;
        }

        if (isClimbing)
        {
            climbC = true;
            Debug.Log("eorking?");
        }
        else
        {
            climbC = false;

        }

        switch (currentCharacterState)
        {
            case CharacterState.die:

                break;
            case CharacterState.jump:

                if (IsGrounded())
                {
                    //We know we need to make a transition because we're not grounded anymore
                    if (IsWalking())
                    {
                        currentCharacterState = CharacterState.walk;
                    }
                    else
                    {
                        currentCharacterState = CharacterState.idle;
                    }
                }

                break;
            case CharacterState.walk:
                if (!IsWalking())
                {
                    currentCharacterState = CharacterState.idle;
                }
                //Are we jumping?
                if (!IsGrounded())
                {
                    currentCharacterState = CharacterState.jump;
                }
                break;
            case CharacterState.idle:
                //Are we walking?
                if (IsWalking())
                {
                    currentCharacterState = CharacterState.walk;
                }
                //Are we jumping?
                if (!IsGrounded())
                {
                    currentCharacterState = CharacterState.jump;
                }

                break;
        }

    }

    private void FixedUpdate()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerInput += Vector2.left;
            direction = DashDirection.none;
            
            
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerInput += Vector2.right;
            direction = DashDirection.none;
        }

        if(climbC)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, Mathf.Clamp(playerRB.velocity.y, wallSpeed, float.MaxValue));
            if (Input.GetKey(KeyCode.Space))
            {
                 playerRB.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
                

            }
        }
        
        
          if (isJumping){

            playerRB.velocity = Vector2.up * dashH;
        
        }

       Dash();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump();
        }
     

        
        MovementUpdate(playerInput);
    }

    private void Dash()
    {

        if (direction == DashDirection.left)
        {
            playerRB.velocity = Vector2.left * dashSpeed;
        }

        if (direction == DashDirection.right)
        {
            playerRB.velocity += Vector2.right * dashSpeed;

        }

        if (Input.GetKey(KeyCode.D))
        {
            direction = DashDirection.left;
            playerRB.velocity = Vector2.left * dashSpeed;


        }
        if (Input.GetKey(KeyCode.G))
        {
            direction = DashDirection.right;
            playerRB.velocity += Vector2.right * dashSpeed;

        }

    }
            private void jump()
    {
        float y = Input.GetAxis("Vertical");

        Vector2 movement = new Vector3(0.0f, y, 0.0f);
        playerRB.velocity = movement.normalized * maxSpeed;

        
    }


    private void MovementUpdate(Vector2 playerInput)
    {
        Vector2 velocity = playerRB.velocity;
        Debug.Log(playerInput.ToString());
        if (playerInput.x != 0)
        {
            velocity += playerInput * acceleration * Time.fixedDeltaTime;
        }
        else
        {
            velocity = new Vector2(0, velocity.y);
        }

        playerRB.velocity = velocity;
    }

    public bool IsWalking()
    {
        return false;
    }
    public bool IsGrounded()
    {
        return true;
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public void OnDeathAnimationComplete()
    {

    }
    public FacingDirection GetFacingDirection()
    {
        if (playerRB.velocity.x > 0)
        {
            currentFacingDirection = FacingDirection.right;
        }
        else if (playerRB.velocity.x < 0)
        {
            currentFacingDirection = FacingDirection.left;
        }

        return currentFacingDirection;
    }

    
        
}




