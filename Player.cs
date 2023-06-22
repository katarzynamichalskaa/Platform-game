using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody2D rigidbodyComponent;
    private bool grounded;
    private float jumpForce = 5f;
    private int forwardForce = 3;
    private bool jumped;
    private bool tripleJump;
    private int BoostTimeDuration = 5;
    private bool isWallTouch;
    private bool isSliding;
    private bool wallJumping;

    [SerializeField] private float wallSlidingSpeed; 
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float wallJumpDuration;
    [SerializeField] private Vector2 wallJumpForce;

    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody2D>();

    }
    void Update()
    {

        isWallTouch = Physics2D.OverlapBox(wallCheck.position, new Vector2(0.07f, 0.96f), 0, groundLayer);
        grounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(1.11f, 0.65f), 0, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (grounded)
            {
                Jump();
                jumped = true;
            }

            else if (jumped)
            {
                Jump();
                jumped = false;
            }

            else if (tripleJump)
            {
                Jump();
                tripleJump = false;
            }

            else if (isSliding)
            {
                wallJumping = true;
                Invoke("StopWallJumping", wallJumpDuration);
            }
        }

        horizontalInput = Input.GetAxis("Horizontal");


        if (isWallTouch && !grounded && horizontalInput != 0)
        {
            isSliding = true;
        }
        else
        {
            isSliding = false;
        }


    }

    void Jump()
    {
        rigidbodyComponent.velocity = Vector2.up * jumpForce;
    }

    void StopWallJumping()
    {
        wallJumping = false;
    }

    private void FixedUpdate()
    {
        

        if(isSliding)
        {
            rigidbodyComponent.velocity = new Vector2(rigidbodyComponent.velocity.x, Mathf.Clamp(rigidbodyComponent.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }

        if(wallJumping)
        {
            rigidbodyComponent.velocity = new Vector2(horizontalInput * wallJumpForce.x, wallJumpForce.y);
        }

        else
        {
            rigidbodyComponent.velocity = new Vector2(horizontalInput * forwardForce, rigidbodyComponent.velocity.y);
        }
    }


    IEnumerator SpeedBooster()
    {

        forwardForce = 5;
        yield return new WaitForSeconds(BoostTimeDuration);
        forwardForce = 3;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
            tripleJump = true;
        }

        if (other.gameObject.layer == 8)
        {
            Destroy(other.gameObject);
            StartCoroutine(SpeedBooster());

        }
    }


}
