using UnityEngine;
using UnityEngine.InputSystem;

namespace KingsAndPigs
{
    public class PlayerInputs : MonoBehaviour
    {
        protected PlayerInputActions _playerInputActions;

        protected bool _isJumping = false;
        protected bool _isAttacking = false;
        protected bool _isInteracting = false;

        protected InputAction _playerMove;
        protected InputAction _playerInteract;
        protected InputAction _playerAttack;
        protected InputAction _playerJump;

        protected void Awake() => _playerInputActions = new PlayerInputActions();

        protected void OnEnable()
        {
            _playerMove = _playerInputActions.Player.Move;
            _playerJump = _playerInputActions.Player.Jump;
            _playerAttack = _playerInputActions.Player.Fire;
            _playerInteract = _playerInputActions.Player.Interact;

            _playerMove.Enable();
            _playerJump.Enable();
            _playerAttack.Enable();
            _playerInteract.Enable();

            _playerJump.performed += PlayerJump;
            _playerAttack.performed += PlayerAttack;
            _playerInteract.performed += PlayerInteraction;
        }

        protected void OnDisable()
        {
            _playerMove.Disable();
            _playerAttack.Disable();
            _playerAttack.Disable();
            _playerInteract.Disable();
        }

        private void PlayerAttack(InputAction.CallbackContext context) => _isAttacking = true;

        private void PlayerJump(InputAction.CallbackContext context)
        {
            Debug.Log("Player jumped");
            _isJumping = true;
        }

        private void PlayerInteraction(InputAction.CallbackContext context) => _isInteracting = true;
    }
}
