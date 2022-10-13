using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomEnemySpawnParameters
{
    public int maxTotalEnemiesToSpawn;
    public int maxConcurrentEnemies;
    public float minSpawnInterval;
    public float maxSpawnInterval;
    public int wavesSurviveTimeInSeconds;
}
