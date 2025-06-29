using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTilemapMover : BattleEntity, IBattleTickable
{
    public GridManager gridManager;
    public TileOwner playerOwner = TileOwner.Player;
    [HideInInspector]
    public Vector3 lastMoveDirection = Vector3.right;

    void Awake()
    {
        if (gridManager == null)
            gridManager = FindObjectOfType<GridManager>();
    }

    protected override void Start()
    {
        base.Start();
        currentCell = tilemap.WorldToCell(transform.position);
        SnapToGrid(currentCell);
        BattleManager.Instance.Register(this);
    }

    void Update()
    {
        Vector3Int nextCell = currentCell;

        if (Input.GetKeyDown(KeyCode.UpArrow))  { nextCell += Vector3Int.up;    lastMoveDirection = Vector3.up; }
        if (Input.GetKeyDown(KeyCode.DownArrow)){ nextCell += Vector3Int.down;  lastMoveDirection = Vector3.down; }
        if (Input.GetKeyDown(KeyCode.LeftArrow)){ nextCell += Vector3Int.left;  lastMoveDirection = Vector3.left; }
        if (Input.GetKeyDown(KeyCode.RightArrow)){ nextCell += Vector3Int.right; lastMoveDirection = Vector3.right; }

        if (nextCell != currentCell && IsValidMove(nextCell))
        {
            MoveTo(nextCell);
        }
    }

    bool IsValidMove(Vector3Int cell)
    {
        return gridManager.IsWalkable(cell, playerOwner);
    }

    public void OnBattleTick()
    {
        Debug.Log("Player ticked.");
    }
}

