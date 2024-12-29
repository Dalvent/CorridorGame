using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerOpenDoor : MonoBehaviour
{
    public float RayDistance = 3f;
    public Camera PlayerCamera;

    private Door _door;
    private InputSystem _inputSystem;
    private LayerMask _doorLayer;
    
    public event Action BeginOpening;
    public event Action EndOpening;

    private void Awake()
    {
        _inputSystem = new InputSystem();
        _doorLayer = LayerMask.GetMask("Door");
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

    private void OnUse(InputAction.CallbackContext context)
    {
        Ray ray = new(PlayerCamera.transform.position, PlayerCamera.transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, RayDistance, _doorLayer)) 
            return;
        
        _door = hit.collider.GetComponent<Door>();
        if (_door == null)
            return; 

        BeginOpening?.Invoke();
        _door.Open(() =>
        {
            EndOpening?.Invoke();
        });
    }
}
