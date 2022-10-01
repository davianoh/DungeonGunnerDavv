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

    [SerializeField] private GameObject costWeaponText;
    private bool buying = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        weaponImageChoice.sprite = weaponDetails.weaponSprite;
    }

    private void OnEnable()
    {
        if (WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex))
        {
            gameObject.GetComponent<Image>().color = Color.white;
            costWeaponText.SetActive(false);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex) && !buying)
        {
            Debug.Log("Begin drag");
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
            isDraged = true;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex) && !buying)
        {
            rectTransform.anchoredPosition += eventData.delta / WeaponMenuUI.Instance.canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex) && !buying)
        {
            Debug.Log("End drag");
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(MapManager.Instance.totalCoinsInGame >= weaponDetails.weaponCost && !WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex))
        {
            buying = true;
            MapManager.Instance.totalCoinsInGame -= weaponDetails.weaponCost;
            WeaponMenuUI.Instance.weaponOwnedList.Add(weaponDetails.weaponListIndex);
            gameObject.GetComponent<Image>().color = Color.white;
            costWeaponText.SetActive(false);

            MapManager.Instance.itemBuyed = true;
        }
        else if (WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex))
        {
            isDraged = false;
            originPosition = rectTransform.anchoredPosition;
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

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDraged && WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex))
        {
            WeaponMenuUI.Instance.weaponImage.sprite = weaponDetails.weaponSprite;
            WeaponMenuUI.Instance.weaponDescription.text = weaponDetails.weaponDescription;
            WeaponMenuUI.Instance.weaponStats.text = "Damage : " + weaponDetails.weaponCurrentAmmo.ammoDamage.ToString() + "\nSpread: " + weaponDetails.weaponCurrentAmmo.ammoSpreadMax.ToString() + "\nFire Rate : " + weaponDetails.weaponFireRate.ToString() + "\nCharged: " + weaponDetails.weaponPrechargeTime.ToString() + "\nAmmo capacity : " + weaponDetails.weaponClipAmmoCapacity.ToString();
        }
        else
        {
            if (WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex))
            {
                rectTransform.anchoredPosition = originPosition;
            }
        }
        buying = false;
    }
}
