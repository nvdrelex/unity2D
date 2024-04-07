using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    // start variables
    private Rigidbody2D rb;
    private Animator animator;
    private Collider2D coll;

  

    //FSM thay doi moi thu dua tren thanh tra
    private enum State { idle, running, jumping, falling, hurt }
    private State state = State.idle;
   


    // Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int cherries = 0;
    [SerializeField] private Text cherryText;
    [SerializeField] private float hurtForce = 10f;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (state != State.hurt)
        {
            Movement();
        }
       

        AnimaltionState();
        animator.SetInteger("state", (int)state);  // thiet lap hoat anh dua tren trang thai

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            cherryText.text = cherries.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" )
        {
            if( state == State.falling)
            {
                Destroy(other.gameObject);
                jump();
            }
            else
            {
                state = State.hurt;
                if(other.gameObject.transform.position.x  > transform.position.x)
                {
                    // enemy is to my right therefore i should be damaged and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    // enemy is to my left thereforre i should be damaged and move right 
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }
            
        }
    }
    private void Movement()
    {
        float hDirection = Input.GetAxis("Horizontal");

        //move left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);

        }
        //move right 
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);

        }

        // jumping 
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            jump();
        }
    }

    private void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void AnimaltionState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                state = State.idle;
            }
        }
       else if(Mathf.Abs(rb.velocity.x) > 2f) // do truot them 2f khi ket thuc di chuyen
        {
            // di chuyen 
            state = State.running;
        }
        else
        {
            state= State.idle;
        }
    }

}
