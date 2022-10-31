using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HowToPlayMenuUI : MonoBehaviour
{
    [SerializeField] private Transform panelGraphic;

    private void OnEnable()
    {
        panelGraphic.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        panelGraphic.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetEase(Ease.OutSine);
    }


    public void ExitTutorialMenu()
    {
        panelGraphic.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 0.1f).SetEase(Ease.InSine).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
