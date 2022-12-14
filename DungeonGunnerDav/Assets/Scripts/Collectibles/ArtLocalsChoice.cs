using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArtLocalsChoice : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ArtLocalsSO artLocalsDetail;
    [SerializeField] private Image artChoiceImage;
    [SerializeField] private GameObject blocker;
    private bool unlock = false;

    private void Start()
    {
        if(artLocalsDetail.obtainInLevel < MapManager.Instance.unlockLevelListIndex)
        {
            artChoiceImage.color = Color.white;
            blocker.SetActive(false);
            unlock = true;
        }
        artChoiceImage.sprite = artLocalsDetail.artImage;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(unlock)
        {
            MapManager.Instance.PlayGridClick();
            ArtLocalMenuUI.Instance.artImage.sprite = artLocalsDetail.artImage;
            ArtLocalMenuUI.Instance.artBriefText.text = "Name : " + artLocalsDetail.nama +
                "\nLokasi : " + artLocalsDetail.location +
                "\nTahun : " + artLocalsDetail.year +
                "\nArtist : " + artLocalsDetail.artist;
            ArtLocalMenuUI.Instance.artHistory.text = artLocalsDetail.artHistory;
            ArtLocalMenuUI.Instance.artDetailName.text = artLocalsDetail.nama;
            //ArtLocalMenuUI.Instance.artistHistory.text = artLocalsDetail.artistHistory;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale += new Vector3(0.1f, 0.1f, 0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale -= new Vector3(0.1f, 0.1f, 0f);
    }
}
