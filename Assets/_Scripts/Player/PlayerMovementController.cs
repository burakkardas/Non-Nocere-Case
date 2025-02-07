using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        #region Serializable Fields

        [SerializeField] private CharacterController characterController;
        [SerializeField] private float movementSpeed;

        #endregion


        #region Fields

        private const float MouseSensitivity = 100f;
        private const float VerticalOffset = 6.5f;

        private PlayerInputHandler _playerInputHandler;
        private Transform _cameraTransform;
        private float _xRotation;

        #endregion


        #region Unity Methods

        private void Awake()
        {
            InitializeMovement();
        }

        private void Update()
        {
            Look();
            Move();
        }

        #endregion


        #region Private Methods

        private void Move()
        {
            var moveInput = _playerInputHandler.MovementInput;
            var move = transform.right * moveInput.x + transform.forward * moveInput.y;

            if (Keyboard.current.qKey.isPressed) move += Vector3.down;
            if (Keyboard.current.eKey.isPressed && transform.position.y < VerticalOffset) move += Vector3.up;

            characterController.Move(move * (movementSpeed * Time.deltaTime));
        }

        private void Look()
        {
            var lookInput = _playerInputHandler.LookInput;
            _xRotation = Mathf.Clamp(_xRotation - lookInput.y * MouseSensitivity * Time.deltaTime, -90f, 90f);
            _cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * (lookInput.x * MouseSensitivity * Time.deltaTime));
        }


        private void InitializeMovement()
        {
            _playerInputHandler = GetComponent<PlayerInputHandler>();
            if (Camera.main != null) _cameraTransform = Camera.main.transform;
            Cursor.lockState = CursorLockMode.Locked;
        }

        #endregion
    }
}