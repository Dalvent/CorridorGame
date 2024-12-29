using System.Collections;
using Script;
using UnityEngine;
using Random = UnityEngine.Random;

public class DoorOpeningEvent : MonoBehaviour
{
    public Door Door;
    public enum OpeningEventType { None, KnockKnock, RunningBro }
    public OpeningEventType EventType;

    public bool IsProcessed
    {
        get
        {
            if (Door.IsLocked)
                return true;

            if (EventType == OpeningEventType.KnockKnock)
                return true;

            return Door.IsOpened;
        }
    }
    
    [Header("KnockKnock")]
    public EventTrigger KnockKnockTrigger;
    public Transform KnockKnockScreamerPosition;
    public GameObject KnockKnockScreamerPrefab;
    public float KnockKnockSoundDelayMin = 1.0f;
    public float KnockKnockSoundDelayMax = 6.0f;
    public AudioSource KnockKnockSound;

    [Header("Running Bro")] 
    public Transform RunningBroPosition;
    public GameObject RunningBroPrefab;
    public Transform[] RunningBroTargets;
    
    private void OnEnable()
    {
        Door.BeginOpening += OnBeginOpening;
        Door.EndOpening += OnEndOpening;
        KnockKnockTrigger.TriggerEnter += OnKnockKnockTriggerEnter;
    }

    private void OnDisable()
    {
        Door.BeginOpening -= OnBeginOpening;
        Door.EndOpening -= OnEndOpening;
        KnockKnockTrigger.TriggerEnter -= OnKnockKnockTriggerEnter;
    }

    private void OnKnockKnockTriggerEnter(Collider obj)
    {
        if (EventType == OpeningEventType.KnockKnock)
            StartCoroutine(PlayKnockKnockSound());
    }

    private IEnumerator PlayKnockKnockSound()
    {
        yield return new WaitForSeconds(Random.Range(KnockKnockSoundDelayMin, KnockKnockSoundDelayMax));
        KnockKnockSound.PlayWithRandomPitch(0.75f, 1.2f);
    }

    private void OnBeginOpening()
    {
        switch (EventType)
        {
            case OpeningEventType.KnockKnock:
                Instantiate(KnockKnockScreamerPrefab, KnockKnockScreamerPosition);
                break;
            case OpeningEventType.RunningBro:
                var runningBro = Instantiate(RunningBroPrefab, RunningBroPosition);
                runningBro.GetComponent<WalkToTargets>().Targets = RunningBroTargets;
                break;
        }
    }

    private void OnEndOpening()
    {
    }
}