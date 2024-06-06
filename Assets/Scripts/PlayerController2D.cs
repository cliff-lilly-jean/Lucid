using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    // UNITY COMPONENTS
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private SpriteRenderer _spriteRenderer;

    // CONTROLS
    private GameControls _gameControls;

    // ANIMATIONS
    private Animator _animator;

    // MOVEMENT
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _moveSpeedMultiplier;

    // UNITY METHODS
    private void Awake()
    {
        _gameControls = new GameControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameControls.Player.Move.performed += _ => Move();
        _gameControls.Player.Attack.performed += _ => Attack();


        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement
        if (_moveDirection.sqrMagnitude == 0) return;
        Move();
    }

    private void OnEnable()
    {
        _gameControls.Enable();
    }

    private void OnDisable()
    {
        _gameControls.Disable();
    }


    // CUSTOM METHODS
    private void Move()
    {
        // Read the controls data
        _moveDirection = _gameControls.Player.Move.ReadValue<Vector2>();

        // Move the sprite
        _rb.velocity = new Vector2(_moveDirection.x, _moveDirection.y).normalized * _moveSpeed * _moveSpeedMultiplier * Time.deltaTime;

        // Flip the sprite on left or right based on move direction
        FlipSprite(_rb);

        // Run animation on keypress
        Run(_rb);
    }

    private void Attack()
    {
        _animator.SetBool("attack", true);

        // Play a random attack animation from the list of attack animations
        // Detect enemies in the range of the attack
        // Damage the enemies in the range
    }

    private void Run(Rigidbody2D rb)
    {
        if (Mathf.Abs(_rb.velocity.x) > 0.01 || Mathf.Abs(_rb.velocity.y) > 0.01)
        {
            _animator.SetBool("isMoving", true);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
    }

    private void FlipSprite(Rigidbody2D rb)
    {
        if (_rb.velocity.x > 0)
        {
            _spriteRenderer.flipX = false;
        }

        if (_rb.velocity.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }
}
