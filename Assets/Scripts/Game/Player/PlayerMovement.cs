using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float speed;

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
        _rigidbody.velocity = MoveInput * speed;
    }

    void OnMove(InputValue inputValue)
    {
        MoveInput = inputValue.Get<Vector2>();
    }

    void AnimationPlayer()
    {
        #region Move
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
}
