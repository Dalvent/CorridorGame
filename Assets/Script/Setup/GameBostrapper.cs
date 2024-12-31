using Script.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBootstrapper : MonoBehaviour
{
    public static GameBootstrapper Instance { get; private set; }
    public UiManager UiManager;
    public InputManager InputManager;
    
    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    private void Start()
    {
        SceneManager.LoadScene("Scenes/Game");
        UiManager.HideLoading();
        UiManager.ShowHowToPlay();
    }
}