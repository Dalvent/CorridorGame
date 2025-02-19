﻿using System;
using UnityEngine;

public class PlayerOpenDoor : MonoBehaviour
{
    public float RayDistance = 3f;
    public Camera PlayerCamera;

    private Door _door;
    private LayerMask _doorLayer;
    
    public event Action BeginOpening;
    public event Action EndOpening;

    private void Awake()
    {
        _doorLayer = LayerMask.GetMask("Door");
    }

    private void Update()
    {
        if (!InputManager.Instance.IsUseDown)
            return;
        
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
