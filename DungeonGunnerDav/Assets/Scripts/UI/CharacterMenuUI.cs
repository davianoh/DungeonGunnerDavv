using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMenuUI : SingletonMonobehaviour<CharacterMenuUI>
{
    #region Header SaveFiles References
    [Header("SAFE FILES REFERENCES")]
    [Space(10)]
    #endregion
    public int selectedPlayerIndex;
    public int healthUpgrade;
    public int attackUpgrade;
    public int speedUpgrade;
    public int coinsUpgrade;

    protected override void Awake()
    {
        base.Awake();

        LoadPlayer();
        MapManager.Instance.itemBuyed = true;
    }

    public void ExitCharacterMenuUI()
    {
        SavePlayer();
        this.gameObject.SetActive(false);
    }

    public void SavePlayer()
    {
        SaveObjectPlayer saveObjectPlayer = new SaveObjectPlayer() { playerSelectIndex = selectedPlayerIndex, healthUpgrade = healthUpgrade, attackUpgrade = attackUpgrade, speedUpgrade = speedUpgrade, coinsUpgrade = coinsUpgrade };
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
}
