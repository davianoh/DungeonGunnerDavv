using System.Collections;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class DestroyedEvent : MonoBehaviour
{
    public event Action<DestroyedEvent, DestroyedEventArgs> OnDestroyed;

    public void CallDestroyedEvent(bool playerDied, int points, int coinsAmount)
    {
        OnDestroyed?.Invoke(this, new DestroyedEventArgs() { playerDied = playerDied, points = points, coinsAmount = coinsAmount });
    }
}

public class DestroyedEventArgs : EventArgs
{
    public bool playerDied;
    public int points;
    public int coinsAmount;
}
