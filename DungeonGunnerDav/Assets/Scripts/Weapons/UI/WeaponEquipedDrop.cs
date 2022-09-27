using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponEquipedDrop : MonoBehaviour, IDropHandler
{
    [SerializeField] private Image weaponEquipedImage;
    [SerializeField] private int equipedIndex;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Droppedd");
        if(eventData.pointerDrag != null)
        {
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
            WeaponMenuUI.Instance.weaponStats.text = "Damage : " + weaponDetails.weaponCurrentAmmo.ammoDamage.ToString() + "\nSpread: " + weaponDetails.weaponCurrentAmmo.ammoSpreadMax.ToString() + "\nFire Rate : " + weaponDetails.weaponFireRate.ToString() + "\nCharged: " + weaponDetails.weaponPrechargeTime.ToString() + "\nAmmo capacity : " + weaponDetails.weaponClipAmmoCapacity.ToString();
            WeaponMenuUI.Instance.weaponDescription.text = eventData.pointerDrag.GetComponent<WeaponChoice>().weaponDetails.weaponDescription;
        }
    }
}
