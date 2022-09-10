using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeType", menuName = "Scriptable Objects/Dungeon/Room Node Type")]
public class RoomNodeTypeSO : ScriptableObject
{
    public string roomNodeTypeName;

    #region Header
    [Header("Only flag if the room node type should be visible in the graph editor")]
    #endregion Header
    public bool displayInNodeGraphEditor = true;

    #region Header
    [Header("One type should be a corridor")]
    #endregion Header
    public bool isCorridor;

    #region Header
    [Header("One type should be a corridor North or South")]
    #endregion Header
    public bool isCorridorNS;

    #region Header
    [Header("One type should be a corridor East or West")]
    #endregion Header
    public bool isCorridorEW;

    #region Header
    [Header("One type should be an entrance")]
    #endregion Header
    public bool isEntrance;

    #region Header
    [Header("One type should be a boss room")]
    #endregion Header
    public bool isBossRoom;

    #region Header
    [Header("One type should be None (Unassigned)")]
    #endregion Header
    public bool isNone;


    #region Validation
#if UNITY_EDITOR    
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName);
    }
#endif
    #endregion
}
