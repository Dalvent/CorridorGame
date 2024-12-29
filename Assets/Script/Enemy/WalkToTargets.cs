using UnityEngine;

public class WalkToTargets : MonoBehaviour
{
    public float Speed = 3f;
    public Transform[] Targets;
    private int _currentTarget;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Targets[_currentTarget].position, Speed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, Targets[_currentTarget].position) >= 0.01f) 
            return;

        if (Targets.Length > _currentTarget + 1)
            _currentTarget++;
        else
            Destroy(gameObject);
    }
}