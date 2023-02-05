using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KingsAndPigs
{
    public class PlayerControllerV2 : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        // Move Mechanics
        public static Vector2 MoveDirection { get; private set; }
        private float _moveSpeed = 5f;

        // Jumping Mechanics
        public static bool isLongJump { get; private set; } = false;
        public static bool isShortJump { get; private set; } = false;
        [SerializeField] private float _jumpForce = 0;
        [SerializeField] private float _linearDrag = 0;
        [SerializeField] private float _fallMultiplier = 0;
        public static bool isGrounded { get; private set; }
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundLength;
        [SerializeField] private Vector3 _colliderOffset;
        private readonly float _gravity = 1f;
        private bool _isFacingRight = true;
        private float _cayoteTime = 0.2f;
        private float _cayoteTimeCounter;

        // Attacking Mechanics
        public static bool isAttacking { get; private set; } = false;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // Ground checking
            isGrounded = GroundCheck();
            
            // Flip Player
            if (!_isFacingRight && MoveDirection.x > 0f)
            {
                FlipPlayer();
            }
            else if (_isFacingRight && MoveDirection.x < 0f)
            {
                FlipPlayer();
            }

            // Cayote Time
            _cayoteTimeCounter = (isGrounded) ? _cayoteTime : _cayoteTimeCounter -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            MoveCheck();
            ModifyPhysics();
            JumpCheck();
        }

        private void FlipPlayer()
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

        private bool GroundCheck() => Physics2D.Raycast(transform.position + _colliderOffset, Vector2.down, _groundLength, _groundLayer) ||
                Physics2D.Raycast(transform.position - _colliderOffset, Vector2.down, _groundLength, _groundLayer);

        public void Move(InputAction.CallbackContext context)
        {
            MoveDirection = context.ReadValue<Vector2>();
        }

        public void MoveCheck() =>_rigidbody2D.velocity = new Vector2(MoveDirection.x* _moveSpeed, _rigidbody2D.velocity.y);

        public void Jump(InputAction.CallbackContext context)
        {
            // context returns a bool
            if (context.performed && _cayoteTimeCounter > 0f)
            {
                isLongJump = true;
            }
            
            if (context.canceled && _rigidbody2D.velocity.y > 0f && _cayoteTimeCounter > 0f)
            {
                isShortJump = true;
                _cayoteTimeCounter = 0f;
            }
        }

        private void JumpCheck()
        {
            if (isLongJump)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _jumpForce);
                isLongJump = false;
            }

            if (isShortJump)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, (_jumpForce / 2));
                isShortJump = false;
            }
        }

        public void Attack(InputAction.CallbackContext context)
        {
            if (context.performed && isGrounded)
            {
                isAttacking = true;
            }

            if (context.canceled)
            {
                isAttacking = false;
            }
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

                if (_rigidbody2D.velocity.y > 0)
                {
                    _rigidbody2D.gravityScale = _gravity * (_fallMultiplier / 2);
                }
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
