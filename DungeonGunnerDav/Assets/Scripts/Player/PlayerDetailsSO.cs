using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails_", menuName = "Scriptable Objects/Player/Player Details")]
public class PlayerDetailsSO : ScriptableObject
{
    #region Header Player Base Details
    [Space(10)]
    [Header("PLAYER BASE DETAILS")]
    #endregion
    #region ToolTip
    [Tooltip("Player character name")]
    #endregion
    public string playerCharacterName;
    public string playerDescription;
    public MovementDetailsSO playerMovementDetails;
    public int playerBonusAttack;
    public int playerBonusCoins;

    #region Tooltip
    [Tooltip("Prefab gameObject for the player")]
    #endregion
    public GameObject playerPrefabs;

    #region Tooltip
    [Tooltip("Player runtime animator controller")]
    #endregion
    public RuntimeAnimatorController runtimeAnimatorController;

    #region Header Health
    [Space(10)]
    [Header("HEALTH")]
    #endregion
    #region Tooltip
    [Tooltip("Player Starting health ammount")]
    #endregion
    public int playerHealthAmmount;
    public bool isImmuneAfterHit = false;
    public float hitImmunityTime;

    #region Header Weapon
    [Space(10)]
    [Header("WEAPON")]
    #endregion
    #region Tooltip
    [Tooltip("Player starting weapon")]
    #endregion
    public WeaponDetailsSO startingWeapon;

    #region Tooltip
    [Tooltip("Populate w/ the list of starting weapon")]
    #endregion
    public List<WeaponDetailsSO> startingWeaponList;

    #region Header Other
    [Space(10)]
    [Header("OTHER")]
    #endregion
    #region Tooltip
    [Tooltip("Player icon sprite to be used in the minimap")]
    #endregion
    public Sprite playerMinimapIcon;

    #region Tooltip
    [Tooltip("Player hand sprite")]
    #endregion
    public Sprite playerHandSprite;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(playerCharacterName), playerCharacterName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerPrefabs), playerPrefabs);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(playerHealthAmmount), playerHealthAmmount, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(startingWeapon), startingWeapon);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(startingWeaponList), startingWeaponList);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerMinimapIcon), playerMinimapIcon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandSprite), playerHandSprite);
        HelperUtilities.ValidateCheckNullValue(this, nameof(runtimeAnimatorController), runtimeAnimatorController);

        if (isImmuneAfterHit)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(hitImmunityTime), hitImmunityTime, false);
        }
    }

#endif
    #endregion
}
