using System.Collections;
using Script;
using UnityEngine;

public class OnceOpenDoor : MonoBehaviour, IInteractable
{
    public Transform RotationPivot;
    public Transform HandleRotationPivot;
    
    public float AccelerationDuration = 1f;
    
    [Header("Handle")]
    public float HandleStartAcceleration = 0.5f;
    public float HandleEndAcceleration = 0.15f;
    public float HandleAngle = 60f;
    
    public bool IsLocked;

    private bool _isInteracted;
    
    public bool CanInteract => !_isInteracted;

    public void Interact()
    {
        if (_isInteracted)
            return;
        
        _isInteracted = true;
        StartCoroutine(DoorAnimation());
    }
    
    private IEnumerator DoorAnimation()
    {
        Quaternion startHandle = HandleRotationPivot.transform.rotation;
        Quaternion endHandle = startHandle * Quaternion.Euler(-HandleAngle, 0, 0);
        yield return RotateObject(HandleRotationPivot, startHandle, endHandle, HandleStartAcceleration);
        yield return RotateObject(HandleRotationPivot, endHandle, startHandle, HandleEndAcceleration);
        
        if (!IsLocked)
        {
            Quaternion startRotation = RotationPivot.transform.rotation;
            Quaternion openedRotation = startRotation * Quaternion.Euler(0, 90, 0);
            yield return RotateObject(RotationPivot, startRotation, openedRotation, AccelerationDuration);
        }
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
