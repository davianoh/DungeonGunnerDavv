using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CoinsManager : SingletonMonobehaviour<CoinsManager>
{
    public int coinsInLevel = 0;
    public int coinsInGame = 0;

    public void AddCoins(int coinsAmount)
    {
        coinsInLevel += coinsAmount;
    }
}
