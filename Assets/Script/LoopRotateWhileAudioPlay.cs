using System.Collections;
using UnityEngine;

public class LoopRotateWhileAudioPlay : MonoBehaviour
{
    public AudioSource AudioSource;
    
    public float RotationSpeed = 10f;
    public float RotationAngle = 30f;
    public float TimeToTalk = 1.5f;
    public float ReturnDuration = 0.2f;

    private Quaternion _initialRotation;
    private Coroutine _rotationCoroutine;

    void Start()
    {
        _initialRotation = transform.localRotation;
    }

    public IEnumerator PlayWithRotationCoroutine()
    {
        AudioSource.Play();
        float timer = 0f;

        while (TimeToTalk > timer)
        {
            float angle = Mathf.Abs(Mathf.Sin(timer * RotationSpeed) * RotationAngle);
            transform.localRotation = _initialRotation * Quaternion.Euler(angle, 0, 0);

            timer += Time.deltaTime;
            yield return null;
        }

        Quaternion currentRotation = transform.localRotation;
        timer = 0f;

        while (timer < ReturnDuration)
        {
            timer += Time.deltaTime;
            transform.localRotation = Quaternion.Slerp(currentRotation, _initialRotation, timer / ReturnDuration);
            yield return null;
        }

        transform.localRotation = _initialRotation;
    }
}