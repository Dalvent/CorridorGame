using UnityEngine;

public class RunnerForwardEnemy : MonoBehaviour
{
    public float Speed;

    void Update()
    {
        transform.position += transform.forward * (Speed * Time.deltaTime);
    }
}