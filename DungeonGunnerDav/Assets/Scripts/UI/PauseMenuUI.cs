using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI musicLevelText;
    [SerializeField] private TextMeshProUGUI soundsLevelText;
    [SerializeField] private Transform panelGraphic;
    [SerializeField] private GameObject pausePage;
    [SerializeField] private GameObject creditPage;


    private void Start()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(InitialiseUI());
        panelGraphic.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        panelGraphic.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetEase(Ease.OutSine).SetUpdate(true);
        Time.timeScale = 0f;
    }


    public void ExitPauseMenu()
    {
        Time.timeScale = 1f;

        panelGraphic.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 0.1f).SetEase(Ease.InSine).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }


    public void LoadMainMapMenu()
    {
        SceneManager.LoadScene("MainMapScene");
    }

    private IEnumerator InitialiseUI()
    {
        yield return null;
        soundsLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    public void IncreaseMusicVolume()
    {
        MusicManager.Instance.IncreaseMusicVolume();
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    public void DecreaseMusicVolume()
    {
        MusicManager.Instance.DecreaseMusicVolume();
        musicLevelText.SetText(MusicManager.Instance.musicVolume.ToString());
    }

    public void IncreaseSoundsVolume()
    {
        SoundEffectManager.Instance.IncreaseSoundsVolume();
        soundsLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }

    public void DecreaseSoundsVolume()
    {
        SoundEffectManager.Instance.DecreaseSoundsVolume();
        soundsLevelText.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void CreditPageOpen()
    {
        pausePage.SetActive(false);
        creditPage.SetActive(true);
    }

    public void PausePageOpen()
    {
        creditPage.SetActive(false);
        pausePage.SetActive(true);
    }
}
