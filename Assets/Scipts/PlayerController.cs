using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
    
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource cherryAudio;
    [SerializeField] private AudioSource footstep;
  



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        PermanenUI.perm.healthAmount.text = PermanenUI.perm.health.ToString();
      
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
            cherryAudio.Play();
            Destroy(collision.gameObject);
            PermanenUI.perm.cherries += 1;
            PermanenUI.perm.cherryText.text = PermanenUI.perm.cherries.ToString();
        }
        if(collision.tag == "Powerup")
        {
            Destroy(collision.gameObject);
            jumpForce = 15f;
            GetComponent<SpriteRenderer>().color = Color.yellow;
            StartCoroutine(ResetPower());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" )
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if ( state == State.falling)
            {
                enemy.JumpedOn();
                jump();
            }
            else
            {
                state = State.hurt;
                HandleHealth();  // deal with health , updating ui ,and will reset level if health is <= 0
                if (other.gameObject.transform.position.x > transform.position.x)
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

    private void HandleHealth()
    {
        PermanenUI.perm.health -= 1;
        PermanenUI.perm.healthAmount.text = PermanenUI.perm.health.ToString();
        if (PermanenUI.perm.health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    private void Footstep()
    {
        footstep.Play(); ;
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 12;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

}
