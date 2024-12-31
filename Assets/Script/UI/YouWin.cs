using UnityEngine;
using UnityEngine.InputSystem;

public class YouWinView : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.Instance.UsePerformed += OnUse;
    }

    private void OnDisable()
    {
        InputManager.Instance.UsePerformed -= OnUse;
    }

    private void OnUse()
    {
        Application.Quit();
    }
}