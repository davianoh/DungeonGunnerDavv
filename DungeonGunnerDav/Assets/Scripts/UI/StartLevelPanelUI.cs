using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartLevelPanelUI : MonoBehaviour
{
    private Transform panelGraphic;

    private void Awake()
    {
        panelGraphic = transform.GetChild(0);
    }

    private void OnEnable()
    {
        panelGraphic.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        panelGraphic.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetEase(Ease.OutSine).SetUpdate(true);
        Time.timeScale = 0f;
    }


    public void ExitPanel()
    {
        panelGraphic.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 0.1f).SetEase(Ease.InSine).SetUpdate(true).OnComplete(() =>
        {
            Time.timeScale = 1f;
            this.gameObject.SetActive(false);
        });
    }
}
