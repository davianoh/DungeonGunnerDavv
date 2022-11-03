using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponEquipedDrop : MonoBehaviour, IDropHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image weaponEquipedImage;
    [SerializeField] private int equipedIndex;
    [SerializeField] private int cost;
    [SerializeField] private GameObject costText;
    [SerializeField] private GameObject blocker;
    [SerializeField] private GameObject switchBlocker;
    [SerializeField] private GameObject switchText;


    private void Start()
    {
        if (WeaponMenuUI.Instance.unlockWeaponSlots > equipedIndex)
        {
            gameObject.GetComponent<Image>().color = Color.white;
            costText.SetActive(false);
            blocker.SetActive(false);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Droppedd");
        if(eventData.pointerDrag != null && WeaponMenuUI.Instance.unlockWeaponSlots > equipedIndex && WeaponMenuUI.Instance.weaponOwnedList.Contains(eventData.pointerDrag.GetComponent<WeaponChoice>().weaponDetails.weaponListIndex) && !eventData.pointerDrag.GetComponent<WeaponChoice>().equiped)
        {
            MapManager.Instance.PlayGridClick();
            weaponEquipedImage.sprite = eventData.pointerDrag.GetComponent<WeaponChoice>().weaponImageChoice.sprite;
            if(equipedIndex == 0)
            {
                WeaponMenuUI.Instance.weaponEquiped1 = eventData.pointerDrag.GetComponent<WeaponChoice>().weaponDetails;
            }
            else if (equipedIndex == 1)
            {
                WeaponMenuUI.Instance.weaponEquiped2 = eventData.pointerDrag.GetComponent<WeaponChoice>().weaponDetails;
            }
            else if (equipedIndex == 2)
            {
                WeaponMenuUI.Instance.weaponEquiped3 = eventData.pointerDrag.GetComponent<WeaponChoice>().weaponDetails;
            }
            WeaponMenuUI.Instance.weaponImage.sprite = weaponEquipedImage.sprite;
            WeaponDetailsSO weaponDetails = eventData.pointerDrag.GetComponent<WeaponChoice>().weaponDetails;
            WeaponMenuUI.Instance.weaponStats.text = "Damage : " + weaponDetails.weaponCurrentAmmo.ammoDamage.ToString() + "\nKejelekan Akurasi : " + weaponDetails.weaponCurrentAmmo.ammoSpreadMax.ToString() + "\nJeda Teriak : " + weaponDetails.weaponFireRate.ToString() + "\nKapasitas Teriak : " + weaponDetails.weaponClipAmmoCapacity.ToString();
            WeaponMenuUI.Instance.weaponDescription.text = eventData.pointerDrag.GetComponent<WeaponChoice>().weaponDetails.weaponDescription;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(MapManager.Instance.totalCoinsInGame >= cost && WeaponMenuUI.Instance.unlockWeaponSlots == equipedIndex)
        {
            MapManager.Instance.PlayBuyClick();
            MapManager.Instance.totalCoinsInGame -= cost;
            gameObject.GetComponent<Image>().color = Color.white;
            costText.SetActive(false);
            blocker.SetActive(false);
            WeaponMenuUI.Instance.unlockWeaponSlots++;

            MapManager.Instance.ItemBuyed();
        }
        else if(WeaponMenuUI.Instance.unlockWeaponSlots > equipedIndex)
        {
            if (WeaponMenuUI.Instance.currentActiveWeaponChoice == WeaponMenuUI.Instance.weaponEquiped1 ||
                WeaponMenuUI.Instance.currentActiveWeaponChoice == WeaponMenuUI.Instance.weaponEquiped2 ||
                WeaponMenuUI.Instance.currentActiveWeaponChoice == WeaponMenuUI.Instance.weaponEquiped3)
            {
                if (WeaponMenuUI.Instance.currentActiveWeaponChoice.weaponListIndex == 0 && ((WeaponMenuUI.Instance.unlockWeaponSlots == 3) ||
                    ((WeaponMenuUI.Instance.weaponEquiped1 == WeaponMenuUI.Instance.currentActiveWeaponChoice || WeaponMenuUI.Instance.weaponEquiped2 == WeaponMenuUI.Instance.currentActiveWeaponChoice) && WeaponMenuUI.Instance.unlockWeaponSlots == 2) ||
                    (WeaponMenuUI.Instance.weaponEquiped1 == WeaponMenuUI.Instance.currentActiveWeaponChoice && WeaponMenuUI.Instance.unlockWeaponSlots == 1)))
                {
                    return;
                }
                else if (WeaponMenuUI.Instance.currentActiveWeaponChoice.weaponListIndex != 0)
                {
                    return;
                }
            }
            MapManager.Instance.PlayGridClick();
            weaponEquipedImage.sprite = WeaponMenuUI.Instance.currentActiveWeaponChoice.weaponCurrentAmmo.ammoSprite;
            if (equipedIndex == 0)
            {
                WeaponMenuUI.Instance.weaponEquiped1 = WeaponMenuUI.Instance.currentActiveWeaponChoice;
            }
            else if (equipedIndex == 1)
            {
                WeaponMenuUI.Instance.weaponEquiped2 = WeaponMenuUI.Instance.currentActiveWeaponChoice;
            }
            else if (equipedIndex == 2)
            {
                WeaponMenuUI.Instance.weaponEquiped3 = WeaponMenuUI.Instance.currentActiveWeaponChoice;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(WeaponMenuUI.Instance.unlockWeaponSlots > equipedIndex)
        {
            if(WeaponMenuUI.Instance.currentActiveWeaponChoice == WeaponMenuUI.Instance.weaponEquiped1 || 
                WeaponMenuUI.Instance.currentActiveWeaponChoice == WeaponMenuUI.Instance.weaponEquiped2 || 
                WeaponMenuUI.Instance.currentActiveWeaponChoice == WeaponMenuUI.Instance.weaponEquiped3)
            {
                if (WeaponMenuUI.Instance.currentActiveWeaponChoice.weaponListIndex == 0 && ((WeaponMenuUI.Instance.unlockWeaponSlots == 3) ||
                    ((WeaponMenuUI.Instance.weaponEquiped1 == WeaponMenuUI.Instance.currentActiveWeaponChoice || WeaponMenuUI.Instance.weaponEquiped2 == WeaponMenuUI.Instance.currentActiveWeaponChoice) && WeaponMenuUI.Instance.unlockWeaponSlots == 2) ||
                    (WeaponMenuUI.Instance.weaponEquiped1 == WeaponMenuUI.Instance.currentActiveWeaponChoice && WeaponMenuUI.Instance.unlockWeaponSlots == 1)))
                {
                    return;
                }
                else if (WeaponMenuUI.Instance.currentActiveWeaponChoice.weaponListIndex != 0)
                {
                    return;
                }
            }
            switchBlocker.SetActive(true);
            switchText.SetActive(true);
            transform.localScale += new Vector3(0.1f, 0.1f, 0f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (WeaponMenuUI.Instance.unlockWeaponSlots > equipedIndex)
        {
            switchBlocker.SetActive(false);
            switchText.SetActive(false);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
