using UnityEngine;
using UnityEngine.InputSystem;

public class ElaraScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float speed;
    [SerializeField] private float speedMultiplier;

    private Animator _animator;
    private Vector2 _moveInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleAnimation();
        FlipSprite();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
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
