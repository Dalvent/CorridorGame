using System;
using Script;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInteract : MonoBehaviour
{
    public float InteractionDistance = 3f;
    public Camera PlayerCamera;
    public GameObject _interactUI;

    private InputSystem _inputSystem;

    private IInteractable _currentInteractable;
    private IInteractable CurrentInteractable
    {
        get => _currentInteractable;
        set
        {
            if (Equals(_currentInteractable, value))
                return;
                
            _currentInteractable = value;
            _interactUI.SetActive(_currentInteractable != null);
        }
    }

    private void Awake()
    {
        _inputSystem = new InputSystem();
    }

    private void Start()
    { 
        _interactUI.SetActive(false);
    }

    private void OnEnable()
    {
        _inputSystem.Player.Interact.performed += OnInteract;
        _inputSystem.Player.Interact.canceled += OnInteract;
        _inputSystem.Enable();
    }

    private void OnDisable()
    {
        _inputSystem.Player.Interact.performed -= OnInteract;
        _inputSystem.Player.Interact.canceled -= OnInteract;
        _inputSystem.Disable();
    }

    private void Update()
    {
        HandleInteractionCheck();
    }

    private void HandleInteractionCheck()
    {
        Ray ray = new(PlayerCamera.transform.position, PlayerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable?.CanInteract == true)
            {
                CurrentInteractable = interactable;
                return;   
            }
        }
        
        CurrentInteractable = null;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        CurrentInteractable?.Interact();
    }
}
