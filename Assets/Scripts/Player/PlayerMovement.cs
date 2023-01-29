using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KingsAndPigs
{
    public class PlayerMovement : PlayerInputs
    {
        
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody2D;
        private Animator _animator;

        private float _moveSpeed = 5f;
        private float _jumpForce = 5f;

        private bool isRunning = false;

        private Vector2 moveDirection;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            moveDirection = _playerMove.ReadValue<Vector2>();
            
            
            PlayerCheck();
            Animations();
        }

        private void PlayerCheck()
        {
            if (moveDirection.x < 0)
            {
                _spriteRenderer.flipX = true;
                isRunning = true;
            }

            if (moveDirection.x > 0)
            {
                _spriteRenderer.flipX = false;
                isRunning = true;
            }

            
        }

        private void FixedUpdate()
        {
            if (isRunning)
            {
                _rigidbody2D.velocity = new Vector2(moveDirection.x * _moveSpeed, _rigidbody2D.velocity.y);
                isRunning = false;
            }

            if (_isJumping)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
                _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                _isJumping = false;
            }
        }

        private void Animations()
        {
            _animator.SetBool("isRunning", isRunning);

            // Jumping animation bug
            _animator.SetBool("isJumping", _isJumping);
        }
    }
}
