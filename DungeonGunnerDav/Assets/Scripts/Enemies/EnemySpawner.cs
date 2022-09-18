using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemySpawner : SingletonMonobehaviour<EnemySpawner>
{
    private int timeToSurvives;
    private int enemiesToSpawn;
    private int currentEnemyCount;
    private int enemiesSpawnedSoFar;
    private int enemyMaxConcurrentSpawnNumber;
    private LevelsSO currentLevel; //Room == Level
    private Room currentRoom;
    private RoomEnemySpawnParameters roomEnemySpawnParameters;
    public int totalWavesDefeated = 0;


    private void Start()
    {
        totalWavesDefeated = 0;
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
        timeToSurvives = currentLevel.GetTimeToSurvivesWavesInSeconds();

        MusicManager.Instance.PlayMusic(currentLevel.backgroundMusic, 0.2f, 2f);

        if(enemiesToSpawn == 0)
        {
            currentLevel.isClearedOfEnemies = true;
            return;
        }

        enemyMaxConcurrentSpawnNumber = roomEnemySpawnParameters.maxConcurrentEnemies;


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
        StartCoroutine(WavesTimeStartCountingRoutine());

    }

    private IEnumerator WavesTimeStartCountingRoutine()
    {
        while(timeToSurvives > 0)
        {
            timeToSurvives--;
            Debug.Log(timeToSurvives);
            yield return new WaitForSeconds(1f);
        }

        while(currentEnemyCount > 0)
        {
            yield return null;
        }

        WavesCleared();
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        Grid grid = currentRoom.instantiatedRoom.grid;

        RandomSpawnableObject<EnemyDetailsSO> randomEnemyHelperClass = new RandomSpawnableObject<EnemyDetailsSO>(currentLevel.GetWavesEnemySpawnTypes());

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
                if (timeToSurvives <= 10)
                {
                    break;
                }
            }
        }
    }

    private float GetEnemySpawnInterval()
    {
        return (Random.Range(roomEnemySpawnParameters.minSpawnInterval, roomEnemySpawnParameters.maxSpawnInterval));
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

        if (currentEnemyCount <= 0 && timeToSurvives <= 0)
        {
            //WavesCleared();

            //Debug.Log("Lanjut waves");
            ////currentLevel.isClearedOfEnemies = true;
            //if (GameManager.Instance.gameState == GameState.engagingEnemies)
            //{
            //    GameManager.Instance.gameState = GameState.playingLevel;
            //    GameManager.Instance.previousGameState = GameState.engagingEnemies;
            //}
            //else if (GameManager.Instance.gameState == GameState.engagingBoss)
            //{
            //    GameManager.Instance.gameState = GameState.bossStage;
            //    GameManager.Instance.previousGameState = GameState.engagingBoss;
            //}

            //totalWavesDefeated++;
            //if(totalWavesDefeated < currentLevel.totalWaves)
            //{
            //    GameManager.Instance.currentLevelWavesIndex++;
            //    EnemySpawns(GameManager.Instance.GetCurrentLevel());
            //}
            //else
            //{
            //    StaticEventHandler.CallLevelWon(1);
            //}
        }
    }

    private void WavesCleared()
    {
        Debug.Log("Lanjut waves");
        //currentLevel.isClearedOfEnemies = true;
        if (GameManager.Instance.gameState == GameState.engagingEnemies)
        {
            GameManager.Instance.gameState = GameState.playingLevel;
            GameManager.Instance.previousGameState = GameState.engagingEnemies;
        }
        else if (GameManager.Instance.gameState == GameState.engagingBoss)
        {
            GameManager.Instance.gameState = GameState.bossStage;
            GameManager.Instance.previousGameState = GameState.engagingBoss;
        }

        totalWavesDefeated++;
        if (totalWavesDefeated < currentLevel.totalWaves)
        {
            GameManager.Instance.currentLevelWavesIndex++;
            EnemySpawns(GameManager.Instance.GetCurrentLevel());
        }
        else
        {
            StaticEventHandler.CallLevelWon(1);
        }
    }
}
