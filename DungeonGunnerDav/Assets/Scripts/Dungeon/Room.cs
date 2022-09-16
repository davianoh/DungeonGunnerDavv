using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leevl_", menuName = "Scriptable Objects/Gemastik/Level")]
public class Room : ScriptableObject
{
    public string id;
    public string levelName;
    public GameObject prefab;
    public RoomNodeTypeSO roomNodeType;
    public Vector2Int lowerBounds;
    public Vector2Int upperBounds;
    public Vector2Int templateLowerBounds;
    public Vector2Int templateUpperBounds;
    public Vector2Int[] spawnPositionArray;

    public List<SpawnableObjectByLevel<EnemyDetailsSO>> enemiesByLevelList;
    public List<RoomEnemySpawnParameters> roomLevelEnemySpawnParametersList;
    public InstantiatedRoom instantiatedRoom; //!
    public bool isLit = false;
    public bool isClearedOfEnemies = false;
    public bool isPreviouslyVisited = false;

    public int GetNumberOfEnemiesToSpawn(Room dungeonLevel)
    {
        foreach(RoomEnemySpawnParameters roomEnemySpawnParameters in roomLevelEnemySpawnParametersList)
        {
            if(roomEnemySpawnParameters.dungeonLevel == dungeonLevel)
            {
                return Random.Range(roomEnemySpawnParameters.minTotalEnemiesToSpawn, roomEnemySpawnParameters.maxTotalEnemiesToSpawn);
            }
        }
        return 0;
    }

    public RoomEnemySpawnParameters GetRoomEnemySpawnParameters(Room dungeonLevel)
    {
        foreach(RoomEnemySpawnParameters roomEnemySpawnParameters in roomLevelEnemySpawnParametersList)
        {
            if(roomEnemySpawnParameters.dungeonLevel == dungeonLevel)
            {
                return roomEnemySpawnParameters;
            }
        }
        return null;
    }

}
