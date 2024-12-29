using UnityEngine;
using UnityEngine.InputSystem;

public class YouWinView : MonoBehaviour
{
    private InputSystem _inputSystem;

    private void Awake()
    {
        _inputSystem = new InputSystem();
    }

    private void OnEnable()
    {
        _inputSystem.Player.Use.performed += OnUse;
        _inputSystem.Enable();
    }

    private void OnDisable()
    {
        _inputSystem.Player.Use.performed -= OnUse;
        _inputSystem.Disable();
    }

    private void OnUse(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }
}