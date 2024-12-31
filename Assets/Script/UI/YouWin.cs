using UnityEngine;
using UnityEngine.InputSystem;

public class YouWinView : MonoBehaviour
{
    private void Update()
    {
        if (InputManager.Instance.IsUseDown)
            Application.Quit();
    }
}