using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    public float MouseSensitivity = 50f;
    public float VerticalAngleRange = 90f;
    public float HorizontalAngleRange = 90f;
    public float SmoothTime = 0.05f;
    public Camera PlayerCamera;
    
    private InputSystem _inputSystem;
    private Vector2 _rotation = Vector2.zero;
    private Vector2 _lookInput;
    private Vector2 _smoothInput;
    private Vector2 _currentInputVelocity;
    private float _startHorizontalRotation;

    private void Awake()
    {
        _inputSystem = new InputSystem();
    }

    private void Start()
    {
        _startHorizontalRotation = transform.rotation.eulerAngles.y;
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _inputSystem.Player.Look.performed += OnLook;
        _inputSystem.Player.Look.canceled += OnLook;
        _inputSystem.Enable();
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        _inputSystem.Disable();
        _inputSystem.Player.Look.performed -= OnLook;
        _inputSystem.Player.Look.canceled -= OnLook;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    public void Update()
    {
        _smoothInput = Vector2.SmoothDamp(_smoothInput, _lookInput, ref _currentInputVelocity, SmoothTime);

        float mouseX = _smoothInput.x * MouseSensitivity * Time.deltaTime;
        float mouseY = _smoothInput.y * MouseSensitivity * Time.deltaTime;

        _rotation.x -= mouseY;
        _rotation.x = Mathf.Clamp(_rotation.x, -VerticalAngleRange, VerticalAngleRange);

        _rotation.y += mouseX;
        _rotation.y = Mathf.Clamp(_rotation.y, _startHorizontalRotation - HorizontalAngleRange, _startHorizontalRotation + HorizontalAngleRange);

        PlayerCamera.transform.localRotation = Quaternion.Euler(_rotation.x, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, _rotation.y, 0f);
    }
}