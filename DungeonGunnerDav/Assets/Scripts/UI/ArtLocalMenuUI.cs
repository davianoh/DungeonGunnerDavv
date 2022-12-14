using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ArtLocalMenuUI : SingletonMonobehaviour<ArtLocalMenuUI>
{
    public Image artImage;
    public TextMeshProUGUI artBriefText;
    public TextMeshProUGUI artHistory;
    public TextMeshProUGUI artDetailName;
    //public TextMeshProUGUI artistHistory;
    [SerializeField] private Transform panelGraphic;

    private void OnEnable()
    {
        panelGraphic.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        panelGraphic.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetEase(Ease.OutSine);
    }


    public void ExitArtLocalMenu()
    {
        panelGraphic.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 0.1f).SetEase(Ease.InSine).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
}
