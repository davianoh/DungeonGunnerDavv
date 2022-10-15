using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterMenuUI : SingletonMonobehaviour<CharacterMenuUI>
{
    #region Header SaveFiles References
    [Header("SAFE FILES REFERENCES")]
    [Space(10)]
    #endregion
    public int selectedPlayerIndex;
    public int selectedPlayerIndexView;
    public List<int> characterOwnedList;
    public int healthUpgrade;
    public int attackUpgrade;
    public int speedUpgrade;
    public int coinsUpgrade;


    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerDescription;
    [SerializeField] private TextMeshProUGUI playerStats; 

    protected override void Awake()
    {
        base.Awake();

        LoadPlayer();
    }

    private void OnEnable()
    {
        MapManager.Instance.ItemBuyed();
    }

    public void CharacterSelectedChange(int selectedPlayerIndexView)
    {
        this.selectedPlayerIndexView = selectedPlayerIndexView;
        PlayerDetailsSO currentPlayerDetails = GameResources.Instance.playerDetailsList[selectedPlayerIndexView];
        playerName.text = currentPlayerDetails.playerCharacterName;
        playerDescription.text = currentPlayerDetails.playerDescription;
        //playerStats.text = "Health: " + currentPlayerDetails.playerHealthAmmount.ToString() + " + " + healthUpgrade + "x10\n" +
        //    "Bonus Attack: " + currentPlayerDetails.playerBonusAttack.ToString() + " + " + attackUpgrade + "x2\n" +
        //    "Speed: " + currentPlayerDetails.playerMovementDetails.maxMoveSpeed.ToString() + " + " + speedUpgrade + "x1\n" +
        //    "Bonus Coins: " + currentPlayerDetails.playerBonusCoins.ToString() + " + " + coinsUpgrade + "x3";
        playerStats.text = "Health: " + (currentPlayerDetails.playerHealthAmmount + healthUpgrade * Settings.healthUpgradeMultiplier).ToString() + "\n" +
            "Bonus Attack: " + (currentPlayerDetails.playerBonusAttack + attackUpgrade * Settings.attackUpgradeMultiplier).ToString() + "\n" +
            "Speed: " + (currentPlayerDetails.playerMovementDetails.maxMoveSpeed + speedUpgrade * Settings.speedUpgradeMultiplier).ToString();
    }


    public void ExitCharacterMenuUI()
    {
        SavePlayer();
        PassingUpgradeStats();
        this.gameObject.SetActive(false);
    }

    public void SavePlayer()
    {
        SaveObjectPlayer saveObjectPlayer = new SaveObjectPlayer() { playerSelectIndex = selectedPlayerIndex, characterOwnedList = characterOwnedList, healthUpgrade = healthUpgrade, attackUpgrade = attackUpgrade, speedUpgrade = speedUpgrade, coinsUpgrade = coinsUpgrade };
        string json = JsonUtility.ToJson(saveObjectPlayer);
        SaveSystem.SavePlayer(json);
    }

    public void LoadPlayer()
    {
        string saveString = SaveSystem.LoadPlayer();
        if (saveString != null)
        {
            SaveObjectPlayer saveObjectPlayer = JsonUtility.FromJson<SaveObjectPlayer>(saveString);
            selectedPlayerIndex = saveObjectPlayer.playerSelectIndex;
            characterOwnedList = saveObjectPlayer.characterOwnedList;
            healthUpgrade = saveObjectPlayer.healthUpgrade;
            attackUpgrade = saveObjectPlayer.attackUpgrade;
            speedUpgrade = saveObjectPlayer.speedUpgrade;
            coinsUpgrade = saveObjectPlayer.coinsUpgrade;
        }
        else
        {
            Debug.Log("No Save");
        }
    }

    private void PassingUpgradeStats()
    {
        GameResources.Instance.healthBonus = healthUpgrade;
        GameResources.Instance.attackBonus = attackUpgrade;
        GameResources.Instance.speedBonus = speedUpgrade;
        GameResources.Instance.coinsBonus = coinsUpgrade;
    }
}
