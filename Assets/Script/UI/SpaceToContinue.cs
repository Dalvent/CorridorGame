using System.Collections;
using Script.UI;
using UnityEngine;

public class SpaceToContinue : MonoBehaviour
{
    // Hack:
    private bool _canUse;

    private void OnEnable()
    {
        _canUse = true;
    }

    private void OnDisable()
    {
        _canUse = false;
    }

    private void Update()
    {
        if (!_canUse)
            return;
        
        if (!InputManager.Instance.IsUseDown)
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