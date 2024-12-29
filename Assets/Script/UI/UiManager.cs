using UnityEngine;

namespace Script.UI
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager Instance => GameBootstrapper.Instance.UiManager;
        
        public GameObject HowToPlayView;
        public GameObject LoadingView;
        public GameObject YouWinView;
        public ScreenFade ScreenFade;
        public AudioSource BackgroundMusic;
        public float FadeOutSeconds = 1.0f;
        
        public void HideLoading()
        {
            LoadingView.SetActive(false);   
        }

        public void ShowYouWin()
        {
            YouWinView.SetActive(true);
        }
        
        public void ShowHowToPlay()
        {
            Debug.Log(nameof(ShowHowToPlay));
            HowToPlayView.SetActive(true);
        }
        
        public void HideHowToPlay()
        {
            Debug.Log(nameof(HideHowToPlay));
            HowToPlayView.SetActive(false);
        }
        
        public void FadeOut()
        {
            ScreenFade.FadeOut(FadeOutSeconds);   
        }
        
        public void FadeIn(float FadeIn)
        {
            ScreenFade.FadeIn(FadeIn);
        }
        
        public void PlayBackgroundMusic()
        {
            BackgroundMusic.Play();
        }
        
        public void StopBackgroundMusic()
        {
            BackgroundMusic.Stop();
            BackgroundMusic.time = 0;
        }
    }
}