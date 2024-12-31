using System;
using Script.Rail;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerForwardMove : MonoBehaviour
{
    public float RailEventActivationDistance = 3f;
    public Camera PlayerCamera;
    public SteppingSystem SteppingSystem;

    public event Action BeginRailEvent;
    public event Action EndRailEvent;

    private LayerMask _doorLayer;

    private IRailEvent _currentRailEvent;
    private bool _havePlayerInput;

    private void Awake()
    {
        _doorLayer = LayerMask.GetMask("Door");
    }

    private void OnEnable()
    {
        InputManager.Instance.UsePerformed += OnUse;
        InputManager.Instance.UseCanceled += OnStopUse;
    }

    private void OnDisable()
    {
        InputManager.Instance.UsePerformed -= OnUse;
        InputManager.Instance.UseCanceled -= OnStopUse;
        
        _havePlayerInput = false;
        (_currentRailEvent as ICancelableRailEvent)?.RequestCancel();
    }

    private void Update()
    {
        if (_havePlayerInput && _currentRailEvent == null)
        {
            _currentRailEvent = FindNextGameObjectRailEvent() ?? new StepRailEvent(SteppingSystem);
            _currentRailEvent.Perform();
            BeginRailEvent?.Invoke();
        }
        else
        {
            (_currentRailEvent as ICancelableRailEvent)?.RequestCancel();
        }
        
        if (_currentRailEvent?.IsPerformed != true)
            return;

        EndRailEvent?.Invoke();
        (_currentRailEvent as IDisposable)?.Dispose();
        _currentRailEvent = null;
    }

    private IRailEvent FindNextGameObjectRailEvent()
    {
        Ray ray = new(PlayerCamera.transform.position, PlayerCamera.transform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, RailEventActivationDistance, _doorLayer))
            return null;

        var railEvent = hit.collider.GetComponent<IRailEvent>();
        return railEvent.IsPerformed ? null : railEvent;
    }

    private void OnUse()
    {
        _havePlayerInput = true;
    }

    private void OnStopUse()
    {
        _havePlayerInput = false;
    }
}

public class StepRailEvent : ICancelableRailEvent, IDisposable
{
    private readonly SteppingSystem _steppingSystem;
    public bool IsPerformed { get; private set; }

    public StepRailEvent(SteppingSystem steppingSystem)
    {
        _steppingSystem = steppingSystem;
        _steppingSystem.StepStateChanged += OnStepStateChanged;
    }

    public void Perform()
    {
        _steppingSystem.StartStep();
    }

    private void OnStepStateChanged()
    {
        if (_steppingSystem.CurrentState == SteppingSystem.StepState.Idle)
            IsPerformed = true;
    }

    public void RequestCancel()
    {
        _steppingSystem.CancelStep();
    }

    public void Dispose()
    {
        _steppingSystem.StepStateChanged -= OnStepStateChanged;
    }
}