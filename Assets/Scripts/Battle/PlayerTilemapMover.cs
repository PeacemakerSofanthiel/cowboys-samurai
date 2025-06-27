using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTilemapMover : MonoBehaviour
{
    public Tilemap tilemap;
    public GridManager gridManager; 
    public TileOwner playerOwner = TileOwner.Player;

    private Vector3Int currentCell;

    void Start()
    {
        currentCell = tilemap.WorldToCell(transform.position);
        MoveToCell(currentCell);
    }

    void Awake()
    {
        if (gridManager == null)
            gridManager = FindObjectOfType<GridManager>();
    }

    void Update()
    {
        Vector3Int nextCell = currentCell;

        if (Input.GetKeyDown(KeyCode.UpArrow)) nextCell += new Vector3Int(0, 1, 0);
        if (Input.GetKeyDown(KeyCode.DownArrow)) nextCell += new Vector3Int(0, -1, 0);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) nextCell += new Vector3Int(-1, 0, 0);
        if (Input.GetKeyDown(KeyCode.RightArrow)) nextCell += new Vector3Int(1, 0, 0);

        if (nextCell != currentCell && IsValidMove(nextCell))
        {
            currentCell = nextCell;
            MoveToCell(currentCell);
        }
    }

    void MoveToCell(Vector3Int cellPos)
    {
        Vector3 worldPos = tilemap.GetCellCenterWorld(cellPos);
        transform.position = worldPos;
    }

    bool IsValidMove(Vector3Int cell)
    {
        return gridManager.IsWalkable(cell, playerOwner);
    }
}
