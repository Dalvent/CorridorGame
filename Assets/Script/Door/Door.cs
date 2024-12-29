using System;
using System.Collections;
using Script;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform RotationPivot;
    public Transform HandleRotationPivot;
    
    public float AccelerationDuration = 1f;
    public float ClosedAccelerationDuration = 0.1f;
    
    [Header("Handle")]
    public float HandleStartAcceleration = 0.5f;
    public float HandleEndAcceleration = 0.15f;
    public float HandleAngle = 60f;
    public int ClosedDoorRotation = 7;
    public bool IsLocked;

    [Header("Audio")]
    public AudioSource UseHandleAudio;
    public AudioSource OpenDoorAudio;
    public AudioSource ClosedDoorAudio;
    
    private bool _isInteracted;

    public event Action BeginOpening;
    public event Action EndOpening;
    
    public bool IsOpened { get; private set; }

    public void Open(Action onFinish = null)
    {
        if (_isInteracted)
        {
            onFinish?.Invoke();
            return;
        }
        
        _isInteracted = true;
        StartCoroutine(DoorAnimation(onFinish));
    }
    
    private IEnumerator DoorAnimation(Action onFinish)
    {
        BeginOpening?.Invoke();

        UseHandleAudio.PlayWithRandomPitch();
        
        Quaternion startHandle = HandleRotationPivot.transform.rotation;
        Quaternion endHandle = startHandle * Quaternion.Euler(-HandleAngle, 0, 0);
        yield return RotateObject(HandleRotationPivot, startHandle, endHandle, HandleStartAcceleration);
        yield return RotateObject(HandleRotationPivot, endHandle, startHandle, HandleEndAcceleration);

        if (IsLocked)
        {
            Quaternion startRotation = RotationPivot.transform.rotation;
            Quaternion openedRotation = startRotation * Quaternion.Euler(0, ClosedDoorRotation, 0);
            ClosedDoorAudio.PlayWithRandomPitch();
            yield return RotateObject(RotationPivot, startRotation, openedRotation, ClosedAccelerationDuration);
            yield return RotateObject(RotationPivot, openedRotation, startRotation, ClosedAccelerationDuration);
        }
        else
        {
            OpenDoorAudio.PlayWithRandomPitch();

            Quaternion startRotation = RotationPivot.transform.rotation;
            Quaternion openedRotation = startRotation * Quaternion.Euler(0, 90, 0);
            yield return RotateObject(RotationPivot, startRotation, openedRotation, AccelerationDuration);
            
            IsOpened = true;
        }
        
        onFinish?.Invoke();
        EndOpening?.Invoke();
    }

    private IEnumerator RotateObject(Transform target, Quaternion from, Quaternion to, float accelerationDuration)
    {
        float _timeElapsed = 0f;

        while (_timeElapsed < accelerationDuration)
        {
            _timeElapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(_timeElapsed / accelerationDuration);
            float smoothProgress = Mathf.Pow(progress, 2);

            target.transform.rotation = Quaternion.Lerp(from, to, smoothProgress);

            yield return null;
        }

        target.transform.rotation = to;
    }
}