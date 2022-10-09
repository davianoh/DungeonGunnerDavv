using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject prefabs;
    public RoomNodeTypeSO roomNodeType;
    public Vector2Int lowerBounds;
    public Vector2Int upperBounds;
    public Vector2Int templateLowerBounds;
    public Vector2Int templateUpperBounds;
    public Vector2Int[] spawnPositionArray;
    public Vector2Int spawnPositionPlayer;
    public InstantiatedRoom instantiatedRoom;
    public bool isLit = false;
    public bool isPreviouslyVisited = false;
    public LevelsSO level;

    private void Awake()
    {
        instantiatedRoom = GetComponent<InstantiatedRoom>();
    }
}
