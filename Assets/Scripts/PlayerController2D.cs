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

        // Move the sprite
        _rigidbody.velocity = new Vector2(_moveDirection.x, _moveDirection.y).normalized * _moveSpeed * Time.deltaTime;

        // flip the sprite
        // face left
        if (_moveDirection.x < 0 || _isSpriteFlipped == true)
        {

            _spriteRenderer.flipX = true;
            _isSpriteFlipped = true;
        }

        // face right
        if (_moveDirection.x > 0 || _isSpriteFlipped == false)
        {
            // the player is facing right
            _spriteRenderer.flipX = false;
            _isSpriteFlipped = false;
        }

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
