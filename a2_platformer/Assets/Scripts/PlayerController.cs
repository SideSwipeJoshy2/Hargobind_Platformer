using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection
    {
        left, right
    }
    public float timeToReachMaxSpeed;
    public float speed;
    public float timeToDeaccel;


    private float accel;
    private float deaccel;

    public FacingDirection direction;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        accel = speed / timeToReachMaxSpeed;
        deaccel = speed / timeToDeaccel;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        MovementUpdate(playerInput);
        GetFacingDirection();
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        Vector2 currentVelo = rb.velocity;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentVelo += accel * Vector2.right * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentVelo += accel * Vector2.left * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentVelo += accel * Vector2.up * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentVelo += accel * Vector2.down * Time.deltaTime;
        }

        rb.velocity = currentVelo;

    }

        public bool IsWalking()
    {
        if (speed > 0)
        {
          return true;
        }
        else
        {
     return false;
     }
    }
    public bool IsGrounded()
    {
        return true;
    }

    public FacingDirection GetFacingDirection()
    {


        if(direction == FacingDirection.left)
        {
            return FacingDirection.left;
        }
        else if(direction == FacingDirection.right)
        {
            return FacingDirection.right;
          
        }
        return direction;
    }


}
