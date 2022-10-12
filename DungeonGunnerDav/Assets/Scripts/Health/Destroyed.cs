using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DestroyedEvent))]
[DisallowMultipleComponent]
public class Destroyed : MonoBehaviour
{
    private DestroyedEvent destroyedEvent;

    private void Awake()
    {
        destroyedEvent = GetComponent<DestroyedEvent>();
    }

    private void OnEnable()
    {
        destroyedEvent.OnDestroyed += DestroyedEvent_OnDestroyed;
    }

    private void OnDisable()
    {
        destroyedEvent.OnDestroyed -= DestroyedEvent_OnDestroyed;
    }

    private void DestroyedEvent_OnDestroyed(DestroyedEvent destroyedEvent, DestroyedEventArgs destroyEventArgs)
    {
        if (destroyEventArgs.playerDied)
        {
            gameObject.SetActive(false);
        }
        else
        {
            GameObject coinsPrefab = GameResources.Instance.coins;
            ICollectible coins = (ICollectible)PoolManager.Instance.ReuseComponent(coinsPrefab, transform.position, Quaternion.identity);
            coins.Initialise(destroyEventArgs.coinsAmount);

            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
