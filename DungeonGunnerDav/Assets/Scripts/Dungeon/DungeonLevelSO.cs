using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevel_", menuName = "Scriptable Objects/Dungeon/Dungeon Level")]
public class DungeonLevelSO : ScriptableObject
{
    #region Header Basic Level Detail
    [Space(10)]
    [Header("BASIC LEVEL DETAIL")]
    #endregion
    #region Tooltip
    [Tooltip("The name of the level")]
    #endregion

    public string levelName;

    #region Header Room Templates for level
    [Space(10)]
    [Header("ROOM TEMPLATES FOR LEVEL")]
    #endregion
    #region Tooltip
    [Tooltip("Populate the list w/ the room templates that you want to be part of the lvl. You need to ensure that room templates are included" +
        " for all room node types that are specified in the room node graphs for the lvl")]
    #endregion

    public List<RoomTemplateSO> roomTemplateList;

    #region Header Room Node Graphs for level
    [Space(10)]
    [Header("ROOM NODE GRAPHS FOR THE LEVEL")]
    #endregion
    #region Tooltip
    [Tooltip("Populate this list w/ the room node graphs which should be randomly selected from, for the level")]
    #endregion

    public List<RoomNodeGraphSO> roomNodeGraphList;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(levelName), levelName);
        if(HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomTemplateList), roomTemplateList))
        {
            return;
        }
        if (HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomNodeGraphList), roomNodeGraphList))
        {
            return;
        }

        bool isEWCorridor = false;
        bool isNSCorridor = false;
        bool isEntrance = false;
        foreach(RoomTemplateSO roomTemplateSO in roomTemplateList)
        {
            if (roomTemplateSO == null)
                return;
            if (roomTemplateSO.roomNodeType.isCorridorEW)
                isEWCorridor = true;
            if (roomTemplateSO.roomNodeType.isCorridorNS)
                isNSCorridor = true;
            if (roomTemplateSO.roomNodeType.isEntrance)
                isEntrance = true;
        }
        if (isEWCorridor == false)
            Debug.Log("In " + this.name.ToString() + " : No E/W Corridor Room Type Spesified");
        if (isNSCorridor == false)
            Debug.Log("In " + this.name.ToString() + " : No N/S Corridor Room Type Spesified");
        if (isEntrance == false)
            Debug.Log("In " + this.name.ToString() + " : No Entrance Room Type Spesified");

        foreach(RoomNodeGraphSO roomNodeGraph in roomNodeGraphList)
        {
            if (roomNodeGraph == null)
                return;
            foreach(RoomNodeSO roomNodeSO in roomNodeGraph.roomNodeList)
            {
                if (roomNodeSO == null)
                    continue;

                if(roomNodeSO.roomNodeType.isEntrance || roomNodeSO.roomNodeType.isCorridorEW || roomNodeSO.roomNodeType.isCorridorNS ||
                    roomNodeSO.roomNodeType.isCorridor || roomNodeSO.roomNodeType.isNone)
                {
                    continue;
                }

                bool isRoomNodeTypeFound = false;
                foreach(RoomTemplateSO roomTemplateSO in roomTemplateList)
                {
                    if (roomTemplateSO == null)
                        continue;
                    if(roomTemplateSO.roomNodeType == roomNodeSO.roomNodeType)
                    {
                        isRoomNodeTypeFound = true;
                        break;
                    }
                }
                if(isRoomNodeTypeFound == false)
                {
                    Debug.Log("In " + this.name.ToString() + " : No Room Template Type " + roomNodeSO.roomNodeType.name.ToString() + " found for Node Graph " +
                        roomNodeGraph.name.ToString());
                }
            }
        }
    }

#endif
    #endregion
}
