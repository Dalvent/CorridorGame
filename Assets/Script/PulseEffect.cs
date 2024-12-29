using System.Collections;
using Script;
using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    public Transform Target;

    [Header("Settings")] public float TargetScale = 4.5f;
    public float MoveDistance = 3f;
    public float Speed = 3f;
    public float DelayToStart = 0.9f;

    public AudioSource StartSound;

    void Start()
    {
        var initialScale = Target.localScale;
        var initialPosition = Target.position;

        StartCoroutine(ScalingAndMoving());

        IEnumerator ScalingAndMoving()
        {
            yield return new WaitForSeconds(DelayToStart);
            
            StartSound.PlayWithRandomPitch();
            
            while (true)
            {
                Target.localScale = Vector3.Lerp(Target.localScale, initialScale * TargetScale, Speed * Time.deltaTime);

                Target.position = Vector3.Lerp(Target.position, initialPosition + Target.forward * MoveDistance, Speed * Time.deltaTime);

                if (HasReachedTarget(initialScale, initialPosition))
                    yield break;

                yield return null;
            }
        }
    }

    private bool HasReachedTarget(Vector3 initialScale, Vector3 initialPosition)
    {
        bool scaleReached = Vector3.Distance(Target.localScale, initialScale * TargetScale) < 0.01f;
        bool positionReached = Vector3.Distance(Target.position, initialPosition + Target.forward * MoveDistance) < 0.01f;
        return scaleReached && positionReached;
    }
}