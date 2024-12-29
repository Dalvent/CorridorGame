using UnityEngine;

public class GameRunner : MonoBehaviour
{
    public GameObject BootstrapperPrefab;
    private void Awake()
    {
        var bootstrapper = GameObject.FindWithTag("GameBootstrapper");
        if (bootstrapper == null)
            Instantiate(BootstrapperPrefab);
    }
}