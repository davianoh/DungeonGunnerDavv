using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Tilemaps;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    public static GameResources Instance
    {
        get
        {
            if(instance == null)
            {
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }

    #region Header DUNGEON
    [Space(10)]
    [Header("DUNGEON")]
    #endregion
    #region Tooltip
    [Tooltip("Populate w/ the dungeon RoomNodeTypeListSO")]
    #endregion
    public RoomNodeTypeListSO roomNodeTypeList;
    public List<Room> roomList;

    #region Header Player Selection
    [Space(10)]
    [Header("PLAYER SELECTION")]
    #endregion
    public GameObject playerSelectionPrefab;

    #region Header Player
    [Space(10)]
    [Header("PLAYER")]
    #endregion
    #region Tooltip
    [Tooltip("The current player SO, this is used to reference the current player between scenes")]
    #endregion
    public CurrentPlayerSO currentPlayer;
    public List<PlayerDetailsSO> playerDetailsList;
    // List of all available weapons in the game. Order is important
    public List<WeaponDetailsSO> weaponList = new List<WeaponDetailsSO>();
    public int healthBonus;
    public int attackBonus;
    public int speedBonus;
    public int coinsBonus;
    public int selectedLevelIndex;

    #region Header Music
    [Space(10)]
    [Header("MUSIC")]
    #endregion
    public AudioMixerGroup musicMasterMixerGroup;
    public AudioMixerSnapshot musicOnFullSnapshot;
    public AudioMixerSnapshot musicOnLowSnapshot;
    public AudioMixerSnapshot musicOffSnapshot;
    public MusicTrackSO mainMapMusic;
    public MusicTrackSO map1Music;
    public MusicTrackSO map2Music;

    public SoundEffectSO buttonClick;
    public SoundEffectSO gridClick;
    public SoundEffectSO buyClick;

    #region Header Materials
    [Space(10)]
    [Header("MATERIALS")]
    #endregion
    #region Tooltip
    [Tooltip("Dimmed Material")]
    #endregion
    public Material dimmedMaterial;

    #region Tooltip
    [Tooltip("Sprite-Lit-Default Material")]
    #endregion
    public Material litMaterial;

    #region Tooltip
    [Tooltip("Populate w/ the Variable Lit Shader")]
    #endregion
    public Shader variableLitShader;

    #region Header Special TileMap Tiles
    [Space(10)]
    [Header("SPECIAL TILEMAP TILES")]
    #endregion
    public TileBase[] enemyUnwalkableCollisionTilesArray;
    public TileBase preferredEnemyPathTile;

    #region Header UI
    [Space(10)]
    [Header("UI")]
    #endregion
    #region Tooltip
    [Tooltip("Populate w/ ammo icon prefab")]
    #endregion
    public GameObject ammoIconPrefab;
    public GameObject heartPrefab;

    #region Header SOUNDS
    [Space(10)]
    [Header("SOUNDS")]
    #endregion
    #region Tooltip
    [Tooltip("Populate w/ Sounds Master Mixer Group")]
    #endregion
    public AudioMixerGroup soundsMasterMixerGroup;

    public SoundEffectSO doorOpenCloseSoundEffect;

    #region Header COLLECTIBLES
    [Space(10)]
    [Header("COLLECTIBLES")]
    #endregion
    public GameObject coins;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(roomNodeTypeList), roomNodeTypeList);
        HelperUtilities.ValidateCheckNullValue(this, nameof(currentPlayer), currentPlayer);
        HelperUtilities.ValidateCheckNullValue(this, nameof(litMaterial), litMaterial);
        HelperUtilities.ValidateCheckNullValue(this, nameof(dimmedMaterial), dimmedMaterial);
        HelperUtilities.ValidateCheckNullValue(this, nameof(variableLitShader), variableLitShader);
        HelperUtilities.ValidateCheckNullValue(this, nameof(ammoIconPrefab), ammoIconPrefab);
        HelperUtilities.ValidateCheckNullValue(this, nameof(doorOpenCloseSoundEffect), doorOpenCloseSoundEffect);
        HelperUtilities.ValidateCheckNullValue(this, nameof(soundsMasterMixerGroup), soundsMasterMixerGroup);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(enemyUnwalkableCollisionTilesArray), enemyUnwalkableCollisionTilesArray);
        HelperUtilities.ValidateCheckNullValue(this, nameof(preferredEnemyPathTile), preferredEnemyPathTile);
    }

#endif
    #endregion
}
