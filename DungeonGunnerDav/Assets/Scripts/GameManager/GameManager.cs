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
    public int currentLevelListIndex = 0;
    public int currentLevelWavesIndex = 0;
    public int currentTotalCoinsInGame;
    public List<int> weaponsOwnedList = new List<int>();
    public List<int> weaponEquipedList = new List<int>();
    public int unlockWeaponSlots;

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

    // For Now.. !!!
    public Room currentRoom;

    // ROOM == SO
    protected override void Awake()
    {
        // inherated class from's awake method call
        base.Awake();

        SaveSystem.Init();
        Load();
        LoadWeapons();

        playerDetails = GameResources.Instance.currentPlayer.playerDetails;
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
        room.level = levelList[currentLevelListIndex];
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
                PlayDungeonLevel(currentLevelListIndex);
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

        currentLevelListIndex++;
        currentTotalCoinsInGame += CoinsManager.Instance.coinsInLevel;
        Save();
        SaveWeapons();
        GetPlayer().playerControl.DisablePlayer();
        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));
        yield return StartCoroutine(DisplayMessageRoutine("WELL DONE " + GameResources.Instance.currentPlayer.playerName + "! \n\n YOU HAVE SURVIVE THE NIGHT", Color.white, 3f));
        yield return StartCoroutine(DisplayMessageRoutine("YOU SCORED : " + gameScore.ToString("###,###0"), Color.white, 4f));
        yield return StartCoroutine(DisplayMessageRoutine("PRESS RETURN TO CONTINUE", Color.white, 0f));

        gameState = GameState.restartGame;
    }

    private IEnumerator GameLost()
    {
        previousGameState = GameState.gameLost;
        GetPlayer().playerControl.DisablePlayer();
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));
        Enemy[] enemyArray = GameObject.FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in enemyArray)
        {
            enemy.gameObject.SetActive(false);
        }

        yield return StartCoroutine(DisplayMessageRoutine("BAD LUCKz " + GameResources.Instance.currentPlayer.playerName + "! \n\n YOU HAVE FAIL TO THIS DUNGEON", Color.white, 2f));
        yield return StartCoroutine(DisplayMessageRoutine("YOU SCORED : " + gameScore.ToString("###,###0"), Color.white, 4f));
        yield return StartCoroutine(DisplayMessageRoutine("PRESS RETURN TO RESTART THE GAME", Color.white, 0f));

        gameState = GameState.restartGame;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    

    private void PlayDungeonLevel(int dungeonLevelListIndex)
    {
        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0f);

        player.gameObject.transform.position = HelperUtilities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);

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
            pauseMenu.SetActive(false);
            GetPlayer().playerControl.EnablePlayer();

            gameState = previousGameState;
            previousGameState = GameState.gamePaused;
        }
    }

    private IEnumerator DisplayDungeonLevelText()
    {
        StartCoroutine(Fade(0f, 1f, 0f, Color.black));
        GetPlayer().playerControl.DisablePlayer();

        string messageText = "LEVEL " + (currentLevelListIndex + 1).ToString() + "\n\n" + levelList[currentLevelListIndex].levelName.ToUpper();
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
            while (timer > 0f && !Input.GetKeyDown(KeyCode.Return))
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }
            SceneManager.LoadScene("MainMapScene");
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
        return levelList[currentLevelListIndex];
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    public void Save()
    {
        SaveObject saveObject = new SaveObject() { levelUnlock = currentLevelListIndex, coinsEarned = currentTotalCoinsInGame };
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);
    }

    public void Load()
    {
        string saveString = SaveSystem.Load();
        if (saveString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
            currentLevelListIndex = saveObject.levelUnlock;
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
