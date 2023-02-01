using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KingsAndPigs
{
    public class PlayerController : PlayerInputs
    {
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody2D;

        private float _moveSpeed = 5f;
        private float _jumpForce = 5f;

        protected bool _isRunning = false;
        protected bool _isLanded = false;

        // Jumping Mechanics
        protected bool _isGrounded = false;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundLength;
        [SerializeField] private Vector3 _colliderOffset;

        protected Vector2 moveDirection;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            _isGrounded = Physics2D.Raycast(transform.position + _colliderOffset, Vector2.down, _groundLength, _groundLayer) || 
                Physics2D.Raycast(transform.position - _colliderOffset, Vector2.down, _groundLength, _groundLayer);
            moveDirection = _playerMove.ReadValue<Vector2>();    
            PlayerCheck();
        }

        private void PlayerCheck()
        {
            //if (moveDirection.x != 0) _spriteRenderer.flipX = moveDirection.x < 0;

            if (moveDirection.x < 0)
            {
                _spriteRenderer.flipX = true;
                _isRunning = true;
            }

            if (moveDirection.x > 0)
            {
                _spriteRenderer.flipX = false;
                _isRunning = true;
            }   
        }

        private void FixedUpdate()
        {
            if (_isRunning)
            {
                _rigidbody2D.velocity = new Vector2(moveDirection.x * _moveSpeed, _rigidbody2D.velocity.y);
                _isRunning = false;
            }

            if (_isJumping && _isGrounded)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
                _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                _isJumping = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + _colliderOffset, transform.position + _colliderOffset + Vector3.down * _groundLength);
            Gizmos.DrawLine(transform.position - _colliderOffset, transform.position - _colliderOffset + Vector3.down * _groundLength);
        }
    }
}
