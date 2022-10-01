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
    public bool itemBuyed = false;

    protected override void Awake()
    {
        base.Awake();

        Load();
    }


    private void Start()
    {
        coinsText.text = totalCoinsInGame.ToString();
    }

    private void Update()
    {
        if (itemBuyed)
        {
            coinsText.text = totalCoinsInGame.ToString();
            coinsTextPlayer.text = totalCoinsInGame.ToString();
            coinsTextWeapon.text = totalCoinsInGame.ToString();
            itemBuyed = false;
        }
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
}
