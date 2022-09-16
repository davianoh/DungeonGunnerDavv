using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D))]
public class InstantiatedRoom : MonoBehaviour
{
    public Room room;
    public Grid grid;
    public Tilemap groundTilemap;
    public Tilemap decoration1Tilemap;
    public Tilemap decoration2Tilemap;
    public Tilemap frontTilemap;
    public Tilemap collisionTilemap;
    public Tilemap minimapTilemap;
    public Bounds roomColliderBounds;
    public int[,] aStarMovementPenalty;

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        roomColliderBounds = boxCollider2D.bounds;
        room = GetComponent<Room>();
        DisableCollisionTilemapRenderer();
        AddObstaclesAndPreferredPath();
    }

    private void Start()
    {
        
    }


    //private void BlockDoorwayHorizontally(Tilemap tilemap, Doorway doorway)
    //{
    //    Vector2Int startPosition = doorway.doorwayStartCopyPosition;

    //    for(int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
    //    {
    //        for(int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
    //        {
    //            // Get rotation of tile being copied
    //            Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));
    //            // Copied n pasted tile
    //            tilemap.SetTile(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));
    //            // Set the rotation of pasted tile
    //            tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), transformMatrix);
    //        }
    //    }
    //}

    private void DisableCollisionTilemapRenderer()
    {
        collisionTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
    }

    private void AddObstaclesAndPreferredPath()
    {
        aStarMovementPenalty = new int[room.templateUpperBounds.x - room.templateLowerBounds.x + 1, room.templateUpperBounds.y - room.templateLowerBounds.y + 1];

        for(int x = 0; x < (room.templateUpperBounds.x - room.templateLowerBounds.x + 1); x++)
        {
            for(int y = 0; y < (room.templateUpperBounds.y - room.templateLowerBounds.y + 1); y++)
            {
                aStarMovementPenalty[x, y] = Settings.defaultAStarMovementPenalty;
                TileBase tile = collisionTilemap.GetTile(new Vector3Int(x + room.templateLowerBounds.x, y + room.templateLowerBounds.y, 0));

                foreach(TileBase collisionTile in GameResources.Instance.enemyUnwalkableCollisionTilesArray)
                {
                    if(tile == collisionTile)
                    {
                        aStarMovementPenalty[x, y] = 0;
                        break;
                    }
                }

                if(tile == GameResources.Instance.preferredEnemyPathTile)
                {
                    aStarMovementPenalty[x, y] = Settings.preferredPathAStarMovementPenalty;
                }
                //Debug.Log(aStarMovementPenalty[x, y] + " and " + x + ":" + y);
            }
        }
    }
}
