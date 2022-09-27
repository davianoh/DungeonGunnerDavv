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

    #region Header General References
    [Header("General References")]
    [Space(10)]
    #endregion
    public Canvas canvas;
    public Image weaponImage;
    public TextMeshProUGUI weaponStats;
    public TextMeshProUGUI weaponDescription;

    public int currentActiveWeaponSlot;
}
