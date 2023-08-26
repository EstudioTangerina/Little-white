using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform player;
    public Rigidbody2D rb;
    public Vector3 directionToPlayer;
    public Animator animator;
    public bool Walking = true;
    public GameObject enemy;

    private void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player != null)
        {
            directionToPlayer = player.position - transform.position;
            directionToPlayer.Normalize();

            rb.velocity = directionToPlayer * moveSpeed;
        }

        AnimationEnemy();
        FlipSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            moveSpeed = 0;
            Walking = false;
            animator.SetBool("IsAttacking", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            moveSpeed = 0.5f;
            Walking = true;
            animator.SetBool("IsAttacking", false);
        }
    }

    void AnimationEnemy()
    {
        #region Walking
        animator.SetFloat("Horizontal", directionToPlayer.x);
        animator.SetFloat("Vertical", directionToPlayer.y);

        if (directionToPlayer.x <= 1 || directionToPlayer.x <= -1 || directionToPlayer.y <= 1 || directionToPlayer.y <= -1)
        {
            animator.SetFloat("LastHorizontal", directionToPlayer.x);
            animator.SetFloat("LastVertical", directionToPlayer.y);

        }
        #endregion

        if (Walking)
        {
            animator.SetBool("IsWalking", true);
            animator.SetFloat("Speed", 1);
        } else
        {
            animator.SetBool("IsWalking", false);
            animator.SetFloat("Speed", 0);
        }
    }

    void FlipSprite()
    {
        if (directionToPlayer.x < -0.01f)
        {
            //GetComponent<SpriteRenderer>().flipX = true;
            enemy.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (directionToPlayer.x > 0.01f)
        {
            enemy.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

}
