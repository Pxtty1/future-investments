using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("BASIC MOVEMENT")] 
    public float speed;
    public float jumpPower;
    public int numExtraJump;
    [Header("DASH")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;

    Rigidbody2D rb;
    float inputx;
    bool facingRight;
    bool isGrounded;
    public LayerMask whatIsGround;
    public Transform groundcheck;
    int extraJumpsLeft;
    bool isDashing;
    bool canDash;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumpsLeft = numExtraJump;
        canDash = true;
    }

    private void Update()
    {
        if (isDashing == true)
        {
            return;
        }
        inputx = Input.GetAxisRaw("Horizontal");

        if (facingRight == true && inputx == -1)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z);
            facingRight = false;
        }
        else if (facingRight == false && inputx == 1)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, -180f, transform.rotation.z);
            facingRight = true;
        }

        isGrounded = Physics2D.OverlapCircle(groundcheck.position, .5f, whatIsGround);

        if (isGrounded == true)
        {
            extraJumpsLeft = numExtraJump;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded == true)
            {
                rb.velocity = Vector2.up * jumpPower;
            }
            else if (isGrounded == false && extraJumpsLeft > 0)
            {
                rb.velocity = Vector2.up * jumpPower;
                extraJumpsLeft--;
            }
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(inputx * dashSpeed, rb.velocity.y);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(inputx * speed, rb.velocity.y);
    }
}
