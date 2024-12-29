using System;
using Script;
using UnityEngine;

public class SteppingSystem : MonoBehaviour
{
    public float StepDistance = 0.1f;
    public float StepTime = 1.0f;
    
    [Header("Distance ratios")]
    public float PrepareStepDistanceRatios = 0.10f;
    public float MainStepDistanceRatios = 0.85f;
    public float EndingStepDistanceRatios = 0.05f;
    
    [Header("Time ratios")]
    public float PrepareStepTimeRatios = 0.4f;
    public float MainStepTimeRatios = 0.5f;
    public float EndingStepTimeRatios = 0.1f;

    public AudioSource PrepareSound;
    public AudioSource MainSound;
    
    public float SumStepDistanceRatios => PrepareStepDistanceRatios + MainStepDistanceRatios + EndingStepDistanceRatios;
    public float SumStepTimeRatios => PrepareStepTimeRatios + MainStepTimeRatios + EndingStepTimeRatios;
    
    public float PrepareStepDistance => StepDistance * (PrepareStepDistanceRatios / SumStepDistanceRatios);
    public float MainStepDistance  => StepDistance * (MainStepDistanceRatios / SumStepDistanceRatios);
    public float EndingStepDistance  => StepDistance * (EndingStepDistanceRatios / SumStepDistanceRatios);
    
    public float PrepareStepTime => StepTime * (PrepareStepTimeRatios / SumStepTimeRatios);
    public float MainStepTime => StepTime * (MainStepTimeRatios / SumStepTimeRatios);
    public float EndingStepTime => StepTime * (EndingStepTimeRatios / SumStepTimeRatios);

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
            
            switch (_currentState)
            {
                case StepState.Preparing:
                    PrepareSound.PlayWithRandomPitch(0.75f, 1.2f);
                    break;
                case StepState.MainStep:
                    MainSound.PlayWithRandomPitch(0.75f, 1.2f);
                    break;
            }

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
    
    public void ForceStop()
    {
        CurrentState = StepState.Idle;
    }

    public void CancelStep()
    {
        // TODO: Make it work :/
        /*if (CurrentState == StepState.Preparing)
        {
            CurrentState = StepState.Canceling;
            _elapsedTime = 0f;
        }*/
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