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
    Vector2 MoveInput;
    private Animator animator;

    public int combo;
    public bool attack;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimationPlayer();
        FlipSprite();
    }

    void FixedUpdate()
    {
        if (animator.GetBool("Running") == true)
        {
            _rigidbody.velocity = MoveInput * speed * speedMultiplier;
        }
        else
        {
            _rigidbody.velocity = MoveInput * speed;
        }

        if(MoveInput.x == 0 || MoveInput.y == 0)
        {
            animator.SetBool("Running", false);
        }
    }

    void OnMove(InputValue inputValue)
    {
        MoveInput = inputValue.Get<Vector2>();
    }

    void OnRun(InputValue inputValueRun)
    {
        if(inputValueRun.isPressed && (MoveInput.x != 0 || MoveInput.y != 0))
        {
            animator.SetBool("Running", true);
        }else
        {
            animator.SetBool("Running", false);
        }

        
    }

    void OnAim(InputValue inputValueAim)
    {
        bool isAiming = inputValueAim.isPressed;
        animator.SetBool("Aiming", isAiming);

        if(!isAiming)
        {
            animator.SetBool("Aiming", false);
            animator.SetBool("Shooting", true);
        }
    }

    void OnFire(InputValue inputValueFire)
    {
        bool isAttacking = inputValueFire.isPressed;

        if(isAttacking && !attack)
        {
            attack = true;
            animator.SetTrigger("" + combo);
        }
    }

    void AnimationPlayer()
    {
        #region Walking
        animator.SetFloat("Horizontal", MoveInput.x);
        animator.SetFloat("Vertical", MoveInput.y);
        animator.SetFloat("Speed", MoveInput.sqrMagnitude);

        if(MoveInput.x == 1 || MoveInput.x == -1 || MoveInput.y == 1 || MoveInput.y == -1)
        {
            animator.SetFloat("LastHorizontal", MoveInput.x);
            animator.SetFloat("LastVertical", MoveInput.y);
            
        }
        #endregion
    }

    void FlipSprite()
    {
        bool playerHasSpeed = Mathf.Abs(_rigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_rigidbody.velocity.x), transform.localScale.y);
        }
    }

    #region Combo
    public void Start_Combo()
    {
        attack = false;
        if(combo < 1)
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

    public void FinishShoot()
    {
        animator.SetBool("Shooting", false);
    }
}
