using Script.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrapper : MonoBehaviour
{
    public static GameBootstrapper Instance { get; private set; }
    public UiManager UiManager;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    private async void Start()
    {
        await SceneManager.LoadSceneAsync("Scenes/Game");
        UiManager.HideLoading();
        UiManager.ShowHowToPlay();
    }
}