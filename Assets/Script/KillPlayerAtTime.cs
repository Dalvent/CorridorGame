using System.Collections;
using Script;
using UnityEngine;

public class KillPlayerAtTime : MonoBehaviour
{
    public float TimeToKillSeconds = 1.9f;
    public AudioSource PlayAtKill;
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        PlayerEnableMediator enableMediator = player.GetComponent<PlayerEnableMediator>();
        enableMediator.DisableAll();

        PlayerDie playerDie = player.GetComponent<PlayerDie>();
        StartCoroutine(KillAtTime(playerDie));
        
        IEnumerator KillAtTime(PlayerDie die)
        {
            yield return new WaitForSeconds(TimeToKillSeconds);
            PlayAtKill.PlayWithRandomPitch();
            die.Die();
        }
    }
}
