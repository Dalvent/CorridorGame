using UnityEngine;

namespace Script
{
    public static class AudioSourceExtensions
    {
        public static void PlayWithRandomPitch(this AudioSource audioSource, float minPitch = 0.9f, float maxPitch = 1.1f)
        {
            if (audioSource == null)
            {
                Debug.LogError("AudioSource is null. Cannot play audio.");
                return;
            }

            if (minPitch > maxPitch)
            {
                Debug.LogError("minPitch cannot be greater than maxPitch.");
                return;
            }

            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.Play();
        }
    }
}