using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UpgradeStats_", menuName = "Scriptable Objects/Player/Player Upgrade")]
public class UpgradeStatPlayerSO : ScriptableObject
{
    public Sprite icon;
    public string upgradeName;
    public int upgradeValue;
    public int upgradeCost;
}
