using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMapUI : MonoBehaviour
{
    [SerializeField] private Transform museum1;
    [SerializeField] private Transform museum2;
    [SerializeField] private Transform logo;

    private void Start()
    {
        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMapMusic, 0f, 2f);
        LogoTween();
        Museum1Tween();
        if(MapManager.Instance.unlockLevelListIndex >= 5)
        {
            Museum2Tween();
        }
    }

    public void PlayLevel(int levelIndex)
    {
        GameResources.Instance.selectedLevelIndex = levelIndex;
        MapManager.Instance.Save();
        SceneManager.LoadScene("MainGameScene");
    }

    public void Museum1Tween()
    {
        museum1.DOScaleX(1.03f, 0.75f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
        museum1.DOScaleY(0.97f, 0.75f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
    }

    public void Museum2Tween()
    {
        museum2.DOScaleX(1.03f, 0.75f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
        museum2.DOScaleY(0.97f, 0.75f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
    }

    public void LogoTween()
    {
        logo.DOScaleY(1.04f, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
        logo.DOScaleX(0.96f, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
    }


}
