using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float movementSpeed;

    private const float MouseSensitivity = 100f;
    private InputMaster _inputMaster;
    private Transform _cameraTransform;
    private float _xRotation;

    private void Awake()
    {
        if (Camera.main != null) _cameraTransform = Camera.main.transform;
        _inputMaster = new InputMaster();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable() => _inputMaster.Enable();
    private void OnDisable() => _inputMaster.Disable();

    private void Update()
    {
        Look();
        Move();
    }

    private void Move()
    {
        var moveInput = _inputMaster.Player.Movement.ReadValue<Vector2>();
        var move = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (Keyboard.current.qKey.isPressed) move += Vector3.down;
        if (Keyboard.current.eKey.isPressed && transform.position.y < 8f) move += Vector3.up;

        characterController.Move(move * (movementSpeed * Time.deltaTime));
    }

    private void Look()
    {
        var lookInput = _inputMaster.Player.Look.ReadValue<Vector2>();
        _xRotation = Mathf.Clamp(_xRotation - lookInput.y * MouseSensitivity * Time.deltaTime, -90f, 90f);
        _cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * (lookInput.x * MouseSensitivity * Time.deltaTime));
    }
}