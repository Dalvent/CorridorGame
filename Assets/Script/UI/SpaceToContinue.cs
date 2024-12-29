using System.Threading.Tasks;
using Script.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceToContinue : MonoBehaviour
{
    private InputSystem _inputSystem;
    
    // Hack:
    private bool _canUse;

    private void Awake()
    {
        _inputSystem = new InputSystem();
    }

    private void OnEnable()
    {
        _inputSystem.Player.Use.performed += OnUse;
        _inputSystem.Enable();
        _canUse = true;
    }

    private void OnDisable()
    {
        _inputSystem.Player.Use.performed -= OnUse;
        _inputSystem.Disable();
        _canUse = false;
    }

    private async void OnUse(InputAction.CallbackContext obj)
    {
        if (!_canUse)
            return;
        
        _canUse = false;
        
        var player = GameObject.FindGameObjectWithTag("Player");
        
        UiManager.Instance.HideHowToPlay();
        UiManager.Instance.FadeOut();

        await Task.Delay((int)(UiManager.Instance.FadeOutSeconds * 1000));
        
        player.GetComponent<PlayerEnableMediator>().EnableAtForward();
    }
}