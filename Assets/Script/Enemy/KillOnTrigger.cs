using UnityEngine;

public class KillOnTrigger : MonoBehaviour
{
    public EventTrigger EventTrigger;
    public AudioSource KillAudio;

    private void OnEnable()
    {
        EventTrigger.TriggerEnter += OnTriggerEnter;
    }

    private void OnDisable()
    {
        EventTrigger.TriggerEnter -= OnTriggerEnter;
    }

    private void OnTriggerEnter(Collider obj)
    {
        obj.GetComponent<PlayerDie>().Die();
        
        if (KillAudio != null)
            KillAudio.Play();
    }
}