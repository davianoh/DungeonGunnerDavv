using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponMenuUI : SingletonMonobehaviour<WeaponMenuUI>
{
    public WeaponDetailsSO weaponEquiped1;
    public WeaponDetailsSO weaponEquiped2;
    public WeaponDetailsSO weaponEquiped3;
    public Image weaponEquipedImage1;
    public Image weaponEquipedImage2;
    public Image weaponEquipedImage3;

    #region Header SaveFiles References
    [Header("SAFE FILE REFERENCES")]
    [Space(10)]
    #endregion
    public List<int> weaponOwnedList;
    public List<int> weaponEquipedList;
    public int unlockWeaponSlots;

    #region Header General References
    [Header("General References")]
    [Space(10)]
    #endregion
    public Canvas canvas;
    public Image weaponImage;
    public TextMeshProUGUI weaponStats;
    public TextMeshProUGUI weaponDescription;

    public int currentActiveWeaponSlot;

    protected override void Awake()
    {
        base.Awake();
        LoadWeapons();
    }

    private void Start()
    {
        weaponEquiped1 = GameResources.Instance.weaponList[weaponEquipedList[0]];
        weaponEquiped2 = GameResources.Instance.weaponList[weaponEquipedList[1]];
        weaponEquiped3 = GameResources.Instance.weaponList[weaponEquipedList[2]];
        weaponEquipedImage1.sprite = weaponEquiped1.weaponSprite;
        weaponEquipedImage2.sprite = weaponEquiped2.weaponSprite;
        weaponEquipedImage3.sprite = weaponEquiped3.weaponSprite;

        MapManager.Instance.ItemBuyed();
    }


    public void WeaponMenuUIToggle()
    {
        SaveWeapons();
        MapManager.Instance.Save();

        this.gameObject.SetActive(false);
    }

    public void SaveWeapons()
    {
        List<int> weaponNewEquipedList = new List<int>();
        weaponNewEquipedList.Add(weaponEquiped1.weaponListIndex);
        weaponNewEquipedList.Add(weaponEquiped2.weaponListIndex);
        weaponNewEquipedList.Add(weaponEquiped3.weaponListIndex);
        
        SaveObjectWeapons saveObjectWeapons = new SaveObjectWeapons() { weaponOwnedList = weaponOwnedList, weaponEquipList = weaponNewEquipedList, unlockSlots = unlockWeaponSlots };
        string json = JsonUtility.ToJson(saveObjectWeapons);
        SaveSystem.SaveWeapons(json);
    }

    public void LoadWeapons()
    {
        string saveString = SaveSystem.LoadWeapons();
        if (saveString != null)
        {
            SaveObjectWeapons saveObjectWeapons = JsonUtility.FromJson<SaveObjectWeapons>(saveString);
            weaponOwnedList = saveObjectWeapons.weaponOwnedList;
            weaponEquipedList = saveObjectWeapons.weaponEquipList;
            unlockWeaponSlots = saveObjectWeapons.unlockSlots;
        }
        else
        {
            Debug.Log("No Save");
        }
    }

}
