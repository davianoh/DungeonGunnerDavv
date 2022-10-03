using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeStats : MonoBehaviour
{
    [SerializeField] private UpgradeStatPlayerSO upgradeStatsSO;
    [SerializeField] private GameObject upgradeBarParent;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI upgradeText;
    [SerializeField] private int upgradeID;
    private int upgradedValue;

    private void Start()
    {
        icon.sprite = upgradeStatsSO.icon;
        upgradeText.text = upgradeStatsSO.upgradeName;
        UpgradeBarUpdate();
    }

    public void Upgradee()
    {
        if(MapManager.Instance.totalCoinsInGame >= upgradeStatsSO.upgradeCost && upgradedValue < 6)
        {
            MapManager.Instance.totalCoinsInGame -= upgradeStatsSO.upgradeCost;
            switch (upgradeID)
            {
                case 0:
                    CharacterMenuUI.Instance.healthUpgrade++;
                    break;

                case 1:
                    CharacterMenuUI.Instance.attackUpgrade++;
                    break;
                case 2:
                    CharacterMenuUI.Instance.speedUpgrade++;
                    break;

                case 3:
                    CharacterMenuUI.Instance.coinsUpgrade++;
                    break;
            }

            upgradedValue++;
            MapManager.Instance.ItemBuyed();
            UpgradeBarUpdate();
            CharacterMenuUI.Instance.CharacterSelectedChange(CharacterMenuUI.Instance.selectedPlayerIndexView);
        }
    }

    private void UpgradeBarUpdate()
    {
        switch (upgradeID)
        {
            case 0:
                upgradedValue = CharacterMenuUI.Instance.healthUpgrade;
                break;

            case 1:
                upgradedValue = CharacterMenuUI.Instance.attackUpgrade;
                break;
            case 2:
                upgradedValue = CharacterMenuUI.Instance.speedUpgrade;
                break;

            case 3:
                upgradedValue = CharacterMenuUI.Instance.coinsUpgrade;
                break;
        }

        for (int i = 0; i < upgradedValue; i++)
        {
            upgradeBarParent.transform.GetChild(i).GetComponent<Image>().color = new Color(0.4874632f, 1f, 0.2113207f, 1f);
        }
    }
}
