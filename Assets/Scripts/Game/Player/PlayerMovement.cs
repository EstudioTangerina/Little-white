using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed;
    public float speedMultiplier;

    private Rigidbody2D _rigidbody;
    public Vector2 MoveInput;
    private Animator animator;

    public int combo;
    public bool attack;
    public bool andando = false;
    public float idleTime = 5f; // Tempo em segundos antes de iniciar a animação de idle
    public float elapsedTime = 0f;

    public GameObject arrowPrefab;
    public Vector3 directionWhenStopped;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimationPlayer();
        FlipSprite();
        ShootCheck();
        IdleAnimations();
    }

    void FixedUpdate()
    {
        if (animator.GetBool("Running") == true)
        {
            _rigidbody.velocity = MoveInput * speed * speedMultiplier;
            andando = true;
        }
        else
        {
            _rigidbody.velocity = MoveInput * speed;
            
        }
    }

    void OnMove(InputValue inputValue)
    {
        MoveInput = inputValue.Get<Vector2>();
    }

    void OnRun(InputValue inputValueRun)
    {
        if (inputValueRun.isPressed && (MoveInput.x != 0 || MoveInput.y != 0))
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
            andando = false;
        }
    }

    void OnAim(InputValue inputValueAim)
    {
        bool isAiming = inputValueAim.isPressed;
        animator.SetBool("Aiming", isAiming);

        if (!isAiming)
        {
            animator.SetBool("Aiming", false);
            animator.SetBool("Shooting", true);
        }
        else
        {
            elapsedTime = 0f;
        }
    }

    void OnFire(InputValue inputValueFire)
    {
        bool isAttacking = inputValueFire.isPressed;

        if (isAttacking && !attack)
        {
            attack = true;
            animator.SetTrigger("" + combo);
            elapsedTime = 0f;
        }
    }

    void AnimationPlayer()
    {
        #region Walking
        animator.SetFloat("Horizontal", MoveInput.x);
        animator.SetFloat("Vertical", MoveInput.y);
        animator.SetFloat("Speed", MoveInput.sqrMagnitude);

        if (MoveInput.x == 1 || MoveInput.x == -1 || MoveInput.y == 1 || MoveInput.y == -1)
        {
            animator.SetFloat("LastHorizontal", MoveInput.x);
            animator.SetFloat("LastVertical", MoveInput.y);

        }
        #endregion
    }

    void FlipSprite()
    {
        bool playerHasSpeed = Mathf.Abs(_rigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_rigidbody.velocity.x), transform.localScale.y);
        }
    }

    #region Combo
    public void Start_Combo()
    {
        attack = false;
        if (combo < 1)
        {
            combo++;
        }
    }

    public void Finish_ani()
    {
        attack = false;
        combo = 0;
    }

    #endregion

   
    public void Shoot()
    {
        if (directionWhenStopped != Vector3.zero)
        {
            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            ArrowController arrowController = arrow.GetComponent<ArrowController>();
            arrowController.direction = directionWhenStopped;
        }
    }

    public void FinishShoot()
    {
        animator.SetBool("Shooting", false);
        animator.SetBool("Aiming", false);
    }

    void ShootCheck()
    {
        if (MoveInput.y == 1)
        {
            directionWhenStopped = Vector3.up;
            arrowPrefab.GetComponent<SpriteRenderer>().flipY = false;
        }
        else if (MoveInput.y == -1)
        {
            directionWhenStopped = Vector3.down;
            arrowPrefab.GetComponent<SpriteRenderer>().flipY = true;
        }
        else if (MoveInput.x == -1)
        {
            directionWhenStopped = Vector3.left;
            arrowPrefab.GetComponent<SpriteRenderer>().flipY = false;
        }
        else if (MoveInput.x == 1)
        {        
            directionWhenStopped = Vector3.right;
            arrowPrefab.GetComponent<SpriteRenderer>().flipY = false;
        }
    }


    void IdleAnimations()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (MoveInput.magnitude > 0f)
        {
            andando = true;
            elapsedTime = 0f;
        } 
        else
        {
            elapsedTime += Time.deltaTime;
            andando = false;
        }

        if (!andando && elapsedTime >= idleTime)
        {
            animator.SetBool("isSitting", true);
            speed = 0f;
        }
        
        if (!andando && stateInfo.IsName("SitIdle") && stateInfo.normalizedTime >= 1.0f)
        {
            animator.SetBool("Sitting", true);
            animator.SetBool("isSitting", false);
        }

        if(stateInfo.IsName("ContinuieSitIdle") && speed == 0f)
        {
            if (Input.anyKeyDown)
            {
                animator.SetBool("Sitting", false);
                animator.SetBool("isSitting", false);
                elapsedTime = 0f;
            }
        }

        if (!andando && (stateInfo.IsName("StandIdle") && stateInfo.normalizedTime >= 1.0f))
        {
            animator.SetTrigger("Standing");
            //elapsedTime = 0f;
            AnimationPlayer();
            speed = 0.5f;

        }
    }
}
