using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class StartScreenUI : SingletonMonobehaviour<StartScreenUI>
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Transform playButton;
    private float duration = 0.5f;
    private bool isShaking = false;
    

    private void Start()
    {
        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMapMusic);
    }

    public void StartGame()
    {
        title.transform.DOKill();
        playButton.DOKill();
        SaveSystem.Init();
    }

    public void ShakeTitle()
    {
        if (isShaking) return;
        isShaking = true;

        title.transform.DOShakePosition(0.75f, duration);
        title.transform.DOShakeRotation(0.75f, duration);
        title.transform.DOShakeScale(0.75f, duration);
        Invoke("isShakingToFalse", duration * 1.5f);
    }

    public void IdlePlayButton()
    {
        playButton.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutElastic);
    }

    public void isShakingToFalse()
    {
        isShaking = false;
    }
}
