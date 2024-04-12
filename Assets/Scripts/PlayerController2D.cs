using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{

    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private GameControls _gameControls;

    [SerializeField] private float _moveSpeed;

    private void Awake()
    {
        _gameControls = new GameControls();
    }


    // Start is called before the first frame update
    void Start()
    {
        _gameControls.Player.Move.performed += _ => Move();

        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_moveDirection.sqrMagnitude == 0) return;
        Move();
    }

    private void Move()
    {
        // read the controls data
        _moveDirection = _gameControls.Player.Move.ReadValue<Vector2>();

        // Move the sprite
        _rb.velocity = new Vector2(_moveDirection.x, _moveDirection.y).normalized * _moveSpeed;

        // TODO: Flip the sprite on left or right move direction
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
