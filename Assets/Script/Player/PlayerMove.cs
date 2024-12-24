using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Header("Player Stats")]
    public float Speed = 2f;
    public float Acceleration = 10f;

    [Header("Gravity")]
    public float Gravity = -9.81f;
    public float GroundCheckDistance = 0.2f;
    public LayerMask GroundLayer;

    private CharacterController _characterController;
    private Vector3 _velocity = Vector3.zero;
    private Vector2 _inputDirection = Vector2.zero;
    private bool _isGrounded;
    
    private InputSystem _inputSystem;

    public float SpeedMultiplayer { get; set; } = 1f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
     
        _inputSystem = new InputSystem();
    }

    private void OnEnable()
    {
        _inputSystem.Player.Move.performed += OnMove;
        _inputSystem.Player.Move.canceled += OnMove;

        _inputSystem.Enable();
    }
    
    private void OnDisable()
    {
        _inputSystem.Disable();
        
        _inputSystem.Player.Move.performed -= OnMove;
        _inputSystem.Player.Move.canceled -= OnMove;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.phase switch
        {
            InputActionPhase.Performed => context.ReadValue<Vector2>(),
            InputActionPhase.Canceled => Vector2.zero,
            _ => _inputDirection
        };
    }

    private void Update()
    {
        CalculateGroundCheck();
        CalculateMovement();
        CalculateGravity();
        
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void CalculateGroundCheck()
    {
        _isGrounded = Physics.CheckSphere(transform.position, GroundCheckDistance, GroundLayer);
    }

    private void CalculateMovement()
    {
        Vector3 desiredVelocity = new Vector3(_inputDirection.x, 0, _inputDirection.y).normalized;
        desiredVelocity = transform.TransformDirection(desiredVelocity) * (Speed * SpeedMultiplayer);

        _velocity.x = Mathf.Lerp(_velocity.x, desiredVelocity.x, Time.deltaTime * Acceleration);
        _velocity.z = Mathf.Lerp(_velocity.z, desiredVelocity.z, Time.deltaTime * Acceleration);
    }

    private void CalculateGravity()
    {
        if (!_isGrounded)
            _velocity.y += Gravity * Time.deltaTime;
    }
}