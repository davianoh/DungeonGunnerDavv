using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomEnemySpawnParameters
{
    public LevelsSO dungeonLevel;
    public int minTotalEnemiesToSpawn;
    public int maxTotalEnemiesToSpawn;
    public int minConcurrentEnemies;
    public int maxConcurrentEnemies;
    public int minSpawnInterval;
    public int maxSpawnInterval;
}
