using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

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

    //public int currentActiveWeaponChoice = 99;
    public WeaponDetailsSO currentActiveWeaponChoice;
    [SerializeField] private Transform panelGraphic;

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
        weaponEquipedImage1.sprite = weaponEquiped1.weaponCurrentAmmo.ammoSprite;
        weaponEquipedImage2.sprite = weaponEquiped2.weaponCurrentAmmo.ammoSprite;
        weaponEquipedImage3.sprite = weaponEquiped3.weaponCurrentAmmo.ammoSprite;

        currentActiveWeaponChoice = GameResources.Instance.weaponList[0];
        MapManager.Instance.ItemBuyed();
    }

    private void OnEnable()
    {
        panelGraphic.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        panelGraphic.DOScale(new Vector3(1f, 1f, 1f), 0.2f).SetEase(Ease.OutSine);
    }


    public void WeaponMenuUIToggle()
    {
        SaveWeapons();
        MapManager.Instance.Save();
        panelGraphic.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 0.1f).SetEase(Ease.InSine).OnComplete(() =>
        {
            this.gameObject.SetActive(false);
        });
        
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
