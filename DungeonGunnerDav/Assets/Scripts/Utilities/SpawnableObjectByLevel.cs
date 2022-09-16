using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableObjectByLevel<T>
{
    public Room dungeonLevel;
    public List<SpawnableObjectRatio<T>> spawnableObjectRatioList;
}
