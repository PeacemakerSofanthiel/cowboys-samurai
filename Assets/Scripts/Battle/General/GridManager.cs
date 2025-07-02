using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileOwner
{
    Neutral,
    Player,
    Enemy
}

public enum TileType
{
    Normal,
    Obstacle,
    Hole
}


public class TileState
{
    public Vector3Int position;
    public TileBase baseTile;
    public TileOwner owner;
    public TileType type;

    public TileState(Vector3Int pos, TileBase tile, TileOwner tileOwner, TileType tileType = TileType.Normal)
    {
        position = pos;
        baseTile = tile;
        owner = tileOwner;
        type = tileType;
    }
}


public class GridManager : MonoBehaviour
{
    public Tilemap tilemap;

    public TileBase playerTileVisual;
    public TileBase enemyTileVisual;
    public TileBase neutralTileVisual;

    public Dictionary<Vector3Int, TileState> tileStates = new();

    void Start()
    {
        InitializeTileStates();
    }




    void InitializeTileStates()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);

            if (tile is TaggedTile tagged)
            {
                tileStates[pos] = new TileState(pos, tile, tagged.owner, tagged.type);
            }
        }
    }

    public bool IsWalkable(Vector3Int cell, TileOwner requester)
    {
        if (!tileStates.TryGetValue(cell, out TileState state)) return false;

        if (state.type == TileType.Obstacle || state.type == TileType.Hole)
            return false;

        return state.owner == TileOwner.Neutral || state.owner == requester;
    }

    public void SetTileType(Vector3Int cell, TileType newType)
    {
        if (!tileStates.ContainsKey(cell)) return;
        tileStates[cell].type = newType;
    }


    public void StealTile(Vector3Int cell, TileOwner newOwner)
    {
        if (!tileStates.ContainsKey(cell)) return;

        tileStates[cell].owner = newOwner;

        switch (newOwner)
        {
            case TileOwner.Player:
                tilemap.SetTile(cell, playerTileVisual);
                break;
            case TileOwner.Enemy:
                tilemap.SetTile(cell, enemyTileVisual);
                break;
            case TileOwner.Neutral:
                tilemap.SetTile(cell, neutralTileVisual);
                break;
        }
    }
}
