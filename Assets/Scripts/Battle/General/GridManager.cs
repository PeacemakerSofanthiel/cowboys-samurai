using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileOwner
{
    Neutral,
    Player,
    Enemy
}

public class TileState
{
    public Vector3Int position;
    public TileBase baseTile;
    public TileOwner owner;

    public TileState(Vector3Int pos, TileBase tile, TileOwner tileOwner)
    {
        position = pos;
        baseTile = tile;
        owner = tileOwner;
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
                tileStates[pos] = new TileState(pos, tile, tagged.owner);
            }
        }
    }

    public bool IsWalkable(Vector3Int cell, TileOwner requester)
    {
        if (!tileStates.ContainsKey(cell)) return false;

        TileState state = tileStates[cell];

        return state.owner == TileOwner.Neutral || state.owner == requester;
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
