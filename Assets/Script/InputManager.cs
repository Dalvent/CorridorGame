using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance => GameBootstrapper.Instance.InputManager;
    
    private InputSystem _inputSystem;

    public event Action<Vector2> LookPerformed;
    public event Action UsePerformed;
    public event Action UseCanceled;
    
    private void OnEnable()
    {
        _inputSystem ??= new InputSystem();
        _inputSystem.Enable();
        _inputSystem.Player.Look.performed += OnLookPerformed;
        _inputSystem.Player.Use.performed += OnUsePerformed;
        _inputSystem.Player.Use.canceled += OnUseCanceled;
        _inputSystem.Player.Exit.performed += OnExit;
        
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        _inputSystem.Disable();
        _inputSystem.Player.Look.performed -= OnLookPerformed;
        _inputSystem.Player.Use.performed -= OnUsePerformed;
        _inputSystem.Player.Use.canceled -= OnUseCanceled;
        _inputSystem.Player.Exit.performed -= OnExit;
    }

    private void OnUsePerformed(InputAction.CallbackContext obj)
    {
        UsePerformed?.Invoke();
    }

    private void OnUseCanceled(InputAction.CallbackContext obj)
    {
        UseCanceled?.Invoke();
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        LookPerformed?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnExit(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }
}
