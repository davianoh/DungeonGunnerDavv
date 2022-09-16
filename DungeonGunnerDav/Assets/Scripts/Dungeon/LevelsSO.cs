using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leevl_", menuName = "Scriptable Objects/Gemastik/Level")]
public class LevelsSO : ScriptableObject
{
    public string id;
    public string levelName;

    public SpawnableObjectByLevel<EnemyDetailsSO> enemiesByLevel;
    public RoomEnemySpawnParameters roomLevelEnemySpawnParameters;

    public bool isClearedOfEnemies = false;


    public int GetNumberOfEnemiesToSpawn()
    {
        return Random.Range(roomLevelEnemySpawnParameters.minTotalEnemiesToSpawn, roomLevelEnemySpawnParameters.maxTotalEnemiesToSpawn);
    }

    public RoomEnemySpawnParameters GetRoomEnemySpawnParameters()
    {
        return roomLevelEnemySpawnParameters;
    }

}
