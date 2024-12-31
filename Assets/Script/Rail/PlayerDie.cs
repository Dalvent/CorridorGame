using System.Collections;
using Script;
using Script.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDie : MonoBehaviour
{
    public PlayerEnableMediator EnableMediator;
    public SteppingSystem SteppingSystem;
    public Rigidbody Rigidbody;
    public float DieFallPower = 10f;
    public float TimeToShowFade = 0.5f;
    public float FadeTime = 0.5f;
    
    public AudioSource PlayWithFade;

    private bool _isDead;
    
    public void Die()
    {
        if (_isDead)
            return;

        StartCoroutine(StartDie());

        IEnumerator StartDie()
        {
            EnableMediator.DisableAll();
            SteppingSystem.ForceStop();
            
            Rigidbody.isKinematic = false;
            Rigidbody.AddForce(transform.forward * DieFallPower);
            
            PlayWithFade.PlayWithRandomPitch();
            yield return new WaitForSeconds(TimeToShowFade);
            
            UiManager.Instance.FadeIn(FadeTime);
            yield return new WaitForSeconds(FadeTime);
            
            UiManager.Instance.StopBackgroundMusic();
            
            SceneManager.LoadScene("Scenes/Game");
            UiManager.Instance.ShowHowToPlay();
        }
    }
}