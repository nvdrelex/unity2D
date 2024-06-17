using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FrogAI : Enemy
{

    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;

    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private LayerMask ground;

    private Collider2D coll;
    private new  Animator animator;
    private bool facingLeft = true;

    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
      
    }

    private void Update()
    {
        // transition from  jump to fall
        if (animator.GetBool("Jumping"))
        {
            if(rb.velocity.y < 0.1)
            {
                animator.SetBool("Falling", true);
                animator.SetBool("Jumping", false);
            }
        }
        // transition from Fall to Idle 
        if(coll.IsTouchingLayers(ground) && animator.GetBool("Falling"))
        {
            animator.SetBool("Falling", false);
        }

    }

    private void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                // make sure sprite is facing right location , and if it is not , then face the right direction
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                //Test to see if i am on the ground , if so jump
                if (coll.IsTouchingLayers(ground))
                {

                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    animator.SetBool("Jumping", true);
                }

            }
            else
            {
                facingLeft = false;
            }



        }
        else
        {
            if (transform.position.x < rightCap)
            {
                // make sure sprite is facing right location , and if it is not , then face the right direction
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                //Test to see if i am on the ground , if so jump
                if (coll.IsTouchingLayers(ground))
                {

                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    animator.SetBool("Jumping", true);
                }

            }
            else
            {
                facingLeft = true;
            }
        }
    }

   
    
}