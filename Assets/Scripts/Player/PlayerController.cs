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

        // Jumping Mechanics
        public static bool isJumping { get; private set; } = false;
        private float _jumpForce = 5f;
        public static bool isGrounded { get; private set; } = false;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundLength;
        [SerializeField] private Vector3 _colliderOffset;

        // Attaching Mechanics
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
            isGrounded = Physics2D.Raycast(transform.position + _colliderOffset, Vector2.down, _groundLength, _groundLayer) || 
                Physics2D.Raycast(transform.position - _colliderOffset, Vector2.down, _groundLength, _groundLayer);

            MoveDirection = PlayerInputs._playerMove.ReadValue<Vector2>();    
            
            FlipPlayer();
        }

        private void FlipPlayer()
        {
            if (MoveDirection.x != 0) _spriteRenderer.flipX = MoveDirection.x < 0;
        }

        private void FixedUpdate()
        {
            if (isRunning)
            {
                _rigidbody2D.velocity = new Vector2(MoveDirection.x * _moveSpeed, _rigidbody2D.velocity.y);
            }

            if (isJumping && isGrounded)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
                _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            }
        }

        public void Moving() => isRunning = true;

        public void Jump()
        {
            isJumping = true;
            StartCoroutine(JumpTimer());
        }

        public void Attack()
        {
            isAttacking = true;
            StartCoroutine(AttackTimer());
        }

        public void Interact()
        {
            Debug.Log("Player is interacting");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + _colliderOffset, transform.position + _colliderOffset + Vector3.down * _groundLength);
            Gizmos.DrawLine(transform.position - _colliderOffset, transform.position - _colliderOffset + Vector3.down * _groundLength);
        }

        IEnumerator JumpTimer()
        {
            yield return new WaitForSeconds(.1f);
            isJumping = false;
        }

        IEnumerator AttackTimer()
        {
            yield return new WaitForSeconds(.1f);
            isAttacking = false;
        }
    }
}
