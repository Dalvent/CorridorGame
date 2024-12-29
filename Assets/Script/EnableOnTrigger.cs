using UnityEngine;

public class EnableOnTrigger : MonoBehaviour
{
    public GameObject GameObject;

    private void OnTriggerEnter(Collider other)
    {
        GameObject.SetActive(true);
    }
}