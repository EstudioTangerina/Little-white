using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ElaraScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float speed;
    [SerializeField] private float speedMultiplier;
    
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;

    private Animator _animator;
    private Vector2 _moveInput;
    private bool isDashing;
    public bool isAttacking;
    public int attackStep = 0; // Alterna entre os ataques

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isDashing && !isAttacking)
        {
            HandleAnimation();
            FlipSprite();
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing && !isAttacking)
        {
            MoveCharacter();
        }
    }

    public void OnMove(InputValue inputValue)
    {
        _moveInput = inputValue.Get<Vector2>();
    }

    public void OnRun(InputValue inputValueRun)
    {
        bool isRunning = inputValueRun.isPressed && _moveInput != Vector2.zero;
        _animator.SetBool("Running", isRunning);
    }

    public void OnDash(InputValue inputValueDash)
    {
        if (inputValueDash.isPressed && _moveInput != Vector2.zero)
        {
            StartCoroutine(DashCoroutine(_moveInput.normalized));
        }
    }

    private IEnumerator DashCoroutine(Vector2 dashDirection)
    {
        isDashing = true;
        GetComponent<SpriteGhostTrailRenderer>().enabled = true;
        _animator.SetBool("Dashing", true);

        _rigidbody.linearVelocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        _animator.SetBool("Dashing", false);
        GetComponent<SpriteGhostTrailRenderer>().enabled = false;
        isDashing = false;
    }

    private void MoveCharacter()
    {
        float adjustedSpeed = _animator.GetBool("Running") ? speed * speedMultiplier : speed;
        _rigidbody.linearVelocity = _moveInput * adjustedSpeed;

        if (_moveInput == Vector2.zero)
        {
            _animator.SetBool("Running", false);
        }
    }

    private void HandleAnimation()
    {
        _animator.SetFloat("Horizontal", _moveInput.x);
        _animator.SetFloat("Vertical", _moveInput.y);
        _animator.SetFloat("Speed", _moveInput.sqrMagnitude);

        if (_moveInput != Vector2.zero)
        {
            _animator.SetFloat("LastHorizontal", _moveInput.x);
            _animator.SetFloat("LastVertical", _moveInput.y);
        }
    }

    private void FlipSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (_moveInput.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }
        else if (_moveInput.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
    }
}
