using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace KingsAndPigs
{
    public class PlayerInputs : MonoBehaviour
    {
        public static PlayerInputActions _playerInputActions;

        public static InputAction _playerMove;
        private InputAction _playerInteract;
        private InputAction _playerAttack;
        private InputAction _playerJump;

        [SerializeField] private UnityEvent _moveEvent;
        [SerializeField] private UnityEvent _jumpEvent;
        [SerializeField] private UnityEvent _attackEvent;
        [SerializeField] private UnityEvent _interactEvent;

        public void Awake() => _playerInputActions = new PlayerInputActions();

        private void OnEnable()
        {
            _playerMove = _playerInputActions.Player.Move;
            _playerJump = _playerInputActions.Player.Jump;
            _playerAttack = _playerInputActions.Player.Fire;
            _playerInteract = _playerInputActions.Player.Interact;

            _playerMove.Enable();
            _playerJump.Enable();
            _playerAttack.Enable();
            _playerInteract.Enable();

            _playerMove.performed += PlayerMove;
            _playerJump.performed += PlayerJump;
            _playerAttack.performed += PlayerAttack;
            _playerInteract.performed += PlayerInteraction;
        }

        private void OnDisable()
        {
            _playerMove.Disable();
            _playerJump.Disable();
            _playerAttack.Disable();
            _playerInteract.Disable();
        }

        private void PlayerMove(InputAction.CallbackContext context) => _moveEvent.Invoke();
        private void PlayerJump(InputAction.CallbackContext context) => _jumpEvent.Invoke();
        
        private void PlayerAttack(InputAction.CallbackContext context) => _attackEvent.Invoke();
        private void PlayerInteraction(InputAction.CallbackContext context) => _interactEvent.Invoke();
    }
}
