using System.Collections;
using Script.UI;
using UnityEngine;

public class SpaceToContinue : MonoBehaviour
{
    // Hack:
    private bool _canUse;

    private void OnEnable()
    {
        InputManager.Instance.UsePerformed += OnUse;
        _canUse = true;
    }

    private void OnDisable()
    {
        InputManager.Instance.UsePerformed -= OnUse;
        _canUse = false;
    }

    private void OnUse()
    {
        if (!_canUse)
            return;

        _canUse = false;
        
        UiManager.Instance.StartCoroutine(StartUse());
        
        IEnumerator StartUse()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            UiManager.Instance.HideHowToPlay();
            UiManager.Instance.PlayBackgroundMusic();
            UiManager.Instance.FadeOut();

            yield return new WaitForSeconds(UiManager.Instance.FadeOutSeconds);

            player.GetComponent<PlayerEnableMediator>().EnableAtForward();
        }
    }
}