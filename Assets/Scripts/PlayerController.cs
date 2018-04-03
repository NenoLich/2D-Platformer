using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;

    public float speed = 5f;
    public float jumpForce = 1000f;
    public float maxVerticalVelocity = 40f;

    private float movement;
    private Animator anim;
    private Rigidbody2D rigBody;
    private Collider2D groundCheck;
    private bool grounded;

    void Awake ()
    {
        anim = GetComponent<Animator>();
        rigBody = GetComponent<Rigidbody2D>();
        groundCheck = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.IsTouchingLayers(groundCheck, LayerMask.GetMask("Ground"));
            
        if (grounded && Input.GetButton("Jump"))
        {
            rigBody.AddForce(new Vector2(0f, jumpForce));
            anim.SetTrigger("Jump");
        }
        rigBody.velocity = rigBody.velocity.y > maxVerticalVelocity ? new Vector2(0f, maxVerticalVelocity) : rigBody.velocity;

        movement = Input.GetAxis("Horizontal");
        if (Mathf.Abs(movement) > 0.01f)
        {
            if (facingRight != movement > 0)
                Flip();

            rigBody.velocity = new Vector2(speed * movement, rigBody.velocity.y);
        }

        anim.SetBool("Run", Mathf.Abs(movement) > 0.01f && grounded);
    }

    void Update ()
    {
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
