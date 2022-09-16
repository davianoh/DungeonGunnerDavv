using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leevl_", menuName = "Scriptable Objects/Gemastik/Level")]
public class LevelsSO : ScriptableObject
{
    public string id;
    public string levelName;
    public Room room;

    public List<SpawnableObjectByLevel<EnemyDetailsSO>> enemiesByLevelList;
    public List<RoomEnemySpawnParameters> roomLevelEnemySpawnParametersList;

    public bool isClearedOfEnemies = false;


    public int GetNumberOfEnemiesToSpawn(LevelsSO dungeonLevel)
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

    public RoomEnemySpawnParameters GetRoomEnemySpawnParameters(LevelsSO dungeonLevel)
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
