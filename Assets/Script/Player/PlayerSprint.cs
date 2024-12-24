using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMove))]
public class PlayerSprint : MonoBehaviour
{
    [Header("Sprint Settings")]
    public float SprintSpeedMultiplier = 2f;

    [Header("Stamina Settings")]
    public float MaxStamina = 3f;
    public float StaminaToStartSprint = 2f;
    public float StaminaRecoveryRate = 1f;
    public float StaminaDrainRate = 1f;

    private float _currentStamina;
    private bool _haveSprintInput;

    private bool _isSprinting;
    private bool IsSprinting
    {
        get => _isSprinting;
        set
        {
            if (_isSprinting == value)
                return;

            _isSprinting = value;

            _playerMove.SpeedMultiplayer = _isSprinting ? SprintSpeedMultiplier : 1.0f;
        }
    }
    
    private PlayerMove _playerMove;
    private InputSystem _inputSystem;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        
        _inputSystem = new InputSystem();
        
        _currentStamina = MaxStamina;
    }

    private void OnEnable()
    {
        _inputSystem.Player.Sprint.performed += OnSprint;
        _inputSystem.Player.Sprint.canceled += OnSprintCancel;

        _inputSystem.Enable();
    }
    
    private void OnDisable()
    {
        _inputSystem.Disable();
        
        _inputSystem.Player.Sprint.performed -= OnSprint;
        _inputSystem.Player.Sprint.canceled -= OnSprintCancel;
    }
    
    public void OnSprint(InputAction.CallbackContext context)
    {
        _haveSprintInput = _currentStamina > StaminaToStartSprint;
    }

    private void OnSprintCancel(InputAction.CallbackContext context)
    {
        _haveSprintInput = false;
    }

    private void Update()
    {
        IsSprinting = _haveSprintInput && _currentStamina > 0;

        if (!IsSprinting)
            _haveSprintInput = false;
        
        _currentStamina = IsSprinting 
            ? Math.Max(_currentStamina - StaminaDrainRate * Time.deltaTime, 0)
            : Math.Min(_currentStamina + StaminaRecoveryRate * Time.deltaTime, MaxStamina); 
        
        Debug.Log(_currentStamina);
    }
}