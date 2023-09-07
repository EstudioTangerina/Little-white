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

    public float acceleration = 5.0f; // Aceleração do inimigo
    public float maxSpeed = 2.0f;     // Velocidade máxima do inimigo
    public float forceDuration = 1.0f; // Duração da força aplicada
    private Vector2 currentVelocity;
    private bool applyingForce = false;
    private Vector2 forceDirection;
    private float forceTimer = 0.0f;
    public bool Test;

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
            forceTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(forceTimer / forceDuration);
            currentVelocity = Vector2.Lerp(Vector2.zero, forceDirection * maxSpeed, t);

            if (forceTimer >= forceDuration)
            {
                applyingForce = false;
                forceTimer = 0.0f;
            }
        }

        if (Test)
        {
            if (currentVelocity.magnitude > maxSpeed)
            {
                currentVelocity = currentVelocity.normalized * maxSpeed;
            }

            rb.velocity = currentVelocity;
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
        Test = true;
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("Hitting", false);
        Test = false;
        StopCoroutine(Blink());
    }
}
