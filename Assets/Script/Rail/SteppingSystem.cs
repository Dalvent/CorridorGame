using System;
using UnityEngine;

// Делай через отдельный компонент - с ним проблем не будет даже при изменение стейта
// при том при отмене важно возращаться именно на позицию
// хз можно будет обернуть обращение в какой-нибудь интерфейс для рефакторинга движения вперед, но это потом
// удачи друг.

public class SteppingSystem : MonoBehaviour
{
    public float PrepareStepDistance = 0.1f;
    public float PrepareStepTime = 0.5f;
    public float MainStepDistance = 0.85f;
    public float MainStepTime = 1.0f;
    public float EndingStepDistance = 0.05f;
    public float EndingStepTime = 0.2f;

    public enum StepState { Idle, Preparing, MainStep, Ending, Canceling }
    private StepState _currentState = StepState.Idle;
    public StepState CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState == value)
                return;

            _currentState = value;
            StepStateChanged?.Invoke();
        }
    }

    public event Action StepStateChanged;

    private Vector3 _startPosition;
    private float _elapsedTime;
    private float _currentStepTime;

    private void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        switch (CurrentState)
        {
            case StepState.Preparing:
                PerformMovement(_startPosition, _startPosition + transform.forward * PrepareStepDistance, PrepareStepTime, StepState.MainStep);
                break;

            case StepState.MainStep:
                PerformMovement(_startPosition + transform.forward * PrepareStepDistance,
                                _startPosition + transform.forward * (PrepareStepDistance + MainStepDistance), MainStepTime, StepState.Ending);
                break;

            case StepState.Ending:
                PerformMovement(_startPosition + transform.forward * (PrepareStepDistance + MainStepDistance),
                                _startPosition + transform.forward * (PrepareStepDistance + MainStepDistance + EndingStepDistance), EndingStepTime, StepState.Idle);
                break;

            case StepState.Canceling:
                PerformMovement(transform.position, _startPosition, PrepareStepTime, StepState.Idle);
                break;

            case StepState.Idle:
                CurrentState = StepState.Idle;
                // Ожидание команды
                break;
        }
    }

    public void StartStep()
    {
        if (CurrentState == StepState.Idle)
        {
            CurrentState = StepState.Preparing;
            _startPosition = transform.position;
            _elapsedTime = 0f;
        }
    }

    public void CancelStep()
    {
        return;
        if (CurrentState == StepState.Preparing)
        {
            CurrentState = StepState.Canceling;
            _elapsedTime = 0f;
        }
    }

    private void PerformMovement(Vector3 start, Vector3 end, float duration, StepState nextState)
    {
        _elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(_elapsedTime / duration);
        t = Mathf.SmoothStep(0, 1, t); // Плавный easing
        transform.position = Vector3.Lerp(start, end, t);

        if (t >= 1f)
        {
            CurrentState = nextState;
            _elapsedTime = 0f;
        }
    }
}