using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemySpawner : SingletonMonobehaviour<EnemySpawner>
{
    private int enemiesToSpawn;
    private int currentEnemyCount;
    private int enemiesSpawnedSoFar;
    private int enemyMaxConcurrentSpawnNumber;
    private LevelsSO currentLevel; //Room == Level
    private Room currentRoom;
    private RoomEnemySpawnParameters roomEnemySpawnParameters;

    private void Start()
    {
        EnemySpawns(GameManager.Instance.GetCurrentLevel());
    }

    private void EnemySpawns(LevelsSO level)
    {
        enemiesSpawnedSoFar = 0;
        currentEnemyCount = 0;
        currentLevel = level;
        currentRoom = GameManager.Instance.GetCurrentRoom();

        

        if (currentLevel.isClearedOfEnemies) return;

        enemiesToSpawn = currentLevel.GetNumberOfEnemiesToSpawn();
        roomEnemySpawnParameters = currentLevel.GetRoomEnemySpawnParameters();

        if(enemiesToSpawn == 0)
        {
            currentLevel.isClearedOfEnemies = true;
            return;
        }

        enemyMaxConcurrentSpawnNumber = GetConcurrentEnemies();


        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if(GameManager.Instance.gameState == GameState.bossStage)
        {
            GameManager.Instance.previousGameState = GameState.bossStage;
            GameManager.Instance.gameState = GameState.engagingBoss;
        }

        if(GameManager.Instance.gameState == GameState.playingLevel)
        {
            GameManager.Instance.previousGameState = GameState.playingLevel;
            GameManager.Instance.gameState = GameState.engagingEnemies;
        }

        StartCoroutine(SpawnEnemiesRoutine());

    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        Grid grid = currentRoom.instantiatedRoom.grid;

        RandomSpawnableObject<EnemyDetailsSO> randomEnemyHelperClass = new RandomSpawnableObject<EnemyDetailsSO>(currentLevel.enemiesByLevel);

        if(currentRoom.spawnPositionArray.Length > 0)
        {
            for(int i = 0; i < enemiesToSpawn; i++)
            {
                while(currentEnemyCount >= enemyMaxConcurrentSpawnNumber)
                {
                    yield return null;
                }

                Vector3Int cellPosition = (Vector3Int)currentRoom.spawnPositionArray[Random.Range(0, currentRoom.spawnPositionArray.Length)];
                CreateEnemy(randomEnemyHelperClass.GetItem(), grid.CellToWorld(cellPosition));

                yield return new WaitForSeconds(GetEnemySpawnInterval());
            }
        }
    }

    private float GetEnemySpawnInterval()
    {
        return (Random.Range(roomEnemySpawnParameters.minSpawnInterval, roomEnemySpawnParameters.maxSpawnInterval));
    }

    private int GetConcurrentEnemies()
    {
        return (Random.Range(roomEnemySpawnParameters.minConcurrentEnemies, roomEnemySpawnParameters.maxConcurrentEnemies));
    }

    private void CreateEnemy(EnemyDetailsSO enemyDetails, Vector3 position)
    {
        enemiesSpawnedSoFar++;
        currentEnemyCount++;

        LevelsSO dungeonLevel = GameManager.Instance.GetCurrentDungeonLevel();
        GameObject enemy = Instantiate(enemyDetails.enemyPrefab, position, Quaternion.identity, transform);
        enemy.GetComponent<Enemy>().EnemyInitialization(enemyDetails, enemiesSpawnedSoFar, dungeonLevel);

        enemy.GetComponent<DestroyedEvent>().OnDestroyed += Enemy_OnDestroyed;
    }

    private void Enemy_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {
        destroyedEvent.OnDestroyed -= Enemy_OnDestroyed;
        currentEnemyCount--;

        StaticEventHandler.CallPointsScoredEvent(destroyedEventArgs.points);

        if(currentEnemyCount <= 0 && enemiesSpawnedSoFar == enemiesToSpawn)
        {
            currentLevel.isClearedOfEnemies = true;
            if(GameManager.Instance.gameState == GameState.engagingEnemies)
            {
                GameManager.Instance.gameState = GameState.playingLevel;
                GameManager.Instance.previousGameState = GameState.engagingEnemies;
            }
            else if(GameManager.Instance.gameState == GameState.engagingBoss)
            {
                GameManager.Instance.gameState = GameState.bossStage;
                GameManager.Instance.previousGameState = GameState.engagingBoss;
            }
            StaticEventHandler.CallLevelWon(1);
        }
    }
}
