using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapManager : SingletonMonobehaviour<MapManager>
{
    public int unlockLevelListIndex;
    public int totalCoinsInGame;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI coinsTextWeapon;
    public TextMeshProUGUI coinsTextPlayer;

    public List<GameObject> levelButtonsLocked;
    public List<long> highScoreList;
    public List<TextMeshProUGUI> highScoreTextList;

    protected override void Awake()
    {
        base.Awake();

        Load();
        LoadHighScore();
        LoadPlayer();

        Time.timeScale = 1f;
    }


    private void Start()
    {
        coinsText.text = totalCoinsInGame.ToString();
        for(int i = 0; i <= unlockLevelListIndex; i++)
        {
            levelButtonsLocked[i].SetActive(false);
            highScoreTextList[i].text = highScoreList[i].ToString("###,###0");
        }
        
    }


    public void ItemBuyed()
    {
        coinsText.text = totalCoinsInGame.ToString();
        coinsTextPlayer.text = totalCoinsInGame.ToString();
        coinsTextWeapon.text = totalCoinsInGame.ToString();
    }

    public void Save()
    {
        SaveObject saveObject = new SaveObject() { levelUnlock = unlockLevelListIndex, coinsEarned = totalCoinsInGame };
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);
    }

    public void Load()
    {
        string saveString = SaveSystem.Load();
        if (saveString != null)
        {
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
            unlockLevelListIndex = saveObject.levelUnlock;
            totalCoinsInGame = saveObject.coinsEarned;
        }
        else
        {
            Debug.Log("No Save");
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
    }

    private void LoadPlayer()
    {
        string saveString = SaveSystem.LoadPlayer();
        if (saveString != null)
        {
            SaveObjectPlayer saveObjectPlayer = JsonUtility.FromJson<SaveObjectPlayer>(saveString);
            GameResources.Instance.healthBonus = saveObjectPlayer.healthUpgrade;
            GameResources.Instance.attackBonus = saveObjectPlayer.attackUpgrade;
            GameResources.Instance.speedBonus = saveObjectPlayer.speedUpgrade;
            GameResources.Instance.coinsBonus = saveObjectPlayer.coinsUpgrade;
        }
        else
        {
            Debug.Log("No Save");
        }
    }

    public void PlayButtonClick()
    {
        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.buttonClick);
    }

    public void PlayGridClick()
    {
        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.gridClick);
    }

    public void PlayBuyClick()
    {
        SoundEffectManager.Instance.PlaySoundEffect(GameResources.Instance.buyClick);
    }
}
