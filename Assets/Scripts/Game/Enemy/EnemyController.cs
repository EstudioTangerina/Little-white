using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float changeDirectionInterval = 2f;
    public Animator animator;

    private Vector2 randomDirection;
    private Rigidbody2D rb;
    private float timeSinceLastDirectionChange;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeSinceLastDirectionChange = changeDirectionInterval;
    }

    private void Update()
    {
        timeSinceLastDirectionChange += Time.deltaTime;

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
}

