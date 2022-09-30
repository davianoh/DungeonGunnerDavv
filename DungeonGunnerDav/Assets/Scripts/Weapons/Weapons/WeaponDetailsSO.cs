using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Weapons/Weapons Details")]
public class WeaponDetailsSO : ScriptableObject
{
    #region Header Weapon Base Details
    [Space(10)]
    [Header("WEAPON BASE DETAILS")]
    #endregion
    #region Tooltip
    [Tooltip("Weapon Name")]
    #endregion
    public string weaponName;
    public int weaponListIndex;
    public int weaponCost;

    public string weaponDescription;

    #region Tooltip
    [Tooltip("The sprite for the weapon - the sprite should have the 'generate physics shape' option selected")]
    #endregion
    public Sprite weaponSprite;

    #region Header Weapon Configuration
    [Space(10)]
    [Header("WEAPON CONFIGURATION")]
    #endregion
    #region Tooltip
    [Tooltip("Weapon shoot position - the offset position for the end of the weapon from the sprite pivot point")]
    #endregion
    public Vector3 weaponShootPosition;

    #region Tooltip
    [Tooltip("Weapon current ammo")]
    #endregion
    public AmmoDetailsSO weaponCurrentAmmo;
    public WeaponShootEffectSO weaponShootEffect;

    public SoundEffectSO weaponFiringSoundEffect;
    public SoundEffectSO weaponReloadingSoundEffect;


    #region Header Weapon Operating Values
    [Space(10)]
    [Header("WEAPON OPERATING VALUES")]
    #endregion
    #region Tooltip
    [Tooltip("Select if the weapon has infinite ammo")]
    #endregion
    public bool hasInfiniteAmmo = false;

    #region Tooltip
    [Tooltip("Select if the weapon has infinite clip capacity")]
    #endregion
    public bool hasInfiniteClipCapacity = false;

    #region Tooltipp
    [Tooltip("The weapon capacity - shots before a reload")]
    #endregion
    public int weaponClipAmmoCapacity = 6;

    #region Tooltip
    [Tooltip("Weapon ammo capacity - the max numbers of rounds at that can be held for this weapon")]
    #endregion
    public int weaponAmmoCapacity = 100;

    #region Tooltipp
    [Tooltip("Weapon fire rate - 0.2 means 5 shots a sec")]
    #endregion
    public float weaponFireRate = 0.2f;

    #region Tooltipp
    [Tooltip("Weapon precharge time - time in sec to hold fire button down before firing")]
    #endregion
    public float weaponPrechargeTime = 0f;

    #region Tooltipp
    [Tooltip("Weapon reload time in sec")]
    #endregion
    public float weaponReloadTime = 0f;


    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(weaponName), weaponName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponCurrentAmmo), weaponCurrentAmmo);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponFireRate), weaponFireRate, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponPrechargeTime), weaponPrechargeTime, true);

        if (!hasInfiniteAmmo)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponAmmoCapacity), weaponAmmoCapacity, false);
        }
        if (!hasInfiniteClipCapacity)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponClipAmmoCapacity), weaponClipAmmoCapacity, false);
        }
    }

#endif
    #endregion
}
