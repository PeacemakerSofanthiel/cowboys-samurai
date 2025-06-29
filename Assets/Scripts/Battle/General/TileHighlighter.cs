using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TileHighlighter : MonoBehaviour
{
    public static TileHighlighter Instance;

    public Tilemap highlightMap;
    public TileBase highlightTilePlayer;
    public TileBase highlightTileEnemy;

    private List<Vector3Int> highlightedCells = new List<Vector3Int>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void HighlightTile(Vector3Int cell, TileBase tile)
    {
        highlightMap.SetTile(cell, tile);
        if (!highlightedCells.Contains(cell))
            highlightedCells.Add(cell);
    }

    public void ClearTile(Vector3Int cell)
    {
        if (highlightedCells.Contains(cell))
        {
            highlightMap.SetTile(cell, null);
            highlightedCells.Remove(cell);
        }
    }

    public void ClearHighlights()
    {
        foreach (var cell in highlightedCells)
        {
            highlightMap.SetTile(cell, null);
        }
        highlightedCells.Clear();
    }

    public void ClearHighlightsAfterDelay(float seconds)
    {
        CancelInvoke(nameof(ClearHighlights));
        Invoke(nameof(ClearHighlights), seconds);
    }
}
