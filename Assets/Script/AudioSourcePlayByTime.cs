using System.Collections;
using Script;
using UnityEngine;

public class AudioSourcePlayByTime : MonoBehaviour
{
    public AudioSource AudioSource;
    public float DelayToPlay;
    public float LoopDelay;
    public bool IsLooped;

    private void Start()
    {
        StartCoroutine(PlaySound());
        
        IEnumerator PlaySound()
        {
            yield return new WaitForSeconds(DelayToPlay);
            AudioSource.PlayWithRandomPitch();
            
            while (IsLooped)
            {
                AudioSource.PlayWithRandomPitch();
                yield return new WaitForSeconds(LoopDelay);
            }
        }
    }
}