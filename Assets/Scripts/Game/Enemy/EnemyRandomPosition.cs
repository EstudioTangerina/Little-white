using UnityEngine;

public class EnemyRandomPosition : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f;
    public Animator animator;

    public Vector2 randomDirection;
    private Rigidbody2D rb;
    private float timeSinceLastDirectionChange;

    public bool Stop;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeSinceLastDirectionChange = changeDirectionInterval;
    }

    private void Update()
    {

        if(Stop == true)
        {
            randomDirection = Vector2.zero;
        }

        timeSinceLastDirectionChange += Time.deltaTime;
        AnimationEnemy();

        if (timeSinceLastDirectionChange >= changeDirectionInterval)
        {
            timeSinceLastDirectionChange = 0f;
            randomDirection = GetRandomDirection();
        }

        if (randomDirection != Vector2.zero)
        {
            // Movimento
            rb.velocity = randomDirection * moveSpeed;

            // Atualizar animação
            animator.SetBool("IsWalking", true);
        }
        else
        {
            rb.velocity = Vector2.zero;

            // Atualizar animação
            animator.SetBool("IsWalking", false);
        }

        if (randomDirection.x < -0.01f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (randomDirection.x > 0.01f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private Vector2 GetRandomDirection()
    {
        int randomIndex = Random.Range(0, 4); // 0: cima, 1: baixo, 2: esquerda, 3: direita

        switch (randomIndex)
        {
            case 0:
                return Vector2.up;
            case 1:
                return Vector2.down;
            case 2:
                return Vector2.left;
            case 3:
                return Vector2.right;
            default:
                return Vector2.zero;
        }
    }


    void AnimationEnemy()
    {
        #region Walking
        animator.SetFloat("Horizontal", randomDirection.x);
        animator.SetFloat("Vertical", randomDirection.y);
        animator.SetFloat("Speed", randomDirection.sqrMagnitude);

        if (randomDirection.x == 1 || randomDirection.x == -1 || randomDirection.y == 1 || randomDirection.y == -1)
        {
            animator.SetFloat("LastHorizontal", randomDirection.x);
            animator.SetFloat("LastVertical", randomDirection.y);

        }
        #endregion
    }
}

