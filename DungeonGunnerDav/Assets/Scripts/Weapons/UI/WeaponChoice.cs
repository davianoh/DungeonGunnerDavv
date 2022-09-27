using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class WeaponChoice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public WeaponDetailsSO weaponDetails;
    public Image weaponImageChoice;
    private RectTransform rectTransform;
    private bool isDraged = false;
    private Vector3 originPosition;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        weaponImageChoice.sprite = weaponDetails.weaponSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        isDraged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / WeaponMenuUI.Instance.canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End drag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDraged = false;
        originPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale += new Vector3(0.1f, 0.1f, 0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale -= new Vector3(0.1f, 0.1f, 0f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDraged)
        {
            WeaponMenuUI.Instance.weaponImage.sprite = weaponDetails.weaponSprite;
            WeaponMenuUI.Instance.weaponDescription.text = weaponDetails.weaponDescription;
            WeaponMenuUI.Instance.weaponStats.text = "Damage : " + weaponDetails.weaponCurrentAmmo.ammoDamage.ToString() + "\nSpread: " + weaponDetails.weaponCurrentAmmo.ammoSpreadMax.ToString() + "\nFire Rate : " + weaponDetails.weaponFireRate.ToString() + "\nCharged: " + weaponDetails.weaponPrechargeTime.ToString() + "\nAmmo capacity : " + weaponDetails.weaponClipAmmoCapacity.ToString();
        }
        else
        {
            if (true)
            {
                rectTransform.anchoredPosition = originPosition;
            }
        }
    }
}
