using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerOpenDoor : MonoBehaviour
{
    public float RayDistance = 3f;
    public Camera PlayerCamera;

    private CorridorDoor _corridorDoor;
    private InputSystem _inputSystem;
    private LayerMask _doorLayer;
    
    public event Action BeginOpening;
    public event Action EndOpening;

    private void Awake()
    {
        _inputSystem = new InputSystem();
        _inputSystem.Enable();
        _doorLayer = LayerMask.GetMask("Door");
    }

    private void OnEnable()
    {
        _inputSystem.Player.Use.performed += OnUse;
    }

    private void OnDisable()
    {
        _inputSystem.Player.Use.performed -= OnUse;
    }

    private void OnUse(InputAction.CallbackContext context)
    {
        Ray ray = new(PlayerCamera.transform.position, PlayerCamera.transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, RayDistance, _doorLayer)) 
            return;
        
        _corridorDoor = hit.collider.GetComponent<CorridorDoor>();
        if (_corridorDoor == null)
            return; 

        BeginOpening?.Invoke();
        _corridorDoor.Open(() =>
        {
            EndOpening?.Invoke();
        });
    }
}
