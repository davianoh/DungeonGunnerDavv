using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour, ICollectible
{
    private int coinsAmount;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void Initialise(int coinsAmount)
    {
        this.coinsAmount = coinsAmount;
        this.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CoinsManager.Instance.AddCoins(coinsAmount);
            gameObject.SetActive(false);
            Debug.Log(CoinsManager.Instance.coinsInLevel);
        }
    }
}
