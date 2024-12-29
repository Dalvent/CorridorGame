using System.Collections;
using UnityEngine;

public class DestroyAtTime : MonoBehaviour
{
    public float TimeToDestroy = 3.0f;

    void Start()
    {
        StartCoroutine(DestroyAtTime());

        IEnumerator DestroyAtTime()
        {
            yield return new WaitForSeconds(TimeToDestroy);
            Destroy(gameObject);
        }
    }
}