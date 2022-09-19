using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CoinsManager : SingletonMonobehaviour<CoinsManager>
{
    public int coinsInLevel = 0;
    public TextMeshProUGUI coinsText;

    private void Start()
    {
        coinsText.text = "0";
    }

    public void AddCoins(int coinsAmount)
    {
        coinsInLevel += coinsAmount;
        coinsText.text = coinsInLevel.ToString();
    }
}
