using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyWeaponAI : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform weaponShootPosition;
    private Enemy enemy;
    private EnemyDetailsSO enemyDetails;
    private float firingIntervalTimer;
    private float firingDurationTimer;

    private EnemyMovementAI enemyMovementAI;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        enemyDetails = enemy.enemyDetails;
        firingIntervalTimer = WeaponShootInterval();
        firingDurationTimer = WeaponShootDuration();
        enemyMovementAI = GetComponent<EnemyMovementAI>();
    }

    private float WeaponShootDuration()
    {
        return Random.Range(enemyDetails.firingDurationMin, enemyDetails.firingDurationMax);
    }

    private float WeaponShootInterval()
    {
        return Random.Range(enemyDetails.firingIntervalMin, enemyDetails.firingIntervalMax);
    }

    private void Update()
    {
        firingIntervalTimer -= Time.deltaTime;

        if(firingIntervalTimer < 0f)
        {
            if(firingDurationTimer >= 0f)
            {
                firingDurationTimer -= Time.deltaTime;
                FireWeapon();
            }
            else
            {
                firingIntervalTimer = WeaponShootInterval();
                firingDurationTimer = WeaponShootDuration();
            }
        }
    }

    private void FireWeapon()
    {
        Vector3 playerDirectionVector = GameManager.Instance.GetPlayer().GetPlayerPosition() - transform.position;
        Vector3 weaponDirection = (GameManager.Instance.GetPlayer().GetPlayerPosition() - weaponShootPosition.position);
        Vector3 artDirection = (GameManager.Instance.GetCurrentRoom().artLocalPosition.position - weaponShootPosition.position);

        float weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);
        float enemyAngleDegrees = HelperUtilities.GetAngleFromVector(playerDirectionVector);
        float artAngleDegrees = HelperUtilities.GetAngleFromVector(artDirection);

        AimDirection enemyAimDirection = new AimDirection();
        if (enemyMovementAI.targetArt)
        {
            AimDirection enemyDirection = HelperUtilities.GetAimDirection(artAngleDegrees);
            enemy.aimWeaponEvent.CallAimWeaponEvent(enemyDirection, enemyAngleDegrees, weaponAngleDegrees, weaponDirection);
        }
        else
        {
            enemyAimDirection = HelperUtilities.GetAimDirection(enemyAngleDegrees);
            enemy.aimWeaponEvent.CallAimWeaponEvent(enemyAimDirection, enemyAngleDegrees, weaponAngleDegrees, weaponDirection);
        }

        if(enemyDetails.enemyWeapon != null && !enemyMovementAI.targetArt)
        {
            float enemyAmmoRange = enemyDetails.enemyWeapon.weaponCurrentAmmo.ammoRange;

            if(playerDirectionVector.magnitude <= enemyAmmoRange)
            {
                if (enemyDetails.firingLineOfSightRequired && !IsPlayerInLineOfSight(weaponDirection, enemyAmmoRange)) return;

                enemy.fireWeaponEvent.CallFireWeaponEvent(true, true, enemyAimDirection, enemyAngleDegrees, weaponAngleDegrees, weaponDirection);
            }
        }
    }

    private bool IsPlayerInLineOfSight(Vector3 weaponDirection, float enemyAmmoRange)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(weaponShootPosition.position, (Vector2)weaponDirection, enemyAmmoRange, layerMask);
        if(raycastHit2D && raycastHit2D.transform.CompareTag(Settings.playerTag))
        {
            return true;
        }

        return false;
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponShootPosition), weaponShootPosition);
    }

#endif
    #endregion

}
