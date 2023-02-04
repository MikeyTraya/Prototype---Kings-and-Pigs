using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KingsAndPigs
{
    public class PlayerController : MonoBehaviour
    {
        public static Vector2 MoveDirection { get; private set; }
        
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody2D;

        // Running Mechanics
        private float _moveSpeed = 5f;
        public static bool isRunning { get; private set; } = false;
        private bool isFacingRight = true;

        // Jumping Mechanics
        public static bool isJumping { get; private set; } = false;
        [SerializeField] private float _jumpForce = 0;
        [SerializeField] private float _linearDrag = 0;
        [SerializeField] private float _fallMultiplier = 0;
        public static bool isGrounded { get; private set; } = false;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundLength;
        [SerializeField] private Vector3 _colliderOffset;
        private readonly float _gravity = 1f;

        // Attacking Mechanics
        public static bool isAttacking { get; private set; } = false;

        // Interacting Mechanics
        public static bool _isInteracting { get; private set; } = false;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void Update()
        {
            MoveDirection = PlayerInputs._playerMove.ReadValue<Vector2>();    
            isGrounded = GroundCheck();
            // FlipPlayer();
            if (!isFacingRight && MoveDirection.x > 0f)
            {
                FlipPlayer();
            }
            else if (isFacingRight && MoveDirection.x < 0f)
            {
                FlipPlayer();
            }
        }


        private void FixedUpdate()
        {
            ModifyPhysics();

            if (isRunning)
            {
                _rigidbody2D.velocity = new Vector2(MoveDirection.x * _moveSpeed, _rigidbody2D.velocity.y);
            }

            if (isJumping && isGrounded)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
                _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                isJumping = false;
            }
        }

        private bool GroundCheck()
        {
            return Physics2D.Raycast(transform.position + _colliderOffset, Vector2.down, _groundLength, _groundLayer) ||
                Physics2D.Raycast(transform.position - _colliderOffset, Vector2.down, _groundLength, _groundLayer);
        }

        private void ModifyPhysics()
        {
            if (isGrounded)
            {
                _rigidbody2D.gravityScale = 0;
            }
            else
            {
                _rigidbody2D.gravityScale = _gravity;
                _rigidbody2D.drag = _linearDrag * 0.15f;
                if (_rigidbody2D.velocity.y < 0)
                {
                    _rigidbody2D.gravityScale = _gravity * _fallMultiplier;
                }
                else if (_rigidbody2D.velocity.y > 0)
                {
                    _rigidbody2D.gravityScale = _gravity * (_fallMultiplier / 2);
                }
            }
        }

        private void FlipPlayer()
        {
            //if (MoveDirection.x != 0) _spriteRenderer.flipX = MoveDirection.x < 0;
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
        public void Moving() => isRunning = true;

        public void Jump()
        {
            isJumping = true;
        }

        public void Attack()
        {
        }

        public void Interact()
        {
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + _colliderOffset, transform.position + _colliderOffset + Vector3.down * _groundLength);
            Gizmos.DrawLine(transform.position - _colliderOffset, transform.position - _colliderOffset + Vector3.down * _groundLength);
        }
    }
}
