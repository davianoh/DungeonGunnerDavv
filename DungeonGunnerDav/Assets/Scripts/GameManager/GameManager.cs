using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    #region Header GameObject References
    [Space(10)]
    [Header("GAMEOBJECT REFERENCES")]
    #endregion
    [SerializeField] private TextMeshProUGUI messageTextTMP;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup canvasGroupLevelWon;
    [SerializeField] private GameObject pauseMenu;

    #region Header Dungeon Level
    [Space(10)]
    [Header("DUNGEON LEVEL")]
    #endregion
    #region ToolTip
    [Tooltip("Populate w/ the dungeon level scriptable objects")]
    #endregion
    [SerializeField] private Transform mainMapParent;
    [SerializeField] private List<LevelsSO> levelList;


    #region Header Save Object Parameters
    [Space(10)]
    [Header("SAVE OBJECT PARAMETERS")]
    #endregion
    #region Tooltip
    [Tooltip("Populate w/ the starting dungeon level for testing, first level = 0")]
    #endregion
    public int currentLevelUnlockedIndex = 0;
    public int currentLevelIndex = 0;
    public int currentLevelWavesIndex = 0;
    public int currentTotalCoinsInGame;
    public List<int> weaponsOwnedList = new List<int>();
    public List<int> weaponEquipedList = new List<int>();
    public int unlockWeaponSlots;

    public List<long> highScoreList = new List<long>();

    #region Header Parameters Other
    [Space(10)]
    [Header("PARAMETERS OTHER")]
    #endregion
    private LevelsSO previousRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;

    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState previousGameState;
    private long gameScore;
    private int scoreMultiplier;
    private bool middleCutScene = false;

    // For Now.. !!!
    public Room currentRoom;

    // ROOM == SO
    protected override void Awake()
    {
        // inherated class from's awake method call
        base.Awake();

        //SaveSystem.Init();
        Load();
        LoadWeapons();
        LoadHighScore();

        playerDetails = GameResources.Instance.currentPlayer.playerDetails;
        currentRoom = GameResources.Instance.roomList[GameResources.Instance.selectedLevelIndex];
        InstantiatePlayer();
        InstantiateLevel(currentRoom); 
    }


    private void InstantiatePlayer()
    {
        GameObject playerGameobject = Instantiate(playerDetails.playerPrefabs);
        player = playerGameobject.GetComponent<Player>();
        player.Initialize(playerDetails);
    }

    private void InstantiateLevel(Room room)
    {
        var generatedRoom = Instantiate(room.prefabs, mainMapParent);
        room.level = levelList[GameResources.Instance.selectedLevelIndex];
        currentRoom = generatedRoom.GetComponent<Room>(); //!!!
    }

    private void OnEnable()
    {
        StaticEventHandler.OnPointsScored += StaticEventHandler_OnPointsScored;
        StaticEventHandler.OnMultiplier += StaticEventHandler_OnMultiplier;
        StaticEventHandler.OnLevelWon += StaticEventHandler_OnLevelWon;
        player.destroyedEvent.OnDestroyed += Player_OnDestroyed;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnPointsScored -= StaticEventHandler_OnPointsScored;
        StaticEventHandler.OnMultiplier -= StaticEventHandler_OnMultiplier;
        StaticEventHandler.OnLevelWon -= StaticEventHandler_OnLevelWon;
        player.destroyedEvent.OnDestroyed -= Player_OnDestroyed;
    }


    private void Player_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyedEventArgs)
    {
        previousGameState = gameState;
        gameState = GameState.gameLost;
    }

    private void StaticEventHandler_OnPointsScored(PointsScoredArgs pointsScoredArgs)
    {
        gameScore += pointsScoredArgs.points * scoreMultiplier;
        StaticEventHandler.CallScoreChangedEvent(gameScore, scoreMultiplier);
    }

    private void StaticEventHandler_OnMultiplier(MultiplierArgs multiplierArgs)
    {
        if (multiplierArgs.multiplier)
        {
            scoreMultiplier++;
        }
        else
        {
            scoreMultiplier--;
        }

        scoreMultiplier = Mathf.Clamp(scoreMultiplier, 1, 30);

        StaticEventHandler.CallScoreChangedEvent(gameScore, scoreMultiplier);
    }

    private void StaticEventHandler_OnLevelWon(LevelWonArgs levelWonArgs)
    {
        int totalCoins = levelWonArgs.coins;
        // Save coins, level won, skors;

        previousGameState = gameState;
        gameState = GameState.levelCompleted;
    }


    // Start is called before the first frame update
    void Start()
    {

        previousGameState = GameState.gameStarted;
        gameState = GameState.gameStarted;

        gameScore = 0;
        scoreMultiplier = 1;

        StartCoroutine(Fade(0f, 1f, 0f, Color.black));
        Debug.Log(GameResources.Instance.healthBonus + " and " + GameResources.Instance.attackBonus);

        if(GameResources.Instance.selectedLevelIndex >= 5)
        {
            MusicManager.Instance.PlayMusic(GameResources.Instance.map2Music, 0.2f, 2f);
        }
        else
        {
            MusicManager.Instance.PlayMusic(GameResources.Instance.map1Music, 0.2f, 2f);
        }
        
    }

    private IEnumerator Fade(float startFadeAlpha, float targetFadeAlpha, float fadeSeconds, Color backgroundColor)
    {
        Image image = canvasGroup.GetComponent<Image>();
        image.color = backgroundColor;
        float time = 0;

        while(time <= fadeSeconds)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startFadeAlpha, targetFadeAlpha, time / fadeSeconds);
            yield return null;
        }
    }

    private IEnumerator FadeLevelWon(float startFadeAlpha, float targetFadeAlpha, float fadeSeconds)
    {
        float time = 0;

        while (time <= fadeSeconds)
        {
            time += Time.deltaTime;
            canvasGroupLevelWon.alpha = Mathf.Lerp(startFadeAlpha, targetFadeAlpha, time / fadeSeconds);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleGameState();

        // For testing
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    gameState = GameState.gameStarted;
        //}
    }

    private void HandleGameState()
    {
        switch (gameState)
        {
            case GameState.gameStarted:
                PlayDungeonLevel(GameResources.Instance.selectedLevelIndex);
                gameState = GameState.playingLevel;
                break;

            case GameState.playingLevel:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }
                break;

            case GameState.levelCompleted:
                if(previousGameState != GameState.levelCompleted)
                {
                    StartCoroutine(LevelWon());
                }
                break;

            case GameState.gameLost:
                if(previousGameState != GameState.gameLost)
                {
                    StopAllCoroutines();
                    StartCoroutine(GameLost());
                }
                break;

            case GameState.restartGame:
                RestartGame();
                break;

            case GameState.gamePaused:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PauseGameMenu();
                }
                break;
        }
    }


    private IEnumerator LevelWon()
    {
        previousGameState = GameState.levelCompleted;

        if(currentLevelUnlockedIndex == GameResources.Instance.selectedLevelIndex)
        {
            if(currentLevelUnlockedIndex == 4)
            {
                middleCutScene = true;
            }
            currentLevelUnlockedIndex++;
        }

        GetPlayer().playerControl.DisablePlayer();
        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));
        yield return StartCoroutine(DisplayMessageRoutine("KERJA BAGUS PLAYER! \n\n KAMU TELAH BERHASIL MELINDUNGI ARTEFAK BUDAYA INI", Color.white, 3f));
        yield return StartCoroutine(DisplayMessageRoutine("SKORMU ADALAH : " + gameScore.ToString("###,###0"), Color.white, 3f));
        yield return StartCoroutine(DisplayMessageRoutine("AMBIL SEMUA KOIN LALU BILA SUDAH SIAP, TEKAN SPACEBAR UNTUK LANJUT", Color.white, 3f));

        GetPlayer().playerControl.EnablePlayer();
        yield return StartCoroutine(Fade(1f, 0f, 2f, Color.black));
        yield return StartCoroutine(FadeLevelWon(0f, 1f, 1f));
        yield return StartCoroutine(DisplayMessageRoutine("TEKAN SPACEBAR", Color.black, 0f));
        
        
        
        

        gameState = GameState.restartGame;
    }


    private IEnumerator GameLost()
    {
        previousGameState = GameState.gameLost;
        currentTotalCoinsInGame += CoinsManager.Instance.coinsInLevel;
        Save();
        SaveWeapons();
        SaveHighScore(gameScore, GameResources.Instance.selectedLevelIndex);

        GetPlayer().playerControl.DisablePlayer();
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));
        Enemy[] enemyArray = GameObject.FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in enemyArray)
        {
            enemy.gameObject.SetActive(false);
        }

        yield return StartCoroutine(DisplayMessageRoutine("KAMU GAGAL PLAYER! \n\n ARTEFAK BUDAYA TELAH RUSAK", Color.white, 2f));
        yield return StartCoroutine(DisplayMessageRoutine("SKORMU ADALAH : " + gameScore.ToString("###,###0"), Color.white, 4f));
        yield return StartCoroutine(DisplayMessageRoutine("TEKAN SPACEBAR UNTUK KEMBALI", Color.white, 0f));

        gameState = GameState.restartGame;
    }


    private void RestartGame()
    {
        SceneManager.LoadScene("MainMapScene");
    }

    

    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {

        //player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0f);

        //player.gameObject.transform.position = HelperUtilities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);
        player.gameObject.transform.position = HelperUtilities.GetSpawnWorldPositionPlayer(currentRoom.spawnPositionPlayer);

        StartCoroutine(DisplayDungeonLevelText());

        // Demo code
        //RoomEnemiesDefeated();
    }

    public void PauseGameMenu()
    {
        if(gameState != GameState.gamePaused)
        {
            pauseMenu.SetActive(true);
            GetPlayer().playerControl.DisablePlayer();

            previousGameState = gameState;
            gameState = GameState.gamePaused;
        }
        else if(gameState == GameState.gamePaused)
        {
            pauseMenu.GetComponent<PauseMenuUI>().ExitPauseMenu();
            GetPlayer().playerControl.EnablePlayer();

            gameState = previousGameState;
            previousGameState = GameState.gamePaused;
        }
    }

    private IEnumerator DisplayDungeonLevelText()
    {
        StartCoroutine(Fade(0f, 1f, 0f, Color.black));
        GetPlayer().playerControl.DisablePlayer();

        string messageText = "LEVEL " + (GameResources.Instance.selectedLevelIndex + 1).ToString() + "\n\n" + levelList[GameResources.Instance.selectedLevelIndex].levelName.ToUpper();
        yield return StartCoroutine(DisplayMessageRoutine(messageText, Color.white, 2f));

        GetPlayer().playerControl.EnablePlayer();
        yield return StartCoroutine(Fade(1f, 0f, 2f, Color.black));
    }

    private IEnumerator DisplayMessageRoutine(string text, Color textColor, float displaySeconds)
    {
        messageTextTMP.SetText(text);
        messageTextTMP.color = textColor;

        if(displaySeconds > 0f)
        {
            float timer = displaySeconds;
            while (timer > 0f && !Input.GetKeyDown(KeyCode.Space))
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                yield return null;
            }

            currentTotalCoinsInGame += CoinsManager.Instance.coinsInLevel;
            Save();
            SaveWeapons();
            SaveHighScore(gameScore, GameResources.Instance.selectedLevelIndex);

            if (middleCutScene)
            {
                middleCutScene = false;
                SceneManager.LoadScene("CutSceneMiddle");
            }
            else
            {
                SceneManager.LoadScene("MainMapScene");
            }
            
        }

        yield return null;
        messageTextTMP.SetText("");
    }

    public Player GetPlayer()
    {
        return player;
    }

    public Sprite GetPlayerMinimapIcon()
    {
        return playerDetails.playerMinimapIcon;
    }

    public LevelsSO GetCurrentLevel()
    {
        return levelList[GameResources.Instance.selectedLevelIndex];
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    public void Save()
    {
        SaveObject saveObject = new SaveObject() { levelUnlock = currentLevelUnlockedIndex, coinsEarned = currentTotalCoinsInGame };
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);
    }

    public void Load()
    {
        string saveString = SaveSystem.Load();
        if (saveString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
            currentLevelUnlockedIndex = saveObject.levelUnlock;
            currentTotalCoinsInGame = saveObject.coinsEarned;
        }
        else
        {
            Debug.Log("No Save");
        }
    }

    public void SaveWeapons()
    {
        SaveObjectWeapons saveObjectWeapons = new SaveObjectWeapons() { weaponOwnedList = weaponsOwnedList, weaponEquipList = weaponEquipedList, unlockSlots = unlockWeaponSlots };
        string json = JsonUtility.ToJson(saveObjectWeapons);
        SaveSystem.SaveWeapons(json);
    }

    public void LoadWeapons()
    {
        string saveString = SaveSystem.LoadWeapons();
        if (saveString != null)
        {
            SaveObjectWeapons saveObjectWeapons = JsonUtility.FromJson<SaveObjectWeapons>(saveString);
            weaponsOwnedList = saveObjectWeapons.weaponOwnedList;
            weaponEquipedList = saveObjectWeapons.weaponEquipList;
            unlockWeaponSlots = saveObjectWeapons.unlockSlots;
        }
        else
        {
            Debug.Log("No Save");
        }
    }

    private void SaveHighScore(long highScoress, int levelIndex)
    {
        int index = 0;
        foreach(long score in highScoreList)
        {
            if(index == levelIndex)
            {
                if(highScoress > highScoreList[index])
                {
                    highScoreList[index] = highScoress;
                    SaveObjectHighScores saveObjectHighScore = new SaveObjectHighScores() { highScoreList = highScoreList };
                    string json = JsonUtility.ToJson(saveObjectHighScore);
                    SaveSystem.SaveHighScore(json);
                    Debug.Log(index);
                    return;
                }
                else
                {
                    return;
                }
            }
            index++;
        }
        
    }

    private void LoadHighScore()
    {
        string saveString = SaveSystem.LoadHighScore();
        if(saveString != null)
        {
            SaveObjectHighScores saveObjectHighScore = JsonUtility.FromJson<SaveObjectHighScores>(saveString);
            highScoreList = saveObjectHighScore.highScoreList;
        }
        else
        {
            Debug.Log("No Save");
        }
    }


    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(levelList), levelList);
        HelperUtilities.ValidateCheckNullValue(this, nameof(messageTextTMP), messageTextTMP);
        HelperUtilities.ValidateCheckNullValue(this, nameof(canvasGroup), canvasGroup);
    }
#endif
    #endregion
}
