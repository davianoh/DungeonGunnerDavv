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

    #region Header Dungeon Level
    [Space(10)]
    [Header("DUNGEON LEVEL")]
    #endregion
    #region ToolTip
    [Tooltip("Populate w/ the dungeon level scriptable objects")]
    #endregion
    [SerializeField] private List<Room> levelList;

    #region Tooltip
    [Tooltip("Populate w/ the starting dungeon level for testing, first level = 0")]
    #endregion
    [SerializeField] private int currentDungeonLevelListIndex = 0;

    [SerializeField] private Room currentLevel;

    private Room previousRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;

    [HideInInspector] public GameState gameState;
    [HideInInspector] public GameState previousGameState;
    private long gameScore;
    private int scoreMultiplier;

    public InstantiatedRoom currentRoom;


    protected override void Awake()
    {
        // inherated class from's awake method call
        base.Awake();

        playerDetails = GameResources.Instance.currentPlayer.playerDetails;
        InstantiatePlayer();
        currentLevel.instantiatedRoom = currentRoom;
    }

    private void InstantiatePlayer()
    {
        GameObject playerGameobject = Instantiate(playerDetails.playerPrefabs);
        player = playerGameobject.GetComponent<Player>();
        player.Initialize(playerDetails);
    }

    private void OnEnable()
    {
        StaticEventHandler.OnPointsScored += StaticEventHandler_OnPointsScored;
        StaticEventHandler.OnMultiplier += StaticEventHandler_OnMultiplier;
        player.destroyedEvent.OnDestroyed += Player_OnDestroyed;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnPointsScored -= StaticEventHandler_OnPointsScored;
        StaticEventHandler.OnMultiplier -= StaticEventHandler_OnMultiplier;
        player.destroyedEvent.OnDestroyed -= Player_OnDestroyed;
    }



    //private IEnumerator BossStage()
    //{
    //    bossRoom.gameObject.SetActive(true);
    //    bossRoom.UnlockDoors(0f);

    //    yield return new WaitForSeconds(2f);
    //    yield return StartCoroutine(Fade(0f, 1f, 2f, new Color(0f, 0f, 0f, 0.4f)));
    //    yield return StartCoroutine(DisplayMessageRoutine("WELL DONE " + GameResources.Instance.currentPlayer.playerName + "! YOU'VE SURVIVED ...SO FAR\n\n NOW FIND AND DEFEAT THE BOSS ...GOOD LUCK", Color.white, 5f));
    //    yield return StartCoroutine(Fade(1f, 0f, 2f, new Color(0f, 0f, 0f, 0.4f)));


    //}

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


    // Start is called before the first frame update
    void Start()
    {
        previousGameState = GameState.gameStarted;
        gameState = GameState.gameStarted;

        gameScore = 0;
        scoreMultiplier = 1;

        StartCoroutine(Fade(0f, 1f, 0f, Color.black));
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
                PlayDungeonLevel(currentDungeonLevelListIndex);
                gameState = GameState.playingLevel;
                break;

            case GameState.gameWon:
                if(previousGameState != GameState.gameWon)
                {
                    StartCoroutine(GameWon());
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
        }
    }


    private IEnumerator GameWon()
    {
        previousGameState = GameState.gameWon;

        GetPlayer().playerControl.DisablePlayer();
        yield return StartCoroutine(Fade(0f, 1f, 2f, Color.black));
        yield return StartCoroutine(DisplayMessageRoutine("WELL DONE " + GameResources.Instance.currentPlayer.playerName + "! \n\n YOU HAVE DEFEATED THE ENTIRE DUNGEON", Color.white, 3f));
        yield return StartCoroutine(DisplayMessageRoutine("YOU SCORED : " + gameScore.ToString("###,###0"), Color.white, 4f));
        yield return StartCoroutine(DisplayMessageRoutine("PRESS RETURN TO RESTART THE GAME", Color.white, 0f));

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
        player.gameObject.transform.position = new Vector3((currentLevel.lowerBounds.x + currentLevel.upperBounds.x) / 2f, (currentLevel.lowerBounds.y + currentLevel.upperBounds.y) / 2f, 0f);

        player.gameObject.transform.position = HelperUtilities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);

        StartCoroutine(DisplayDungeonLevelText());

        // Demo code
        //RoomEnemiesDefeated();
    }

    private IEnumerator DisplayDungeonLevelText()
    {
        StartCoroutine(Fade(0f, 1f, 0f, Color.black));
        GetPlayer().playerControl.DisablePlayer();

        string messageText = "LEVEL " + (currentDungeonLevelListIndex + 1).ToString() + "\n\n" + levelList[currentDungeonLevelListIndex].levelName.ToUpper();
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

    public Room GetCurrentLevel()
    {
        return currentLevel;
    }

    public Room GetCurrentDungeonLevel()
    {
        return levelList[currentDungeonLevelListIndex];
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
