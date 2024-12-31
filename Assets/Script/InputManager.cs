using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance => GameBootstrapper.Instance.InputManager;

    public Vector2 Look => new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    public bool IsUseDown => Input.GetButtonDown("Jump");
    public bool InUse => Input.GetButton("Jump");
    private bool IsExitDown => Input.GetKeyDown("escape");
    
    private void OnEnable()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        if (IsExitDown)
            Application.Quit();
    }
}
