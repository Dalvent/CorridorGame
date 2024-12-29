using System.Threading.Tasks;
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
    
    public async void Die()
    {
        if (_isDead)
            return;

        _isDead = true;
        
        EnableMediator.DisableAll();
        SteppingSystem.ForceStop();
        Rigidbody.isKinematic = false;
        Rigidbody.AddForce(transform.forward * DieFallPower);
        
        PlayWithFade.PlayWithRandomPitch();
        await Task.Delay((int)(TimeToShowFade * 1000));
        UiManager.Instance.FadeIn(FadeTime);
        await Task.Delay((int)(FadeTime * 1000));

        await SceneManager.LoadSceneAsync("Scenes/Game");
        UiManager.Instance.ShowHowToPlay();
    }
}