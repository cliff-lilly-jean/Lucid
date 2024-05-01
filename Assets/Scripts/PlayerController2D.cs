using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Unity Components")]
    private Rigidbody2D _rigidbody;
    private Vector2 _moveDirection;
    private GameControls _gameControls;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private RaycastHit2D _raycastHit;

    [Header("Float")]
    [SerializeField] private float _moveSpeed;

    [Header("Bool")]
    private bool _isSpriteFlipped = false;

    private void Awake()
    {
        _gameControls = new GameControls();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogWarning("no spriterender component");
        }
    }


    void Start()
    {
        _gameControls.Player.Move.performed += _ => Move();

        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        if (_moveDirection.sqrMagnitude == 0) return;
        Move();
    }

    private void Move()
    {
        // read the controls data
        _moveDirection = _gameControls.Player.Move.ReadValue<Vector2>();

        // Check if object can be passed or collided with
        _raycastHit = Physics2D.BoxCast(transform.position, _boxCollider.size, 0, new Vector2(0, _moveDirection.y), Mathf.Abs(_moveDirection.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));

        // Make sure we can move in this direction, by casting a box there first. if the box returns null, we're free to move
        if (_raycastHit.collider == null)
        {
            // Move the sprite
            _rigidbody.velocity = new Vector2(_moveDirection.x, _moveDirection.y).normalized * _moveSpeed * Time.deltaTime;

            if (_moveDirection.x > 0.1f)
            {
                _isSpriteFlipped = false;
                UpdateMoveDirection(_moveDirection);
            }

            if (_moveDirection.x < -0.1f)
            {
                _isSpriteFlipped = true;
                UpdateMoveDirection(_moveDirection);
            }
        }

    }

    private void UpdateMoveDirection(Vector2 moveDirection)
    {
        // face left
        if (moveDirection.x < 0 || _isSpriteFlipped == true)
        {
            FlipSprite(_isSpriteFlipped, _spriteRenderer);
        }

        // face right
        if (moveDirection.x > 0 || _isSpriteFlipped == false)
        {
            FlipSprite(_isSpriteFlipped, _spriteRenderer);
        }
    }

    private void FlipSprite(bool isSpriteFlipped, SpriteRenderer spriteRenderer)
    {
        spriteRenderer.flipX = isSpriteFlipped;
    }

    private void OnEnable()
    {
        _gameControls.Enable();
    }

    private void OnDisable()
    {
        _gameControls.Disable();
    }


}
