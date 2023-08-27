using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public float forceDuration = 1.0f; // O tempo durante o qual a for�a � aplicada
    private float forceTimer = 0.0f;
    private Vector2 forceDirection = Vector2.zero;
    private bool applyingForce = false;
    public float forceMagnitude;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        Knockback();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            moveSpeed = 0;
            Walking = false;
            animator.SetBool("IsAttacking", true);
        }

        if (collision.CompareTag("HitBoxAttacks"))
        {
            StartCoroutine(Blink());
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

    void Knockback()
    {
        if (applyingForce)
        {
            forceTimer += Time.deltaTime;
            float t = Mathf.Clamp01(forceTimer / forceDuration);
            rb.velocity = Vector2.Lerp(Vector2.zero, forceDirection * forceMagnitude, t);

            if (forceTimer >= forceDuration)
            {
                applyingForce = false;
                forceTimer = 0.0f;
                rb.velocity = Vector2.zero; // Reset velocity after applying force
            }
        }
    }

    public void ApplyForce(Vector2 direction)
    {
        forceDirection = direction.normalized;
        applyingForce = true;
    }

    IEnumerator Blink()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Hitting", true);
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("Hitting", false);
        StopCoroutine(Blink());
    }
}
