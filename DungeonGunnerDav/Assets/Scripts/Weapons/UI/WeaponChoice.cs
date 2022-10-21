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
    [SerializeField] private GameObject buyWeaponText;
    [SerializeField] private GameObject blocker;
    [SerializeField] private GameObject equipedText;
    public bool equiped = false;
    private bool buying = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        weaponImageChoice.sprite = weaponDetails.weaponCurrentAmmo.ammoSprite;
    }


    private void Start()
    {
        if (WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex))
        {
            gameObject.GetComponent<Image>().color = Color.white;
            costWeaponText.SetActive(false);
            blocker.SetActive(false);
        }
    }

    private void Update()
    {
        if (!WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex))
        {
            if(WeaponMenuUI.Instance.currentActiveWeaponChoice == weaponDetails.weaponListIndex)
            {
                costWeaponText.SetActive(false);
                buyWeaponText.SetActive(true);
            }
            else
            {
                costWeaponText.SetActive(true);
                buyWeaponText.SetActive(false);
            }
        }
        else
        {
            if(WeaponMenuUI.Instance.weaponEquiped1 == weaponDetails || WeaponMenuUI.Instance.weaponEquiped2 == weaponDetails || WeaponMenuUI.Instance.weaponEquiped3 == weaponDetails)
            {
                equiped = true;
                equipedText.SetActive(true);
            }
            else
            {
                equiped = false;
                equipedText.SetActive(false);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex) && !buying && !equiped)
        {
            Debug.Log("Begin drag");
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
            isDraged = true;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex) && !buying && !equiped)
        {
            rectTransform.anchoredPosition += eventData.delta / WeaponMenuUI.Instance.canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex) && !buying && !equiped)
        {
            Debug.Log("End drag");
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(MapManager.Instance.totalCoinsInGame >= weaponDetails.weaponCost && !WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex) && WeaponMenuUI.Instance.currentActiveWeaponChoice == weaponDetails.weaponListIndex)
        {
            MapManager.Instance.PlayBuyClick();
            buying = true;
            MapManager.Instance.totalCoinsInGame -= weaponDetails.weaponCost;
            WeaponMenuUI.Instance.weaponOwnedList.Add(weaponDetails.weaponListIndex);
            gameObject.GetComponent<Image>().color = Color.white;
            costWeaponText.SetActive(false);
            buyWeaponText.SetActive(false);
            blocker.SetActive(false);

            MapManager.Instance.ItemBuyed();
        }
        else if (WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex))
        {
            isDraged = false;
            originPosition = rectTransform.anchoredPosition;
        }
        WeaponMenuUI.Instance.currentActiveWeaponChoice = weaponDetails.weaponListIndex;
        MapManager.Instance.PlayGridClick();
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
        if(isDraged && WeaponMenuUI.Instance.weaponOwnedList.Contains(weaponDetails.weaponListIndex))
        {
            rectTransform.anchoredPosition = originPosition;
        }
        WeaponMenuUI.Instance.weaponImage.sprite = weaponDetails.weaponCurrentAmmo.ammoSprite;
        WeaponMenuUI.Instance.weaponDescription.text = weaponDetails.weaponDescription;
        WeaponMenuUI.Instance.weaponStats.text = "Damage : " + weaponDetails.weaponCurrentAmmo.ammoDamage.ToString() + "\nKejelekan Akurasi : " + weaponDetails.weaponCurrentAmmo.ammoSpreadMax.ToString() + "\nJeda Teriak : " + weaponDetails.weaponFireRate.ToString() + "\nKapasitas Teriak : " + weaponDetails.weaponClipAmmoCapacity.ToString();

        buying = false;
    }
}
