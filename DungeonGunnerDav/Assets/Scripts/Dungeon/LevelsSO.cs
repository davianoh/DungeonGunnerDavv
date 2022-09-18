using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leevl_", menuName = "Scriptable Objects/Gemastik/Level")]
public class LevelsSO : ScriptableObject
{
    public string id;
    public string levelName;

    public SpawnableObjectByLevel<EnemyDetailsSO> enemiesByLevel;
    public List<RoomEnemySpawnParameters> wavesEnemySpawnParameters;

    public bool isClearedOfEnemies = false;


    public int GetNumberOfEnemiesToSpawn()
    {
        return wavesEnemySpawnParameters[GameManager.Instance.currentLevelWavesIndex].maxTotalEnemiesToSpawn;
    }

    public RoomEnemySpawnParameters GetRoomEnemySpawnParameters()
    {
        return wavesEnemySpawnParameters[GameManager.Instance.currentLevelWavesIndex];
    }

    public int GetTimeToSurvivesWavesInSeconds()
    {
        return wavesEnemySpawnParameters[GameManager.Instance.currentLevelWavesIndex].wavesSurviveTimeInSeconds;
    }

}
