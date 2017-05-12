using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Vector2 camVelocity;
    private float moveSpeed;
    public float jumpStrength;
    public float dJumpStrength;
    public float dJumpModifier;
    public float baseMoveSpeed;
    public float dJumpMoveSpeed;
    public float dJumpBoost;
    public LayerMask whatIsGround;
    public bool grounded;
    public bool moving;
    public bool doubleJumped;
    public GameObject groundCheck;
    public int direction;
    public float airDamping;
    public float airSpeed;
    private Animator animator;
    public Camera mainCamera;
    public float smoothXTime;
    public float smoothYTime;

	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        float camX = mainCamera.transform.position.x;
        float offsetX = mainCamera.transform.position.x - transform.position.x;

        if (Mathf.Abs(offsetX) > 2.0f)
        {
            camX = transform.position.x + (offsetX / Mathf.Abs(offsetX)) * 2.0f;
        }

        float camY = mainCamera.transform.position.y;
        float offsetY = mainCamera.transform.position.y - transform.position.y;

        if (Mathf.Abs(offsetY) > 1.0f)
        {
            camY = transform.position.y + (offsetY/Mathf.Abs(offsetY))*1.0f;
        }

        mainCamera.transform.position = new Vector3(camX, camY, mainCamera.transform.position.z);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        SpriteRenderer model = GetComponent<SpriteRenderer>();
        if (groundCheck.GetComponent<BoxCollider2D>().IsTouchingLayers(whatIsGround))
        {
            grounded = true;
            doubleJumped = false;
            animator.SetBool("grounded", true);
            moveSpeed = baseMoveSpeed;
            //rb.velocity = new Vector2(0, 0);
        }
        else
        {
            grounded = false;
            animator.SetBool("grounded", false);
        }
        if(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow) || !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            moving = false;
            /*if (!grounded)
            {
                rb.velocity = new Vector2(rb.velocity.x * (1 - (airDamping * Time.deltaTime)), rb.velocity.y);
            }*/

            if(!grounded)
            {
                if(rb.velocity.x < 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x + airDamping * Time.deltaTime, rb.velocity.y);
                    if (rb.velocity.x > 0)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);

                    }
                }
                if (rb.velocity.x > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x - airDamping * Time.deltaTime, rb.velocity.y);
                    if (rb.velocity.x < 0)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);

                    }
                }
            }
        }
        else if(Input.GetKey(KeyCode.RightArrow))
        {
            moving = true;
            direction = 1;
            if (grounded)
            {
                
                rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
            }
            else
            {
                
                if(rb.velocity.x < moveSpeed)
                {
                    
                        rb.velocity = new Vector2(rb.velocity.x + (airSpeed * Time.deltaTime), rb.velocity.y);
                    if (rb.velocity.x > moveSpeed)
                    {
                        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                    }
                    else if (rb.velocity.x < 0)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }

            }
            
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            moving = true;
            direction = -1;
            if (grounded)
            {       
                rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
            }
            if(rb.velocity.x > -1 * moveSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x - (airSpeed * Time.deltaTime), rb.velocity.y);
                    if (rb.velocity.x < -1 * moveSpeed)
                    {
                        rb.velocity = new Vector2(-1 * moveSpeed, rb.velocity.y);
                    }
                }
            else
            {
                if (rb.velocity.x > -1 * moveSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x + (airSpeed * Time.deltaTime), rb.velocity.y);
                    if (rb.velocity.x > moveSpeed)
                    {
                        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                    }
                    else if (rb.velocity.x > 0)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                }

            }

        }
        if (Input.GetKey(KeyCode.Space))
        {
            if(grounded)
            {              
                rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
            }
            
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (!grounded && !doubleJumped)
            {
                rb.velocity = new Vector2(rb.velocity.x * dJumpBoost, dJumpStrength);
                /*
                if (rb.velocity.y < 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x * dJumpBoost, jumpStrength * dJumpModifier);
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x * dJumpBoost, rb.velocity.y + dJumpStrength);
                }
                */
                moveSpeed = dJumpMoveSpeed;
                doubleJumped = true;
            }
        }
        if(direction == 1)
        {
            model.flipX = true;
        }
        else
        {
            model.flipX = false;
        }
        animator.SetBool("moving", moving);

    }
}
