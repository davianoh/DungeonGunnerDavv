using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class StaticEventHandler
{
    // Room changed event

    public static event Action<PointsScoredArgs> OnPointsScored;
    public static void CallPointsScoredEvent(int points)
    {
        OnPointsScored?.Invoke(new PointsScoredArgs() { points = points });
    }

    public static event Action<ScoreChangedArgs> OnScoreChanged;
    public static void CallScoreChangedEvent(long score, int multiplier)
    {
        OnScoreChanged?.Invoke(new ScoreChangedArgs() { score = score, multiplier = multiplier });
    }

    public static event Action<MultiplierArgs> OnMultiplier;
    public static void CallMultiplierEvent(bool multiplier)
    {
        OnMultiplier?.Invoke(new MultiplierArgs() { multiplier = multiplier });
    }

    public static event Action<LevelWonArgs> OnLevelWon;
    public static void CallLevelWon(int coins)
    {
        OnLevelWon?.Invoke(new LevelWonArgs() { coins = coins });
    }
}



public class PointsScoredArgs : EventArgs
{
    public int points;
}

public class ScoreChangedArgs : EventArgs
{
    public long score;
    public int multiplier;
}

public class MultiplierArgs : EventArgs
{
    public bool multiplier;
}

public class LevelWonArgs : EventArgs
{
    public int coins;
}
