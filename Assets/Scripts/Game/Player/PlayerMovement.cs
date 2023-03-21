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
    }

    void OnMove(InputValue inputValue)
    {
        MoveInput = inputValue.Get<Vector2>();
    }

    void OnAim(InputValue inputValueAim)
    {
        bool isAiming = inputValueAim.isPressed;
        animator.SetBool("Aiming", isAiming);

        if(!isAiming)
        {
            animator.SetBool("Aiming", false);
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

        #region Running

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && (MoveInput.x != 0 || MoveInput.y != 0);
        animator.SetBool("Running", isRunning);
        
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
}
