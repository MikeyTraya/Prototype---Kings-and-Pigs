using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KingsAndPigs
{
    public class PlayerAnimations : MonoBehaviour
    {

        [SerializeField] private PlayerController _playerController;

        #region Cached Animation Properties
        private int _currentState;

        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Land = Animator.StringToHash("Land");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Idle = Animator.StringToHash("Idle");
        #endregion

        private Animator _animator;

        [SerializeField] private float _attackAnimDuration = 0.2f;
        [SerializeField] private float _landAnimDuration = 0.1f;
        private float _lockedTill = 0;

        void Start()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        /*void Update()
        {
            var state = GetState();

            if (state == _currentState) return;

            _animator.CrossFade(state, 0, 0);
            _currentState = state;
        }

        private int GetState()
        {
            if (Time.time < _lockedTill) return _currentState;

            // Priorities
            if (_isAttacking) return LockState(Attack, _attackAnimDuration);
            if (_isJumping) return Jump;
            if (_isLanded) return LockState(Land, _landAnimDuration);

            if (_isGrounded) return moveDirection.x == 0 ? Idle : Run;

            return moveDirection.y > 0 ? Jump : Idle;

            // Locked state before transition
            int LockState(int s, float t)
            {
                _lockedTill = Time.time + t;
                return s;
            }
        }*/
    }
}
