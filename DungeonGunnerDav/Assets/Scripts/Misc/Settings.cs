using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region Units
    public const float pixelsPerUnit = 16f;
    public const float tileSizePixels = 16f;
    #endregion

    #region Dungeon Build Settings
    public const int maxDungeonRebuildAttempsForRoomGraph = 1000;
    public const int maxDungeonBuildAttemps = 10;
    #endregion

    #region Room Settings
    public const float fadeInTime = 0.5f;
    public const int maxChildCorridors = 3;
    public const float doorUnlockDelay = 1f;
    #endregion

    #region Animator Parameters
    // Animator parameters - Player
    public static int aimUp = Animator.StringToHash("aimUp");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int aimUpRight = Animator.StringToHash("aimUpRight");
    public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int aimLeft = Animator.StringToHash("aimLeft");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int rollUp = Animator.StringToHash("rollUp");
    public static int rollDown = Animator.StringToHash("rollDown");
    public static int rollLeft = Animator.StringToHash("rollLeft");
    public static int rollRight = Animator.StringToHash("rollRight");
    public static float baseSpeedPlayerAnimation = 8f;
    public static float enemyWarningSpawnTime = 3f;
    public static float enemyWarningSpawnInterval = 0.2f;

    // Animator parameters - Enemies
    public static float baseSpeedEnemyAnimation = 8f;

    // Animator parameters - Door
    public static int open = Animator.StringToHash("open");
    #endregion

    #region GameObjects Tags
    public const string playerTag = "Player";
    public const string playerWeapon = "playerWeapon";
    #endregion

    #region Audio
    public const float musicFadeOutTime = 0.5f;
    public const float musicFadeInTime = 0.5f;
    #endregion

    #region Firing Control
    public const float useAimAngleDistance = 3.5f; //if target distance less than this, than the aim angle will be used, else weapon aim angle will be used
    #endregion

    #region AStar PathFinding Parameters
    public const int defaultAStarMovementPenalty = 40;
    public const int preferredPathAStarMovementPenalty = 1;
    public const float playerMoveDistanceToRebuildPath = 3f;
    public const float enemyPathRebuildCooldown = 2f;
    public const int targetFrameRateToSpreadPathfindingOver = 60;
    #endregion

    #region UI Parameters
    public const float uiAmmoIconSpacing = 4f;
    public const float uiHeartSpacing = 16f;
    #endregion

    #region Enemy Parameters
    public const int defaultEnemyHealth = 20;
    #endregion

    #region Contact Damage Parameters
    public const float contactDamageCollisionResetDelay = 0.5f;
    #endregion

    #region Upgrade Stats Player Parameters
    public const int healthUpgradeMultiplier = 10;
    public const int attackUpgradeMultiplier = 2;
    public const int speedUpgradeMultiplier = 1;
    public const int coinsUpgradeMultiplier = 3;
    #endregion
}
