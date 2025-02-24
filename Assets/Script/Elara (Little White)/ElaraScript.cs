using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class ElaraScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float speed;
    [SerializeField] private float speedMultiplier;

    // Parâmetros para o dash instantâneo
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration; // duração curta, ex: 0.2f

    private Animator _animator;
    private Vector2 _moveInput;
    public bool isDashing;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Se não estiver dashando, atualiza as animações e o flip do sprite
        if (!isDashing)
        {
            HandleAnimation();
            FlipSprite();
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
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

    // Método acionado pelo input de dash
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
        _animator.SetBool("Dashing", isDashing);

        // Dash instantâneo: aplica a velocidade de dash imediatamente
        _rigidbody.linearVelocity = dashDirection * dashSpeed;

        // Aguarda o tempo de dash (pode ser um valor muito curto para efeito instantâneo)
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