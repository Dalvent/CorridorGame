using System;
using System.Collections;
using UnityEngine;

public class PlayerDirectionState : MonoBehaviour
{
    public enum DirectionState { Forward, Back, Left, Right }
    public delegate void BeginRotatingEvent(DirectionState from, DirectionState to);
    public delegate void EndRotatingEvent(DirectionState updatedState);
    
    public float RotationSpeed = 2f;
    
    public event BeginRotatingEvent BeginRotating;
    public event EndRotatingEvent EndRotating;
    
    private Coroutine _rotationCoroutine;
    private Quaternion _forwardRotation;

    public DirectionState Direction { get; private set; } = DirectionState.Forward;
    
    private void StartRotating(DirectionState targetDirection)
    {
        if (Direction == targetDirection)
            return;
        
        if (_rotationCoroutine != null)
        {
            StopCoroutine(_rotationCoroutine);
            EndRotating?.Invoke(Direction);
                
            _rotationCoroutine = null;
        }
        
        _rotationCoroutine = StartCoroutine(RotateToTarget());
        
        IEnumerator RotateToTarget()
        {
            BeginRotating?.Invoke(Direction, targetDirection);

            var targetRotation = targetDirection switch
            {
                DirectionState.Forward => _forwardRotation,
                DirectionState.Back => _forwardRotation * Quaternion.Euler(0, 180, 0),
                DirectionState.Left => _forwardRotation * Quaternion.Euler(0, -90, 0),
                DirectionState.Right => _forwardRotation * Quaternion.Euler(0, 90, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
                
            while (targetRotation != transform.rotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
                yield return null;
            }
            
            _rotationCoroutine = null;

            Direction = targetDirection;
            EndRotating?.Invoke(Direction);
        }
    }
    
    private void Awake()
    {
        _forwardRotation = transform.rotation;
    }
    
    private void OnEnable()
    {
        InputManager.Instance.LookPerformed += OnLook;

        _rotationCoroutine = null;
    }

    private void OnDisable()
    {
        InputManager.Instance.LookPerformed -= OnLook;
    }
    
    private void OnLook(Vector2 input)
    {
        if (_rotationCoroutine != null)
            return;

        if (input.x >= 0.5f)
            StartRotating(DirectionState.Right);
        else if (input.x <= -0.5f)
            StartRotating(DirectionState.Left);
        else if (input.y >= 0.5f)
            StartRotating(DirectionState.Forward);
        else if (input.y <= -0.5f)
            StartRotating(DirectionState.Back);
    }
}
