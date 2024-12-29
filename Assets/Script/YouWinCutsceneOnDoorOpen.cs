using System.Collections;
using Script.UI;
using UnityEngine;

public class YouWinCutsceneOnDoorOpen : MonoBehaviour
{
    public Door Door;
    public LoopRotateWhileAudioPlay LoopRotateWhileAudioPlay;
    private float DelayBeforeTalkSeconds = 0.2f;
    private float DelayAfterTalkSecondsToFade = 0.5f;
    private float YouWinFadeSeconds = 1.0f;

    private PlayerEnableMediator _player;
    private DummyTimer _dummyTimer;

    // TODO: Make it init in normal way
    public void Init(PlayerEnableMediator player, DummyTimer dummyTimer)
    {
        _player = player;
        _dummyTimer = dummyTimer;
    }
    
    public void OnEnable()
    {
        Door.BeginOpening += OnBeginOpening;
        Door.EndOpening += OnEndOpening;
    }

    public void OnDisable()
    {
        Door.BeginOpening -= OnBeginOpening;
        Door.EndOpening -= OnEndOpening;
    }

    private void OnBeginOpening()
    {
        _dummyTimer.gameObject.SetActive(false);
        _player.DisableAll();
    }

    private void OnEndOpening()
    {
        StartCoroutine(ShowCutsceneAndYouWin());
        
        // actually, it's not cutscene 🤓.
        IEnumerator ShowCutsceneAndYouWin()
        {
            yield return new WaitForSeconds(DelayBeforeTalkSeconds);
            yield return LoopRotateWhileAudioPlay.PlayWithRotationCoroutine();
            yield return new WaitForSeconds(DelayAfterTalkSecondsToFade);
            
            UiManager.Instance.FadeIn(YouWinFadeSeconds);
            yield return new WaitForSeconds(UiManager.Instance.FadeOutSeconds);
            
            UiManager.Instance.ShowYouWin();
        }
    }
}